using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
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

	[DefaultProperty("Text")]
	[Designer(typeof(ContentContainerDesigner))]
	[ToolboxData("<{0}:ContentContainer runat=server></{0}:ContentContainer>")]
	public class ContentContainer : Literal, ICMSCoreControl {

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
				return ((s == null) ? String.Empty : s);
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

			string sCtrl = cu.GetResourceText("Carrotware.CMS.UI.Controls.ucAdminContentContainer.ascx");

			sCtrl = sCtrl.Replace("{ZONE_ID}", this.ClientID);
			sCtrl = sCtrl.Replace("{ZONE_CHAR}", this.ZoneChar);

			Control userControl = cu.CreateControlFromString(sCtrl);

			return userControl;
		}

		protected override void Render(HtmlTextWriter w) {

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

			if (IsAdminMode) {

				ctrl = GetCtrl(this);
				Literal lit = (Literal)cu.FindControl("litContent", ctrl);
				lit.Text = this.Text;

			} else {
				ctrl.Controls.Add(new Literal { Text = "<span style=\"display: none;\" id=\"BEGIN-" + this.ClientID + "\"></span>\r\n" });
				ctrl.Controls.Add(new Literal { Text = this.Text });
				ctrl.Controls.Add(new Literal { Text = "<span style=\"display: none;\" id=\"END-" + this.ClientID + "\"></span>\r\n" });
			}

			ctrl.RenderControl(w);

		}

	}

	//=======================

	public class ContentContainerDesigner : ControlDesigner {

		public override string GetDesignTimeHtml() {
			ContentContainer myctrl = (ContentContainer)base.ViewControl;
			string sType = myctrl.GetType().ToString().Replace(myctrl.GetType().Namespace + ".", "CMS, ");
			string sID = myctrl.ID;

			string sTextOut = "<span>[" + sType + " - " + sID + "]</span>\r\n";
			string sPageOutText = "";

			string sPageText = SiteNavHelper.GetSampleBody(myctrl, "SampleContent3");
			if (myctrl.TextZone == ContentContainer.TextFieldZone.Unknown) {
				myctrl.TextZone = ContentContainer.TextFieldZone.TextCenter;
			}
			sPageOutText = "<h2>Content D CENTER</h2>\r\n" + sPageText;
			if (myctrl.ClientID.ToLower().Contains("left") || myctrl.TextZone == ContentContainer.TextFieldZone.TextLeft) {
				sPageOutText = "<h2>Content D LEFT</h2>\r\n" + sPageText;
			}

			if (myctrl.ClientID.ToLower().Contains("right") || myctrl.TextZone == ContentContainer.TextFieldZone.TextRight) {
				sPageOutText = "<h2>Content D RIGHT</h2>\r\n" + sPageText;
			}

			return sTextOut + sPageOutText;
		}

	}

}
