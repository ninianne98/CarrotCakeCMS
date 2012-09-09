using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Core {

	public class CMSAdminModule {
		public CMSAdminModule() {
			PluginMenus = new List<CMSAdminModuleMenu>();
		}
		public Guid PluginID { get; set; }
		public string PluginName { get; set; }
		public List<CMSAdminModuleMenu> PluginMenus { get; set; }
	}

	public class CMSAdminModuleMenu {
		public Guid PluginID { get; set; }
		public int SortOrder { get; set; }
		public string Caption { get; set; }
		public string PluginParm { get; set; }
		public string ControlFile { get; set; }
		public bool UseAjax { get; set; }
		public bool UsePopup { get; set; }
		public bool IsVisible { get; set; }

	}

	public class CMSPlugin {
		public string FilePath { get; set; }
		public string Caption { get; set; }
	}

	public class CMSTemplate {
		public string TemplatePath { get; set; }
		public string Caption { get; set; }
		public string EncodedPath { get; set; }
	}

	public class DynamicSite {
		public Guid SiteID { get; set; }
		public string DomainName { get; set; }

	}
}
