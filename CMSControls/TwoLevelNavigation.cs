using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
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

	[ToolboxData("<{0}:TwoLevelNavigation runat=server></{0}:TwoLevelNavigation>")]
	public class TwoLevelNavigation : BaseServerControl, IWidgetLimitedProperties {

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
		public string ExtraCSS {
			get {
				string s = (string)ViewState["ExtraCSS"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["ExtraCSS"] = value;
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

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSItem {
			get {
				string s = (string)ViewState["CSSItem"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSItem"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSULClassTop {
			get {
				string s = (string)ViewState["ULClassTop"];
				return ((s == null) ? "parent" : s);
			}
			set {
				ViewState["ULClassTop"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSULClassLower {
			get {
				string s = (string)ViewState["ULClassLower"];
				return ((s == null) ? "children" : s);
			}
			set {
				ViewState["ULClassLower"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSHasChildren {
			get {
				string s = (string)ViewState["CSSHasChildren"];
				return ((s == null) ? "sub" : s);
			}
			set {
				ViewState["CSSHasChildren"] = value;
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


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool WrapList {
			get {
				String s = (String)ViewState["WrapList"];
				return ((s == null) ? true : Convert.ToBoolean(s));
			}

			set {
				ViewState["WrapList"] = value.ToString();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool AutoStylingDisabled {
			get {
				String s = (String)ViewState["AutoStylingDisabled"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}

			set {
				ViewState["AutoStylingDisabled"] = value.ToString();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		[Obsolete("This property is obsolete, do not use.")]
		public Unit MenuWidth {
			get {
				Unit s = new Unit("940px");
				if (ViewState["MenuWidth"] != null) {
					try { s = new Unit(ViewState["MenuWidth"].ToString()); } catch { }
				}
				return s;
			}
			set {
				ViewState["MenuWidth"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Unit FontSize {
			get {
				Unit s = new Unit("14px");
				if (ViewState["FontSize"] != null) {
					try { s = new Unit(ViewState["FontSize"].ToString()); } catch { }
				}
				return s;
			}
			set {
				ViewState["FontSize"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		[Obsolete("This property is obsolete, do not use.")]
		public Unit MenuHeight {
			get {
				Unit s = new Unit("60px");
				if (ViewState["MenuHeight"] != null) {
					try { s = new Unit(ViewState["MenuHeight"].ToString()); } catch { }
				}
				return s;
			}
			set {
				ViewState["MenuHeight"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		[Obsolete("This property is obsolete, do not use.")]
		public Unit SubMenuWidth {
			get {
				Unit s = new Unit("300px");
				if (ViewState["SubMenuWidth"] != null) {
					try { s = new Unit(ViewState["SubMenuWidth"].ToString()); } catch { }
				}
				return s;
			}
			set {
				ViewState["SubMenuWidth"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string TopBackgroundStyle {
			get {
				String s = (String)ViewState["TopBackgroundStyle"];
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["TopBackgroundStyle"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		[Obsolete("This property is obsolete, do not use.")]
		public string ItemBackgroundStyle {
			get {
				String s = (String)ViewState["ItemBackgroundStyle"];
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["ItemBackgroundStyle"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public override Color ForeColor {
			get {
				string s = (string)ViewState["ForeColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#758569") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["ForeColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public override Color BackColor {
			get {
				string s = (string)ViewState["BackColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#DDDDDD") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["BackColor"] = ColorTranslator.ToHtml(value);
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color HoverBGColor {
			get {
				string s = (string)ViewState["HoverBGColor"];
				return ((s == null) ? ForeColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["HoverBGColor"] = ColorTranslator.ToHtml(value);
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color HoverFGColor {
			get {
				string s = (string)ViewState["HoverFGColor"];
				return ((s == null) ? BackColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["HoverFGColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color UnSelBGColor {
			get {
				string s = (string)ViewState["UnSelBGColor"];
				return ((s == null) ? ColorTranslator.FromHtml("Transparent") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["UnSelBGColor"] = ColorTranslator.ToHtml(value);
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color UnSelFGColor {
			get {
				string s = (string)ViewState["UnSelFGColor"];
				return ((s == null) ? ForeColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["UnSelFGColor"] = ColorTranslator.ToHtml(value);
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color SelBGColor {
			get {
				string s = (string)ViewState["SelBGColor"];
				return ((s == null) ? ForeColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["SelBGColor"] = ColorTranslator.ToHtml(value);
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color SelFGColor {
			get {
				string s = (string)ViewState["SelFGColor"];
				return ((s == null) ? BackColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["SelFGColor"] = ColorTranslator.ToHtml(value);
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color SubBGColor {
			get {
				string s = (string)ViewState["SubBGColor"];
				return ((s == null) ? BackColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["SubBGColor"] = ColorTranslator.ToHtml(value);
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color SubFGColor {
			get {
				string s = (string)ViewState["SubFGColor"];
				return ((s == null) ? ForeColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["SubFGColor"] = ColorTranslator.ToHtml(value);
			}
		}


		private List<SiteNav> lstTwoLevelNav = new List<SiteNav>();

		protected List<SiteNav> GetTopNav() {
			return lstTwoLevelNav.Where(ct => ct.Parent_ContentID == null).OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected List<SiteNav> GetChildren(Guid rootContentID) {
			return lstTwoLevelNav.Where(ct => ct.Parent_ContentID == rootContentID).OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected void LoadData() {

			lstTwoLevelNav = navHelper.GetTwoLevelNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);

			lstTwoLevelNav.RemoveAll(x => x.ShowInSiteNav == false);
			lstTwoLevelNav.ToList().ForEach(q => IdentifyLinkAsInactive(q));
		}

		private Literal cssText = new Literal();

		protected override void OnInit(EventArgs e) {
			this.Controls.Clear();

			base.OnInit(e);

			LoadData();

			this.Page.Header.Controls.Add(cssText);
		}


		protected override void RenderContents(HtmlTextWriter output) {

			ParseCSS();

			if (HttpContext.Current == null) {
				cssText.RenderControl(output);
				output.Write(GetCtrlText(cssText));
			}

			int indent = output.Indent;
			output.Indent = indent + 2;

			List<SiteNav> lstNav = GetTopNav();
			SiteNav pageNav = GetParentPage();

			string sParent = pageNav.FileName.ToLower();

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = string.Format(" class=\"{0}\"", CssClass);
			}

			string sItemCSS = "";
			if (!string.IsNullOrEmpty(CSSItem)) {
				sItemCSS = string.Format(" {0} ", CSSItem);
			}

			output.WriteLine();

			if (WrapList) {
				output.WriteLine("<div" + sCSS + " id=\"" + this.ClientID + "\">");
				output.Indent++;
				output.WriteLine("<div id=\"" + this.ClientID + "-inner\">");
				output.Indent++;
			}

			if (!WrapList) {
				output.WriteLine("<ul id=\"" + this.ClientID + "\" class=\"" + CSSULClassTop + " " + CssClass + "\">");
			} else {
				output.WriteLine("<ul class=\"" + CSSULClassTop + "\">");
			}
			output.Indent++;

			int indent2 = output.Indent;

			foreach (SiteNav c1 in lstNav) {
				output.Indent = indent2;
				List<SiteNav> cc = GetChildren(c1.Root_ContentID);
				string sChild = " ";
				if (cc != null && cc.Count > 0) {
					sChild = " level1-haschildren " + CSSHasChildren + " ";
				}

				if (SiteData.IsFilenameCurrentPage(c1.FileName) || AreFilenamesSame(c1.FileName, sParent)) {
					output.WriteLine("<li class=\"level1 " + sItemCSS + CSSSelected + sChild + "\"><a href=\"" + c1.FileName + "\">" + c1.NavMenuText + "</a>");
				} else {
					output.WriteLine("<li class=\"level1 " + sItemCSS + sChild + "\"><a href=\"" + c1.FileName + "\">" + c1.NavMenuText + "</a>");
				}

				output.Indent++;
				if (cc != null && cc.Count > 0) {
					int indent3 = output.Indent;
					output.WriteLine("<ul class=\"" + CSSULClassLower + "\">");
					output.Indent++;
					foreach (SiteNav c2 in cc) {
						if (SiteData.IsFilenameCurrentPage(c2.FileName)) {
							output.WriteLine("<li class=\"level2 " + sItemCSS + CSSSelected + "\"><a href=\"" + c2.FileName + "\">" + c2.NavMenuText + "</a></li>");
						} else {
							output.WriteLine("<li class=\"level2 " + sItemCSS + "\"><a href=\"" + c2.FileName + "\">" + c2.NavMenuText + "</a></li>");
						}
					}
					output.Indent = indent3;
					output.WriteLine("</ul>");
				}
				output.Indent--;
				output.WriteLine("</li>");
			}

			output.Indent--;
			output.WriteLine("</ul>");

			if (WrapList) {
				output.Indent--;
				output.WriteLine("</div>");
				output.Indent--;
				output.WriteLine("</div>");
			}

			output.WriteLine();

			output.Indent = indent;
		}

		public List<string> LimitedPropertyList {
			get {
				List<string> lst = new List<string>();
				lst.Add("OverrideCSS");
				lst.Add("ExtraCSS");
				lst.Add("AutoStylingDisabled");
				lst.Add("CSSSelected");
				lst.Add("CSSHasChildren");
				lst.Add("WrapList");
				lst.Add("FontSize");
				lst.Add("TopBackgroundStyle");
				lst.Add("ForeColor");
				lst.Add("BackColor");
				lst.Add("HoverFGColor");
				lst.Add("HoverBGColor");
				lst.Add("UnSelFGColor");
				lst.Add("UnSelBGColor");
				lst.Add("SelFGColor");
				lst.Add("SelBGColor");
				lst.Add("SubFGColor");
				lst.Add("SubBGColor");
				return lst;
			}
		}

		protected override void OnPreRender(EventArgs e) {
			try {

				if (PublicParmValues.Count > 0) {

					string sTmp = "";

					OverrideCSS = GetParmValue("OverrideCSS", "");
					ExtraCSS = GetParmValue("ExtraCSS", "");

					sTmp = GetParmValue("AutoStylingDisabled", "false");
					if (!string.IsNullOrEmpty(sTmp)) {
						AutoStylingDisabled = Convert.ToBoolean(sTmp);
					}

					sTmp = GetParmValue("CSSSelected", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						CSSSelected = sTmp;
					}

					sTmp = GetParmValue("CSSHasChildren", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						CSSHasChildren = sTmp;
					}

					sTmp = GetParmValue("WrapList", "false");
					if (!string.IsNullOrEmpty(sTmp)) {
						WrapList = Convert.ToBoolean(sTmp);
					}

					if (!AutoStylingDisabled) {
						sTmp = GetParmValue("FontSize", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							FontSize = new Unit(sTmp);
						}

						sTmp = GetParmValue("TopBackgroundStyle", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							TopBackgroundStyle = sTmp;
						}

						sTmp = GetParmValue("ForeColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							ForeColor = ColorTranslator.FromHtml(sTmp);
						}
						sTmp = GetParmValue("BackColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							BackColor = ColorTranslator.FromHtml(sTmp);
						}

						sTmp = GetParmValue("HoverFGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							HoverFGColor = ColorTranslator.FromHtml(sTmp);
						}
						sTmp = GetParmValue("HoverBGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							HoverBGColor = ColorTranslator.FromHtml(sTmp);
						}

						sTmp = GetParmValue("UnSelFGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							UnSelFGColor = ColorTranslator.FromHtml(sTmp);
						}
						sTmp = GetParmValue("UnSelBGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							UnSelBGColor = ColorTranslator.FromHtml(sTmp);
						}

						sTmp = GetParmValue("SelFGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							SelFGColor = ColorTranslator.FromHtml(sTmp);
						}
						sTmp = GetParmValue("SelBGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							SelBGColor = ColorTranslator.FromHtml(sTmp);
						}

						sTmp = GetParmValue("SubFGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							SubFGColor = ColorTranslator.FromHtml(sTmp);
						}
						sTmp = GetParmValue("SubBGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							SubBGColor = ColorTranslator.FromHtml(sTmp);
						}
					}
				}
			} catch (Exception ex) {
			}


			if (string.IsNullOrEmpty(OverrideCSS) && !AutoStylingDisabled) {

				ParseCSS();

				//BasicControlUtils.MakeXUACompatibleFirst(this.Page);

			} else {
				if (!string.IsNullOrEmpty(OverrideCSS)) {
					HtmlLink link = new HtmlLink();
					link.Href = OverrideCSS;
					link.Attributes.Add("rel", "stylesheet");
					link.Attributes.Add("type", "text/css");
					this.Page.Header.Controls.Add(link);
				}
			}


			if (!string.IsNullOrEmpty(ExtraCSS)) {
				HtmlLink link = new HtmlLink();
				link.Href = ExtraCSS;
				link.Attributes.Add("rel", "stylesheet");
				link.Attributes.Add("type", "text/css");
				this.Page.Header.Controls.Add(link);
			}

			if (!AutoStylingDisabled) {
				WrapList = false;
			}

			base.OnPreRender(e);
		}


		private void ParseCSS() {
			if (string.IsNullOrEmpty(OverrideCSS) && !AutoStylingDisabled && string.IsNullOrEmpty(cssText.Text)) {
				string sCSSText = ControlUtilities.GetManifestResourceStream("Carrotware.CMS.UI.Controls.TopMenu.txt");

				if (sCSSText != null) {
					sCSSText = sCSSText.Replace("{FORE_HEX}", ColorTranslator.ToHtml(ForeColor));
					sCSSText = sCSSText.Replace("{BG_HEX}", ColorTranslator.ToHtml(BackColor));

					sCSSText = sCSSText.Replace("{HOVER_FORE_HEX}", ColorTranslator.ToHtml(HoverFGColor));
					sCSSText = sCSSText.Replace("{HOVER_BG_HEX}", ColorTranslator.ToHtml(HoverBGColor));

					sCSSText = sCSSText.Replace("{SEL_FORE_HEX}", ColorTranslator.ToHtml(SelFGColor));
					sCSSText = sCSSText.Replace("{SEL_BG_HEX}", ColorTranslator.ToHtml(SelBGColor));

					sCSSText = sCSSText.Replace("{UNSEL_FORE_HEX}", ColorTranslator.ToHtml(UnSelFGColor));
					sCSSText = sCSSText.Replace("{UNSEL_BG_HEX}", ColorTranslator.ToHtml(UnSelBGColor));

					sCSSText = sCSSText.Replace("{SUB_FORE_HEX}", ColorTranslator.ToHtml(SubFGColor));
					sCSSText = sCSSText.Replace("{SUB_BG_HEX}", ColorTranslator.ToHtml(SubBGColor));

					sCSSText = sCSSText.Replace("{FONT_SIZE}", FontSize.Value.ToString() + "px");

					sCSSText = sCSSText.Replace("{MENU_SELECT_CLASS}", CSSSelected);
					sCSSText = sCSSText.Replace("{MENU_HASCHILD_CLASS}", CSSHasChildren);

					if (!string.IsNullOrEmpty(TopBackgroundStyle)) {
						TopBackgroundStyle = TopBackgroundStyle.Replace(";", "");
						sCSSText = sCSSText.Replace("{TOP_BACKGROUND_STYLE}", "background: " + TopBackgroundStyle + ";");
					} else {
						sCSSText = sCSSText.Replace("{TOP_BACKGROUND_STYLE}", "");
					}

					sCSSText = sCSSText.Replace("{MENU_ID}", "#" + this.ClientID + "");
					sCSSText = sCSSText.Replace("{MENU_WRAPPER_ID}", "#" + this.ClientID + "-wrapper");
					sCSSText = "\r\n\t<style type=\"text/css\">\r\n" + sCSSText + "\r\n\t</style>\r\n";
					//sCSSText = "\r\n\t<div type=\"text/css\">\r\n" + sCSSText + "\r\n\t</div>\r\n";
					cssText.Text = sCSSText;
				}
			}
		}


	}
}
