using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class UserPassword : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserFn);

			divInfoMsg.Visible = false;
			InfoMessage.Text = "";

			if (!IsPostBack) {
				txtEmail.Text = SecurityData.CurrentUser.Email;
				ExtendedUserData ud = new ExtendedUserData(SecurityData.CurrentUser.UserName);
				txtNickName.Text = ud.UserNickName;
				txtFirstName.Text = ud.FirstName;
				txtLastName.Text = ud.LastName;
			}
		}

		protected void btnSaveEmail_Click(object sender, EventArgs e) {
			MembershipUser usr = SecurityData.CurrentUser;
			usr.Email = txtEmail.Text;
			Membership.UpdateUser(usr);

			ExtendedUserData ud = new ExtendedUserData(SecurityData.CurrentUser.UserName);
			ud.UserNickName = txtNickName.Text;
			ud.FirstName = txtFirstName.Text;
			ud.LastName = txtLastName.Text;
			ud.Save();

			divInfoMsg.Visible = true;
			InfoMessage.Text = "Profile Updated";
		}


	}
}
