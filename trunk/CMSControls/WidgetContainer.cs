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

		[Category("Appearance")]
		[DefaultValue(false)]
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

		private ControlUtilities cu = new ControlUtilities();

		private Control GetCtrl(Control X) {
			cu = new ControlUtilities(this);

			string sCtrl = cu.GetResourceText("Carrotware.CMS.UI.Controls.ucAdminWidgetContainer.ascx");

			sCtrl = sCtrl.Replace("{WIDGETCONTAINER_ID}", this.ID);

			Control userControl = cu.CreateControlFromString(sCtrl);

			return userControl;
		}

		protected override void Render(HtmlTextWriter w) {

			Control ctrl = new Control();
			PlaceHolder ph = new PlaceHolder();

			if (this.IsAdminMode) {

				ctrl = GetCtrl(this);
				ph = (PlaceHolder)cu.FindControl("phWidgetZone", ctrl);

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
