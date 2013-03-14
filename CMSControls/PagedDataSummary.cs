using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:PagedDataSummary runat=server></{0}:PagedDataSummary>")]
	public class PagedDataSummary : BasePagedDataTemplate, IWidgetLimitedProperties {

		private List<GuidItem> guidList = null;
		[
		Category("Behavior"),
		Description("The GuidItem collection"),
		Browsable(false),
		DefaultValue(null),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(GuidItemCollectionEditor), typeof(UITypeEditor)),
		NotifyParentProperty(true),
		TemplateContainer(typeof(GuidItem)),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public List<GuidItem> CategoryGuidList {
			get {
				if (guidList == null) {
					guidList = new List<GuidItem>();
				}
				return guidList;
			}
		}


		private List<PagedDataSummaryTitleOption> typeLabels = null;

		[
		Category("Behavior"),
		Description("The TypeLabels collection"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(PagedDataSummaryTitleOptionEditor), typeof(UITypeEditor)),
		NotifyParentProperty(true),
		Browsable(true),
		TemplateContainer(typeof(PagedDataSummaryTitleOption)),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public List<PagedDataSummaryTitleOption> TypeLabelPrefixes {
			get {
				if (typeLabels == null) {
					typeLabels = new List<PagedDataSummaryTitleOption>();
					//typeLabels.Add(new PagedDataSummaryTitleOption { KeyValue = PageViewType.ViewType.SinglePage, LabelText = "" });
					//typeLabels.Add(new PagedDataSummaryTitleOption { KeyValue = PageViewType.ViewType.DateIndex, LabelText = "Date" });
					//typeLabels.Add(new PagedDataSummaryTitleOption { KeyValue = PageViewType.ViewType.CategoryIndex, LabelText = "Category" });
					//typeLabels.Add(new PagedDataSummaryTitleOption { KeyValue = PageViewType.ViewType.TagIndex, LabelText = "Tag" });
					//typeLabels.Add(new PagedDataSummaryTitleOption { KeyValue = PageViewType.ViewType.SearchResults, LabelText = "Search results for" });
				}
				return typeLabels;
			}
		}


		[Category("Appearance")]
		[DefaultValue("Blog")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstContentType")]
		public SummaryContentType ContentType {
			get {
				String s = (String)ViewState["ContentType"];
				SummaryContentType c = SummaryContentType.Blog;
				if (!string.IsNullOrEmpty(s)) {
					c = (SummaryContentType)Enum.Parse(typeof(SummaryContentType), s, true);
				}
				return c;
			}

			set {
				ViewState["ContentType"] = value.ToString();
			}
		}

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstContentType {
			get {
				Dictionary<string, string> _dict = new Dictionary<string, string>();
				_dict.Add("Blog", "Blog");
				_dict.Add("ContentPage", "Content Page");
				_dict.Add("ChildContentPage", "Child Content Page");
				_dict.Add("SpecifiedCategories", "Specified Categories");
				return _dict;
			}
		}

		public enum SummaryContentType {
			Unknown,
			Blog,
			ContentPage,
			ChildContentPage,
			SpecifiedCategories,
			SiteSearch
		}

		private List<Guid> _guids = null;

		[Widget(WidgetAttribute.FieldMode.CheckBoxList, "lstCategories")]
		public List<Guid> SelectedCategories {
			get {
				if (_guids == null) {
					if (CategoryGuidList.Count > 0) {
						_guids = (from n in CategoryGuidList select n.GuidValue).ToList();
					} else {
						_guids = new List<Guid>();
					}
				}
				return _guids;
			}
			set {
				_guids = value;
			}
		}

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstCategories {
			get {

				Dictionary<string, string> _dict = (from c in SiteData.CurrentSite.GetCategoryList()
													orderby c.CategoryText
													where c.SiteID == SiteData.CurrentSiteID
													select c).ToList().ToDictionary(k => k.ContentCategoryID.ToString(), v => v.CategoryText + " (" + v.CategorySlug + ")");

				return _dict;
			}
		}

		public override void FetchData() {
			HttpContext context = HttpContext.Current;

			if (string.IsNullOrEmpty(this.OrderBy)) {
				this.OrderBy = "GoLiveDate  desc";
			}

			List<SiteNav> lstContents = new List<SiteNav>();

			string sSearchTerm = String.Empty;

			ContentPageType.PageType viewContentType = ContentPageType.PageType.BlogEntry;

			if (context != null) {
				if (SiteData.CurrentScriptName.ToLower() == SiteData.CurrentSite.SiteSearchPath.ToLower()) {
					this.ContentType = SummaryContentType.SiteSearch;
					if (HttpContext.Current.Request.QueryString[SiteData.SearchQueryParameter] != null) {
						sSearchTerm = HttpContext.Current.Request.QueryString[SiteData.SearchQueryParameter].ToString();
					}
				}
			}

			switch (this.ContentType) {
				case SummaryContentType.Blog:
				case SummaryContentType.ContentPage:
				case SummaryContentType.SiteSearch:
					this.OrderBy = "GoLiveDate  desc";
					break;
			}

			SortParm sp = this.ParseSort();
			string sSortFld = sp.SortField;
			string sSortDir = sp.SortDirection;

			int iPageNbr = this.PageNumberZeroIndex;

			if (context != null) {
				switch (this.ContentType) {
					case SummaryContentType.Blog:
						viewContentType = ContentPageType.PageType.BlogEntry;
						TotalRecords = navHelper.GetFilteredContentPagedCount(SiteData.CurrentSite, SiteData.CurrentScriptName, !SecurityData.IsAuthEditor);
						lstContents = navHelper.GetFilteredContentPagedList(SiteData.CurrentSite, SiteData.CurrentScriptName, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;
					case SummaryContentType.ChildContentPage:
						viewContentType = ContentPageType.PageType.ContentEntry;
						TotalRecords = navHelper.GetChildNavigationCount(SiteData.CurrentSiteID, SiteData.CurrentScriptName, !SecurityData.IsAuthEditor);
						lstContents = navHelper.GetLatestChildContentPagedList(SiteData.CurrentSiteID, SiteData.CurrentScriptName, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;
					case SummaryContentType.ContentPage:
						viewContentType = ContentPageType.PageType.ContentEntry;
						TotalRecords = navHelper.GetSitePageCount(SiteData.CurrentSiteID, viewContentType, !SecurityData.IsAuthEditor);
						lstContents = navHelper.GetLatestContentPagedList(SiteData.CurrentSiteID, viewContentType, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;
					case SummaryContentType.SpecifiedCategories:
						viewContentType = ContentPageType.PageType.BlogEntry;
						TotalRecords = navHelper.GetFilteredContentByIDPagedCount(SiteData.CurrentSite, SelectedCategories, !SecurityData.IsAuthEditor);
						lstContents = navHelper.GetFilteredContentByIDPagedList(SiteData.CurrentSite, SelectedCategories, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;
					case SummaryContentType.SiteSearch:
						TotalRecords = navHelper.GetSiteSearchCount(SiteData.CurrentSiteID, sSearchTerm, !SecurityData.IsAuthEditor);
						lstContents = navHelper.GetLatestContentSearchList(SiteData.CurrentSiteID, sSearchTerm, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;
				}
			} else {
				viewContentType = ContentPageType.PageType.ContentEntry;
				TotalRecords = navHelper.GetSitePageCount(SiteData.CurrentSiteID, viewContentType, false);
				lstContents = navHelper.GetLatestContentPagedList(Guid.NewGuid(), viewContentType, false, this.PageSize, iPageNbr, sSortFld, sSortDir);
			}

			lstContents.ToList().ForEach(q => IdentifyLinkAsInactive(q));

			this.DataSource = lstContents;
		}

		public List<string> LimitedPropertyList {
			get {
				List<string> lst = new List<string>();
				lst.Add("PageSize");
				lst.Add("PagerBelowContent");
				lst.Add("ShowPager");
				lst.Add("EnableViewState");
				lst.Add("CSSSelectedPage");
				lst.Add("CSSPageListing");
				lst.Add("CSSPageFooter");
				lst.Add("ContentType");
				lst.Add("SelectedCategories");
				return lst;
			}
		}

		protected override void OnInit(EventArgs e) {

			if (ContentTemplate == null) {
				ContentTemplate = new DefaultSummaryTemplate();
			}

			base.OnInit(e);
		}

		protected override void OnPreRender(EventArgs e) {

			base.OnPreRender(e);

			try {

				if (PublicParmValues.Count > 0) {

					this.ContentType = (SummaryContentType)Enum.Parse(typeof(SummaryContentType), GetParmValue("ContentType", "Blog"), true);

					SelectedCategories = new List<Guid>();

					List<string> lstCategories = GetParmValueList("SelectedCategories");
					foreach (string sCat in lstCategories) {
						if (!string.IsNullOrEmpty(sCat)) {
							SelectedCategories.Add(new Guid(sCat));
						}
					}
				}
				if (SelectedCategories.Count > 0) {
					this.ContentType = SummaryContentType.SpecifiedCategories;
				}
			} catch (Exception ex) {
			}

		}

	}
}