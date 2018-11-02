using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using System;

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

	public partial class UserMembership : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserAdmin);

			LoadGrid();
		}

		protected void LoadGrid() {
			GeneralUtilities.BindDataBoundControl(dvMembers, ExtendedUserData.GetUserList());
		}
	}
}