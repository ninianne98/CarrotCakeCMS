using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	public class ExtendedUserData {

		public Guid UserId { get; set; }
		public string UserName { get; set; }

		public string UserNickName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string UserBio { get; set; }

		public string EmailAddress { get; set; }
		public bool IsLockedOut { get; set; }

		public string EditorURL { get { return SiteData.CurrentSite.BlogEditorFolderPath + this.UserName + ".aspx"; } }

		public virtual DateTime LastActivityDate { get; set; }
		public virtual DateTime CreateDate { get; set; }
		public virtual DateTime LastLoginDate { get; set; }

		public override string ToString() {
			return this.FullName_FirstLast;
		}

		public string FullName_FirstLast {
			get {
				if (!string.IsNullOrEmpty(this.LastName)) {
					return String.Format("{0} {1}", this.FirstName, this.LastName);
				} else {
					if (!string.IsNullOrEmpty(this.UserName)) {
						return this.UserName;
					} else {
						return "Unknown User";
					}
				}
			}
		}

		public string FullName_LastFirst {
			get {
				if (!string.IsNullOrEmpty(this.LastName)) {
					return String.Format("{0}, {1}", this.LastName, this.FirstName);
				} else {
					if (!string.IsNullOrEmpty(this.UserName)) {
						return this.UserName;
					} else {
						return "Unknown User";
					}
				}
			}
		}


		public ExtendedUserData() { }

		public ExtendedUserData(string UserName) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				vw_carrot_UserData rc = CompiledQueries.cqFindUserByName(_db, UserName);
				LoadUserData(rc);
			}
		}

		public ExtendedUserData(Guid UserID) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				vw_carrot_UserData rc = CompiledQueries.cqFindUserByID(_db, UserID);
				LoadUserData(rc);
			}
		}

		private void LoadUserData(vw_carrot_UserData c) {
			this.UserId = Guid.Empty;
			this.EmailAddress = "";
			this.UserName = "";

			if (c != null) {
				this.UserId = c.UserId;
				this.UserNickName = c.UserNickName;
				this.FirstName = c.FirstName;
				this.LastName = c.LastName;
				this.EmailAddress = c.LoweredEmail;
				this.IsLockedOut = c.IsLockedOut;
				this.UserName = c.UserName;
				this.LastActivityDate = c.LastActivityDate;
				this.CreateDate = c.CreateDate;
				this.LastLoginDate = c.LastLoginDate;
				this.UserBio = c.UserBio;
			}
		}

		public List<SiteData> GetSiteList() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				return (from m in _db.carrot_UserSiteMappings
						join s in _db.carrot_Sites on m.SiteID equals s.SiteID
						where m.UserId == this.UserId
						select new SiteData(s)).ToList();
			}
		}

		public bool AddToRole(string roleName) {
			if (!Roles.IsUserInRole(this.UserName, roleName)) {
				Roles.AddUserToRole(this.UserName, roleName);
				return true;
			} else {
				return false;
			}
		}

		public bool RemoveFromRole(string roleName) {
			if (Roles.IsUserInRole(this.UserName, roleName)) {
				Roles.RemoveUserFromRole(this.UserName, roleName);
				return true;
			} else {
				return false;
			}
		}


		public bool AddToSite(Guid siteID) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				carrot_UserSiteMapping map = (from m in _db.carrot_UserSiteMappings
											  where m.UserId == this.UserId
												&& m.SiteID == siteID
											  select m).FirstOrDefault();

				if (map == null) {
					map = new carrot_UserSiteMapping();
					map.UserSiteMappingID = Guid.NewGuid();
					map.SiteID = siteID;
					map.UserId = this.UserId;

					_db.carrot_UserSiteMappings.InsertOnSubmit(map);
					_db.SubmitChanges();

					return true;
				} else {
					return false;
				}
			}
		}

		public bool RemoveFromSite(Guid siteID) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				carrot_UserSiteMapping map = (from m in _db.carrot_UserSiteMappings
											  where m.UserId == this.UserId
												&& m.SiteID == siteID
											  select m).FirstOrDefault();

				if (map != null) {
					_db.carrot_UserSiteMappings.DeleteOnSubmit(map);
					_db.SubmitChanges();

					return true;
				} else {
					return false;
				}
			}
		}


		public void Save() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				bool bNew = false;
				carrot_UserData usr = CompiledQueries.cqFindUserTblByID(_db, this.UserId);

				if (usr == null) {
					usr = new carrot_UserData();
					usr.UserId = this.UserId;
					bNew = true;
				}

				usr.UserNickName = this.UserNickName;
				usr.FirstName = this.FirstName;
				usr.LastName = this.LastName;
				usr.UserBio = this.UserBio;

				if (bNew) {
					_db.carrot_UserDatas.InsertOnSubmit(usr);
				}

				_db.SubmitChanges();

				this.UserId = usr.UserId;
			}
		}

		internal ExtendedUserData(vw_carrot_UserData c) {
			if (c != null) {
				this.UserId = c.UserId;
				this.UserNickName = c.UserNickName;
				this.FirstName = c.FirstName;
				this.LastName = c.LastName;
				this.EmailAddress = c.LoweredEmail;
				this.IsLockedOut = c.IsLockedOut;
				this.UserName = c.UserName;
				this.LastActivityDate = c.LastActivityDate;
				this.CreateDate = c.CreateDate;
				this.LastLoginDate = c.LastLoginDate;
				this.UserBio = c.UserBio;
			}
		}

		public bool IsAdmin {
			get {
				try {
					return Roles.IsUserInRole(this.UserName, SecurityData.CMSGroup_Admins);
				} catch {
					return false;
				}
			}
		}
		public bool IsEditor {
			get {
				try {
					return Roles.IsUserInRole(this.UserName, SecurityData.CMSGroup_Editors);
				} catch {
					return false;
				}
			}
		}





		public static List<ExtendedUserData> GetUserList() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				List<ExtendedUserData> lstUsr = (from u in CompiledQueries.cqGetUserList(_db)
												 select new ExtendedUserData(u)).ToList();
				return lstUsr;
			}
		}

		public static IQueryable<ExtendedUserData> GetUsers() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<ExtendedUserData> lstUsr = (from u in CompiledQueries.cqGetUserList(_db)
													   select new ExtendedUserData(u));
				return lstUsr;
			}
		}

		public static ExtendedUserData GetEditorFromURL() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				vw_carrot_EditorURL query = CompiledQueries.cqGetEditorByURL(_db, SiteData.CurrentSiteID, SiteData.CurrentScriptName);
				if (query != null) {
					ExtendedUserData usr = new ExtendedUserData(query.UserId);
					return usr;
				} else {
					return null;
				}
			}
		}

	}
}
