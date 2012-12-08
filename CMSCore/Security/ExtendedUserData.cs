using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	public class ExtendedUserData : IDisposable {

		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion

		public Guid UserId { get; set; }
		public string UserName { get; set; }

		public string UserNickName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string EmailAddress { get; set; }
		public bool IsLockedOut { get; set; }

		public virtual DateTime LastActivityDate { get; set; }
		public virtual DateTime CreateDate { get; set; }
		public virtual DateTime LastLoginDate { get; set; }


		public string FullName_FirstLast {
			get {
				if (!string.IsNullOrEmpty(this.LastName)) {
					return String.Format("{0} {1}", this.FirstName, this.LastName);
				} else {
					return this.UserName;
				}
			}
		}

		public string FullName_LastFirst {
			get {
				if (!string.IsNullOrEmpty(this.LastName)) {
					return String.Format("{0}, {1}", this.LastName, this.FirstName);
				} else {
					return this.UserName;
				}
			}
		}


		public ExtendedUserData() { }

		public ExtendedUserData(string UserName) {
			vw_carrot_UserData rc = CompiledQueries.cqFindUserByName(db, UserName);
			LoadUserData(rc);
		}

		public ExtendedUserData(Guid UserID) {
			vw_carrot_UserData rc = CompiledQueries.cqFindUserByID(db, UserID);
			LoadUserData(rc);
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
			}
		}

		public void Save() {
			bool bNew = false;
			carrot_UserData usr = CompiledQueries.cqFindUserTblByID(db, this.UserId);

			if (usr == null) {
				usr = new carrot_UserData();
				usr.UserId = this.UserId;
				bNew = true;
			}

			usr.UserNickName = this.UserNickName;
			usr.FirstName = this.FirstName;
			usr.LastName = this.LastName;

			if (bNew) {
				db.carrot_UserDatas.InsertOnSubmit(usr);
			}

			db.SubmitChanges();

			this.UserId = usr.UserId;
		}

		public static List<ExtendedUserData> GetUserList() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				List<ExtendedUserData> lstUsr = (from u in CompiledQueries.cqGetUserList(_db)
												 select CreateUserData(u)).ToList();
				return lstUsr;
			}
		}

		internal static ExtendedUserData CreateUserData(vw_carrot_UserData c) {
			ExtendedUserData cont = null;
			if (c != null) {
				cont = new ExtendedUserData();
				cont.UserId = c.UserId;
				cont.UserNickName = c.UserNickName;
				cont.FirstName = c.FirstName;
				cont.LastName = c.LastName;
				cont.EmailAddress = c.LoweredEmail;
				cont.IsLockedOut = c.IsLockedOut;
				cont.UserName = c.UserName;
				cont.LastActivityDate = c.LastActivityDate;
				cont.CreateDate = c.CreateDate;
				cont.LastLoginDate = c.LastLoginDate;
			}

			return cont;
		}


	}
}
