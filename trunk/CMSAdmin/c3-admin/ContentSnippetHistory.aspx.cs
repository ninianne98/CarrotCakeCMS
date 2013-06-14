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
	public partial class ContentSnippetHistory : AdminBasePage {
		public Guid guidSnippetID = Guid.Empty;


		protected void Page_Load(object sender, EventArgs e) {

			guidSnippetID = GetGuidIDFromQuery();

			if (!IsPostBack) {
				BindDataGrid();
			}
		}


		private void BindDataGrid() {

			ContentSnippet current = ContentSnippet.Get(guidSnippetID);
			litSlug.Text = current.ContentSnippetSlug;
			litSnippetName.Text = current.ContentSnippetName;

			if (current.ContentSnippetActive != true) {
				imgStatus.ImageUrl = hdnInactive.Value;
				imgStatus.AlternateText = "Inactive";
			}
			imgStatus.ToolTip = imgStatus.AlternateText;

			var lstS = current.GetHistory();

			GeneralUtilities.BindDataBoundControl(gvPages, lstS);

			foreach (GridViewRow dgItem in gvPages.Rows) {
				CheckBox chkContent = (CheckBox)dgItem.FindControl("chkContent");

				if (chkContent.Attributes["value"].ToString() == current.ContentSnippetID.ToString()) {
					chkContent.Visible = false;
				}
			}

			if (lstS.Count < 1) {
				btnRemove.Visible = false;
			}
		}

		protected void btnRemove_Click(object sender, EventArgs e) {
			List<Guid> lstDel = GeneralUtilities.GetCheckedItemGuidsByValue(gvPages, true, "chkContent");

			foreach (Guid delID in lstDel) {
				ContentSnippet.GetVersion(delID).DeleteThisVersion();
			}

			BindDataGrid();
		}

	}
}