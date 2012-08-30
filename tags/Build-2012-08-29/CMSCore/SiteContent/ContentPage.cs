using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carrotware.CMS.Data;
using System.Text.RegularExpressions;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Core {

	public class ContentPage : IDisposable, ISiteContent {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public ContentPage() {

		}

		public ContentPage(carrot_RootContent rc, carrot_Content c) {

			if (rc == null) {
				rc = new carrot_RootContent();
				rc.Root_ContentID = Guid.NewGuid();
				rc.PageActive = true;
			}
			if (c == null) {
				c = new carrot_Content();
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

		public SiteNav GetSiteNav() {
			SiteNav sd = null;
			using (SiteNavHelper sdh = new SiteNavHelper()) {
				sd = sdh.GetLatestVersion(this.SiteID, this.Root_ContentID);
			}
			return sd;
		}

		public List<Widget> GetAllWidgetsAndUnsaved() {
			CMSConfigHelper ch = new CMSConfigHelper();
			List<Widget> widgets = new List<Widget>();
			if (ch.cmsAdminContent == null) {
				using (WidgetHelper h = new WidgetHelper()) {
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

			carrot_RootContent rc = (from r in db.carrot_RootContents
								 where r.Root_ContentID == this.Root_ContentID
								   && r.SiteID == this.SiteID
								 select r).FirstOrDefault();

			rc.EditHeartbeat = DateTime.Now.AddHours(-2);
			rc.Heartbeat_UserId = null;
			db.SubmitChanges();
		}

		public void RecordHeartbeatLock(Guid currentUserID) {

			carrot_RootContent rc = (from r in db.carrot_RootContents
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
				if (this.Heartbeat_UserId != SecurityData.CurrentUserGuid
						&& this.EditHeartbeat.Value > DateTime.Now.AddMinutes(-2)) {
					bLock = true;
				}
				if (this.Heartbeat_UserId == SecurityData.CurrentUserGuid
					|| this.Heartbeat_UserId == null) {
					bLock = false;
				}
			}
			return bLock;
		}

		private void FixMeta() {
			this.MetaKeyword = string.IsNullOrEmpty(this.MetaKeyword) ? String.Empty : this.MetaKeyword;
			this.MetaDescription = string.IsNullOrEmpty(this.MetaDescription) ? String.Empty : this.MetaDescription;

		}

		public void SavePageEdit() {

			carrot_RootContent rc = (from r in db.carrot_RootContents
								 where r.Root_ContentID == this.Root_ContentID
								   && r.SiteID == this.SiteID
								 select r).FirstOrDefault();

			carrot_Content oldC = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   where ct.Root_ContentID == this.Root_ContentID
								   && r.SiteID == this.SiteID
								   && ct.IsLatestVersion == true
							   select ct).FirstOrDefault();

			bool bNew = false;

			if (rc == null) {
				rc = new carrot_RootContent();
				rc.Root_ContentID = this.Root_ContentID;
				rc.PageActive = true;
				rc.SiteID = this.SiteID;
				rc.CreateDate = DateTime.Now;
				db.carrot_RootContents.InsertOnSubmit(rc);
				bNew = true;
			}

			carrot_Content c = new carrot_Content();
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

			FixMeta();
			c.MetaKeyword = this.MetaKeyword.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");
			c.MetaDescription = this.MetaDescription.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");

			this.Root_ContentID = rc.Root_ContentID;
			this.ContentID = c.ContentID;
			this.FileName = rc.FileName;
			this.EditDate = c.EditDate;
			this.CreateDate = rc.CreateDate;

			db.carrot_Contents.InsertOnSubmit(c);
			db.SubmitChanges();

		}


		public void SavePageAsDraft() {

			carrot_RootContent rc = (from r in db.carrot_RootContents
								 where r.Root_ContentID == this.Root_ContentID
								   && r.SiteID == this.SiteID
								 select r).FirstOrDefault();

			bool bNew = false;

			if (rc == null) {
				rc = new carrot_RootContent();
				rc.Root_ContentID = this.Root_ContentID;
				rc.PageActive = true;
				rc.SiteID = this.SiteID;
				rc.CreateDate = DateTime.Now;
				db.carrot_RootContents.InsertOnSubmit(rc);
				bNew = true;
			}

			carrot_Content c = new carrot_Content();
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

			FixMeta();
			c.MetaKeyword = this.MetaKeyword.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");
			c.MetaDescription = this.MetaDescription.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");

			this.Root_ContentID = rc.Root_ContentID;
			this.ContentID = c.ContentID;
			this.FileName = rc.FileName;
			this.EditDate = c.EditDate;
			this.CreateDate = rc.CreateDate;

			db.carrot_Contents.InsertOnSubmit(c);
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


		private carrot_RootContent RootContent { get; set; }
		private carrot_Content Content { get; set; }

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion
	}


}
