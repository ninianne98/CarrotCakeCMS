using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Carrotware.CMS.Data;
using System.Text.RegularExpressions;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Core {

	public class ContentPage : IDisposable {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public ContentPage() {

		}

		public ContentPage(tblRootContent rc, tblContent c) {

			if (rc == null) {
				rc = new tblRootContent();
				rc.Root_ContentID = Guid.NewGuid();
				rc.PageActive = true;
			}
			if (c == null) {
				c = new tblContent();
				c.ContentID = rc.Root_ContentID;
				c.Root_ContentID = rc.Root_ContentID;
			}

			this.RootContent = rc;
			this.Content = c;

			this.Root_ContentID = rc.Root_ContentID;
			this.SiteID = rc.SiteID;
			this.Heartbeat_UserId = rc.Heartbeat_UserId;
			this.EditHeartbeat = rc.EditHeartbeat;
			this.FileName = rc.FileName;
			this.CreateDate = rc.CreateDate;

			this.PageActive = Convert.ToBoolean(rc.PageActive);

			this.ContentID = c.ContentID;
			this.Parent_ContentID = c.Parent_ContentID;
			this.IsLatestVersion = Convert.ToBoolean(c.IsLatestVersion);
			this.TitleBar = c.TitleBar;
			this.NavMenuText = c.NavMenuText;
			this.PageHead = c.PageHead;
			this.PageText = c.PageText;
			this.LeftPageText = c.LeftPageText;
			this.RightPageText = c.RightPageText;
			this.NavOrder = c.NavOrder;
			this.EditUserId = c.EditUserId;
			this.EditDate = c.EditDate;
			this.TemplateFile = c.TemplateFile;
			this.NavFileName = rc.FileName;

			this.MetaDescription = c.MetaDescription;
			this.MetaKeyword = c.MetaKeyword;

		}


		public List<PageWidget> GetAllWidgetsAndUnsaved() {
			CMSConfigHelper ch = new CMSConfigHelper();
			List<PageWidget> widgets = new List<PageWidget>();
			if (ch.cmsAdminContent == null) {
				using (var h = new PageWidgetHelper()) {
					widgets = h.GetWidgets(this.Root_ContentID, true);
				}
			} else {
				widgets = (from w in ch.cmsAdminWidget
						   orderby w.WidgetOrder
						   select w).ToList();
			}

			return widgets;
		}

		public void ResetHeartbeatLock() {

			var rc = (from r in db.tblRootContents
					  where r.Root_ContentID == this.Root_ContentID
						&& r.SiteID == this.SiteID
					  select r).FirstOrDefault();

			rc.EditHeartbeat = DateTime.Now.AddHours(-2);
			rc.Heartbeat_UserId = null;
			db.SubmitChanges();
		}

		public void RecordHeartbeatLock(Guid currentUserID) {

			var rc = (from r in db.tblRootContents
					  where r.Root_ContentID == this.Root_ContentID
					  && r.SiteID == this.SiteID
					  select r).FirstOrDefault();

			rc.Heartbeat_UserId = currentUserID;
			rc.EditHeartbeat = DateTime.Now;

			db.SubmitChanges();
		}

		public bool IsPageLocked() {

			bool bLock = false;
			if (this.Heartbeat_UserId != null) {
				if (this.Heartbeat_UserId != SiteData.CurrentUserGuid
						&& this.EditHeartbeat.Value > DateTime.Now.AddMinutes(-2)) {
					bLock = true;
				}
				if (this.Heartbeat_UserId == SiteData.CurrentUserGuid
					|| this.Heartbeat_UserId == null) {
					bLock = false;
				}
			}
			return bLock;
		}

		public void SavePageEdit() {

			var rc = (from r in db.tblRootContents
					  where r.Root_ContentID == this.Root_ContentID
						&& r.SiteID == this.SiteID
					  select r).FirstOrDefault();

			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						where ct.Root_ContentID == this.Root_ContentID
							&& r.SiteID == this.SiteID
							&& ct.IsLatestVersion == true
						select ct).FirstOrDefault();

			bool bNew = false;

			if (rc == null) {
				rc = new tblRootContent();
				rc.Root_ContentID = this.Root_ContentID;
				rc.PageActive = true;
				rc.SiteID = this.SiteID;
				rc.CreateDate = DateTime.Now;
				db.tblRootContents.InsertOnSubmit(rc);
				bNew = true;
			}

			var c = new tblContent();
			if (bNew) {
				c.ContentID = this.Root_ContentID;
			} else {
				c.ContentID = Guid.NewGuid();
				oldC.IsLatestVersion = false;
			}
			c.Root_ContentID = this.Root_ContentID;

			rc.Heartbeat_UserId = this.Heartbeat_UserId;
			rc.EditHeartbeat = this.EditHeartbeat;
			rc.FileName = this.FileName;
			rc.PageActive = this.PageActive;

			rc.FileName = ContentPageHelper.ScrubFilename(this.Root_ContentID, rc.FileName);

			c.Parent_ContentID = this.Parent_ContentID;
			c.IsLatestVersion = true;
			c.TitleBar = this.TitleBar;
			c.NavMenuText = this.NavMenuText;
			c.PageHead = this.PageHead;
			c.PageText = this.PageText;
			c.LeftPageText = this.LeftPageText;
			c.RightPageText = this.RightPageText;
			c.NavOrder = this.NavOrder;
			c.EditUserId = this.EditUserId;
			c.EditDate = DateTime.Now;
			c.TemplateFile = this.TemplateFile;

			c.MetaKeyword = this.MetaKeyword;
			c.MetaDescription = this.MetaDescription;

			db.tblContents.InsertOnSubmit(c);
			db.SubmitChanges();

		}


		public void SavePageAsDraft() {

			var rc = (from r in db.tblRootContents
					  where r.Root_ContentID == this.Root_ContentID
						&& r.SiteID == this.SiteID
					  select r).FirstOrDefault();

			bool bNew = false;

			if (rc == null) {
				rc = new tblRootContent();
				rc.Root_ContentID = this.Root_ContentID;
				rc.PageActive = true;
				rc.SiteID = this.SiteID;
				rc.CreateDate = DateTime.Now;
				db.tblRootContents.InsertOnSubmit(rc);
				bNew = true;
			}

			var c = new tblContent();
			if (bNew) {
				c.ContentID = this.Root_ContentID;
			} else {
				c.ContentID = Guid.NewGuid();
			}
			c.Root_ContentID = this.Root_ContentID;

			rc.Heartbeat_UserId = this.Heartbeat_UserId;
			rc.EditHeartbeat = this.EditHeartbeat;
			rc.FileName = this.FileName;
			rc.PageActive = this.PageActive;

			rc.FileName = ContentPageHelper.ScrubFilename(this.Root_ContentID, rc.FileName);

			c.Parent_ContentID = this.Parent_ContentID;
			c.IsLatestVersion = false; // draft, leave existing version latest

			c.TitleBar = this.TitleBar;
			c.NavMenuText = this.NavMenuText;
			c.PageHead = this.PageHead;
			c.PageText = this.PageText;
			c.LeftPageText = this.LeftPageText;
			c.RightPageText = this.RightPageText;
			c.NavOrder = this.NavOrder;
			c.EditUserId = this.EditUserId;
			c.EditDate = DateTime.Now;
			c.TemplateFile = this.TemplateFile;

			c.MetaKeyword = this.MetaKeyword;
			c.MetaDescription = this.MetaDescription;

			db.tblContents.InsertOnSubmit(c);
			db.SubmitChanges();
		}




		public Guid ContentID { get; set; }
		public DateTime EditDate { get; set; }
		public DateTime CreateDate { get; set; }
		public Guid? EditUserId { get; set; }
		public bool IsLatestVersion { get; set; }
		public string LeftPageText { get; set; }
		public string NavMenuText { get; set; }
		public int? NavOrder { get; set; }
		public string PageHead { get; set; }
		public string PageText { get; set; }
		public Guid? Parent_ContentID { get; set; }
		public string RightPageText { get; set; }
		public Guid Root_ContentID { get; set; }
		public string TemplateFile { get; set; }
		public string TitleBar { get; set; }

		public DateTime? EditHeartbeat { get; set; }
		public string FileName { get; set; }
		public Guid? Heartbeat_UserId { get; set; }
		public bool PageActive { get; set; }
		public Guid SiteID { get; set; }

		public string NavFileName { get; set; }

		public string MetaDescription { get; set; }
		public string MetaKeyword { get; set; }


		public string PageTextSummary {
			get {
				string txt = !string.IsNullOrEmpty(PageText) ? PageText : "";

				if (txt.Length > 512) {
					return txt.Substring(0, 500) + "........";
				} else {
					return txt;
				}
			}
		}

		public string PageTextPlainSummary {
			get {
				string txt = !string.IsNullOrEmpty(PageText) ? PageText : "";
				txt = Regex.Replace(txt, @"<(.|\n)*?>", " ");

				if (txt.Length > 512) {
					return txt.Substring(0, 500) + "........";
				} else {
					return txt;
				}
			}
		}


		public string TemplateFolderPath {
			get {
				if (!string.IsNullOrEmpty(TemplateFile)) {
					if (TemplateFile.LastIndexOf("/") >= 2) {
						return TemplateFile.Substring(0, TemplateFile.LastIndexOf("/") + 1);
					} else {
						return "/";
					}
				} else {
					return "/";
				}
			}
		}


		private tblRootContent RootContent { get; set; }
		private tblContent Content { get; set; }

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion
	}

	public class SiteNav : IDisposable {
		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteNav() { }

		public SiteNav(tblRootContent rc, tblContent c) {

			if (rc == null) {
				rc = new tblRootContent();
				rc.Root_ContentID = Guid.NewGuid();
				rc.PageActive = true;
			}
			if (c == null) {
				c = new tblContent();
				c.ContentID = rc.Root_ContentID;
				c.Root_ContentID = rc.Root_ContentID;
			}

			this.Root_ContentID = rc.Root_ContentID;
			this.SiteID = rc.SiteID;
			this.FileName = rc.FileName;
			this.PageActive = Convert.ToBoolean(rc.PageActive);
			this.CreateDate = rc.CreateDate;

			this.ContentID = c.ContentID;
			this.Parent_ContentID = c.Parent_ContentID;
			this.TitleBar = c.TitleBar;
			this.NavMenuText = c.NavMenuText;
			this.PageHead = c.PageHead;
			this.PageText = c.PageText;
			this.NavOrder = c.NavOrder;
			this.EditDate = c.EditDate;
			this.NavFileName = rc.FileName;
		}



		public Guid ContentID { get; set; }
		public DateTime EditDate { get; set; }
		public DateTime CreateDate { get; set; }
		public string NavMenuText { get; set; }
		public int? NavOrder { get; set; }
		public string PageHead { get; set; }
		public string PageText { get; set; }
		public Guid? Parent_ContentID { get; set; }
		public Guid Root_ContentID { get; set; }
		public string TitleBar { get; set; }

		public string FileName { get; set; }
		public bool PageActive { get; set; }
		public Guid SiteID { get; set; }

		public string NavFileName { get; set; }

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion

	}

	public class SiteMapOrder {

		public SiteMapOrder() { }


		public int? NavOrder { get; set; }
		public Guid? Parent_ContentID { get; set; }
		public Guid Root_ContentID { get; set; }
		public string NavMenuText { get; set; }
		public bool PageActive { get; set; }
		public int NavLevel { get; set; }

	}

}
