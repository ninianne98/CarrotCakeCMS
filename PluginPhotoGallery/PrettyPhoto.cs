using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carrotware.Web.UI.Controls;



namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:PrettyPhoto runat=server></{0}:PrettyPhoto>")]
	public class PrettyPhoto : BaseWebControl {

		protected override void RenderContents(HtmlTextWriter output) {

			if (HttpContext.Current != null) {
				string sJQFile = "";

				sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.prettyPhoto.css");
				output.Write("<link href=\"" + HttpUtility.HtmlEncode(sJQFile) + "\" type=\"text/css\" rel=\"stylesheet\" /> \r\n");

				sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.prettyPhoto.js");
				output.Write("<script src=\"" + HttpUtility.HtmlEncode(sJQFile) + "\" type=\"text/javascript\"></script> \r\n");

			}
		}


	}
}
