using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
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
	public partial class PageWidgets : AdminBasePage {

		public Guid guidRootID = Guid.Empty;


		protected void Page_Load(object sender, EventArgs e) {

			guidRootID = GetGuidIDFromQuery();

			if (!IsPostBack) {
				LoadGrid();
			}

		}


		protected void LoadGrid() {

			var lstCont = widgetHelper.GetWidgets(guidRootID, false);

			GeneralUtilities.BindDataBoundControl(gvPages, lstCont);

		}



		protected void btnUpdate_Click(object sender, EventArgs e) {

			List<Guid> lsActive = GeneralUtilities.GetCheckedItemGuidsByValue(gvPages, true, "chkContent");
			widgetHelper.SetStatusList(guidRootID, lsActive, true);

			List<Guid> lsInactive = GeneralUtilities.GetCheckedItemGuidsByValue(gvPages, false, "chkContent");
			widgetHelper.SetStatusList(guidRootID, lsInactive, false);

			LoadGrid();
		}

	}
}