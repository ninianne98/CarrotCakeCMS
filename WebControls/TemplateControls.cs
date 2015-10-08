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
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.Web.UI.Controls {

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

		[Category("Appearance")]
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue(false)]
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

		[Category("Appearance")]
		[DefaultValue(true)]
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

		[Category("Appearance")]
		[DefaultValue(null)]
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
			if (!String.IsNullOrEmpty(CssClassNormal) || !String.IsNullOrEmpty(CSSSelected)) {
				string sCSS = "";
				string sSelCss = "";

				if (IsSelected) {
					sSelCss = CSSSelected.Trim();
				}

				sCSS = String.Format("{0} {1}", CssClassNormal.Trim(), sSelCss);

				lnk.CssClass = sCSS.Trim();
			}
		}

		private string _linkNavURL = string.Empty;
		private string _linkTextDefault = string.Empty;

		private HyperLink lnk = new HyperLink();

		protected override void OnDataBinding(EventArgs e) {
			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			_linkNavURL = DataBinder.Eval(container, "DataItem.FileName").ToString();
			_linkTextDefault = DataBinder.Eval(container, "DataItem.NavMenuText").ToString();

			Guid pageID = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());

			this.ContentID = pageID;

			LoadCtrsl();

			base.OnDataBinding(e);

			AssignVals();
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);
		}

		private void AssignVals() {
			SetCSS();

			if (!String.IsNullOrEmpty(_linkNavURL) && String.IsNullOrEmpty(this.NavigateUrl)) {
				this.NavigateUrl = _linkNavURL;
			}

			if (!String.IsNullOrEmpty(_linkTextDefault) && String.IsNullOrEmpty(this.LinkText)) {
				if (UseDefaultText) {
					this.LinkText = _linkTextDefault;
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
		}
	}

	//========================================

	[ToolboxData("<{0}:ListItemWrapper runat=server></{0}:ListItemWrapper>")]
	public class ListItemWrapper : Control, IActivateNavItem {

		[Category("Appearance")]
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue(false)]
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

		[Category("Appearance")]
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue(null)]
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

		[Category("Appearance")]
		[DefaultValue("li")]
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

			if (!String.IsNullOrEmpty(CssClassNormal) || !String.IsNullOrEmpty(CSSSelected)) {
				string sCSS = "";
				string sSelCss = "";

				if (IsSelected) {
					sSelCss = CSSSelected.Trim();
				}

				if (!String.IsNullOrEmpty(CssClassNormal) || !String.IsNullOrEmpty(sSelCss)) {
					sCSS = String.Format(" class=\"{0} {1}\"", CssClassNormal.Trim(), sSelCss);
				}

				litOpen.Text = HtmlTextWriter.TagLeftChar + HtmlTagName + sCSS + HtmlTextWriter.TagRightChar;
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

	//================================================
	[ToolboxData("<{0}:NavPageNumberDisplay runat=server></{0}:NavPageNumberDisplay>")]
	public class NavPageNumberDisplay : Control {

		[Category("Appearance")]
		[DefaultValue(0)]
		public int PageNumber {
			get {
				int s = 0;
				try { s = int.Parse(ViewState["PageNumber"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["PageNumber"] = value.ToString();
			}
		}

		private Literal litPageNbr = new Literal();

		private void LoadCtrsl() {
			litPageNbr.Text = PageNumber.ToString();

			this.Controls.Clear();
			this.Controls.Add(litPageNbr);
		}

		protected override void OnDataBinding(EventArgs e) {
			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			int PageNbr = int.Parse(DataBinder.Eval(container, "DataItem").ToString());

			this.PageNumber = PageNbr;

			LoadCtrsl();

			base.OnDataBinding(e);
		}
	}

	//========================================

	[ToolboxData("<{0}:ListItemWrapperForPager runat=server></{0}:ListItemWrapperForPager>")]
	public class ListItemWrapperForPager : Control, IActivatePageNavItem {

		[Category("Appearance")]
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue(false)]
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

		[Category("Appearance")]
		[DefaultValue(0)]
		public int PageNumber {
			get {
				int s = 0;
				try { s = int.Parse(ViewState["PageNumber"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["PageNumber"] = value.ToString();
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

			if (!String.IsNullOrEmpty(CssClassNormal) || !String.IsNullOrEmpty(CSSSelected)) {
				string sCSS = "";
				string sSelCss = "";

				if (IsSelected) {
					sSelCss = CSSSelected.Trim();
				}

				if (!String.IsNullOrEmpty(CssClassNormal) || !String.IsNullOrEmpty(sSelCss)) {
					sCSS = String.Format(" class=\"{0} {1}\"", CssClassNormal.Trim(), sSelCss);
				}

				litOpen.Text = HtmlTextWriter.TagLeftChar + HtmlTagName + sCSS + HtmlTextWriter.TagRightChar;
			}
		}

		protected override void OnDataBinding(EventArgs e) {
			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			int PageNbr = int.Parse(DataBinder.Eval(container, "DataItem").ToString());

			this.PageNumber = PageNbr;

			SetTag();

			LoadCtrsl();

			base.OnDataBinding(e);
		}
	}

	//========================================
	[ToolboxData("<{0}:NavLinkForPagerTemplate runat=server></{0}:NavLinkForPagerTemplate>")]
	public class NavLinkForPagerTemplate : Control, IActivatePageNavItem {
		private string _linkTextDefault = string.Empty;

		private string _linkText = string.Empty;

		public string LinkText {
			get {
				return _linkText;
			}

			set {
				_linkText = value;
				SetText();
			}
		}

		private string _toolTip = string.Empty;

		public string ToolTip {
			get {
				return _toolTip;
			}

			set {
				_toolTip = value;
				SetText();
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue(false)]
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

		[Category("Appearance")]
		[DefaultValue(true)]
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

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool RenderAsHyperlink {
			get {
				bool s = false;
				if (ViewState["RenderAsHyperlink"] != null) {
					try { s = (bool)ViewState["RenderAsHyperlink"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["RenderAsHyperlink"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(0)]
		public int PageNumber {
			get {
				int s = 0;
				try { s = int.Parse(ViewState["PageNumber"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["PageNumber"] = value.ToString();
			}
		}

		private void SetText() {
			lnkNav.Text = _linkText;
			lnkBtn.Text = _linkText;
			lnkNav.ToolTip = _toolTip;
			lnkBtn.ToolTip = _toolTip;
		}

		private void SetCSS() {
			if (!String.IsNullOrEmpty(CssClassNormal) || !String.IsNullOrEmpty(CSSSelected)) {
				string sCSS = "";
				string sSelCss = "";

				if (IsSelected) {
					sSelCss = CSSSelected.Trim();
				}

				sCSS = String.Format("{0} {1}", CssClassNormal, sSelCss);

				lnkBtn.CssClass = sCSS.Trim();
				lnkNav.CssClass = sCSS.Trim();
			}
		}

		private string sBtnName = "lnkPagerBtn";

		private LinkButton lnkBtn = new LinkButton();
		private HyperLink lnkNav = new HyperLink();

		protected override void OnDataBinding(EventArgs e) {
			string sPageParm = "PageNbr";

			RepeaterItem container = (RepeaterItem)this.NamingContainer;
			Repeater repeater = (Repeater)container.NamingContainer;

			if (repeater != null && repeater.Parent != null) {
				sPageParm = repeater.Parent.ID.ToString() + "Nbr";
			}

			int PageNbr = int.Parse(DataBinder.Eval(container, "DataItem").ToString());

			lnkBtn.ID = sBtnName + PageNbr.ToString();
			lnkBtn.Click += new EventHandler(this.lnkBtn_Click);

			lnkNav.ID = sBtnName + "Lnk" + PageNbr.ToString();

			_linkTextDefault = PageNbr.ToString();
			this.PageNumber = PageNbr;

			if (!String.IsNullOrEmpty(_linkTextDefault) && String.IsNullOrEmpty(this.LinkText)) {
				if (UseDefaultText) {
					_linkText = _linkTextDefault;
				}
			}
			SetText();

			HttpContext context = HttpContext.Current;
			if (context != null) {
				string sSearch = "";
				if (context.Request[BasicControlUtils.SearchQueryParameter] != null) {
					sSearch = context.Request[BasicControlUtils.SearchQueryParameter].ToString();
					lnkNav.NavigateUrl = BasicControlUtils.CurrentScriptName + "?" + sPageParm + "=" + PageNbr.ToString() + "&" + BasicControlUtils.SearchQueryParameter + "=" + context.Server.UrlEncode(sSearch);
				} else {
					lnkNav.NavigateUrl = BasicControlUtils.CurrentScriptName + "?" + sPageParm + "=" + PageNbr.ToString();
				}
			}

			LoadCtrsl();

			base.OnDataBinding(e);
		}

		protected void lnkBtn_Click(object sender, EventArgs e) {
		}

		protected override void OnPreRender(EventArgs e) {
			SetCSS();

			base.OnPreRender(e);
		}

		private void LoadCtrsl() {
			int iMax = this.Controls.Count;
			lnkBtn.Controls.Clear();
			lnkNav.Controls.Clear();

			if (RenderAsHyperlink) {
				for (int i = 0; i < iMax; i++) {
					lnkNav.Controls.Add(this.Controls[0]);
				}
				this.Controls.Add(lnkNav);
			} else {
				for (int i = 0; i < iMax; i++) {
					lnkBtn.Controls.Add(this.Controls[0]);
				}
				this.Controls.Add(lnkBtn);
			}
		}
	}

	//========================================
	public class ListItemRepeater : Repeater {
	}

	//========================================
	public class ListItemPlaceHolder : PlaceHolder {

		protected override void RenderChildren(HtmlTextWriter writer) {
			int indent1 = writer.Indent;

			foreach (Control c in this.Controls) {
				writer.Indent = indent1;
				if (c is ListItemRepeater) {
					writer.Write("\t");
					writer.Write(BasicControlUtils.GetCtrlText(c).Replace("\r\n", "\r\n\t\t"));
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

		public DefaultListOpenNavTemplate() { }

		public void InstantiateIn(Control container) {
			Literal litL = new Literal();

			litL.Text = HtmlTextWriter.TagLeftChar + "ul" + HtmlTextWriter.TagRightChar;

			container.Controls.Add(litL);
		}
	}

	//========================================
	public class DefaultListCloseNavTemplate : ITemplate {

		public DefaultListCloseNavTemplate() { }

		public void InstantiateIn(Control container) {
			Literal litL = new Literal();
			litL.Text = HtmlTextWriter.EndTagLeftChars + "ul" + HtmlTextWriter.TagRightChar;

			container.Controls.Add(litL);
		}
	}

	//========================================
	public class DefaultLinkNavTemplate : ITemplate {

		public DefaultLinkNavTemplate() { }

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
	public class DefaultPagerTemplate : ITemplate {

		public DefaultPagerTemplate() { }

		private Control GetCtrl(Control X) {
			cu = new BasicControlUtils(X);
			Control userControl = cu.CreateControlFromResource("Carrotware.Web.UI.Controls.ucSimplePager1.ascx");

			return userControl;
		}

		private BasicControlUtils cu = new BasicControlUtils();

		public void InstantiateIn(Control container) {
			PlaceHolder ph = new PlaceHolder();
			container.Controls.Add(ph);

			Control c = GetCtrl(ph);

			NavLinkForPagerTemplate lnkBtn = (NavLinkForPagerTemplate)cu.FindControl("lnkBtn", c);

			lnkBtn.DataBinding += new EventHandler(lnkBtn_DataBinding);

			ph.Controls.Add(c);

			//Literal litL = new Literal();
			//litL.Text = " [ ";
			//Literal litR = new Literal();
			//litR.Text = " ]   ";

			//NavLinkForPagerTemplate lnkBtn = new NavLinkForPagerTemplate();
			//lnkBtn.ID = "lnkBtn";
			//lnkBtn.CSSSelected = "selected";

			//lnkBtn.DataBinding += new EventHandler(lnkBtn_DataBinding);

			//container.Controls.Add(litL);
			//container.Controls.Add(lnkBtn);
			//container.Controls.Add(litR);
		}

		private void lnkBtn_DataBinding(object sender, EventArgs e) {
			NavLinkForPagerTemplate lnkBtn = (NavLinkForPagerTemplate)sender;
			RepeaterItem container = (RepeaterItem)lnkBtn.NamingContainer;

			string sTxt = DataBinder.Eval(container, "DataItem").ToString();
			lnkBtn.LinkText = sTxt;
			lnkBtn.PageNumber = int.Parse(sTxt);
		}
	}

	//========================================
	public interface IActivatePageNavItem {
		string CSSSelected { get; set; }

		string CssClassNormal { get; set; }

		bool IsSelected { get; set; }

		int PageNumber { get; set; }
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