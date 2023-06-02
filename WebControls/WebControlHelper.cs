using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Xml.Linq;

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

	public static class WebControlHelper {

		private static Page CachedPage {
			get {
				if (_cachedPage == null) {
					_cachedPage = new Page();
					try {
						_cachedPage.AppRelativeVirtualPath = "~/";
					} catch (Exception ex) { }
				}
				return _cachedPage;
			}
		}

		private static Page _cachedPage;

		public static string HtmlFormat(StringBuilder input) {
			if (input != null) {
				return HtmlFormat(input.ToString());
			}

			return string.Empty;
		}

		public static string HtmlFormat(string input) {
			if (!string.IsNullOrEmpty(input)) {
				bool autoAddTypes = false;
				var subs = new Dictionary<string, int>();
				subs.Add("ndash", 150);
				subs.Add("mdash", 151);
				subs.Add("nbsp", 153);
				subs.Add("trade", 153);
				subs.Add("copy", 169);
				subs.Add("reg", 174);
				subs.Add("laquo", 171);
				subs.Add("raquo", 187);
				subs.Add("lsquo", 145);
				subs.Add("rsquo", 146);
				subs.Add("ldquo", 147);
				subs.Add("rdquo", 148);
				subs.Add("bull", 149);
				subs.Add("amp", 38);
				subs.Add("quot", 34);

				var subs2 = new Dictionary<string, int>();
				subs2.Add("ndash", 150);
				subs2.Add("mdash", 151);
				subs2.Add("nbsp", 153);
				subs2.Add("trade", 153);
				subs2.Add("copy", 169);
				subs2.Add("reg", 174);
				subs2.Add("laquo", 171);
				subs2.Add("raquo", 187);
				subs2.Add("bull", 149);

				string docType = string.Empty;

				if (!input.ToLowerInvariant().StartsWith("<!doctype")) {
					autoAddTypes = true;

					docType = "<!DOCTYPE html [ ";
					foreach (var s in subs) {
						docType += string.Format(" <!ENTITY {0} \"&#{1};\"> ", s.Key, s.Value);
					}
					docType += " ]>".Replace("  ", " ");

					input = docType + Environment.NewLine + input;
				}

				var doc = XDocument.Parse(input);

				if (autoAddTypes) {
					var sb = new StringBuilder();
					sb.Append(doc.ToString().Replace(docType, string.Empty));

					foreach (var s in subs2) {
						sb.Replace(Convert.ToChar(s.Value).ToString(), string.Format("&{0};", s.Key));
					}

					return sb.ToString();
				}

				return doc.ToString();
			}

			return string.Empty;
		}

		public static string DateKey() {
			return DateKey(15);
		}

		public static string DateKey(int interval) {
			DateTime now = DateTime.UtcNow;
			TimeSpan d = TimeSpan.FromMinutes(interval);
			DateTime dt = new DateTime(((now.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
			byte[] dateStringBytes = Encoding.ASCII.GetBytes(dt.ToString("U"));

			return Convert.ToBase64String(dateStringBytes);
		}

		internal static string GetWebResourceUrl(string resource) {
			return GetWebResourceUrl(CachedPage, typeof(WebControlHelper), resource);
		}

		internal static string GetWebResourceUrl(Type type, string resource) {
			return GetWebResourceUrl(CachedPage, type, resource);
		}

		internal static string GetWebResourceUrl(Control control, string resource) {
			return GetWebResourceUrl(control.Page, control.GetType(), resource);
		}

		internal static string GetWebResourceUrl(Page page, Type type, string resource) {
			string sPath = string.Empty;

			if (page != null) {
				try {
					sPath = page.ClientScript.GetWebResourceUrl(type, resource);
				} catch (Exception ex) { }
			} else {
				sPath = GetWebResourceUrl(type, resource);
			}

			return sPath;
		}

		internal static string GetManifestResourceStream(string resource) {
			string returnText = null;

			Assembly _assembly = Assembly.GetExecutingAssembly();
			using (var stream = new StreamReader(_assembly.GetManifestResourceStream(resource))) {
				returnText = stream.ReadToEnd();
			}

			return returnText;
		}

		internal static byte[] GetManifestResourceBinary(string resource) {
			byte[] returnBytes = null;
			Assembly _assembly = Assembly.GetExecutingAssembly();

			using (var stream = _assembly.GetManifestResourceStream(resource)) {
				returnBytes = new byte[stream.Length];
				stream.Read(returnBytes, 0, returnBytes.Length);
			}

			return returnBytes;
		}

		internal static string RenderCtrl(Control ctrl) {
			var sw = new StringWriter();
			using (var tw = new HtmlTextWriter(sw)) {
				ctrl.RenderControl(tw);
				return sw.ToString();
			}
		}

		public static string ShortDateFormatPattern {
			get {
				return "{0:" + ShortDatePattern + "}";
			}
		}

		public static string ShortDateTimeFormatPattern {
			get {
				return "{0:" + ShortDatePattern + "} {0:" + ShortTimePattern + "}";
			}
		}

		private static string _shortDatePattern = null;

		public static string ShortDatePattern {
			get {
				if (_shortDatePattern == null) {
					DateTimeFormatInfo _dtf = CultureInfo.CurrentCulture.DateTimeFormat;
					if (_dtf == null) {
						_dtf = CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat;
					}

					_shortDatePattern = _dtf.ShortDatePattern ?? "M/d/yyyy";
					_shortDatePattern = _shortDatePattern.Replace("MM", "M").Replace("dd", "d");
				}

				return _shortDatePattern;
			}
		}

		private static string _shortTimePattern = null;

		public static string ShortTimePattern {
			get {
				if (_shortTimePattern == null) {
					DateTimeFormatInfo _dtf = CultureInfo.CurrentCulture.DateTimeFormat;
					if (_dtf == null) {
						_dtf = CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat;
					}
					_shortTimePattern = _dtf.ShortTimePattern ?? "hh:mm tt";
				}

				return _shortTimePattern;
			}
		}
	}
}