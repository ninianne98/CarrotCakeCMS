using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/
namespace Carrotware.CMS.UI.Admin {

	public partial class Default : AdminBasePage {


		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.Home);

			litID.Text = SiteData.CurrentSiteID.ToString();

			if (!IsPostBack) {
				SiteData site = siteHelper.GetCurrentSite();
				txtURL.Text = "http://" + Request.ServerVariables["SERVER_NAME"];
				txtSiteName.Text = Request.ServerVariables["SERVER_NAME"];

				if (site != null) {
					txtSiteName.Text = site.SiteName;
					txtURL.Text = site.MainURL;
					txtKey.Text = site.MetaKeyword;
					txtDescription.Text = site.MetaDescription;
					chkHide.Checked = site.BlockIndex;
				}
			}

			siteHelper.CleanUpSerialData();
		}

		protected void btnSave_Click(object sender, EventArgs e) {

			SiteData site = siteHelper.GetCurrentSite();

			if (site == null) {
				site = new SiteData();
				site.SiteID = SiteID;
			}

			if (site != null) {
				site.SiteName = txtSiteName.Text;
				site.MainURL = txtURL.Text;
				site.MetaKeyword = txtKey.Text.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");
				site.MetaDescription = txtDescription.Text;
				site.BlockIndex = chkHide.Checked;
			}

			site.Save();

			Response.Redirect(SiteData.CurrentScriptName);
		}

		protected void btnResetVars_Click(object sender, EventArgs e) {
			CMSConfigHelper cmsHelper = new CMSConfigHelper();
			cmsHelper.ResetConfigs();
		}

	}
}
