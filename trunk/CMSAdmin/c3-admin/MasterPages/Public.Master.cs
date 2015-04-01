using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.Web.UI.Controls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Admin.c3_admin.MasterPages {
	public partial class Public : AdminBaseMasterPage {
		protected void Page_Load(object sender, EventArgs e) {

			LoadFooterCtrl(plcFooter, ControlLocation.PublicFooter);

			litCMSBuildInfo.Text = SiteData.CarrotCakeCMSVersion;

#if DEBUG
			metaCrawl.Visible = false;
#endif

		}

	}
}
