using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
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

	public class NavLinkForTemplate : Control, IActivateNavItem {

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


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CssClassNormal {
			get {
				string s = (string)ViewState["CssClassNormal"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CssClassNormal"] = value;
				SetCSS();
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
				SetCSS();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CssClassHasChild {
			get {
				string s = (string)ViewState["HasChildCssClass"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["HasChildCssClass"] = value;
				SetCSS();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool IsSelected {
			get {
				bool s = false;
				if (ViewState["IsSelected"] != null) {
					try { s = (bool)ViewState["IsSelected"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["IsSelected"] = value;
				SetCSS();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		[Localizable(true)]
		public bool UseDefaultText {
			get {
				bool s = true;
				if (ViewState["UseDefaultText"] != null) {
					try { s = (bool)ViewState["UseDefaultText"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["UseDefaultText"] = value;
				SetCSS();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Guid ContentID {
			get {
				Guid s = Guid.Empty;
				if (ViewState["ContentID"] != null) {
					try { s = new Guid(ViewState["ContentID"].ToString()); } catch { }
				}
				return s;
			}
			set {
				ViewState["ContentID"] = value;
			}
		}


		private void SetCSS() {

			if (!string.IsNullOrEmpty(CssClassNormal) || !string.IsNullOrEmpty(CSSSelected)) {
				string sCSS = "";
				string sSelCss = "";

				if (IsSelected) {
					sSelCss = CSSSelected.Trim();
				}

				sCSS = string.Format("{0} {1}", CssClassNormal.Trim(), sSelCss);

				lnk.CssClass = sCSS.Trim();
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

			Guid pageID = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());

			this.ContentID = pageID;

			LoadCtrsl();

			base.OnDataBinding(e);
		}


		protected override void OnPreRender(EventArgs e) {

			base.OnPreRender(e);

			AssignVals();
		}


		private void AssignVals() {

			SetCSS();

			if (!string.IsNullOrEmpty(_linkPage) && string.IsNullOrEmpty(this.NavigateUrl)) {
				this.NavigateUrl = _linkPage;
			}

			if (!string.IsNullOrEmpty(_linkText) && string.IsNullOrEmpty(this.LinkText)) {
				//var ctrl = FindSubControl(this, _linkText);
				//if (ctrl == null) {
				//    this.Text = _linkText;
				//}
				//if (!this.HasControls()) {
				//    this.Text = _linkText;
				//}
				if (UseDefaultText) {
					this.LinkText = _linkText;
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

		}

		bool bFoundCtrl = false;
		Control fndCtrl = null;

		private Control FindSubControl(Control X, string txt) {

			bFoundCtrl = false;
			fndCtrl = null;

			FindSubControl2(X, txt.ToLower().Trim());

			return fndCtrl;
		}

		private void FindSubControl2(Control X, string txt) {
			foreach (Control c in X.Controls) {
				if (!bFoundCtrl) {
					if (c is ITextControl) {
						ITextControl lnk = (ITextControl)c;
						if (lnk.Text.ToLower().Trim().Contains(txt)) {
							fndCtrl = c;
						}
						bFoundCtrl = true;
					} else {
						FindSubControl2(c, txt);
					}
				}
			}
		}

	}


	//========================================

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:ListItemWrapper runat=server></{0}:ListItemWrapper>")]

	public class ListItemWrapper : Control, IActivateNavItem {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CssClassNormal {
			get {
				string s = (string)ViewState["CssClassNormal"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CssClassNormal"] = value;
				SetTag();
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
				SetTag();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool IsSelected {
			get {
				bool s = false;
				if (ViewState["IsSelected"] != null) {
					try { s = (bool)ViewState["IsSelected"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["IsSelected"] = value;
				SetTag();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CssClassHasChild {
			get {
				string s = (string)ViewState["HasChildCssClass"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["HasChildCssClass"] = value;
				SetTag();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string NavigateUrl {
			get {
				string s = (string)ViewState["NavigateUrl"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["NavigateUrl"] = value;
				SetTag();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Guid ContentID {
			get {
				Guid s = Guid.Empty;
				try { s = new Guid(ViewState["ContentID"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["ContentID"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string HtmlTagName {
			get {
				string s = (string)ViewState["HtmlTagName"];
				return ((s == null) ? "li" : s);
			}
			set {
				ViewState["HtmlTagName"] = value;
				SetTag();
			}
		}


		private Literal litOpen = new Literal();
		private Literal litClose = new Literal();
		private Control ctrlAll = new Control();


		private void LoadCtrsl() {
			int iMax = this.Controls.Count;

			SetTag();

			ctrlAll.Controls.Clear();
			ctrlAll.Controls.Add(litOpen);

			//instead of wind/unwind, pop stack X times
			for (int i = 0; i < iMax; i++) {
				ctrlAll.Controls.Add(this.Controls[0]);
			}

			ListItemPlaceHolder ph = new ListItemPlaceHolder();

			ctrlAll.Controls.Add(ph);
			ctrlAll.Controls.Add(litClose);

			this.Controls.Clear();
			this.Controls.Add(ctrlAll);

		}


		private void SetTag() {

			litOpen.Text = HtmlTextWriter.TagLeftChar + HtmlTagName + HtmlTextWriter.TagRightChar;
			litClose.Text = HtmlTextWriter.EndTagLeftChars + HtmlTagName + HtmlTextWriter.TagRightChar;

			if (!string.IsNullOrEmpty(CssClassNormal) || !string.IsNullOrEmpty(CSSSelected)) {
				string sCSS = "";
				string sSelCss = "";

				if (IsSelected) {
					sSelCss = CSSSelected.Trim();
				}

				if (!string.IsNullOrEmpty(CssClassNormal) || !string.IsNullOrEmpty(sSelCss)) {
					sCSS = string.Format(" class=\"{0} {1}\"", CssClassNormal.Trim(), sSelCss);
				}

				litOpen.Text = HtmlTextWriter.TagLeftChar + HtmlTagName + sCSS + HtmlTextWriter.TagRightChar;
			}
		}

		//protected override void RenderChildren(HtmlTextWriter writer) {
		//    int indent = writer.Indent++;

		//    writer.Indent = indent + 2;

		//    //writer.WriteLine();
		//    base.RenderChildren(writer);
		//    //writer.WriteLine();

		//    writer.Indent = indent;
		//    writer.Indent--;
		//}

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
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:ListItemNavText runat=server></{0}:ListItemNavText>")]
	public class ListItemNavText : Control, ITextControl {

		public enum NavTextFieldName {
			Root_ContentID,
			SiteID,
			FileName,
			PageActive,
			CreateDate,
			ContentID,
			Parent_ContentID,
			TitleBar,
			NavMenuText,
			PageHead,
			PageTextPlainSummary,
			NavOrder,
			EditUserId,
			EditDate,
			TemplateFile
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Guid ContentID {
			get {
				Guid s = Guid.Empty;
				try { s = new Guid(ViewState["ContentID"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["ContentID"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string NavigateUrl {
			get {
				string s = (string)ViewState["NavigateUrl"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["NavigateUrl"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string Text {
			get {
				string s = (string)ViewState["Text"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["Text"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("{0}")]
		[Localizable(true)]
		public string FieldFormat {
			get {
				String s = ViewState["FieldFormat"] as String;
				return ((s == null) ? "{0}" : s);
			}
			set {
				ViewState["FieldFormat"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("NavMenuText")]
		[Localizable(true)]
		public NavTextFieldName DataField {
			get {
				string s = (string)ViewState["DataField"];
				NavTextFieldName c = (NavTextFieldName)Enum.Parse(typeof(NavTextFieldName), "NavMenuText", true);
				if (!string.IsNullOrEmpty(s)) {
					try {
						c = (NavTextFieldName)Enum.Parse(typeof(NavTextFieldName), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["DataField"] = value.ToString();
			}
		}


		protected override void Render(HtmlTextWriter output) {

			output.Write(this.Text);

			//base.Render(output);

		}


		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			string sFieldValue = string.Format(FieldFormat, DataBinder.Eval(container, "DataItem." + DataField.ToString()));
			string sFileName = DataBinder.Eval(container, "DataItem.FileName").ToString();
			Guid pageID = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());

			this.Text = sFieldValue;
			this.NavigateUrl = sFileName;
			this.ContentID = pageID;

			base.OnDataBinding(e);
		}


	}

	//========================================
	public class ListItemRepeater : Repeater {

		//[Bindable(true)]
		//[Category("Appearance")]
		//[DefaultValue("")]
		//[Localizable(true)]
		//public int IndentPad {
		//    get {
		//        int s = 1;
		//        try { s = int.Parse(ViewState["IndentPad"].ToString()); } catch { }
		//        return s;
		//    }
		//    set {
		//        ViewState["IndentPad"] = value.ToString();
		//    }
		//}

		//protected override void Render(HtmlTextWriter writer) {
		//    int indent = writer.Indent++;

		//    writer.Indent = indent + IndentPad;

		//    base.Render(writer);

		//    writer.Indent = indent;
		//    writer.Indent--;
		//}

		//public override void RenderControl(HtmlTextWriter writer) {
		//    int indent = writer.Indent++;

		//    writer.Indent = indent + IndentPad;

		//    base.RenderControl(writer);

		//    writer.Indent = indent;
		//    writer.Indent--;
		//}

		//protected override void RenderChildren(HtmlTextWriter writer) {
		//    int indent = writer.Indent++;

		//    foreach (Control c in this.Controls) {
		//        writer.Indent = indent;
		//        if (c is ListItemRepeater) {
		//            string html = "\t\t" + BaseServerControl.GetCtrlText(c).Replace("\r\n", "\r\n\t\t");
		//            writer.Write(html);
		//            writer.WriteLine();
		//        } else {
		//            writer.Indent--;
		//            c.RenderControl(writer);
		//        }
		//        writer.Indent = indent;
		//    }

		//    writer.Indent--;
		//}
	}

	//========================================
	public class ListItemPlaceHolder : PlaceHolder {

		//[Bindable(true)]
		//[Category("Appearance")]
		//[DefaultValue("")]
		//[Localizable(true)]
		//public int IndentPad {
		//    get {
		//        int s = 1;
		//        try { s = int.Parse(ViewState["IndentPad"].ToString()); } catch { }
		//        return s;
		//    }
		//    set {
		//        ViewState["IndentPad"] = value.ToString();
		//    }
		//}

		//protected override void Render(HtmlTextWriter writer) {
		//    int indent = writer.Indent++;

		//    writer.Indent = indent + IndentPad;

		//    base.Render(writer);

		//    writer.Indent = indent;
		//    writer.Indent--;
		//}

		//public override void RenderControl(HtmlTextWriter writer) {
		//    int indent = writer.Indent++;

		//    writer.Indent = indent + IndentPad;

		//    base.RenderControl(writer);

		//    writer.Indent = indent;
		//    writer.Indent--;
		//}

		protected override void RenderChildren(HtmlTextWriter writer) {
			int indent1 = writer.Indent;

			foreach (Control c in this.Controls) {
				writer.Indent = indent1;
				if (c is ListItemRepeater) {
					writer.Write("\t");
					writer.Write(BaseServerControl.GetCtrlText(c).Replace("\r\n", "\r\n\t\t"));
					writer.WriteLine();
					writer.Write("\t\t");
				} else {
					c.RenderControl(writer);
				}
			}

			writer.Indent = indent1;
		}
	}

	//========================================
	public class DefaultListOpenNavTemplate : ITemplate {


		public DefaultListOpenNavTemplate() {

		}

		public void InstantiateIn(Control container) {

			Literal litL = new Literal();

			litL.Text = HtmlTextWriter.TagLeftChar + "ul" + HtmlTextWriter.TagRightChar;

			container.Controls.Add(litL);
		}

	}

	//========================================
	public class DefaultListCloseNavTemplate : ITemplate {

		public DefaultListCloseNavTemplate() {

		}

		public void InstantiateIn(Control container) {

			Literal litL = new Literal();
			litL.Text = HtmlTextWriter.EndTagLeftChars + "ul" + HtmlTextWriter.TagRightChar;

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
			lnk.LinkText = " LINK ";
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
			lnk.LinkText = sNavMenuText;
		}
	}


	//========================================
	public interface IActivateNavItem {

		string CSSSelected { get; set; }

		string CssClassNormal { get; set; }

		string CssClassHasChild { get; set; }

		bool IsSelected { get; set; }

		string NavigateUrl { get; set; }

		Guid ContentID { get; set; }

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