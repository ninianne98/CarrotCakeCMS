using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
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

		public static string GetWebResourceUrl(Type type, string resource) {
			return GetWebResourceUrl(CachedPage, type, resource);
		}

		public static string GetWebResourceUrl(Control control, string resource) {
			return GetWebResourceUrl(control.Page, control.GetType(), resource);
		}

		public static string GetWebResourceUrl(Page page, Type type, string resource) {
			string uri = string.Empty;

			var assembly = Assembly.GetExecutingAssembly();
			var a_name = assembly.GetName().Name;

			if (resource.ToLowerInvariant().StartsWith(a_name.ToLowerInvariant()) == false) {
				resource = string.Format("{0}.{1}", a_name, resource);
			}

			if (page != null) {
				try {
					uri = page.ClientScript.GetWebResourceUrl(type, resource);
				} catch (Exception ex) { }
			} else {
				uri = GetWebResourceUrl(type, resource);
			}

			return uri;
		}

		internal static string GetManifestResourceStream(string resource) {
			return GetManifestResourceText(resource);
		}

		internal static byte[] GetManifestResourceBinary(string resource) {
			return GetManifestResourceBytes(typeof(WebControlHelper), resource);
		}

		internal static string GetManifestResourceText(string resource) {
			return GetManifestResourceText(typeof(WebControlHelper), resource);
		}

		internal static byte[] GetManifestResourceBytes(string resource) {
			return GetManifestResourceBytes(typeof(WebControlHelper), resource);
		}

		public static string GetManifestResourceText(Type type, string resource) {
			string returnText = null;

			Assembly _assembly = Assembly.GetAssembly(type);
			using (var stream = new StreamReader(_assembly.GetManifestResourceStream(resource))) {
				returnText = stream.ReadToEnd();
			}

			return returnText;
		}

		public static byte[] GetManifestResourceBytes(Type type, string resource) {
			byte[] returnBytes = null;
			Assembly _assembly = Assembly.GetAssembly(type);

			using (var stream = _assembly.GetManifestResourceStream(resource)) {
				returnBytes = new byte[stream.Length];
				stream.Read(returnBytes, 0, returnBytes.Length);
			}

			return returnBytes;
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

		public static string RenderControl(this Control ctrl) {
			var sb = new StringBuilder();

			using (var tw = new StringWriter(sb)) {
				using (var hw = new HtmlTextWriter(tw)) {

					ctrl.RenderControl(hw);

					return sb.ToString();
				}
			}
		}

		public static HtmlMeta CreateHtmlMeta(string attrib, string attribValue, string content) {
			var meta = new HtmlMeta();

			meta.Attributes[attrib] = attribValue;
			meta.Content = content;

			return meta;
		}

		public static HtmlMeta CreateHtmlMetaProp(string property, string content) {
			return CreateHtmlMeta("property", property, content);
		}

		public static ControlCollection AddLiteral(this ControlCollection collection, string content) {
			collection.Add(new LiteralControl() { Text = content });
			return collection;
		}

		public static ControlCollection AddHtmlMetaProp(this ControlCollection collection, string property, string content) {
			collection.Add(CreateHtmlMetaProp(property, content));
			return collection;
		}
	}
}