using System;
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

	public abstract class BaseWebControl : WebControl {

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsPostBack {
			get {
				string sReq = "GET";
				try { sReq = HttpContext.Current.Request.ServerVariables["REQUEST_METHOD"].ToString().ToUpperInvariant(); } catch { }
				return sReq != "GET" ? true : false;
			}
		}

		[Browsable(true)]
		public bool IsWebView {
			get { return (HttpContext.Current != null); }
		}

		protected string CurrentScriptName {
			get { return HttpContext.Current.Request.ServerVariables["script_name"].ToString(); }
		}

		protected override void Render(HtmlTextWriter writer) {
			RenderContents(writer);
		}

		protected override void RenderContents(HtmlTextWriter output) {
		}

		protected void BaseRender(HtmlTextWriter writer) {
			base.Render(writer);
		}

		protected void BaseRenderContents(HtmlTextWriter output) {
			base.RenderContents(output);
		}
	}
}