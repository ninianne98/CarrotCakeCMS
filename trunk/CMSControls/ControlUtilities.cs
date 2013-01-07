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
		private Page _page;

		public ControlUtilities() {
			ResetFind();
			_page = null;
		}

		public void AssignPage(Control X) {
			ResetFind();

			if (X != null && X is Control && ((Control)X).Page != null) {
				_page = ((Control)X).Page;
			} else {
				_page = GetContainerPage(X);
			}
		}

		public ControlUtilities(Control X) {
			ResetFind();

			if (X != null && X is Control && ((Control)X).Page != null) {
				_page = ((Control)X).Page;
			} else {
				_page = GetContainerPage(X);
			}
		}

		public ControlUtilities(Page X) {
			ResetFind();
			_page = X;
		}

		public Page GetContainerPage(object X) {
			ResetFind();

			Page foundPage = FindPage(X);

			if (foundPage == null) {
				foundPage = CachedPage;
			}

			return foundPage;
		}

		public string GetResourceUrl(Type type, string resource) {
			string sPath = "";

			if (_page != null) {
				try { sPath = _page.ClientScript.GetWebResourceUrl(type, resource); } catch { }
			} else {
				try { sPath = CachedPage.ClientScript.GetWebResourceUrl(type, resource); } catch { }
			}

			return sPath;
		}

		public Control CreateControlFromResource(Type type, string resourceName) {
			string s = GetManifestResourceStream(resourceName);

			return _page.ParseControl(s);
		}

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
			string sPath = "";

			try { sPath = CachedPage.ClientScript.GetWebResourceUrl(type, resource); } catch { }

			return sPath;
		}

		public static Control ParseControlByName(Type type, string resourceName) {
			string s = GetManifestResourceStream(resourceName);

			return CachedPage.ParseControl(s);
		}

		public static Control ParseControl(Type type, string resource) {

			return CachedPage.ParseControl(resource);
		}

		public static string GetManifestResourceStream(string sResouceName) {
			string sReturn = null;

			//try {
			Assembly _assembly = Assembly.GetExecutingAssembly();
			using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream(sResouceName))) {
				sReturn = oTextStream.ReadToEnd();
			}
			//} catch { }

			return sReturn;
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
			ResetFind();

			ContentPage cp = null;
			Page foundPage = null;

			foundPage = FindPage(X);
			
			try {
				object obj = ReflectionUtilities.GetPropertyValue(foundPage, "pageContents");

				if (foundPage != null && obj is ContentPage) {
					cp = obj as ContentPage;
				}
			} catch (Exception ex) { }

			return cp;
		}

		public SiteData GetContainerSiteData(object X) {
			ResetFind();

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

		private bool bFoundPage = false;
		private Page page = null;
		public Page FindPage(object X) {

			if (!bFoundPage) {
				if (X is Page) {
					bFoundPage = true;
					page = (Page)X;
				} else {
					if (!bFoundPage) {
						if (X is Control && X != null) {
							Control c = (Control)X;
							if (c.Page != null) {
								bFoundPage = true;
								page = c.Page;
							}
						}
						if (!bFoundPage) {
							if (X is Control) {
								Control c = (Control)X;
								FindPage(c.Parent);
							}
						}
					}
				}
			}

			return page;
		}


		private bool bFoundPlaceHolder = false;
		private PlaceHolder plcholder = null;
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


		private bool bFoundControl = false;
		private Control ctrl = null;
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