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
* http://www.carrotware.com/
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
			try {
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

					if (site == null) {
						btnSave.Text = "Click to Create Site";
					}
				}

				siteHelper.CleanUpSerialData();

			} catch (Exception ex) {
				if (DatabaseUpdate.SystemNeedsChecking(ex)) {
					Response.Redirect("./DatabaseSetup.aspx");
				}
				//if the error is not the kind DatabaseUpdate recomends checking the database for, make sure the error is sent back to the user
				throw;
			}
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
				site.MetaKeyword = txtKey.Text;
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
