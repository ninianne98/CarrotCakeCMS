using Carrotware.Web.UI.Controls;
using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:FancyBox runat=server></{0}:FancyBox>")]
	public class FancyBox : BaseWebControl {

		protected string GetResourceURL(string resourceName) {
			string sJQFile = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Plugins.PhotoGallery." + resourceName);
			return HttpUtility.HtmlEncode(sJQFile);
		}

		protected override void RenderContents(HtmlTextWriter writer) {
			if (HttpContext.Current != null) {
				writer.Write("<link href=\"" + GetResourceURL("fancybox.fancybox.css") + "\" type=\"text/css\" rel=\"stylesheet\" /> \r\n");

				writer.Write("<script src=\"" + GetResourceURL("fancybox.fancybox-p.js") + "\" type=\"text/javascript\"></script> \r\n");

				writer.Write("<script src=\"" + GetResourceURL("fancybox.mousewheel-p.js") + "&load=effects,builder\" type=\"text/javascript\"></script> \r\n");

				writer.Write("<script src=\"" + GetResourceURL("fancybox.easing-p.js") + "\" type=\"text/javascript\"></script> \r\n");
			}
		}
	}
}