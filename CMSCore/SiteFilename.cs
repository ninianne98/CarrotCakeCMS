using System;

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

	public static class SiteFilename {
		public static string RssFeedUri { get { return "/rss.ashx"; } }
		public static string SiteMapUri { get { return "/sitemap.ashx"; } }
		public static string TrackbackUri { get { return "/trackback.ashx"; } }

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

		public static string CreateFirstAdminURL {
			get { return SiteData.AdminFolderPath + "CreateFirstAdmin.aspx"; }
		}

		public static string DatabaseSetupURL {
			get { return SiteData.AdminFolderPath + "DatabaseSetup.aspx"; }
		}

		public static string PageImportURL {
			get { return SiteData.AdminFolderPath + "PageImport.aspx"; }
		}

		public static string SiteExportURL {
			get { return SiteData.AdminFolderPath + "SiteExport.aspx"; }
		}

		public static string SiteTemplateUpdateURL {
			get { return SiteData.AdminFolderPath + "SiteTemplateUpdate.aspx"; }
		}

		public static string SiteContentStatusChangeURL {
			get { return SiteData.AdminFolderPath + "SiteContentStatusChange.aspx"; }
		}

		public static string CategoryAddEditURL {
			get { return SiteData.AdminFolderPath + "CategoryAddEdit.aspx"; }
		}

		public static string TagAddEditURL {
			get { return SiteData.AdminFolderPath + "TagAddEdit.aspx"; }
		}

		public static string ModuleIndexURL {
			get { return SiteData.AdminFolderPath + "ModuleIndex.aspx"; }
		}

		public static string SiteMapURL {
			get { return SiteData.AdminFolderPath + "SiteMap.aspx"; }
		}

		public static string SiteSkinIndexURL {
			get { return SiteData.AdminFolderPath + "SiteSkinIndex.aspx"; }
		}

		public static string AboutURL {
			get { return SiteData.AdminFolderPath + "About.aspx"; }
		}

		public static string LogonURL {
			get { return SiteData.AdminFolderPath + "Logon.aspx"; }
		}

		public static string ForgotPasswordURL {
			get { return SiteData.AdminFolderPath + "ForgotPassword.aspx"; }
		}

		public static string DashboardURL {
			get { return SiteData.AdminFolderPath + "default.aspx"; }
		}

		public static string SiteInfoURL {
			get { return SiteData.AdminFolderPath + "SiteInfo.aspx"; }
		}

		public static string PageHistoryURL {
			get { return SiteData.AdminFolderPath + "PageHistory.aspx"; }
		}

		public static string PageWidgetsURL {
			get { return SiteData.AdminFolderPath + "PageWidgets.aspx"; }
		}

		public static string WidgetTimeURL {
			get { return SiteData.AdminFolderPath + "WidgetTime.aspx"; }
		}

		public static string WidgetHistoryURL {
			get { return SiteData.AdminFolderPath + "WidgetHistory.aspx"; }
		}

		public static string ContentSnippetAddEditURL {
			get { return SiteData.AdminFolderPath + "ContentSnippetAddEdit.aspx"; }
		}

		public static string ContentSnippetIndexURL {
			get { return SiteData.AdminFolderPath + "ContentSnippetIndex.aspx"; }
		}

		public static string TagIndexURL {
			get { return SiteData.AdminFolderPath + "TagIndex.aspx"; }
		}

		public static string CategoryIndexURL {
			get { return SiteData.AdminFolderPath + "CategoryIndex.aspx"; }
		}

		public static string CommentIndexURL {
			get { return SiteData.AdminFolderPath + "CommentIndex.aspx"; }
		}

		public static string UserURL {
			get { return SiteData.AdminFolderPath + "User.aspx"; }
		}

		public static string UserMembershipURL {
			get { return SiteData.AdminFolderPath + "UserMembership.aspx"; }
		}

		public static string UserGroupAddEditURL {
			get { return SiteData.AdminFolderPath + "UserGroupAddEdit.aspx"; }
		}

		public static string UserGroupsURL {
			get { return SiteData.AdminFolderPath + "UserGroups.aspx"; }
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
			get { return "~" + SiteData.AdminFolderPath + "ucAdminModule.ascx"; }
		}

		public static string EditNotifierControlPath {
			get { return "~" + SiteData.AdminFolderPath + "ucEditNotifier.ascx"; }
		}

		public static string AdvancedEditControlPath {
			get { return "~" + SiteData.AdminFolderPath + "ucAdvancedEdit.ascx"; }
		}
	}
}