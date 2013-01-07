using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
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
	public class SecurityData {

		public SecurityData() { }


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
				if (HttpContext.Current != null) {
					return AdvancedEditMode || IsAdmin || IsEditor;
				} else {
					return false;
				}
			}
		}

		public static Guid CurrentUserGuid {
			get {
				Guid _currentUserGuid = Guid.Empty;
				if (HttpContext.Current != null) {
					if (HttpContext.Current.User.Identity.IsAuthenticated) {
						if (!String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) && CurrentUser != null) {
							_currentUserGuid = new Guid(CurrentUser.ProviderUserKey.ToString());
						}
					}
				}
				return _currentUserGuid;
			}
		}

		public static MembershipUser CurrentUser {

			get {
				MembershipUser _currentUser = null;
				if (HttpContext.Current != null) {
					if (HttpContext.Current.User.Identity.IsAuthenticated) {
						if (!String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)) {
							_currentUser = Membership.GetUser(HttpContext.Current.User.Identity.Name);
						}
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
				if (HttpContext.Current != null) {
					if (HttpContext.Current.User.Identity.IsAuthenticated) {
						if (HttpContext.Current.Request.QueryString["carrotedit"] != null && (SecurityData.IsAdmin || SecurityData.IsEditor)) {
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
}
