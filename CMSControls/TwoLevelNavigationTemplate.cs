﻿using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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

	[ToolboxData("<{0}:TwoLevelNavigationTemplate runat=server></{0}:TwoLevelNavigationTemplate>")]
	public class TwoLevelNavigationTemplate : BaseServerControl {

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool ShowSecondLevel {
			get {
				String s = (String)ViewState["ShowSecondLevel"];
				return ((s == null) ? true : Convert.ToBoolean(s));
			}

			set {
				ViewState["ShowSecondLevel"] = value.ToString();
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

		[Category("Appearance")]
		[DefaultValue("")]
		public string OverrideCSS {
			get {
				string s = (string)ViewState["OverrideCSS"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["OverrideCSS"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("div")]
		public string HtmlTagName {
			get {
				string s = (string)ViewState["HtmlTagName"];
				return ((s == null) ? "div" : s);
			}
			set {
				ViewState["HtmlTagName"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool WrapList {
			get {
				String s = (String)ViewState["WrapList"];
				return ((s == null) ? true : Convert.ToBoolean(s));
			}

			set {
				ViewState["WrapList"] = value.ToString();
			}
		}

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public ITemplate TopNavHeaderTemplate { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public ITemplate TopNavTemplate { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public ITemplate TopNavFooterTemplate { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public ITemplate SubNavHeaderTemplate { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public ITemplate SubNavTemplate { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public ITemplate SubNavFooterTemplate { get; set; }

		private SiteNav ParentPageNav { get; set; }

		private List<SiteNav> lstTwoLevelNav = new List<SiteNav>();

		protected List<SiteNav> GetTopNav() {
			return lstTwoLevelNav.Where(ct => ct.Parent_ContentID == null).OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected List<SiteNav> GetChildren(Guid rootContentID) {
			return lstTwoLevelNav.Where(ct => ct.Parent_ContentID == rootContentID).OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected SiteNav GetPageInfo(string sPath) {
			return lstTwoLevelNav.Where(ct => ct.FileName.ToLowerInvariant() == sPath.ToLowerInvariant()).FirstOrDefault();
		}

		private Control fndCtrl = null;

		private Control FindSubControl(Control X) {
			fndCtrl = null;

			FindSubControl2(X);

			return fndCtrl;
		}

		private void FindSubControl2(Control X) {
			foreach (Control c in X.Controls) {
				if (c is ListItemPlaceHolder || c is NavLinkForTemplate) {
					fndCtrl = c;
				} else {
					FindSubControl2(c);
				}
			}
		}

		protected void SetSubNav(RepeaterItem container, Guid rootContentID) {
			Control ctrl = FindSubControl(container);

			if (ctrl == null) {
				ctrl = new ListItemPlaceHolder();
				container.Controls.Add(ctrl);
			} else {
				Control ctrl2 = FindSubControl(ctrl);
				if (ctrl2 != null) {
					ctrl = ctrl2;
				}
			}

			List<SiteNav> lstNav = GetChildren(rootContentID);

			if (lstNav != null && lstNav.Any()) {
				ListItemRepeater rSubNav = new ListItemRepeater();

				rSubNav.ID = "rSubNav";
				rSubNav.HeaderTemplate = SubNavHeaderTemplate;
				rSubNav.ItemTemplate = SubNavTemplate;
				rSubNav.FooterTemplate = SubNavFooterTemplate;

				ctrl.Controls.Add(rSubNav);

				rSubNav.DataSource = lstNav;
				rSubNav.DataBind();

				rSubNav.EnableViewState = this.EnableViewState;

				UpdateHyperLink(rSubNav);
			}
		}

		private ListItemRepeater rTopNav = new ListItemRepeater();

		protected override void RenderContents(HtmlTextWriter writer) {
			writer.Indent++;
			writer.Indent++;

			writer.WriteLine();

			UpdateHyperLink(rTopNav);

			rTopNav.EnableViewState = this.EnableViewState;

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = string.Format(" class=\"{0}\"", CssClass);
			}
			int indent = writer.Indent;

			if (this.WrapList) {
				writer.WriteLine(HtmlTextWriter.TagLeftChar + HtmlTagName + sCSS + " id=\"" + this.ClientID + "\"" + HtmlTextWriter.TagRightChar);
			}

			writer.WriteLine();
			writer.Write("\t");
			rTopNav.RenderControl(writer);

			writer.Indent = indent;

			writer.WriteLine();

			if (this.WrapList) {
				writer.WriteLine(HtmlTextWriter.EndTagLeftChars + HtmlTagName + HtmlTextWriter.TagRightChar);
				writer.WriteLine();
			}

			writer.Indent--;
			writer.Indent--;
		}

		protected void LoadData() {
			ParentPageNav = GetParentPage();

			if (ShowSecondLevel) {
				lstTwoLevelNav = navHelper.GetTwoLevelNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);
			} else {
				lstTwoLevelNav = navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);
			}

			lstTwoLevelNav = CMSConfigHelper.TweakData(lstTwoLevelNav, false, true);
		}

		protected override void OnInit(EventArgs e) {
			this.Controls.Clear();

			base.OnInit(e);

			LoadData();

			if (TopNavHeaderTemplate == null || TopNavFooterTemplate == null) {
				TopNavHeaderTemplate = new DefaultListOpenNavTemplate();
				TopNavFooterTemplate = new DefaultListCloseNavTemplate();
			}

			if (SubNavHeaderTemplate == null || SubNavFooterTemplate == null) {
				SubNavHeaderTemplate = new DefaultListOpenNavTemplate();
				SubNavFooterTemplate = new DefaultListCloseNavTemplate();
			}

			if (TopNavTemplate == null) {
				TopNavTemplate = new DefaultLinkNavTemplate();
			}

			if (SubNavTemplate == null) {
				SubNavTemplate = new DefaultLinkNavTemplate();
			}

			List<SiteNav> lstTop = GetTopNav();

			rTopNav.ID = this.ClientID + "_rTopNav";
			rTopNav.HeaderTemplate = TopNavHeaderTemplate;
			rTopNav.ItemTemplate = TopNavTemplate;
			rTopNav.FooterTemplate = TopNavFooterTemplate;

			this.Controls.Add(rTopNav);

			rTopNav.DataSource = lstTop;
			rTopNav.DataBind();

			if (ShowSecondLevel) {
				int iMax = lstTop.Count;
				for (int iIdx = 0; iIdx < iMax; iIdx++) {
					SetSubNav(rTopNav.Items[iIdx], lstTop[iIdx].Root_ContentID);
				}
			}
		}

		private void ModWrap(IActivateNavItem lnk) {
			if (!string.IsNullOrEmpty(this.CSSSelected)) {
				lnk.CSSSelected = this.CSSSelected;
			}

			if ((SiteData.IsFilenameCurrentPage(lnk.NavigateUrl) || AreFilenamesSame(lnk.NavigateUrl, ParentPageNav.FileName))
					&& !string.IsNullOrEmpty(lnk.CSSSelected)) {
				lnk.IsSelected = true;
			}

			if (GetChildren(lnk.ContentID).Any() && !string.IsNullOrEmpty(lnk.CssClassHasChild)) {
				if (!string.IsNullOrEmpty(lnk.CssClassNormal)) {
					lnk.CssClassNormal = lnk.CssClassNormal + " " + lnk.CssClassHasChild;
				} else {
					lnk.CssClassNormal = lnk.CssClassHasChild;
				}
			}
		}

		private void UpdateHyperLink(Control X) {
			foreach (Control c in X.Controls) {
				if (c is IActivateNavItem) {
					IActivateNavItem lnk = (IActivateNavItem)c;
					ModWrap(lnk);
					UpdateHyperLink(c);
				} else {
					UpdateHyperLink(c);
				}
			}
		}

		protected override void OnPreRender(EventArgs e) {
			try {
				if (PublicParmValues.Any()) {
					OverrideCSS = GetParmValue("OverrideCSS", "");

					CSSSelected = GetParmValue("CSSSelected", "");
				}
			} catch (Exception ex) {
			}

			if (!string.IsNullOrEmpty(OverrideCSS)) {
				HtmlLink link = new HtmlLink();
				link.Href = OverrideCSS;
				link.Attributes.Add("rel", "stylesheet");
				link.Attributes.Add("type", "text/css");
				this.Page.Header.Controls.Add(link);
			}

			base.OnPreRender(e);
		}
	}
}

/*
<div>
 	<carrot:TwoLevelNavigationTemplate runat="server" ID="TwoLevelNavigationTemplate1" ShowSecondLevel="true">
		<%--<TopNavTemplate>
			<carrot:ListItemWrapper runat="server" ID="wrap">
				~
				<carrot:NavLinkForTemplate ID="lnk" runat="server" NavigateUrl='<%# Eval("FileName").ToString()%>' Text='<%# Eval("NavMenuText").ToString()%>' />
			</carrot:ListItemWrapper>
		</TopNavTemplate>--%>
		<%--<TopNavTemplate>--%>
		<%--<li>
				<carrot:NavLinkForTemplate ID="lnk" runat="server" Target="_blank">
					<%# Eval("NavMenuText").ToString()%>
				</carrot:NavLinkForTemplate>
			</li>--%>
		<%--<li><a href='<%# string.Format("{0}", Eval( "FileName"))%>'>
				<%# string.Format("{0}", Eval("NavMenuText"))%></a>
			</li>--%>
		<%--<li>
				<carrot:NavLinkForTemplate ID="lnk" runat="server" Target="_blank"></carrot:NavLinkForTemplate>
			</li>
		</TopNavTemplate>--%>
		<TopNavFooterTemplate>
			</ol>
		</TopNavFooterTemplate>
		<TopNavHeaderTemplate>
			<ol>
		</TopNavHeaderTemplate>
		<%--<SubNavFooterTemplate>
			</div>
		</SubNavFooterTemplate>
		<SubNavHeaderTemplate>
			<div>
		</SubNavHeaderTemplate>--%>
		<%--<SubNavTemplate>
			<carrot:ListItemWrapper runat="server" ID="wrap">
				~~~~
				<carrot:NavLinkForTemplate ID="lnk" runat="server" NavigateUrl='<%# Eval("FileName").ToString()%>' Text='<%# Eval("NavMenuText").ToString()%>' />
			</carrot:ListItemWrapper>
		</SubNavTemplate>--%>
		<SubNavTemplate>
			<li>--
				<%--<asp:HyperLink ID="lnkNav" NavigateUrl='<%# Eval("FileName").ToString()%>' runat="server">
						<%# Eval("NavMenuText").ToString()%></asp:HyperLink>--%>
				<%--<asp:HyperLink ID="lnkNav" NavigateUrl='<%# Eval("FileName").ToString()%>' Text='<%# Eval("NavMenuText").ToString()%>' runat="server">
				</asp:HyperLink>--%>
				<carrot:NavLinkForTemplate ID="lnk" runat="server" NavigateUrl='<%# Eval("FileName").ToString()%>' Text='<%# Eval("NavMenuText").ToString()%>' />
				<%--<li> -- <a href='<%# string.Format("{0}", Eval( "FileName"))%>'>
				<%# string.Format("{0}", Eval("NavMenuText"))%></a>
			</li>--%>
				-- </li>
		</SubNavTemplate>
		<SubNavFooterTemplate>
			</ol>
		</SubNavFooterTemplate>
		<SubNavHeaderTemplate>
			<ol type="i">
		</SubNavHeaderTemplate>
	</carrot:TwoLevelNavigationTemplate>
</div>
*/
