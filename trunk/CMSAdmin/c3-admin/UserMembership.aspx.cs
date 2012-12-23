using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Data;
using Carrotware.CMS.UI.Base;


namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class UserMembership : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserAdmin);

			LoadGrid();

		}


		protected void LoadGrid() {
			List<ExtendedUserData> usrs = ExtendedUserData.GetUserList();

			dvMembers.DataSource = usrs;
			dvMembers.DataBind();
		}


	}
}
