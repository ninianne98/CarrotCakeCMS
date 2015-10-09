using System;
using System.Collections.Generic;
using Carrotware.CMS.Core;
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

	public partial class ucCommentIndex : AdminBaseUserControl {
		private ContentPageType.PageType pageType = ContentPageType.PageType.BlogEntry;
		public Guid guidRootContentID = Guid.Empty;

		public string LinkingPage { get; set; }

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["type"])) {
				pageType = ContentPageType.GetTypeByName(Request.QueryString["type"].ToString());
			}

			if (!IsPostBack) {
				ddlSize.SelectedValue = pagedDataGrid.PageSize.ToString();

				GeneralUtilities.BindOptionalYesNoList(ddlActive);
				GeneralUtilities.BindOptionalYesNoList(ddlSpam);
			}

			guidRootContentID = GetGuidIDFromQuery();

			BindData();
		}

		protected void btnApply_Click(object sender, EventArgs e) {
		}

		protected void BindData() {
			int iRecCount = -1;

			bool? active = GeneralUtilities.GetNullableBoolValue(ddlActive);
			bool? spam = GeneralUtilities.GetNullableBoolValue(ddlSpam);
			pagedDataGrid.PageSize = int.Parse(ddlSize.SelectedValue);

			if (guidRootContentID == Guid.Empty) {
				iRecCount = PostComment.GetCommentCountBySiteAndType(SiteData.CurrentSiteID, pageType, active, spam);
			} else {
				iRecCount = PostComment.GetCommentCountByContent(guidRootContentID, active, spam);
			}

			lblPages.Text = String.Format(" {0} ", iRecCount);

			pagedDataGrid.BuildSorting();

			pagedDataGrid.TotalRecords = iRecCount;
			string sSort = pagedDataGrid.SortingBy;
			int iPgNbr = pagedDataGrid.PageNumber - 1;
			int iPageSize = pagedDataGrid.PageSize;

			List<PostComment> lstComments = new List<PostComment>();

			if (guidRootContentID == Guid.Empty) {
				lstComments = PostComment.GetCommentsBySitePageNumber(SiteData.CurrentSiteID, iPgNbr, iPageSize, sSort, pageType, active, spam);
			} else {
				lstComments = PostComment.GetCommentsByContentPageNumber(guidRootContentID, iPgNbr, iPageSize, sSort, active, spam);
			}

			GeneralUtilities.BindDataBoundControl(pagedDataGrid, lstComments);
		}
	}
}