using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.Web.UI.Controls;

namespace Carrotware.CMS.UI.Admin.c3_admin.MasterPages {
	public partial class Main : AdminBaseMasterPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (!SecurityData.IsAdmin) {
				tabUserSecurity.Visible = false;
			}
			tabUserAdmin.Visible = tabUserSecurity.Visible;
			tabGroupAdmin.Visible = tabUserSecurity.Visible;

			if (SiteData.CurrentSite != null) {
				litServerTime.Text = SiteData.CurrentSite.Now.ToString() + " " + SiteData.CurrentSite.TimeZoneIdentifier;
			} else {
				litServerTime.Text = DateTime.UtcNow.ToString() + " UTC";
			}

			LoadFooterCtrl(plcFooter, ControlLocation.MainFooter);

			litCMSBuildInfo.Text = SiteData.CarrotCakeCMSVersion;
			litVersion.Text = SiteData.CarrotCakeCMSVersionMM;

			HideWhenNoSiteProfileExists();

			string sPlugCfg = Server.MapPath("~/AdminModules.config");
			if (!File.Exists(sPlugCfg)) {
				tabModules.Visible = false;
			}

		}

		public void HideWhenNoSiteProfileExists() {

			SiteData site = siteHelper.GetCurrentSite();

			if (site == null) {
				tabContentTop.Visible = false;
			} else {
				tabContentTop.Visible = true;
			}

			tabExportSite.Visible = tabContentTop.Visible;
			tabBlogTop.Visible = tabContentTop.Visible;
			tabContent.Visible = tabContentTop.Visible;
			tabModules.Visible = tabContentTop.Visible;
			tabMainTemplate.Visible = tabContentTop.Visible;
			tabContentSiteMap.Visible = tabContentTop.Visible;
			tabImportContent.Visible = tabContentTop.Visible;
			tabStatusChange.Visible = tabContentTop.Visible;
		}


		protected void lnkLogout_Click(object sender, EventArgs e) {
			FormsAuthentication.SignOut();
			Response.Redirect("./Logon.aspx");
		}


		public void ActivateTab(SectionID sectionID) {
			string sCSSTop = "current sub";
			string sCSSSecondary = "current";

			switch (sectionID) {
				case SectionID.SiteInfo:
					tabMainTop.Attributes["class"] = sCSSTop;
					tabMain.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.SiteTemplate:
					tabMainTop.Attributes["class"] = sCSSTop;
					tabMainTemplate.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.ContentSkinEdit:
					tabMainTop.Attributes["class"] = sCSSTop;
					tabContentSkin.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.DataImport:
					tabMainTop.Attributes["class"] = sCSSTop;
					tabImportContent.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.SiteExport:
					tabMainTop.Attributes["class"] = sCSSTop;
					tabExportSite.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.StatusChange:
					tabMainTop.Attributes["class"] = sCSSTop;
					tabStatusChange.Attributes["class"] = sCSSSecondary;
					break;

				case SectionID.ContentAdd:
					tabContentTop.Attributes["class"] = sCSSTop;
					tabContent.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.ContentTemplate:
					tabContentTop.Attributes["class"] = sCSSTop;
					tabContentTemplate.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.PageComment:
					tabContentTop.Attributes["class"] = sCSSTop;
					tabContentCommentIndex.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.ContentSiteMap:
					tabContentTop.Attributes["class"] = sCSSTop;
					tabContentSiteMap.Attributes["class"] = sCSSSecondary;
					break;

				case SectionID.UserAdmin:
					tabUserSecurity.Attributes["class"] = sCSSTop;
					tabUserAdmin.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.GroupAdmin:
					tabUserSecurity.Attributes["class"] = sCSSTop;
					tabGroupAdmin.Attributes["class"] = sCSSSecondary;
					break;

				case SectionID.Modules:
					tabModules.Attributes["class"] = sCSSSecondary;
					break;

				case SectionID.BlogContentAdd:
					tabBlogTop.Attributes["class"] = sCSSTop;
					tabBlogContent.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.BlogIndex:
					tabBlogTop.Attributes["class"] = sCSSTop;
					tabBlogContent.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.BlogCategory:
					tabBlogTop.Attributes["class"] = sCSSTop;
					tabBlogCategoryIndex.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.BlogTag:
					tabBlogTop.Attributes["class"] = sCSSTop;
					tabBlogTagIndex.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.BlogTemplate:
					tabBlogTop.Attributes["class"] = sCSSTop;
					tabBlogTemplate.Attributes["class"] = sCSSSecondary;
					break;
				case SectionID.BlogComment:
					tabBlogTop.Attributes["class"] = sCSSTop;
					tabBlogCommentIndex.Attributes["class"] = sCSSSecondary;
					break;
			}

		}

		protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e) {
			string sError = String.Empty;

			if (e.Exception != null) {
				Exception objErr = e.Exception;
				sError = objErr.Message;
				if (objErr.StackTrace != null) {
					sError += "\r\n<hr />\r\n" + objErr.StackTrace;
				}

				if (objErr.InnerException != null) {
					sError += "\r\n<hr />\r\n" + objErr.InnerException;
				}
			} else {
				sError = " An error occurred. ";
			}

			ScriptManager1.AsyncPostBackErrorMessage = sError;
		}

	}
}
