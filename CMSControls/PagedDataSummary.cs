using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:PagedDataSummary runat=server></{0}:PagedDataSummary>")]
	public class PagedDataSummary : BasePagedDataTemplate, IWidgetLimitedProperties {
		private List<GuidItem> _guidList = null;

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
				if (_guidList == null) {
					_guidList = new List<GuidItem>();
				}
				return _guidList;
			}
		}

		private List<StringItem> _stringList = null;

		[
		Category("Behavior"),
		Description("The StringItem collection"),
		Browsable(false),
		DefaultValue(null),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(StringItemCollectionEditor), typeof(UITypeEditor)),
		NotifyParentProperty(true),
		TemplateContainer(typeof(StringItem)),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public List<StringItem> CategorySlugList {
			get {
				if (_stringList == null) {
					_stringList = new List<StringItem>();
				}
				return _stringList;
			}
		}

		private List<PagedDataSummaryTitleOption> _typeLabels = null;

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
				if (_typeLabels == null) {
					_typeLabels = new List<PagedDataSummaryTitleOption>();
					//typeLabels.Add(new PagedDataSummaryTitleOption { KeyValue = PageViewType.ViewType.SinglePage, LabelText = "" });
					//typeLabels.Add(new PagedDataSummaryTitleOption { KeyValue = PageViewType.ViewType.DateIndex, LabelText = "Date" });
					//typeLabels.Add(new PagedDataSummaryTitleOption { KeyValue = PageViewType.ViewType.CategoryIndex, LabelText = "Category" });
					//typeLabels.Add(new PagedDataSummaryTitleOption { KeyValue = PageViewType.ViewType.TagIndex, LabelText = "Tag" });
					//typeLabels.Add(new PagedDataSummaryTitleOption { KeyValue = PageViewType.ViewType.SearchResults, LabelText = "Search results for" });
				}
				return _typeLabels;
			}
		}

		[Category("Appearance")]
		public string LinkNext {
			get {
				string s = (string)ViewState["LinkNext"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["LinkNext"] = value;
			}
		}

		[Category("Appearance")]
		public string LinkPrev {
			get {
				string s = (string)ViewState["LinkPrev"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["LinkPrev"] = value;
			}
		}

		[Category("Appearance")]
		public string LinkFirst {
			get {
				string s = (string)ViewState["LinkFirst"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["LinkFirst"] = value;
			}
		}

		[Category("Appearance")]
		public string LinkLast {
			get {
				string s = (string)ViewState["LinkLast"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["LinkLast"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool IgnoreSitePath {
			get {
				bool s = false;
				if (ViewState["IgnoreSitePath"] != null) {
					try { s = (bool)ViewState["IgnoreSitePath"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["IgnoreSitePath"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("Blog")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, nameof(lstContentType))]
		public SummaryContentType ContentType {
			get {
				string s = (string)ViewState["ContentType"];
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

		[Browsable(false)]
		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstContentType {
			get {
				var _dict = typeof(SummaryContentType).ToDescriptionDictionary()
						.Where(x => x.Key != SummaryContentType.Unknown.ToString()
										&& x.Key != SummaryContentType.SiteSearch.ToString())
						.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

				return _dict;
			}
		}

		public enum SummaryContentType {
			Unknown,

			[Description("Blog")]
			Blog,

			[Description("Content Page")]
			ContentPage,

			[Description("Child Content Page")]
			ChildContentPage,

			[Description("Specified Categories")]
			SpecifiedCategories,

			[Description("Site Search")]
			SiteSearch
		}

		private List<Guid> _guids = null;

		[Browsable(false)]
		[Widget(WidgetAttribute.FieldMode.CheckBoxList, "lstCategories")]
		public List<Guid> SelectedCategories {
			get {
				if (_guids == null) {
					if (this.CategoryGuidList.Any()) {
						_guids = (from n in this.CategoryGuidList select n.GuidValue).ToList();
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

		private List<string> _slugs = null;

		public List<string> SelectedCategorySlugs {
			get {
				if (_slugs == null) {
					if (this.CategorySlugList.Any()) {
						_slugs = (from n in this.CategorySlugList select n.StringValue).ToList();
					} else {
						_slugs = new List<string>();
					}
				}
				return _slugs;
			}
			set {
				_slugs = value;
			}
		}

		[Browsable(false)]
		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstCategories {
			get {
				var _dict = (from c in SiteData.CurrentSite.GetCategoryList()
							 orderby c.CategoryText
							 where c.SiteID == SiteData.CurrentSiteID
							 select c).ToList()
							.ToDictionary(k => k.ContentCategoryID.ToString(), v => v.CategoryText + " (" + v.CategorySlug + ")");

				return _dict;
			}
		}

		public string GetSearchTerm() {
			string sSearchTerm = HttpContext.Current.SafeQueryString(SiteData.SearchQueryParameter);

			return sSearchTerm;
		}

		public override void FetchData() {
			this.EnableViewState = false;
			HttpContext context = HttpContext.Current;

			string sPagePath = SiteData.CurrentScriptName;

			if (string.IsNullOrEmpty(this.OrderBy)) {
				this.OrderBy = "GoLiveDate  desc";
			}

			List<SiteNav> lstContents = new List<SiteNav>();

			string sSearchTerm = string.Empty;

			ContentPageType.PageType viewContentType = ContentPageType.PageType.BlogEntry;

			if (this.IgnoreSitePath) {
				sPagePath = string.Format("/siteid-{0}.aspx", SiteData.CurrentSiteID);
			}

			if (context != null) {
				if (SiteData.CurrentSite.IsSiteSearchPath && !this.IgnoreSitePath) {
					this.ContentType = SummaryContentType.SiteSearch;
					sSearchTerm = GetSearchTerm();
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
						TotalRecords = _navHelper.GetFilteredContentPagedCount(SiteData.CurrentSite, sPagePath, !SecurityData.IsAuthEditor);
						lstContents = _navHelper.GetFilteredContentPagedList(SiteData.CurrentSite, sPagePath, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;

					case SummaryContentType.ChildContentPage:
						viewContentType = ContentPageType.PageType.ContentEntry;
						TotalRecords = _navHelper.GetChildNavigationCount(SiteData.CurrentSiteID, sPagePath, !SecurityData.IsAuthEditor);
						lstContents = _navHelper.GetLatestChildContentPagedList(SiteData.CurrentSiteID, sPagePath, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;

					case SummaryContentType.ContentPage:
						viewContentType = ContentPageType.PageType.ContentEntry;
						TotalRecords = _navHelper.GetSitePageCount(SiteData.CurrentSiteID, viewContentType, !SecurityData.IsAuthEditor);
						lstContents = _navHelper.GetLatestContentPagedList(SiteData.CurrentSiteID, viewContentType, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;

					case SummaryContentType.SpecifiedCategories:
						viewContentType = ContentPageType.PageType.BlogEntry;
						TotalRecords = _navHelper.GetFilteredContentByIDPagedCount(SiteData.CurrentSite, SelectedCategories, SelectedCategorySlugs, !SecurityData.IsAuthEditor);
						lstContents = _navHelper.GetFilteredContentByIDPagedList(SiteData.CurrentSite, SelectedCategories, SelectedCategorySlugs, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;

					case SummaryContentType.SiteSearch:
						TotalRecords = _navHelper.GetSiteSearchCount(SiteData.CurrentSiteID, sSearchTerm, !SecurityData.IsAuthEditor);
						lstContents = _navHelper.GetLatestContentSearchList(SiteData.CurrentSiteID, sSearchTerm, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;
				}
			} else {
				viewContentType = ContentPageType.PageType.ContentEntry;
				TotalRecords = _navHelper.GetSitePageCount(SiteData.CurrentSiteID, viewContentType, false);
				lstContents = _navHelper.GetLatestContentPagedList(Guid.NewGuid(), viewContentType, false, this.PageSize, iPageNbr, sSortFld, sSortDir);
			}

			lstContents = CMSConfigHelper.TweakData(lstContents);

			this.DataSource = lstContents;

			PrevNext();
		}

		public override void Pager_ItemDataBound(object sender, RepeaterItemEventArgs e) {
			base.Pager_ItemDataBound(sender, e);

			if (e.Item.ItemType == ListItemType.Footer || e.Item.ItemType == ListItemType.Header) {
				PrevNext();
			}
		}

		[Browsable(false)]
		public List<string> LimitedPropertyList {
			get {
				List<string> lst = new List<string>();
				lst.Add("PageSize");
				lst.Add("PagerBelowContent");
				lst.Add("ShowPager");
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
				ContentTemplate = new DefaultSummaryTemplate(this);
			}

			base.OnInit(e);
		}

		protected void SetNextPrevLink(PagedDataNextPrevLinkWrapper.PagedDataDirection dir, PagedDataNextPrevLink lnkNP, int iPage) {
			string sSearchTerm = GetSearchTerm();
			string sPageParm = this.ID.ToString() + "Nbr";

			if (lnkNP != null) {
				HttpContext context = HttpContext.Current;

				lnkNP.NavDirection = dir;
				lnkNP.SetText();

				lnkNP.NavigateUrl = string.Format("{0}?{1}={2}", SiteData.CurrentScriptName, sPageParm, iPage);

				if (!string.IsNullOrEmpty(sSearchTerm)) {
					lnkNP.NavigateUrl = string.Format("{0}&{1}={2}", lnkNP.NavigateUrl, SiteData.SearchQueryParameter, context.Server.UrlEncode(sSearchTerm));
				}
			}
		}

		protected void SetNextPrevLinkVisibility(PagedDataNextPrevLinkWrapper.PagedDataDirection dir, PagedDataNextPrevLinkPair lnkPair, int iPage, bool ShowLink) {
			if (lnkPair.PageLink != null) {
				SetNextPrevLink(dir, lnkPair.PageLink, iPage);
				lnkPair.PageLink.Visible = ShowLink;
			}

			if (lnkPair.LinkWrapper != null) {
				lnkPair.LinkWrapper.Visible = ShowLink;
			}
		}

		protected PagedDataNextPrevLinkPair FindPrevNextCtrl(string sCtrlName) {
			PagedDataNextPrevLinkPair pair = new PagedDataNextPrevLinkPair();

			try {
				ControlUtilities cu = new ControlUtilities(this.Page);
				Control ctrl = cu.FindControl(sCtrlName, this.Page);
				pair.LinkWrapper = (PagedDataNextPrevLinkWrapper)ctrl;
				if (pair.LinkWrapper == null) {
					pair.PageLink = (PagedDataNextPrevLink)cu.FindControl(sCtrlName, this.Page);
				} else {
					pair.PageLink = (PagedDataNextPrevLink)cu.FindControl(typeof(PagedDataNextPrevLink), pair.LinkWrapper);
				}
			} catch (Exception ex) { }

			return pair;
		}

		protected void PrevNext() {
			int iTotalPages = this.TotalRecords / this.PageSize;
			if ((this.TotalRecords % this.PageSize) > 0) {
				iTotalPages++;
			}

			if (!string.IsNullOrEmpty(this.LinkNext)) {
				PagedDataNextPrevLinkPair pair = FindPrevNextCtrl(this.LinkNext);

				int iPageNum = this.PageNumber + 1;

				bool bShowLink = (iPageNum < this.MaxPage && this.MaxPage > 0) || this.PageNumber < iTotalPages;

				SetNextPrevLinkVisibility(PagedDataNextPrevLinkWrapper.PagedDataDirection.Next, pair, iPageNum, bShowLink);
			}

			if (!string.IsNullOrEmpty(this.LinkPrev)) {
				PagedDataNextPrevLinkPair pair = FindPrevNextCtrl(this.LinkPrev);

				int iPageNum = this.PageNumber - 1;
				bool bShowLink = this.PageNumber > 1;

				SetNextPrevLinkVisibility(PagedDataNextPrevLinkWrapper.PagedDataDirection.Previous, pair, iPageNum, bShowLink);
			}

			if (!string.IsNullOrEmpty(this.LinkFirst)) {
				PagedDataNextPrevLinkPair pair = FindPrevNextCtrl(this.LinkFirst);

				int iPageNum = 1;
				bool bShowLink = (this.PageNumber > iPageNum);

				SetNextPrevLinkVisibility(PagedDataNextPrevLinkWrapper.PagedDataDirection.First, pair, iPageNum, bShowLink);
			}

			if (!string.IsNullOrEmpty(this.LinkLast)) {
				PagedDataNextPrevLinkPair pair = FindPrevNextCtrl(this.LinkLast);

				int iPageNum = iTotalPages;

				if (this.MaxPage > 0) {
					iPageNum = this.MaxPage;
				}

				bool bShowLink = (this.PageNumber < iPageNum);

				SetNextPrevLinkVisibility(PagedDataNextPrevLinkWrapper.PagedDataDirection.Last, pair, iPageNum, bShowLink);
			}
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);

			try {
				if (this.PublicParmValues.Any()) {
					var foundVal = this.GetValue(x => x.ContentType, this.ContentType);
					this.ContentType = foundVal;

					this.SelectedCategories = new List<Guid>();

					List<string> lstCategories = this.GetParmValueList(nameof(this.SelectedCategories));
					foreach (string cat in lstCategories) {
						if (!string.IsNullOrEmpty(cat)) {
							this.SelectedCategories.Add(new Guid(cat));
						}
					}
				}
				if (this.SelectedCategories.Any()) {
					this.ContentType = SummaryContentType.SpecifiedCategories;
				}
			} catch (Exception ex) {
			}
		}
	}
}