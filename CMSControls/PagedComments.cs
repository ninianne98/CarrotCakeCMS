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

	[ToolboxData("<{0}:PagedComments runat=server></{0}:PagedComments>")]
	public class PagedComments : BaseServerControl, INamingContainer {

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
		public virtual ITemplate CommentHeaderTemplate { get; set; }

		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate CommentTemplate { get; set; }

		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate CommentFooterTemplate { get; set; }



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


		private Repeater rpComments = new Repeater();
		private Repeater rpPagedComment = new Repeater();
		private HiddenField hdnPageNbr = new HiddenField();

		protected override void OnInit(EventArgs e) {

			hdnPageNbr.ID = "hdnPageNbr";
			this.Controls.Add(hdnPageNbr);

			base.OnInit(e);

			if (CommentTemplate == null) {
				CommentTemplate = new DefaultCommentTemplate();
			}
			if (PagerTemplate == null) {
				PagerTemplate = new DefaultPagerTemplate();
			}

		}

		private string sBtnName = "lnkPagerBtn";

		protected override void RenderContents(HtmlTextWriter writer) {

			HttpContext context = HttpContext.Current;

			rpComments.EnableViewState = this.EnableViewState;
			rpPagedComment.EnableViewState = true;

			rpComments.ID = "rpComments";
			rpComments.ItemTemplate = CommentTemplate;
			rpComments.HeaderTemplate = CommentHeaderTemplate;
			rpComments.FooterTemplate = CommentFooterTemplate;

			rpPagedComment.ID = "rpPagedComment";
			rpPagedComment.ItemTemplate = PagerTemplate;
			rpPagedComment.HeaderTemplate = PagerHeaderTemplate;
			rpPagedComment.FooterTemplate = PagerFooterTemplate;

			if (IsPostBack) {
				if (context.Request.Form["__EVENTARGUMENT"] != null) {
					string arg = context.Request.Form["__EVENTARGUMENT"].ToString();
					string tgt = context.Request.Form["__EVENTTARGET"].ToString();

					string sParm = this.ClientID.Replace(this.ID, "").Replace("_", "$");
					if (string.IsNullOrEmpty(sParm)) {
						sParm = this.ID + "$";
					}

					if (tgt.StartsWith(sParm)
						&& tgt.Contains(this.ID + "$")
						&& tgt.Contains("$" + sBtnName)
						&& tgt.Contains("$" + rpPagedComment.ID + "$")) {
						string[] parms = tgt.Split('$');
						int pg = int.Parse(parms[parms.Length - 1].Replace(sBtnName, ""));
						PageNumber = pg;
						hdnPageNbr.Value = PageNumber.ToString();
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
				if (!string.IsNullOrEmpty(sPageNbr)) {
					int pg = int.Parse(sPageNbr);
					PageNumber = pg;
					hdnPageNbr.Value = PageNumber.ToString();
				}
			}

			if (PageNumber <= 1 && !string.IsNullOrEmpty(hdnPageNbr.Value)) {
				PageNumber = int.Parse(hdnPageNbr.Value);
			}

			if (string.IsNullOrEmpty(OrderBy)) {
				OrderBy = "createdate  desc";
			}

			List<PostComment> lstContents = new List<PostComment>();

			int TotalPages = 0;
			int TotalRecords = 0;
			int iPageNbr = PageNumber - 1;

			SiteNav sn = navHelper.FindByFilename(SiteData.CurrentSiteID, SiteData.CurrentScriptName);
			if (sn != null) {
				TotalRecords = PostComment.GetCommentCountByContent(sn.Root_ContentID, !SecurityData.IsAuthEditor);
				lstContents = PostComment.GetCommentsByContentPageNumber(sn.Root_ContentID, iPageNbr, PageSize, OrderBy, !SecurityData.IsAuthEditor);
			}

			rpComments.DataSource = lstContents;
			rpComments.DataBind();

			TotalPages = TotalRecords / PageSize;

			if ((TotalRecords % PageSize) > 0) {
				TotalPages++;
			}

			this.Controls.Add(rpComments);
			this.Controls.Add(rpPagedComment);

			if (ShowPager && TotalPages > 1) {
				List<int> pagelist = new List<int>();
				pagelist = Enumerable.Range(1, TotalPages).ToList();

				rpPagedComment.DataSource = pagelist;
				rpPagedComment.DataBind();
			}

			WalkCtrlsForAssignment(rpPagedComment);

			writer.Indent++;
			writer.Indent++;

			writer.WriteLine();

			writer.Write("\r\n<span id=\"" + this.ClientID + "\">\r\n");

			if (PagerBelowContent) {
				RenderWrappedControl(writer, rpComments, CSSPageListing);
				RenderWrappedControl(writer, rpPagedComment, CSSPageFooter);
			} else {
				RenderWrappedControl(writer, rpPagedComment, CSSPageFooter);
				RenderWrappedControl(writer, rpComments, CSSPageListing);
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