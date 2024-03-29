﻿using Carrotware.CMS.Core;
using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
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

		private Control GetCtrl(string CtrlFile, Control X) {
			ControlUtilities cu = new ControlUtilities(this);
			var sb = new StringBuilder();
			sb.Append(cu.GetResourceText("Carrotware.CMS.UI.Controls." + CtrlFile + ".ascx"));

			sb.Replace("{WIDGETCONTAINER_ID}", this.ID);

			Control userControl = cu.CreateControlFromString(sb.ToString());

			return userControl;
		}

		protected Control ctrl1 = new Control();
		protected Control ctrl2 = new Control();

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);

			if (SiteData.IsWebView) {
				if (this.IsAdminMode) {
					ctrl1 = GetCtrl("ucAdminWidgetContainer1", this);
					ctrl2 = GetCtrl("ucAdminWidgetContainer2", this);
				} else {
					ctrl1 = new Literal { Text = "\r\n" };
					ctrl2 = new Literal { Text = "\r\n" };
#if DEBUG
					ctrl1 = new Literal { Text = "<span style=\"display: none;\" id=\"BEGIN-" + this.ClientID + "\"></span>\r\n" };
					ctrl2 = new Literal { Text = "<span style=\"display: none;\" id=\"END-" + this.ClientID + "\"></span>\r\n" };
#endif
				}
			}
		}

		public WidgetWrapper AddWidget(Control widget, Widget widgetData) {
			WidgetWrapper wrapper = new WidgetWrapper();

			wrapper.WidgetData = widgetData;

			wrapper.IsAdminMode = true;
			wrapper.ControlPath = widgetData.ControlPath;
			wrapper.ControlTitle = widgetData.ControlPath;

			wrapper.Order = widgetData.WidgetOrder;
			wrapper.DatabaseKey = widgetData.Root_WidgetID;

			AddWidget(widget, wrapper);

			return wrapper;
		}

		public void AddWidget(Control widget, WidgetWrapper wrapper) {
			wrapper.Controls.Add(widget);
			this.Controls.Add(wrapper);
		}

		public void AddWidget(Control widget) {
			this.Controls.Add(widget);
		}

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			ctrl1.RenderControl(writer);

			foreach (Control c in this.Controls) {
				c.RenderControl(writer);
			}

			ctrl2.RenderControl(writer);
		}
	}
}