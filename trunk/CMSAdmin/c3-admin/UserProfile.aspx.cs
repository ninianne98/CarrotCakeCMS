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

	public partial class UserProfile : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserFn);

			divInfoMsg.Visible = false;
			InfoMessage.Text = "";

			if (!IsPostBack) {
				txtEmail.Text = SecurityData.CurrentUser.Email;
				ExtendedUserData exUsr = new ExtendedUserData(SecurityData.CurrentUser.UserName);
				txtNickName.Text = exUsr.UserNickName;
				txtFirstName.Text = exUsr.FirstName;
				txtLastName.Text = exUsr.LastName;
				reBody.Text = exUsr.UserBio;
			}
		}

		protected void btnSaveEmail_Click(object sender, EventArgs e) {
			MembershipUser usr = SecurityData.CurrentUser;
			usr.Email = txtEmail.Text;
			Membership.UpdateUser(usr);

			ExtendedUserData exUsr = new ExtendedUserData(SecurityData.CurrentUser.UserName);
			exUsr.UserNickName = txtNickName.Text;
			exUsr.FirstName = txtFirstName.Text;
			exUsr.LastName = txtLastName.Text;
			exUsr.UserBio = reBody.Text;

			exUsr.Save();

			divInfoMsg.Visible = true;
			InfoMessage.Text = "Profile Updated";
		}


	}
}
