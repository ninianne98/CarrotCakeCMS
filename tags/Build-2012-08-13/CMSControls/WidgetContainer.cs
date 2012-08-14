using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/



namespace Carrotware.CMS.UI.Controls {
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:WidgetContainer runat=server></{0}:WidgetContainer>")]
	public class WidgetContainer : PlaceHolder {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public bool IsAdminMode {
			get {
				bool s = false;
				try { s = (bool)ViewState["IsAdminMode"]; } catch { }
				return s;
			}
			set {
				ViewState["IsAdminMode"] = value;
			}
		}


		protected override void Render(HtmlTextWriter w) {
			if (IsAdminMode) {

				string sEdit = " <li><a class=\"cmsWidgetBarLink cmsWidgetBarIconWidget\" id=\"cmsContentEditLink\" href=\"javascript:cmsManageWidgetList('" + this.ID + "')\">\r\n"
							+ " Widgets </a></li> \r\n";


				string sCog = "<a class=\"cmsWidgetBarLink cmsWidgetBarIconCog\" id=\"cmsWidgetBarIcon\" href=\"javascript:void(0);\">Modify</a>";

				string sMenu = "<div id=\"cmsEditMenuList\"><div id=\"cmsEditMenuList-inner\"> <ul class=\"cmsMnuParent\"> <li class=\"cmsWidgetCogIcon\"> "
							+ sCog + "\r\n <ul class=\"cmsMnuChildren\">" + sEdit + " </ul> </li> </ul> </div> </div>";

				string sPrefix = "<div id=\"cms_" + this.ClientID + "\" class=\"cmsWidgetTargetOuterControl\">\r\n" +
						"<div class=\"cmsWidgetControlTitle\"><div class=\"cmsWidgetControlIDZone\">\r\n" +
						"<div id=\"cmsWidgetContainerName\">" + this.ID + "</div> " + sMenu + "</div></div>\r\n" +
						"<div class=\"cmsTargetArea cmsTargetMove cmsWidgetControl\" id=\"cms_" + this.ClientID + "\" > \r\n";

				w.Write(sPrefix);

			} else {
				w.Write("\r\n<!--div id=\"" + this.ClientID + "\"-->\r\n");
			}

			base.Render(w);

			if (IsAdminMode) {
				w.Write("\r\n&nbsp;\r\n<div style=\"clear: both;\"></div>\r\n</div></div>\r\n");
			} else {
				w.Write("\r\n<!--/div--> <!-- END DIV " + this.ClientID + "\"-->\r\n");
			}

		}


	}
}
