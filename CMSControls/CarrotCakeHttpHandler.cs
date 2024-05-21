using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
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

namespace Carrotware.CMS.UI.Controls {

	public class CarrotCakeHttpHandler : IHttpHandler, IRequiresSessionState {

		public bool IsReusable {
			get { return true; }
		}

		public void ProcessRequest(HttpContext context) {
			if (context.Request.Path.ToLowerInvariant() == SiteFilename.AdminScriptValues) {
				GetAdminScript(context);
			}

			if (context.Request.Path.ToLowerInvariant() == TwoLevelNavigation.NavigationStylePath) {
				GetNavigationCss(context);
			}
		}

		private void SetExpires(HttpContext context, double interval) {
			DateTime now = DateTime.Now;
			DateTime dtExpire = now.ToUniversalTime().AddMinutes(interval);

			context.Response.Cache.SetExpires(dtExpire);
			context.Response.Cache.SetValidUntilExpires(true);
			context.Response.Cache.SetCacheability(HttpCacheability.Private);
		}

		public void GetAdminScript(HttpContext context) {
			context.VaryCacheByQuery(new string[] { "ts", "cms", "a" });

			var expireIn = 1;
			if (SecurityData.IsAuthenticated) {
				expireIn = 3;
			}
			SetExpires(context, expireIn);

			var adminFolder = SiteData.AdminFolderPath;

			var sb = new StringBuilder();
			sb.Append(ControlUtilities.GetManifestResourceStream("Carrotware.CMS.UI.Controls.adminHelp.js").ToString());

			sb.Replace("[[TIMESTAMP]]", DateTime.UtcNow.ToString("u"));

			if (SecurityData.UserPrincipal.Identity.IsAuthenticated) {
				if (SecurityData.IsAdmin || SecurityData.IsEditor) {
					sb.Replace("[[ADMIN_PATH]]", adminFolder.FixPathSlashes());
					sb.Replace("[[API_PATH]]", (adminFolder + "/CMS.asmx").FixPathSlashes());
					sb.Replace("[[TEMPLATE_PATH]]", SiteData.PreviewTemplateFilePage);
					sb.Replace("[[TEMPLATE_QS]]", SiteData.TemplatePreviewParameter);
				}
			}

			context.Response.ContentType = "text/javascript";
			context.Response.Write(sb.ToString());
			context.Response.StatusCode = 200;
			context.Response.StatusDescription = "OK";
		}

		public void GetNavigationCss(HttpContext context) {
			context.VaryCacheByQuery(new string[] { "el", "ts", "tc", "f", "bg", "fc", "bc" });

			SetExpires(context, 3);

			string el = context.SafeQueryString("el");
			string sel = context.SafeQueryString("sel");
			string tbg = context.SafeQueryString("tbg");
			string f = context.SafeQueryString("f");
			string bg = context.SafeQueryString("bg");
			string ubg = context.SafeQueryString("ubg");

			string fc = context.SafeQueryString("fc");
			string bc = context.SafeQueryString("bc");
			string hbc = context.SafeQueryString("hbc");
			string hfc = context.SafeQueryString("hfc");
			string uf = context.SafeQueryString("uf");

			string sbg = context.SafeQueryString("sbg");
			string sfg = context.SafeQueryString("sfg");
			string bc2 = context.SafeQueryString("bc2");
			string fc2 = context.SafeQueryString("fc2");
			string tc = context.SafeQueryString("tc");

			var nav = new TwoLevelNavigation();
			nav.RestoreNavColors(f, tc, bg, ubg, fc, bc, hbc, hfc, uf, sbg, sfg, bc2, fc2);

			nav.ID = el.DecodeBase64().ScrubQueryElement();
			nav.CSSSelected = sel.DecodeBase64().ScrubQueryElement();
			nav.TopBackgroundStyle = tbg.DecodeBase64().ScrubQueryElement();

			var txt = nav.GenerateCSS();

			context.Response.ContentType = "text/css";
			context.Response.Write(txt);
			context.Response.StatusCode = 200;
			context.Response.StatusDescription = "OK";
		}
	}
}