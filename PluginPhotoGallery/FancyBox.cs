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
	[ToolboxData("<{0}:FancyBox runat=server></{0}:FancyBox>")]
	public class FancyBox : BaseWebControl {
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



		protected override void RenderContents(HtmlTextWriter output) {

			string sJQFile = "";

			sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.fancybox.css");
			output.Write("<link href=\"" + sJQFile + "\" type=\"text/css\" rel=\"stylesheet\" /> \r\n");

			sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.fancybox-p.js");
			output.Write("<script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> \r\n");

			sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.mousewheel-p.js");
			output.Write("<script src=\"" + sJQFile + "&load=effects,builder\" type=\"text/javascript\"></script> \r\n");

			sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.easing-p.js");
			output.Write("<script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> \r\n");

			//sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Plugins.PhotoGallery.scripts.builder.js");
			//output.Write("<script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> \r\n");

			//sJQFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Plugins.PhotoGallery.scripts.effects.js");
			//output.Write("<script src=\"" + sJQFile + "\" type=\"text/javascript\"></script> \r\n");


		}


	}
}
