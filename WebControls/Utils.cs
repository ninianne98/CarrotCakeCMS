using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

	public static class Utils {

		public static string GetAssemblyName(this Assembly assembly) {
			var assemblyName = assembly.ManifestModule.Name;
			return Path.GetFileNameWithoutExtension(assemblyName);
		}

		public static string ScrubQueryElement(this string text) {
			return text.Replace("{", "").Replace(">", "").Replace("<", "").Replace(">", "")
										.Replace("'", "").Replace("\\", "").Replace("//", "").Replace(":", "");
		}

		public static string SafeQueryString(this HttpContext context, string key) {
			return SafeQueryString(context, key, string.Empty);
		}

		public static string SafeQueryString(this HttpContext context, string key, string defaultVal) {
			if (context.Request.QueryString[key] != null) {
				return context.Request.QueryString[key].ToString();
			}
			return defaultVal;
		}

		public static void VaryCacheByQuery(this HttpContext context, string[] keys) {
			if (keys != null) {
				foreach (var k in keys) {
					context.Response.Cache.VaryByParams[k] = true;
				}
			}
		}

		public static string NormalizeFilename(this string path) {
			if (path != null) {
				var p = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
				path = string.Join(Path.AltDirectorySeparatorChar.ToString(), p.Split(Path.AltDirectorySeparatorChar).Where(x => x.Length > 0).ToArray());
				return path;
			}
			return string.Empty;
		}

		public static string TrimPathSlashes(this string path) {
			path = path.NormalizeFilename();
			if (path.StartsWith("/")) {
				path = path.Substring(1);
			}
			if (path.EndsWith("/")) {
				path = path.Substring(0, path.Length - 1);
			}
			return path;
		}

		public static string FixPathSlashes(this string path) {
			path = path.NormalizeFilename();
			if (path != @"/" && path != "") {
				path = string.Format("/{0}", path);
			} else {
				path = @"/";
			}
			return path;
		}

		public static string FixFolderSlashes(this string path) {
			path = path.NormalizeFilename();
			if (path != @"/" && path != "") {
				path = string.Format("/{0}/", path);
			} else {
				path = @"/";
			}
			return path;
		}

		public static string EncodeColor(this Color color) {
			var colorCode = ColorTranslator.ToHtml(color);

			string sColor = string.Empty;
			if (!string.IsNullOrEmpty(colorCode)) {
				sColor = colorCode.ToLowerInvariant();
				sColor = sColor.Replace("#", string.Empty);
				sColor = sColor.Replace("HEX-", string.Empty);
				sColor = HttpUtility.HtmlEncode(sColor);
			}
			return sColor;
		}

		public static Color DecodeColor(this string colorCode) {
			string sColor = DecodeColorString(colorCode);

			if (sColor.ToLowerInvariant().Contains("transparent")) {
				return Color.Transparent;
			}
			if (sColor == "#" || string.IsNullOrEmpty(sColor)
					|| sColor.ToLowerInvariant().Contains("empty")) {
				return Color.Empty;
			}

			try {
				return ColorTranslator.FromHtml(sColor);
			} catch { }

			var colorname = sColor.Replace("#", "");
			var color = Color.FromName(colorname);
			return color;
		}

		public static string DecodeColorString(this string colorCode) {
			string sColor = string.Empty;
			if (!string.IsNullOrEmpty(colorCode)) {
				sColor = colorCode;
				sColor = HttpUtility.HtmlDecode(sColor);
				sColor = sColor.Replace("HEX-", string.Empty);
				if (!sColor.StartsWith("#")) {
					sColor = string.Format("#{0}", sColor);
				}
			}

			return sColor;
		}

		public static string DecodeBase64(this string text) {
			string val = string.Empty;
			if (!string.IsNullOrEmpty(text)) {
				Encoding enc = Encoding.GetEncoding("ISO-8859-1"); //Western European (ISO)
				val = enc.GetString(Convert.FromBase64String(text));
			}
			return val;
		}

		public static string EncodeBase64(this string text) {
			string val = string.Empty;
			if (!string.IsNullOrEmpty(text)) {
				Encoding enc = Encoding.GetEncoding("ISO-8859-1"); //Western European (ISO)
				byte[] toEncodeAsBytes = enc.GetBytes(text);
				val = Convert.ToBase64String(toEncodeAsBytes);
			}
			return val;
		}
	}
}