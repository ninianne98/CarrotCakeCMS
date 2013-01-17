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

		[DefaultValue(false)]
		[Themeable(false)]
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

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
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


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
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

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
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


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("NavMenuText")]
		[Localizable(true)]
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

		protected override void Render(HtmlTextWriter w) {

			if (this.TextZone != TextFieldZone.Unknown && (string.IsNullOrEmpty(this.Text) || this.DatabaseKey == Guid.Empty)) {

				ControlUtilities cu = new ControlUtilities();
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

			if (IsAdminMode) {

				string sPrefix = "<div style=\"clear: both;\"></div>\r\n<div id=\"cms_" + this.ClientID + "\" class=\"cmsContentContainer\">\r\n" +
							" <div class=\"cmsContentContainerTitle\">\r\n" +
							"<a title=\"Edit " + this.ID + "\" id=\"cmsContentAreaLink_" + this.ClientID + "\" class=\"cmsContentContainerInnerTitle\" " +
							" href=\"javascript:cmsShowEditContentForm('" + this.ZoneChar + "','html'); \"> " +
							" Edit " + this.ID + " <span class=\"cmsWidgetBarIconPencil2\" /></span> </a></div> \r\n" +
							"<div class=\"cmsWidgetControl\" id=\"cmsAdmin_" + this.ClientID + "\" ><div>\r\n";

				w.Write(sPrefix);

			} else {
				w.WriteLine("<span style=\"display: none;\" id=\"BEGIN-" + this.ClientID + "\"></span>");
			}

			base.Render(w);

			if (IsAdminMode) {
				w.Write("\r\n</div>\r\n<div style=\"clear: both; \"></div>\r\n</div></div>\r\n");
			} else {
				w.WriteLine("<span style=\"display: none;\" id=\"END-" + this.ClientID + "\"></span>");
			}
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
