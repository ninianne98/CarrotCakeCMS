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
	[ToolboxData("<{0}:jquery runat=server></{0}:jquery>")]
	public class jquery : BaseWebControl {


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string JQVersion {
			get {
				String s = (String)ViewState["JQVersion"];
				return ((s == null) ? "1.7" : s);
			}
			set {
				ViewState["JQVersion"] = value;
			}
		}

		protected override void RenderContents(HtmlTextWriter output) {

			string sJQFile = "";
			string jqVer = JQVersion;

			if (!string.IsNullOrEmpty(jqVer) && jqVer.Length > 3) {
				jqVer = jqVer.Substring(0, 3);
			}

			switch (jqVer) {
				case "1.8":
					jqVer = "1.8.2";
					sJQFile = BaseWebControl.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquery182.js");
					break;
				case "1.6":
					jqVer = "1.6.4";
					sJQFile = BaseWebControl.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquery164.js");
					break;
				case "1.5":
					jqVer = "1.5.2";
					sJQFile = BaseWebControl.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquery152.js");
					break;
				case "1.4":
					jqVer = "1.4.2";
					sJQFile = BaseWebControl.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquery142.js");
					break;
				case "1.3":
					jqVer = "1.3.2";
					sJQFile = BaseWebControl.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquery132.js");
					break;
				default:
					jqVer = "1.7.2";
					sJQFile = BaseWebControl.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.jquery172.js");
					break;
			}

			output.WriteLine("<!-- JQuery v. " + jqVer + " --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> ");

		}


	}
}
