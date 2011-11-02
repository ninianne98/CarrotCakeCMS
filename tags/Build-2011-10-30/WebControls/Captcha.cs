using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Carrotware.Web.UI.Controls {
	[DefaultProperty("CaptchaText")]
	[ToolboxData("<{0}:Captcha runat=server></{0}:Captcha>")]

	[ValidationPropertyAttribute("CaptchaText")]
	public class Captcha : BaseWebControl {

		public string CaptchaText {
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
		public Color ForeColor {
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
		public Color BackColor {
			get {
				string s = (string)ViewState["BackColor"];
				return ((s == null) ? ColorTranslator.FromHtml(CaptchaImage.BGColorDef) : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["BackColor"] = ColorTranslator.ToHtml(value);
			}
		}


		public bool Validate() {
			IsValid = CaptchaImage.Validate(CaptchaText);

			if (!IsValid) {
				ValidationMessage = "Code incorrect, try again!";
			} else {
				ValidationMessage = "Code correct!";
			}

			return IsValid;
		}

		protected override void RenderContents(HtmlTextWriter output) {

			var key = CaptchaImage.GetKey();

			output.Write("<div style=\"clear: both;\" id=\"" + this.ClientID + "_wrapper\">\r\n");

			if (!string.IsNullOrEmpty(ValidationMessage)) {
				if (IsValid) {
					output.Write("<div style=\"clear: both; color:green;\" ");
				} else {
					output.Write("<div style=\"clear: both; color:red;\" ");
				}
				output.Write(" id=\"" + this.ClientID + "_msg\">\r\n");
				output.Write(ValidationMessage);
				output.Write("\r\n</div>\r\n");
			}

			output.Write("<div style=\"clear: both;\"> <a href=\"javascript:Show" + this.ClientID + "();\"> ");
			output.Write("<img title=\"" + key + "\" alt=\"" + key + "\" border=\"0\" id=\"" + this.ClientID + "_img\" src=\"/CarrotwareCaptcha.axd?t=" + DateTime.Now.Ticks);
			output.Write("&fgcolor=" + CaptchaImage.EncodeColor(ColorTranslator.ToHtml(ForeColor)) +
					"&bgcolor=" + CaptchaImage.EncodeColor(ColorTranslator.ToHtml(BackColor)) +
					"&ncolor=" + CaptchaImage.EncodeColor(ColorTranslator.ToHtml(NoiseColor)) + "\" /> </a> </div>\r\n");
			output.Write("<div style=\"clear: both;\">" + Instructions + " </div>\r\n");
			output.Write("<div style=\"clear: both;\"><input type=\"text\" id=\"" + this.ClientID + "\" name=\"" + this.UniqueID + "\" value=\"" + HttpUtility.HtmlEncode(CaptchaText) + "\"> </div>\r\n");
			output.Write("</div>\r\n");

			output.Write("\r\n<script  type=\"text/javascript\">\r\n");
			output.Write("\r\nfunction Show" + this.ClientID + "(){\r\n");
			output.Write("alert('" + key.Substring(0, 3) + "' + '" + key.Substring(3) + "');\r\n");
			output.Write("}\r\n");
			output.Write("</script>\r\n");

		}

		public override string ToString() {
			return CaptchaText;
		}


		protected override void OnInit(EventArgs e) {
			if (HttpContext.Current.Request.Form.Count > 0) {
				var s = HttpContext.Current.Request.Form[this.UniqueID];
				CaptchaText = s;
			}
			base.OnInit(e);
		}


	}
}
