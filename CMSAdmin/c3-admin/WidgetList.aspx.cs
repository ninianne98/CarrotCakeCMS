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
	public partial class WidgetList : AdminBasePage {
		public Guid guidContentID = Guid.Empty;
		public string sZone = "";


		protected void Page_Load(object sender, EventArgs e) {
			guidContentID = GetGuidPageIDFromQuery();
			sZone = GetStringParameterFromQuery("zone");
			cmsHelper.OverrideKey(guidContentID);

			Master.UsesSaved = true;
			Master.HideSave();

			if (!IsPostBack) {
				BindDataGrid();
			}
		}

		protected string GetCtrlName(string sCtrlName) {
			string sName = "";
			CMSPlugin plug = (from p in cmsHelper.ToolboxPlugins
							  where p.FilePath.ToLower() == sCtrlName.ToLower()
							  select p).FirstOrDefault();

			if (plug != null) {
				sName = plug.Caption;
			}

			return sName;
		}

		private void BindDataGrid() {

			if (sZone.ToLower() != "cms-all-placeholder-zones") {
				gvPages.Columns[4].Visible = false;
			}

			var lstW = (from aw in cmsHelper.cmsAdminWidget
						where aw.PlaceholderName.ToLower() == sZone.ToLower() || sZone.ToLower() == "cms-all-placeholder-zones"
						orderby aw.PlaceholderName ascending, aw.IsWidgetPendingDelete ascending, aw.IsWidgetActive descending, aw.WidgetOrder
						select aw).ToList();

			GeneralUtilities.BindDataBoundControl(gvPages, lstW);

			foreach (GridViewRow dgItem in gvPages.Rows) {
				Button btnRestore = (Button)dgItem.FindControl("btnRestore");
				Button btnCancel = (Button)dgItem.FindControl("btnCancel");
				Button btnDelete = (Button)dgItem.FindControl("btnDelete");
				Button btnRemove = (Button)dgItem.FindControl("btnRemove");

				HiddenField hdnActive = (HiddenField)dgItem.FindControl("hdnActive");
				HiddenField hdnDelete = (HiddenField)dgItem.FindControl("hdnDelete");

				if (hdnActive != null && hdnDelete != null) {
					bool bActive = Convert.ToBoolean(hdnActive.Value);
					bool bDelete = Convert.ToBoolean(hdnDelete.Value);

					if (bActive) {
						btnDelete.Visible = true;
						btnRemove.Visible = true;
					}

					if (bDelete) {
						btnCancel.Visible = true;
					}

					if (!bActive && !bDelete) {
						btnRestore.Visible = true;
					}

					if (!bDelete) {
						btnDelete.Visible = true;
					}
				}

			}

		}

		protected void ClickAction(object sender, CommandEventArgs e) {
			string sAction = "";
			Guid guidWidget = Guid.Empty;

			if (!string.IsNullOrEmpty(e.CommandName)) {
				var vals = e.CommandName.Split('_');
				sAction = vals[0];
				guidWidget = new Guid(vals[1]);

			}

			cmsHelper.OverrideKey(guidContentID);

			var cacheWidget = cmsHelper.cmsAdminWidget;

			var ww = (from w in cacheWidget
					  where w.Root_WidgetID == guidWidget
					  select w).ToList();

			int iPos = cacheWidget.Max(x => x.WidgetOrder) + 1;

			if (ww != null) {

				foreach (var w in ww) {
					iPos++;

					if (sAction == "delete") {
						w.IsWidgetPendingDelete = true;
						w.IsWidgetActive = false;
					}
					if (sAction == "remove") {
						w.IsWidgetActive = false;
					}
					if (sAction == "cancel" || sAction == "restore") {
						w.IsWidgetPendingDelete = false;
						w.IsWidgetActive = true;
						w.WidgetOrder = iPos;
					}

					w.EditDate = SiteData.CurrentSite.Now;
				}
			}

			cmsHelper.cmsAdminWidget = cacheWidget;

			BindDataGrid();

			Master.ShowSave();
		}




	}
}
