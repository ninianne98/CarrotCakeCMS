﻿using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using System;
using System.Collections.ObjectModel;

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

	public partial class SiteInfo : AdminBasePage {
		private bool _isNewSite = true;

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.SiteInfo);

			_isNewSite = !SiteData.CurretSiteExists;

			litID.Text = SiteData.CurrentSiteID.ToString();

			if (!IsPostBack) {
				CheckDatabase();

				SiteData site = siteHelper.GetCurrentSite();

				if (site == null || _isNewSite) {
					site = SiteData.InitNewSite(SiteID);
				}

				phTrackback.Visible = _isNewSite ? false : (site.AcceptTrackbacks || site.SendTrackbacks);

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
					txtEditorPath.Text = site.Blog_EditorPath;
					txtDatePath.Text = site.Blog_DatePath;

					GeneralUtilities.SelectListValue(ddlTimeZone, site.TimeZoneIdentifier);

					GeneralUtilities.SelectListValue(ddlDatePattern, site.Blog_DatePattern);

					ParentPagePicker.SelectedPage = site.Blog_Root_ContentID;
				}

				phCreatePage.Visible = false;

				if (!SiteData.CurretSiteExists) {
					btnSave.Text = "Click to Create Site";
					phCreatePage.Visible = true;
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
				site.Blog_EditorPath = txtEditorPath.Text;
				site.Blog_DatePattern = ddlDatePattern.SelectedValue;
				site.Blog_Root_ContentID = ParentPagePicker.SelectedPage;
			}

			site.Save();

			if (sDatePatternOld != ddlDatePattern.SelectedValue || sTimezoneOld != ddlTimeZone.SelectedValue) {
				using (ContentPageHelper cph = new ContentPageHelper()) {
					cph.BulkBlogFileNameUpdateFromDate(SiteID);
				}
			}

			if (!_isNewSite) {
				Response.Redirect(SiteData.CurrentScriptName);
			} else {
				DateTime dtSite = CMSConfigHelper.CalcNearestFiveMinTime(SiteData.CurrentSite.Now);

				if (chkHomepage.Checked) {
					ContentPage pageContents = new ContentPage {
						SiteID = SiteID,
						Root_ContentID = Guid.NewGuid(),
						ContentID = Guid.NewGuid(),
						EditDate = SiteData.CurrentSite.Now,
						CreateUserId = SecurityData.CurrentUserGuid,
						CreateDate = SiteData.CurrentSite.Now,
						GoLiveDate = dtSite.AddMinutes(-5),
						RetireDate = dtSite.AddYears(200),
						TitleBar = "Home",
						NavMenuText = "Home",
						PageHead = "Home",
						FileName = "/home.aspx",
						PageText = SiteData.StarterHomePageSample,
						LeftPageText = string.Empty,
						RightPageText = string.Empty,
						NavOrder = 0,
						IsLatestVersion = true,
						PageActive = true,
						ShowInSiteNav = true,
						ShowInSiteMap = true,
						BlockIndex = false,
						EditUserId = SecurityData.CurrentUserGuid,
						ContentType = ContentPageType.PageType.ContentEntry,
						TemplateFile = SiteData.DefaultTemplateFilename
					};

					pageContents.SavePageEdit();
				}

				Response.Redirect(SiteFilename.DashboardURL);
			}
		}

		protected void btnResetVars_Click(object sender, EventArgs e) {
			using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
				cmsHelper.ResetConfigs();
			}
		}
	}
}