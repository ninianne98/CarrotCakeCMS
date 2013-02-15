using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
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
	public partial class DatabaseSetup : BasePage {

		bool bOK = false;

		protected void Page_Load(object sender, EventArgs e) {
			DatabaseUpdate du = new DatabaseUpdate();

			if (!string.IsNullOrEmpty(Request.QueryString["signout"])) {
				FormsAuthentication.SignOut();
			}

			List<DatabaseUpdateMessage> lst = new List<DatabaseUpdateMessage>();

			btnLogin.Visible = false;
			btnCreate.Visible = false;

			if (DatabaseUpdate.LastSQLError != null) {
				du.HandleResponse(lst, DatabaseUpdate.LastSQLError.Message);
				DatabaseUpdate.LastSQLError = null;
			} else {

				bool bUpdate = true;

				if (!du.DoCMSTablesExist()) {
					bUpdate = false;
				}

				bUpdate = du.DatabaseNeedsUpdate();

				if (bUpdate) {

					DatabaseUpdateStatus status = du.PerformUpdates();
					lst = du.MergeMessages(lst, status.Messages);

				} else {
					du.HandleResponse(lst, "Database up-to-date ");
				}

				bUpdate = du.DatabaseNeedsUpdate();

				if (!bUpdate && DatabaseUpdate.LastSQLError == null) {
					if (du.UsersExist) {
						btnLogin.Visible = true;
					} else {
						btnCreate.Visible = true;
					}
				}
			}

			if (DatabaseUpdate.LastSQLError != null) {
				du.HandleResponse(lst, DatabaseUpdate.LastSQLError.Message);
			}

			if (lst.Where(x => !string.IsNullOrEmpty(x.ExceptionText)).Count() > 0) {
				bOK = false;
			} else {
				bOK = true;
			}

			rpMessages.DataSource = lst.OrderBy(x => x.Order);
			rpMessages.DataBind();

			using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
				cmsHelper.ResetConfigs();
			}
		}

		protected string CSSMsg() {
			string sCSS = "okMsg";
			if (!bOK) {
				sCSS = "errMsg";
			}

			return sCSS;
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