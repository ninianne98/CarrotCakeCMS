using Carrotware.Web.UI.Controls;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:PrettyPhoto runat=server></{0}:PrettyPhoto>")]
	public class PrettyPhoto : BaseWebControl {

		protected override void RenderContents(HtmlTextWriter writer) {
			if (HttpContext.Current != null) {
				string sJQFile = string.Empty;

				sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.prettyPhoto.css");
				writer.Write("<link href=\"" + HttpUtility.HtmlEncode(sJQFile) + "\" type=\"text/css\" rel=\"stylesheet\" /> \r\n");

				sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.prettyPhoto.js");
				writer.Write("<script src=\"" + HttpUtility.HtmlEncode(sJQFile) + "\" type=\"text/javascript\"></script> \r\n");
			}
		}
	}
}