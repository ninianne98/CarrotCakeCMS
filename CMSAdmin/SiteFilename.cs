using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Admin {
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

	}
}