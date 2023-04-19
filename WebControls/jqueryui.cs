using System;
using System.ComponentModel;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.Web.UI.Controls {

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:jqueryui runat=server></{0}:jqueryui>")]
	public class jqueryui : BaseWebControl {

		public static string DefaultJQUIVersion {
			get {
				return "1.13";
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

		private static string GetWebResourceUrl(string resource) {
			return WebControlHelper.GetWebResourceUrl(resource);
		}

		private static string _generalUri = null;

		public static string GeneralUri {
			get {
				if (string.IsNullOrEmpty(_generalUri)) {
					_generalUri = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jqueryui-1-13-2.js");
				}

				return _generalUri;
			}
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
					jqVer = "1.10.2";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jqueryui-1-10-2.js");
					break;

				case "1.9":
					jqVer = "1.9.2";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jqueryui-1-9-2.js");
					break;

				case "1.8":
					jqVer = "1.8.24";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jqueryui-1-8-24.js");
					break;

				case "1.7":
					jqVer = "1.7.3";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jqueryui-1-7-3.js");
					break;

				case "1.11":
					jqVer = "1.11.4";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jqueryui-1-11-4.js");
					break;

				case "1.12":
					jqVer = "1.12.1";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jqueryui-1-12-1.js");
					break;

				case "1.13":
				default:
					jqVer = "1.13.2";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jqueryui-1-13-2.js");
					break;
			}

			output.WriteLine("<!-- JQuery UI v. " + jqVer + " --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> ");
		}
	}
}