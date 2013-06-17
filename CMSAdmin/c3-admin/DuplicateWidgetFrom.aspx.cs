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
	public partial class DuplicateWidgetFrom : AdminBasePage {

		public Guid guidContentID = Guid.Empty;
		public string sZone = "";

		public string sSearchTerm = string.Empty;
		public string sSortFld = string.Empty;
		public string sSortDir = string.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			guidContentID = GetGuidPageIDFromQuery();
			sZone = GetStringParameterFromQuery("zone");
			cmsHelper.OverrideKey(guidContentID);

			phResults.Visible = false;
			phWidgets.Visible = false;
			phDone.Visible = false;
		}

		protected void btnSearch_Click(object sender, EventArgs e) {
			List<SiteNav> lstContents = new List<SiteNav>();
			int iTake = 10;
			int iTotal = -1;

			phResults.Visible = true;
			sSearchTerm = txtSearchTerm.Text;
			hdnSelectedItem.Value = "";

			GeneralUtilities.BindDataBoundControl(gvWidgets, null);

			if (!string.IsNullOrEmpty(gvPages.DefaultSort)) {
				int pos = gvPages.DefaultSort.LastIndexOf(" ");
				sSortFld = gvPages.DefaultSort.Substring(0, pos).Trim();
				sSortDir = gvPages.DefaultSort.Substring(pos).Trim();
			}

			bool bLimit = chkActive.Checked;

			using (SiteNavHelper navHelper = new SiteNavHelper()) {
				if (!string.IsNullOrEmpty(sSearchTerm)) {
					iTotal = navHelper.GetSiteSearchCount(SiteData.CurrentSiteID, sSearchTerm, bLimit);
					lstContents = navHelper.GetLatestContentSearchList(SiteData.CurrentSiteID, sSearchTerm, bLimit, iTake, 0, sSortFld, sSortDir);
				}
			}

			if (iTotal > 0) {
				if (iTotal > lstContents.Count) {
					litResults.Text = String.Format("Showing {0} of {1} total results", lstContents.Count, iTotal);
				} else {
					litResults.Text = String.Format("Showing {0} results", lstContents.Count);
				}
			}

			GeneralUtilities.BindDataBoundControl(gvPages, lstContents);
		}

		protected void btnLoadWidgets_Click(object sender, EventArgs e) {
			phWidgets.Visible = true;
			GeneralUtilities.BindDataBoundControl(gvPages, null);

			Guid gSrc = new Guid(hdnSelectedItem.Value);
			ContentPage pageSrc = pageHelper.FindContentByID(SiteID, gSrc);

			if (pageSrc != null) {
				litSrc.Text = string.Format("{0}  [{1}]", pageSrc.NavMenuText, pageSrc.FileName);
				GeneralUtilities.BindDataBoundControl(gvWidgets, pageSrc.GetWidgetList());
			} else {
				GeneralUtilities.BindDataBoundControl(gvWidgets, null);
			}

		}

		protected void btnDuplicate_Click(object sender, EventArgs e) {
			Guid gSrc = new Guid(hdnSelectedItem.Value);
			ContentPage pageSrc = pageHelper.FindContentByID(SiteID, gSrc);

			if (pageSrc != null) {

				phDone.Visible = true;
				List<Guid> lstSel = GeneralUtilities.GetCheckedItemGuidsByValue(gvWidgets, true, "chkContent");
				litCount.Text = string.Format(" {0} ", lstSel.Count);

				if (cmsHelper.cmsAdminWidget != null) {

					List<Widget> cacheWidget = cmsHelper.cmsAdminWidget;

					List<Widget> ww = (from w in pageSrc.GetWidgetList()
									   where lstSel.Contains(w.Root_WidgetID) && w.IsLatestVersion == true
									   select w).ToList();

					if (ww != null) {
						foreach (var w in ww) {
							Guid newWidget = Guid.NewGuid();

							Widget wCpy = new Widget {
								Root_ContentID = guidContentID,
								Root_WidgetID = newWidget,
								WidgetDataID = Guid.NewGuid(),
								PlaceholderName = sZone,
								ControlPath = w.ControlPath,
								ControlProperties = w.ControlProperties,
								IsLatestVersion = true,
								IsWidgetActive = w.IsWidgetActive,
								IsPendingChange = true,
								IsWidgetPendingDelete = false,
								WidgetOrder = w.WidgetOrder,
								EditDate = SiteData.CurrentSite.Now
							};

							cacheWidget.Add(wCpy);
						}
					}

					cmsHelper.cmsAdminWidget = cacheWidget;
				}
			}
		}


	}
}