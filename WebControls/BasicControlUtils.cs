﻿using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

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

	public class BasicControlUtils {
		private Page _page;

		public BasicControlUtils() {
			bFoundPage = false;
			_page2 = null;

			_page = GetContainerPage(this);
		}

		public BasicControlUtils(object X) {
			bFoundPage = false;
			_page2 = null;

			if (X != null && X is Control && ((Control)X).Page != null) {
				_page = ((Control)X).Page;
			} else {
				_page = GetContainerPage(X);
			}
		}

		public Page GetContainerPage(object X) {
			bFoundPage = false;
			_page2 = null;

			Page foundPage = FindPage(X);

			return foundPage;
		}

		public Control CreateControlFromResource(string resourceName) {
			string s = GetResourceText(resourceName);

			return CreateControlFromString(s);
		}

		public string GetResourceText(string resourceName) {
			string s = GetManifestResourceStream(resourceName);

			return s;
		}

		public Control CreateControlFromString(string sControlText) {
			return _page.ParseControl(sControlText);
		}

		public string GetWebResourceUrl(Type type, string resource) {
			return WebControlHelper.GetWebResourceUrl(_page, type, resource);
		}

		public string GetWebResourceUrl(string resource) {
			return WebControlHelper.GetWebResourceUrl(_page, this.GetType(), resource);
		}

		public string GetWebResourceUrl(Page page, Type type, string resource) {
			return WebControlHelper.GetWebResourceUrl(page, type, resource);
		}

		public string GetManifestResourceStream(string resource) {
			return WebControlHelper.GetManifestResourceStream(resource);
		}

		public static string GetCtrlText(Control ctrl) {
			StringBuilder sb = new StringBuilder();
			StringWriter tw = new StringWriter(sb);
			HtmlTextWriter hw = new HtmlTextWriter(tw);

			ctrl.RenderControl(hw);

			return sb.ToString();
		}

		private bool bFoundPage = false;
		private Page _page2 = null;

		private Page FindPage(object X) {
			if (X is Page) {
				bFoundPage = true;
				_page2 = (Page)X;
			} else {
				if (!bFoundPage) {
					if (X is Control) {
						Control c = (Control)X;
						FindPage(c.Parent);
					}
				}
			}

			return _page2;
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

		public Control FindControl(Type type, Control X) {
			foreach (Control c in X.Controls) {
				if (c.GetType() == type) {
					bFoundControl = true;
					ctrl = (Control)c;
					return ctrl;
				} else {
					if (!bFoundControl) {
						FindControl(type, c);
					}
				}
			}

			return ctrl;
		}

		public static string SearchQueryParameter {
			get { return "search".ToLowerInvariant(); }
		}

		public static string CurrentScriptName {
			get {
				string sPath = "/";
				try { sPath = HttpContext.Current.Request.ServerVariables["script_name"].ToString(); } catch { }
				return sPath;
			}
		}

		public static string RefererScriptName {
			get {
				string sPath = string.Empty;
				try { sPath = HttpContext.Current.Request.ServerVariables["http_referer"].ToString(); } catch { }
				return sPath;
			}
		}

		public static void MakeXUACompatibleFirst(Page thePage) {
			int iOrder = 0;
			bool bFoundEdge = false;
			HtmlMeta metaEdge = null;
			foreach (var c in thePage.Header.Controls) {
				if (c is HtmlMeta) {
					HtmlMeta metaTest = (HtmlMeta)c;
					if (metaTest.HttpEquiv.ToLowerInvariant() == "x-ua-compatible") {
						metaEdge = new HtmlMeta();
						metaEdge.HttpEquiv = metaTest.HttpEquiv;
						metaEdge.Content = metaTest.Content;
						bFoundEdge = true;
						break;
					}
				}
				iOrder++;
			}

			if (!bFoundEdge) {
				metaEdge = new HtmlMeta();
				metaEdge.HttpEquiv = "X-UA-Compatible";
				metaEdge.Content = "IE=edge,chrome=1";
			}

			if (metaEdge != null && iOrder > 0) {
				thePage.Header.Controls.AddAt(0, metaEdge);
			}
		}

		public static void InsertjQuery(Page thePage) {
			bool bFoundjQuery = false;
			//jquery jq = null;
			//jqueryui jqui = null;
			jquerybasic jquerybasic2 = new jquerybasic();
			jquerybasic2.SelectedSkin = jquerybasic.jQueryTheme.NotUsed;

			foreach (var c in thePage.Header.Controls) {
				//if (c is jquery && !bFoundjQuery) {
				//    jq = (jquery)c;
				//    if (jq.JQVersion.ToLowerInvariant() == (new jquery()).JQVersion) {
				//        bFoundjQuery = true;
				//    }
				//}
				//if (c is jqueryui) {
				//    jqui = (jqueryui)c;
				//}
				if (c is jquerybasic) {
					jquerybasic2 = (jquerybasic)c;
					bFoundjQuery = true;
					break;
				}
			}

			//if (bFoundjQuery) {
			//    if (jq != null) {
			//        thePage.Header.Controls.Remove(jq);
			//    }
			//}

			//if (jqui != null) {
			//    (new PlaceHolder()).Controls.Add(jqui);
			//}

			//(new PlaceHolder()).Controls.Add(jquerybasic2);

			if (!bFoundjQuery) {
				thePage.Header.Controls.AddAt(0, jquerybasic2);
			} else {
				if (jquerybasic2.StylesheetOnly) {
					jquerybasic jb1 = new jquerybasic();
					jb1.SelectedSkin = jquerybasic.jQueryTheme.NotUsed;
					thePage.Header.Controls.AddAt(0, jb1);
				}
			}
		}

		public static jquerybasic FindjQuery(Control control) {
			jquerybasic jquerybasic1 = null;

			if (control is Page) {
				Page thePage = (Page)control;
				foreach (Control c in thePage.Header.Controls) {
					if (c is jquerybasic) {
						jquerybasic1 = (jquerybasic)c;
						break;
					} else {
						jquerybasic1 = FindjQuery(c);
						if (jquerybasic1 != null) {
							break;
						}
					}
				}
			} else {
				foreach (Control c in control.Controls) {
					if (c is jquerybasic) {
						jquerybasic1 = (jquerybasic)c;
						break;
					} else {
						jquerybasic1 = FindjQuery(c);
						if (jquerybasic1 != null) {
							break;
						}
					}
				}
			}

			return jquerybasic1;
		}

		public static jquery FindjQueryMain(Control control) {
			jquery jquery1 = null;

			if (control is Page) {
				Page thePage = (Page)control;
				foreach (Control c in thePage.Header.Controls) {
					if (c is jquery) {
						jquery1 = (jquery)c;
						break;
					} else {
						jquery1 = FindjQueryMain(c);
						if (jquery1 != null) {
							break;
						}
					}
				}
			} else {
				foreach (Control c in control.Controls) {
					if (c is jquery) {
						jquery1 = (jquery)c;
						break;
					} else {
						jquery1 = FindjQueryMain(c);
						if (jquery1 != null) {
							break;
						}
					}
				}
			}

			return jquery1;
		}

		public static jqueryui FindjQueryUI(Control control) {
			jqueryui jqueryui1 = null;

			if (control is Page) {
				Page thePage = (Page)control;
				foreach (Control c in thePage.Header.Controls) {
					if (c is jqueryui) {
						jqueryui1 = (jqueryui)c;
						break;
					} else {
						jqueryui1 = FindjQueryUI(c);
						if (jqueryui1 != null) {
							break;
						}
					}
				}
			} else {
				foreach (Control c in control.Controls) {
					if (c is jqueryui) {
						jqueryui1 = (jqueryui)c;
						break;
					} else {
						jqueryui1 = FindjQueryUI(c);
						if (jqueryui1 != null) {
							break;
						}
					}
				}
			}

			return jqueryui1;
		}

		public static void InsertjQueryMain(Page thePage) {
			jquery jq = FindjQueryMain(thePage);

			if (jq == null) {
				jq = new jquery();
				jq.UseJqueryMigrate = true;

				thePage.Header.Controls.AddAt(0, jq);
			}
		}

		public static void InsertjQueryUI(Page thePage) {
			jqueryui jq = FindjQueryUI(thePage);

			if (jq == null) {
				jq = new jqueryui();

				thePage.Header.Controls.AddAt(1, jq);
			}
		}
	}
}