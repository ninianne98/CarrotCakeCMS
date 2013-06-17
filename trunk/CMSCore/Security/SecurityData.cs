using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Carrotware.CMS.Data;
using System.Web.Caching;
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
	public class SecurityData {

		public SecurityData() { }

		public static MembershipRole FindMembershipRole(string RoleName) {
			MembershipRole role = null;

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				role = (from l in _db.aspnet_Roles
						where l.LoweredRoleName.ToLower() == RoleName.ToLower()
						select new MembershipRole(l)).FirstOrDefault();
			}

			return role;
		}

		public static MembershipRole FindMembershipRole(Guid roleID) {
			MembershipRole role = null;

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				role = (from l in _db.aspnet_Roles
						where l.RoleId == roleID
						select new MembershipRole(l)).FirstOrDefault();
			}

			return role;
		}

		public static List<MembershipRole> GetRoleList() {
			List<MembershipRole> roles = new List<MembershipRole>();

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				roles = (from l in _db.aspnet_Roles
						 orderby l.RoleName
						 select new MembershipRole(l)).ToList();
			}

			return roles;
		}

		public static List<MembershipRole> GetRoleListRestricted() {
			List<MembershipRole> roles = new List<MembershipRole>();

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				if (!SecurityData.IsAdmin) {
					roles = (from l in _db.aspnet_Roles
							 where l.RoleName != SecurityData.CMSGroup_Users && l.RoleName != SecurityData.CMSGroup_Admins
							 orderby l.RoleName
							 select new MembershipRole(l)).ToList();
				} else {
					roles = (from l in _db.aspnet_Roles
							 where l.RoleName != SecurityData.CMSGroup_Users
							 orderby l.RoleName
							 select new MembershipRole(l)).ToList();
				}
			}

			return roles;
		}

		public static List<MembershipUser> GetUserSearch(string searchTerm) {
			List<MembershipUser> usrs = null;
			//usrs = GetUserListByEmail(searchTerm);
			//List<MembershipUser> usrs2 = GetUserListByName(searchTerm);
			//List<string> usrKeys = (from u in usrs
			//                        select u.ProviderUserKey.ToString()).ToList();
			//usrs2.RemoveAll(x => usrKeys.Contains(x.ProviderUserKey.ToString()));
			//usrs = usrs.Union(usrs2).ToList();

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				usrs = (from u in _db.aspnet_Users
						join m in _db.aspnet_Memberships on u.UserId equals m.UserId
						where u.UserName.ToLower().Contains(searchTerm)
							|| m.Email.ToLower().Contains(searchTerm)
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

		public static List<MembershipUser> GetUsersInRole(string groupName) {
			string[] usersInRole = Roles.GetUsersInRole(groupName);
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

		public static bool IsAdmin {
			get {
				try {
					return Roles.IsUserInRole(SecurityData.CMSGroup_Admins);
				} catch {
					return false;
				}
			}
		}
		public static bool IsEditor {
			get {
				try {
					return Roles.IsUserInRole(SecurityData.CMSGroup_Editors);
				} catch {
					return false;
				}
			}
		}
		public static bool IsUsers {
			get {
				try {
					return Roles.IsUserInRole(SecurityData.CMSGroup_Users);
				} catch {
					return false;
				}
			}
		}

		public static bool IsSiteEditor {
			get {
				if (SiteData.IsWebView && HttpContext.Current.User.Identity.IsAuthenticated) {
					ExtendedUserData usrEx = SecurityData.CurrentExtendedUser;

					return usrEx.IsEditor && usrEx.MemberSiteIDs.Contains(SiteData.CurrentSiteID);
				} else {
					return false;
				}
			}
		}


		public static bool IsAuthEditor {
			get {
				if (SiteData.IsWebView) {
					return AdvancedEditMode || IsAdmin || IsSiteEditor;
				} else {
					return false;
				}
			}
		}


		public static ExtendedUserData CurrentExtendedUser {
			get {
				Guid userID = SecurityData.CurrentUserGuid;
				string cacheKey = "cms_CurrentExUser" + userID.ToString();
				ExtendedUserData currentUser = null;
				if (SiteData.IsWebView && userID != Guid.Empty) {
					try { currentUser = (ExtendedUserData)HttpContext.Current.Cache[cacheKey]; } catch { }
					if (currentUser == null) {
						currentUser = new ExtendedUserData(userID);
						if (currentUser != null) {
							HttpContext.Current.Cache.Insert(cacheKey, currentUser, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration);
						} else {
							HttpContext.Current.Cache.Remove(cacheKey);
						}
					}
				} else {
					currentUser = new ExtendedUserData();
					currentUser.UserId = Guid.Empty;
					currentUser.UserName = "anonymous-user-" + userID.ToString();
				}
				return currentUser;
			}
		}

		public static Guid CurrentUserGuid {
			get {
				Guid _currentUserGuid = Guid.Empty;
				if (CurrentUser != null) {
					_currentUserGuid = new Guid(CurrentUser.ProviderUserKey.ToString());
				}
				return _currentUserGuid;
			}
		}

		public static MembershipUser CurrentUser {

			get {
				MembershipUser _currentUser = null;
				if (SiteData.IsWebView && HttpContext.Current.User.Identity.IsAuthenticated) {
					if (!String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)) {
						_currentUser = Membership.GetUser(HttpContext.Current.User.Identity.Name);
					}
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
				if (SiteData.IsWebView) {
					if (HttpContext.Current.User.Identity.IsAuthenticated) {
						if (HttpContext.Current.Request.QueryString["carrotedit"] != null && (SecurityData.IsAdmin || SecurityData.IsSiteEditor)) {
							_Advanced = true;
						} else {
							_Advanced = false;
						}
					}
				}
				return _Advanced;
			}
		}

	}


	//============
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

		public string LoweredRoleName { get { return this.RoleName.ToLower(); } }

		public string Description { get; set; }

		public void Save() {

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {

				aspnet_Role role = (from l in _db.aspnet_Roles
									where l.LoweredRoleName.ToLower() == this.RoleName.ToLower()
										|| l.RoleId == this.RoleId
									select l).FirstOrDefault();

				if (role == null) {
					if (!Roles.RoleExists(this.RoleName) && this.RoleId == Guid.Empty) {
						Roles.CreateRole(this.RoleName);
					}
				} else {
					role.RoleName = this.RoleName;
					role.LoweredRoleName = role.RoleName.ToLower();
					_db.SubmitChanges();
				}
			}
		}


	}
}
