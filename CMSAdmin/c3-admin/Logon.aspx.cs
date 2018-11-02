using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using System;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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

	public partial class Logon : BasePage {

		protected HtmlGenericControl divMsg {
			get {
				return ((HtmlGenericControl)loginTemplate.FindControl("divMsg"));
			}
		}

		protected HtmlAnchor lnkForgot {
			get {
				return ((HtmlAnchor)loginTemplate.FindControl("lnkForgot"));
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			divMsg.Visible = false;

			lnkForgot.HRef = SiteFilename.ForgotPasswordURL;

			CheckDatabase();

			if (SecurityData.IsAuthenticated) {
				Response.Redirect(SiteFilename.DashboardURL);
			}
		}

		protected void cmdLogon_Click(object sender, EventArgs e) {
		}

		protected void loginTemplate_LoggedIn(object sender, EventArgs e) {
			if (SecurityData.IsAuthenticated) {
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