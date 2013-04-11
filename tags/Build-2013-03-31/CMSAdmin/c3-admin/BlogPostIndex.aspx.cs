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
	public partial class BlogPostIndex : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.BlogIndex);

			RedirectIfNoSite();

			CMSConfigHelper.CleanUpSerialData();

			if (!IsPostBack) {
				txtDate.Text = SiteData.CurrentSite.Now.ToShortDateString();
				ContentPage cp = pageHelper.GetLatestPosts(SiteData.CurrentSiteID, 2, false).FirstOrDefault();
				if (cp != null) {
					txtDate.Text = cp.GoLiveDate.ToShortDateString();
				}

				ddlSize.SelectedValue = pagedDataGrid.PageSize.ToString();
			}

			LoadGrid();
		}


		protected void SetGrid(bool bAll, DateTime dateRange, int dateRangeDays) {

			int iRecCount = -1;
			List<ContentPage> lstContent = null;

			if (bAll) {
				pnlPager.Visible = true;
				iRecCount = pageHelper.GetSitePageCount(SiteData.CurrentSiteID, ContentPageType.PageType.BlogEntry, false);
				pagedDataGrid.PageSize = int.Parse(ddlSize.SelectedValue);
			} else {
				pnlPager.Visible = false;
				lstContent = pageHelper.GetPostsByDateRange(SiteID, dateRange, dateRangeDays, false);
				iRecCount = lstContent.Count();
				pagedDataGrid.PageSize = iRecCount + 10;
			}

			lblPages.Text = iRecCount.ToString();

			pagedDataGrid.BuildSorting();

			pagedDataGrid.TotalRecords = iRecCount;
			string sSort = pagedDataGrid.SortingBy;
			int iPgNbr = pagedDataGrid.PageNumber - 1;
			int iPageSize = pagedDataGrid.PageSize;

			if (bAll) {
				lstContent = pageHelper.GetPagedSortedContent(SiteID, ContentPageType.PageType.BlogEntry, false, iPageSize, iPgNbr, sSort);
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
				SetGrid(true, SiteData.CurrentSite.Now, 0);
			} else {
				trFilter.Attributes["style"] = "";
				SetGrid(false, Convert.ToDateTime(txtDate.Text), int.Parse(ddlDateRange.SelectedValue));
			}
		}


	}
}
