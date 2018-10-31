using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;

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
				if (_CachedPage == null) {
					_CachedPage = new Page();
					_CachedPage.AppRelativeVirtualPath = "~/";
				}
				return _CachedPage;
			}
		}

		private static Page _CachedPage;

		public static string DateKey() {
			return DateKey(15);
		}

		public static string DateKey(int interval) {
			DateTime now = DateTime.UtcNow;
			TimeSpan d = TimeSpan.FromMinutes(interval);
			DateTime dt = new DateTime(((now.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
			byte[] dateStringBytes = ASCIIEncoding.ASCII.GetBytes(dt.ToString("U"));

			return Convert.ToBase64String(dateStringBytes);
		}

		public static string GetWebResourceUrl(Type type, string resource) {
			return GetWebResourceUrl(CachedPage, type, resource);
		}

		public static string GetWebResourceUrl(Page page, Type type, string resource) {
			string sPath = String.Empty;

			if (!resource.StartsWith("Carrotware.Web.UI")) {
				resource = String.Format("Carrotware.Web.UI.Controls.(0}", resource);
			}

			if (page != null) {
				try {
					sPath = page.ClientScript.GetWebResourceUrl(type, resource);
					sPath = HttpUtility.HtmlEncode(sPath);
				} catch { }
			} else {
				sPath = GetWebResourceUrl(type, resource);
			}

			return sPath;
		}

		public static string GetManifestResourceStream(string resource) {
			string returnText = null;

			if (!resource.StartsWith("Carrotware.Web.UI")) {
				resource = String.Format("Carrotware.Web.UI.Controls.(0}", resource);
			}

			Assembly _assembly = Assembly.GetExecutingAssembly();
			using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream(resource))) {
				returnText = oTextStream.ReadToEnd();
			}

			return returnText;
		}

		public static string ShortDateFormatPattern {
			get {
				return "{0:" + ShortDatePattern + "}";
			}
		}

		public static string ShortDateTimeFormatPattern {
			get {
				return "{0:" + ShortDatePattern + "} {0:" + ShortTimePattern + "} ";
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