using Carrotware.CMS.Core;
using System;

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

	public partial class SiteMapPop : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.ContentSiteMap);

			Master.UsesSaved = true;

			if (IsPostBack) {
				Master.ShowSave();
			} else {
				if (!SiteData.RefererScriptName.ToLowerInvariant().EndsWith(SiteData.CurrentScriptName.ToLowerInvariant())) {
					Master.HideSave();
				} else {
					Master.ShowSave();
				}
			}
		}
	}
}