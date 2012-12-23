using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carrotware.CMS.UI.Admin {
	public static class SiteFilename {

		public static string PageAddEditURL {
			get { return "/c3-admin/PageAddEdit.aspx"; }
		}
		public static string PageIndexURL {
			get { return "/c3-admin/PageIndex.aspx"; }
		}
		public static string BlogPostAddEditURL {
			get { return "/c3-admin/BlogPostAddEdit.aspx"; }
		}
		public static string BlogPostIndexURL {
			get { return "/c3-admin/BlogPostIndex.aspx"; }
		}

		public static string DataExportURL {
			get { return "/c3-admin/PageExport.aspx"; }
		}
		public static string SiteImportURL {
			get { return "/c3-admin/SiteImport.aspx"; }
		}
		public static string WPSiteImportURL {
			get { return "/c3-admin/wp-SiteImport.aspx"; }
		}

	}
}