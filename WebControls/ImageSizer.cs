﻿using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

	[ToolboxData("<{0}:ImageSizer runat=server></{0}:ImageSizer>")]
	public class ImageSizer : Image {
		private string handlerURL = UrlPaths.ThumbnailPath;

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

		public override Unit BorderWidth {
			get {
				if (base.BorderWidth.IsEmpty) {
					return Unit.Pixel(0);
				} else {
					return base.BorderWidth;
				}
			}

			set {
				base.BorderWidth = value;
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