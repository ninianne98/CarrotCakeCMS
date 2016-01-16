using System;
using System.Collections.Generic;
using System.Linq;
using Carrotware.CMS.Core;

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

	public partial class WidgetTime : AdminBasePage {
		public Guid guidContentID = Guid.Empty;
		public Guid guidWidgetID = Guid.Empty;

		private List<Widget> lstPageWidgets = null;

		protected void Page_Load(object sender, EventArgs e) {
			Master.UsesSaved = true;
			Master.HideSave();

			guidWidgetID = GetGuidParameterFromQuery("widgetid");
			guidContentID = GetGuidPageIDFromQuery();

			if (guidContentID != Guid.Empty) {
				cmsHelper.OverrideKey(guidContentID);
				lstPageWidgets = cmsHelper.cmsAdminWidget;
				phNavIndex.Visible = false;
			}

			if (!IsPostBack) {
				Widget ww = null;

				if (guidContentID != Guid.Empty) {
					ww = (from w in lstPageWidgets
						  where w.Root_WidgetID == guidWidgetID
						  select w).FirstOrDefault();
				} else {
					ww = widgetHelper.Get(guidWidgetID);
				}

				ucReleaseDate.SetDate(ww.GoLiveDate);
				ucRetireDate.SetDate(ww.RetireDate);

				chkActive.Checked = ww.IsWidgetActive;

				litControlPath.Text = ww.ControlPath;

				GetCtrlName(ww);
			}
		}

		protected void GetCtrlName(Widget ww) {
			string sName = "";

			CMSPlugin plug = (from p in cmsHelper.ToolboxPlugins
							  where p.FilePath.ToLower() == ww.ControlPath.ToLower()
							  select p).FirstOrDefault();

			if (plug != null) {
				sName = plug.Caption;
			}

			lnkIndex.NavigateUrl = String.Format("{0}?id={1}", SiteFilename.PageWidgetsURL, ww.Root_ContentID);

			litControlPathName.Text = sName;
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			Widget ww = null;

			if (guidContentID != Guid.Empty) {
				ww = (from w in lstPageWidgets
					  where w.Root_WidgetID == guidWidgetID
					  select w).FirstOrDefault();
			} else {
				ww = widgetHelper.Get(guidWidgetID);
			}

			ww.IsPendingChange = true;
			ww.IsWidgetActive = chkActive.Checked;

			ww.EditDate = SiteData.CurrentSite.Now;
			ww.GoLiveDate = ucReleaseDate.GetDate();
			ww.RetireDate = ucRetireDate.GetDate();

			if (guidContentID != Guid.Empty) {
				lstPageWidgets.RemoveAll(x => x.Root_WidgetID == ww.Root_WidgetID);
				lstPageWidgets.Add(ww);
				cmsHelper.cmsAdminWidget = lstPageWidgets;
			} else {
				ww.Save();
			}

			Master.ShowSave();
		}
	}
}