using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
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
				return ((s == null) ? string.Empty : s);
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
				return ((s == null) ? string.Empty : s);
			}
			set {
				ViewState["ExtraCSS"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool InsertCssEarly {
			get {
				string s = (string)ViewState["InsertCssEarly"];
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
				string s = (string)ViewState["WrapList"];
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
				string s = (string)ViewState["AutoStylingDisabled"];
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
				string s = (string)ViewState["AttemptResponsiveCSS"];
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
				string s = (string)ViewState["TopBackgroundStyle"];
				return ((s == null) ? string.Empty : s);
			}
			set {
				ViewState["TopBackgroundStyle"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public Color ThemeColor {
			get {
				string s = (string)ViewState["ThemeColor"];
				return ((s == null) ? Color.Transparent : s.DecodeColor());
			}
			set {
				ViewState["ThemeColor"] = ConvertColorCode(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public override Color ForeColor {
			get {
				string s = (string)ViewState["ForeColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#758569") : s.DecodeColor());
			}
			set {
				ViewState["ForeColor"] = ConvertColorCode(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public Color BGColor {
			get {
				string s = (string)ViewState["BGColor"];
				return ((s == null) ? Color.Transparent : s.DecodeColor());
			}
			set {
				ViewState["BGColor"] = ConvertColorCode(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public override Color BackColor {
			get {
				string s = (string)ViewState["BackColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#DDDDDD") : s.DecodeColor());
			}
			set {
				ViewState["BackColor"] = ConvertColorCode(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color HoverBGColor {
			get {
				string s = (string)ViewState["HoverBGColor"];
				return ((s == null) ? this.ForeColor : s.DecodeColor());
			}
			set {
				ViewState["HoverBGColor"] = ConvertColorCode(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color HoverFGColor {
			get {
				string s = (string)ViewState["HoverFGColor"];
				return ((s == null) ? this.BackColor : s.DecodeColor());
			}
			set {
				ViewState["HoverFGColor"] = ConvertColorCode(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public Color UnSelBGColor {
			get {
				string s = (string)ViewState["UnSelBGColor"];
				return ((s == null) ? ColorTranslator.FromHtml("Transparent") : s.DecodeColor());
			}
			set {
				ViewState["UnSelBGColor"] = ConvertColorCode(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color UnSelFGColor {
			get {
				string s = (string)ViewState["UnSelFGColor"];
				return ((s == null) ? this.ForeColor : s.DecodeColor());
			}
			set {
				ViewState["UnSelFGColor"] = ConvertColorCode(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color SelBGColor {
			get {
				string s = (string)ViewState["SelBGColor"];
				return ((s == null) ? this.ForeColor : s.DecodeColor());
			}
			set {
				ViewState["SelBGColor"] = ConvertColorCode(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color SelFGColor {
			get {
				string s = (string)ViewState["SelFGColor"];
				return ((s == null) ? this.BackColor : s.DecodeColor());
			}
			set {
				ViewState["SelFGColor"] = ConvertColorCode(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color SubBGColor {
			get {
				string s = (string)ViewState["SubBGColor"];
				return ((s == null) ? this.BackColor : s.DecodeColor());
			}
			set {
				ViewState["SubBGColor"] = ConvertColorCode(value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Color SubFGColor {
			get {
				string s = (string)ViewState["SubFGColor"];
				return ((s == null) ? this.ForeColor : s.DecodeColor());
			}
			set {
				ViewState["SubFGColor"] = ConvertColorCode(value);
			}
		}

		protected override void LoadData() {
			base.LoadData();

			this.NavigationData = navHelper.GetTwoLevelNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);
		}

		private Literal _cssText = new Literal();

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			if (_cssText != null) {
				if (!this.InsertCssEarly) {
					this.Page.Header.Controls.Add(_cssText);
				} else {
					int indexPos = 2;
					if (this.Page.Header.Controls.Count > indexPos) {
						this.Page.Header.Controls.AddAt(indexPos, new Literal { Text = "\r\n" });
						this.Page.Header.Controls.AddAt(indexPos, _cssText);
					}
				}
			}
		}

		protected override void WriteListPrefix(HtmlTextWriter output) {
			if (!SiteData.IsWebView) {
				_cssText.RenderControl(output);
				output.Write(GetCtrlText(_cssText));
			}

			string sCSSWrap = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSSWrap = string.Format(" class=\"{0}\"", CssClass);
			}

			if (this.WrapList) {
				output.WriteLine("<div" + sCSSWrap + " id=\"" + this.HtmlClientID + "\">");
				output.Indent++;
				output.WriteLine("<div id=\"" + this.HtmlClientID + "-inner\">");
				output.Indent++;
			}

			string sCSSList = this.CSSULClassTop.Trim();
			string sLstID = this.HtmlClientID + "-list";

			if (!this.WrapList) {
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

			if (this.WrapList) {
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
				CreateCssLink();
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

		private void ResetColor() {
			this.FontSize = new Unit("14px");
			this.MenuFontSize = this.FontSize;

			this.BGColor = Color.Transparent;
			this.UnSelBGColor = Color.Transparent;
			this.ForeColor = ColorTranslator.FromHtml("#758569");
			this.BackColor = ColorTranslator.FromHtml("#DDDDDD");

			this.HoverBGColor = Color.Empty;
			this.HoverFGColor = Color.Empty;
			this.UnSelFGColor = Color.Empty;
			this.SelBGColor = Color.Empty;
			this.SelFGColor = Color.Empty;
			this.SubBGColor = Color.Empty;
			this.SubFGColor = Color.Empty;
		}

		public void RestoreNavColors(string f, string tc, string bg, string ubg, string fc, string bc,
					string hbc, string hfc, string uf, string sbg, string sfg,
					string bc2, string fc2) {
			this.RenderHTMLWithID = true;
			ResetColor();

			var themeColor = tc.DecodeColor();

			this.ThemeColor = themeColor;

			this.AutoStylingDisabled = false;
			this.FontSize = new Unit(f);
			this.MenuFontSize = this.FontSize;

			// use a single color to build the menu, Transparent is the unset default
			// if this is used, any other colors will be ignored/overwritten/wiped
			if (themeColor != Color.Transparent) {
				SetThemeColor(themeColor);
				UseSingleColor(this.FontSize, themeColor);
			} else {
				this.BGColor = bg.DecodeColor();
				this.UnSelBGColor = ubg.DecodeColor();
				this.ForeColor = fc.DecodeColor();
				this.BackColor = bc.DecodeColor();

				this.HoverBGColor = hbc.DecodeColor();
				this.HoverFGColor = hfc.DecodeColor();

				this.UnSelFGColor = uf.DecodeColor();
				this.SelBGColor = sbg.DecodeColor();
				this.SelFGColor = sfg.DecodeColor();

				this.SubBGColor = bc2.DecodeColor();
				this.SubFGColor = fc2.DecodeColor();
			}
		}

		private string ConvertColorCode(Color color) {
			var hcolor = ColorTranslator.ToHtml(color).ToLowerInvariant();
			return hcolor;
		}

		public void SetThemeColor(string main) {
			var mainColor = main.DecodeColor();
			SetThemeColor(mainColor);
		}

		public void SetThemeColor(Color main) {
			this.ThemeColor = main;
		}

		protected void UseSingleColor(Unit f, Color main) {
			this.ThemeColor = main;

			var mainD1 = CmsSkin.DarkenColor(main, 0.25);
			var mainD3 = CmsSkin.DarkenColor(main, 0.85);

			var mainL2 = CmsSkin.LightenColor(main, 0.65);
			var mainL3 = CmsSkin.LightenColor(main, 0.95);

			this.AutoStylingDisabled = false;
			this.FontSize = f;
			this.MenuFontSize = this.FontSize;

			this.ForeColor = main;
			this.SubBGColor = mainD1;

			this.UnSelBGColor = mainL2;
			this.UnSelFGColor = mainD3;

			this.SelFGColor = mainL3;
			this.SubFGColor = mainL3;
		}

		public static string NavigationStylePath {
			get {
				return "/carrotcakenavstyle.axd";
			}
		}

		public void CreateCssLink() {
			string sCSSText = string.Empty;

			if (!this.AutoStylingDisabled) {
				var sb = new StringBuilder();

				// use a single color to build the menu, Transparent is the unset default
				// if this is used, any other colors will be ignored/overwritten/wiped
				if (this.ThemeColor != Color.Transparent) {
					UseSingleColor(this.FontSize, this.ThemeColor);
				}

				sb.Append(string.Format("{0}?el={1}&sel={2}&f={3}", NavigationStylePath, HttpUtility.HtmlEncode(this.HtmlClientID.EncodeBase64()), HttpUtility.HtmlEncode(this.CSSSelected.EncodeBase64()), this.FontSize));
				sb.Append(string.Format("&tc={0}", this.ThemeColor.EncodeColor()));

				sb.Append(string.Format("&bg={0}&ubg={1}&fc={2}&bc={3}", this.BGColor.EncodeColor(), this.UnSelBGColor.EncodeColor(), this.ForeColor.EncodeColor(), this.BackColor.EncodeColor()));
				sb.Append(string.Format("&hbc={0}&hfc={1}", this.HoverBGColor.EncodeColor(), this.HoverFGColor.EncodeColor()));
				sb.Append(string.Format("&uf={0}&sbg={1}&sfg={2}", this.UnSelFGColor.EncodeColor(), this.SelBGColor.EncodeColor(), this.SelFGColor.EncodeColor()));
				sb.Append(string.Format("&bc2={0}&fc2={1}", this.SubBGColor.EncodeColor(), this.SubFGColor.EncodeColor()));

				if (!string.IsNullOrEmpty(this.TopBackgroundStyle)) {
					sb.Append(string.Format("&tbg={0}", HttpUtility.HtmlEncode(this.TopBackgroundStyle)));
				}
				sb.Append(string.Format("&ts={0}", WebControlHelper.DateKey()));

				sCSSText = UrlPaths.CreateCssTag(string.Format("Nav CSS: {0}", this.HtmlClientID), sb.ToString());
			}

			_cssText.Text = sCSSText;
		}

		private void FlipColor() {
			this.HoverBGColor = this.HoverBGColor == Color.Empty ? this.ForeColor : this.HoverBGColor;
			this.HoverFGColor = this.HoverFGColor == Color.Empty ? this.BackColor : this.HoverFGColor;
			this.UnSelFGColor = this.UnSelFGColor == Color.Empty ? this.ForeColor : this.UnSelFGColor;
			this.SelBGColor = this.SelBGColor == Color.Empty ? this.ForeColor : this.SelBGColor;
			this.SelFGColor = this.SelFGColor == Color.Empty ? this.BackColor : this.SelFGColor;
			this.SubBGColor = this.SubBGColor == Color.Empty ? this.BackColor : this.SubBGColor;
			this.SubFGColor = this.SubFGColor == Color.Empty ? this.ForeColor : this.SubFGColor;
		}

		public string GenerateCSS() {
			var sb = new StringBuilder();

			if (string.IsNullOrEmpty(this.OverrideCSS) && !this.AutoStylingDisabled) {
				FlipColor();

				sb.Append(ControlUtilities.GetManifestResourceStream("Carrotware.CMS.UI.Controls.TopMenu.txt"));

				if (this.AttemptResponsiveCSS && sb.Length > 1) {
					var sbCSS1 = new StringBuilder();
					sbCSS1.Append(ControlUtilities.GetManifestResourceStream("Carrotware.CMS.UI.Controls.TopMenuRes.txt"));
					sbCSS1.Replace("{DESKTOP_CSS}", sb.ToString());

					sb = sbCSS1;
				}

				if (sb.Length > 1) {
					sb.Replace("[[TIMESTAMP]]", DateTime.UtcNow.ToString("u"));

					sb.Replace("{FORE_HEX}", ConvertColorCode(this.ForeColor));
					sb.Replace("{BG_HEX}", ConvertColorCode(this.BGColor));

					sb.Replace("{HOVER_FORE_HEX}", ConvertColorCode(this.HoverFGColor));
					sb.Replace("{HOVER_BG_HEX}", ConvertColorCode(this.HoverBGColor));

					sb.Replace("{SEL_FORE_HEX}", ConvertColorCode(this.SelFGColor));
					sb.Replace("{SEL_BG_HEX}", ConvertColorCode(this.SelBGColor));

					sb.Replace("{UNSEL_FORE_HEX}", ConvertColorCode(this.UnSelFGColor));
					sb.Replace("{UNSEL_BG_HEX}", ConvertColorCode(this.UnSelBGColor));

					sb.Replace("{SUB_FORE_HEX}", ConvertColorCode(this.SubFGColor));
					sb.Replace("{SUB_BG_HEX}", ConvertColorCode(this.SubBGColor));

					if (this.FontSize.Value > 0) {
						sb.Replace("{FONT_SIZE}", this.FontSize.ToString());
					} else {
						sb.Replace("{FONT_SIZE}", "inherit");
					}

					if (this.MenuFontSize.Value > 0) {
						sb.Replace("{MAIN_FONT_SIZE}", this.MenuFontSize.ToString());
					} else {
						sb.Replace("{MAIN_FONT_SIZE}", "inherit");
					}

					if (this.MobileWidth.Value > 50) {
						sb.Replace("{MOBILE_WIDTH}", this.MobileWidth.ToString());
					} else {
						sb.Replace("{MOBILE_WIDTH}", "100%");
					}

					if (this.MobileWidth.Value > 50) {
						sb.Replace("{DESK_WIDTH}", (this.MobileWidth.Value + 1).ToString() + this.MobileWidth.Type);
					} else {
						sb.Replace("{DESK_WIDTH}", "100%");
					}

					sb.Replace("{MENU_SELECT_CLASS}", this.CSSSelected);
					sb.Replace("{MENU_HASCHILD_CLASS}", this.CSSHasChildren);

					if (!string.IsNullOrEmpty(this.TopBackgroundStyle)) {
						this.TopBackgroundStyle = this.TopBackgroundStyle.Replace(";", "");
						sb.Replace("{TOP_BACKGROUND_STYLE}", "background: " + this.TopBackgroundStyle + ";");
					} else {
						sb.Replace("{TOP_BACKGROUND_STYLE}", string.Empty);
					}

					sb.Replace("{MENU_ID}", string.Format("#{0}", this.HtmlClientID));
					sb.Replace("{MENU_WRAPPER_ID}", string.Format("#{0}-wrapper", this.HtmlClientID));
				}
			}

			return sb.ToString();
		}
	}
}