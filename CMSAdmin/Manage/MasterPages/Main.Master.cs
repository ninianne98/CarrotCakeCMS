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
			if (!SiteData.IsAdmin) {
				tabUserAdmin.Visible = false;
			}

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
				tabContent.Visible = false;
				tabModules.Visible = false;
			} else {
				tabContent.Visible = true;
				tabModules.Visible = true;
			}
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
					tabContent.Attributes["class"] = "current";
					break;
				case SectionID.UserAdmin:
					tabUserAdmin.Attributes["class"] = "current";
					break;
				case SectionID.Modules:
					tabModules.Attributes["class"] = "current";
					break;

			}
		}
	}
}
