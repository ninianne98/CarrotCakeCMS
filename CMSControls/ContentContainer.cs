﻿using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using System.Text;

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

	[DefaultProperty("Text")]
	[Designer(typeof(ContentContainerDesigner))]
	[ToolboxData("<{0}:ContentContainer runat=server></{0}:ContentContainer>")]
	public class ContentContainer : Literal, ICMSCoreControl, INamingContainer {

		[Category("Appearance")]
		[DefaultValue(false)]
		public override bool EnableViewState {
			get {
				String s = (String)ViewState["EnableViewState"];
				bool b = ((s == null) ? false : Convert.ToBoolean(s));
				base.EnableViewState = b;
				return b;
			}

			set {
				ViewState["EnableViewState"] = value.ToString();
				base.EnableViewState = value;
			}
		}

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
		[DefaultValue("")]
		public string ZoneChar {
			get {
				String s = (String)ViewState["ZoneChar"];
				return ((s == null) ? string.Empty : s);
			}

			set {
				ViewState["ZoneChar"] = value;
			}
		}

		public enum TextFieldZone {
			Unknown,
			TextLeft,
			TextCenter,
			TextRight,
		}

		[Category("Appearance")]
		[DefaultValue("Unknown")]
		public TextFieldZone TextZone {
			get {
				string s = (string)ViewState["TextZone"];
				TextFieldZone c = TextFieldZone.Unknown;
				if (!string.IsNullOrEmpty(s)) {
					c = (TextFieldZone)Enum.Parse(typeof(TextFieldZone), s, true);
				}
				return c;
			}
			set {
				ViewState["TextZone"] = value.ToString();
			}
		}

		private ControlUtilities cu = new ControlUtilities();

		private Control GetCtrl(Control X) {
			cu = new ControlUtilities(this);
			var sb = new StringBuilder();
			sb.Append(cu.GetResourceText("Carrotware.CMS.UI.Controls.ucAdminContentContainer.ascx"));

			sb.Replace("{HTML_FLAG}", SiteData.HtmlMode);
			sb.Replace("{PLAIN_FLAG}", SiteData.RawMode);
			sb.Replace("{ZONE_ID}", this.ClientID);
			sb.Replace("{SHORT_ZONE_ID}", this.ID);
			sb.Replace("{ZONE_CHAR}", this.ZoneChar);
			sb.Replace("{ZONE_TYPE}", this.TextZone.ToString());

			Control userControl = cu.CreateControlFromString(sb.ToString());

			return userControl;
		}

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			if (this.TextZone != TextFieldZone.Unknown && (string.IsNullOrEmpty(this.Text) || this.DatabaseKey == Guid.Empty)) {
				ContentPage pageContents = cu.GetContainerContentPage(this);

				if (pageContents != null) {
					this.DatabaseKey = pageContents.Root_ContentID;
					this.IsAdminMode = SecurityData.AdvancedEditMode;

					switch (this.TextZone) {
						case TextFieldZone.TextLeft:
							this.ZoneChar = "l";
							this.Text = pageContents.LeftPageText;
							break;

						case TextFieldZone.TextCenter:
							this.ZoneChar = "c";
							this.Text = pageContents.PageText;
							break;

						case TextFieldZone.TextRight:
							this.ZoneChar = "r";
							this.Text = pageContents.RightPageText;
							break;

						default:
							break;
					}
				}
			}

			Control ctrl = new Control();

			string outputText = SiteData.CurrentSite.UpdateContent(this.Text);

			if (IsAdminMode) {
				ctrl = GetCtrl(this);
				Literal lit = (Literal)cu.FindControl("litContent", ctrl);
				lit.Text = outputText;
			} else {
#if DEBUG
				ctrl.Controls.Add(new Literal { Text = "\r\n<span style=\"display: none;\" id=\"BEGIN-" + this.ClientID + "\"></span>" });
#endif
				ctrl.Controls.Add(new Literal { Text = string.Format("\r\n {0} \r\n", outputText) });
#if DEBUG
				ctrl.Controls.Add(new Literal { Text = "<span style=\"display: none;\" id=\"END-" + this.ClientID + "\"></span>\r\n" });
#endif
			}

			ctrl.RenderControl(writer);
		}
	}

	//=======================

	public class ContentContainerDesigner : ControlDesigner {

		public override string GetDesignTimeHtml() {
			ContentContainer myctrl = (ContentContainer)base.ViewControl;
			string sType = myctrl.GetType().ToString().Replace(myctrl.GetType().Namespace + ".", "CMS, ");
			string sID = myctrl.ID;

			string sTextOut = "<span>[" + sType + " - " + sID + "]</span>\r\n";
			string sPageOutText = string.Empty;

			string sPageText = SiteNavHelper.GetSampleBody("SampleContent3");
			if (myctrl.TextZone == ContentContainer.TextFieldZone.Unknown) {
				myctrl.TextZone = ContentContainer.TextFieldZone.TextCenter;
			}
			sPageOutText = "<h2>Content D CENTER</h2>\r\n" + sPageText;
			if (myctrl.ClientID.ToLowerInvariant().Contains("left") || myctrl.TextZone == ContentContainer.TextFieldZone.TextLeft) {
				sPageOutText = "<h2>Content D LEFT</h2>\r\n" + sPageText;
			}

			if (myctrl.ClientID.ToLowerInvariant().Contains("right") || myctrl.TextZone == ContentContainer.TextFieldZone.TextRight) {
				sPageOutText = "<h2>Content D RIGHT</h2>\r\n" + sPageText;
			}

			return sTextOut + sPageOutText;
		}
	}
}