using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
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
	public class PagedDataSummary : BaseServerControl, INamingContainer {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int PageSize {
			get {
				String s = (String)ViewState["PageSize"];
				return ((s == null) ? 10 : int.Parse(s));
			}
			set {
				ViewState["PageSize"] = value.ToString();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public bool PagerBelowContent {
			get {
				String s = (String)ViewState["PagerBelowContent"];
				return ((s == null) ? true : Convert.ToBoolean(s));
			}
			set {
				ViewState["PagerBelowContent"] = value.ToString();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public bool ShowPager {
			get {
				String s = (String)ViewState["ShowPager"];
				return ((s == null) ? true : Convert.ToBoolean(s));
			}
			set {
				ViewState["ShowPager"] = value.ToString();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int PageNumber {
			get {
				String s = (String)ViewState["PageNumber"];
				return ((s == null) ? 1 : int.Parse(s));
			}
			set {
				ViewState["PageNumber"] = value.ToString();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string OrderBy {
			get {
				String s = (String)ViewState["OrderBy"];
				return ((s == null) ? "createdate  desc" : s);
			}
			set {
				ViewState["OrderBy"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string DelimetedSelectedCategoriesString {
			get {
				String s = (String)ViewState["DelimetedCategoryString"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["DelimetedCategoryString"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		[Localizable(true)]
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

				return _dict;
			}
		}

		public enum SummaryContentType {
			Blog,
			ContentPage,
		}


		[Widget(WidgetAttribute.FieldMode.CheckBoxList, "lstCategories")]
		public List<Guid> SelectedCategories { get; set; }

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



		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSSelectedPage {
			get {
				string s = (string)ViewState["CSSSelectedPage"];
				return ((s == null) ? "SelectedCurrentPager" : s);
			}
			set {
				ViewState["CSSSelectedPage"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSPageFooter {
			get {
				string s = (string)ViewState["CSSPageFooter"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSPageFooter"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSPageListing {
			get {
				string s = (string)ViewState["CSSPageListing"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSPageListing"] = value;
			}
		}

		[DefaultValue(false)]
		[Themeable(false)]
		public override bool EnableViewState {
			get {
				String s = (String)ViewState["EnableViewState"];
				bool b = ((s == null) ? false : Convert.ToBoolean(s));
				base.EnableViewState = b;
				return b;
			}

			set {
				ViewState["EnableViewState"] = value.ToString();
				base.EnableViewState = value;
			}
		}


		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate SummaryHeaderTemplate { get; set; }

		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate SummaryTemplate { get; set; }

		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate SummaryFooterTemplate { get; set; }



		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate PagerHeaderTemplate { get; set; }

		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate PagerTemplate { get; set; }

		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate PagerFooterTemplate { get; set; }


		private Repeater rpDetails = new Repeater();
		private Repeater rpPagedSummary = new Repeater();
		private HiddenField hdnPageNbr = new HiddenField();

		protected override void OnInit(EventArgs e) {

			hdnPageNbr.ID = "hdnPageNbr";
			this.Controls.Add(hdnPageNbr);

			base.OnInit(e);

			if (SummaryTemplate == null) {
				SummaryTemplate = new DefaultSummaryTemplate();
			}
			if (PagerTemplate == null) {
				PagerTemplate = new DefaultPagerTemplate();
			}

		}

		private string sBtnName = "lnkPagerBtn";

		protected override void RenderContents(HtmlTextWriter writer) {

			HttpContext context = HttpContext.Current;

			rpDetails.EnableViewState = this.EnableViewState;
			rpPagedSummary.EnableViewState = true;

			rpDetails.ID = "rpDetails";
			rpDetails.ItemTemplate = SummaryTemplate;
			rpDetails.HeaderTemplate = SummaryHeaderTemplate;
			rpDetails.FooterTemplate = SummaryFooterTemplate;

			rpPagedSummary.ID = "rpPagedSummary";
			rpPagedSummary.ItemTemplate = PagerTemplate;
			rpPagedSummary.HeaderTemplate = PagerHeaderTemplate;
			rpPagedSummary.FooterTemplate = PagerFooterTemplate;

			if (IsPostBack) {
				if (context.Request.Form["__EVENTARGUMENT"] != null) {
					string arg = context.Request.Form["__EVENTARGUMENT"].ToString();
					string tgt = context.Request.Form["__EVENTTARGET"].ToString();

					if (tgt.Contains(this.ID + "$") && tgt.Contains("$" + rpPagedSummary.ID + "$")) {
						string[] parms = tgt.Split('$');
						int pg = int.Parse(parms[parms.Length - 1].Replace(sBtnName, ""));
						PageNumber = pg;
						hdnPageNbr.Value = PageNumber.ToString();
					}
				}
			}

			if (PageNumber <= 1 && !string.IsNullOrEmpty(hdnPageNbr.Value)) {
				PageNumber = int.Parse(hdnPageNbr.Value);
			}

			if (string.IsNullOrEmpty(OrderBy)) {
				OrderBy = "createdate  desc";
			}

			List<SiteNav> lstContents = null;
			OrderBy = OrderBy.Replace("|", "  ");

			string sSortFld = string.Empty;
			string sSortDir = string.Empty;

			if (!string.IsNullOrEmpty(OrderBy)) {
				int pos = OrderBy.LastIndexOf(" ");
				sSortFld = OrderBy.Substring(0, pos).Trim();
				sSortDir = OrderBy.Substring(pos).Trim();
			}

			int TotalPages = 0;
			int TotalRecords = 0;
			int iPageNbr = PageNumber - 1;

			ContentPageType.PageType viewContentType = ContentPageType.PageType.BlogEntry;

			if (ContentType != SummaryContentType.Blog) {
				viewContentType = ContentPageType.PageType.ContentEntry;
				TotalRecords = navHelper.GetSitePageCount(SiteData.CurrentSiteID, viewContentType, !SecurityData.IsAuthEditor);
				lstContents = navHelper.GetLatestContentPagedList(SiteData.CurrentSiteID, viewContentType, !SecurityData.IsAuthEditor, PageSize, iPageNbr, sSortFld, sSortDir);
			} else {
				TotalRecords = navHelper.GetFilteredContentPagedCount(SiteData.CurrentSite, SiteData.CurrentScriptName, !SecurityData.IsAuthEditor);
				lstContents = navHelper.GetFilteredContentPagedList(SiteData.CurrentSite, SiteData.CurrentScriptName, !SecurityData.IsAuthEditor, PageSize, iPageNbr, sSortFld, sSortDir);
			}

			lstContents.ToList().ForEach(q => IdentifyLinkAsInactive(q));

			rpDetails.DataSource = lstContents;
			rpDetails.DataBind();


			TotalPages = TotalRecords / PageSize;

			if ((TotalRecords % PageSize) > 0) {
				TotalPages++;
			}

			if (ShowPager && TotalPages > 1) {
				List<int> pagelist = new List<int>();
				pagelist = Enumerable.Range(1, TotalPages).ToList();

				rpPagedSummary.DataSource = pagelist;
				rpPagedSummary.DataBind();
			}

			WalkCtrlsForAssignment(rpPagedSummary);

			this.Controls.Add(rpDetails);
			this.Controls.Add(rpPagedSummary);

			writer.Indent++;
			writer.Indent++;

			writer.WriteLine();

			writer.Write("\r\n<span id=\"" + this.ClientID + "\">\r\n");

			if (PagerBelowContent) {
				RenderWrappedControl(writer, rpDetails, CSSPageListing);
				RenderWrappedControl(writer, rpPagedSummary, CSSPageFooter);
			} else {
				RenderWrappedControl(writer, rpPagedSummary, CSSPageFooter);
				RenderWrappedControl(writer, rpDetails, CSSPageListing);
			}

			hdnPageNbr.RenderControl(writer);

			base.RenderContents(writer);

			writer.Write("\r\n</span>\r\n");

			writer.Indent--;
			writer.Indent--;

		}


		private void RenderWrappedControl(HtmlTextWriter writer, Control ctrl, string sCSSValue) {
			writer.WriteLine();
			if (!string.IsNullOrEmpty(sCSSValue)) {
				writer.WriteLine("<span class=\"" + sCSSValue + "\">");
			}
			ctrl.RenderControl(writer);
			if (!string.IsNullOrEmpty(sCSSValue)) {
				writer.WriteLine("</span>");
			}
			writer.WriteLine();
		}

		protected override void OnPreRender(EventArgs e) {

			try {

				if (PublicParmValues.Count > 0) {

					PageSize = int.Parse(GetParmValue("PageSize", "10"));

					PagerBelowContent = Convert.ToBoolean(GetParmValue("PagerBelowContent", "true"));

					EnableViewState = Convert.ToBoolean(GetParmValue("EnableViewState", "false"));

					OrderBy = GetParmValue("OrderBy", "createdate  desc");

					CSSSelectedPage = GetParmValue("CSSSelectedPage", "SelectedCurrentPager");

					CSSPageListing = GetParmValue("CSSPageListing", "");

					CSSPageFooter = GetParmValue("CSSPageFooter", "");

					DelimetedSelectedCategoriesString = GetParmValue("DelimetedCategoryString", "");

					SelectedCategories = new List<Guid>();

					try {
						List<string> lst = GetParmValueList("SelectedCategories");

						foreach (string str in lst) {
							if (!string.IsNullOrEmpty(str)) {
								SelectedCategories.Add(new Guid(str));
							}
						}
					} catch (Exception ex) { }

					ContentType = (SummaryContentType)Enum.Parse(typeof(SummaryContentType), GetParmValue("ContentType", "Blog"), true);
				}
			} catch (Exception ex) {
			}


			base.OnPreRender(e);
		}

		private void WalkCtrlsForAssignment(Control X) {
			foreach (Control c in X.Controls) {
				if (c is IActivatePageNavItem) {
					IActivatePageNavItem btn = (IActivatePageNavItem)c;
					if (btn.PageNumber == PageNumber) {
						btn.IsSelected = true;
					}
					WalkCtrlsForAssignment(c);
				} else {
					WalkCtrlsForAssignment(c);
				}
			}
		}

	}

}