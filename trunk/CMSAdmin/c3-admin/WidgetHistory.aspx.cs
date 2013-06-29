using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class WidgetHistory : AdminBasePage {
		public Guid guidContentID = Guid.Empty;
		public Guid guidWidgetID = Guid.Empty;

		List<Widget> lstPageWidgets = null;

		protected void Page_Load(object sender, EventArgs e) {

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

				BindDataGrid();
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

			litControlPath.Text = ww.ControlPath;
			litControlPathName.Text = sName;
		}

		private void BindDataGrid() {

			var lstW = widgetHelper.GetWidgetVersionHistory(guidWidgetID);
			var current = lstW.Where(x => x.IsLatestVersion == true).FirstOrDefault();

			GeneralUtilities.BindDataBoundControl(gvPages, lstW);

			foreach (GridViewRow dgItem in gvPages.Rows) {
				CheckBox chkContent = (CheckBox)dgItem.FindControl("chkContent");

				if (chkContent.Attributes["value"].ToString() == current.WidgetDataID.ToString()) {
					chkContent.Visible = false;
				}
			}

			if (lstW.Count < 1) {
				btnRemove.Visible = false;
			}
		}

		protected void btnRemove_Click(object sender, EventArgs e) {
			List<Guid> lstDel = GeneralUtilities.GetCheckedItemGuidsByValue(gvPages, true, "chkContent");

			widgetHelper.RemoveVersions(lstDel);

			BindDataGrid();
		}

	}
}