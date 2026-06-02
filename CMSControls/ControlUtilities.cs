using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Controls {

	public class ControlUtilities {
		private Page _page;

		public ControlUtilities() {
			ResetFind();
			_page = null;
		}

		public void AssignPage(Control ctrl) {
			ResetFind();

			if (ctrl != null && ctrl is Control && ctrl.Page != null) {
				_page = ctrl.Page;
			} else {
				_page = GetContainerPage(ctrl);
			}
		}

		public ControlUtilities(Control ctrl) {
			ResetFind();

			if (ctrl != null && ctrl is Control && ctrl.Page != null) {
				_page = ctrl.Page;
			} else {
				_page = GetContainerPage(ctrl);
			}
		}

		public ControlUtilities(Page p) {
			ResetFind();
			_page = p;
		}

		public Page GetContainerPage(object o) {
			ResetFind();

			Page foundPage = FindPage(o);

			if (foundPage == null) {
				foundPage = CachedPage;
			}

			return foundPage;
		}

		public string GetResourceUrl(Type type, string resource) {
			string path = "";

			if (_page != null) {
				try { path = _page.ClientScript.GetWebResourceUrl(type, resource); } catch { }
			} else {
				try { path = CachedPage.ClientScript.GetWebResourceUrl(type, resource); } catch { }
			}

			try {
				path = HttpUtility.HtmlEncode(path);
			} catch { }

			return path;
		}

		public Control CreateControlFromResource(string resourceName) {
			string s = GetResourceText(resourceName);

			return CreateControlFromString(s);
		}

		public string GetResourceText(string resourceName) {
			string s = GetManifestResourceStream(resourceName);

			return s;
		}

		public Control CreateControlFromString(string controlText) {
			return _page.ParseControl(controlText);
		}

		public Control CreateControlFromString(StringBuilder sb) {
			return _page.ParseControl(sb.ToString());
		}

		private static Page _cachedPage;

		private static Page CachedPage {
			get {
				if (_cachedPage == null) {
					_cachedPage = new Page();
					_cachedPage.AppRelativeVirtualPath = "~/";
				}
				return _cachedPage;
			}
		}

		public static string GetWebResourceUrl(Type type, string resource) {
			string path = "";

			try {
				path = CachedPage.ClientScript.GetWebResourceUrl(type, resource);
				path = HttpUtility.HtmlEncode(path);
			} catch { }

			return path;
		}

		public static Control ParseControlByName(string resourceName) {
			string s = GetManifestResourceStream(resourceName);

			return CachedPage.ParseControl(s);
		}

		public static Control ParseControl(string resource) {
			return CachedPage.ParseControl(resource);
		}

		internal static string GetManifestResourceStream(string resourceName) {
			var sb = new StringBuilder();
			var assembly = Assembly.GetExecutingAssembly();
			var a_name = assembly.GetName().Name;

			if (resourceName.ToLowerInvariant().StartsWith(a_name.ToLowerInvariant()) == false) {
				resourceName = string.Format("{0}.{1}", a_name, resourceName);
			}

			using (var sr = new StreamReader(assembly.GetManifestResourceStream(resourceName))) {
				sb.Append(sr.ReadToEnd());
			}

			return sb.ToString();
		}

		internal static string ReadEmbededResource(string resourceName) {
			var assembly = Assembly.GetExecutingAssembly();
			var a_name = assembly.GetName().Name;

			if (resourceName.ToLowerInvariant().StartsWith(a_name.ToLowerInvariant()) == false) {
				resourceName = string.Format("{0}.{1}", a_name, resourceName);
			}

			return GetManifestResourceStream(resourceName);
		}

		public void ResetFind() {
			_isPageFound = false;
			_pageFound = null;

			_isFoundPlaceHolder = false;
			_plcholder = null;

			_isFoundControl = false;
			_ctrl = null;
		}

		public ContentPage GetContainerContentPage(object o) {
			ResetFind();

			ContentPage cp = null;
			Page foundPage = null;

			foundPage = FindPage(o);

			try {
				object obj = ReflectionUtilities.GetPropertyValue(foundPage, "ThePage");

				if (foundPage != null && obj is ContentPage) {
					cp = obj as ContentPage;
				}
			} catch (Exception ex) { }

			return cp;
		}

		public SiteData GetContainerSiteData(object o) {
			ResetFind();

			SiteData sd = null;
			Page foundPage = FindPage(o);

			try {
				object obj = ReflectionUtilities.GetPropertyValue(foundPage, "TheSite");

				if (foundPage != null && obj is SiteData) {
					sd = obj as SiteData;
				}
			} catch (Exception ex) { }

			return sd;
		}

		private bool _isPageFound = false;
		private Page _pageFound = null;

		public Page FindPage(object o) {
			if (!_isPageFound) {
				if (o is Page) {
					_isPageFound = true;
					_pageFound = (Page)o;
				} else {
					if (!_isPageFound) {
						if (o is Control && o != null) {
							Control c = (Control)o;
							if (c.Page != null) {
								_isPageFound = true;
								_pageFound = c.Page;
							}
						}
						if (!_isPageFound) {
							if (o is Control) {
								Control c = (Control)o;
								FindPage(c.Parent);
							}
						}
					}
				}
			}

			return _pageFound;
		}

		private bool _isFoundPlaceHolder = false;
		private PlaceHolder _plcholder = null;

		public PlaceHolder FindPlaceHolder(string controlName, Control ctrl) {
			if (ctrl is Page) {
				_isFoundPlaceHolder = false;
				_plcholder = new PlaceHolder();
			}

			foreach (Control c in ctrl.Controls) {
				if (c.ID == controlName && c is PlaceHolder) {
					_isFoundPlaceHolder = true;
					_plcholder = (PlaceHolder)c;
					return _plcholder;
				} else {
					if (!_isFoundPlaceHolder) {
						FindPlaceHolder(controlName, c);
					}
				}
			}

			return _plcholder;
		}

		private bool _isFoundControl = false;
		private Control _ctrl = null;

		public Control FindControl(string controlName, Control ctrl) {
			if (ctrl is Page) {
				_isFoundControl = false;
				_ctrl = new Control();
			}

			foreach (Control c in ctrl.Controls) {
				if (c.ID == controlName && c is Control) {
					_isFoundControl = true;
					_ctrl = (Control)c;
					return _ctrl;
				} else {
					if (!_isFoundControl) {
						FindControl(controlName, c);
					}
				}
			}

			return _ctrl;
		}

		public Control FindControl(Type type, Control ctrl) {
			foreach (Control c in ctrl.Controls) {
				if (c.GetType() == type) {
					_isFoundControl = true;
					_ctrl = (Control)c;
					return _ctrl;
				} else {
					if (!_isFoundControl) {
						FindControl(type, c);
					}
				}
			}

			return _ctrl;
		}
	}
}