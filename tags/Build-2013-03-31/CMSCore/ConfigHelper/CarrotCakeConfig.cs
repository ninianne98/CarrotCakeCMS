﻿using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Web;
using System.Web.Configuration;
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
		public String AdminFolderPath {
			get { return (String)this["AdminFolderPath"]; }
			set { this["AdminFolderPath"] = value; }
		}

	}

	//==============================
	public class FileBrowserElement : ConfigurationElement {

		[Description("File extensions to block from the CMS file browser")]
		[ConfigurationProperty("BlockedExtensions", DefaultValue = null, IsRequired = false)]
		public String BlockedExtensions {
			get { return (String)this["BlockedExtensions"]; }
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
		public String OldSiteQuerystring {
			get { return (String)this["OldSiteQuerystring"]; }
			set { this["OldSiteQuerystring"] = value; }
		}
	}

	//==============================
	public class ConfigFileElement : ConfigurationElement {

		[ConfigurationProperty("SiteSkins", DefaultValue = "SiteSkins.config", IsRequired = false)]
		public String SiteSkins {
			get { return (String)this["SiteSkins"]; }
			set { this["SiteSkins"] = value; }
		}

		[ConfigurationProperty("SiteMapping", DefaultValue = "SiteMapping.config", IsRequired = false)]
		public String SiteMapping {
			get { return (String)this["SiteMapping"]; }
			set { this["SiteMapping"] = value; }
		}

		[ConfigurationProperty("PublicControls", DefaultValue = "PublicControls.config", IsRequired = false)]
		public String PublicControls {
			get { return (String)this["PublicControls"]; }
			set { this["PublicControls"] = value; }
		}

		[ConfigurationProperty("AdminModules", DefaultValue = "AdminModules.config", IsRequired = false)]
		public String AdminModules {
			get { return (String)this["AdminModules"]; }
			set { this["AdminModules"] = value; }
		}

		[ConfigurationProperty("TemplatePath", DefaultValue = "~/cmsTemplates/", IsRequired = false)]
		public String TemplatePath {
			get { return (String)this["TemplatePath"]; }
			set { this["TemplatePath"] = value; }
		}

		[ConfigurationProperty("PluginPath", DefaultValue = "~/cmsPlugins/", IsRequired = false)]
		public String PluginPath {
			get { return (String)this["PluginPath"]; }
			set { this["PluginPath"] = value; }
		}

	}

	//==============================
	public class AdminFooterElement : ConfigurationElement {

		[ConfigurationProperty("ControlPathMain", DefaultValue = null, IsRequired = false)]
		public String ControlPathMain {
			get { return (String)this["ControlPathMain"]; }
			set { this["ControlPathMain"] = value; }
		}

		[ConfigurationProperty("ControlPathPopup", DefaultValue = null, IsRequired = false)]
		public String ControlPathPopup {
			get { return (String)this["ControlPathPopup"]; }
			set { this["ControlPathPopup"] = value; }
		}

		[ConfigurationProperty("ControlPathPublic", DefaultValue = null, IsRequired = false)]
		public String ControlPathPublic {
			get { return (String)this["ControlPathPublic"]; }
			set { this["ControlPathPublic"] = value; }
		}

	}


}
