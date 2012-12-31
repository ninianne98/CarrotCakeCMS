using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class ModuleIndex : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.Modules);

		}


		protected override void OnInit(EventArgs e) {
			ucAdminModule loadedModule = (ucAdminModule)Page.LoadControl(SiteFilename.AdminModuleControlPath);
			loadedModule.HideList = false;
			loadedModule.LoadModule();

			if (loadedModule.UseAjax) {
				phAjax.Controls.Add(loadedModule);
			} else {
				phNoAjax.Controls.Add(loadedModule);
			}

			if (loadedModule != null && loadedModule.ModuleFamily != null && loadedModule.PluginItem != null) {
				this.Title = "Module: " + loadedModule.ModuleFamily.PluginName + " - " + loadedModule.PluginItem.Caption;
			}

			base.OnInit(e);
		}

	}
}
