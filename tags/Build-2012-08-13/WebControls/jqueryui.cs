using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace Carrotware.Web.UI.Controls {
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:jqueryui runat=server></{0}:jqueryui>")]
	public class jqueryui : BaseWebControl {


		protected override void RenderContents(HtmlTextWriter output) {
			string sJQFile = "";

			sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jqueryui-1-8-21.js");

			output.Write("<!-- JQuery UI --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> \r\n");
		}


	}
}
