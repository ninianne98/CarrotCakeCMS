using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
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
	public class SiteData : IDisposable {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteData() { }

		public SiteData(tblSite s) {

			this.SiteID = s.SiteID;
			this.MetaKeyword = s.MetaKeyword;
			this.MetaDescription = s.MetaDescription;
			this.SiteName = s.SiteName;
			this.SiteFolder = s.SiteFolder;
			this.MainURL = s.MainURL;
			this.BlockIndex = s.BlockIndex;
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


		private static string SiteKeyPrefix = "cms_SiteData_";
		public static SiteData CurrentSite {
			get {
				string ContentKey = SiteKeyPrefix + CurrentSiteID.ToString();
				SiteData currentSite = null;
				try { currentSite = (SiteData)HttpContext.Current.Cache[ContentKey]; } catch { }
				if (currentSite == null) {
					using (SiteData sd = new SiteData()) {
						currentSite = sd.Get(CurrentSiteID);
					}
					if (currentSite != null) {
						HttpContext.Current.Cache.Insert(ContentKey, currentSite, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
					} else {
						HttpContext.Current.Cache.Remove(ContentKey);
					}
				}
				return currentSite;
			}
			set {
				string ContentKey = SiteKeyPrefix + CurrentSiteID.ToString();
				if (value == null) {
					HttpContext.Current.Cache.Remove(ContentKey);
				} else {
					HttpContext.Current.Cache.Insert(ContentKey, value, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}
			}
		}


		public SiteData GetCurrentSite() {
			//return Get(CurrentSiteID);
			return CurrentSite;
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

			// if updating the current site then blank out its cache
			if (CurrentSiteID == this.SiteID) {
				CurrentSite = null;
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


		public bool VerifyUserHasSiteAccess(Guid siteID, Guid userID) {

			//all admins have rights to all sites
			if (SiteData.IsAdmin) {
				return true;
			}

			// if user is neither admin nor editor, they should not be in the backend of the site
			if (!(SiteData.IsEditor || SiteData.IsAdmin)) {
				return false;
			}

			// by this point, the user is probably an editor, make sure they have rights to this site
			var lstSites = (from l in db.tblUserSiteMappings
							where l.UserId == userID
								 && l.SiteID == siteID
							select l.SiteID).ToList();

			if (lstSites.Count > 0) {
				return true;
			}

			return false;
		}


		public void CleanUpSerialData() {

			var lst = (from c in db.tblSerialCaches
					   where c.EditDate < DateTime.Now.AddHours(-3)
					   && c.SiteID == CurrentSiteID
					   select c).ToList();

			if (lst.Count > 0) {
				foreach (var l in lst) {
					db.tblSerialCaches.DeleteOnSubmit(l);
				}
				db.SubmitChanges();
			}

		}


		public void MapUserToSite(Guid siteID, Guid userID) {

			tblUserSiteMapping map = new tblUserSiteMapping();
			map.UserSiteMappingID = Guid.NewGuid();
			map.SiteID = siteID;
			map.UserId = userID;

			db.tblUserSiteMappings.InsertOnSubmit(map);
			db.SubmitChanges();

		}

		public static Guid CurrentSiteID {
			get {
				Guid _site = Guid.Empty;

				if (System.Configuration.ConfigurationManager.AppSettings["CarrotSiteID"] != null) {
					_site = new Guid(System.Configuration.ConfigurationManager.AppSettings["CarrotSiteID"].ToString());
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

		private static string _siteQS = null;
		public static string OldSiteQuerystring {
			get {
				if (_siteQS == null) {
					_siteQS = String.Empty;
					if (System.Configuration.ConfigurationManager.AppSettings["CarrotOldSiteQuerystring"] != null) {
						_siteQS = System.Configuration.ConfigurationManager.AppSettings["CarrotOldSiteQuerystring"].ToString().ToLower();
					}
				}
				return _siteQS;
			}
		}


		public bool BlockIndex { get; set; }
		public string MainURL { get; set; }
		public string MetaDescription { get; set; }
		public string MetaKeyword { get; set; }
		public Guid SiteID { get; set; }
		public string SiteName { get; set; }
		public string SiteFolder { get; set; }

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion


		public static void PerformRedirectToErrorPage(string sErrorKey, string sReqURL) {
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

			CustomErrorsSection section = (CustomErrorsSection)config.GetSection("system.web/customErrors");

			if (section != null) {
				if (section.Mode != CustomErrorsMode.Off) {
					CustomError configuredError = section.Errors[sErrorKey];
					if (configuredError != null) {
						if (!string.IsNullOrEmpty(configuredError.Redirect)) {
							HttpContext.Current.Response.Redirect(configuredError.Redirect + "?aspxerrorpath=" + sReqURL);
						}
					} else {
						if (!string.IsNullOrEmpty(section.DefaultRedirect)) {
							HttpContext.Current.Response.Redirect(section.DefaultRedirect + "?aspxerrorpath=" + sReqURL);
						}
					}
				}
			}
		}

		public static string DefaultDirectoryFilename {
			get { return "/default.aspx"; }
		}
		public static string DefaultTemplateFilename {
			get { return "/Manage/PlainTemplate.aspx"; }
		}

		public static string CurrentScriptName {
			get { return HttpContext.Current.Request.ServerVariables["script_name"].ToString(); }
		}

		public static string ReferringPage {
			get {
				var r = SiteData.CurrentScriptName;
				try { r = HttpContext.Current.Request.ServerVariables["http_referer"].ToString(); } catch { }
				if (string.IsNullOrEmpty(r))
					r = DefaultDirectoryFilename;
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
