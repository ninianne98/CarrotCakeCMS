﻿using Carrotware.CMS.Core;
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

	public partial class PageIndex : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.ContentIndex);

			RedirectIfNoSite();

			CMSConfigHelper.CleanUpSerialData();

			if (!IsPostBack) {
				ddlSize.SelectedValue = pagedDataGrid.PageSize.ToString();
			}

			LoadGrid();
		}

		protected void SetGrid(bool bAll, Guid? guidParentID) {
			List<ContentPage> lstContent = null;
			int iRecCount = -1;

			if (bAll) {
				pnlPager.Visible = true;
				iRecCount = pageHelper.GetSitePageCount(SiteData.CurrentSiteID, ContentPageType.PageType.ContentEntry, false);
				pagedDataGrid.PageSize = int.Parse(ddlSize.SelectedValue);
			} else {
				pnlPager.Visible = false;
				if (guidParentID == null) {
					lstContent = pageHelper.GetTopNavigation(SiteID, false);
				} else {
					lstContent = pageHelper.GetParentWithChildNavigation(SiteID, guidParentID, false);
				}
				iRecCount = lstContent.Count();
				pagedDataGrid.PageSize = iRecCount + 10;
			}

			lblPages.Text = string.Format(" {0} ", iRecCount);

			pagedDataGrid.BuildSorting();

			pagedDataGrid.TotalRecords = iRecCount;
			string sSort = pagedDataGrid.SortingBy;
			int iPgNbr = pagedDataGrid.PageNumber - 1;
			int iPageSize = pagedDataGrid.PageSize;

			if (bAll) {
				lstContent = pageHelper.GetPagedSortedContent(SiteID, ContentPageType.PageType.ContentEntry, false, iPageSize, iPgNbr, sSort);
			}

			GeneralUtilities.BindDataBoundControl(pagedDataGrid, lstContent);
		}

		protected void btnFilter_Click(object sender, EventArgs e) {
			LoadGridClick();
		}

		protected void rdoFilterResults_CheckedChanged(object sender, EventArgs e) {
			LoadGridClick();
		}

		protected void btnChangePage_Click(object sender, EventArgs e) {
			LoadGridClick();
		}

		private void LoadGridClick() {
			pagedDataGrid.PageNumber = 1;
			LoadGrid();
		}

		private void LoadGrid() {
			trFilter.Attributes["style"] = "display:none;";

			if (rdoFilterResults2.Checked) {
				trFilter.Attributes["style"] = "display:none;";
				SetGrid(true, Guid.Empty);
			} else {
				trFilter.Attributes["style"] = "";
				SetGrid(false, ParentPagePicker.SelectedPage);
			}

			//SetGrid(rdoFilterResults2.Checked, ParentPagePicker.SelectedPage);
		}
	}
}