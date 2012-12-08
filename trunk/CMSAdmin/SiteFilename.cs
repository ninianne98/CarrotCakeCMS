using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carrotware.CMS.UI.Admin {
	public static class SiteFilename {

		public static string PageAddEditURL {
			get { return "/Manage/PageAddEdit.aspx"; }
		}
		public static string PageIndexURL {
			get { return "/Manage/PageIndex.aspx"; }
		}
		public static string BlogPostAddEditURL {
			get { return "/Manage/BlogPostAddEdit.aspx"; }
		}
		public static string BlogPostIndexURL {
			get { return "/Manage/BlogPostIndex.aspx"; }
		}

		public static string DataExportURL {
			get { return "/Manage/PageExport.aspx"; }
		}
		public static string SiteImportURL {
			get { return "/Manage/SiteImport.aspx"; }
		}
		public static string WPSiteImportURL {
			get { return "/Manage/wp-SiteImport.aspx"; }
		}

	}
}