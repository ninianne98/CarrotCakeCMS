using Carrotware.CMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Core {

	public class SecurityData {

		public SecurityData() { }

		public static MembershipRole FindMembershipRole(string roleName) {
			MembershipRole role = null;

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				role = (from r in _db.aspnet_Roles
						where r.LoweredRoleName == roleName
						select new MembershipRole(r)).FirstOrDefault();
			}

			return role;
		}

		public static MembershipRole FindMembershipRole(Guid roleID) {
			MembershipRole role = null;

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				role = (from r in _db.aspnet_Roles
						where r.RoleId == roleID
						select new MembershipRole(r)).FirstOrDefault();
			}

			return role;
		}

		public static List<MembershipRole> GetRoleList() {
			List<MembershipRole> roles = new List<MembershipRole>();

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				roles = (from r in _db.aspnet_Roles
						 orderby r.RoleName
						 select new MembershipRole(r)).ToList();
			}

			return roles;
		}

		public static List<MembershipRole> GetRoleListRestricted() {
			List<MembershipRole> roles = new List<MembershipRole>();

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				if (!SecurityData.IsAdmin) {
					roles = (from r in _db.aspnet_Roles
							 where r.RoleName != SecurityData.CMSGroup_Users && r.RoleName != SecurityData.CMSGroup_Admins
							 orderby r.RoleName
							 select new MembershipRole(r)).ToList();
				} else {
					roles = (from r in _db.aspnet_Roles
							 where r.RoleName != SecurityData.CMSGroup_Users
							 orderby r.RoleName
							 select new MembershipRole(r)).ToList();
				}
			}

			return roles;
		}

		public static List<MembershipUser> GetUserSearch(string searchTerm) {
			List<MembershipUser> usrs = null;

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				usrs = (from u in _db.aspnet_Users
						join m in _db.aspnet_Memberships on u.UserId equals m.UserId
						where u.UserName.Contains(searchTerm)
							|| m.Email.Contains(searchTerm)
						select Membership.GetUser(u.UserName)).Take(50).ToList();
			}

			return usrs;
		}

		public static List<MembershipUser> GetCreditUserSearch(string searchTerm) {
			List<MembershipUser> usrs = null;

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				List<Guid> admins = (from ur in _db.aspnet_UsersInRoles
									 join r in _db.aspnet_Roles on ur.RoleId equals r.RoleId
									 where r.RoleName == CMSGroup_Admins
									 select ur.UserId).ToList();

				List<Guid> editors = (from sm in _db.carrot_UserSiteMappings
									  where sm.SiteID == SiteData.CurrentSiteID
									  select sm.UserId).ToList();

				usrs = (from u in _db.aspnet_Users
						join m in _db.aspnet_Memberships on u.UserId equals m.UserId
						where (u.UserName.Contains(searchTerm)
									|| m.Email.Contains(searchTerm))
							&& admins.Union(editors).Contains(u.UserId)
						select Membership.GetUser(u.UserName)).Take(50).ToList();
			}

			return usrs;
		}

		public static List<MembershipUser> GetUserListByEmail(string email) {
			List<MembershipUser> usrs = new List<MembershipUser>();
			int iCt = 0;
			foreach (MembershipUser usr in Membership.FindUsersByEmail(email, 0, 25, out iCt)) {
				usrs.Add(usr);
			}
			return usrs;
		}

		public static List<MembershipUser> GetUserListByName(string usrName) {
			List<MembershipUser> usrs = new List<MembershipUser>();
			int iCt = 0;
			foreach (MembershipUser usr in Membership.FindUsersByName(usrName, 0, 25, out iCt)) {
				usrs.Add(usr);
			}
			return usrs;
		}

		public static List<MembershipUser> GetUserList() {
			List<MembershipUser> usrs = new List<MembershipUser>();
			foreach (MembershipUser usr in Membership.GetAllUsers()) {
				usrs.Add(usr);
			}
			return usrs;
		}

		public static List<MembershipUser> GetUsersInRole(string roleName) {
			string[] usersInRole = Roles.GetUsersInRole(roleName);
			List<MembershipUser> usrs = new List<MembershipUser>();
			foreach (string u in usersInRole) {
				foreach (MembershipUser usr in Membership.FindUsersByName(u)) {
					usrs.Add(usr);
				}
			}
			return usrs;
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

		private static string keyIsAdmin = "cms_IsAdmin";

		private static string keyIsSiteEditor = "cms_IsSiteEditor";

		public static bool GetIsAdminFromCache() {
			bool keyVal = false;

			if (SiteData.IsWebView && IsAuthenticated) {
				string key = String.Format("{0}_{1}", keyIsAdmin, SecurityData.CurrentUserIdentityName);
				if (HttpContext.Current.Cache[key] != null) {
					keyVal = Convert.ToBoolean(HttpContext.Current.Cache[key]);
				} else {
					keyVal = IsUserInRole(SecurityData.CMSGroup_Admins);
					HttpContext.Current.Cache.Insert(key, keyVal.ToString(), null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration);
				}
			}

			return keyVal;
		}

		public static bool GetIsSiteEditorFromCache() {
			bool keyVal = false;

			if (SiteData.IsWebView && IsAuthenticated) {
				string key = String.Format("{0}_{1}_{2}", keyIsSiteEditor, SecurityData.CurrentUserIdentityName, SiteData.CurrentSiteID);
				if (HttpContext.Current.Cache[key] != null) {
					keyVal = Convert.ToBoolean(HttpContext.Current.Cache[key]);
				} else {
					ExtendedUserData usrEx = SecurityData.CurrentExtendedUser;

					keyVal = (IsEditor || usrEx.IsEditor) && usrEx.MemberSiteIDs.Contains(SiteData.CurrentSiteID);

					HttpContext.Current.Cache.Insert(key, keyVal.ToString(), null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration);
				}
			}

			return keyVal;
		}

		public static bool IsAdmin {
			get {
				return GetIsAdminFromCache();
			}
		}

		public static bool IsEditor {
			get {
				try {
					return IsUserInRole(SecurityData.CMSGroup_Editors);
				} catch {
					return false;
				}
			}
		}

		public static bool IsUsers {
			get {
				try {
					return IsUserInRole(SecurityData.CMSGroup_Users);
				} catch {
					return false;
				}
			}
		}

		public static string AuthKey {
			get {
				if (SecurityData.IsAuthenticated) {
					return String.Format("cms_authToken_{0}", SecurityData.CurrentUserIdentityName);
				}
				return "cms_authToken";
			}
		}

		public static void ResetAuth() {
			if (SecurityData.IsAuthenticated) {
				HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName);

				authCookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];
				authCookie.Value = String.Empty;
				authCookie.Expires = DateTime.Now.AddDays(-10);
				authCookie.Path = "/";

				HttpContext.Current.Cache.Remove(SecurityData.AuthKey);
			}

			FormsAuthentication.SignOut();
		}

		public static void AuthCookieTime() {
			if (SecurityData.IsAuthenticated && FormsAuthentication.SlidingExpiration) {
				string key = SecurityData.AuthKey;

				string lastSet = HttpContext.Current.Cache[key] != null ? HttpContext.Current.Cache[key].ToString() : String.Empty;

				if (String.IsNullOrEmpty(lastSet)) {
					string tOut = SiteData.GetAuthFormProp("timeout");
					int timeout = Convert.ToInt32((tOut == null ? "30" : tOut));

					if (timeout < 5) {
						timeout = 5;
					}

					int expCache = timeout <= 60 ? 5 : 30;

					HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName);

					FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(SecurityData.CurrentUserIdentityName, true, timeout);

					string theTicket = FormsAuthentication.Encrypt(ticket);

					authCookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];
					authCookie.Value = theTicket;
					authCookie.Expires = DateTime.Now.AddMinutes((timeout + 2));
					authCookie.Path = "/";

					HttpContext.Current.Cache.Insert(key, SecurityData.CurrentUserIdentityName, null, DateTime.Now.AddMinutes(expCache), Cache.NoSlidingExpiration);
				}
			}
		}

		public static IPrincipal UserPrincipal {
			get {
				return HttpContext.Current.User;
			}
		}

		public static bool IsAuthenticated {
			get {
				if (SiteData.IsWebView && UserPrincipal.Identity.IsAuthenticated) {
					return true;
				}

				return false;
			}
		}

		public static string GetUserName() {
			if (SiteData.IsWebView && IsAuthenticated) {
				return UserPrincipal.Identity.Name;
			}

			return String.Empty;
		}

		public static string CurrentUserIdentityName {
			get {
				return GetUserName();
			}
		}

		public static bool IsUserInRole(string groupName) {
			return Roles.IsUserInRole(groupName);
		}

		private static string keyIsUserInRole = "cms_IsUserInRole";

		public static bool IsUserInRole(string userName, string groupName) {
			bool keyVal = false;

			if (SiteData.IsWebView && IsAuthenticated) {
				string key = String.Format("{0}_{1}_{2}", keyIsUserInRole, userName, groupName);

				if (HttpContext.Current.Cache[key] != null) {
					keyVal = Convert.ToBoolean(HttpContext.Current.Cache[key]);
				} else {
					keyVal = Roles.IsUserInRole(userName, groupName);

					HttpContext.Current.Cache.Insert(key, keyVal.ToString(), null, DateTime.Now.AddSeconds(15), Cache.NoSlidingExpiration);
				}
			}

			return keyVal;
		}

		public static bool IsSiteEditor {
			get {
				return GetIsSiteEditorFromCache();
			}
		}

		public static bool IsAuthEditor {
			get {
				if (SiteData.IsWebView && IsAuthenticated) {
					return AdvancedEditMode || IsAdmin || IsSiteEditor;
				} else {
					return false;
				}
			}
		}

		public static ExtendedUserData CurrentExtendedUser {
			get {
				ExtendedUserData currentUser = null;

				if (SiteData.IsWebView && IsAuthenticated) {
					Guid userID = SecurityData.CurrentUserGuid;
					string key = String.Format("cms_CurrentExtendedUser_{0}", userID);

					if (HttpContext.Current.Cache[key] != null) {
						currentUser = (ExtendedUserData)HttpContext.Current.Cache[key];
					} else {
						currentUser = new ExtendedUserData(userID);
						if (currentUser != null) {
							HttpContext.Current.Cache.Insert(key, currentUser, null, DateTime.Now.AddSeconds(90), Cache.NoSlidingExpiration);
						}
					}
				} else {
					currentUser = new ExtendedUserData();
					currentUser.UserId = Guid.Empty;
					currentUser.UserName = "anonymous-user-" + Guid.Empty.ToString();
				}
				return currentUser;
			}
		}

		public static Guid CurrentUserGuid {
			get {
				Guid _currentUserGuid = Guid.Empty;
				if (SiteData.IsWebView && CurrentUser != null && IsAuthenticated) {
					_currentUserGuid = new Guid(CurrentUser.ProviderUserKey.ToString());
				}
				return _currentUserGuid;
			}
		}

		public static MembershipUser CurrentUser {
			get {
				MembershipUser _currentUser = null;
				if (SiteData.IsWebView && IsAuthenticated) {
					_currentUser = GetUserByName(CurrentUserIdentityName);
				}
				return _currentUser;
			}
		}

		public static MembershipUser GetUserByGuid(Guid providerUserKey) {
			return Membership.GetUser(providerUserKey);
		}

		public static MembershipUser GetUserByName(string username) {
			return Membership.GetUser(username);
		}

		public static bool AdvancedEditMode {
			get {
				bool _Advanced = false;
				if (SiteData.IsWebView && IsAuthenticated) {
					if (HttpContext.Current.Request.QueryString["carrotedit"] != null && (SecurityData.IsAdmin || SecurityData.IsSiteEditor)) {
						_Advanced = true;
					} else {
						_Advanced = false;
					}
				}
				return _Advanced;
			}
		}
	}

	//================================================
	public class MembershipRole {

		public MembershipRole() {
			this.RoleId = Guid.Empty;
		}

		public MembershipRole(string roleName) {
			this.RoleName = roleName;
			this.RoleId = Guid.Empty;
		}

		public MembershipRole(string roleName, Guid roleID) {
			this.RoleName = roleName;
			this.RoleId = roleID;
		}

		internal MembershipRole(aspnet_Role role) {
			if (role != null) {
				this.ApplicationId = role.ApplicationId;
				this.RoleId = role.RoleId;
				this.RoleName = role.RoleName;
				this.Description = role.Description;
			}
		}

		public Guid ApplicationId { get; set; }

		public Guid RoleId { get; set; }

		public string RoleName { get; set; }

		public string LoweredRoleName { get { return this.RoleName.ToLowerInvariant(); } }

		public string Description { get; set; }

		public void Save() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				aspnet_Role role = (from l in _db.aspnet_Roles
									where l.LoweredRoleName == this.RoleName
										|| l.RoleId == this.RoleId
									select l).FirstOrDefault();

				if (role == null) {
					if (!Roles.RoleExists(this.RoleName) && this.RoleId == Guid.Empty) {
						Roles.CreateRole(this.RoleName);
					}
				} else {
					role.RoleName = this.RoleName;
					role.LoweredRoleName = role.RoleName.ToLowerInvariant();
					_db.SubmitChanges();
				}
			}
		}
	}
}