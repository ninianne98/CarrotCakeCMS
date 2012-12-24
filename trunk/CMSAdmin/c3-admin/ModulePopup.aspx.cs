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
	public partial class ModulePopup : AdminBasePage {



		protected void Page_Load(object sender, EventArgs e) {

		
		}

		protected override void OnInit(EventArgs e) {
			ucAdminModule c = (ucAdminModule)Page.LoadControl(SiteData.AdminFolderPath + "ucAdminModule.ascx");
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
