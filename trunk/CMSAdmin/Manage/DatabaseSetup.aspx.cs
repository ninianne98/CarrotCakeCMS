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

			//FormsAuthentication.SignOut();

			btnLogin.Visible = false;
			btnCreate.Visible = false;

			if (du.LastSQLError != null) {
				HandleResponse(du.LastSQLError.Message.ToString());
				du.LastSQLError = null;
			} else {

				bool bUpdate = true;

				if (!du.DoCMSTablesExist()) {
					HandleResponse("Create Database ", du.CreateCMSDatabase());
					bUpdate = false;
				} else {
					HandleResponse("Database already exists ");
					bUpdate = du.DatabaseNeedsUpdate();
				}

				if (bUpdate) {
					HandleResponse("Update 1 ", du.AlterStep01());
					HandleResponse("Update 2 ", du.AlterStep02());
					HandleResponse("Update 3 ", du.AlterStep03());
					HandleResponse("Update 4 ", du.AlterStep04());
				} else {
					HandleResponse("Database up-to-date ");
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
				HandleResponse(du.LastSQLError.Message.ToString());
			}

			HandleResponse("  ");
		}

		protected void HandleResponse(string sMsg) {
			HandleResponse(sMsg, null);
		}

		protected void HandleResponse(string sMsg, DatabaseUpdateResponse execMessage) {
			string sResponse = "";

			if (!string.IsNullOrEmpty(sMsg)) {

				sResponse += "<hr /> <b>" + sMsg + " </b><br /> ";

				if (execMessage != null) {
					sResponse += execMessage.Response + " <br />";
					if (execMessage.LastException != null && !string.IsNullOrEmpty(execMessage.LastException.Message.ToString())) {
						sResponse += "<i>" + execMessage.LastException.Message.ToString() + "</i> <br />";
					}
				}
			}

			litMsg.Text += sResponse;
		}


		protected void btnLogin_Click(object sender, EventArgs e) {
			Response.Redirect("./Logon.aspx");
		}

		protected void btnCreate_Click(object sender, EventArgs e) {
			Response.Redirect("./CreateFirstAdmin.aspx");
		}


	}
}