using System;
using System.Collections.Generic;

namespace Carrotware.CMS.Core {
	public interface ISiteContent {

		Guid ContentID { get; set; }
		DateTime CreateDate { get; set; }
		DateTime GoLiveDate { get; set; }
		DateTime RetireDate { get; set; }
		DateTime EditDate { get; set; }
		Guid? EditUserId { get; set; }
		Guid CreateUserId { get; set; }
		string FileName { get; set; }
		string Thumbnail { get; set; }
		string NavMenuText { get; set; }
		int NavOrder { get; set; }
		bool PageActive { get; set; }
		string PageHead { get; set; }
		string PageText { get; set; }
		string PageTextPlainSummaryMedium { get; }
		string PageTextPlainSummary { get; }
		Guid? Parent_ContentID { get; set; }
		Guid Root_ContentID { get; set; }
		Guid SiteID { get; set; }
		bool ShowInSiteNav { get; set; }
		string TemplateFile { get; set; }
		string TemplateFolderPath { get; }
		string TitleBar { get; set; }
		ContentPageType.PageType ContentType { get; set; }

		bool IsRetired { get; }
		bool IsUnReleased { get; }

		List<ContentTag> ContentTags { get; set; }
		List<ContentCategory> ContentCategories { get; set; }

		ExtendedUserData GetUserInfo();


	}
}
