using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
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

	[ToolboxData("<{0}:IFrameWidgetWrapper runat=server></{0}:IFrameWidgetWrapper>")]
	public class IFrameWidgetWrapper : BaseServerControl {


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string Hyperlink {
			get {
				string s = (string)ViewState["Hyperlink"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["Hyperlink"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSStyle {
			get {
				string s = (string)ViewState["CSSStyle"];
				return ((s == null) ? "width: 300px; height: 100px;" : s);
			}
			set {
				ViewState["CSSStyle"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public bool ScrollingFrame {
			get {
				String s = (String)ViewState["ScrollingFrame"];
				return ((s == null) ? true : Convert.ToBoolean(s));
			}
			set {
				ViewState["ScrollingFrame"] = value.ToString();
			}
		}


		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			output.Indent = indent + 3;
			output.WriteLine();

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

			output.Indent++;

			output.WriteLine("<div id=\"" + this.ClientID + "\">");
			output.WriteLine("\t<iframe id=\"" + this.ClientID + "_frame\" " + sScroll + sStyle + sCSS + sHREF + " > </iframe>");
			output.WriteLine("</div>");

			output.Indent--;

			output.Indent = indent;
		}

		protected override void OnPreRender(EventArgs e) {

			try {

				if (PublicParmValues.Count > 0) {

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
