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
	public class TwoLevelNavigation : BaseNavSel, IWidgetLimitedProperties {

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
		[DefaultValue("")]
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
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue("")]
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

		[Category("Appearance")]
		[DefaultValue("")]
		public string TopBackgroundStyle {
			get {
				String s = (String)ViewState["TopBackgroundStyle"];
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["TopBackgroundStyle"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
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

			this.Page.Header.Controls.Add(cssText);
		}

		protected override void WriteListPrefix(HtmlTextWriter output) {

			if (HttpContext.Current == null) {
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

		public List<string> LimitedPropertyList {
			get {
				List<string> lst = new List<string>();
				lst.Add("OverrideCSS");
				lst.Add("CssClass");
				lst.Add("ExtraCSS");
				lst.Add("AutoStylingDisabled");
				//lst.Add("AttemptResponsiveCSS");
				//lst.Add("MobileWidth");
				lst.Add("CSSSelected");
				lst.Add("CSSHasChildren");
				lst.Add("WrapList");
				lst.Add("FontSize");
				lst.Add("TopBackgroundStyle");
				lst.Add("ForeColor");
				lst.Add("BackColor");
				lst.Add("BGColor");
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

			base.OnPreRender(e);

			try {

				if (PublicParmValues.Count > 0) {

					string sTmp = "";

					OverrideCSS = GetParmValue("OverrideCSS", "");
					ExtraCSS = GetParmValue("ExtraCSS", "");

					sTmp = GetParmValue("AutoStylingDisabled", "false");
					if (!string.IsNullOrEmpty(sTmp)) {
						AutoStylingDisabled = Convert.ToBoolean(sTmp);
					}

					sTmp = GetParmValue("AttemptResponsiveCSS", "true");
					if (!string.IsNullOrEmpty(sTmp)) {
						AttemptResponsiveCSS = Convert.ToBoolean(sTmp);
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
						sTmp = GetParmValue("BGColor", "");
						if (!string.IsNullOrEmpty(sTmp)) {
							BGColor = ColorTranslator.FromHtml(sTmp);
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

		}

		private void ParseCSS() {
			if (string.IsNullOrEmpty(OverrideCSS) && !this.AutoStylingDisabled && string.IsNullOrEmpty(cssText.Text)) {
				string sCSSText = ControlUtilities.GetManifestResourceStream("Carrotware.CMS.UI.Controls.TopMenu.txt");

				if (AttemptResponsiveCSS) {
					string sCSS1 = ControlUtilities.GetManifestResourceStream("Carrotware.CMS.UI.Controls.TopMenuRes.txt");
					sCSSText = sCSS1.Replace("{DESKTOP_CSS}", sCSSText);
				}

				if (sCSSText != null) {
					sCSSText = sCSSText.Replace("{FORE_HEX}", ColorTranslator.ToHtml(this.ForeColor).ToLower());
					sCSSText = sCSSText.Replace("{BG_HEX}", ColorTranslator.ToHtml(this.BGColor).ToLower());

					sCSSText = sCSSText.Replace("{HOVER_FORE_HEX}", ColorTranslator.ToHtml(this.HoverFGColor).ToLower());
					sCSSText = sCSSText.Replace("{HOVER_BG_HEX}", ColorTranslator.ToHtml(this.HoverBGColor).ToLower());

					sCSSText = sCSSText.Replace("{SEL_FORE_HEX}", ColorTranslator.ToHtml(this.SelFGColor).ToLower());
					sCSSText = sCSSText.Replace("{SEL_BG_HEX}", ColorTranslator.ToHtml(this.SelBGColor).ToLower());

					sCSSText = sCSSText.Replace("{UNSEL_FORE_HEX}", ColorTranslator.ToHtml(this.UnSelFGColor).ToLower());
					sCSSText = sCSSText.Replace("{UNSEL_BG_HEX}", ColorTranslator.ToHtml(this.UnSelBGColor).ToLower());

					sCSSText = sCSSText.Replace("{SUB_FORE_HEX}", ColorTranslator.ToHtml(this.SubFGColor).ToLower());
					sCSSText = sCSSText.Replace("{SUB_BG_HEX}", ColorTranslator.ToHtml(this.SubBGColor).ToLower());

					sCSSText = sCSSText.Replace("{FONT_SIZE}", this.FontSize.Value.ToString() + "px");

					sCSSText = sCSSText.Replace("{MOBILE_WIDTH}", this.MobileWidth.Value.ToString() + "px");
					sCSSText = sCSSText.Replace("{DESK_WIDTH}", (this.MobileWidth.Value + 1).ToString() + "px");

					sCSSText = sCSSText.Replace("{MENU_SELECT_CLASS}", this.CSSSelected);
					sCSSText = sCSSText.Replace("{MENU_HASCHILD_CLASS}", this.CSSHasChildren);

					if (!string.IsNullOrEmpty(TopBackgroundStyle)) {
						TopBackgroundStyle = TopBackgroundStyle.Replace(";", "");
						sCSSText = sCSSText.Replace("{TOP_BACKGROUND_STYLE}", "background: " + TopBackgroundStyle + ";");
					} else {
						sCSSText = sCSSText.Replace("{TOP_BACKGROUND_STYLE}", "");
					}

					sCSSText = sCSSText.Replace("{MENU_ID}", "#" + this.HtmlClientID + "");
					sCSSText = sCSSText.Replace("{MENU_WRAPPER_ID}", "#" + this.HtmlClientID + "-wrapper");
					sCSSText = "\r\n\t<style type=\"text/css\">\r\n" + sCSSText + "\r\n\t</style>\r\n";
					//sCSSText = "\r\n\t<div type=\"text/css\">\r\n" + sCSSText + "\r\n\t</div>\r\n";
					cssText.Text = sCSSText;
				}
			}
		}

	}
}
