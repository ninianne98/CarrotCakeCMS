using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;


namespace Carrotware.CMS.UI.Admin {

	public partial class UserPassword : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserFn);

			if (!IsPostBack) {
				txtEmail.Text = CurrentUser.Email;
			} else {
				lblEmail.Text = "";
			}

		}

		protected void btnSaveEmail_Click(object sender, EventArgs e) {
			MembershipUser usr = CurrentUser;
			usr.Email = txtEmail.Text;
			Membership.UpdateUser(usr);
			lblEmail.Text = "Email Updated";
		}


	}
}
