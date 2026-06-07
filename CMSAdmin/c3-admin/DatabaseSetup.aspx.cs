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

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["signout"])) {
				SecurityData.ResetAuth();
				Response.Redirect(SiteFilename.DatabaseSetupURL);
			}

			var du = new DatabaseUpdate();
			var lst = new List<DatabaseUpdateMessage>();

			btnLogin.Visible = false;
			btnCreate.Visible = false;

			btnCreate.Visible = du.DoUsersExist() == false;

			if (DatabaseSchemaState.LastSQLError != null) {
				du.HandleResponse(lst, DatabaseSchemaState.LastSQLError);
				du.ClearTest();
			} else {
				du.ClearTest();
				var update = du.DatabaseNeedsUpdate();

				DatabaseUpdateStatus status = du.PerformUpdates();
				lst = du.MergeMessages(lst, status.Messages);

				update = du.DatabaseNeedsUpdate();
			}

			btnLogin.Visible = btnCreate.Visible == false;

			if (DatabaseSchemaState.LastSQLError != null) {
				du.HandleResponse(lst, DatabaseSchemaState.LastSQLError);
			}

			_ok = lst.Any() && (lst.Where(x => !string.IsNullOrWhiteSpace(x.ExceptionText)).Count() > 0) == false;

			GeneralUtilities.BindRepeater(rpMessages, lst.OrderBy(x => x.Order));

			using (var cmsHelper = new CMSConfigHelper()) {
				cmsHelper.ResetConfigs();
			}

			lnkRun1.HRef = SiteData.CurrentScriptName;
			lnkRun2.HRef = SiteData.CurrentScriptName + "?signout=true&carrot_tick=" + DateTime.UtcNow.Ticks.ToString();
		}

		protected string CSSMsg {
			get {
				return _ok ? " msg-ok " : " msg-err ";
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