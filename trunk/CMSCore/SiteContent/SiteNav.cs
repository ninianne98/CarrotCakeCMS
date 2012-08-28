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

	public class SiteNav : IDisposable, ISiteContent {
		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteNav() { }

		public SiteNav(carrot_RootContent rc, carrot_Content c) {

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
			this.TemplateFile = c.TemplateFile;
			this.NavFileName = rc.FileName;

		}

		public ContentPage GetContentPage() {
			ContentPage cp = null;
			using (ContentPageHelper cph = new ContentPageHelper()) {
				cp = cph.GetLatestContent(this.SiteID, this.Root_ContentID);
			}
			return cp;
		}


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
		public string TemplateFile { get; set; }


		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion

	}

}
