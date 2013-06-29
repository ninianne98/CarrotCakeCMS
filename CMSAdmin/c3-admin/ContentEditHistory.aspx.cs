using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
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
	public partial class ContentEditHistory : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.ContentHistory);

			if (!IsPostBack) {
				txtDate.Text = SiteData.CurrentSite.Now.ToShortDateString();
				ddlSize.SelectedValue = pagedDataGrid.PageSize.ToString();

				GeneralUtilities.BindListDefaultText(ddlUsers, ExtendedUserData.GetUserList(), null, "select user", Guid.Empty.ToString());
			}

			LoadGrid();
		}

		protected void btnChangePage_Click(object sender, EventArgs e) {
			LoadGridClick();
		}

		protected void btnFilter_Click(object sender, EventArgs e) {
			LoadGridClick();
		}

		private void LoadGridClick() {
			pagedDataGrid.PageNumber = 1;
			LoadGrid();
		}


		protected void LoadGrid() {

			int iRecCount = -1;
			List<EditHistory> lstHistory = null;

			DateTime? dtFilter = null;
			if (!string.IsNullOrEmpty(txtDate.Text)) {
				dtFilter = Convert.ToDateTime(txtDate.Text);
			}

			Guid? userID = null;
			if (ddlUsers.SelectedIndex > 0) {
				userID = new Guid(ddlUsers.SelectedValue);
			}

			iRecCount = EditHistory.GetHistoryListCount(SiteData.CurrentSiteID, chkLatest.Checked, dtFilter, userID);

			pagedDataGrid.PageSize = int.Parse(ddlSize.SelectedValue);

			lblPages.Text = iRecCount.ToString();

			pagedDataGrid.BuildSorting();

			pagedDataGrid.TotalRecords = iRecCount;

			string sSort = pagedDataGrid.SortingBy;
			int iPgNbr = pagedDataGrid.PageNumber - 1;
			int iPageSize = pagedDataGrid.PageSize;

			lstHistory = EditHistory.GetHistoryList(sSort, iPgNbr, iPageSize, SiteData.CurrentSiteID, chkLatest.Checked, dtFilter, userID);

			GeneralUtilities.BindDataBoundControl(pagedDataGrid, lstHistory);

		}

	}
}