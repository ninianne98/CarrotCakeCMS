using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Carrotware.CMS.UI.Plugins {
	public partial class CAPTCHA : System.Web.UI.Page {
		protected override void OnInit(EventArgs e) {

			var imageHeight = 50;
			var topPadding = 10; // top and bottom padding in pixels
			var sidePadding = 10; // side padding in pixels
			var textBrush = new SolidBrush(ColorTranslator.FromHtml("#97AC88"));
			var font = new Font("Verdana", 18);

			var text = Guid.NewGuid().ToString().Substring(0, 6);

			var bitmap = new Bitmap(500, 500);
			var graphics = Graphics.FromImage(bitmap);
			var textSize = graphics.MeasureString(text, font);
			bitmap.Dispose();
			graphics.Dispose();

			var bitmapWidth = sidePadding * 2 + (int)textSize.Width;
			bitmap = new Bitmap(bitmapWidth, imageHeight);
			graphics = Graphics.FromImage(bitmap);

			graphics.DrawString(text, font, textBrush, sidePadding, topPadding);

			this.Response.ContentType = "image/x-png";


			var memStream = new System.IO.MemoryStream();
			bitmap.Save(memStream, ImageFormat.Png);
			memStream.WriteTo(this.Response.OutputStream);

			// Some cleanup, not sure if it is all needed
			this.Response.End();
			memStream.Dispose();
			graphics.Dispose();
			bitmap.Dispose();

		}
	}
}
