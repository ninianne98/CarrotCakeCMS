using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Data;
using Carrotware.CMS.DBUpdater;
using Carrotware.CMS.UI.Base;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class CreateFirstAdmin : BasePage {
		protected void Page_Load(object sender, EventArgs e) {
			DatabaseUpdate du = new DatabaseUpdate();
			if (du.UsersExist()) {
				createWizard.Visible = false;
				lblLogon.Visible = true;
			} else {
				createWizard.Visible = true;
				lblLogon.Visible = false;
			}
		}

		protected void createWizard_CreatedUser(object sender, EventArgs e) {
			var pb = ProfileBase.Create(createWizard.UserName, false);
			Roles.AddUserToRole(createWizard.UserName, SecurityData.CMSGroup_Users);

			try {
				MembershipUser usr = Membership.GetUser(createWizard.UserName);
				Guid userID = new Guid(usr.ProviderUserKey.ToString());

				if (!Roles.IsUserInRole(usr.UserName, SecurityData.CMSGroup_Users)) {
					Roles.AddUserToRole(usr.UserName, SecurityData.CMSGroup_Users);
				}
				if (!Roles.IsUserInRole(usr.UserName, SecurityData.CMSGroup_Admins)) {
					Roles.AddUserToRole(usr.UserName, SecurityData.CMSGroup_Admins);
				}
			} catch (Exception ex) {

			}
		}

		protected void createWizard_CreatingUser(object sender, LoginCancelEventArgs e) {

		}


	}
}
