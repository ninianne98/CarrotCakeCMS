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
	public class WidgetWrapper : PlaceHolder, ICMSCoreControl, INamingContainer {

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

		private Control GetCtrl(string CtrlFile, Control X) {
			cu = new ControlUtilities(this);

			string sCtrl = cu.GetResourceText("Carrotware.CMS.UI.Controls." + CtrlFile + ".ascx");

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


		protected Control ctrl1 = new Control();
		protected Control ctrl2 = new Control();

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);

			if (this.IsAdminMode) {
				ctrl1 = GetCtrl("ucAdminWidget1", this);
				ctrl2 = GetCtrl("ucAdminWidget2", this);

				if (string.IsNullOrEmpty(this.JSEditFunction)) {

					HtmlGenericControl edit = (HtmlGenericControl)cu.FindControl("liEdit", ctrl1);
					HtmlGenericControl hist = (HtmlGenericControl)cu.FindControl("liHistory", ctrl1);

					edit.Visible = false;
					hist.Visible = false;
				}

			} else {
				ctrl1 = new Literal { Text = "<span style=\"display: none;\" id=\"BEGIN-" + this.ClientID + "\"></span>\r\n" };
				ctrl2 = new Literal { Text = "<span style=\"display: none;\" id=\"END-" + this.ClientID + "\"></span>\r\n" };
			}
		}


		protected override void Render(HtmlTextWriter writer) {

			ctrl1.RenderControl(writer);

			foreach (Control c in this.Controls) {
				c.RenderControl(writer);
			}

			ctrl2.RenderControl(writer);
		}


	}
}
