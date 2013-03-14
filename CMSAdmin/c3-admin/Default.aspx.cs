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
			Master.ActivateTab(AdminBaseMasterPage.SectionID.SiteInfo);

			litID.Text = SiteData.CurrentSiteID.ToString();

			if (!IsPostBack) {

				if (DatabaseUpdate.AreCMSTablesIncomplete()) {
					Response.Redirect(SiteFilename.DatabaseSetupURL);
				}

				SiteData site = siteHelper.GetCurrentSite();
				txtURL.Text = "http://" + Request.ServerVariables["SERVER_NAME"];
				txtSiteName.Text = Request.ServerVariables["SERVER_NAME"];

				txtFolderPath.Text = "archive";
				txtCategoryPath.Text = "category";
				txtTagPath.Text = "tag";
				txtDatePath.Text = "date";

				ddlDatePattern.SelectedValue = "yyyy/MM/dd";
				txtTitleBar.Text = SiteData.DefaultPageTitlePattern;

				ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();

				GeneralUtilities.BindList(ddlTimeZone, timeZones, TimeZoneInfo.Local.Id);

				trSiteIndex.Visible = false;

				if (site != null) {

					if (site.GetSitePageCount(ContentPageType.PageType.ContentEntry) > 0) {
						trSiteIndex.Visible = true;
					}

					txtSiteName.Text = site.SiteName;
					txtTagline.Text = site.SiteTagline;
					txtTitleBar.Text = site.SiteTitlebarPattern;
					txtURL.Text = site.MainURL;
					txtKey.Text = site.MetaKeyword;
					txtDescription.Text = site.MetaDescription;

					chkHide.Checked = site.BlockIndex;
					chkSendTrackback.Checked = site.SendTrackbacks;
					chkAcceptTrackbacks.Checked = site.AcceptTrackbacks;

					txtFolderPath.Text = site.Blog_FolderPath;
					txtCategoryPath.Text = site.Blog_CategoryPath;
					txtTagPath.Text = site.Blog_TagPath;
					txtDatePath.Text = site.Blog_DatePath;

					GeneralUtilities.SelectListValue(ddlTimeZone, site.TimeZoneIdentifier);

					GeneralUtilities.SelectListValue(ddlDatePattern, site.Blog_DatePattern);

					ParentPagePicker.SelectedPage = site.Blog_Root_ContentID;
				}

				if (site == null) {
					btnSave.Text = "Click to Create Site";
				}
			}

			CMSConfigHelper.CleanUpSerialData();
		}

		protected void btnSave_Click(object sender, EventArgs e) {

			SiteData site = siteHelper.GetCurrentSite();
			string sDatePatternOld = "yy-MM-dd";
			string sTimezoneOld = "ZZZ";

			if (site == null) {
				site = new SiteData();
				site.SiteID = SiteID;
			}

			if (site != null) {
				sDatePatternOld = site.Blog_DatePattern;
				sTimezoneOld = site.TimeZoneIdentifier;

				site.SiteName = txtSiteName.Text;
				site.SiteTagline = txtTagline.Text;
				site.SiteTitlebarPattern = txtTitleBar.Text;
				site.MainURL = txtURL.Text;
				site.MetaKeyword = txtKey.Text;
				site.MetaDescription = txtDescription.Text;
				site.BlockIndex = chkHide.Checked;
				site.SendTrackbacks = chkSendTrackback.Checked;
				site.AcceptTrackbacks = chkAcceptTrackbacks.Checked;

				site.TimeZoneIdentifier = ddlTimeZone.SelectedValue;

				site.Blog_FolderPath = txtFolderPath.Text;
				site.Blog_CategoryPath = txtCategoryPath.Text;
				site.Blog_DatePath = txtDatePath.Text;
				site.Blog_TagPath = txtTagPath.Text;
				site.Blog_DatePattern = ddlDatePattern.SelectedValue;
				site.Blog_Root_ContentID = ParentPagePicker.SelectedPage;

			}

			site.Save();

			if (sDatePatternOld != ddlDatePattern.SelectedValue || sTimezoneOld != ddlTimeZone.SelectedValue) {
				using (ContentPageHelper cph = new ContentPageHelper()) {
					cph.BulkBlogFileNameUpdateFromDate(SiteID);
				}
			}

			Response.Redirect(SiteData.CurrentScriptName);
		}

		protected void btnResetVars_Click(object sender, EventArgs e) {
			using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
				cmsHelper.ResetConfigs();
			}
		}

	}
}
