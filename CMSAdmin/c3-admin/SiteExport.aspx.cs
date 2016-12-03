using System;
using Carrotware.CMS.Core;

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

	public partial class SiteExportPage : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.SiteExport);

			if (!IsPostBack) {
				txtBegin.Text = SiteData.CurrentSite.Now.AddMonths(-6).ToString(Helper.ShortDatePattern);
				txtEnd.Text = SiteData.CurrentSite.Now.AddDays(5).ToString(Helper.ShortDatePattern);
			}
		}
	}
}