using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using System.Text;
using System.Web;
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
				if (c is PlaceHolder) {
					return c;
				} else {
					FindSubControl(c);
				}
			}
			return null;
		}

		protected void SetSubNav(RepeaterItem container, Guid rootContentID) {
			Control ph = FindSubControl(container);
			if (ph == null) {
				ph = new PlaceHolder();
				container.Controls.Add(ph);
			}

			List<SiteNav> lst = GetChildren(rootContentID);

			Repeater rSubNav = new Repeater();
			rSubNav.ID = "rSubNav";
			rSubNav.HeaderTemplate = SubNavHeaderTemplate;
			rSubNav.ItemTemplate = SubNavTemplate;
			rSubNav.FooterTemplate = SubNavFooterTemplate;

			ph.Controls.Add(rSubNav);

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

			if (TopNavTemplate == null) {
				TopNavTemplate = new DefaultTopNavTemplate();
			}

			if (SubNavTemplate == null) {
				SubNavTemplate = new DefaultSubNavTemplate();
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


		private string GetCtrlText(Control ctrl) {
			StringBuilder sb = new StringBuilder();
			StringWriter tw = new StringWriter(sb);
			HtmlTextWriter hw = new HtmlTextWriter(tw);

			ctrl.RenderControl(hw);

			return sb.ToString();
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
	public class DefaultTopNavTemplate : ITemplate {

		public DefaultTopNavTemplate() {

		}

		public void InstantiateIn(Control container) {

			Literal litContent = new Literal();
			litContent.Text = " <a href='{0}'>{1}</a> \r\n";

			litContent.DataBinding += new EventHandler(litContent_DataBinding);

			container.Controls.Add(litContent);
		}

		private void litContent_DataBinding(object sender, EventArgs e) {
			Literal litContent = (Literal)sender;
			RepeaterItem container = (RepeaterItem)litContent.NamingContainer;
			string sTxt = litContent.Text;

			string sTxt1 = DataBinder.Eval(container, "DataItem.FileName").ToString();
			string sTxt2 = DataBinder.Eval(container, "DataItem.NavMenuText").ToString();
			bool bAct = Convert.ToBoolean(DataBinder.Eval(container, "DataItem.PageActive"));

			if (!bAct) {
				sTxt2 = BaseServerControl.InactivePagePrefix + sTxt2;
			}

			litContent.Text = String.Format(sTxt, sTxt1, sTxt2);
		}

	}


	//========================================
	public class DefaultSubNavTemplate : ITemplate {

		public DefaultSubNavTemplate() {

		}

		public void InstantiateIn(Control container) {

			Literal litContent = new Literal();
			litContent.Text = "\t&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a href='{0}'>{1}</a> \r\n";

			litContent.DataBinding += new EventHandler(litContent_DataBinding);

			container.Controls.Add(litContent);
		}

		private void litContent_DataBinding(object sender, EventArgs e) {
			Literal litContent = (Literal)sender;
			RepeaterItem container = (RepeaterItem)litContent.NamingContainer;
			string sTxt = litContent.Text;

			string sTxt1 = DataBinder.Eval(container, "DataItem.FileName").ToString();
			string sTxt2 = DataBinder.Eval(container, "DataItem.NavMenuText").ToString();
			bool bAct = Convert.ToBoolean(DataBinder.Eval(container, "DataItem.PageActive"));

			if (!bAct) {
				sTxt2 = BaseServerControl.InactivePagePrefix + sTxt2;
			}

			litContent.Text = String.Format(sTxt, sTxt1, sTxt2);
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