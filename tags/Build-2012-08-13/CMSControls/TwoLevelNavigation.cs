using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/
namespace Carrotware.CMS.UI.Controls {

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:TwoLevelNavigation runat=server></{0}:TwoLevelNavigation>")]
	public class TwoLevelNavigation : BaseServerControl, IWidgetParmData, IWidget {

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
		public Unit MenuWidth {
			get {
				Unit s = new Unit("940px");
				try { s = new Unit(ViewState["MenuWidth"].ToString()); } catch { }
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
				try { s = new Unit(ViewState["FontSize"].ToString()); } catch { }
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
		public Unit MenuHeight {
			get {
				Unit s = new Unit("60px");
				try { s = new Unit(ViewState["MenuHeight"].ToString()); } catch { }
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
		public Unit SubMenuWidth {
			get {
				Unit s = new Unit("300px");
				try { s = new Unit(ViewState["SubMenuWidth"].ToString()); } catch { }
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
		public Color ForeColor {
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
		public Color BackColor {
			get {
				string s = (string)ViewState["BackColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#DDDDDD") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["BackColor"] = ColorTranslator.ToHtml(value);
			}
		}


		protected List<SiteNav> GetTopNav() {
			return navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SiteData.IsAuthEditor);
		}

		protected List<SiteNav> GetChildren(Guid Root_ContentID) {
			return navHelper.GetChildNavigation(SiteData.CurrentSiteID, Root_ContentID, !SiteData.IsAuthEditor);
		}



		protected override void RenderContents(HtmlTextWriter output) {

		SiteNav pageNav = navHelper.GetPageNavigation(SiteData.CurrentSiteID, SiteData.CurrentScriptName);

			string sParent = "";
			if (pageNav != null) {
				sParent = pageNav.FileName.ToLower();
			}

			List<SiteNav> lst = GetTopNav();
			output.Write("<div id=\"" + this.ClientID + "\">\r\n");
			output.Write("<div id=\"" + this.ClientID + "-inner\">\r\n");

			output.Write("<ul class=\"" + CSSULClassTop + "\">\r\n");
			foreach (SiteNav c1 in lst) {
				List<SiteNav> cc = GetChildren(c1.Root_ContentID);
				if (!c1.PageActive) {
					c1.NavMenuText = "&#9746; " + c1.NavMenuText;
				}
				if (c1.NavFileName.ToLower() == SiteData.CurrentScriptName.ToLower() || c1.NavFileName.ToLower() == sParent) {
					output.Write("\t<li class=\"" + CSSSelected + "\"><a href=\"" + c1.NavFileName + "\">" + c1.NavMenuText + "</a>");
				} else {
					output.Write("\t<li><a href=\"" + c1.NavFileName + "\">" + c1.NavMenuText + "</a>");
				}

				if (cc.Count > 0) {
					output.Write("\r\n\t<ul class=\"" + CSSULClassLower + "\">\r\n");
					foreach (SiteNav c2 in cc) {
						if (!c2.PageActive) {
							c2.NavMenuText = "&#9746; " + c2.NavMenuText;
						}
						if (c2.NavFileName.ToLower() == SiteData.CurrentScriptName.ToLower()) {
							output.Write("\t\t<li class=\"" + CSSSelected + "\"><a href=\"" + c2.NavFileName + "\">" + c2.NavMenuText + "</a></li>\r\n");
						} else {
							output.Write("\t\t<li><a href=\"" + c2.NavFileName + "\">" + c2.NavMenuText + "</a></li>\r\n");
						}
					}
					output.Write("\t</ul>\r\n\t");
				}
				output.Write("</li>\r\n");
			}
			output.Write("</ul>\r\n");

			output.Write("</div>\r\n");
			output.Write("</div>\r\n");
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


					sTmp = "";
					sTmp = (from c in PublicParmValues
							where c.Key.ToLower() == "cssulclasstop"
							select c.Value).FirstOrDefault();

					if (!string.IsNullOrEmpty(sTmp)) {
						CSSULClassTop = sTmp;
					}
					sTmp = "";
					sTmp = (from c in PublicParmValues
							where c.Key.ToLower() == "cssulclasslower"
							select c.Value).FirstOrDefault();

					if (!string.IsNullOrEmpty(sTmp)) {
						CSSULClassLower = sTmp;
					}

					sTmp = "";
					sTmp = (from c in PublicParmValues
							where c.Key.ToLower() == "menuwidth"
							select c.Value).FirstOrDefault();

					if (!string.IsNullOrEmpty(sTmp)) {
						MenuWidth = new Unit(sTmp);
					}

					sTmp = "";
					sTmp = (from c in PublicParmValues
							where c.Key.ToLower() == "menuheight"
							select c.Value).FirstOrDefault();

					if (!string.IsNullOrEmpty(sTmp)) {
						MenuHeight = new Unit(sTmp);
					}

					sTmp = "";
					sTmp = (from c in PublicParmValues
							where c.Key.ToLower() == "submenuwidth"
							select c.Value).FirstOrDefault();

					if (!string.IsNullOrEmpty(sTmp)) {
						SubMenuWidth = new Unit(sTmp);
					}

					sTmp = "";
					sTmp = (from c in PublicParmValues
							where c.Key.ToLower() == "fontsize"
							select c.Value).FirstOrDefault();

					if (!string.IsNullOrEmpty(sTmp)) {
						FontSize = new Unit(sTmp);
					}

					sTmp = "";
					sTmp = (from c in PublicParmValues
							where c.Key.ToLower() == "forecolor"
							select c.Value).FirstOrDefault();

					if (!string.IsNullOrEmpty(sTmp)) {
						ForeColor = ColorTranslator.FromHtml(sTmp);
					}

					sTmp = "";
					sTmp = (from c in PublicParmValues
							where c.Key.ToLower() == "backcolor"
							select c.Value).FirstOrDefault();

					if (!string.IsNullOrEmpty(sTmp)) {
						BackColor = ColorTranslator.FromHtml(sTmp);
					}
				}
			} catch (Exception ex) {
			}


			if (string.IsNullOrEmpty(OverrideCSS)) {
				string sCSS = String.Empty;
				string sCSS7 = String.Empty;

				Assembly _assembly = Assembly.GetExecutingAssembly();

				using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.UI.Controls.TopMenu.txt"))) {
					sCSS = oTextStream.ReadToEnd();
				}

				using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.UI.Controls.TopMenu7.txt"))) {
					sCSS7 = oTextStream.ReadToEnd();
				}

				sCSS = sCSS.Replace("{FORE_HEX}", ColorTranslator.ToHtml(ForeColor));
				sCSS = sCSS.Replace("{BG_HEX}", ColorTranslator.ToHtml(BackColor));

				sCSS7 = sCSS7.Replace("{FORE_HEX}", ColorTranslator.ToHtml(ForeColor));
				sCSS7 = sCSS7.Replace("{BG_HEX}", ColorTranslator.ToHtml(BackColor));

				sCSS = sCSS.Replace("{MENU_WIDTH}", MenuWidth.Value.ToString() + "px");
				sCSS7 = sCSS7.Replace("{MENU_WIDTH}", MenuWidth.Value.ToString() + "px");

				sCSS = sCSS.Replace("{MENU_HEIGHT}", MenuHeight.Value.ToString() + "px");
				sCSS7 = sCSS7.Replace("{MENU_HEIGHT}", MenuHeight.Value.ToString() + "px");

				sCSS = sCSS.Replace("{FONT_SIZE}", FontSize.Value.ToString() + "px");
				sCSS7 = sCSS7.Replace("{FONT_SIZE}", FontSize.Value.ToString() + "px");

				sCSS = sCSS.Replace("{LINK_PAD1}", Convert.ToInt16(FontSize.Value * .45).ToString() + "px");
				sCSS7 = sCSS7.Replace("{LINK_PAD1}", Convert.ToInt16(FontSize.Value * .45).ToString() + "px");

				sCSS = sCSS.Replace("{LINK_PAD2}", Convert.ToInt16(FontSize.Value * 1.40).ToString() + "px");
				sCSS7 = sCSS7.Replace("{LINK_PAD2}", Convert.ToInt16(FontSize.Value * 1.40).ToString() + "px");

				sCSS = sCSS.Replace("{SUB_MENU_WIDTH}", SubMenuWidth.Value.ToString() + "px");
				sCSS7 = sCSS7.Replace("{SUB_MENU_WIDTH}", SubMenuWidth.Value.ToString() + "px");

				sCSS = sCSS.Replace("{MENU_LINE_HEIGHT}", Convert.ToInt16(MenuHeight.Value / 3).ToString() + "px");
				sCSS7 = sCSS7.Replace("{MENU_LINE_HEIGHT}", Convert.ToInt16(MenuHeight.Value / 3).ToString() + "px");

				sCSS = sCSS.Replace("{MENU_TOP_PAD}", Convert.ToInt16((MenuHeight.Value + 5) / 4).ToString() + "px");
				sCSS7 = sCSS7.Replace("{MENU_TOP_PAD}", Convert.ToInt16((MenuHeight.Value + 5) / 4).ToString() + "px");

				sCSS = sCSS.Replace("{MENU_TOP_2_PAD}", Convert.ToInt16(FontSize.Value * .5).ToString() + "px");
				sCSS7 = sCSS7.Replace("{MENU_TOP_2_PAD}", Convert.ToInt16(FontSize.Value * .5).ToString() + "px");

				sCSS = sCSS.Replace("{MENU_TOP_3_PAD}", Convert.ToInt16(FontSize.Value * .8).ToString() + "px");
				sCSS7 = sCSS7.Replace("{MENU_TOP_3_PAD}", Convert.ToInt16(FontSize.Value * .8).ToString() + "px");

				sCSS = sCSS.Replace("{MENU_SELECT_CLASS}", CSSSelected);
				sCSS7 = sCSS7.Replace("{MENU_SELECT_CLASS}", CSSSelected);

				sCSS = sCSS.Replace("{MENU_UL_TOP}", CSSULClassTop);
				sCSS7 = sCSS7.Replace("{MENU_UL_TOP}", CSSULClassTop);

				sCSS = sCSS.Replace("{MENU_UL_LOWER}", CSSULClassLower);
				sCSS7 = sCSS7.Replace("{MENU_UL_LOWER}", CSSULClassLower);

				if (!string.IsNullOrEmpty(TopBackgroundStyle)) {
					TopBackgroundStyle = TopBackgroundStyle.Replace(";", "");
					sCSS = sCSS.Replace("{TOP_BACKGROUND_STYLE}", "background: " + TopBackgroundStyle + ";");
					sCSS7 = sCSS7.Replace("{TOP_BACKGROUND_STYLE}", "background: " + TopBackgroundStyle + ";");
				} else {
					sCSS = sCSS.Replace("{TOP_BACKGROUND_STYLE}", "");
					sCSS7 = sCSS7.Replace("{TOP_BACKGROUND_STYLE}", "");
				}

				if (!string.IsNullOrEmpty(ItemBackgroundStyle)) {
					ItemBackgroundStyle = ItemBackgroundStyle.Replace(";", "");
					sCSS = sCSS.Replace("{ITEM_BACKGROUND_STYLE}", "background: " + ItemBackgroundStyle + ";");
					sCSS7 = sCSS7.Replace("{ITEM_BACKGROUND_STYLE}", "background: " + ItemBackgroundStyle + ";");
				} else {
					sCSS = sCSS.Replace("{ITEM_BACKGROUND_STYLE}", "");
					sCSS7 = sCSS7.Replace("{ITEM_BACKGROUND_STYLE}", "");
				}

				sCSS = sCSS.Replace("{MENU_ID}", "#" + this.ClientID + "-inner");
				sCSS = sCSS.Replace("{MENU_WRAPPER_ID}", "#" + this.ClientID + "");
				sCSS = "\r\n<style type=\"text/css\">\r\n" + sCSS + "\r\n</style>\r\n";

				sCSS7 = sCSS7.Replace("{MENU_ID}", "#" + this.ClientID + "-inner");
				sCSS7 = sCSS7.Replace("{MENU_WRAPPER_ID}", "#" + this.ClientID + "");
				sCSS7 = "\r\n<!--[if IE 7]> \r\n<style type=\"text/css\">\r\n" + sCSS7 + "\r\n</style>\r\n<![endif]-->\r\n";

				Literal link = new Literal();
				link.Text = sCSS;
				Page.Header.Controls.Add(link);
				Literal link7 = new Literal();
				link7.Text = sCSS7;
				Page.Header.Controls.Add(link7);
			} else {
				HtmlLink link = new HtmlLink();
				link.Href = OverrideCSS;
				link.Attributes.Add("rel", "stylesheet");
				link.Attributes.Add("type", "text/css");
				Page.Header.Controls.Add(link);
			}

			base.OnPreRender(e);
		}



	}
}
