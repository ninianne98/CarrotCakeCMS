using Carrotware.CMS.Core;
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
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class DatabaseSetup : BasePage {
		private bool _ok = false;
		private bool _update = true;

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["signout"])) {
				SecurityData.ResetAuth();
				Response.Redirect(SiteFilename.DatabaseSetupURL);
			}

			var du = new DatabaseUpdate(true);

			var lst = new List<DatabaseUpdateMessage>();

			btnLogin.Visible = false;
			btnCreate.Visible = false;

			if (DatabaseSchemaState.LastSQLError != null) {
				du.HandleResponse(lst, DatabaseSchemaState.LastSQLError);
				DatabaseSchemaState.LastSQLError = null;
			} else {
				if (!du.DoCMSTablesExist()) {
					_update = false;
				}

				_update = du.DatabaseNeedsUpdate();

				if (_update) {
					DatabaseUpdateStatus status = du.PerformUpdates();
					lst = du.MergeMessages(lst, status.Messages);
				} else {
					DataInfo ver = DatabaseSchemaState.GetDbSchemaVersion();
					du.HandleResponse(lst, "Database up-to-date [" + ver.DataValue + "] ");
				}

				_update = du.DatabaseNeedsUpdate();

				if (!_update && DatabaseSchemaState.LastSQLError == null) {
					if (DatabaseSchemaState.UsersExist) {
						btnLogin.Visible = true;
					} else {
						btnCreate.Visible = true;
					}
				}
			}

			if (DatabaseSchemaState.LastSQLError != null) {
				du.HandleResponse(lst, DatabaseSchemaState.LastSQLError);
			}

			_ok = lst.Any() && (lst.Where(x => !string.IsNullOrWhiteSpace(x.ExceptionText)).Count() > 0) == false;

			GeneralUtilities.BindRepeater(rpMessages, lst.OrderBy(x => x.Order));

			using (var cmsHelper = new CMSConfigHelper()) {
				cmsHelper.ResetConfigs();
			}
		}

		protected string CSSMsg {
			get {
				return _ok ? " okMsg " : " errMsg ";
			}
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