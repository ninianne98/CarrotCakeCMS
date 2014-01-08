using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
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

namespace Carrotware.Web.UI.Controls {
	[DefaultProperty("CaptchaText")]
	[ToolboxData("<{0}:Captcha runat=server></{0}:Captcha>")]

	[ValidationPropertyAttribute("CaptchaText")]
	public class Captcha : BaseWebControl, ITextControl {

		public Captcha() {

			this.CaptchaImageBoxStyle = new SimpleStyle();
			this.CaptchaTextStyle = new SimpleStyle();
			this.CaptchaInstructionStyle = new SimpleStyle();
			this.CaptchaImageStyle = new SimpleStyle();

			this.CaptchaIsValidStyle = new SimpleStyle();
			this.CaptchaIsNotValidStyle = new SimpleStyle();

		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string Text {
			get {
				String s = (String)ViewState["Text"];
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["Text"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CaptchaText {
			get {
				return this.Text;
			}
			set {
				this.Text = value;
			}
		}

		public string ValidationGroup {
			get;
			set;
		}

		public string ValidationMessage {
			get;
			set;
		}

		private bool IsValid {
			get;
			set;
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string Instructions {
			get {
				string s = (string)ViewState["Instructions"];
				return ((s == null) ? "Please enter the code from the image above in the box below." : s);
			}
			set {
				ViewState["Instructions"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string IsValidMessage {
			get {
				string s = (string)ViewState["ValidMessage"];
				return ((s == null) ? "Code correct!" : s);
			}
			set {
				ViewState["ValidMessage"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string IsNotValidMessage {
			get {
				string s = (string)ViewState["IsNotValidMessage"];
				return ((s == null) ? "Code incorrect, try again!" : s);
			}
			set {
				ViewState["IsNotValidMessage"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public override Color ForeColor {
			get {
				string s = (string)ViewState["ForeColor"];
				return ((s == null) ? ColorTranslator.FromHtml(CaptchaImage.FGColorDef) : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["ForeColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color NoiseColor {
			get {
				string s = (string)ViewState["NoiseColor"];
				return ((s == null) ? ColorTranslator.FromHtml(CaptchaImage.NColorDef) : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["NoiseColor"] = ColorTranslator.ToHtml(value);
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public override Color BackColor {
			get {
				string s = (string)ViewState["BackColor"];
				return ((s == null) ? ColorTranslator.FromHtml(CaptchaImage.BGColorDef) : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["BackColor"] = ColorTranslator.ToHtml(value);
			}
		}


		[NotifyParentProperty(true)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SimpleStyle CaptchaImageBoxStyle { get; set; }

		[NotifyParentProperty(true)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SimpleStyle CaptchaTextStyle { get; set; }

		[NotifyParentProperty(true)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SimpleStyle CaptchaInstructionStyle { get; set; }

		[NotifyParentProperty(true)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SimpleStyle CaptchaImageStyle { get; set; }

		[NotifyParentProperty(true)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SimpleStyle CaptchaIsValidStyle { get; set; }

		[NotifyParentProperty(true)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SimpleStyle CaptchaIsNotValidStyle { get; set; }


		public bool Validate() {
			this.IsValid = CaptchaImage.Validate(this.CaptchaText);

			if (!this.IsValid) {
				this.ValidationMessage = this.IsNotValidMessage;
			} else {
				this.ValidationMessage = this.IsValidMessage;
			}

			return this.IsValid;
		}

		protected override void RenderContents(HtmlTextWriter output) {

			var key = CaptchaImage.GetKey();

			output.Write("<div style=\"clear: both;\" id=\"" + this.ClientID + "_wrapper\">\r\n");

			if (!string.IsNullOrEmpty(this.ValidationMessage)) {
				output.Write("<div");

				if (this.IsValid) {
					output.Write(this.CaptchaIsValidStyle.ToString());
				} else {
					output.Write(this.CaptchaIsNotValidStyle.ToString());
				}

				output.Write(" id=\"" + this.ClientID + "_msg\">\r\n");
				output.Write(this.ValidationMessage);
				output.Write("\r\n</div>\r\n");
			}

			string sJSFuncName = "Show_" + this.ClientID;

			output.Write("<div" + this.CaptchaImageBoxStyle.ToString() + "> ");
			output.Write("<a href=\"javascript:" + sJSFuncName + "();\"> <img" + this.CaptchaImageStyle.ToString() + " title=\"" + key + "\" alt=\"" + key + "\" border=\"0\" id=\""
				+ this.ClientID + "_img\" src=\"" + GetCaptchaImageURI() + "\" /> </a> \r\n");

			output.Write("</div>\r\n");
			output.Write("<div" + this.CaptchaInstructionStyle.ToString() + ">" + this.Instructions + " </div>\r\n");
			output.Write("<div" + this.CaptchaTextStyle.ToString() + "><input type=\"text\" id=\"" + this.ClientID + "\" name=\"" + this.UniqueID + "\" value=\"" + HttpUtility.HtmlEncode(this.CaptchaText) + "\" /> </div>\r\n");

			output.Write("\r\n<script  type=\"text/javascript\">\r\n");
			output.Write("\r\nfunction " + sJSFuncName + "(){\r\n");
			if (!string.IsNullOrEmpty(key)) {
				output.Write("alert('" + key.Substring(0, 3) + "' + '" + key.Substring(3) + "');\r\n");
			} else {
				output.Write("alert('no code');\r\n");
			}
			output.Write("}\r\n");
			output.Write("</script>\r\n");

			output.Write("\r\n</div>\r\n");
		}

		private string GetCaptchaImageURI() {

			if (this.IsWebView) {
				return "/CarrotwareCaptcha.axd?t=" + DateTime.Now.Ticks +
						"&fgcolor=" + CaptchaImage.EncodeColor(ColorTranslator.ToHtml(this.ForeColor)) +
						"&bgcolor=" + CaptchaImage.EncodeColor(ColorTranslator.ToHtml(this.BackColor)) +
						"&ncolor=" + CaptchaImage.EncodeColor(ColorTranslator.ToHtml(this.NoiseColor));
			} else {
				return "/CarrotwareCaptcha.axd?t=" + DateTime.Now.Ticks;
			}
		}

		public override string ToString() {
			return this.CaptchaText;
		}

		protected override void OnInit(EventArgs e) {


			if (this.IsWebView) {
				if (HttpContext.Current.Request.Form.Count > 0) {
					var s = HttpContext.Current.Request.Form[this.UniqueID];
					this.CaptchaText = s;
				}
			}

			base.OnInit(e);
		}


	}
}
