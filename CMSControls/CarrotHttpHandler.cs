using System;
using System.Web.SessionState;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Carrotware.CMS.UI.Controls {
	class CarrotHttpHandler : IHttpHandler, IRequiresSessionState {


		public bool IsReusable {
			get { return true; }
		}


		public void ProcessRequest(HttpContext context) {

			if (context.Request.Path.ToLower() == "/carrotwarecaptcha.axd") {

				var f = ColorTranslator.FromHtml(CaptchaImage.FGColorDef);
				var b = ColorTranslator.FromHtml(CaptchaImage.BGColorDef);
				var n = ColorTranslator.FromHtml(CaptchaImage.NColorDef);

				var captchaImg = CaptchaImage.GetCaptchaImage(f, b, n);

				if (captchaImg == null) {
					context.Response.StatusCode = 404;
					context.Response.StatusDescription = "Not Found";
					context.ApplicationInstance.CompleteRequest();
					return;
				}

				context.Response.ContentType = "image/x-png";

				var memStream = new System.IO.MemoryStream();
				captchaImg.Save(memStream, ImageFormat.Png);
				memStream.WriteTo(context.Response.OutputStream);

				context.Response.StatusCode = 200;
				context.Response.StatusDescription = "OK";
				context.ApplicationInstance.CompleteRequest();

				context.Response.End();
				memStream.Dispose();
				captchaImg.Dispose();

			}

		}



	}
}