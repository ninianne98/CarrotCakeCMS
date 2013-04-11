﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;


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
				string s = "#EEEEEE";
				try { s = (string)HttpContext.Current.Request.QueryString["bgcolor"]; } catch { }
				return ((s == null) ? "#EEEEEE" : DecodeColor(s));
			}
		}

		public static string NColorDef {
			get {
				string s = "#C46314";
				try { s = (string)HttpContext.Current.Request.QueryString["ncolor"]; } catch { }
				return ((s == null) ? "#C46314" : DecodeColor(s));
			}
		}

		public static string FGColorDef {
			get {
				string s = "#69785F";
				try { s = (string)HttpContext.Current.Request.QueryString["fgcolor"]; } catch { }
				return ((s == null) ? "#69785F" : DecodeColor(s));
			}
		}

		public static bool Validate(string TestValue) {
			bool bValid = false;
			string guid = GetKey();

			if (TestValue.ToLower() == guid.ToLower()) {
				bValid = true;
			}

			if (HttpContext.Current != null) {
				guid = Guid.NewGuid().ToString().Substring(0, 6);
				HttpContext.Current.Session["captcha_key"] = guid;
			}
			return bValid;
		}

		public static Bitmap GetCachedCaptcha() {
			Color medGreen = ColorTranslator.FromHtml("#69785F");
			Color medOrange = ColorTranslator.FromHtml("#C46314");
			return GetCaptchaImage(medGreen, Color.White, medOrange);
		}

		public static string GetKey() {
			string guid = "ABCXYZ";
			if (HttpContext.Current != null) {
				try {
					if (HttpContext.Current.Session["captcha_key"] != null) {
						guid = HttpContext.Current.Session["captcha_key"].ToString();
					} else {
						guid = Guid.NewGuid().ToString().Substring(0, 6);
						HttpContext.Current.Session["captcha_key"] = guid;
					}
				} catch {
					guid = Guid.NewGuid().ToString().Substring(0, 6);
					HttpContext.Current.Session["captcha_key"] = guid;
				}
			}
			return guid.ToUpper();
		}


		public static Bitmap GetCaptchaImage(Color fg, Color bg, Color n) {
			int topPadding = 2; // top and bottom padding in pixels
			int sidePadding = 3; // side padding in pixels

			SolidBrush textBrush = new SolidBrush(fg);
			Font font = new Font(FontFamily.GenericSansSerif, 32, FontStyle.Bold);

			string guid = GetKey();

			Bitmap bmpCaptcha = new Bitmap(500, 500);
			Graphics graphics = Graphics.FromImage(bmpCaptcha);
			SizeF textSize = graphics.MeasureString(guid, font);

			bmpCaptcha.Dispose();
			graphics.Dispose();

			int bitmapWidth = sidePadding * 2 + (int)textSize.Width;
			int bitmapHeight = topPadding * 2 + (int)textSize.Height;
			bmpCaptcha = new Bitmap(bitmapWidth, bitmapHeight);
			graphics = Graphics.FromImage(bmpCaptcha);

			Rectangle rect = new Rectangle(0, 0, bmpCaptcha.Width, bmpCaptcha.Height);

			HatchBrush hatch1 = new HatchBrush(HatchStyle.SmallGrid, n, bg);

			HatchBrush hatch2 = new HatchBrush(HatchStyle.DiagonalCross, bg, Color.Transparent);

			graphics.FillRectangle(hatch1, rect);
			graphics.DrawString(guid, font, textBrush, sidePadding, topPadding);
			graphics.FillRectangle(hatch2, rect);

			HttpContext.Current.Response.ContentType = "image/x-png";

			using (MemoryStream memStream = new MemoryStream()) {
				bmpCaptcha.Save(memStream, ImageFormat.Png);
			}

			textBrush.Dispose();
			font.Dispose();
			hatch1.Dispose();
			hatch2.Dispose();
			graphics.Dispose();

			return bmpCaptcha;
		}


	}
}
