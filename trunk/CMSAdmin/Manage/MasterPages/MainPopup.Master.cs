using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.Web.UI.Controls;

namespace Carrotware.CMS.UI.Admin.MasterPages {
	public partial class MainPopup : AdminBaseMasterPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				pnlDirty.Visible = false; 
			} else {
				pnlDirty.Visible = true;
			}

			LoadFooterCtrl(plcFooter, "Carrotware.CMS.UI.Admin.MasterPages.MainPopup.Ctrl");
		}

		// so that it is easy to toggle master pages
		public void ActivateTab(SectionID sectionID) {
		}


	}
}
