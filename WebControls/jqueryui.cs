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

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string JQUIVersion {
			get {
				String s = (String)ViewState["JQUIVersion"];
				return ((s == null) ? "1.8" : s);
			}
			set {
				ViewState["JQUIVersion"] = value;
			}
		}

		protected override void RenderContents(HtmlTextWriter output) {

			string sJQFile = "";
			string jqVer = JQUIVersion;

			if (!string.IsNullOrEmpty(jqVer) && jqVer.Length > 3) {
				jqVer = jqVer.Substring(0, 3);
			}

			switch (jqVer) {
				case "1.9":
					jqVer = "1.9.2";
					sJQFile = BaseWebControl.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jqueryui-1-9-2.js");
					break;
				case "1.7":
					jqVer = "1.7.3";
					sJQFile = BaseWebControl.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jqueryui-1-7-3.js");
					break;
				default:
					jqVer = "1.8.24";
					sJQFile = BaseWebControl.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jqueryui-1-8-24.js");
					break;
			}

			output.WriteLine("<!-- JQuery UI v. " + jqVer + " --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> ");


		}


	}
}
