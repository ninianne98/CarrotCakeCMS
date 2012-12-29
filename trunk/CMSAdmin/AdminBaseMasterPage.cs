using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin {
	public class AdminBaseMasterPage : BaseMasterPage {

		protected SiteData siteHelper = new SiteData();

		public enum SectionID {
			SiteInfo,
			SiteTemplate,
			ContentAdd,
			PageComment,
			ContentTemplate,
			ContentSkinEdit,
			ContentSiteMap,
			SiteExport,
			DataImport,
			SiteImport,
			UserAdmin,
			GroupAdmin,
			UserFn,
			Modules,
			BlogIndex,
			BlogContentAdd,
			BlogCategory,
			BlogTag,
			BlogTemplate,
			BlogComment
		}


		protected void LoadFooterCtrl(PlaceHolder plcHolder, string sCtrlKey) {
			string sControlPath = "";
			if (System.Configuration.ConfigurationManager.AppSettings[sCtrlKey] != null) {
				sControlPath = System.Configuration.ConfigurationManager.AppSettings[sCtrlKey].ToString();
			}
			if (!string.IsNullOrEmpty(sControlPath)) {
				if (File.Exists(Server.MapPath(sControlPath))) {
					Control ctrl = new Control();
					ctrl = Page.LoadControl(sControlPath);
					plcHolder.Controls.Add(ctrl);
				}
			}
		}

	}
}
