using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.Web.UI.Controls {

	internal class CarrotHttpHandler : IHttpHandler, IRequiresSessionState {

		public bool IsReusable {
			get { return true; }
		}

		public void ProcessRequest(HttpContext context) {
			if (context.Request.Path.ToLowerInvariant() == "/carrotwarecaptcha.axd") {
				DoCaptcha(context);
			}

			if (context.Request.Path.ToLowerInvariant() == "/carrotwarethumb.axd") {
				DoThumb(context);
			}

			if (context.Request.Path.ToLowerInvariant() == "/carrotwarehelper.axd") {
				DoHelper(context);
			}
		}

		private Bitmap ResizeBitmap(Bitmap bmpIn, int w, int h) {
			if (w < 1) {
				w = 1;
			}
			if (h < 1) {
				h = 1;
			}

			Bitmap bmpNew = new Bitmap(w, h);
			using (Graphics g = Graphics.FromImage(bmpNew)) {
				g.DrawImage(bmpIn, 0, 0, w, h);
			}
			return bmpNew;
		}

		private void DoCaptcha(HttpContext context) {
			context.Response.Cache.VaryByParams["fgcolor"] = true;
			context.Response.Cache.VaryByParams["bgcolor"] = true;
			context.Response.Cache.VaryByParams["ncolor"] = true;

			DoCacheMagic(context, 3);

			Color f = ColorTranslator.FromHtml(CaptchaImage.FGColorDef);
			Color b = ColorTranslator.FromHtml(CaptchaImage.BGColorDef);
			Color n = ColorTranslator.FromHtml(CaptchaImage.NColorDef);

			Bitmap captchaImg = CaptchaImage.GetCaptchaImage(f, b, n);

			if (captchaImg == null) {
				context.Response.StatusCode = 404;
				context.Response.StatusDescription = "Not Found";
				context.ApplicationInstance.CompleteRequest();
				return;
			}

			context.Response.ContentType = "image/x-png";

			using (var memStream = new MemoryStream()) {
				captchaImg.Save(memStream, ImageFormat.Png);
				memStream.WriteTo(context.Response.OutputStream);
			}

			captchaImg.Dispose();

			context.ApplicationInstance.CompleteRequest();
			context.Response.End();
		}

		private void DoThumb(HttpContext context) {
			context.Response.Cache.VaryByParams["thumb"] = true;
			context.Response.Cache.VaryByParams["scale"] = true;
			context.Response.Cache.VaryByParams["square"] = true;

			DoCacheMagic(context, 3);

			int iThumb = 150;
			string sImageIn = context.Request.QueryString["thumb"];
			string sImg = sImageIn;

			if (sImageIn.Contains("../") || sImageIn.Contains(@"..\")) {
				throw new Exception("Cannot use relative paths.");
			}
			if (sImageIn.Contains(":")) {
				throw new Exception("Cannot specify drive letters.");
			}
			if (sImageIn.Contains("//") || sImageIn.Contains(@"\\")) {
				throw new Exception("Cannot use UNC paths.");
			}

			string sScale = "false";
			if (context.Request.QueryString["scale"] != null) {
				sScale = context.Request.QueryString["scale"].ToLowerInvariant();
			}

			if (context.Request.QueryString["square"] != null) {
				string sImgPX = context.Request.QueryString["square"];
				if (!string.IsNullOrEmpty(sImgPX)) {
					try {
						iThumb = int.Parse(sImgPX);
					} catch { }
					if (iThumb < 10 || iThumb > 500) {
						iThumb = 100;
					}
				}
			}

			Bitmap bmpIn = new Bitmap(25, 25);
			Bitmap bmpThumb = new Bitmap(iThumb, iThumb);

			int iHeight = iThumb;
			int iWidth = iThumb;

			if (!string.IsNullOrEmpty(sImageIn)) {
				sImg = context.Server.MapPath(sImageIn);
				if (File.Exists(sImg)) {
					bmpIn = new Bitmap(sImg);

					if (sScale == "true") {
						int h = bmpIn.Height;
						int w = bmpIn.Width;

						if (iHeight > 0) {
							iWidth = (int)(((float)w / (float)h) * (float)iHeight);
						} else {
							iHeight = h;
							iWidth = w;
						}
					}

					bmpThumb = ResizeBitmap(bmpIn, iWidth, iHeight);
				} else {
					using (Graphics graphics = Graphics.FromImage(bmpThumb)) {
						Rectangle rect = new Rectangle(0, 0, bmpThumb.Width, bmpThumb.Height);
						using (HatchBrush hatch = new HatchBrush(HatchStyle.Weave, Color.BurlyWood, Color.AntiqueWhite)) {
							graphics.FillRectangle(hatch, rect);
							int topPadding = 2; // top and bottom padding in pixels
							int sidePadding = 2; // side padding in pixels
							Font font = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
							SolidBrush textBrush = new SolidBrush(Color.Black);

							if (sImageIn.Contains("/")) {
								sImageIn = sImageIn.Substring(sImageIn.LastIndexOf("/") + 1);
							}

							sImageIn = "404 \r\n" + sImageIn;

							graphics.DrawString(sImageIn, font, textBrush, sidePadding, topPadding);

							using (var memStream = new MemoryStream()) {
								bmpThumb.Save(memStream, ImageFormat.Png);
							}

							textBrush.Dispose();
							font.Dispose();
						}
					}
				}
			}

			if (bmpThumb == null) {
				context.Response.StatusCode = 404;
				context.Response.StatusDescription = "Not Found";
				context.ApplicationInstance.CompleteRequest();
				return;
			}

			context.Response.Expires = 5;
			context.Response.ContentType = "image/x-png";

			using (var memStream = new MemoryStream()) {
				bmpThumb.Save(memStream, ImageFormat.Png);
				memStream.WriteTo(context.Response.OutputStream);
			}

			bmpThumb.Dispose();
			bmpIn.Dispose();

			context.ApplicationInstance.CompleteRequest();
			context.Response.End();
		}

		private void DoHelper(HttpContext context) {
			context.Response.Cache.VaryByParams["ts"] = true;

			DoCacheMagic(context, 10);

			string sBody = WebControlHelper.GetManifestResourceStream("Carrotware.Web.UI.Controls.carrotHelp.js");
			DateTime timeAM = DateTime.Now.Date.AddHours(7);  // AM
			DateTime timePM = DateTime.Now.Date.AddHours(17);  // PM

			sBody = sBody.Replace("[[TIMESTAMP]]", DateTime.UtcNow.ToString("u"));

			sBody = sBody.Replace("[[SHORTDATEPATTERN]]", WebControlHelper.ShortDatePattern);
			sBody = sBody.Replace("[[SHORTTIMEPATTERN]]", WebControlHelper.ShortTimePattern);
			sBody = sBody.Replace("[[SHORTDATEFORMATPATTERN]]", WebControlHelper.ShortDateFormatPattern);
			sBody = sBody.Replace("[[SHORTDATETIMEFORMATPATTERN]]", WebControlHelper.ShortDateTimeFormatPattern);

			sBody = sBody.Replace("[[AM_TIMEPATTERN]]", timeAM.ToString("tt"));
			sBody = sBody.Replace("[[PM_TIMEPATTERN]]", timePM.ToString("tt"));

			context.Response.ContentType = "text/javascript";

			var byteArray = Encoding.UTF8.GetBytes(sBody);

			using (var memStream = new MemoryStream(byteArray)) {
				memStream.WriteTo(context.Response.OutputStream);
			}

			context.ApplicationInstance.CompleteRequest();
			context.Response.End();
		}

		protected void DoCacheMagic(HttpContext context, int interval) {
			DateTime now = DateTime.Now;

			DateTime dtModified = GetFauxModDate(10);
			DateTime? dtM = GetModDate(context);

			string strModifed = dtModified.ToUniversalTime().ToString("r");
			context.Response.AppendHeader("Last-Modified", strModifed);
			context.Response.AppendHeader("Date", strModifed);
			context.Response.Cache.SetLastModified(dtModified);

			DateTime dtExpire = now.ToUniversalTime().AddMinutes(interval);
			context.Response.Cache.SetExpires(dtExpire);
			context.Response.Cache.SetValidUntilExpires(true);
			context.Response.Cache.SetCacheability(HttpCacheability.Private);

			if (dtM == null || dtM.Value != dtModified) {
				context.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
				context.Response.StatusDescription = "OK";
			} else {
				context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
				context.Response.SuppressContent = true;
			}
		}

		protected DateTime GetFauxModDate(int interval) {
			DateTime now = DateTime.Now;

			DateTime dtMod = now.AddMinutes(-90);
			TimeSpan ts = TimeSpan.FromMinutes(interval);
			DateTime dtModified = new DateTime(((dtMod.Ticks + ts.Ticks - 1) / ts.Ticks) * ts.Ticks);

			return dtModified;
		}

		protected DateTime? GetModDate(HttpContext context) {
			DateTime? dtModSince = null;
			string modSince = context.Request.Headers.Get("If-Modified-Since");

			if (!String.IsNullOrEmpty(modSince)) {
				dtModSince = DateTime.Parse(modSince);
				dtModSince = dtModSince.Value.ToUniversalTime();
			}

			return dtModSince;
		}
	}
}