using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;


namespace Carrotware.Web.UI.Controls {
	public class CaptchaImage {

		public static string EncodeColor(string ColorCode) {
			string sColor = "";
			if (!string.IsNullOrEmpty(ColorCode)) {
				sColor = ColorCode;
				sColor = sColor.Replace("#", "HEX-");
				sColor = HttpUtility.HtmlEncode(sColor);
			}
			return sColor;
		}

		public static string DecodeColor(string ColorCode) {
			string sColor = "";
			if (!string.IsNullOrEmpty(ColorCode)) {
				sColor = ColorCode;
				sColor = HttpUtility.HtmlDecode(sColor);
				sColor = sColor.Replace("HEX-", "#");
			}
			return sColor;
		}


		public static string BGColorDef {
			get {
				string s = (string)HttpContext.Current.Request.QueryString["bgcolor"];
				return ((s == null) ? "#eeeeee" : DecodeColor(s));
			}
		}

		public static string NColorDef {
			get {
				string s = (string)HttpContext.Current.Request.QueryString["ncolor"];
				return ((s == null) ? "#C46314" : DecodeColor(s));
			}
		}

		public static string FGColorDef {
			get {
				string s = (string)HttpContext.Current.Request.QueryString["fgcolor"];
				return ((s == null) ? "#69785F" : DecodeColor(s));
			}
		}

		public static bool Validate(string TestValue) {
			bool bValid = false;
			string guid = GetKey();

			if (TestValue.ToLower() == guid.ToLower()) {
				bValid = true;
			}

			guid = Guid.NewGuid().ToString().Substring(0, 6);
			HttpContext.Current.Session["captcha_key"] = guid;

			return bValid;
		}

		public static Bitmap GetCachedCaptcha() {
			var medGreen = ColorTranslator.FromHtml("#69785F");
			var medOrange = ColorTranslator.FromHtml("#C46314");
			return GetCaptchaImage(medGreen, Color.White, medOrange);
		}

		public static string GetKey() {
			string guid = "";
			try {
				guid = HttpContext.Current.Session["captcha_key"].ToString();
			} catch {
				guid = Guid.NewGuid().ToString().Substring(0, 6);
				HttpContext.Current.Session["captcha_key"] = guid;
			}
			return guid;
		}


		public static Bitmap GetCaptchaImage(Color fg, Color bg, Color n) {
			int imageHeight = 50;
			int topPadding = 2; // top and bottom padding in pixels
			int sidePadding = 3; // side padding in pixels

			SolidBrush textBrush = new SolidBrush(fg);
			Font font = new Font("Verdana", 28, FontStyle.Bold);

			string guid = GetKey();

			Bitmap bitmap = new Bitmap(500, 500);
			Graphics graphics = Graphics.FromImage(bitmap);
			SizeF textSize = graphics.MeasureString(guid, font);

			bitmap.Dispose();
			graphics.Dispose();

			int bitmapWidth = sidePadding * 2 + (int)textSize.Width;
			bitmap = new Bitmap(bitmapWidth, imageHeight);
			graphics = Graphics.FromImage(bitmap);

			Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

			HatchBrush hatch1 = new HatchBrush(HatchStyle.SmallGrid, n, bg);

			HatchBrush hatch2 = new HatchBrush(HatchStyle.DiagonalCross, bg, Color.Transparent);

			graphics.FillRectangle(hatch1, rect);
			graphics.DrawString(guid, font, textBrush, sidePadding, topPadding);
			graphics.FillRectangle(hatch2, rect);

			HttpContext.Current.Response.ContentType = "image/x-png";

			using (MemoryStream memStream = new MemoryStream()) {
				bitmap.Save(memStream, ImageFormat.Png);
			}
			graphics.Dispose();

			return bitmap;
		}


	}
}
