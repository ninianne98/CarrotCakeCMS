using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace Carrotware.CMS.UI.Controls {
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:jqueryui runat=server></{0}:jqueryui>")]
	public class jqueryui : BaseServerControl {
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

			sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Controls.jqueryui-1-8-11.js");

			output.Write("<!-- JQuery UI --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> \r\n");
		}


	}
}
