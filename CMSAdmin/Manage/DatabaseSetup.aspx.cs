using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
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


namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class DatabaseSetup : BasePage {

		protected void Page_Load(object sender, EventArgs e) {
			DatabaseUpdate du = new DatabaseUpdate();
			litMsg.Text = "";

			//FormsAuthentication.SignOut();

			btnLogin.Visible = false;
			btnCreate.Visible = false;

			if (DatabaseUpdate.LastSQLError != null) {
				HandleResponse(DatabaseUpdate.LastSQLError.Message);
				DatabaseUpdate.LastSQLError = null;
			} else {

				bool bUpdate = true;

				if (!du.DoCMSTablesExist()) {
					HandleResponse("Create Database ", du.CreateCMSDatabase());
					bUpdate = false;
				} else {
					HandleResponse("Database already exists ");
				}

				bUpdate = du.DatabaseNeedsUpdate();

				int iUpdate = 1;

				if (bUpdate) {
					if (!du.IsPostStep04()) {
						HandleResponse("Update  " + (iUpdate++).ToString() + " ", du.AlterStep00());
						HandleResponse("Update  " + (iUpdate++).ToString() + " ", du.AlterStep01());
						HandleResponse("Update  " + (iUpdate++).ToString() + " ", du.AlterStep02());
						HandleResponse("Update  " + (iUpdate++).ToString() + " ", du.AlterStep03());
						HandleResponse("Update  " + (iUpdate++).ToString() + " ", du.AlterStep04());
					}
					HandleResponse("Update  " + (iUpdate++).ToString() + " ", du.AlterStep05());
					HandleResponse("Update  " + (iUpdate++).ToString() + " ", du.AlterStep06());
				} else {
					HandleResponse("Database up-to-date ");
				}

				bUpdate = du.DatabaseNeedsUpdate();

				if (!bUpdate && DatabaseUpdate.LastSQLError == null) {
					if (du.UsersExist()) {
						btnLogin.Visible = true;
					} else {
						btnCreate.Visible = true;
					}
				}
			}

			if (DatabaseUpdate.LastSQLError != null) {
				HandleResponse(DatabaseUpdate.LastSQLError.Message);
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
					if (execMessage.LastException != null && !string.IsNullOrEmpty(execMessage.LastException.Message)) {
						sResponse += "<i>" + execMessage.LastException.Message + "</i> <br />";
					}
				}
			}

			litMsg.Text += sResponse;
		}


		protected void btnLogin_Click(object sender, EventArgs e) {
			if (Page.User.Identity.IsAuthenticated) {
				Response.Redirect("./default.aspx");
			}

			Response.Redirect("./Logon.aspx");
		}

		protected void btnCreate_Click(object sender, EventArgs e) {
			Response.Redirect("./CreateFirstAdmin.aspx");
		}


	}
}