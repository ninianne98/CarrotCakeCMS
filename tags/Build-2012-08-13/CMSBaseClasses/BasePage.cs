using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Base {
	public class BasePage : System.Web.UI.Page {

		//protected GridSorting gs = new GridSorting();

		protected ContentPageHelper pageHelper = new ContentPageHelper();
		protected SiteData siteHelper = new SiteData();
		protected PageWidgetHelper widgetHelper = new PageWidgetHelper();
		protected CMSConfigHelper cmsHelper = new CMSConfigHelper();

		protected string CurrentDLLVersion {
			get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
		}

		private List<ContentPage> _pages = null;
		protected List<ContentPage> lstActivePages {
			get {
				if (_pages == null) {
					_pages = pageHelper.GetLatestContentList(SiteID, true);
				}
				return _pages;
			}
		}

		protected Guid SiteID {
			get {
				return SiteData.CurrentSiteID;
			}
		}

		public bool IsPageRefreshJavaScript {
			get {
				var arg = Request["__EVENTARGUMENT"].ToString();
				var tgt = Request["__EVENTTARGET"].ToString();
				if (!string.IsNullOrEmpty(arg) && !string.IsNullOrEmpty(tgt)) {
					if (tgt.ToLower() == "pagerefresh" && arg.ToLower() == "javascript") {
						return true;
					}
				}
				return false;
			}
		}




		/*
				public void LoadGrid<T>(GridView TheGrid, HiddenField SortValue, List<T> lst, string sSortKey) {
					List<T> lstVals = null;
					string VSKey = TheGrid.ClientID + "_Data";
					gs.DefaultSort = SortValue.Value;
					if (lst != null) {
						lstVals = lst;
						gs.Sort = SortValue.Value;
					} else {
						SortValue.Value = sSortKey;
						gs.Sort = sSortKey;
						lstVals = (List<T>)ViewState[VSKey];
						lstVals = gs.SortDataList<T>(lstVals);
					}
					ViewState[VSKey] = lstVals;
					TheGrid.DataSource = lstVals;
					TheGrid.DataBind();
				}


				public void LoadGridLive<T>(GridView TheGrid, HiddenField SortValue, List<T> lstVals, string sSortKey) {
					gs.DefaultSort = SortValue.Value;

					SortValue.Value = sSortKey;
					gs.Sort = sSortKey;
					lstVals = gs.SortDataList<T>(lstVals);

					TheGrid.DataSource = lstVals;
					TheGrid.DataBind();
				}
		*/



		bool bFound = false;
		WidgetContainer x = new WidgetContainer();
		protected WidgetContainer FindTheControl(string ControlName, Control X) {

			if (X is Page) {
				bFound = false;
				x = new WidgetContainer();
			}

			foreach (Control c in X.Controls) {
				if (c.ID == ControlName && c is WidgetContainer) {
					bFound = true;
					x = (WidgetContainer)c;
					return x;
				} else {
					if (!bFound) {
						FindTheControl(ControlName, c);
					}
				}
			}
			return x;
		}





	}
}