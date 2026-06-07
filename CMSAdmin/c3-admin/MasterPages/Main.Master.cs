using Carrotware.CMS.Core;
using System;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Admin.c3_admin.MasterPages {

	public partial class Main : AdminBaseMasterPage {

		public string UserName {
			get {
				return SecurityData.CurrentUserIdentityName ?? string.Empty;
			}
		}

		public string AntiCache {
			get {
				return Helper.AntiCache;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			siteSkin.SelectedColor = AdminBaseMasterPage.SiteSkin;

			litUsername.Text = string.Format("My Profile [{0}]", this.UserName);

			if (!SecurityData.IsAuthenticated) {
				SecurityData.ResetAuth();
				Response.Redirect(SiteFilename.LogonURL);
			}

			if (!SecurityData.IsAdmin) {
				tabUserSecurity.Visible = false;
			}

			tabUserAdmin.Visible = tabUserSecurity.Visible;
			tabGroupAdmin.Visible = tabUserSecurity.Visible;
			tabSites.Visible = tabUserSecurity.Visible;

			litSmallHead.Text = SiteData.CarrotCakeCMSVersionMM;

			if (!this.Page.Title.StartsWith(SiteData.CarrotCakeCMSVersionMM)) {
				this.Page.Title = string.Format("{0} - {1}", SiteData.CarrotCakeCMSVersionMM, this.Page.Title);
			}

			if (SiteData.CurrentSiteExists) {
				litServerTime.Text = string.Format("{0} {1}", SiteData.CurrentSite.Now, SiteData.CurrentSite.TimeZoneIdentifier);
				litSiteIdent.Text = SiteData.CurrentSite.SiteName;
				litTag.Text = SiteData.CurrentSite.SiteTagline;

				litSmallHead.Text = string.Format("{0} | {1}", SiteData.CarrotCakeCMSVersionMM, SiteData.CurrentSite.SiteName);

				if (!string.IsNullOrEmpty(SiteData.CurrentSite.SiteName) && !string.IsNullOrEmpty(SiteData.CurrentSite.SiteTagline)) {
					litSiteIdent.Text = string.Format("{0}: ", SiteData.CurrentSite.SiteName.Trim());
					litSmallHead.Text = string.Format("{0} | {1}: {2}", SiteData.CarrotCakeCMSVersionMM, SiteData.CurrentSite.SiteName, SiteData.CurrentSite.SiteTagline);
				}
			} else {
				litServerTime.Text = string.Format("{0} UTC", DateTime.UtcNow);
			}

			LoadFooterCtrl(plcFooter, ControlLocation.MainFooter);

			litCMSBuildInfo.Text = SiteData.CarrotCakeCMSVersion;
			litVersion.Text = SiteData.CarrotCakeCMSVersionMM;

			HideWhenNoSiteProfileExists();
		}

		public void HideWhenNoSiteProfileExists() {
			bool bShowTop = SiteData.CurrentSiteExists;

			tabContentTop.Visible = bShowTop;
			tabExportSite.Visible = bShowTop;
			tabTxtWidgets.Visible = bShowTop;
			tabSnippets.Visible = bShowTop;
			tabBlogTop.Visible = bShowTop;
			tabContent.Visible = bShowTop;
			tabModules.Visible = bShowTop;
			tabMainTemplate.Visible = bShowTop;
			tabContentSiteMap.Visible = bShowTop;
			tabImportContent.Visible = bShowTop;
			tabStatusChange.Visible = bShowTop;
			//tabDashboard.Visible = bShowTop;
			tabExtensions.Visible = bShowTop;
			tabHistory.Visible = bShowTop;
		}

		protected void btnLogout_Click(object sender, EventArgs e) {
			SecurityData.ResetAuth();

			Response.Redirect(SiteFilename.LogonURL + "?carrot_cache=" + DateTime.UtcNow.Ticks.ToString());
		}

		public void ActivateTab(SectionID sectionID) {
			string cssTop = "current sub";
			string cssSecondary = "current";

			switch (sectionID) {
				case SectionID.SiteDashboard:
					tabMainTop.Attributes["class"] = cssTop;
					//tabDashboard.Attributes["class"] = cssSecondary;
					break;

				case SectionID.SiteInfo:
					tabMainTop.Attributes["class"] = cssTop;
					tabMain.Attributes["class"] = cssSecondary;
					break;

				case SectionID.ContentHistory:
					tabMainTop.Attributes["class"] = cssTop;
					tabHistory.Attributes["class"] = cssSecondary;
					break;

				case SectionID.SiteTemplate:
					tabMainTop.Attributes["class"] = cssTop;
					tabMainTemplate.Attributes["class"] = cssSecondary;
					break;

				case SectionID.ContentSkinEdit:
					tabMainTop.Attributes["class"] = cssTop;
					tabContentSkin.Attributes["class"] = cssSecondary;
					break;

				case SectionID.DataImport:
					tabMainTop.Attributes["class"] = cssTop;
					tabImportContent.Attributes["class"] = cssSecondary;
					break;

				case SectionID.SiteExport:
					tabMainTop.Attributes["class"] = cssTop;
					tabExportSite.Attributes["class"] = cssSecondary;
					break;

				case SectionID.StatusChange:
					tabMainTop.Attributes["class"] = cssTop;
					tabStatusChange.Attributes["class"] = cssSecondary;
					break;

				case SectionID.ContentIndex:
					tabContentTop.Attributes["class"] = cssTop;
					tabContent.Attributes["class"] = cssSecondary;
					break;

				case SectionID.ContentAdd:
					tabContentTop.Attributes["class"] = cssTop;
					tabAddContent.Attributes["class"] = cssSecondary;
					break;

				case SectionID.ContentTemplate:
					tabContentTop.Attributes["class"] = cssTop;
					tabContentTemplate.Attributes["class"] = cssSecondary;
					break;

				case SectionID.PageComment:
					tabContentTop.Attributes["class"] = cssTop;
					tabContentCommentIndex.Attributes["class"] = cssSecondary;
					break;

				case SectionID.ContentSiteMap:
					tabContentTop.Attributes["class"] = cssTop;
					tabContentSiteMap.Attributes["class"] = cssSecondary;
					break;

				case SectionID.Modules:
					tabExtensions.Attributes["class"] = cssTop;
					tabModules.Attributes["class"] = cssSecondary;
					break;

				case SectionID.TextWidget:
					tabExtensions.Attributes["class"] = cssTop;
					tabTxtWidgets.Attributes["class"] = cssSecondary;
					break;

				case SectionID.ContentSnippet:
					tabExtensions.Attributes["class"] = cssTop;
					tabSnippets.Attributes["class"] = cssSecondary;
					break;

				case SectionID.UserAdmin:
					tabUserSecurity.Attributes["class"] = cssTop;
					tabUserAdmin.Attributes["class"] = cssSecondary;
					break;

				case SectionID.GroupAdmin:
					tabUserSecurity.Attributes["class"] = cssTop;
					tabGroupAdmin.Attributes["class"] = cssSecondary;
					break;

				case SectionID.SiteIndex:
					tabUserSecurity.Attributes["class"] = cssTop;
					tabSites.Attributes["class"] = cssSecondary;
					break;

				case SectionID.BlogContentAdd:
					tabBlogTop.Attributes["class"] = cssTop;
					tabAddBlogContent.Attributes["class"] = cssSecondary;
					break;

				case SectionID.BlogIndex:
					tabBlogTop.Attributes["class"] = cssTop;
					tabBlogContent.Attributes["class"] = cssSecondary;
					break;

				case SectionID.BlogCategory:
					tabBlogTop.Attributes["class"] = cssTop;
					tabBlogCategoryIndex.Attributes["class"] = cssSecondary;
					break;

				case SectionID.BlogTag:
					tabBlogTop.Attributes["class"] = cssTop;
					tabBlogTagIndex.Attributes["class"] = cssSecondary;
					break;

				case SectionID.BlogTemplate:
					tabBlogTop.Attributes["class"] = cssTop;
					tabBlogTemplate.Attributes["class"] = cssSecondary;
					break;

				case SectionID.BlogComment:
					tabBlogTop.Attributes["class"] = cssTop;
					tabBlogCommentIndex.Attributes["class"] = cssSecondary;
					break;
			}
		}

		protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e) {
			string sError = string.Empty;

			if (e.Exception != null) {
				Exception objErr = e.Exception;
				sError = objErr.Message;
				if (objErr.StackTrace != null) {
					sError += "\r\n<hr />\r\n" + objErr.StackTrace;
				}

				if (objErr.InnerException != null) {
					sError += "\r\n<hr />\r\n" + objErr.InnerException;
				}

				SiteData.WriteDebugException("main master - AsyncPostBackError", objErr);
			} else {
				sError = " An error occurred. (Generic Main) ";
			}

			ScriptManager1.AsyncPostBackErrorMessage = sError;
		}
	}
}