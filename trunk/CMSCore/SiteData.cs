using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using Carrotware.CMS.Data;
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
	public class SiteData {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteData() {
		}

		public void LoadSiteFromCache() {
			SiteData s = null;
			if (cmsSite == null) {
				s = (from r in db.tblSites
					 where r.SiteID == CurrentSiteID
					 select new SiteData(r)).FirstOrDefault();
				cmsSite = s;
			} else {
				s = cmsSite;
			}

			if (s != null) {
				this.SiteID = s.SiteID;
				this.MetaKeyword = s.MetaKeyword;
				this.MetaDescription = s.MetaDescription;
				this.SiteName = s.SiteName;
				this.SiteFolder = s.SiteFolder;
				this.MainURL = s.MainURL;
				this.BlockIndex = s.BlockIndex;
			}

		}


		public SiteData(tblSite s) {

			this.SiteID = s.SiteID;
			this.MetaKeyword = s.MetaKeyword;
			this.MetaDescription = s.MetaDescription;
			this.SiteName = s.SiteName;
			this.SiteFolder = s.SiteFolder;
			this.MainURL = s.MainURL;
			this.BlockIndex = s.BlockIndex;
		}



		private string ContentKey = "cms_SiteData_" + CurrentSiteID;
		private SiteData cmsSite {
			get {
				SiteData c = null;
				try { c = (SiteData)HttpContext.Current.Cache[ContentKey]; } catch { }
				return c;
			}
			set {
				if (value == null) {
					HttpContext.Current.Cache.Remove(ContentKey);
				} else {
					HttpContext.Current.Cache.Insert(ContentKey, value, null, DateTime.Now.AddMinutes(3), Cache.NoSlidingExpiration);
				}
			}
		}


		public SiteData Get(Guid siteID) {
			var s = (from r in db.tblSites
					 where r.SiteID == siteID
					 select r).FirstOrDefault();

			if (s != null) {
				return new SiteData(s);
			} else {
				return null;
			}
		}


		public void Save() {

			var s = (from r in db.tblSites
					 where r.SiteID == this.SiteID
					 select r).FirstOrDefault();

			bool bNew = false;
			if (s == null) {
				s = new tblSite();
				if (this.SiteID == Guid.Empty) {
					this.SiteID = Guid.NewGuid();
				}
				bNew = true;
			}

			s.SiteID = this.SiteID;
			s.MetaKeyword = this.MetaKeyword;
			s.MetaDescription = this.MetaDescription;
			s.SiteName = this.SiteName;
			s.SiteFolder = this.SiteFolder;
			s.MainURL = this.MainURL;
			s.BlockIndex = this.BlockIndex;

			if (bNew) {
				db.tblSites.InsertOnSubmit(s);
			}
			db.SubmitChanges();

			//System.Web.HttpRuntime.UnloadAppDomain();
		}


		public static Guid CurrentSiteID {
			get {
				Guid _site = Guid.Empty;

				if (System.Configuration.ConfigurationManager.AppSettings["CarrotSiteID"] != null) {
					//try {
					_site = new Guid(System.Configuration.ConfigurationManager.AppSettings["CarrotSiteID"].ToString());
					//} catch {
					//    _site = Guid.Empty;
					//}
				}

				if (_site == Guid.Empty) {
					CMSConfigHelper h = new CMSConfigHelper();
					var s = h.DynSite;
					if (s != null) {
						_site = s.SiteID;
					}
				}

				return _site;
			}
		}

		public bool BlockIndex { get; set; }
		public string MainURL { get; set; }
		public string MetaDescription { get; set; }
		public string MetaKeyword { get; set; }
		public Guid SiteID { get; set; }
		public string SiteName { get; set; }
		public string SiteFolder { get; set; }



		public static string CurrentScriptName {
			get { return HttpContext.Current.Request.ServerVariables["script_name"].ToString(); }
		}

		public static string ReferringPage {
			get {
				var r = SiteData.CurrentScriptName;
				try { r = HttpContext.Current.Request.ServerVariables["http_referer"].ToString(); } catch { }
				if (string.IsNullOrEmpty(r))
					r = "./default.aspx";
				return r;
			}
		}


		public static string CMSGroup_Admins {
			get {
				return "CarrotCMS Administrators";
			}
		}
		public static string CMSGroup_Editors {
			get {
				return "CarrotCMS Editors";
			}
		}
		public static string CMSGroup_Users {
			get {
				return "CarrotCMS Users";
			}
		}

		public static bool IsAdmin {
			get {
				try {
					return Roles.IsUserInRole(CMSGroup_Admins);
				} catch {
					return false;
				}
			}
		}
		public static bool IsEditor {
			get {
				try {
					return Roles.IsUserInRole(CMSGroup_Editors);
				} catch {
					return false;
				}
			}
		}
		public static bool IsUsers {
			get {
				try {
					return Roles.IsUserInRole(CMSGroup_Users);
				} catch {
					return false;
				}
			}
		}

		public static bool IsAuthEditor {
			get {
				return AdvancedEditMode || IsAdmin || IsEditor;
			}
		}

		public static Guid CurrentUserGuid {
			get {
				Guid _currentUserGuid = Guid.Empty;
				if (HttpContext.Current.User.Identity.IsAuthenticated) {
					if (!String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)) {
						_currentUserGuid = new Guid(CurrentUser.ProviderUserKey.ToString());
					}
				}
				return _currentUserGuid;
			}
		}

		public static MembershipUser CurrentUser {

			get {
				MembershipUser _currentUser = null;
				if (HttpContext.Current.User.Identity.IsAuthenticated) {
					if (!String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)) {
						_currentUser = Membership.GetUser(HttpContext.Current.User.Identity.Name);
					}
				}
				return _currentUser;
			}
		}

		public static bool AdvancedEditMode {
			get {
				bool _Advanced = false;
				if (HttpContext.Current.User.Identity.IsAuthenticated) {
					if (HttpContext.Current.Request.QueryString["carrotedit"] != null && (SiteData.IsAdmin || SiteData.IsEditor)) {
						_Advanced = true;
					} else {
						_Advanced = false;
					}
				}
				return _Advanced;
			}
		}

	}


}
