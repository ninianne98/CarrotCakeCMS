using System;
using Carrotware.CMS.Core;
using Carrotware.CMS.DBUpdater;

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

	public partial class Default : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.SiteDashboard);

			if (!IsPostBack) {
				if (DatabaseUpdate.AreCMSTablesIncomplete()) {
					Response.Redirect(SiteFilename.DatabaseSetupURL);
				}
			}

			RedirectIfNoSite();

			CMSConfigHelper.CleanUpSerialData();

			phUserSecurity.Visible = SecurityData.IsAdmin;

			litPage.Text = String.Format(" ({0}) ", pageHelper.GetSitePageCount(SiteID, ContentPageType.PageType.ContentEntry));
			litPost.Text = String.Format(" ({0}) ", pageHelper.GetSitePageCount(SiteID, ContentPageType.PageType.BlogEntry));

			litCat.Text = String.Format(" ({0}) ", ContentCategory.GetSiteCount(SiteID));
			litTag.Text = String.Format(" ({0}) ", ContentTag.GetSiteCount(SiteID));

			litSmippet.Text = String.Format(" ({0}) ", pageHelper.GetSiteSnippetCount(SiteID));
		}
	}
}