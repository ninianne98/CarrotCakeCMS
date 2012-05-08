﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/
namespace Carrotware.CMS.UI.Admin {
	public partial class Logon : BasePage {

		// http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.login.aspx


		protected void Page_Load(object sender, EventArgs e) {
			if (Page.User.Identity.IsAuthenticated) {
				Response.Redirect("./default.aspx");
			}
		}

		protected void cmdLogon_Click(object sender, EventArgs e) {

		}

		protected void loginTemplate_LoggedIn(object sender, EventArgs e) {
			//var user = new Users(loginTemplate.UserName);
			if (Page.User.Identity.IsAuthenticated) {
				Response.Redirect("./default.aspx");
			}
		}

		protected void loginTemplate_LoggingIn(object sender, LoginCancelEventArgs e) {
			if (FormsAuthentication.Authenticate(loginTemplate.UserName, loginTemplate.Password)) {
				FormsAuthentication.RedirectFromLoginPage(loginTemplate.UserName, false);
			}
		}



	}
}
