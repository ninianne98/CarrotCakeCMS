using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

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

	public partial class Logon : BasePage {

		protected void Page_Load(object sender, EventArgs e) {
			divMsg.Visible = false;
			FailureText.Text = string.Empty;

			lnkForgot.HRef = SiteFilename.ForgotPasswordURL;

			CheckDatabase();

			if (SecurityData.IsAuthenticated) {
				Response.Redirect(SiteFilename.DashboardURL);
			}
		}

		protected async void cmdLogon_Click(object sender, EventArgs e) {
			var authState = await DoAuthAsync();

			if (authState) {
				Response.Redirect(SiteFilename.DashboardURL);
			} else {
				divMsg.Visible = true;
				FailureText.Text = "Invalid login attempt.";
			}
		}

		protected async Task<bool> DoAuthAsync() {
			var userName = txtUserName.Text;
			var pass = txtPassword.Text;

			if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(pass)) {
				var user = securityHelper.UserManager.FindByName(userName);
				var result = (user == null) ? false : await securityHelper.SimpleLogInAsync(userName, pass, false);

				if (result && user != null & user.IsLocked == false) {
					await securityHelper.UserManager.ResetAccessFailedCountAsync(user.Id);

					Response.Redirect(SiteFilename.DashboardURL);

					return true;
				} else {
					if (user != null) {
						if (user.IsLocked == false) {
							user.AccessFailedCount++;
							securityHelper.UserManager.Update(user);
						}
					}
				}
			}

			return false;
		}
	}
}