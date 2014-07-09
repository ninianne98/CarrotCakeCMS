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
	[ToolboxData("<{0}:jquery runat=server></{0}:jquery>")]
	public class jquery : BaseWebControl {


		public static string DefaultJQVersion {
			get {
				return "1.11";
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string JQVersion {
			get {
				String s = (String)ViewState["JQVersion"];
				return ((s == null) ? DefaultJQVersion : s);
			}
			set {
				ViewState["JQVersion"] = value;
			}
		}

		public static string GetWebResourceUrl(string resource) {
			return BaseWebControl.GetWebResourceUrl(typeof(jquery), resource);
		}

		protected override void RenderContents(HtmlTextWriter output) {

			string sJQFile = "";
			string jqVer = JQVersion;

			if (!string.IsNullOrEmpty(jqVer) && jqVer.Length > 2) {
				if (jqVer.LastIndexOf(".") != jqVer.IndexOf(".")) {
					jqVer = jqVer.Substring(0, jqVer.LastIndexOf("."));
				}
			}

			switch (jqVer) {
				case "2":
				case "2.0":
				case "1.10":
				case "1.11":
					jqVer = "1.11.1";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery-1111.js");
					break;
				case "1.9":
					jqVer = "1.9.1";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery191.js");
					break;
				case "1.7":
					jqVer = "1.7.2";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery172.js");
					break;
				case "1.3":
				case "1.4":
				case "1.5":
				case "1.6":
					jqVer = "1.6.4";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery164.js");
					break;
				default:
					jqVer = "1.8.3";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery183.js");
					break;
			}

			output.WriteLine("<!-- JQuery v. " + jqVer + " --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> ");

			if (jqVer.StartsWith("1.9")) {
				sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery-110mig.js");
				output.WriteLine("<!-- jQuery Migrate Plugin --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> ");
			}
		}


	}
}
