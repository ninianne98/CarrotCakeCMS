using Carrotware.CMS.Core;
using Carrotware.CMS.Security;
using Carrotware.CMS.UI.Base;
using System;
using System.Web.Profile;

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class ForgotPassword : BasePage {

		protected void Page_Load(object sender, EventArgs e) {
			divLogonLink.Visible = false;

			FailureText.Text = "";
			InfoMessage.Text = "";

			SetMsgVisible();
		}

		protected void cmdReset_Click(object sender, EventArgs e) {
			bool bReset = false;
			var email = txtEmail.Text ?? string.Empty;
			var usr = SecurityData.GetUserByEmail(email);

			InfoMessage.Text = string.Empty;
			lblErr.Text = string.Empty;
			FailureText.Text = string.Empty;
			divErrMsg.Visible = false;

			try {
				if (usr != null) {
					var sd = new SecurityData();
					sd.ResetPassword(usr.Email);
				}
			} catch (Exception ex) { lblErr.Text = ex.ToString(); }

			InfoMessage.Text = "Please check your email to reset your password.";

			if (!bReset) {
				if (lblErr.Text.ToLowerInvariant().Contains("system.net.mail.smtpclient")
						|| lblErr.Text.ToLowerInvariant().Contains("system.net.mime.mailbnfhelper.readmailaddress")
						|| lblErr.Text.ToLowerInvariant().Contains("system.net.mail.mailaddresscollection")
						|| lblErr.Text.ToLowerInvariant().Contains("system.security.securityexception")) {
					FailureText.Text = "Error sending reset message.";
				}
			}

			divLogonLink.Visible = InfoMessage.Text.Length > 0;

			SetMsgVisible();

			txtEmail.Text = "";
		}

		private void SetMsgVisible() {
			divErrMsg.Visible = !string.IsNullOrEmpty(FailureText.Text);
			divInfoMsg.Visible = !string.IsNullOrEmpty(InfoMessage.Text);
		}

		protected void cmdCancel_Click(object sender, EventArgs e) {
			Response.Redirect(SiteFilename.LogonURL);
		}
	}
}