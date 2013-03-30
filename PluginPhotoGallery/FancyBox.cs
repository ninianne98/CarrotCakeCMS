using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.Web.UI.Controls;



namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:FancyBox runat=server></{0}:FancyBox>")]
	public class FancyBox : BaseWebControl {

		protected string GetResourceURL(string sResourceName) {
			string sJQFile = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Plugins.PhotoGallery." + sResourceName);
			return HttpUtility.HtmlEncode(sJQFile);
		}

		protected override void RenderContents(HtmlTextWriter output) {

			if (HttpContext.Current != null) {

				output.Write("<link href=\"" + GetResourceURL("fancybox.fancybox.css") + "\" type=\"text/css\" rel=\"stylesheet\" /> \r\n");

				output.Write("<script src=\"" + GetResourceURL("fancybox.fancybox-p.js") + "\" type=\"text/javascript\"></script> \r\n");

				output.Write("<script src=\"" + GetResourceURL("fancybox.mousewheel-p.js") + "&load=effects,builder\" type=\"text/javascript\"></script> \r\n");

				output.Write("<script src=\"" + GetResourceURL("fancybox.easing-p.js") + "\" type=\"text/javascript\"></script> \r\n");
			}
		}


	}
}
