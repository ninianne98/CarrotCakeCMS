using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;

namespace Carrotware.Web.UI.Controls {

	[ToolboxData("<{0}:ImageSizer runat=server></{0}:ImageSizer>")]
	public class ImageSizer : Image {

		private string handlerURL = "/carrotwarethumb.axd";

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int ThumbSize {
			get {
				int s = 150;
				try { s = (int)ViewState["ThumbSize"]; } catch { }
				return s;
			}
			set {
				ViewState["ThumbSize"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool ScaleImage {
			get {
				bool s = true;
				if (ViewState["ScaleImage"] != null) {
					try { s = (bool)ViewState["ScaleImage"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["ScaleImage"] = value;
			}
		}


		protected override void OnPreRender(EventArgs e) {

			if (!this.ImageUrl.StartsWith(handlerURL)) {
				string sImg = this.ImageUrl;

				this.ImageUrl = string.Format("{0}?thumb={1}&square={2}&scale={3}", handlerURL, HttpUtility.UrlEncode(sImg), HttpUtility.UrlEncode(this.ThumbSize.ToString()), HttpUtility.UrlEncode(this.ScaleImage.ToString()));

			}

			base.OnPreRender(e);
		}


	}


}
