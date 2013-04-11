using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using Carrotware.CMS.Core;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Controls {

	[Designer(typeof(GeneralControlDesigner))]
	[ToolboxData("<{0}:TrackBack runat=server></{0}:TrackBack>")]
	public class TrackBack : BaseServerControl {

		[Category("Appearance")]
		[DefaultValue("/trackback.ashx")]
		public string TrackBackURI {
			get {
				string s = (string)ViewState["TrackBackURI"];
				return ((s == null) ? "/trackback.ashx" : s);
			}
			set {
				ViewState["TrackBackURI"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool EnableDirectTrackback {
			get {
				bool s = false;
				if (ViewState["EnableDirectTrackback"] != null) {
					try { s = (bool)ViewState["EnableDirectTrackback"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["EnableDirectTrackback"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public override bool EnableViewState {
			get {
				String s = (String)ViewState["EnableViewState"];
				bool b = ((s == null) ? false : Convert.ToBoolean(s));
				base.EnableViewState = b;
				return b;
			}

			set {
				ViewState["EnableViewState"] = value.ToString();
				base.EnableViewState = value;
			}
		}

		private ControlUtilities cu = new ControlUtilities();

		protected override void RenderContents(HtmlTextWriter output) {

			string sTrackback = ControlUtilities.GetManifestResourceStream("Carrotware.CMS.UI.Controls.Trackback.txt");

			ContentPage cp = cu.GetContainerContentPage(this);

			if (cp != null) {
				sTrackback = sTrackback.Replace("{URL}", SiteData.CurrentSite.ConstructedCanonicalURL(cp));
				sTrackback = sTrackback.Replace("{TB_TITLE}", cp.NavMenuText);
				sTrackback = sTrackback.Replace("{TB_URL_ID}", SiteData.CurrentSite.ConstructedCanonicalURL(TrackBackURI) + "?id=" + HttpUtility.UrlEncode(cp.FileName));
				output.Write(sTrackback);
			}

			if (IsPostBack && EnableDirectTrackback) {
				TrackbackHelper tbh = new TrackbackHelper();
				tbh.ProcessTrackback(HttpContext.Current, false);
			}
		}

	}

}
