using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.Web.UI.Controls;

namespace Carrotware.CMS.UI.Admin.MasterPages {
	public partial class Public : AdminBaseMasterPage {
		protected void Page_Load(object sender, EventArgs e) {

			LoadFooterCtrl(plcFooter, "Carrotware.CMS.UI.Admin.MasterPages.Public.Ctrl");

		}
	}
}
