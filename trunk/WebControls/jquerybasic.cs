using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


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
			LightGreen
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

		protected override void RenderContents(HtmlTextWriter output) {
			string sJQFile = "";

			sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquery172.js");

			output.Write("<!-- JQuery --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> \r\n");

			sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jqueryui-1-8-21.js");

			output.Write("<!-- JQuery UI --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> \r\n");

			sJQFile = "";

			switch (SelectedSkin) {
				case jQueryTheme.Silver:
					sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-silver.css");
					break;
				case jQueryTheme.Purple:
					sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-purple.css");
					break;
				case jQueryTheme.Green:
					sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-green.css");
					break;
				case jQueryTheme.Blue:
					sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-blue.css");
					break;
				case jQueryTheme.LightGreen:
					sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-lightgreen.css");
					break;
				case jQueryTheme.GlossyBlack:
				default:
					sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-black.css");
					break;
			}

			output.Write("<!-- JQuery CSS  " + SelectedSkin.ToString() + " --> <link href=\"" + sJQFile + "\" type=\"text/css\" rel=\"stylesheet\" /> \r\n");

		}

		protected override void OnPreRender(EventArgs e) {

		}


	}
}
