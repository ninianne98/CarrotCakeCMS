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
	[ToolboxData("<{0}:jquerybasic runat=server></{0}:jquerybasic>")]
	public class jquerybasic : BaseWebControl {

		public enum jQueryTheme {
			GlossyBlack,
			Silver,
			Purple,
			Blue,
			Green,
			LightGreen,
			NotUsed
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public jQueryTheme SelectedSkin {
			get {
				string s = (string)ViewState["SelectedSkin"];
				jQueryTheme c = (jQueryTheme)Enum.Parse(typeof(jQueryTheme), "GlossyBlack", true);
				if (!string.IsNullOrEmpty(s)) {
					try {
						c = (jQueryTheme)Enum.Parse(typeof(jQueryTheme), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["SelectedSkin"] = value.ToString();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool StylesheetOnly {
			get {
				String s = (String)ViewState["StylesheetOnly"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}
			set {
				ViewState["StylesheetOnly"] = value.ToString();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string JQVersion {
			get {
				String s = (String)ViewState["JQVersion"];
				return ((s == null) ? jquery.DefaultJQVersion : s);
			}
			set {
				ViewState["JQVersion"] = value;
			}
		}

		//[Bindable(true)]
		//[Category("Appearance")]
		//[DefaultValue("")]
		//[Localizable(true)]
		//public string JQUIVersion {
		//    get {
		//        String s = (String)ViewState["JQUIVersion"];
		//        return ((s == null) ? "1.10" : s);
		//    }
		//    set {
		//        ViewState["JQUIVersion"] = value;
		//    }
		//}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool UseJqueryMigrate {
			get {
				String s = (String)ViewState["UseJqueryMigrate"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}
			set {
				ViewState["UseJqueryMigrate"] = value.ToString();
			}
		}

		public static string GetWebResourceUrl(string resource) {
			return BaseWebControl.GetWebResourceUrl(typeof(jquerybasic), resource);
		}

		protected override void RenderContents(HtmlTextWriter output) {
			string sJQFile = "";

			output.WriteLine();

			if (!this.StylesheetOnly) {
				jquery j1 = new jquery();
				j1.JQVersion = this.JQVersion;
				j1.UseJqueryMigrate = this.UseJqueryMigrate;
				this.Controls.Add(j1);

				jqueryui j2 = new jqueryui();
				//j2.JQUIVersion = this.JQUIVersion;
				this.Controls.Add(j2);

				j1.RenderControl(output);
				j2.RenderControl(output);
			}

			sJQFile = "";

			switch (this.SelectedSkin) {
				case jQueryTheme.GlossyBlack:
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-black.css");
					break;
				case jQueryTheme.Purple:
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-purple.css");
					break;
				case jQueryTheme.Green:
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-green.css");
					break;
				case jQueryTheme.Blue:
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-blue.css");
					break;
				case jQueryTheme.LightGreen:
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-lightgreen.css");
					break;
				case jQueryTheme.Silver:
				default:
					sJQFile = GetWebResourceUrl("Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-silver.css");
					break;
			}

			if (this.SelectedSkin != jQueryTheme.NotUsed) {
				output.Write("<!-- JQuery UI CSS " + SelectedSkin.ToString() + " --> <link href=\"" + sJQFile + "\" type=\"text/css\" rel=\"stylesheet\" /> \r\n");
			}

		}

		protected override void OnPreRender(EventArgs e) {

		}


	}
}
