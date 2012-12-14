using System;
using System.Web.SessionState;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;



namespace Carrotware.Web.UI.Controls {
	class CarrotHttpHandler : IHttpHandler, IRequiresSessionState {


		public bool IsReusable {
			get { return true; }
		}

		public void ProcessRequest(HttpContext context) {

			if (context.Request.Path.ToLower() == "/carrotwarecaptcha.axd") {
				DoCaptcha(context);
			}

			if (context.Request.Path.ToLower() == "/carrotwarethumb.axd") {
				DoThumb(context);
			}
		}



		private Bitmap ResizeBitmap(Bitmap bmpIn, int w, int h) {
			Bitmap bmpNew = new Bitmap(w, h);
			using (Graphics g = Graphics.FromImage(bmpNew)) {
				g.DrawImage(bmpIn, 0, 0, w, h);
			}
			return bmpNew;
		}


		private void DoCaptcha(HttpContext context) {
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

			using (MemoryStream memStream = new MemoryStream()) {
				captchaImg.Save(memStream, ImageFormat.Png);
				memStream.WriteTo(context.Response.OutputStream);
			}
			context.Response.StatusCode = 200;
			context.Response.StatusDescription = "OK";
			context.ApplicationInstance.CompleteRequest();

			captchaImg.Dispose();
			context.Response.End();
		}

		private void DoThumb(HttpContext context) {
			int iThumb = 150;
			string sImageIn = context.Request.QueryString["thumb"];
			string sImg = sImageIn;
			string sScale = context.Request.QueryString["scale"];

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

							using (MemoryStream memStream = new MemoryStream()) {
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

			using (MemoryStream memStream = new MemoryStream()) {
				bmpThumb.Save(memStream, ImageFormat.Png);
				memStream.WriteTo(context.Response.OutputStream);
			}

			context.Response.StatusCode = 200;
			context.Response.StatusDescription = "OK";
			context.ApplicationInstance.CompleteRequest();

			bmpThumb.Dispose();
			bmpIn.Dispose();

			context.Response.End();
		}




	}

}