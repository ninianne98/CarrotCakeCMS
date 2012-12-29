using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
	public static class SiteFilename {

		public static string PageAddEditURL {
			get { return SiteData.AdminFolderPath + "PageAddEdit.aspx"; }
		}
		public static string PageIndexURL {
			get { return SiteData.AdminFolderPath + "PageIndex.aspx"; }
		}
		public static string BlogPostAddEditURL {
			get { return SiteData.AdminFolderPath + "BlogPostAddEdit.aspx"; }
		}
		public static string BlogPostIndexURL {
			get { return SiteData.AdminFolderPath + "BlogPostIndex.aspx"; }
		}

		public static string DataExportURL {
			get { return SiteData.AdminFolderPath + "PageExport.aspx"; }
		}
		public static string SiteImportURL {
			get { return SiteData.AdminFolderPath + "SiteImport.aspx"; }
		}
		public static string WPSiteImportURL {
			get { return SiteData.AdminFolderPath + "wp-SiteImport.aspx"; }
		}

		public static string AdminModuleControlPath {
			get { return SiteData.AdminFolderPath + "ucAdminModule.ascx"; }
		}
		public static string EditNotifierControlPath {
			get { return SiteData.AdminFolderPath + "ucEditNotifier.ascx"; }
		}
		public static string AdvancedEditControlPath {
			get { return SiteData.AdminFolderPath + "ucAdvancedEdit.ascx"; }
		}

	}
}