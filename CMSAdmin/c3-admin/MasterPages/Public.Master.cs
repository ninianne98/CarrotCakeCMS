using System;
using System.Collections.Generic;
using System.Linq;
using Carrotware.CMS.Core;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin.MasterPages {

	public partial class Public : AdminBaseMasterPage {

		protected void Page_Load(object sender, EventArgs e) {
			LoadFooterCtrl(plcFooter, ControlLocation.PublicFooter);

			litCMSBuildInfo.Text = SiteData.CarrotCakeCMSVersion;

			phLogin.Visible = !SecurityData.IsAuthenticated;

#if DEBUG
			metaCrawl.Visible = false;
#endif
		}
	}
}