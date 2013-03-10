﻿using System;
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

		private Control GetCtrl(string CtrlFile, Control X) {
			cu = new ControlUtilities(this);

			string sCtrl = cu.GetResourceText("Carrotware.CMS.UI.Controls." + CtrlFile + ".ascx");

			sCtrl = sCtrl.Replace("{WIDGETCONTAINER_ID}", this.ID);

			Control userControl = cu.CreateControlFromString(sCtrl);

			return userControl;
		}


		protected Control ctrl1 = new Control();
		protected Control ctrl2 = new Control();

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);

			if (this.IsAdminMode) {
				ctrl1 = GetCtrl("ucAdminWidgetContainer1", this);
				ctrl2 = GetCtrl("ucAdminWidgetContainer2", this);
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
