using Carrotware.CMS.Core;
using Microsoft.AspNet.Identity;
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

	public partial class UserProfile1 : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserFn);

			Master.UsesSaved = true;
			Master.HideSave();
			Master.SetSaveMessage("Profile Updated");

			divInfoMsg.Visible = false;
			InfoMessage.Text = string.Empty;

			if (!IsPostBack) {
				txtEmail.Text = SecurityData.CurrentUser.Email;
				ExtendedUserData exUsr = new ExtendedUserData(SecurityData.CurrentUser.UserName);
				txtNickName.Text = exUsr.UserNickName;
				txtFirstName.Text = exUsr.FirstName;
				txtLastName.Text = exUsr.LastName;
				reBody.Text = exUsr.UserBio;
			}
		}

		protected void btnSaveEmail_Click(object sender, EventArgs e) {
			var usr = SecurityData.CurrentUser;
			usr.Email = txtEmail.Text;

			IdentityResult result = securityHelper.UserManager.SetEmail(usr.UserKey, usr.Email);
			ExtendedUserData exUsr = new ExtendedUserData(SecurityData.CurrentUser.UserName);

			exUsr.UserNickName = txtNickName.Text;
			exUsr.FirstName = txtFirstName.Text;
			exUsr.LastName = txtLastName.Text;
			exUsr.UserBio = reBody.Text;

			exUsr.Save();

			divInfoMsg.Visible = true;
			InfoMessage.Text = "Profile Updated";

			Master.ShowSave();
		}
	}
}