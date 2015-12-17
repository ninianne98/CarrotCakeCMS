using System;
using System.Web.UI;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Base {

	public abstract class BasePage : System.Web.UI.Page {
		//protected GridSorting gs = new GridSorting();

		protected ContentPageHelper pageHelper = new ContentPageHelper();
		protected SiteData siteHelper = new SiteData();
		protected WidgetHelper widgetHelper = new WidgetHelper();
		protected CMSConfigHelper cmsHelper = new CMSConfigHelper();

		public override void Dispose() {
			base.Dispose();

			if (pageHelper != null) {
				pageHelper.Dispose();
			}

			if (widgetHelper != null) {
				widgetHelper.Dispose();
			}

			if (cmsHelper != null) {
				cmsHelper.Dispose();
			}
		}

		protected string CurrentDLLVersion {
			get { return SiteData.CurrentDLLVersion; }
		}

		protected Guid SiteID {
			get {
				return SiteData.CurrentSiteID;
			}
		}

		public bool IsPageRefreshJavaScript {
			get {
				if (Request.Form["__EVENTARGUMENT"] != null) {
					string arg = Request["__EVENTARGUMENT"].ToString();
					string tgt = Request["__EVENTTARGET"].ToString();
					if (!String.IsNullOrEmpty(arg) && !String.IsNullOrEmpty(tgt)) {
						if (tgt.ToLower() == "pagerefresh" && arg.ToLower() == "javascript") {
							return true;
						}
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

		private bool bFound = false;
		private WidgetContainer x = new WidgetContainer();

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