﻿using Carrotware.CMS.Core;
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
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
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
				String s = (String)ViewState["ControlTitle"];
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
				String s = (String)ViewState["JSEditFunction"];
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

		private Control GetCtrl(string CtrlFile, Control X) {
			ControlUtilities cu = new ControlUtilities(this);

			var sb = new StringBuilder();
			sb.Append(cu.GetResourceText("Carrotware.CMS.UI.Controls." + CtrlFile + ".ascx"));

			sb = ScrubCtrl(sb);

			Control userControl = cu.CreateControlFromString(sb.ToString());

			return userControl;
		}

		private Control GetCtrl(Control X, string MenuText, string MenuFunc) {
			ControlUtilities cu = new ControlUtilities(this);

			var sb = new StringBuilder();
			sb.Append(cu.GetResourceText("Carrotware.CMS.UI.Controls.ucAdminWidgetMenuItem.ascx"));

			sb = ScrubCtrl(sb);
			sb.Replace("{WIDGET_MENU_TEXT}", MenuText);
			sb.Replace("{WIDGET_MENU_JS}", MenuFunc);

			Control userControl = cu.CreateControlFromString(sb.ToString());

			return userControl;
		}

		protected Control ctrl1 = new Control();
		protected Control ctrl2 = new Control();

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
				ControlUtilities cu = new ControlUtilities();

				if (this.IsAdminMode) {
					ctrl1 = GetCtrl("ucAdminWidget1", this);
					ctrl2 = GetCtrl("ucAdminWidget2", this);

					if (this.JSEditFunctions != null && this.JSEditFunctions.Any()) {
						PlaceHolder phMenuItems = (PlaceHolder)cu.FindControl("phMenuItems", ctrl1);
						foreach (KeyValuePair<string, string> f in this.JSEditFunctions) {
							Control itm = GetCtrl(this, f.Key, f.Value);
							phMenuItems.Controls.Add(itm);
						}
						this.JSEditFunction = null;
					}

					HtmlGenericControl remove = (HtmlGenericControl)cu.FindControl("liRemove", ctrl1);
					HtmlGenericControl act = (HtmlGenericControl)cu.FindControl("liActivate", ctrl1);

					act.Visible = !this.WidgetData.IsWidgetActive;
					remove.Visible = this.WidgetData.IsWidgetActive;

					if (string.IsNullOrEmpty(this.JSEditFunction)) {
						HtmlGenericControl edit = (HtmlGenericControl)cu.FindControl("liEdit", ctrl1);
						HtmlGenericControl hist = (HtmlGenericControl)cu.FindControl("liHistory", ctrl1);

						edit.Visible = false;
						hist.Visible = false;
					}
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