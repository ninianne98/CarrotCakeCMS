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



		private string _linkPage = string.Empty;
		private string _linkText = string.Empty;

		private HyperLink lnk = new HyperLink();
		private ListItemPlaceHolder ph = new ListItemPlaceHolder();


		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			_linkPage = DataBinder.Eval(container, "DataItem.FileName").ToString();
			_linkText = DataBinder.Eval(container, "DataItem.NavMenuText").ToString();

			base.OnDataBinding(e);
		}


		private void AssignVals() {
			if (!string.IsNullOrEmpty(_linkPage) && string.IsNullOrEmpty(this.NavigateUrl)) {
				this.NavigateUrl = _linkPage;
			}

			if (!string.IsNullOrEmpty(_linkText) && string.IsNullOrEmpty(this.Text)) {
				//var ctrl = FindSubControl(this, _linkText);
				//if (ctrl == null) {
				//    this.Text = _linkText;
				//}
				if (!this.HasControls()) {
					this.Text = _linkText;
				}
			}
		}


		private void LoadCtrsl() {

			int iMax = this.Controls.Count;
			lnk.Controls.Clear();

			for (int i = 0; i < iMax; i++) {
				lnk.Controls.Add(this.Controls[0]);
			}

			this.Controls.Add(lnk);
			this.Controls.Add(ph);

			AssignVals();
		}

		protected override void OnPreRender(EventArgs e) {

			LoadCtrsl();

			base.OnPreRender(e);
		}


		private void lnkContent_PreRender(object sender, EventArgs e) {
			AssignVals();
		}


		private void lnkContent_DataBinding(object sender, EventArgs e) {
			HyperLink lnk = (HyperLink)sender;
			RepeaterItem container = (RepeaterItem)lnk.NamingContainer;

			_linkPage = DataBinder.Eval(container, "DataItem.FileName").ToString();
			_linkText = DataBinder.Eval(container, "DataItem.NavMenuText").ToString();
		}
		

		Control fndCtrl = null;

		private Control FindSubControl(Control X, string txt) {

			fndCtrl = null;

			FindSubControl2(X, txt.ToLower().Trim());

			return fndCtrl;
		}

		private void FindSubControl2(Control X, string txt) {
			foreach (Control c in X.Controls) {
				if (c is Literal) {
					Literal lnk = (Literal)c;
					if (lnk.Text.ToLower().Trim().Contains(txt)) {
						fndCtrl = c;
					}
					FindSubControl2(c, txt);
				} else if (c is DataBoundLiteralControl) {
					DataBoundLiteralControl lnk = (DataBoundLiteralControl)c;
					if (lnk.Text.ToLower().Trim().Contains(txt)) {
						fndCtrl = c;
					}
					FindSubControl2(c, txt);
				} else {
					FindSubControl2(c, txt);
				}
			}
		}

	}


	//========================================

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:ListItemWrapper runat=server></{0}:ListItemWrapper>")]

	public class ListItemWrapper : Control {

		private string _css = "";
		public string CssClass {
			get {
				return _css.Trim();
			}

			set {
				_css = value;
				SetTag();
			}
		}

		private string _childCSS = "";
		public string HasChildCssClass {
			get {
				return _childCSS.Trim();
			}

			set {
				_childCSS = value;
				SetTag();
			}
		}

		private string _url = "";
		public string NavigateUrl {
			get {
				return _url;
			}

			set {
				_url = value;
			}
		}


		private Guid _id = Guid.Empty;
		public Guid ContentID {
			get {
				return _id;
			}

			set {
				_id = value;
			}
		}


		private string _tag = "li";
		public string HtmlTagName {
			get {
				return _tag.Trim();
			}

			set {
				_tag = value;
				SetTag();
			}
		}


		private Literal litL = new Literal();
		private Literal litR = new Literal();
		private Control ctrlAll = new Control();


		private void LoadCtrsl() {
			//Dictionary<int, Control> ctrls = new Dictionary<int, Control>();

			int iMax = this.Controls.Count;

			SetTag();

			// using the for counter because of enumeration error
			//foreach (Control ctrl in this.Controls) {
			//    Ctrl.Add(ctrl);
			//}

			//for (int i = iMax; i >= 0; i--) {
			//    ctrls.Add(i, this.Controls[i]);
			//}

			ctrlAll.Controls.Clear();
			ctrlAll.Controls.Add(litL);

			//instead of wind/unwind, pop stack X times
			for (int i = 0; i < iMax; i++) {
				ctrlAll.Controls.Add(this.Controls[0]);
			}

			//foreach (KeyValuePair<int, Control> d in ctrls.OrderBy(x => x.Key)) {
			//    ctrlAll.Controls.Add(d.Value);
			//}

			ListItemPlaceHolder ph = new ListItemPlaceHolder();
			ph.ID = "phListItemWrapper";

			ctrlAll.Controls.Add(ph);
			ctrlAll.Controls.Add(litR);

			this.Controls.Clear();
			this.Controls.Add(ctrlAll);

		}


		private void SetTag() {

			litL.Text = "\t<" + HtmlTagName + ">";
			litR.Text = "</" + HtmlTagName + ">\r\n";

			if (!string.IsNullOrEmpty(CssClass)) {
				string sCSS = "";
				sCSS = string.Format(" class=\"{0}\"", CssClass);
				litL.Text = "\t<" + HtmlTagName + sCSS + ">";
			}
		}


		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			string sFileName = DataBinder.Eval(container, "DataItem.FileName").ToString();
			Guid pageID = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());

			this.NavigateUrl = sFileName;
			this.ContentID = pageID;

			SetTag();

			LoadCtrsl();

			base.OnDataBinding(e);
		}

	}


	//========================================
	public class ListItemPlaceHolder : PlaceHolder {

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

			ListItemPlaceHolder phAll = new ListItemPlaceHolder();

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
				<asp:ListItemPlaceHolder runat="server" ID="ph"></asp:ListItemPlaceHolder>
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