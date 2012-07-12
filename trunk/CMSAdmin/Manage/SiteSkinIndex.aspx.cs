using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class SiteSkinIndex : AdminBasePage {
		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.Content);
			
			//get the detected templates in use and mask off the template that is baked in as a the default template.
			gvPages.DataSource = cmsHelper.Templates.Where(x => x.TemplatePath.ToLower() != "/manage/plaintemplate.aspx").ToList();
			gvPages.DataBind();

		}
	}
}