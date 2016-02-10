using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Core {

	[Serializable()]
	public class CMSAdminModule {

		public CMSAdminModule() {
			this.PluginMenus = new List<CMSAdminModuleMenu>();
		}

		public Guid PluginID { get; set; }
		public string PluginName { get; set; }
		public List<CMSAdminModuleMenu> PluginMenus { get; set; }
	}

	[Serializable()]
	public class CMSAdminModuleMenu {

		public CMSAdminModuleMenu() { }

		public Guid PluginID { get; set; }
		public int SortOrder { get; set; }
		public string Caption { get; set; }
		public string PluginParm { get; set; }
		public string ControlFile { get; set; }
		public bool UseAjax { get; set; }
		public bool UsePopup { get; set; }
		public bool IsVisible { get; set; }
	}

	[Serializable()]
	public class CMSPlugin {

		public CMSPlugin() {
			this.SortOrder = 1000;
		}

		public int SortOrder { get; set; }
		public string FilePath { get; set; }
		public string Caption { get; set; }
	}

	[Serializable()]
	public class CMSTemplate {

		public CMSTemplate() { }

		public string TemplatePath { get; set; }
		public string Caption { get; set; }
		public string EncodedPath { get; set; }
	}

	[Serializable()]
	public class CMSTextWidget {

		public CMSTextWidget() { }

		public string AssemblyString { get; set; }
		public string DisplayName { get; set; }
	}

	[Serializable()]
	public class CMSTextWidgetPicker {

		public CMSTextWidgetPicker() { }

		public Guid TextWidgetPickerID { get; set; }
		public string AssemblyString { get; set; }
		public string DisplayName { get; set; }
		public bool ProcessBody { get; set; }
		public bool ProcessPlainText { get; set; }
		public bool ProcessHTMLText { get; set; }
		public bool ProcessComment { get; set; }
		public bool ProcessSnippet { get; set; }
	}

	[Serializable()]
	public class DynamicSite {

		public DynamicSite() { }

		public Guid SiteID { get; set; }
		public string DomainName { get; set; }
	}

	[Serializable()]
	public class CMSFilePath {

		public CMSFilePath() {
			this.DateChecked = DateTime.UtcNow;
			this.FileExists = false;
			this.SiteID = Guid.Empty;
			this.TemplateFile = null;
		}

		public CMSFilePath(string fileName)
			: this() {
			this.TemplateFile = fileName.ToLowerInvariant();
			this.FileExists = File.Exists(HttpContext.Current.Server.MapPath(this.TemplateFile));
		}

		public CMSFilePath(string fileName, Guid siteID)
			: this(fileName) {
			this.SiteID = siteID;
		}

		public DateTime DateChecked { get; set; }
		public string TemplateFile { get; set; }
		public bool FileExists { get; set; }
		public Guid SiteID { get; set; }

		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || this.GetType() != obj.GetType()) return false;
			if (obj is CMSFilePath) {
				CMSFilePath p = (CMSFilePath)obj;
				return (this.TemplateFile.ToLowerInvariant() == p.TemplateFile.ToLowerInvariant())
					&& (this.SiteID == p.SiteID);
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return this.TemplateFile.ToLowerInvariant().GetHashCode() ^ this.SiteID.GetHashCode();
		}
	}
}