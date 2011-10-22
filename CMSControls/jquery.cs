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
	[ToolboxData("<{0}:jquery runat=server></{0}:jquery>")]
	public class jquery : BaseServerControl {
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

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string JQVersion {
			get {
				String s = (String)ViewState["JQVersion"];
				return ((s == null) ? "1.6" : s);
			}
			set {
				ViewState["JQVersion"] = value;
			}
		}

		protected override void RenderContents(HtmlTextWriter output) {

			string sJQFile = "";

			switch (JQVersion) {
				case "1.6.0":
					sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Controls.jquery160.js");
					break;
				case "1.6":
				case "1.6.2":
					sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Controls.jquery162.js");
					break;
				case "1.5.2":
				case "1.5":
					sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Controls.jquery152.js");
					break;
				case "1.4.2":
				case "1.4":
					sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Controls.jquery142.js");
					break;
				case "1.3.2":
				case "1.3":
					sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Controls.jquery132.js");
					break;
				default:
					JQVersion = "1.5.2";
					sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Controls.jquery152.js");
					break;
			}

			output.Write("<!-- JQuery v. " + JQVersion + " --> <script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> \r\n");

			//var link = new HtmlLink();
			//link.Href = sJQFile;
			//link.Attributes.Add("rel", "stylesheet");
			//link.Attributes.Add("type", "text/css");
			//Page.Header.Controls.Add(link);
		}


	}
}
