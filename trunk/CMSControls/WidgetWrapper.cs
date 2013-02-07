using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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

		[Category("Appearance")]
		[DefaultValue(false)]
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

		[Category("Appearance")]
		[DefaultValue(null)]
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

		[Category("Appearance")]
		[DefaultValue(0)]
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

		[Category("Appearance")]
		[DefaultValue("")]
		public string ControlPath {
			get {
				String s = (String)ViewState["ControlPath"];
				return ((s == null) ? String.Empty : s);
			}

			set {
				ViewState["ControlPath"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string ControlTitle {
			get {
				String s = (String)ViewState["ControlTitle"];
				return ((s == null) ? String.Empty : s);
			}

			set {
				ViewState["ControlTitle"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string JSEditFunction {
			get {
				String s = (String)ViewState["JSEditFunction"];
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["JSEditFunction"] = value;
			}
		}

		private ControlUtilities cu = new ControlUtilities();

		private Control GetCtrl(Control X) {
			cu = new ControlUtilities(this);

			string sCtrl = cu.GetResourceText("Carrotware.CMS.UI.Controls.ucAdminWidget.ascx");

			sCtrl = sCtrl.Replace("{WIDGET_ID}", this.ClientID);
			sCtrl = sCtrl.Replace("{WIDGET_KEY}", this.DatabaseKey.ToString());
			sCtrl = sCtrl.Replace("{WIDGET_ORDER}", this.Order.ToString());
			sCtrl = sCtrl.Replace("{WIDGET_PATH}", this.ControlPath);
			sCtrl = sCtrl.Replace("{WIDGET_TITLE}", this.ControlTitle);
			if (!string.IsNullOrEmpty(this.JSEditFunction)) {
				sCtrl = sCtrl.Replace("{WIDGET_JS}", this.JSEditFunction);
			}

			Control userControl = cu.CreateControlFromString(sCtrl);

			return userControl;
		}


		protected override void Render(HtmlTextWriter w) {

			Control ctrl = new Control();
			PlaceHolder ph = new PlaceHolder();

			if (this.IsAdminMode) {

				ctrl = GetCtrl(this);
				ph = (PlaceHolder)cu.FindControl("phWidgetZone", ctrl);

				if (string.IsNullOrEmpty(this.JSEditFunction)) {

					HtmlGenericControl edit = (HtmlGenericControl)cu.FindControl("liEdit", ctrl);
					HtmlGenericControl hist = (HtmlGenericControl)cu.FindControl("liHistory", ctrl);

					edit.Visible = false;
					hist.Visible = false;
				}

			} else {
				ctrl.Controls.Add(new Literal { Text = "<span style=\"display: none;\" id=\"BEGIN-" + this.ClientID + "\"></span>\r\n" });
				ctrl.Controls.Add(ph);
				ctrl.Controls.Add(new Literal { Text = "<span style=\"display: none;\" id=\"END-" + this.ClientID + "\"></span>\r\n" });
			}

			int iChild = this.Controls.Count;

			for (int i = 0; i < iChild; i++) {
				ph.Controls.Add(this.Controls[0]);
			}

			ctrl.RenderControl(w);

		}

	}
}
