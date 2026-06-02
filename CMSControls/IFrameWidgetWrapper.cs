using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:IFrameWidgetWrapper runat=server></{0}:IFrameWidgetWrapper>")]
	public class IFrameWidgetWrapper : BaseServerControl {

		[Category("Appearance")]
		[DefaultValue(false)]
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

		[Category("Appearance")]
		[DefaultValue("")]
		public string Hyperlink {
			get {
				string s = (string)ViewState["Hyperlink"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["Hyperlink"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("width: 300px; height: 100px;")]
		public string CSSStyle {
			get {
				string s = (string)ViewState["CSSStyle"];
				return ((s == null) ? "width: 300px; height: 100px;" : s);
			}
			set {
				ViewState["CSSStyle"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool ScrollingFrame {
			get {
				String s = (String)ViewState["ScrollingFrame"];
				return ((s == null) ? true : Convert.ToBoolean(s));
			}
			set {
				ViewState["ScrollingFrame"] = value.ToString();
			}
		}

		protected override void RenderContents(HtmlTextWriter writer) {
			int indent = writer.Indent;

			writer.Indent = indent + 3;
			writer.WriteLine();

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}
			string sStyle = "";
			if (!string.IsNullOrEmpty(CSSStyle)) {
				sStyle = " style=\"" + CSSStyle + "\" ";
			}
			string sHREF = "";
			if (!string.IsNullOrEmpty(Hyperlink)) {
				sHREF = " src=\"" + Hyperlink + "\" ";
			}
			string sScroll = "";
			if (ScrollingFrame) {
				sScroll = " scrolling=\"auto\" ";
			}

			writer.Indent++;

			writer.WriteLine("<div id=\"" + this.ClientID + "\">");
			writer.WriteLine("\t<iframe id=\"" + this.ClientID + "_frame\" " + sScroll + sStyle + sCSS + sHREF + " > </iframe>");
			writer.WriteLine("</div>");

			writer.Indent--;

			writer.Indent = indent;
		}

		protected override void OnPreRender(EventArgs e) {
			try {
				if (PublicParmValues.Any()) {
					CssClass = GetParmValue("CssClass", "");

					CSSStyle = GetParmValue("CSSStyle", "width: 300px; height: 100px;");

					Hyperlink = GetParmValue("Hyperlink", "");

					ScrollingFrame = Convert.ToBoolean(GetParmValue("ScrollingFrame", "true"));
				}
			} catch (Exception ex) {
			}

			base.OnPreRender(e);
		}
	}
}