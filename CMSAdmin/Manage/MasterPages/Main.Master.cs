using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.Web.UI.Controls;

namespace Carrotware.CMS.UI.Admin.MasterPages {
	public partial class Main : AdminBaseMasterPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsAdmin) {
				tabUserAdmin.Visible = false;
			}

			string sPlugCfg = Server.MapPath("~/AdminModules.config");
			if (!File.Exists(sPlugCfg)) {
				tabModules.Visible = false;
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
				case SectionID.UserFn:
					tabUser.Attributes["class"] = "current";
					break;
				case SectionID.Modules:
					tabModules.Attributes["class"] = "current";
					break;

			}
		}
	}
}
