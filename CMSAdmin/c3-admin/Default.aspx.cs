using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.DBUpdater;
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
	public partial class Default : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.SiteDashboard);

			RedirectIfNoSite();

			CMSConfigHelper.CleanUpSerialData();

			phUserSecurity.Visible = SecurityData.IsAdmin;

			litPage.Text = String.Format(" ({0}) ", pageHelper.GetSitePageCount(SiteID, ContentPageType.PageType.ContentEntry));
			litPost.Text = String.Format(" ({0}) ", pageHelper.GetSitePageCount(SiteID, ContentPageType.PageType.BlogEntry));


			litCat.Text = String.Format(" ({0}) ", ContentCategory.GetSiteCount(SiteID));
			litTag.Text = String.Format(" ({0}) ", ContentTag.GetSiteCount(SiteID));
		}


	}
}