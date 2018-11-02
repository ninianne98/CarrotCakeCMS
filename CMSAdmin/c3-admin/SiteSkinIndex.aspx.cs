using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using System;
using System.Linq;

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

	public partial class SiteSkinIndex : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.ContentSkinEdit);

			//get the detected templates in use and mask off the template that is baked in as a the default template.
			GeneralUtilities.BindDataBoundControl(gvPages, cmsHelper.Templates.Where(x => x.TemplatePath.ToLowerInvariant() != SiteData.DefaultTemplateFilename.ToLowerInvariant()).ToList());
		}
	}
}