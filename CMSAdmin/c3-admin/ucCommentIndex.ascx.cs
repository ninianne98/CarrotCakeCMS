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
	public partial class ucCommentIndex : BaseUserControl {
		private int PageNumber = 1;

		private ContentPageType.PageType pageType = ContentPageType.PageType.BlogEntry;
		public Guid guidRootContentID = Guid.Empty;

		public string LinkingPage { get; set; }

		protected void Page_Load(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["type"])) {
				pageType = ContentPageType.GetTypeByName(Request.QueryString["type"].ToString());
			}
			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				guidRootContentID = new Guid(Request.QueryString["id"].ToString());
			}

			BindData();
		}


		protected void BindData() {
			int iRecCount = -1;

			if (guidRootContentID == Guid.Empty) {
				iRecCount = PostComment.GetCommentCountBySiteAndType(SiteData.CurrentSiteID, pageType);
			} else {
				iRecCount = PostComment.GetCommentCountByContent(guidRootContentID, false);
			}

			pagedDataGrid.BuildSorting();

			pagedDataGrid.TotalRecords = iRecCount;
			string sSort = pagedDataGrid.SortingBy;
			int iPgNbr = pagedDataGrid.PageNumber - 1;
			int iPageSize = pagedDataGrid.PageSize;

			List<PostComment> comments = new List<PostComment>();

			if (guidRootContentID == Guid.Empty) {
				comments = PostComment.GetCommentsBySitePageNumber(SiteData.CurrentSiteID, iPgNbr, iPageSize, sSort, pageType);
			} else {
				comments = PostComment.GetCommentsByContentPageNumber(guidRootContentID, iPgNbr, iPageSize, sSort, false);
			}

			pagedDataGrid.DataSource = comments;
			pagedDataGrid.DataBind();

		}

	}
}