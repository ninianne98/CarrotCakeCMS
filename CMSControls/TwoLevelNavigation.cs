using Carrotware.CMS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
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

	[ToolboxData("<{0}:TwoLevelNavigation runat=server></{0}:TwoLevelNavigation>")]
	public class TwoLevelNavigation : BaseNavSel {

		[Category("Appearance")]
		[DefaultValue(true)]
		public override bool MultiLevel {
			get {
				return true;
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
		public string ExtraCSS {
			get {
				string s = (string)ViewState["ExtraCSS"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["ExtraCSS"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool InsertCssEarly {
			get {
				String s = (String)ViewState["InsertCssEarly"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}

			set {
				ViewState["InsertCssEarly"] = value.ToString();
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

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool AutoStylingDisabled {
			get {
				String s = (String)ViewState["AutoStylingDisabled"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}

			set {
				ViewState["AutoStylingDisabled"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		private bool AttemptResponsiveCSS {
			get {
				String s = (String)ViewState["AttemptResponsiveCSS"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}

			set {
				ViewState["AttemptResponsiveCSS"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		private Unit MobileWidth {
			get {
				Unit s = new Unit("575px");
				if (ViewState["MobileWidth"] != null) {
					try { s = new Unit(ViewState["MobileWidth"].ToString()); } catch { }
				}
				return s;
			}
			set {
				ViewState["MobileWidth"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
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

		[Category("Appearance")]
		[DefaultValue(null)]
		public Unit MenuFontSize {
			get {
				Unit s = this.FontSize;
				if (ViewState["MenuFontSize"] != null) {
					try { s = new Unit(ViewState["MenuFontSize"].ToString()); } catch { }
				}
				return s;
			}
			set {
				ViewState["MenuFontSize"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string TopBackgroundStyle {
			get {
				String s = (String)ViewState["TopBackgroundStyle"];
				return ((s == null) ? string.Empty : s);
			}
			set {
				ViewState["TopBackgroundStyle"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public override Color ForeColor {
			get {
				string s = (string)ViewState["ForeColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#758569") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["ForeColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public Color BGColor {
			get {
				string s = (string)ViewState["BGColor"];
				return ((s == null) ? Color.Transparent : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["BGColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public override Color BackColor {
			get {
				string s = (string)ViewState["BackColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#DDDDDD") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["BackColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color HoverBGColor {
			get {
				string s = (string)ViewState["HoverBGColor"];
				return ((s == null) ? ForeColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["HoverBGColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color HoverFGColor {
			get {
				string s = (string)ViewState["HoverFGColor"];
				return ((s == null) ? BackColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["HoverFGColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public Color UnSelBGColor {
			get {
				string s = (string)ViewState["UnSelBGColor"];
				return ((s == null) ? ColorTranslator.FromHtml("Transparent") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["UnSelBGColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color UnSelFGColor {
			get {
				string s = (string)ViewState["UnSelFGColor"];
				return ((s == null) ? ForeColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["UnSelFGColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color SelBGColor {
			get {
				string s = (string)ViewState["SelBGColor"];
				return ((s == null) ? ForeColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["SelBGColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color SelFGColor {
			get {
				string s = (string)ViewState["SelFGColor"];
				return ((s == null) ? BackColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["SelFGColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color SubBGColor {
			get {
				string s = (string)ViewState["SubBGColor"];
				return ((s == null) ? BackColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["SubBGColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color SubFGColor {
			get {
				string s = (string)ViewState["SubFGColor"];
				return ((s == null) ? ForeColor : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["SubFGColor"] = ColorTranslator.ToHtml(value);
			}
		}

		protected override void LoadData() {
			base.LoadData();

			this.NavigationData = navHelper.GetTwoLevelNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);
		}

		private Literal cssText = new Literal();

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			if (cssText != null) {
				if (!this.InsertCssEarly) {
					this.Page.Header.Controls.Add(cssText);
				} else {
					int indexPos = 2;
					if (this.Page.Header.Controls.Count > indexPos) {
						this.Page.Header.Controls.AddAt(indexPos, new Literal { Text = "\r\n" });
						this.Page.Header.Controls.AddAt(indexPos, cssText);
					}
				}
			}
		}

		protected override void WriteListPrefix(HtmlTextWriter output) {
			if (!SiteData.IsWebView) {
				cssText.RenderControl(output);
				output.Write(GetCtrlText(cssText));
			}

			string sCSSWrap = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSSWrap = string.Format(" class=\"{0}\"", CssClass);
			}

			if (WrapList) {
				output.WriteLine("<div" + sCSSWrap + " id=\"" + this.HtmlClientID + "\">");
				output.Indent++;
				output.WriteLine("<div id=\"" + this.HtmlClientID + "-inner\">");
				output.Indent++;
			}

			string sCSSList = this.CSSULClassTop.Trim();
			string sLstID = this.HtmlClientID + "-list";

			if (!WrapList) {
				sCSSList = (this.CSSULClassTop + " " + this.CssClass).Trim();
				sLstID = this.HtmlClientID;
			}

			if (!string.IsNullOrEmpty(sCSSList)) {
				output.WriteLine("<ul id=\"" + sLstID + "\" class=\"" + sCSSList + "\">");
			} else {
				output.WriteLine("<ul id=\"" + sLstID + "\" >");
			}
		}

		protected override void WriteListSuffix(HtmlTextWriter output) {
			output.WriteLine("</ul>");

			if (WrapList) {
				output.Indent--;
				output.WriteLine("</div>");
				output.Indent--;
				output.WriteLine("</div>");
			}
		}

		public override List<string> LimitedPropertyList {
			get {
				List<string> lst = base.LimitedPropertyList;
				lst.Add("OverrideCSS");
				lst.Add("CssClass");
				lst.Add("AutoStylingDisabled");
				lst.Add("WrapList");
				lst.Add("CSSSelected");
				lst.Add("CSSHasChildren");
				lst.Add("FontSize");
				lst.Add("ForeColor");
				lst.Add("BackColor");

				return lst.Distinct().ToList();
			}
		}

		protected override void OnPreRender(EventArgs e) {
			try {
				if (this.PublicParmValues.Count > 0) {
					string sTmp = "";

					this.OverrideCSS = GetParmValue("OverrideCSS", "");
					this.ExtraCSS = GetParmValue("ExtraCSS", "");

					sTmp = GetParmValue("AutoStylingDisabled", "false");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.AutoStylingDisabled = Convert.ToBoolean(sTmp);
					}

					sTmp = GetParmValue("AttemptResponsiveCSS", "false");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.AttemptResponsiveCSS = Convert.ToBoolean(sTmp);
					}

					sTmp = GetParmValue("WrapList", "false");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.WrapList = Convert.ToBoolean(sTmp);
					}

					if (!this.AutoStylingDisabled) {
						sTmp = GetParmValue("FontSize", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							FontSize = new Unit(sTmp);
						}

						sTmp = GetParmValue("TopBackgroundStyle", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							this.TopBackgroundStyle = sTmp;
						}

						sTmp = GetParmValue("ForeColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							this.ForeColor = ColorTranslator.FromHtml(sTmp);
						}
						sTmp = GetParmValue("BackColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							this.BackColor = ColorTranslator.FromHtml(sTmp);
						}
						sTmp = GetParmValue("BGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							this.BGColor = ColorTranslator.FromHtml(sTmp);
						}
						sTmp = GetParmValue("HoverFGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							this.HoverFGColor = ColorTranslator.FromHtml(sTmp);
						}
						sTmp = GetParmValue("HoverBGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							this.HoverBGColor = ColorTranslator.FromHtml(sTmp);
						}

						sTmp = GetParmValue("UnSelFGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							this.UnSelFGColor = ColorTranslator.FromHtml(sTmp);
						}
						sTmp = GetParmValue("UnSelBGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							this.UnSelBGColor = ColorTranslator.FromHtml(sTmp);
						}

						sTmp = GetParmValue("SelFGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							this.SelFGColor = ColorTranslator.FromHtml(sTmp);
						}
						sTmp = GetParmValue("SelBGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							this.SelBGColor = ColorTranslator.FromHtml(sTmp);
						}

						sTmp = GetParmValue("SubFGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							this.SubFGColor = ColorTranslator.FromHtml(sTmp);
						}
						sTmp = GetParmValue("SubBGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							this.SubBGColor = ColorTranslator.FromHtml(sTmp);
						}
					}
				}
			} catch (Exception ex) {
			}

			if (string.IsNullOrEmpty(this.OverrideCSS) && !this.AutoStylingDisabled) {
				ParseCSS();
			} else {
				if (!string.IsNullOrEmpty(this.OverrideCSS)) {
					HtmlLink link = new HtmlLink();
					link.Href = OverrideCSS;
					link.Attributes.Add("rel", "stylesheet");
					link.Attributes.Add("type", "text/css");
					this.Page.Header.Controls.Add(link);
				}
			}

			if (!string.IsNullOrEmpty(this.ExtraCSS)) {
				HtmlLink link = new HtmlLink();
				link.Href = ExtraCSS;
				link.Attributes.Add("rel", "stylesheet");
				link.Attributes.Add("type", "text/css");
				this.Page.Header.Controls.Add(link);
			}

			if (!this.AutoStylingDisabled) {
				this.WrapList = false;
			}

			base.OnPreRender(e);
		}

		private string ConvertColorCode(Color color) {
			return ColorTranslator.ToHtml(color).ToLowerInvariant();
		}

		private void ParseCSS() {
			if (string.IsNullOrEmpty(this.OverrideCSS) && !this.AutoStylingDisabled && string.IsNullOrEmpty(cssText.Text)) {
				var sbCSSText = new StringBuilder();
				sbCSSText.Append(ControlUtilities.GetManifestResourceStream("Carrotware.CMS.UI.Controls.TopMenu.txt"));

				if (this.AttemptResponsiveCSS && sbCSSText.Length > 1) {
					var sbCSS1 = new StringBuilder();
					sbCSS1.Append(ControlUtilities.GetManifestResourceStream("Carrotware.CMS.UI.Controls.TopMenuRes.txt"));
					sbCSS1.Replace("{DESKTOP_CSS}", sbCSSText.ToString());

					sbCSSText = sbCSS1;
				}

				if (sbCSSText != null && sbCSSText.Length > 1) {
					sbCSSText.Replace("{FORE_HEX}", ConvertColorCode(this.ForeColor));
					sbCSSText.Replace("{BG_HEX}", ConvertColorCode(this.BGColor));

					sbCSSText.Replace("{HOVER_FORE_HEX}", ConvertColorCode(this.HoverFGColor));
					sbCSSText.Replace("{HOVER_BG_HEX}", ConvertColorCode(this.HoverBGColor));

					sbCSSText.Replace("{SEL_FORE_HEX}", ConvertColorCode(this.SelFGColor));
					sbCSSText.Replace("{SEL_BG_HEX}", ConvertColorCode(this.SelBGColor));

					sbCSSText.Replace("{UNSEL_FORE_HEX}", ConvertColorCode(this.UnSelFGColor));
					sbCSSText.Replace("{UNSEL_BG_HEX}", ConvertColorCode(this.UnSelBGColor));

					sbCSSText.Replace("{SUB_FORE_HEX}", ConvertColorCode(this.SubFGColor));
					sbCSSText.Replace("{SUB_BG_HEX}", ConvertColorCode(this.SubBGColor));

					if (this.FontSize.Value > 0) {
						sbCSSText.Replace("{FONT_SIZE}", this.FontSize.ToString());
					} else {
						sbCSSText.Replace("{FONT_SIZE}", "inherit");
					}

					if (this.MenuFontSize.Value > 0) {
						sbCSSText.Replace("{MAIN_FONT_SIZE}", this.MenuFontSize.ToString());
					} else {
						sbCSSText.Replace("{MAIN_FONT_SIZE}", "inherit");
					}

					if (this.MobileWidth.Value > 50) {
						sbCSSText.Replace("{MOBILE_WIDTH}", this.MobileWidth.ToString());
					} else {
						sbCSSText.Replace("{MOBILE_WIDTH}", "100%");
					}

					if (this.MobileWidth.Value > 50) {
						sbCSSText.Replace("{DESK_WIDTH}", (this.MobileWidth.Value + 1).ToString() + this.MobileWidth.Type);
					} else {
						sbCSSText.Replace("{DESK_WIDTH}", "100%");
					}

					sbCSSText.Replace("{MENU_SELECT_CLASS}", this.CSSSelected);
					sbCSSText.Replace("{MENU_HASCHILD_CLASS}", this.CSSHasChildren);

					if (!string.IsNullOrEmpty(this.TopBackgroundStyle)) {
						this.TopBackgroundStyle = this.TopBackgroundStyle.Replace(";", "");
						sbCSSText.Replace("{TOP_BACKGROUND_STYLE}", "background: " + this.TopBackgroundStyle + ";");
					} else {
						sbCSSText.Replace("{TOP_BACKGROUND_STYLE}", "");
					}

					sbCSSText.Replace("{MENU_ID}", "#" + this.HtmlClientID + "");
					sbCSSText.Replace("{MENU_WRAPPER_ID}", "#" + this.HtmlClientID + "-wrapper");

					var sCSSText = "\r\n\t<style type=\"text/css\">\r\n" + sbCSSText.ToString() + "\r\n\t</style>\r\n";
					//sCSSText = "\r\n\t<div type=\"text/css\">\r\n" + sCSSText + "\r\n\t</div>\r\n";
					cssText.Text = sCSSText;
				}
			}
		}
	}
}