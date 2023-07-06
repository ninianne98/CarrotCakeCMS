using Carrotware.Web.UI.Controls;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace Carrotware.CMS.Core {

	public static class CoreHelper {

		internal static string ReadEmbededScript(string sResouceName) {
			return WebControlHelper.GetManifestResourceText(typeof(CoreHelper), sResouceName);
		}

		internal static byte[] ReadEmbededBinary(string sResouceName) {
			return WebControlHelper.GetManifestResourceBytes(typeof(CoreHelper), sResouceName);
		}

		internal static string GetWebResourceUrl(string resource) {
			string sPath = string.Empty;

			try {
				sPath = WebControlHelper.GetWebResourceUrl(typeof(CoreHelper), resource);
			} catch { }

			return sPath;
		}

		public static Guid GetGuidParmFromQuery(this HttpContext context, string key) {
			Guid id = Guid.Empty;
			if (SiteData.IsWebView) {
				if (context.Request.QueryString[key] != null
					&& !string.IsNullOrEmpty(context.Request.QueryString[key].ToString())) {
					id = new Guid(context.Request.QueryString[key].ToString());
				}
			}
			return id;
		}

		public static T Clone<T>(this T source) {
			if (object.ReferenceEquals(source, null)) {
				return default(T);
			}

			var bf = new BinaryFormatter();
			using (var ms = new MemoryStream()) {
				bf.Serialize(ms, source);
				ms.Seek(0, SeekOrigin.Begin);
				return (T)bf.Deserialize(ms);
			}
		}
	}
}