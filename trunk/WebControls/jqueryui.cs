using System;
using System.ComponentModel;
using System.Web.UI;
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
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:jqueryui runat=server></{0}:jqueryui>")]
	public class jqueryui : BaseWebControl {

		public static string DefaultJQUIVersion {
			get {
				return "1.11";
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string JQUIVersion {
			get {
				String s = (String)ViewState["JQUIVersion"];
				return ((s == null) ? DefaultJQUIVersion : s);
			}
			set {
				ViewState["JQUIVersion"] = value;
			}
		}

		public static string GetWebResourceUrl(string resource) {
			return BaseWebControl.GetWebResourceUrl(typeof(jqueryui), resource);
		}

		protected override void RenderContents(HtmlTextWriter output) {

			string sJQFile = "";
			string jqVer = JQUIVersion;

			if (!string.IsNullOrEmpty(jqVer) && jqVer.Length > 2) {
				if (jqVer.LastIndexOf(".") != jqVer.IndexOf(".")) {
					jqVer = jqVer.Substring(0, jqVer.LastIndexOf("."));
				}
			}

			switch (jqVer) {
				case "1.10":
				case "1.11":
					jqVer = "1.11.3";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jqueryui-1-11-3.js");
					break;
				case "1.9":
					jqVer = "1.9.2";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jqueryui-1-9-2.js");
					break;
				case "1.8":
					jqVer = "1.8.24";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jqueryui-1-8-24.js");
					break;
				case "1.7":
					jqVer = "1.7.3";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jqueryui-1-7-3.js");
					break;
				default:
					jqVer = "1.10.2";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jqueryui-1-10-2.js");
					break;
			}

			output.WriteLine("<!-- JQuery UI v. " + jqVer + " --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> ");


		}


	}
}
