using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using Carrotware.Web.UI.Controls;
using System;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Admin {

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

		public static string AntiCache {
			get {
				return string.Format("?cms={0}", SiteData.CurrentDLLVersion);
			}
		}

		public static CmsSkin.SkinOption _theme = CmsSkin.SkinOption.None;

		public static CmsSkin.SkinOption SiteSkin {
			get {
				if (_theme == CmsSkin.SkinOption.None) {
					var config = CarrotCakeConfig.GetConfig();
					string skin = config.MainConfig.SiteSkin;
					var actualSkin = CmsSkin.SkinOption.Classic;
					try { actualSkin = (CmsSkin.SkinOption)Enum.Parse(typeof(CmsSkin.SkinOption), skin, true); } catch { }

					_theme = actualSkin;
				}

				return _theme;
			}
		}

		public static string MainColorCode {
			get {
				return CmsSkin.GetPrimaryColorCode(SiteSkin);
			}
		}

		public static string GetWebResourceUrl(string resouceName) {
			return ControlUtilities.GetWebResourceUrl(typeof(AdminBaseMasterPage), resouceName);
		}
	}
}