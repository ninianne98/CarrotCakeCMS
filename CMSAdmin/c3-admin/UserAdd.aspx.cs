using Carrotware.CMS.Core;
using Carrotware.CMS.Security.Models;
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

	public partial class UserAdd : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserAdmin);
			phStep1.Visible = SecurityData.IsAdmin;
			phStep2.Visible = false;

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

			if (result != null && result.Succeeded) {

				exUser.AddToRole(SecurityData.CMSGroup_Users);

				try {
					siteHelper.MapUserToSite(SiteID, exUser.UserId);

					phStep1.Visible = false;
					phStep2.Visible = true;

					Response.Redirect(string.Format("{0}?id={1}", SiteFilename.UserURL, exUser.UserId));
				} catch (Exception ex) {
					divMsg.Visible = true;
					FailureText.Text = ex.Message;
				}
			} else {
				FailureText.Text = string.Empty;

				if (result != null) {
					divMsg.Visible = true;
					foreach (var err in result.Errors) {
						FailureText.Text += err + "<br>";
					}
				}
			}

		}

	}
}