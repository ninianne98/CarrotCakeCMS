using Carrotware.Web.UI.Controls;

using System;

using System.Collections.Generic;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public static class Helper {

		public static string ShortDateFormatPattern {
			get {
				return WebControlHelper.ShortDateFormatPattern;
			}
		}

		public static string ShortDateTimeFormatPattern {
			get {
				return WebControlHelper.ShortDateTimeFormatPattern;
			}
		}

		public static string ShortDatePattern {
			get {
				return WebControlHelper.ShortDatePattern;
			}
		}

		public static string ShortTimePattern {
			get {
				return WebControlHelper.ShortTimePattern;
			}
		}
	}
}