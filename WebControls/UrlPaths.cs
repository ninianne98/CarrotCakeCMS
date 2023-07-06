using System.Web;

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

	public static class UrlPaths {

		public static string CaptchaPath {
			get {
				return "/carrotwarecaptcha.axd";
			}
		}

		public static string ThumbnailPath {
			get {
				return "/carrotwarethumb.axd";
			}
		}

		public static string HelperPath {
			get {
				return "/carrotwarehelper.axd";
			}
		}

		//==========================

		public static string CreateCssTag(string comment, string uri) {
			return "<!-- " + HttpUtility.HtmlEncode(comment) + " --> " + CreateCssTag(uri);
		}

		public static string CreateCssTag(string uri) {
			return "<link href=\"" + uri + "\" type=\"text/css\" rel=\"stylesheet\" />";
		}

		public static string CreateJavascriptTag(string comment, string uri) {
			return "<!-- " + HttpUtility.HtmlEncode(comment) + " --> " + CreateJavascriptTag(uri);
		}

		public static string CreateJavascriptTag(string uri) {
			return "<script src=\"" + uri + "\" type=\"text/javascript\"></script>";
		}
	}
}