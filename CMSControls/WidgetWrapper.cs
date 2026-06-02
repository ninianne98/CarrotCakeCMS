using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
				string s = (string)ViewState["ControlPath"];
				return ((s == null) ? string.Empty : s);
			}

			set {
				ViewState["ControlPath"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string ControlTitle {
			get {
				string s = (string)ViewState["ControlTitle"];
				return ((s == null) ? string.Empty : s);
			}

			set {
				ViewState["ControlTitle"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string JSEditFunction {
			get {
				string s = (string)ViewState["JSEditFunction"];
				return ((s == null) ? string.Empty : s);
			}
			set {
				ViewState["JSEditFunction"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public Dictionary<string, string> JSEditFunctions { get; set; }

		public Widget WidgetData { get; set; }

		private StringBuilder ScrubCtrl(StringBuilder sb) {
			sb.Replace("{WIDGET_ID}", this.ClientID);
			sb.Replace("{WIDGET_KEY}", this.DatabaseKey.ToString());
			sb.Replace("{WIDGET_ORDER}", this.Order.ToString());
			sb.Replace("{WIDGET_PATH}", this.ControlPath);
			sb.Replace("{WIDGET_TITLE}", this.ControlTitle);
			sb.Replace("{HTML_FLAG}", SiteData.HtmlMode);
			sb.Replace("{PLAIN_FLAG}", SiteData.RawMode);

			if (!string.IsNullOrEmpty(this.JSEditFunction)) {
				sb.Replace("{WIDGET_JS}", this.JSEditFunction);
			}

			return sb;
		}

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

		private ControlUtilities _cu = new ControlUtilities();

		private Control GetCtrl(Control ctrl, string menuText, string menuFunc) {
			_cu = new ControlUtilities(this);
			var sb = new StringBuilder();

			sb.Append(_cu.GetResourceText("ucAdminWidgetMenuItem.ascx"));

			sb = ScrubCtrl(sb);
			sb.Replace("{WIDGET_MENU_TEXT}", menuText);
			sb.Replace("{WIDGET_MENU_JS}", menuFunc);

			Control userControl = _cu.CreateControlFromString(sb.ToString());

			return userControl;
		}

		protected Control _ctrl = new Control();

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);

			//this.ControlPath = this.WidgetSettings.ControlPath;
			//this.ControlTitle = this.WidgetSettings.ControlPath;
			//this.Order = this.WidgetSettings.WidgetOrder;
			this.DatabaseKey = this.WidgetData.Root_WidgetID;

			if (!this.WidgetData.IsWidgetActive) {
				this.ControlTitle = string.Format("{0} {1}", CMSConfigHelper.InactivePagePrefix, this.ControlTitle);
			}
			if (this.WidgetData.IsRetired) {
				this.ControlTitle = string.Format("{0} {1}", CMSConfigHelper.RetiredPagePrefix, this.ControlTitle);
			}
			if (this.WidgetData.IsUnReleased) {
				this.ControlTitle = string.Format("{0} {1}", CMSConfigHelper.UnreleasedPagePrefix, this.ControlTitle);
			}
			if (this.WidgetData.IsWidgetPendingDelete) {
				this.ControlTitle = string.Format("{0} {1}", CMSConfigHelper.PendingDeletePrefix, this.ControlTitle);
			}

			if (SiteData.IsWebView) {
				_cu = new ControlUtilities();

				if (this.IsAdminMode) {
					_ctrl = GetCtrl("ucAdminWidget", this);

					if (this.JSEditFunctions != null && this.JSEditFunctions.Any()) {
						var phMenuItems = (PlaceHolder)_cu.FindControl("phMenuItems", _ctrl);
						foreach (KeyValuePair<string, string> f in this.JSEditFunctions) {
							Control itm = GetCtrl(this, f.Key, f.Value);
							phMenuItems.Controls.Add(itm);
						}
						this.JSEditFunction = null;
					}

					var remove = (HtmlGenericControl)_cu.FindControl("liRemove", _ctrl);
					var act = (HtmlGenericControl)_cu.FindControl("liActivate", _ctrl);

					act.Visible = !this.WidgetData.IsWidgetActive;
					remove.Visible = this.WidgetData.IsWidgetActive;

					if (string.IsNullOrEmpty(this.JSEditFunction)) {
						var edit = (HtmlGenericControl)_cu.FindControl("liEdit", _ctrl);
						var hist = (HtmlGenericControl)_cu.FindControl("liHistory", _ctrl);

						edit.Visible = false;
						hist.Visible = false;
					}
				} else {
					_ctrl = new PlaceHolder();
				}
			}
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