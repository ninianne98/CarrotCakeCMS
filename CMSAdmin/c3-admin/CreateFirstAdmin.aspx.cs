using Carrotware.CMS.Core;
using Carrotware.CMS.DBUpdater;
using Carrotware.CMS.Security.Models;
using Carrotware.CMS.UI.Base;
using System;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class CreateFirstAdmin : BasePage {

		protected void Page_Load(object sender, EventArgs e) {
			if (DatabaseSchemaState.UsersExist) {
				phStep1.Visible = false;
				phStep2.Visible = true;
			} else {
				SecurityData.ResetAuth();
				phStep1.Visible = true;
				phStep2.Visible = false;
			}

			divMsg.Visible = false;
			FailureText.Text = string.Empty;
		}

		protected void btnStepNextButton_Click(object sender, EventArgs e) {
			var userName = UserName.Text;
			var email = Email.Text;
			var password = Password.Text;

			var sd = new SecurityData();
			var user = new ApplicationUser { UserName = userName, Email = email };

			var nu = sd.CreateApplicationUser(user, password);
			var result = nu.IdentityResult;
			ExtendedUserData exUser = nu.ExtendedUserData;

			try {
				exUser.AddToRole(SecurityData.CMSGroup_Users);
				exUser.AddToRole(SecurityData.CMSGroup_Admins);

				phStep1.Visible = true;
				phStep2.Visible = false;

				Response.Redirect(SiteFilename.DashboardURL);
			} catch (Exception ex) {
				divMsg.Visible = true;
				FailureText.Text = ex.Message;
			}
		}
	}
}