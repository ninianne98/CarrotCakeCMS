using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Web;
using System.Web.Configuration;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.Core {

	[AspNetHostingPermissionAttribute(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermissionAttribute(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CarrotCakeSectionGroup : ConfigurationSectionGroup {

		[ConfigurationProperty("Settings", IsRequired = true)]
		public CarrotCakeConfig Settings {
			get { return (CarrotCakeConfig)this.Sections["Settings"]; }
		}
	}

	//===============
	[AspNetHostingPermissionAttribute(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermissionAttribute(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CarrotCakeConfig : ConfigurationSection {

		public static CarrotCakeConfig GetConfig() {
			return (CarrotCakeConfig)WebConfigurationManager.GetSection("CarrotCakeCMS.Web/Settings") ?? new CarrotCakeConfig();
		}

		[ConfigurationProperty("Config")]
		public MainConfigElement MainConfig {
			get {
				return (MainConfigElement)this["Config"];
			}
			set {
				this["Config"] = value;
			}
		}

		[ConfigurationProperty("FileManager")]
		public FileBrowserElement FileManagerConfig {
			get {
				return (FileBrowserElement)this["FileManager"];
			}
			set {
				this["FileManager"] = value;
			}
		}

		[ConfigurationProperty("Options")]
		public OptionsElement ExtraOptions {
			get {
				return (OptionsElement)this["Options"];
			}
			set {
				this["Options"] = value;
			}
		}

		[ConfigurationProperty("AdminFooter")]
		public AdminFooterElement AdminFooterControls {
			get {
				return (AdminFooterElement)this["AdminFooter"];
			}
			set {
				this["AdminFooter"] = value;
			}
		}

		[ConfigurationProperty("PublicSite")]
		public PublicSiteElement PublicSiteControls {
			get {
				return (PublicSiteElement)this["PublicSite"];
			}
			set {
				this["PublicSite"] = value;
			}
		}

		[ConfigurationProperty("OverrideConfigFile")]
		public ConfigFileElement ConfigFileLocation {
			get {
				return (ConfigFileElement)this["OverrideConfigFile"];
			}
			set {
				this["OverrideConfigFile"] = value;
			}
		}
	}

	//==============================
	public class MainConfigElement : ConfigurationElement {

		[Description("Site identity")]
		[ConfigurationProperty("SiteID", DefaultValue = null, IsRequired = false)]
		public Guid? SiteID {
			get {
				if (this["SiteID"] != null) {
					return new Guid(this["SiteID"].ToString());
				} else {
					return null;
				}
			}
			set {
				if (this["SiteID"] != null) {
					this["SiteID"] = value.ToString();
				} else {
					this["SiteID"] = null;
				}
			}
		}

		[Description("Override parameter for admin folder")]
		[ConfigurationProperty("AdminFolderPath", DefaultValue = "/c3-admin/", IsRequired = false)]
		public string AdminFolderPath {
			get { return (string)this["AdminFolderPath"]; }
			set { this["AdminFolderPath"] = value; }
		}

		[Description("Override parameter for site skin")]
		[ConfigurationProperty("SiteSkin", DefaultValue = "Classic", IsRequired = false)]
		public string SiteSkin {
			get { return (string)this["SiteSkin"]; }
			set { this["SiteSkin"] = value; }
		}
	}

	//==============================
	public class FileBrowserElement : ConfigurationElement {

		[Description("File extensions to block from the CMS file browser")]
		[ConfigurationProperty("BlockedExtensions", DefaultValue = null, IsRequired = false)]
		public string BlockedExtensions {
			get { return (string)this["BlockedExtensions"]; }
			set { this["BlockedExtensions"] = value; }
		}
	}

	//==============================
	public class OptionsElement : ConfigurationElement {

		[Description("Indicates if error log should be written to")]
		[ConfigurationProperty("WriteErrorLog", DefaultValue = false, IsRequired = false)]
		public bool WriteErrorLog {
			get { return (bool)this["WriteErrorLog"]; }
			set { this["WriteErrorLog"] = value; }
		}

		[Description("Parameter to aid/assist migration from older CMSs that used querystring parameters")]
		[ConfigurationProperty("OldSiteQuerystring", DefaultValue = null, IsRequired = false)]
		public string OldSiteQuerystring {
			get { return (string)this["OldSiteQuerystring"]; }
			set { this["OldSiteQuerystring"] = value; }
		}
	}

	//==============================
	public class ConfigFileElement : ConfigurationElement {

		[ConfigurationProperty("SiteSkins", DefaultValue = "SiteSkins.config", IsRequired = false)]
		public string SiteSkins {
			get { return (string)this["SiteSkins"]; }
			set { this["SiteSkins"] = value; }
		}

		[ConfigurationProperty("SiteMapping", DefaultValue = "SiteMapping.config", IsRequired = false)]
		public string SiteMapping {
			get { return (string)this["SiteMapping"]; }
			set { this["SiteMapping"] = value; }
		}

		[ConfigurationProperty("PublicControls", DefaultValue = "PublicControls.config", IsRequired = false)]
		public string PublicControls {
			get { return (string)this["PublicControls"]; }
			set { this["PublicControls"] = value; }
		}

		[ConfigurationProperty("AdminModules", DefaultValue = "AdminModules.config", IsRequired = false)]
		public string AdminModules {
			get { return (string)this["AdminModules"]; }
			set { this["AdminModules"] = value; }
		}

		[ConfigurationProperty("TextContentProcessors", DefaultValue = "TextContentProcessors.config", IsRequired = false)]
		public string TextContentProcessors {
			get { return (string)this["TextContentProcessors"]; }
			set { this["TextContentProcessors"] = value; }
		}

		[ConfigurationProperty("TemplatePath", DefaultValue = "~/cmsTemplates/", IsRequired = false)]
		public string TemplatePath {
			get { return (string)this["TemplatePath"]; }
			set { this["TemplatePath"] = value; }
		}

		[ConfigurationProperty("PluginPath", DefaultValue = "~/cmsPlugins/", IsRequired = false)]
		public string PluginPath {
			get { return (string)this["PluginPath"]; }
			set { this["PluginPath"] = value; }
		}
	}

	//==============================
	public class AdminFooterElement : ConfigurationElement {

		[ConfigurationProperty("ControlPathMain", DefaultValue = null, IsRequired = false)]
		public string ControlPathMain {
			get { return (string)this["ControlPathMain"]; }
			set { this["ControlPathMain"] = value; }
		}

		[ConfigurationProperty("ControlPathPopup", DefaultValue = null, IsRequired = false)]
		public string ControlPathPopup {
			get { return (string)this["ControlPathPopup"]; }
			set { this["ControlPathPopup"] = value; }
		}

		[ConfigurationProperty("ControlPathPublic", DefaultValue = null, IsRequired = false)]
		public string ControlPathPublic {
			get { return (string)this["ControlPathPublic"]; }
			set { this["ControlPathPublic"] = value; }
		}
	}

	//==============================
	public class PublicSiteElement : ConfigurationElement {

		[ConfigurationProperty("ControlPathHeader", DefaultValue = null, IsRequired = false)]
		public string ControlPathHeader {
			get { return (string)this["ControlPathHeader"]; }
			set { this["ControlPathHeader"] = value; }
		}

		[ConfigurationProperty("ControlPathFooter", DefaultValue = null, IsRequired = false)]
		public string ControlPathFooter {
			get { return (string)this["ControlPathFooter"]; }
			set { this["ControlPathFooter"] = value; }
		}
	}
}