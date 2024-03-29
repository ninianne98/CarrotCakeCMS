﻿using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Controls {

	public abstract class BasePagedDataTemplate : BaseServerControl, INamingContainer {

		[Category("Appearance")]
		[DefaultValue(10)]
		public int PageSize {
			get {
				String s = (String)ViewState["PageSize"];
				return ((s == null) ? 10 : int.Parse(s));
			}
			set {
				ViewState["PageSize"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue(-1)]
		public int TotalRecords {
			get {
				String s = (String)ViewState["TotalRecords"];
				return ((s == null) ? -1 : int.Parse(s));
			}
			set {
				ViewState["TotalRecords"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool PagerBelowContent {
			get {
				String s = (String)ViewState["PagerBelowContent"];
				return ((s == null) ? true : Convert.ToBoolean(s));
			}
			set {
				ViewState["PagerBelowContent"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool ShowPager {
			get {
				String s = (String)ViewState["ShowPager"];
				return ((s == null) ? true : Convert.ToBoolean(s));
			}
			set {
				ViewState["ShowPager"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool HideSpanWrapper {
			get {
				String s = (String)ViewState["HideSpanWrapper"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}
			set {
				ViewState["HideSpanWrapper"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue(1)]
		public int PageNumber {
			get {
				String s = (String)ViewState["PageNumber"];
				return ((s == null) ? 1 : int.Parse(s));
			}
			set {
				ViewState["PageNumber"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue(-1)]
		public int MaxPage {
			get {
				String s = (String)ViewState["MaxPage"];
				return ((s == null) ? -1 : int.Parse(s));
			}
			set {
				ViewState["MaxPage"] = value.ToString();
			}
		}

		protected int PageNumberZeroIndex {
			get {
				return this.PageNumber - 1;
			}
		}

		[Category("Appearance")]
		[DefaultValue("GoLiveDate  desc")]
		public virtual string OrderBy {
			get {
				String s = (String)ViewState["OrderBy"];
				return ((s == null) ? "GoLiveDate  desc" : s);
			}
			set {
				ViewState["OrderBy"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("SelectedCurrentPager")]
		public string CSSSelectedPage {
			get {
				string s = (string)ViewState["CSSSelectedPage"];
				return ((s == null) ? "SelectedCurrentPager" : s);
			}
			set {
				ViewState["CSSSelectedPage"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string CSSPageFooter {
			get {
				string s = (string)ViewState["CSSPageFooter"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSPageFooter"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string CSSPageListing {
			get {
				string s = (string)ViewState["CSSPageListing"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSPageListing"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
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

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ITemplate EmptyDataTemplate { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty), TemplateInstance(TemplateInstance.Single)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate ContentHeaderTemplate { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate ContentTemplate { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate ContentTemplateAlt { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty), TemplateInstance(TemplateInstance.Single)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate ContentFooterTemplate { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty), TemplateInstance(TemplateInstance.Single)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate PagerHeaderTemplate { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate PagerTemplate { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate PagerTemplateAlt { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty), TemplateInstance(TemplateInstance.Single)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate PagerFooterTemplate { get; set; }

		private Repeater rpPagedContents = new Repeater();
		private Repeater rpPager = new Repeater();
		private HiddenField hdnPageNbr = new HiddenField();

		protected override void OnInit(EventArgs e) {
			hdnPageNbr.ID = "hdnPageNbr";
			this.Controls.Add(hdnPageNbr);

			rpPagedContents.ID = "rpPagedContents";
			this.Controls.Add(rpPagedContents);

			rpPager.ID = "rpPager";
			this.Controls.Add(rpPager);

			base.OnInit(e);

			if (this.PagerTemplate == null) {
				var defaultTemp = new DefaultPagerTemplate(this);
				//this.PagerTemplate = defaultTemp;

				if (this.PagerHeaderTemplate == null
						&& this.PagerFooterTemplate == null) {
					var rep = defaultTemp.GetAlternatePager();

					this.PagerHeaderTemplate = rep.HeaderTemplate;
					this.PagerTemplate = rep.ItemTemplate;
					this.PagerFooterTemplate = rep.FooterTemplate;
				} else {
					this.PagerTemplate = defaultTemp;
				}
			}

			rpPagedContents.ItemTemplate = this.ContentTemplate;
			rpPagedContents.HeaderTemplate = this.ContentHeaderTemplate;
			rpPagedContents.FooterTemplate = this.ContentFooterTemplate;
			if (this.ContentTemplateAlt != null) {
				rpPagedContents.AlternatingItemTemplate = this.ContentTemplateAlt;
			}

			rpPager.ItemTemplate = this.PagerTemplate;
			rpPager.HeaderTemplate = this.PagerHeaderTemplate;
			rpPager.FooterTemplate = this.PagerFooterTemplate;
			if (this.PagerTemplateAlt != null) {
				rpPager.AlternatingItemTemplate = this.PagerTemplateAlt;
			}

			rpPagedContents.ItemDataBound += new RepeaterItemEventHandler(this.Content_ItemDataBound);
			rpPager.ItemDataBound += new RepeaterItemEventHandler(this.Pager_ItemDataBound);

			rpPagedContents.ItemCreated += new RepeaterItemEventHandler(this.Content_ItemCreated);
			rpPager.ItemCreated += new RepeaterItemEventHandler(this.Pager_ItemCreated);

			rpPagedContents.ItemCommand += new RepeaterCommandEventHandler(this.Content_ItemCommand);
			rpPager.ItemCommand += new RepeaterCommandEventHandler(this.Pager_ItemCommand);
		}

		public virtual void Content_ItemDataBound(object sender, RepeaterItemEventArgs e) {
		}

		public virtual void Pager_ItemDataBound(object sender, RepeaterItemEventArgs e) {
		}

		public virtual void Content_ItemCreated(object sender, RepeaterItemEventArgs e) {
		}

		public virtual void Pager_ItemCreated(object sender, RepeaterItemEventArgs e) {
		}

		public virtual void Content_ItemCommand(object sender, RepeaterCommandEventArgs e) {
		}

		public virtual void Pager_ItemCommand(object sender, RepeaterCommandEventArgs e) {
		}

		public virtual object DataSource { get; set; }

		//important to override so as to do any assignment of your data in your implementing class
		public virtual void FetchData() {
			this.PageNumber = 1;
			this.PageSize = 10;
			this.DataSource = new List<object>();
		}

		protected string sBtnName = "lnkPagerBtn";

		protected override void RenderContents(HtmlTextWriter writer) {
			DetectPagePosition();

			FetchData();

			WriteOutData(writer);
		}

		public SortParm ParseSort() {
			SortParm Sort = new SortParm();

			string sSortFld = string.Empty;
			string sSortDir = string.Empty;

			if (!String.IsNullOrEmpty(this.OrderBy)) {
				int pos = this.OrderBy.LastIndexOf(" ");
				sSortFld = this.OrderBy.Substring(0, pos).Trim();
				sSortDir = this.OrderBy.Substring(pos).Trim();
			}

			Sort.SortField = sSortFld;
			Sort.SortDirection = sSortDir;

			return Sort;
		}

		public void DetectPagePosition() {
			HttpContext context = HttpContext.Current;

			if (context != null) {
				if (IsPostBack) {
					if (context.Request.Form["__EVENTARGUMENT"] != null) {
						string arg = context.Request.Form["__EVENTARGUMENT"].ToString();
						string tgt = context.Request.Form["__EVENTTARGET"].ToString();

						string sParm = this.ClientID.Replace(this.ID, "").Replace("_", "$");
						if (String.IsNullOrEmpty(sParm)) {
							sParm = this.ID + "$";
						}

						if (tgt.StartsWith(sParm)
							&& tgt.Contains(this.ID + "$")
							&& tgt.Contains("$" + sBtnName)
							&& tgt.Contains("$" + rpPager.ID + "$")) {
							string[] parms = tgt.Split('$');
							int pg = int.Parse(parms[parms.Length - 1].Replace(sBtnName, ""));
							this.PageNumber = pg;
							hdnPageNbr.Value = this.PageNumber.ToString();
						}
					}
				} else {
					string sPageParm = "PageNbr";
					string sPageNbr = "";

					if (context.Request[sPageParm] != null) {
						sPageNbr = context.Request[sPageParm].ToString();
					}

					sPageParm = this.ID.ToString() + "Nbr";
					if (context.Request[sPageParm] != null) {
						sPageNbr = context.Request[sPageParm].ToString();
					}
					if (!String.IsNullOrEmpty(sPageNbr)) {
						int pg = int.Parse(sPageNbr);
						this.PageNumber = pg;
						hdnPageNbr.Value = this.PageNumber.ToString();
					}
				}
			}

			if (this.PageNumber <= 1 && !String.IsNullOrEmpty(hdnPageNbr.Value)) {
				this.PageNumber = int.Parse(hdnPageNbr.Value);
			}
		}

		public virtual void WriteOutData(HtmlTextWriter writer) {
			rpPagedContents.EnableViewState = this.EnableViewState;
			rpPager.EnableViewState = true;

			List<object> lstContents = new List<object>();

			int iTotalPages = 0;

			this.Controls.Add(rpPagedContents);
			this.Controls.Add(rpPager);

			rpPagedContents.DataSource = this.DataSource;
			rpPagedContents.DataBind();

			iTotalPages = this.TotalRecords / this.PageSize;

			if ((this.TotalRecords % this.PageSize) > 0) {
				iTotalPages++;
			}

			if (this.ShowPager && iTotalPages > 1) {
				List<int> pagelist = new List<int>();

				if (this.MaxPage > iTotalPages) {
					pagelist = Enumerable.Range(1, this.MaxPage).ToList();
				} else {
					pagelist = Enumerable.Range(1, iTotalPages).ToList();
				}

				rpPager.DataSource = pagelist;
				rpPager.DataBind();
			}

			WalkCtrlsForAssignment(rpPager);

			writer.Indent++;
			writer.Indent++;

			writer.WriteLine();

			if (!this.HideSpanWrapper) {
				writer.WriteLine("<span id=\"" + this.ClientID + "\">");
				writer.WriteLine();
			}

			if (PagerBelowContent) {
				RenderWrappedControl(writer, rpPagedContents, this.CSSPageListing);
				RenderWrappedControl(writer, rpPager, this.CSSPageFooter);
			} else {
				RenderWrappedControl(writer, rpPager, this.CSSPageFooter);
				RenderWrappedControl(writer, rpPagedContents, this.CSSPageListing);
			}

			if (this.TotalRecords <= 0) {
				PlaceHolder phEntry = new PlaceHolder();
				if (this.EmptyDataTemplate != null) {
					this.EmptyDataTemplate.InstantiateIn(phEntry);
				}

				this.Controls.Add(phEntry);
				phEntry.RenderControl(writer);
			}

			hdnPageNbr.RenderControl(writer);

			base.RenderContents(writer);

			writer.WriteLine();

			if (!this.HideSpanWrapper) {
				writer.WriteLine("</span>");
			}

			writer.Indent--;
			writer.Indent--;
		}

		protected void RenderWrappedControl(HtmlTextWriter writer, Control ctrl, string sCSSValue) {
			writer.WriteLine();
			if (!String.IsNullOrEmpty(sCSSValue)) {
				writer.WriteLine("<span class=\"" + sCSSValue + "\">");
			}
			ctrl.RenderControl(writer);
			if (!String.IsNullOrEmpty(sCSSValue)) {
				writer.WriteLine("</span>");
			}
			writer.WriteLine();
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);

			try {
				if (PublicParmValues.Any()) {
					this.PageSize = int.Parse(GetParmValue("PageSize", "10"));

					this.PagerBelowContent = Convert.ToBoolean(GetParmValue("PagerBelowContent", "true"));

					this.ShowPager = Convert.ToBoolean(GetParmValue("ShowPager", "true"));

					this.EnableViewState = Convert.ToBoolean(GetParmValue("EnableViewState", "false"));

					this.OrderBy = GetParmValue("OrderBy", "GoLiveDate  desc");

					this.CSSSelectedPage = GetParmValue("CSSSelectedPage", "SelectedCurrentPager");

					this.CSSPageListing = GetParmValue("CSSPageListing", "");

					this.CSSPageFooter = GetParmValue("CSSPageFooter", "");
				}
			} catch (Exception ex) {
			}
		}

		protected void WalkCtrlsForAssignment(Control X) {
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

	//===================
	public class SortParm {
		public string SortField { get; set; }
		public string SortDirection { get; set; }
	}
}