using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/



namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:WidgetContainer runat=server></{0}:WidgetContainer>")]
	public class WidgetContainer : PlaceHolder, ICMSCoreControl {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool IsAdminMode {
			get {
				bool s = false;
				if (ViewState["IsAdminMode"] != null) {
					try { s = (bool)ViewState["IsAdminMode"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["IsAdminMode"] = value;
			}
		}

		public Guid DatabaseKey { get; set; }

		protected override void Render(HtmlTextWriter w) {
			if (this.IsAdminMode) {

				string sEdit = " <li><a class=\"cmsWidgetBarLink cmsWidgetBarIconWidget\" id=\"cmsContentEditLink\" href=\"javascript:cmsManageWidgetList('" + this.ID + "')\">\r\n"
							+ " Widgets </a></li> \r\n";

				string sCog = "<a class=\"cmsWidgetBarLink cmsWidgetBarIconCog\" id=\"cmsWidgetBarIcon\" href=\"javascript:void(0);\">Modify</a>";

				string sMenu = "<div id=\"cmsEditMenuList\"><div id=\"cmsEditMenuList-inner\"> <ul class=\"cmsMnuParent\"> <li class=\"cmsWidgetCogIcon\"> "
							+ sCog + "\r\n <ul class=\"cmsMnuChildren\">" + sEdit + " </ul> </li> </ul> </div> </div>";

				string sPrefix = "<div style=\"clear: both;\"></div>\r\n<div id=\"cms_" + this.ID + "\" class=\"cmsWidgetTargetOuterControl\">\r\n" +
						"<div class=\"cmsWidgetControlTitle\"><div class=\"cmsWidgetControlIDZone\">\r\n" +
						"<div id=\"cmsWidgetContainerName\">" + this.ID + "</div> " + sMenu + "</div></div>\r\n" +
						"<div class=\"cmsTargetArea cmsTargetMove cmsWidgetControl\" id=\"cms_" + this.ID + "\" > \r\n";

				w.Write(sPrefix);

			} else {
				w.WriteLine("<span style=\"display: none;\" id=\"BEGIN-" + this.ClientID + "\"></span>");
			}

			base.Render(w);

			if (this.IsAdminMode) {
				w.Write("\r\n&nbsp;\r\n<div style=\"clear: both;\"></div>\r\n</div></div>\r\n");
			} else {
				w.WriteLine("<span style=\"display: none;\" id=\"END-" + this.ClientID + "\"></span>");
			}

		}


	}
}
