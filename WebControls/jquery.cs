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
	[ToolboxData("<{0}:jquery runat=server></{0}:jquery>")]
	public class jquery : BaseWebControl {

		public static string DefaultJQVersion {
			get {
				return "3";
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

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool UseJqueryMigrate {
			get {
				String s = (String)ViewState["UseJqueryMigrate"];
				return ((s == null) ? (this.JQVersion.StartsWith("3") ? true : false) : Convert.ToBoolean(s));
			}
			set {
				ViewState["UseJqueryMigrate"] = value.ToString();
			}
		}

		private static string GetWebResourceUrl(string resource) {
			return WebControlHelper.GetWebResourceUrl(resource);
		}

		private static string _generalUri = null;

		public static string GeneralUri {
			get {
				if (string.IsNullOrEmpty(_generalUri)) {
					_generalUri = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jquery-1-11-3.js");
				}

				return _generalUri;
			}
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
				case "3":
				case "3.0":
				case "3.6":
				case "3.7":
					jqVer = "3.7.1";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jquery-3-7-1.js");
					break;

				case "2":
				case "2.0":
				case "2.2":
					jqVer = "2.2.4";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jquery-2-2-4.js");
					break;

				case "1":
				case "1.0":
				case "1.11":
					jqVer = "1.11.3";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jquery-1-11-3.js");
					break;

				case "1.12":
					jqVer = "1.12.4";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jquery-1-12-4.js");
					break;

				case "1.10":
					jqVer = "1.10.2";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jquery-1-10-2.js");
					break;

				// older versions get dumped to 1.9
				case "1.1":
				case "1.2":
				case "1.3":
				case "1.4":
				case "1.5":
				case "1.6":
				case "1.7":
				case "1.8":
				case "1.9":
					jqVer = "1.9.1";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jquery-1-9-1.js");
					break;

				// if you didn't provide a version or a meaningful version, this is what you get
				default:
					jqVer = "3.7.1";
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jquery-3-7-1.js");
					break;
			}

			output.WriteLine("<!-- JQuery v. " + jqVer + " --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> ");

			if (this.UseJqueryMigrate) {
				string sJQFileMig = string.Empty;

				if (jqVer.StartsWith("1.9") || jqVer.StartsWith("1.10") || jqVer.StartsWith("1.11")) {
					sJQFileMig = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jquery-mig-1-2-1.js");
				}

				if (jqVer.StartsWith("1.12") || jqVer.StartsWith("1.13")) {
					sJQFileMig = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jquery-mig-1-3-0.js");
				}

				if (jqVer.StartsWith("3")) {
					sJQFileMig = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquery.jquery-mig-3-4-0.js");
				}

				if (!string.IsNullOrEmpty(sJQFileMig)) {
					output.WriteLine("<!-- jQuery Migrate Plugin --> <script src=\"" + sJQFileMig + "\" type=\"text/javascript\"></script> ");
				}
			}

			string key = WebControlHelper.DateKey();

			output.WriteLine("<!-- Carrot Helpers --> <script src=\"" + UrlPaths.HelperPath + "?ts=" + key + "\" type=\"text/javascript\"></script> ");
		}
	}
}