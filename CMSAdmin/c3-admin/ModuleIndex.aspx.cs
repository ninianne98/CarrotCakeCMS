using Carrotware.CMS.Core;
using System;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
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