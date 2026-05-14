using Carrotware.CMS.Core;
using System;

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

	public partial class Default : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.SiteDashboard);

			if (!IsPostBack) {
				CheckDatabase();
			}

			RedirectIfNoSite();

			CMSConfigHelper.CleanUpSerialData();

			phUserSecurity.Visible = SecurityData.IsAdmin;

			litPage.Text = string.Format(" ({0}) ", pageHelper.GetSitePageCount(SiteID, ContentPageType.PageType.ContentEntry));
			litPost.Text = string.Format(" ({0}) ", pageHelper.GetSitePageCount(SiteID, ContentPageType.PageType.BlogEntry));

			litCat.Text = string.Format(" ({0}) ", ContentCategory.GetSiteCount(SiteID));
			litTag.Text = string.Format(" ({0}) ", ContentTag.GetSiteCount(SiteID));

			litSmippet.Text = string.Format(" ({0}) ", pageHelper.GetSiteSnippetCount(SiteID));
		}
	}
}