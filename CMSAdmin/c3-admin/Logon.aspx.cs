using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
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
	public partial class Logon : BasePage {

		protected HtmlGenericControl divMsg {
			get {
				return ((HtmlGenericControl)loginTemplate.FindControl("divMsg"));
			}
		}

		protected void Page_Load(object sender, EventArgs e) {

			DatabaseUpdate du = new DatabaseUpdate();
			divMsg.Visible = false;

			if (DatabaseUpdate.FailedSQL || du.DatabaseNeedsUpdate() || !du.UsersExist) {
				FormsAuthentication.SignOut();
				Response.Redirect(SiteFilename.DatabaseSetupURL);
			}

			if (Page.User.Identity.IsAuthenticated) {
				Response.Redirect(SiteFilename.DashboardURL);
			}
		}

		protected void cmdLogon_Click(object sender, EventArgs e) {

		}

		protected void loginTemplate_LoggedIn(object sender, EventArgs e) {
			if (Page.User.Identity.IsAuthenticated) {
				Response.Redirect(SiteFilename.DashboardURL);
			}
		}

		protected void loginTemplate_LoggingIn(object sender, LoginCancelEventArgs e) {
			if (FormsAuthentication.Authenticate(loginTemplate.UserName, loginTemplate.Password)) {
				FormsAuthentication.RedirectFromLoginPage(loginTemplate.UserName, false);
			} else {
				divMsg.Visible = true;
			}
		}

	}
}
