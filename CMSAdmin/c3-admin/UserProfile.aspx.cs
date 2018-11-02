using Carrotware.CMS.Core;
using System;
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

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class UserProfile : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserFn);

			Master.UsesSaved = true;
			Master.HideSave();
			Master.SetSaveMessage("Profile Updated");

			divInfoMsg.Visible = false;
			InfoMessage.Text = String.Empty;

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
			MembershipUser usr = SecurityData.CurrentUser;
			usr.Email = txtEmail.Text;
			Membership.UpdateUser(usr);

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