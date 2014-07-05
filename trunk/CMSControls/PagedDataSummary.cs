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

		private List<StringItem> stringList = null;
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
				if (stringList == null) {
					stringList = new List<StringItem>();
				}
				return stringList;
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

		[Browsable(false)]
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

		[Browsable(false)]
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

		private List<string> _slugs = null;
		public List<string> SelectedCategorySlugs {
			get {
				if (_slugs == null) {
					if (CategorySlugList.Count > 0) {
						_slugs = (from n in CategorySlugList select n.StringValue).ToList();
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

				Dictionary<string, string> _dict = (from c in SiteData.CurrentSite.GetCategoryList()
													orderby c.CategoryText
													where c.SiteID == SiteData.CurrentSiteID
													select c).ToList().ToDictionary(k => k.ContentCategoryID.ToString(), v => v.CategoryText + " (" + v.CategorySlug + ")");

				return _dict;
			}
		}

		public string GetSearchTerm() {
			string sSearchTerm = String.Empty;

			if (HttpContext.Current.Request.QueryString[SiteData.SearchQueryParameter] != null) {
				sSearchTerm = HttpContext.Current.Request.QueryString[SiteData.SearchQueryParameter].ToString();
			}

			return sSearchTerm;
		}

		public override void FetchData() {
			HttpContext context = HttpContext.Current;
			string sPagePath = SiteData.CurrentScriptName;

			if (string.IsNullOrEmpty(this.OrderBy)) {
				this.OrderBy = "GoLiveDate  desc";
			}

			List<SiteNav> lstContents = new List<SiteNav>();

			string sSearchTerm = String.Empty;

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
						TotalRecords = navHelper.GetFilteredContentPagedCount(SiteData.CurrentSite, sPagePath, !SecurityData.IsAuthEditor);
						lstContents = navHelper.GetFilteredContentPagedList(SiteData.CurrentSite, sPagePath, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;
					case SummaryContentType.ChildContentPage:
						viewContentType = ContentPageType.PageType.ContentEntry;
						TotalRecords = navHelper.GetChildNavigationCount(SiteData.CurrentSiteID, sPagePath, !SecurityData.IsAuthEditor);
						lstContents = navHelper.GetLatestChildContentPagedList(SiteData.CurrentSiteID, sPagePath, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;
					case SummaryContentType.ContentPage:
						viewContentType = ContentPageType.PageType.ContentEntry;
						TotalRecords = navHelper.GetSitePageCount(SiteData.CurrentSiteID, viewContentType, !SecurityData.IsAuthEditor);
						lstContents = navHelper.GetLatestContentPagedList(SiteData.CurrentSiteID, viewContentType, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
						break;
					case SummaryContentType.SpecifiedCategories:
						viewContentType = ContentPageType.PageType.BlogEntry;
						TotalRecords = navHelper.GetFilteredContentByIDPagedCount(SiteData.CurrentSite, SelectedCategories, SelectedCategorySlugs, !SecurityData.IsAuthEditor);
						lstContents = navHelper.GetFilteredContentByIDPagedList(SiteData.CurrentSite, SelectedCategories, SelectedCategorySlugs, !SecurityData.IsAuthEditor, this.PageSize, iPageNbr, sSortFld, sSortDir);
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

			PrevNext();
		}

		[Browsable(false)]
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
				pair.LinkWrapper = (PagedDataNextPrevLinkWrapper)cu.FindControl(sCtrlName, this.Page);

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