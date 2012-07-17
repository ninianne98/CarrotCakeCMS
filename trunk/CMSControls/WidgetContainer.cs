﻿using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;




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


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string JQueryUIScope {
			get {
				String s = (String)ViewState["jQueryUIScope"];
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["jQueryUIScope"] = value;
			}
		}


		protected override void Render(HtmlTextWriter w) {
			if (IsAdminMode) {

				string sEdit = " <li><a class=\"cmsWidgetBarLink cmsWidgetBarIconWidget\" id=\"cmsContentEditLink{0}\" class=\"ui-state-hover\" href=\"javascript:cmsManageWidgetHistory('" + this.ID + "')\">\r\n"
							+ " Widgets </a></li> \r\n";
			

				string sCog = "<a class=\"cmsWidgetBarLink cmsWidgetBarIconCog\" id=\"cmsWidgetBarIcon\" href=\"javascript:void(0);\">Modify</a>";

				string sMenu = "<div id=\"cmsEditMenuList\"><div id=\"cmsEditMenuList-inner\"> <ul class=\"cmsMnuParent\"> <li class=\"cmsWidgetCogIcon\"> "
							+ sCog + "\r\n <ul class=\"cmsMnuChildren\">" + sEdit +  " </ul> </li> </ul> </div> </div>";

				string sPrefix = "";

				if (!string.IsNullOrEmpty(JQueryUIScope)) {
					sPrefix = "<div id=\"cms" + this.ID + "Outer\" class=\"cmsWidgetTargetOuterControl\">\r\n" +
							"<div class=\"" + JQueryUIScope + "\"><div class=\"" + JQueryUIScope + " ui-state-default ui-state-active ui-widget-header cmsWidgetControlIDZone\">\r\n" +
							" <div id=\"cmsWidgetContainerName\">" + this.ID + "</div> " + sMenu + "</div></div>\r\n" +
							"<div class=\"cmsTargetArea cmsTargetMove cmsWidgetControl\" id=\"cms_" + this.ClientID + "\" > \r\n";
				} else {
					sPrefix = "<div id=\"cms" + this.ID + "Outer\" class=\"cmsWidgetTargetOuterControl\">\r\n" +
							"<div ><div class=\"ui-state-default ui-state-active ui-widget-header cmsWidgetControlIDZone\">\r\n" +
							" <div id=\"cmsWidgetContainerName\">" + this.ID + "</div> " + sMenu + "</div></div>\r\n" +
							"<div class=\"cmsTargetArea cmsTargetMove cmsWidgetControl\" id=\"cms_" + this.ClientID + "\" > \r\n";
				}
				w.Write(sPrefix);

			} else {
				w.Write("<div id=\"" + this.ClientID + "\">");
			}

			base.Render(w);

			if (IsAdminMode) {
				w.Write("\r\n&nbsp;\r\n<div style=\"clear: both;\"></div>\r\n</div></div>");
			} else {
				w.Write("\r\n</div>");
			}

		}


	}
}
