using System;
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
	[ToolboxData("<{0}:WidgetWrapper runat=server></{0}:WidgetWrapper>")]
	public class WidgetWrapper : PlaceHolder {

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
		public string JQueryUIScope {
			get {
				String s = (String)ViewState["jQueryUIScope"];
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["jQueryUIScope"] = value;
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
			if (IsAdminMode) {
				string sEdit = "";
				if (!string.IsNullOrEmpty(JSEditFunction)) {
					sEdit = " <a style=\"float:right; color:#676F6A;font-weight: bold;margin: 0px;padding:0px;padding-right: 10px;border: 0px solid #fff;\" id=\"cmsContentEditLink{0}\" class=\"ui-state-hover\" href=\"javascript:" + JSEditFunction + "\">\r\n"
							+ " Edit <img style=\"margin: 0px; padding:0px; padding-right: 10px;\" border=\"0\" src=\"/manage/images/pencil.png\" alt=\"Edit\" title=\"Edit\" /> </a> ";
				}

				string sPrefix = "";
				if (!string.IsNullOrEmpty(JQueryUIScope)) {
					sPrefix = "<div id=\"" + DatabaseKey + "\" style=\"border: 2px solid #000000; margin: 0px; padding:2px; margin-top:5px;\" >"
										+ "<div id=\"" + this.ClientID + "\" class=\"" + JQueryUIScope + "\">"
										+ "<p id=\"cmsWidgetHead\" class=\"toolitem ui-state-hover ui-widget-header\" style=\"height:30px; margin: 0px; padding:2px;\">" + ControlPath
										+ sEdit + " <a style=\"float:right; color:#676F6A;font-weight: bold;margin: 0px;padding:0px;padding-right: 10px;border: 0px solid #fff;\" id=\"cmsContentLink{0}\" class=\"ui-state-hover\" href=\"javascript:CarrotCMSRemoveWidget('" + DatabaseKey + "');\">\r\n"
										+ " Remove <img style=\"margin: 0px; padding:0px; padding-right: 10px;\" border=\"0\" src=\"/manage/images/cross.png\" alt=\"Remove\" title=\"Remove\" /> </a> </p></div>\r\n"
										+ "<div style=\"border: 2px dashed #ffffff;\" id=\"cmsControl\" >\r\n"
										+ "<input type=\"hidden\" id=\"cmsCtrlID\" value=\"" + DatabaseKey + "\"  />\r\n"
										+ "<input type=\"hidden\" id=\"cmsCtrlOrder\" value=\"" + Order + "\"  />\r\n";
				} else {
					sPrefix = "<div id=\"" + DatabaseKey + "\" style=\"border: 2px solid #000000; margin: 0px; padding:2px; margin-top:5px;\" >"
										+ "<div id=\"" + this.ClientID + "\">"
										+ "<p id=\"cmsWidgetHead\" class=\"toolitem ui-state-hover ui-widget-header\" style=\"height:30px; margin: 0px; padding:2px;\">" + ControlPath
										+ sEdit + " <a style=\"float:right; color:#676F6A;font-weight: bold;margin: 0px;padding:0px;padding-right: 10px;border: 0px solid #fff;\" id=\"cmsContentLink{0}\" class=\"ui-state-hover\" href=\"javascript:CarrotCMSRemoveWidget('" + DatabaseKey + "');\">\r\n"
										+ " Remove <img style=\"margin: 0px; padding:0px; padding-right: 10px;\" border=\"0\" src=\"/manage/images/cross.png\" alt=\"Remove\" title=\"Remove\" /> </a> </p></div>\r\n"
										+ "<div style=\"border: 2px dashed #ffffff;\" id=\"cmsControl\" >\r\n"
										+ "<input type=\"hidden\" id=\"cmsCtrlID\" value=\"" + DatabaseKey + "\"  />\r\n"
										+ "<input type=\"hidden\" id=\"cmsCtrlOrder\" value=\"" + Order + "\"  />\r\n";
				}
				w.Write(sPrefix);

			} else {
				w.Write("<div id=\"" + this.ClientID + "\">");
			}

			base.Render(w);
			if (IsAdminMode) {
				w.Write("\r\n<div style=\"text-align:right\" id=\"ctrlBtn\"></div><br /></div></div>");
			} else {
				w.Write("\r\n</div>");
			}

		}


	}
}
