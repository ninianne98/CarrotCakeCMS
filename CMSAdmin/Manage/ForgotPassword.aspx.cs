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
			bool bReset = false;
			lblErr.Text = "";

			try { bReset = p.ResetPassword(txtEmail.Text, this); } catch (Exception ex) { lblErr.Text = ex.ToString(); }

			if (bReset) {
				FailureText.Text = "Email sent with new password.";
			} else {
				if (lblErr.Text.ToLower().IndexOf("system.net.mail.smtpclient") > 0) {
					FailureText.Text = "Error sending reset message.";
				} else {
					FailureText.Text = "Invalid username/email.";
				}
			}
			divLogonLink.Visible = bReset;

			txtEmail.Text = "";
		}

		protected void cmdCancel_Click(object sender, EventArgs e) {
			Response.Redirect("./default.aspx?");
		}



	}
}
