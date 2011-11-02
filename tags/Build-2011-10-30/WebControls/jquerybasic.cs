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
		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string Text {
			get {
				String s = (String)ViewState["Text"];
				return ((s == null) ? String.Empty : s);
			}

			set {
				ViewState["Text"] = value;
			}
		}


		protected override void RenderContents(HtmlTextWriter output) {
			string sJQFile = "";

			sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquerybasic.jquery-ui.css");

			output.Write("<!-- JQuery CSS --> <link href=\"" + sJQFile + "\" type=\"text/css\" rel=\"stylesheet\" /> \r\n");

			sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquerybasic.jquery-162.js");

			output.Write("<!-- JQuery --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> \r\n");

			sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-1816.js");

			output.Write("<!-- JQuery UI --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> \r\n");


		}

		protected override void OnPreRender(EventArgs e) {

		}


	}
}
