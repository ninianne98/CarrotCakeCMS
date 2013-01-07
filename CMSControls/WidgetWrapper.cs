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

	[ToolboxData("<{0}:WidgetWrapper runat=server></{0}:WidgetWrapper>")]
	public class WidgetWrapper : PlaceHolder, ICMSCoreControl {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
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
		public Guid DatabaseKey {
			get {
				Guid s = Guid.Empty;
				try { s = new Guid(ViewState["DatabaseKey"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["DatabaseKey"] = value;
			}
		}



		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int Order {
			get {
				int s = 0;
				try { s = int.Parse(ViewState["Order"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["Order"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string ControlPath {
			get {
				String s = (String)ViewState["ControlPath"];
				return ((s == null) ? String.Empty : s);
			}

			set {
				ViewState["ControlPath"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string ControlTitle {
			get {
				String s = (String)ViewState["ControlTitle"];
				return ((s == null) ? String.Empty : s);
			}

			set {
				ViewState["ControlTitle"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string JSEditFunction {
			get {
				String s = (String)ViewState["JSEditFunction"];
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["JSEditFunction"] = value;
			}
		}


		protected override void Render(HtmlTextWriter w) {
			if (this.IsAdminMode) {
				string sEdit = "";
				if (!string.IsNullOrEmpty(this.JSEditFunction)) {
					sEdit = " <li><a class=\"cmsWidgetBarLink cmsWidgetBarIconPencil\" id=\"cmsContentEditLink\" href=\"javascript:" + this.JSEditFunction + "\">\r\n"
							+ " Edit </a></li> \r\n";
					sEdit += " <li><a class=\"cmsWidgetBarLink cmsWidgetBarIconWidget2\" id=\"cmsContentEditLink\" href=\"javascript:cmsManageWidgetHistory('" + this.DatabaseKey + "')\">\r\n"
								+ " History </a></li> \r\n";
				}

				//string sShrink = " <li><a class=\"cmsWidgetBarLink cmsWidgetBarIconShrink\" id=\"cmsContentLink\" href=\"javascript:cmsShrinkWidgetHeight('" + DatabaseKey + "');\">\r\n"
				//                + " Shrink </a></li> \r\n";

				string sRemove = " <li><a class=\"cmsWidgetBarLink cmsWidgetBarIconCross\" id=\"cmsContentLink\" href=\"javascript:cmsRemoveWidgetLink('" + this.DatabaseKey + "');\">\r\n"
								+ " Remove </a></li> \r\n";

				string sCog = "<a class=\"cmsWidgetBarLink cmsWidgetBarIconCog\" id=\"cmsWidgetBarIcon\" href=\"javascript:void(0);\">Modify</a>";

				string sMenu = "<div id=\"cmsEditMenuList\"><div id=\"cmsEditMenuList-inner\"> <ul class=\"cmsMnuParent\"> <li class=\"cmsWidgetCogIcon\"> "
							+ sCog + "\r\n <ul class=\"cmsMnuChildren\">" + sEdit + sRemove + " </ul> </li> </ul> </div> </div>";

				string sPrefix = "<div id=\"" + this.DatabaseKey + "\" class=\"cmsWidgetContainerWrapper\" >"
									+ "<div id=\"cms_" + this.ClientID + "\">"
									+ "<div id=\"cmsWidgetHead\" class=\"cmsWidgetTitleBar\">"
									+ "<div id=\"cmsControlPath\" title=\"" + this.ControlPath + "\" tooltip=\"" + this.ControlPath + "\">" + this.ControlTitle + "</div> " + sMenu + " </div>"
									+ "</div>\r\n<div style=\"clear: both;\"></div>\r\n"
									+ "<div class=\"cmsWidgetControl\" id=\"cmsControl\" >\r\n"
									+ "<input type=\"hidden\" id=\"cmsCtrlID\" value=\"" + this.DatabaseKey + "\"  />\r\n"
									+ "<input type=\"hidden\" id=\"cmsCtrlOrder\" value=\"" + this.Order + "\"  />\r\n";

				w.Write(sPrefix);

			} else {
				w.WriteLine("<span style=\"display: none;\" id=\"BEGIN-" + this.ClientID + "\"></span>");
			}

			base.Render(w);

			if (IsAdminMode) {
				w.Write("\r\n<div style=\"clear: both;\"></div> </div></div>\r\n");
			} else {
				w.WriteLine("<span style=\"display: none;\" id=\"END-" + this.ClientID + "\"></span>");
			}

		}


	}
}
