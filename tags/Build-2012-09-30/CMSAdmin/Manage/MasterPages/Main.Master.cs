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

namespace Carrotware.CMS.UI.Admin.MasterPages {
	public partial class Main : AdminBaseMasterPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (!SecurityData.IsAdmin) {
				tabUserSecurity.Visible = false;
			}
			tabUserAdmin.Visible = tabUserSecurity.Visible;
			tabGroupAdmin.Visible = tabUserSecurity.Visible;

			LoadFooterCtrl(plcFooter, "Carrotware.CMS.UI.Admin.MasterPages.Main.Ctrl");

			litCMSBuildInfo.Text = string.Format("CarrotCake CMS {0}", CurrentDLLVersion);

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

			tabContent.Visible = tabContentTop.Visible;
			tabModules.Visible = tabContentTop.Visible;
		}


		protected void lnkLogout_Click(object sender, EventArgs e) {
			FormsAuthentication.SignOut();
			Response.Redirect("./Logon.aspx");
		}


		public void ActivateTab(SectionID sectionID) {

			switch (sectionID) {
				case SectionID.Home:
					tabMain.Attributes["class"] = "current";
					break;

				case SectionID.Content:
					tabContentTop.Attributes["class"] = "current";
					tabContent.Attributes["class"] = "current";
					break;
				case SectionID.ContentTemplate:
					tabContentTop.Attributes["class"] = "current";
					tabContentTemplate.Attributes["class"] = "current";
					break;
				case SectionID.ContentSkinEdit:
					tabContentTop.Attributes["class"] = "current";
					tabContentSkin.Attributes["class"] = "current";
					break;
				case SectionID.ContentSiteMap:
					tabContentTop.Attributes["class"] = "current";
					tabContentSiteMap.Attributes["class"] = "current";
					break;
				case SectionID.UserAdmin:
					tabUserSecurity.Attributes["class"] = "current";
					tabUserAdmin.Attributes["class"] = "current";
					break;
				case SectionID.GroupAdmin:
					tabUserSecurity.Attributes["class"] = "current";
					tabGroupAdmin.Attributes["class"] = "current";
					break;
				case SectionID.Modules:
					tabModules.Attributes["class"] = "current";
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
