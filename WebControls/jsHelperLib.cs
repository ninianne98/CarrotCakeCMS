using System;
using System.ComponentModel;
using System.Web.UI;

namespace Carrotware.Web.UI.Controls {

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:jsHelperLib runat=server></{0}:jsHelperLib>")]
	public class jsHelperLib : BaseWebControl {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public bool LoadJQueryAsServerControl {
			get {
				String s = (String)ViewState["LoadJQueryAsServerControl"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}
			set {
				ViewState["LoadJQueryAsServerControl"] = value.ToString();
			}
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int ident = output.Indent;

			string sJSFile = WebControlHelper.GetWebResourceUrl("Carrotware.Web.UI.Controls.jsHelperLibrary.js");
			string sJQFile = jquery.GeneralUri;

			output.Indent = ident + 3;
			output.WriteLine();
			output.WriteLine("<!-- Javascript Helper Functions BEGIN -->");
			output.WriteLine("<script src=\"" + sJSFile + "\" type=\"text/javascript\"></script> ");

			if (LoadJQueryAsServerControl) {
				this.Page.Header.Controls.AddAt(0, new jquery());
			} else {
				output.WriteLine("<script type=\"text/javascript\">__carrotware_SetJQueryURL('" + sJQFile + "');</script> ");
			}

			output.WriteLine("<!-- Javascript Helper Functions END -->");
			output.WriteLine();

			output.Indent = ident;
		}
	}
}