using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Carrotware.CMS.UI.Base;


namespace Carrotware.CMS.UI.Admin.MasterPages {
	public partial class MainPopup : AdminBaseMasterPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				pnlDirty.Visible = false; 
			} else {
				pnlDirty.Visible = true;
			}
		}

		// so that it is easy to toggle master pages
		public void ActivateTab(SectionID sectionID) {
		}


	}
}
