﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
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
	public partial class SiteSkinIndex : AdminBasePage {
		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.ContentSkinEdit);

			//get the detected templates in use and mask off the template that is baked in as a the default template.
			GeneralUtilities.BindDataBoundControl(gvPages, cmsHelper.Templates.Where(x => x.TemplatePath.ToLower() != SiteData.DefaultTemplateFilename.ToLower()).ToList());

		}
	}
}