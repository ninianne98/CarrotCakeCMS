﻿using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Carrotware.CMS.Interface {

	public static class ParmParser {

		public static string GetParmValue(Dictionary<string, string> parmDictionary, string sKey) {
			string ret = null;

			if (parmDictionary.Any()) {
				ret = (from c in parmDictionary
					   where c.Key.ToLowerInvariant() == sKey.ToLowerInvariant()
					   select c.Value).FirstOrDefault();
			}

			return ret;
		}

		public static string GetParmValue(Dictionary<string, string> parmDictionary, string sKey, string sDefault) {
			string ret = null;

			if (parmDictionary.Any()) {
				ret = (from c in parmDictionary
					   where c.Key.ToLowerInvariant() == sKey.ToLowerInvariant()
					   select c.Value).FirstOrDefault();
			}

			ret = ret == null ? sDefault : ret;

			return ret;
		}

		public static string GetParmValueDefaultEmpty(Dictionary<string, string> parmDictionary, string sKey, string sDefault) {
			string ret = GetParmValue(parmDictionary, sKey, sDefault);

			ret = string.IsNullOrEmpty(ret) ? sDefault : ret;

			return ret;
		}

		public static List<string> GetParmValueList(Dictionary<string, string> parmDictionary, string sKey) {
			sKey = sKey.EndsWith("|") ? sKey : sKey + "|";
			sKey = sKey.ToLowerInvariant();

			List<string> ret = new List<string>();

			if (parmDictionary.Any()) {
				ret = (from c in parmDictionary
					   where c.Key.ToLowerInvariant().StartsWith(sKey)
					   select c.Value).ToList();
			}

			return ret;
		}

		#region QueryString Parsers

		public static Guid GetGuidPageIDFromQuery() {
			return GetGuidParameterFromQuery("pageid");
		}

		public static Guid GetGuidIDFromQuery() {
			return GetGuidParameterFromQuery("id");
		}

		public static Guid GetGuidParameterFromQuery(string ParmName) {
			Guid id = Guid.Empty;
			if (HttpContext.Current != null) {
				if (HttpContext.Current.Request.QueryString[ParmName] != null
					&& !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[ParmName].ToString())) {
					id = new Guid(HttpContext.Current.Request.QueryString[ParmName].ToString());
				}
			}
			return id;
		}

		public static string GetStringParameterFromQuery(string ParmName) {
			string id = string.Empty;
			if (HttpContext.Current != null) {
				if (HttpContext.Current.Request.QueryString[ParmName] != null
					&& !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[ParmName].ToString())) {
					id = HttpContext.Current.Request.QueryString[ParmName].ToString();
				}
			}
			return id;
		}

		#endregion QueryString Parsers
	}
}