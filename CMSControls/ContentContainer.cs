using Carrotware.CMS.Core;
using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
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

	[DefaultProperty("Text")]
	[Designer(typeof(ContentContainerDesigner))]
	[ToolboxData("<{0}:ContentContainer runat=server></{0}:ContentContainer>")]
	public class ContentContainer : Literal, ICMSCoreControl, INamingContainer {

		[Category("Appearance")]
		[DefaultValue(false)]
		public override bool EnableViewState {
			get {
				string s = (string)ViewState["EnableViewState"];
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
				string s = (string)ViewState["ZoneChar"];
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

		private StringBuilder ScrubCtrl(StringBuilder sb) {
			sb.Replace("{HTML_FLAG}", SiteData.HtmlMode);
			sb.Replace("{PLAIN_FLAG}", SiteData.RawMode);
			sb.Replace("{ZONE_ID}", this.ClientID);
			sb.Replace("{SHORT_ZONE_ID}", this.ID);
			sb.Replace("{ZONE_CHAR}", this.ZoneChar);
			sb.Replace("{ZONE_TYPE}", this.TextZone.ToString());

			return sb;
		}

		private Control _ctrl = new Control();

		private ControlUtilities _cu = new ControlUtilities();

		private Control GetCtrl(Control ctrl) {
			_cu = new ControlUtilities(this);
			var sb = new StringBuilder();

			sb.Append(_cu.GetResourceText("ucAdminContentContainer.ascx"));

			sb = ScrubCtrl(sb);

			Control userControl = _cu.CreateControlFromString(sb.ToString());
			userControl.Page = ctrl.Page;

			return userControl;
		}

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			if (this.TextZone != TextFieldZone.Unknown && (string.IsNullOrEmpty(this.Text) || this.DatabaseKey == Guid.Empty)) {
				ContentPage pageContents = _cu.GetContainerContentPage(this);

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

			string outputText = SiteData.CurrentSite.UpdateContent(this.Text);

			var lit1 = new Literal { Text = string.Empty };
			var lit2 = new Literal { Text = string.Empty };
#if DEBUG
			lit1 = new Literal { Text = "<span style=\"display: none;\" id=\"BEGIN-" + this.ClientID + "\"></span>\r\n" };
			lit2 = new Literal { Text = "<span style=\"display: none;\" id=\"END-" + this.ClientID + "\"></span>\r\n" };
#endif
			var lit = new Literal();

			if (this.IsAdminMode) {
				_ctrl = GetCtrl(this);
				lit = (Literal)_cu.FindControl("litContent", _ctrl);
				lit.Text = outputText;
			} else {
				lit = new Literal { Text = string.Format(Environment.NewLine + " {0} " + Environment.NewLine, outputText) };
				_ctrl.Controls.Add(lit);
			}

			lit.ID = "litContent";

			var idx = _ctrl.Controls.IndexOf(lit);

			_ctrl.Controls.AddAt(idx, lit1);
			_ctrl.Controls.AddAt(idx + 2, lit2);

			_ctrl.RenderControl(writer);
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