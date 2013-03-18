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


		protected void Page_Load(object sender, EventArgs e) {

			guidWidgetID = GetGuidParameterFromQuery("widgetid");
			guidContentID = GetGuidPageIDFromQuery();

			cmsHelper.OverrideKey(guidContentID);

			if (!IsPostBack) {
				BindDataGrid();
			}
			GetCtrlName();
		}

		protected void GetCtrlName() {
			string sName = "";

			CMSPlugin plug = (from p in cmsHelper.ToolboxPlugins
							  join w in cmsHelper.cmsAdminWidget on p.FilePath.ToLower() equals w.ControlPath.ToLower()
							  where w.Root_WidgetID == guidWidgetID
							  select p).FirstOrDefault();

			if (plug != null) {
				sName = plug.Caption;
			}

			var ww = (from w in cmsHelper.cmsAdminWidget
					  where w.Root_WidgetID == guidWidgetID
					  select w).FirstOrDefault();

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
			List<Guid> lstDel = new List<Guid>();
			foreach (GridViewRow dgItem in gvPages.Rows) {
				CheckBox chkContent = (CheckBox)dgItem.FindControl("chkContent");
				if (chkContent != null) {
					if (chkContent.Checked) {
						lstDel.Add(new Guid(chkContent.Attributes["value"].ToString()));
					}
				}
			}

			widgetHelper.RemoveVersions(lstDel);

			BindDataGrid();
		}

	}
}