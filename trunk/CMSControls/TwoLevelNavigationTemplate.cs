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


		bool bFound = false;
		Control fndCtrl = null;

		private Control FindSubControl(Control X) {

			bFound = false;
			fndCtrl = null;

			FindSubControl2(X);

			return fndCtrl;
		}

		private void FindSubControl2(Control X) {
			foreach (Control c in X.Controls) {
				if (c is PlaceHolder || c is NavLinkForTemplate) {
					if (!bFound) {
						bFound = true;
						fndCtrl = c;
					}
				} else {
					if (!bFound) {
						FindSubControl2(c);
					}
				}
			}
		}


		protected void SetSubNav(RepeaterItem container, Guid rootContentID) {
			Control ctrl = FindSubControl(container);

			if (ctrl == null) {
				ctrl = new PlaceHolder();
				container.Controls.Add(ctrl);
			} else {
				Control ctrl2 = FindSubControl(ctrl);
				if (ctrl2 != null) {
					ctrl = ctrl2;
				}
			}

			List<SiteNav> lst = GetChildren(rootContentID);

			Repeater rSubNav = new Repeater();
			ctrl.Controls.Add(rSubNav);

			rSubNav.ID = "rSubNav";
			rSubNav.HeaderTemplate = SubNavHeaderTemplate;
			rSubNav.ItemTemplate = SubNavTemplate;
			rSubNav.FooterTemplate = SubNavFooterTemplate;

			rSubNav.DataSource = lst;
			rSubNav.DataBind();

			UpdateHyperLink(rSubNav);

		}


		private Repeater rTopNav = new Repeater();


		protected override void RenderContents(HtmlTextWriter output) {

			UpdateHyperLink(rTopNav);

			output.Write("<span id=\"" + this.ClientID + "\">\r\n");

			base.RenderContents(output);

			rTopNav.RenderControl(output);

			output.Write("\r\n</span>");
		}

		protected void LoadData() {

			if (ShowSecondLevel) {
				lstTwoLevelNav = navHelper.GetTwoLevelNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);
			} else {
				lstTwoLevelNav = navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);
			}

			foreach (var nav in lstTwoLevelNav.Where(n => !n.PageActive)) {
				if (!nav.PageActive) {
					nav.NavMenuText = InactivePagePrefix + nav.NavMenuText;
					nav.PageHead = InactivePagePrefix + nav.PageHead;
					nav.TitleBar = InactivePagePrefix + nav.TitleBar;
				}
			}
		}


		protected override void OnInit(EventArgs e) {
			Controls.Clear();

			base.OnInit(e);

			LoadData();

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
				SubNavHeaderTemplate = new DefaultListOpenNavTemplate();
				SubNavFooterTemplate = new DefaultListCloseNavTemplate();
			}

			if (TopNavTemplate == null) {
				TopNavTemplate = new DefaultLinkNavTemplate();
			}

			if (SubNavTemplate == null) {
				SubNavTemplate = new DefaultLinkNavTemplate();
			}

			List<SiteNav> lst = GetTopNav();

			rTopNav.ID = "rTopNav";
			rTopNav.HeaderTemplate = TopNavHeaderTemplate;
			rTopNav.ItemTemplate = TopNavTemplate;
			rTopNav.FooterTemplate = TopNavFooterTemplate;

			this.Controls.Add(rTopNav);

			rTopNav.DataSource = lst;
			rTopNav.DataBind();


			if (ShowSecondLevel) {
				int iIdx = 0;
				foreach (SiteNav c1 in lst) {
					RepeaterItem container = rTopNav.Items[iIdx];

					SetSubNav(container, c1.Root_ContentID);

					iIdx++;
				}
			}

		}


		private void ModWrap(ListItemWrapper lnk) {
			string sPage = HttpContext.Current.Request.Path.ToLower();

			if (lnk.NavigateUrl.ToLower() == sPage && !string.IsNullOrEmpty(CSSSelected)) {
				lnk.CssClass = CSSSelected;
			}
		}

		private void ModHyperLink(HyperLink lnk) {
			SiteNav nav = GetPageInfo(lnk.NavigateUrl.ToLower());

			string sPage = HttpContext.Current.Request.Path.ToLower();

			if (lnk.NavigateUrl.ToLower() == sPage && !string.IsNullOrEmpty(CSSSelected)) {
				lnk.CssClass = CSSSelected;
			}

			//if (nav != null && !nav.PageActive) {
			//    if (string.IsNullOrEmpty(lnk.Text)) {
			//        Literal lit = new Literal();
			//        lit.Text = BaseServerControl.InactivePagePrefix;
			//        lnk.Controls.AddAt(0, lit);
			//    } else {
			//        if (!lnk.Text.StartsWith(BaseServerControl.InactivePagePrefix)) {
			//            lnk.Text = BaseServerControl.InactivePagePrefix + lnk.Text;
			//        }
			//    }
			//}
		}

		private void ModHyperLink(NavLinkForTemplate lnk) {
			SiteNav nav = GetPageInfo(lnk.NavigateUrl.ToLower());

			string sPage = HttpContext.Current.Request.Path.ToLower();

			if (lnk.NavigateUrl.ToLower() == sPage && !string.IsNullOrEmpty(CSSSelected)) {
				lnk.CssClass = CSSSelected;
			}

			//if (nav != null && !nav.PageActive) {
			//    if (string.IsNullOrEmpty(lnk.Text)) {
			//        Literal lit = new Literal();
			//        lit.Text = BaseServerControl.InactivePagePrefix;
			//        lnk.Controls.AddAt(0, lit);
			//    } else {
			//        if (!lnk.Text.StartsWith(BaseServerControl.InactivePagePrefix)) {
			//            lnk.Text = BaseServerControl.InactivePagePrefix + lnk.Text;
			//        }
			//    }
			//}
		}


		private void UpdateHyperLink(Control X) {

			foreach (Control c in X.Controls) {
				if (c is HyperLink) {
					HyperLink lnk = (HyperLink)c;
					ModHyperLink(lnk);
				} else if (c is NavLinkForTemplate) {
					NavLinkForTemplate lnk = (NavLinkForTemplate)c;
					ModHyperLink(lnk);
					UpdateHyperLink(c);
				} else if (c is ListItemWrapper) {
					ListItemWrapper lnk = (ListItemWrapper)c;
					ModWrap(lnk);
					UpdateHyperLink(c);
				} else {
					UpdateHyperLink(c);
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
	[ToolboxData("<{0}:NavLinkForTemplate runat=server></{0}:NavLinkForTemplate>")]

	public class NavLinkForTemplate : Control {

		public string NavigateUrl {
			get {
				return lnk.NavigateUrl;
			}

			set {
				lnk.NavigateUrl = value;
			}
		}

		public string Text {
			get {
				return lnk.Text;
			}

			set {
				lnk.Text = value;
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

		public string ToolTip {
			get {
				return lnk.ToolTip;
			}

			set {
				lnk.ToolTip = value;
			}
		}

		public string CssClass {
			get {
				return lnk.CssClass;
			}

			set {
				lnk.CssClass = value;
			}
		}

		private HyperLink lnk = new HyperLink();
		private PlaceHolder ph = new PlaceHolder();

		public NavLinkForTemplate()
			: base() {

			LoadCtrsl();
		}

		private void LoadCtrsl() {

			this.Controls.Add(lnk);
			this.Controls.Add(ph);

			lnk.DataBinding += new EventHandler(lnkContent_DataBinding);
		}

		private void lnkContent_DataBinding(object sender, EventArgs e) {
			HyperLink lnk = (HyperLink)sender;
			RepeaterItem container = (RepeaterItem)lnk.NamingContainer;

			string sFileName = DataBinder.Eval(container, "DataItem.FileName").ToString();
			string sNavMenuText = DataBinder.Eval(container, "DataItem.NavMenuText").ToString();
			bool bPageActive = Convert.ToBoolean(DataBinder.Eval(container, "DataItem.PageActive").ToString());

			if (string.IsNullOrEmpty(lnk.NavigateUrl)) {
				lnk.NavigateUrl = sFileName;
			}

			if (string.IsNullOrEmpty(lnk.Text)) {
				lnk.Text = sNavMenuText;
			}

			//if (!bPageActive) {
			//    if (string.IsNullOrEmpty(lnk.Text)) {
			//        Literal lit = new Literal();
			//        lit.Text = BaseServerControl.InactivePagePrefix;
			//        lnk.Controls.AddAt(0, lit);
			//    } else {
			//        if (!lnk.Text.StartsWith(BaseServerControl.InactivePagePrefix)) {
			//            lnk.Text = BaseServerControl.InactivePagePrefix + lnk.Text;
			//        }
			//    }
			//}
		}
	}

	//========================================

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:ListItemWrapper runat=server></{0}:ListItemWrapper>")]

	public class ListItemWrapper : Control {

		private string _CssClass = "";
		public string CssClass {
			get {
				return _CssClass;
			}

			set {
				_CssClass = value;
				SetTag();
			}
		}

		private string _NavigateUrl = "";
		public string NavigateUrl {
			get {
				return _NavigateUrl;
			}

			set {
				_NavigateUrl = value;
			}
		}

		private string _TagName = "li";
		public string TagName {
			get {
				return _TagName;
			}

			set {
				_TagName = value;
				SetTag();
			}
		}


		private Literal litL = new Literal();
		private Literal litR = new Literal();
		private Control ctrlAll = new Control();

		public ListItemWrapper()
			: base() {

			litL.DataBinding += new EventHandler(litL_DataBinding);
		}

		private void LoadCtrsl() {
			ControlCollection ctrls = new ControlCollection(this);

			litL.Text = "\t<" + TagName + ">";
			litR.Text = "</" + TagName + ">\r\n";

			// using the for counter because of enumeration error
			//foreach (Control ctrl in this.Controls) {
			//    Ctrl.Add(ctrl);
			//}
			int iMax = this.Controls.Count - 1;

			for (int i = iMax; i >= 0; i--) {
				ctrls.Add(this.Controls[i]);
			}

			ctrlAll.Controls.Clear();

			ctrlAll.Controls.Add(litL);

			iMax = ctrls.Count - 1;
			for (int i = iMax; i >= 0; i--) {
				ctrlAll.Controls.Add(ctrls[i]);
			}

			ctrlAll.Controls.Add(litR);

			this.Controls.Clear();

			this.Controls.Add(ctrlAll);

		}


		private void SetTag() {

			litL.Text = "\t<" + TagName + ">";
			litR.Text = "</" + TagName + ">\r\n";

			if (!string.IsNullOrEmpty(CssClass)) {
				string sCSS = "";
				sCSS = string.Format(" class=\"{0}\"", CssClass);
				litL.Text = "\t<" + TagName + sCSS + ">";
			}
		}

		private void litL_DataBinding(object sender, EventArgs e) {
			SetTag();

			Literal lnk = (Literal)sender;
			RepeaterItem container = (RepeaterItem)lnk.NamingContainer;

			string sFileName = DataBinder.Eval(container, "DataItem.FileName").ToString();

			this.NavigateUrl = sFileName;
		}


		protected override void OnDataBinding(EventArgs e) {
			LoadCtrsl();

			base.OnDataBinding(e);
		}

	}

	//========================================
	public class DefaultListOpenNavTemplate : ITemplate {


		public DefaultListOpenNavTemplate() {

		}

		public void InstantiateIn(Control container) {

			Literal litL = new Literal();

			litL.Text = "\r\n<ul>\r\n";

			container.Controls.Add(litL);
		}

	}

	//========================================
	public class DefaultListCloseNavTemplate : ITemplate {

		public DefaultListCloseNavTemplate() {

		}

		public void InstantiateIn(Control container) {

			Literal litL = new Literal();
			litL.Text = "</ul>\r\n";

			container.Controls.Add(litL);
		}

	}

	//========================================
	public class DefaultLinkNavTemplate : ITemplate {

		public DefaultLinkNavTemplate() {

		}

		public void InstantiateIn(Control container) {

			PlaceHolder phAll = new PlaceHolder();

			NavLinkForTemplate lnk = new NavLinkForTemplate();
			lnk.Text = " LINK ";
			lnk.NavigateUrl = "#";

			ListItemWrapper wrap = new ListItemWrapper();
			wrap.Controls.Add(lnk);

			lnk.DataBinding += new EventHandler(lnkContent_DataBinding);

			phAll.Controls.Add(wrap);

			container.Controls.Add(phAll);
		}


		private void lnkContent_DataBinding(object sender, EventArgs e) {
			NavLinkForTemplate lnk = (NavLinkForTemplate)sender;
			RepeaterItem container = (RepeaterItem)lnk.NamingContainer;

			string sFileName = DataBinder.Eval(container, "DataItem.FileName").ToString();
			string sNavMenuText = DataBinder.Eval(container, "DataItem.NavMenuText").ToString();

			lnk.NavigateUrl = sFileName;
			lnk.Text = sNavMenuText;
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
				<asp:HyperLink ID="lnkNav" NavigateUrl='<%# Eval("FileName").ToString()%>' runat="server">
					<%# Eval("NavMenuText").ToString()%></asp:HyperLink>
				<asp:PlaceHolder runat="server" ID="ph"></asp:PlaceHolder>
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