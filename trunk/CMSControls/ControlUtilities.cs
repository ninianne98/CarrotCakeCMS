using System;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Controls {
	public class ControlUtilities {

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

		public static string GetWebResourceUrl(Type type, string resource) {
			return CachedPage.ClientScript.GetWebResourceUrl(type, resource);
		}

		public static Control ParseControlByName(Type type, string resourceName) {
			string s = GetManifestResourceStream(resourceName);

			return CachedPage.ParseControl(s);
		}

		public static Control ParseControl(Type type, string resource) {

			return CachedPage.ParseControl(resource);
		}

		public static string GetManifestResourceStream(string sResouceName) {
			Assembly _assembly = Assembly.GetExecutingAssembly();
			string sReturn = null;

			using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream(sResouceName))) {
				sReturn = oTextStream.ReadToEnd();
			}

			return sReturn;
		}


		public ControlUtilities() {
			ResetFind();
		}

		public void ResetFind() {
			bFoundPage = false;
			page = null;

			bFoundPlaceHolder = false;
			plcholder = null;

			bFoundControl = false;
			ctrl = null;
		}

		public ContentPage GetContainerContentPage(object X) {
			bFoundPage = false;
			page = null;

			ContentPage cp = null;
			Page foundPage = FindPage(X);

			try {
				object obj = ReflectionUtilities.GetPropertyValue(foundPage, "pageContents");

				if (foundPage != null && obj is ContentPage) {
					cp = obj as ContentPage;
				}
			} catch (Exception ex) { }

			return cp;
		}

		public SiteData GetContainerSiteData(object X) {
			bFoundPage = false;
			page = null;

			SiteData sd = null;
			Page foundPage = FindPage(X);

			try {
				object obj = ReflectionUtilities.GetPropertyValue(foundPage, "theSite");

				if (foundPage != null && obj is SiteData) {
					sd = obj as SiteData;
				}
			} catch (Exception ex) { }

			return sd;
		}

		bool bFoundPage = false;
		Page page = null;
		public Page FindPage(object X) {

			if (X is Page) {
				bFoundPage = true;
				page = (Page)X;
			} else {
				if (!bFoundPage) {
					if (X is Control) {
						Control c = (Control)X;
						FindPage(c.Parent);
					}
				}
			}

			return page;
		}


		bool bFoundPlaceHolder = false;
		PlaceHolder plcholder = null;
		public PlaceHolder FindPlaceHolder(string ControlName, Control X) {

			if (X is Page) {
				bFoundPlaceHolder = false;
				plcholder = new PlaceHolder();
			}

			foreach (Control c in X.Controls) {
				if (c.ID == ControlName && c is PlaceHolder) {
					bFoundPlaceHolder = true;
					plcholder = (PlaceHolder)c;
					return plcholder;
				} else {
					if (!bFoundPlaceHolder) {
						FindPlaceHolder(ControlName, c);
					}
				}
			}

			return plcholder;
		}


		bool bFoundControl = false;
		Control ctrl = null;
		public Control FindControl(string ControlName, Control X) {

			if (X is Page) {
				bFoundControl = false;
				ctrl = new Control();
			}

			foreach (Control c in X.Controls) {
				if (c.ID == ControlName && c is Control) {
					bFoundControl = true;
					ctrl = (Control)c;
					return ctrl;
				} else {
					if (!bFoundControl) {
						FindControl(ControlName, c);
					}
				}
			}

			return ctrl;
		}

	}
}