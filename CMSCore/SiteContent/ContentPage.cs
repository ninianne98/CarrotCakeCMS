using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Carrotware.CMS.Data;
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
#if DEBUG
			db.Log = new DebugTextWriter();
#endif
		}


		public SiteNav GetSiteNav() {
			SiteNav sd = null;
			if (SiteData.IsPageSampler) {
				sd = SiteNavHelper.GetSamplerView();
			} else {
				using (SiteNavHelper sdh = new SiteNavHelper()) {
					sd = sdh.GetLatestVersion(this.SiteID, this.Root_ContentID);
				}
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

			carrot_RootContent rc = CompiledQueries.cqGetRootContent(db, this.SiteID, this.Root_ContentID);

			rc.EditHeartbeat = DateTime.Now.AddHours(-2);
			rc.Heartbeat_UserId = null;
			db.SubmitChanges();
		}

		public void RecordHeartbeatLock(Guid currentUserID) {

			carrot_RootContent rc = CompiledQueries.cqGetRootContent(db, this.SiteID, this.Root_ContentID);

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

			carrot_RootContent rc = CompiledQueries.cqGetRootContent(db, this.SiteID, this.Root_ContentID);

			carrot_Content oldC = CompiledQueries.cqGetLatestContent(db, this.SiteID, this.Root_ContentID);

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

			carrot_RootContent rc = CompiledQueries.cqGetRootContent(db, this.SiteID, this.Root_ContentID);

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

		public string MetaDescription { get; set; }
		public string MetaKeyword { get; set; }

		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is ContentPage) {
				ContentPage p = (ContentPage)obj;
				return (this.ContentID == p.ContentID)
						&& (this.SiteID == p.SiteID)
						&& (this.FileName.ToLower() == p.FileName.ToLower());
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return ContentID.GetHashCode() ^ SiteID.GetHashCode() ^ Root_ContentID.GetHashCode() ^ FileName.GetHashCode();
		}

		public string PageTextSummary {
			get {
				string txt = !string.IsNullOrEmpty(PageText) ? PageText : "";
				txt = txt.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
				txt = Regex.Replace(txt, @"<!--(\n|.)*-->", " ");
				if (txt.Length > 4096) {
					txt = txt.Substring(0, 4096);
				}

				txt = txt.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");

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
				txt = txt.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
				txt = Regex.Replace(txt, @"<!--(\n|.)*-->", " ");
				if (txt.Length > 4096) {
					txt = txt.Substring(0, 4096);
				}

				txt = Regex.Replace(txt, @"<(.|\n)*?>", " ");

				txt = txt.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");

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


		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion
	}


}
