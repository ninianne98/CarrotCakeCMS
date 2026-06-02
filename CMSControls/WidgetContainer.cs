using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
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

		public Guid DatabaseKey { get; set; } = Guid.Empty;

		private StringBuilder ScrubCtrl(StringBuilder sb) {
			sb.Replace("{WIDGETCONTAINER_ID}", this.ID);

			return sb;
		}

		private ControlUtilities _cu = new ControlUtilities();

		private Control GetCtrl(string ctrlFile, Control ctrl) {
			_cu = new ControlUtilities(this);
			var sb = new StringBuilder();

			var txt = ControlUtilities.GetManifestResourceStream(ctrlFile + ".ascx");
			sb.Append(txt);

			sb = ScrubCtrl(sb);

			Control userControl = _cu.CreateControlFromString(sb);
			userControl.Page = ctrl.Page;

			return userControl;
		}

		protected Control _ctrl = new Control();

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);

			if (SiteData.IsWebView) {
				if (this.IsAdminMode) {
					_ctrl = GetCtrl("ucAdminWidgetContainer", this);
				} else {
					_ctrl = new PlaceHolder();
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

			_cu = new ControlUtilities(this);

			var lit1 = new Literal { Text = string.Empty };
			var lit2 = new Literal { Text = string.Empty };
#if DEBUG
			lit1 = new Literal { Text = "<span style=\"display: none;\" id=\"BEGIN-" + this.ClientID + "\"></span>\r\n" };
			lit2 = new Literal { Text = "<span style=\"display: none;\" id=\"END-" + this.ClientID + "\"></span>\r\n" };
#endif
			var widgetBody = (PlaceHolder)_cu.FindControl("phWidgetZone", _ctrl);
			if (widgetBody == null && _ctrl is PlaceHolder) {
				widgetBody = (PlaceHolder)_ctrl;
			}
			widgetBody.ID = "phWidgetZone";

			_ctrl.Controls.Add(lit1);

			foreach (Control c in this.Controls) {
				var txt = BasicControlUtils.GetCtrlText(c);
				var lit = new Literal { Text = txt };

				widgetBody.Controls.Add(lit);
			}

			_ctrl.Controls.Add(lit2);

			_ctrl.RenderControl(writer);
		}
	}
}