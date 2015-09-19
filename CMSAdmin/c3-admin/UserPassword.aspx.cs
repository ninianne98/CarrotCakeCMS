using System;

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class UserPassword : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserFn);
		}
	}
}