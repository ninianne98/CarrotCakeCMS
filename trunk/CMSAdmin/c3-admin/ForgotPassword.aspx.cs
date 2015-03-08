using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class ForgotPassword : BasePage {

		protected void Page_Load(object sender, EventArgs e) {
			divLogonLink.Visible = false;

			FailureText.Text = "";
			InfoMessage.Text = "";

			SetMsgVisible();
		}


		protected void cmdReset_Click(object sender, EventArgs e) {
			ProfileManager p = new ProfileManager();
			bool bReset = false;
			lblErr.Text = "";
			FailureText.Text = "";
			divErrMsg.Visible = false;

			try { bReset = p.ResetPassword(txtEmail.Text, this); } catch (Exception ex) { lblErr.Text = ex.ToString(); }
			//bReset = p.ResetPassword(txtEmail.Text, this); 

			if (bReset) {
				InfoMessage.Text = "Email sent with new password.";
			} else {
				if (lblErr.Text.ToLower().Contains("system.net.mail.smtpclient")
						|| lblErr.Text.ToLower().Contains("system.net.mime.mailbnfhelper.readmailaddress")
						|| lblErr.Text.ToLower().Contains("system.net.mail.mailaddresscollection")
						|| lblErr.Text.ToLower().Contains("system.security.securityexception")) {
					FailureText.Text = "Error sending reset message.";
				} else {
					FailureText.Text = "Invalid username/email.";
				}
			}
			divLogonLink.Visible = bReset;

			SetMsgVisible();

			txtEmail.Text = "";
		}

		private void SetMsgVisible() {
			divErrMsg.Visible = !string.IsNullOrEmpty(FailureText.Text);
			divInfoMsg.Visible = !string.IsNullOrEmpty(InfoMessage.Text);
		}

		protected void cmdCancel_Click(object sender, EventArgs e) {
			Response.Redirect(SiteFilename.DashboardURL);
		}

	}

}
