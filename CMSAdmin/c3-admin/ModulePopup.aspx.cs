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

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class ModulePopup : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
		}

		protected override void OnInit(EventArgs e) {
			ucAdminModule c = (ucAdminModule)Page.LoadControl(SiteFilename.AdminModuleControlPath);
			c.HideList = true;
			c.LoadModule();

			if (c.UseAjax) {
				phAjax.Controls.Add(c);
			} else {
				phNoAjax.Controls.Add(c);
			}

			base.OnInit(e);
		}
	}
}