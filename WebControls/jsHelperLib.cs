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
				string s = (string)ViewState["LoadJQueryAsServerControl"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}
			set {
				ViewState["LoadJQueryAsServerControl"] = value.ToString();
			}
		}

		protected override void RenderContents(HtmlTextWriter writer) {
			int ident = writer.Indent;

			string sJSFile = WebControlHelper.GetWebResourceUrl("jsHelperLibrary.js");
			string sJQFile = jquery.GeneralUri;

			writer.Indent = ident + 3;
			writer.WriteLine();
			writer.WriteLine("<!-- Javascript Helper Functions BEGIN -->");
			writer.WriteLine("<script src=\"" + sJSFile + "\" type=\"text/javascript\"></script> ");

			if (this.LoadJQueryAsServerControl) {
				this.Page.Header.Controls.AddAt(0, new jquery());
			} else {
				writer.WriteLine("<script type=\"text/javascript\">__carrotware_SetJQueryURL('" + sJQFile + "');</script> ");
			}

			writer.WriteLine("<!-- Javascript Helper Functions END -->");
			writer.WriteLine();

			writer.Indent = ident;
		}
	}
}