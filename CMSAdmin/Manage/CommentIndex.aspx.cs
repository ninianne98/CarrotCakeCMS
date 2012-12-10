using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
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


namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class CommentIndex : AdminBasePage {
		private int PageNumber = 1;
		private string sBtnName = "lnkPagerBtn";
		private bool bHeadClicked = true;
		private ContentPageType.PageType pageType = ContentPageType.PageType.BlogEntry;


		protected void Page_Load(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["type"])) {
				pageType = ContentPageType.GetTypeByName(Request.QueryString["type"].ToString());
			}

			if (pageType == ContentPageType.PageType.ContentEntry) {
				Master.ActivateTab(AdminBaseMasterPage.SectionID.PageComment);
			} else {
				Master.ActivateTab(AdminBaseMasterPage.SectionID.BlogComment);
			}


			if (!IsPostBack) {
				bHeadClicked = false;
				hdnPageNbr.Value = "1";
				BindData();
			} else {
				if (Request.Form["__EVENTARGUMENT"] != null) {
					string arg = Request.Form["__EVENTARGUMENT"].ToString();
					string tgt = Request.Form["__EVENTTARGET"].ToString();

					if (tgt.Contains("$lnkHead") && tgt.Contains("$" + gvPages.ID + "$")) {
						bHeadClicked = true;
					}

					if (tgt.Contains("$" + sBtnName) && tgt.Contains("$" + rpDataPager.ID + "$")) {
						string[] parms = tgt.Split('$');
						int pg = int.Parse(parms[parms.Length - 1].Replace(sBtnName, ""));
						PageNumber = pg;
						hdnPageNbr.Value = PageNumber.ToString();
						bHeadClicked = false;
					}
				}
			}

			if (PageNumber <= 1 && !string.IsNullOrEmpty(hdnPageNbr.Value)) {
				PageNumber = int.Parse(hdnPageNbr.Value);
			}

			if (IsPostBack) {
				BindData();
			}

		}


		protected void BindData() {
			int TotalPages = 0;
			int PageSize = int.Parse(hdnPageSize.Value);

			int TotalRecords = PostComment.GetCommentCountBySiteAndType(SiteData.CurrentSiteID, pageType);

			int iPageNbr = PageNumber - 1;

			TotalPages = TotalRecords / PageSize;

			if ((TotalRecords % PageSize) > 0) {
				TotalPages++;
			}

			if (TotalPages > 1) {
				List<int> pagelist = new List<int>();
				pagelist = Enumerable.Range(1, TotalPages).ToList();

				rpDataPager.DataSource = pagelist;
				rpDataPager.DataBind();
			}

			string sSort = gvPages.CurrentSort;
			if (bHeadClicked) {
				sSort = gvPages.PredictNewSort;
			}

			List<PostComment> comments = PostComment.GetCommentsBySitePageNumber(SiteData.CurrentSiteID, iPageNbr, PageSize, sSort, pageType);

			gvPages.DataSource = comments;
			gvPages.DataBind();

			WalkCtrlsForAssignment(rpDataPager);
		}

		private void WalkCtrlsForAssignment(Control X) {
			foreach (Control c in X.Controls) {
				if (c is IActivatePageNavItem) {
					IActivatePageNavItem btn = (IActivatePageNavItem)c;
					if (btn.PageNumber == PageNumber) {
						btn.IsSelected = true;
					}
					WalkCtrlsForAssignment(c);
				} else {
					WalkCtrlsForAssignment(c);
				}
			}
		}
	}
}