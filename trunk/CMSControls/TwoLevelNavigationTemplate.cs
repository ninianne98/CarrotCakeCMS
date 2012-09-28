using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:TwoLevelNavigationTemplate runat=server></{0}:TwoLevelNavigationTemplate>")]
	public class TwoLevelNavigationTemplate : BaseServerControl {


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public bool ShowSecondLevel {
			get {
				String s = (String)ViewState["ShowSecondLevel"];
				return ((s == null) ? true : Convert.ToBoolean(s));
			}

			set {
				ViewState["ShowSecondLevel"] = value.ToString();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string OverrideCSS {
			get {
				string s = (string)ViewState["OverrideCSS"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["OverrideCSS"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string HtmlTagName {
			get {
				string s = (string)ViewState["HtmlTagName"];
				return ((s == null) ? "div" : s);
			}
			set {
				ViewState["HtmlTagName"] = value;
			}
		}




		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate TopNavHeaderTemplate { get; set; }

		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate TopNavTemplate { get; set; }

		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate TopNavFooterTemplate { get; set; }



		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate SubNavHeaderTemplate { get; set; }

		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate SubNavTemplate { get; set; }

		[DefaultValue("")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate SubNavFooterTemplate { get; set; }



		private List<SiteNav> lstTwoLevelNav = new List<SiteNav>();

		protected List<SiteNav> GetTopNav() {
			return lstTwoLevelNav.Where(ct => ct.Parent_ContentID == null).OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected List<SiteNav> GetChildren(Guid rootContentID) {
			return lstTwoLevelNav.Where(ct => ct.Parent_ContentID == rootContentID).OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected SiteNav GetPageInfo(string sPath) {
			return lstTwoLevelNav.Where(ct => ct.FileName.ToLower() == sPath.ToLower()).FirstOrDefault();
		}


		Control fndCtrl = null;

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

			List<SiteNav> lst = GetChildren(rootContentID);
			if (lst.Count > 0) {
				Repeater rSubNav = new Repeater();
				ctrl.Controls.Add(rSubNav);

				rSubNav.ID = "rSubNav";
				rSubNav.HeaderTemplate = SubNavHeaderTemplate;
				rSubNav.ItemTemplate = SubNavTemplate;
				rSubNav.FooterTemplate = SubNavFooterTemplate;

				rSubNav.DataSource = lst;
				rSubNav.DataBind();

				rSubNav.EnableViewState = false;

				UpdateHyperLink(rSubNav);
			}

		}


		private Repeater rTopNav = new Repeater();


		protected override void RenderContents(HtmlTextWriter output) {

			UpdateHyperLink(rTopNav);

			rTopNav.EnableViewState = false;

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = string.Format(" class=\"{0}\"", CssClass);
			}

			output.Write("<" + HtmlTagName + sCSS + " id=\"" + this.ClientID + "\">\r\n");

			base.RenderContents(output);

			rTopNav.RenderControl(output);

			output.Write("\r\n</" + HtmlTagName + ">");
		}

		protected void LoadData() {

			if (ShowSecondLevel) {
				lstTwoLevelNav = navHelper.GetTwoLevelNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);
			} else {
				lstTwoLevelNav = navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);
			}

			foreach (var nav in lstTwoLevelNav.Where(n => !n.PageActive)) {
				IdentifyLinkAsInactive(nav);
			}
		}


		protected override void OnInit(EventArgs e) {
			this.Controls.Clear();

			base.OnInit(e);

			this.EnableViewState = false;

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

			if (!string.IsNullOrEmpty(CSSSelected)) {
				lnk.CSSSelected = CSSSelected;
			}

			if (SiteData.IsFilenameCurrentPage(lnk.NavigateUrl) && !string.IsNullOrEmpty(lnk.CSSSelected)) {
				lnk.IsSelected = true;
			}

			if (GetChildren(lnk.ContentID).Count > 0 && !string.IsNullOrEmpty(lnk.CssClassHasChild)) {
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

				if (PublicParmValues.Count > 0) {

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
				Page.Header.Controls.Add(link);
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
		<%--<li><a href='<%# String.Format("{0}", Eval( "FileName"))%>'>
				<%# String.Format("{0}", Eval("NavMenuText"))%></a>
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
				<%--<li> -- <a href='<%# String.Format("{0}", Eval( "FileName"))%>'>
				<%# String.Format("{0}", Eval("NavMenuText"))%></a>
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