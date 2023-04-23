﻿using Carrotware.CMS.Core;
using Carrotware.CMS.DBUpdater;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

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

	public partial class DatabaseSetup : BasePage {
		private bool bOK = false;

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["signout"])) {
				SecurityData.ResetAuth();
				Response.Redirect(SiteFilename.DatabaseSetupURL);
			}

			DatabaseUpdate du = new DatabaseUpdate(true);

			var lst = new List<DatabaseUpdateMessage>();

			btnLogin.Visible = false;
			btnCreate.Visible = false;

			if (DatabaseUpdate.LastSQLError != null) {
				du.HandleResponse(lst, DatabaseUpdate.LastSQLError);
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
					DataInfo ver = DatabaseUpdate.GetDbSchemaVersion();
					du.HandleResponse(lst, "Database up-to-date [" + ver.DataValue + "] ");
				}

				bUpdate = du.DatabaseNeedsUpdate();

				if (!bUpdate && DatabaseUpdate.LastSQLError == null) {
					if (DatabaseUpdate.UsersExist) {
						btnLogin.Visible = true;
					} else {
						btnCreate.Visible = true;
					}
				}
			}

			if (DatabaseUpdate.LastSQLError != null) {
				du.HandleResponse(lst, DatabaseUpdate.LastSQLError);
			}

			if (lst.Where(x => !string.IsNullOrEmpty(x.ExceptionText)).Count() > 0) {
				bOK = false;
			} else {
				bOK = true;
			}

			GeneralUtilities.BindRepeater(rpMessages, lst.OrderBy(x => x.Order));

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
			if (SecurityData.IsAuthenticated) {
				Response.Redirect(SiteFilename.DashboardURL);
			}

			Response.Redirect(SiteFilename.LogonURL);
		}

		protected void btnCreate_Click(object sender, EventArgs e) {
			Response.Redirect(SiteFilename.CreateFirstAdminURL);
		}
	}
}