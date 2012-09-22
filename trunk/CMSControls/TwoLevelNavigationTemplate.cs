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
	public class TwoLevelNavigationTemplate : BaseServerControl, IWidgetParmData, IWidget {

		#region IWidgetParmData Members

		private Dictionary<string, string> _parms = new Dictionary<string, string>();
		public Dictionary<string, string> PublicParmValues {
			get { return _parms; }
			set { _parms = value; }
		}

		#endregion

		#region IWidget Members

		public Guid PageWidgetID { get; set; }

		public Guid RootContentID { get; set; }

		Guid IWidget.SiteID { get; set; }

		public string JSEditFunction {
			get { return ""; }
		}
		public bool EnableEdit {
			get { return true; }
		}
		#endregion


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string OverrideCSS {
			get {
				string s = "";
				try { s = Convert.ToString(ViewState["OverrideCSS"]); } catch { ViewState["OverrideCSS"] = ""; }
				return s;
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
				return ((s == null) ? "selected" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
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

		protected SiteNav GetPageInfo(string sPage) {
			return lstTwoLevelNav.Where(ct => ct.FileName.ToLower() == sPage.ToLower()).FirstOrDefault();
		}

		private Control FindSubControl(Control X) {
			//add the command click event to the link buttons on the datagrid heading
			foreach (Control c in X.Controls) {
				if (c is PlaceHolder || c is StructuredNavLink) {
					return c;
				} else {
					FindSubControl(c);
				}
			}
			return null;
		}

		protected void SetSubNav(RepeaterItem container, Guid rootContentID) {
			Control ph1 = FindSubControl(container);

			if (ph1 == null) {
				ph1 = new PlaceHolder();
				container.Controls.Add(ph1);
			} else {
				Control ph2 = FindSubControl(ph1);
				if (ph2 != null) {
					ph1 = ph2;
				}
			}

			List<SiteNav> lst = GetChildren(rootContentID);

			Repeater rSubNav = new Repeater();
			rSubNav.ID = "rSubNav";
			rSubNav.HeaderTemplate = SubNavHeaderTemplate;
			rSubNav.ItemTemplate = SubNavTemplate;
			rSubNav.FooterTemplate = SubNavFooterTemplate;

			ph1.Controls.Add(rSubNav);

			rSubNav.DataSource = lst;
			rSubNav.DataBind();
		}

		protected override void RenderContents(HtmlTextWriter output) {

			Controls.Clear();

			lstTwoLevelNav = navHelper.GetTwoLevelNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);

			SiteNav pageNav = navHelper.GetPageCrumbNavigation(SiteData.CurrentSiteID, SiteData.CurrentScriptName);

			string sParent = "";
			if (pageNav != null) {
				sParent = pageNav.FileName.ToLower();
			}

			if (TopNavHeaderTemplate == null || TopNavFooterTemplate == null) {
				TopNavHeaderTemplate = new DefaultListOpenNavTemplate();
				TopNavFooterTemplate = new DefaultListCloseNavTemplate();
			}

			if (SubNavHeaderTemplate == null || SubNavFooterTemplate == null) {
				SubNavHeaderTemplate = new DefaultListOpenNavTemplate(true);
				SubNavFooterTemplate = new DefaultListCloseNavTemplate(true);
			}

			if (TopNavTemplate == null) {
				TopNavTemplate = new DefaultLinkNavTemplate();
			}

			if (SubNavTemplate == null) {
				SubNavTemplate = new DefaultLinkNavTemplate(true);
			}

			List<SiteNav> lst = GetTopNav();

			Repeater rTopNav = new Repeater();
			rTopNav.ID = "rTopNav";
			rTopNav.HeaderTemplate = TopNavHeaderTemplate;
			rTopNav.ItemTemplate = TopNavTemplate;
			rTopNav.FooterTemplate = TopNavFooterTemplate;

			this.Controls.Add(rTopNav);

			rTopNav.DataSource = lst;
			rTopNav.DataBind();


			int iIdx = 0;
			foreach (SiteNav c1 in lst) {
				RepeaterItem container = rTopNav.Items[iIdx];

				SetSubNav(container, c1.Root_ContentID);

				iIdx++;
			}

			this.ChildControlsCreated = true;

			UpdateHyperlinks(rTopNav);

			output.Write("<span id=\"" + this.ClientID + "\">\r\n");

			output.Write(GetCtrlText(rTopNav));

			output.Write("\r\n</span>");
		}


		private void UpdateHyperlinks(Control X) {

			foreach (Control c in X.Controls) {
				if (c is HyperLink) {
					HyperLink lnk = (HyperLink)c;
					string sPage = HttpContext.Current.Request.Path.ToLower();
					if (lnk.NavigateUrl.ToLower() == sPage && !string.IsNullOrEmpty(CSSSelected)) {
						lnk.CssClass = CSSSelected;
					}
					SiteNav nav = GetPageInfo(lnk.NavigateUrl.ToLower());
					if (nav != null && !nav.PageActive) {
						Literal lit = new Literal();
						if (string.IsNullOrEmpty(lnk.Text)) {
							lit.Text = BaseServerControl.InactivePagePrefix;
							lnk.Controls.AddAt(0, lit);
						} else {
							lnk.Text = BaseServerControl.InactivePagePrefix + lnk.Text;
						}
					}
				} else {
					UpdateHyperlinks(c);
				}
			}
		}


		protected override void OnPreRender(EventArgs e) {
			try {
				string sTmp = "";

				if (PublicParmValues.Count > 0) {
					sTmp = (from c in PublicParmValues
							where c.Key.ToLower() == "overridecss"
							select c.Value).FirstOrDefault();

					if (!string.IsNullOrEmpty(sTmp)) {
						OverrideCSS = sTmp;
					}

					sTmp = "";
					sTmp = (from c in PublicParmValues
							where c.Key.ToLower() == "cssselected"
							select c.Value).FirstOrDefault();

					if (!string.IsNullOrEmpty(sTmp)) {
						CSSSelected = sTmp;
					}

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



	//========================================

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:StructuredNavLink runat=server></{0}:StructuredNavLink>")]

	public class StructuredNavLink : WebControl {

		public string NavigateUrl {
			get {
				return lnk.NavigateUrl;
			}

			set {
				lnk.NavigateUrl = value;
			}
		}

		public string LinkText {
			get {
				return lnk.Text;
			}

			set {
				lnk.Text = value;
			}
		}

		public override string CssClass {
			get {
				return lnk.CssClass;
			}

			set {
				lnk.CssClass = value;
			}
		}


		public string Target {
			get {
				return lnk.Target;
			}

			set {
				lnk.Target = value;
			}
		}

		public override string ToolTip {
			get {
				return lnk.ToolTip;
			}

			set {
				lnk.ToolTip = value;
			}
		}

		private Control phAll = new Control();
		private HyperLink lnk = new HyperLink();
		private PlaceHolder ph = new PlaceHolder();

		public StructuredNavLink()
			: base() {

			LoadCtrsl();
		}

		public StructuredNavLink(HtmlTextWriterTag tag)
			: base(tag) {

			LoadCtrsl();
		}

		private void LoadCtrsl() {
			lnk.DataBinding += new EventHandler(lnkContent_DataBinding);

			phAll.Controls.Add(lnk);
			phAll.Controls.Add(ph);

			this.Controls.Add(phAll);
		}

		private void lnkContent_DataBinding(object sender, EventArgs e) {
			HyperLink lnk = (HyperLink)sender;
			RepeaterItem container = (RepeaterItem)lnk.NamingContainer;

			string sTxt1 = DataBinder.Eval(container, "DataItem.FileName").ToString();
			string sTxt2 = DataBinder.Eval(container, "DataItem.NavMenuText").ToString();

			if (string.IsNullOrEmpty(lnk.NavigateUrl)) {
				lnk.NavigateUrl = sTxt1;
			}

			if (string.IsNullOrEmpty(lnk.Text)) {
				lnk.Text = sTxt2;
			}
		}


		protected override void Render(HtmlTextWriter output) {
			RenderContents(output);
		}

		protected override void RenderContents(HtmlTextWriter output) {

			base.RenderContents(output);
		}
	}

	//========================================
	public class DefaultListOpenNavTemplate : ITemplate {

		private bool _indent = false;

		public DefaultListOpenNavTemplate() {

		}

		public DefaultListOpenNavTemplate(bool Indent) {
			_indent = Indent;
		}

		public void InstantiateIn(Control container) {

			Literal litL = new Literal();

			if (_indent) {
				litL.Text = "\r\n\t\t<ul>\r\n";
			} else {
				litL.Text = "\r\n<ul>\r\n";
			}

			container.Controls.Add(litL);
		}

	}

	//========================================
	public class DefaultListCloseNavTemplate : ITemplate {
		private bool _indent = false;

		public DefaultListCloseNavTemplate() {

		}

		public DefaultListCloseNavTemplate(bool Indent) {
			_indent = Indent;
		}

		public void InstantiateIn(Control container) {

			Literal litL = new Literal();
			if (_indent) {
				litL.Text = "\t\t</ul>\r\n";
			} else {
				litL.Text = "</ul>\r\n";
			}

			container.Controls.Add(litL);
		}

	}

	//========================================
	public class DefaultLinkNavTemplate : ITemplate {

		private bool _indent = false;

		public DefaultLinkNavTemplate() {

		}

		public DefaultLinkNavTemplate(bool Indent) {
			_indent = Indent;
		}

		public void InstantiateIn(Control container) {

			Literal litL = new Literal();

			if (_indent) {
				litL.Text = "\t\t\t<li>";
			} else {
				litL.Text = "<li>";
			}

			Literal litR = new Literal();
			litR.Text = "</li>\r\n";

			PlaceHolder phAll = new PlaceHolder();

			StructuredNavLink lnk = new StructuredNavLink();
			lnk.LinkText = " LINK ";
			lnk.NavigateUrl = "#";

			lnk.DataBinding += new EventHandler(lnkContent_DataBinding);

			phAll.Controls.Add(litL);
			phAll.Controls.Add(lnk);
			phAll.Controls.Add(litR);

			container.Controls.Add(phAll);
		}


		private void lnkContent_DataBinding(object sender, EventArgs e) {
			StructuredNavLink lnk = (StructuredNavLink)sender;
			RepeaterItem container = (RepeaterItem)lnk.NamingContainer;

			string sTxt1 = DataBinder.Eval(container, "DataItem.FileName").ToString();
			string sTxt2 = DataBinder.Eval(container, "DataItem.NavMenuText").ToString();

			lnk.NavigateUrl = sTxt1;
			lnk.LinkText = sTxt2;
		}



	}




}


/*
<div>
<carrot:TwoLevelNavigationTemplate runat="server" ID="TwoLevelNavigationTemplate1">
	<TopNavTemplate>
		<li>
			<asp:HyperLink ID="lnkNav" NavigateUrl='<%# Eval("FileName").ToString()%>' runat="server">
				<%# Eval("NavMenuText").ToString()%></asp:HyperLink>
			<asp:PlaceHolder runat="server" ID="ph"></asp:PlaceHolder>
		</li>
		<li>
			<carrot:StructuredNavLink ID="lnk" runat="server" NavigateUrl='<%# Eval("FileName").ToString()%>' LinkText='<%# Eval("NavMenuText").ToString()%>'>
			</carrot:StructuredNavLink>
		</li>
		<%--<li><a href='<%# String.Format("{0}", Eval( "FileName"))%>'>
			<%# String.Format("{0}", Eval("NavMenuText"))%></a>
		</li>--%>
	</TopNavTemplate>
	<TopNavFooterTemplate>
		</ul>
	</TopNavFooterTemplate>
	<TopNavHeaderTemplate>
		<ul>
	</TopNavHeaderTemplate>
	<%--<SubNavFooterTemplate>
		</div>
	</SubNavFooterTemplate>
	<SubNavHeaderTemplate>
		<div>
	</SubNavHeaderTemplate>--%>
	<SubNavTemplate>
			<li>--
				<%--
				<asp:HyperLink ID="lnkNav" NavigateUrl='<%# Eval("FileName").ToString()%>' runat="server">
					<%# Eval("NavMenuText").ToString()%></asp:HyperLink>
				--%>
				<asp:HyperLink ID="lnkNav" NavigateUrl='<%# Eval("FileName").ToString()%>' Text='<%# Eval("NavMenuText").ToString()%>' runat="server">
					</asp:HyperLink>
			</li>
		<%--<li> -- <a href='<%# String.Format("{0}", Eval( "FileName"))%>'>
			<%# String.Format("{0}", Eval("NavMenuText"))%></a>
		</li>--%>
	</SubNavTemplate>
	<SubNavFooterTemplate>
		</ol>
	</SubNavFooterTemplate>
	<SubNavHeaderTemplate>
		<ol>
	</SubNavHeaderTemplate>
</carrot:TwoLevelNavigationTemplate>
</div>
*/