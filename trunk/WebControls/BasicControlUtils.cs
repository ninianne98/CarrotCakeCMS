using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
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

		public string GetWebResourceUrl(Type type, string resource) {
			string sPath = "";

			try {
				sPath = _page.ClientScript.GetWebResourceUrl(type, resource);
				sPath = HttpUtility.HtmlEncode(sPath);
			} catch { }

			return sPath;
		}

		public string GetWebResourceUrl(Page page, Type type, string resource) {
			string sPath = "";

			try {
				sPath = page.ClientScript.GetWebResourceUrl(type, resource);
				sPath = HttpUtility.HtmlEncode(sPath);
			} catch { }

			return sPath;
		}

		public Page GetContainerPage(object X) {
			bFoundPage = false;
			_page2 = null;

			Page foundPage = FindPage(X);

			return foundPage;
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

		public static void MakeXUACompatibleFirst(Page thePage) {
			int iOrder = 0;
			bool bFoundEdge = false;
			HtmlMeta metaEdge = null;
			foreach (var c in thePage.Header.Controls) {
				if (c is HtmlMeta) {
					HtmlMeta metaTest = (HtmlMeta)c;
					if (metaTest.HttpEquiv.ToLower() == "x-ua-compatible") {
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
				//    if (jq.JQVersion.ToLower() == (new jquery()).JQVersion) {
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

		public static jquerybasic FindjQuery(Page thePage) {
			jquerybasic jquerybasic2 = new jquerybasic();
			jquerybasic2.SelectedSkin = jquerybasic.jQueryTheme.NotUsed;

			foreach (var c in thePage.Header.Controls) {
				if (c is jquerybasic) {
					jquerybasic2 = (jquerybasic)c;
					break;
				}
			}

			return jquerybasic2;
		}

	}
}
