using System;
namespace Carrotware.CMS.Core {
	public interface ISiteContent {
		Guid ContentID { get; set; }
		DateTime CreateDate { get; set; }
		DateTime EditDate { get; set; }
		string FileName { get; set; }
		string NavFileName { get; set; }
		string NavMenuText { get; set; }
		int? NavOrder { get; set; }
		bool PageActive { get; set; }
		string PageHead { get; set; }
		string PageText { get; set; }
		string PageTextPlainSummary { get; }
		string PageTextSummary { get; }
		Guid? Parent_ContentID { get; set; }
		Guid Root_ContentID { get; set; }
		Guid SiteID { get; set; }
		string TemplateFile { get; set; }
		string TemplateFolderPath { get; }
		string TitleBar { get; set; }
	}
}
