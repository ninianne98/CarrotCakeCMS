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
		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();
		//private CarrotCMSDataContext db = CompiledQueries.dbConn;


		public SiteNav() {
			//#if DEBUG
			//            db.Log = new DebugTextWriter();
			//#endif
		}


		public ContentPage GetContentPage() {
			ContentPage cp = null;
			if (SiteData.IsPageSampler) {
				cp = ContentPageHelper.GetSamplerView();
			} else {
				using (ContentPageHelper cph = new ContentPageHelper()) {
					cp = cph.FindContentByID(this.SiteID, this.Root_ContentID);
				}
			}
			return cp;
		}


		//public string PageTextSummary {
		//    get {
		//        string txt = !string.IsNullOrEmpty(PageText) ? PageText.Replace("   ", " ").Replace("  ", " ") : "";

		//        if (txt.Length > 300) {
		//            return txt.Substring(0, 256) + "[.....]";
		//        } else {
		//            return txt;
		//        }
		//    }
		//}

		public string PageTextPlainSummary {
			get {
				string txt = !string.IsNullOrEmpty(PageText) ? PageText : "";
				txt = txt.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("&nbsp;", " ").Replace('\u00A0', ' ');

				txt = Regex.Replace(txt, @"<!--(\n|.)*-->", " ");
				if (txt.Length > 4096) {
					txt = txt.Substring(0, 4096);
				}

				txt = Regex.Replace(txt, @"<(.|\n)*?>", " ");

				txt = txt.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ").Replace("  ", " ");

				if (txt.Length > 300) {
					return txt.Substring(0, 256) + "[.....]";
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
		public Guid? EditUserId { get; set; }
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

		public ContentPageType.PageType ContentType { get; set; }
		public string TemplateFile { get; set; }

		private List<ContentTag> _contentTags = null;
		public List<ContentTag> ContentTags {
			get {
				if (_contentTags == null) {
					_contentTags = ContentTag.BuildTagList(this.Root_ContentID);
				}
				return _contentTags;
			}
			set {
				_contentTags = value;
			}
		}

		private List<ContentCategory> _ContentCategories = null;
		public List<ContentCategory> ContentCategories {
			get {
				if (_ContentCategories == null) {
					_ContentCategories = ContentCategory.BuildCategoryList(this.Root_ContentID);
				}
				return _ContentCategories;
			}
			set {
				_ContentCategories = value;
			}
		}


		#region IDisposable Members



		ExtendedUserData _user = null;
		public ExtendedUserData GetUserInfo() {
			if (_user == null && this.EditUserId.HasValue) {
				_user = new ExtendedUserData(this.EditUserId.Value);
			}
			return _user;
		}


		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is SiteNav) {
				SiteNav p = (SiteNav)obj;
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


		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion

	}

}
