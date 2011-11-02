using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Admin {
	public partial class ForgotPassword : BasePage {
		protected void Page_Load(object sender, EventArgs e) {
			divLogonLink.Visible = false;
		}


		protected void cmdReset_Click(object sender, EventArgs e) {
			ProfileManager p = new ProfileManager();
			var r = p.ResetPassword(txtEmail.Text, this);

			if (r) {
				FailureText.Text = "Email sent with new password.";
			} else {
				FailureText.Text = "Invalid username.";
			}
			divLogonLink.Visible = r;

			txtEmail.Text = "";
		}

		protected void cmdCancel_Click(object sender, EventArgs e) {
			Response.Redirect("./default.aspx?");
		}



	}
}
