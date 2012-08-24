using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Admin {
	public partial class DatabaseSetup : BasePage {

		protected void Page_Load(object sender, EventArgs e) {
			DatabaseUpdate du = new DatabaseUpdate();
			litMsg.Text = "";
			string ret = "";

			//FormsAuthentication.SignOut();

			btnLogin.Visible = false;
			btnCreate.Visible = false;

			if (du.LastSQLError != null) {
				litMsg.Text += "<hr>" + du.LastSQLError.Message.ToString();
				du.LastSQLError = null;
			} else {

				if (!du.DoCMSTablesExist()) {
					ret = du.CreateCMSDatabase();
					litMsg.Text += "<hr> Create Database <br> " + ret;
				} else {
					litMsg.Text += "<hr> Database already exists ";
				}

				bool bUpdate = du.DatabaseNeedsUpdate();

				if (bUpdate) {
					ret = du.AlterStep01();
					litMsg.Text += "<hr> Update 1 <br> " + ret;
					ret = du.AlterStep02();
					litMsg.Text += "<hr> Update 2 <br> " + ret;
					ret = du.AlterStep03();
					litMsg.Text += "<hr> Update 3 <br> " + ret;
					ret = du.AlterStep04();
					litMsg.Text += "<hr> Update 4 <br> " + ret;
				} else {
					litMsg.Text += "<hr> Database up-to-date ";
				}

				bUpdate = du.DatabaseNeedsUpdate();

				if (!bUpdate && du.LastSQLError == null) {
					if (du.UsersExist()) {
						btnLogin.Visible = true;
					} else {
						btnCreate.Visible = true;
					}
				}
			}


			if (du.LastSQLError != null) {
				litMsg.Text += "<hr>" + du.LastSQLError.Message.ToString();
			}



			litMsg.Text += "<hr>";
		}

		protected void btnLogin_Click(object sender, EventArgs e) {
			Response.Redirect("./Logon.aspx");
		}

		protected void btnCreate_Click(object sender, EventArgs e) {
			Response.Redirect("./CreateFirstAdmin.aspx");
		}



	}
}