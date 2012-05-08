using System;
using System.Web.SessionState;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;



namespace Carrotware.Web.UI.Controls {
	class CarrotHttpHandler : IHttpHandler, IRequiresSessionState {


		public bool IsReusable {
			get { return true; }
		}

		private Bitmap ResizeBitmap(Bitmap bmpIn, int w, int h) {
			Bitmap bmpNew = new Bitmap(w, h);
			using (Graphics g = Graphics.FromImage(bmpNew)) {
				g.DrawImage(bmpIn, 0, 0, w, h);
			}
			return bmpNew;
		}



		public void ProcessRequest(HttpContext context) {

			if (context.Request.Path.ToLower() == "/carrotwarecaptcha.axd") {

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

			if (context.Request.Path.ToLower() == "/carrotwarethumb.axd") {

				int iThumb = 150;

				string sImg = context.Request.QueryString["thumb"];

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

				if (!string.IsNullOrEmpty(sImg)) {
					sImg = context.Server.MapPath(sImg);
					if (File.Exists(sImg)) {
						bmpIn = new Bitmap(sImg);
						bmpThumb = ResizeBitmap(bmpIn, iThumb, iThumb);
					}
				}

				if (bmpThumb == null) {
					context.Response.StatusCode = 404;
					context.Response.StatusDescription = "Not Found";
					context.ApplicationInstance.CompleteRequest();
					return;
				}

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



}