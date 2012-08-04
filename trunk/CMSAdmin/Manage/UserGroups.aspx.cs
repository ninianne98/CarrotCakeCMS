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


namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class UserGroups : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.GroupAdmin);

			BindData();
		}


		protected void BindData() {

			var dsRoles = (from l in db.aspnet_Roles
						   select l).ToList();

			gvRoles.DataSource = dsRoles;
			gvRoles.DataBind();
		
		}

	}
}