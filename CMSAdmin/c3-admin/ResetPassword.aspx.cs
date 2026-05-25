using Carrotware.CMS.Core;
using Carrotware.CMS.Security.Models;
using Microsoft.AspNet.Identity;
using System;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class ResetPassword : AdminBasePage {
		private string code = string.Empty;
		private string key = string.Empty;
		private string token = string.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			var user = new ApplicationUser();
			var sd = new SecurityData();
			var password = new ResetPasswordViewModel();

			code = GetQueryValue("code");
			key = GetQueryValue("key");
			token = GetQueryValue("token");

			lnkForgot.HRef = SiteFilename.ForgotPasswordURL;

			string userId = "";
			string email = "";

			phLogonLink.Visible = false;
			phExpired.Visible = false;

			FailureText.Text = string.Empty;

			if (!IsPostBack) {
				if (string.IsNullOrEmpty(code)
						&& string.IsNullOrEmpty(key)
						&& string.IsNullOrEmpty(token)) {
					// Redirect to forgot password if no token is provided
					Response.Redirect(SiteFilename.ForgotPasswordURL);
				} else {
					if (string.IsNullOrEmpty(key)) {
						password = new ResetPasswordViewModel {
							Token = token,
							ValidToken = string.IsNullOrEmpty(token) == false
						};
					}

					if (!string.IsNullOrEmpty(key)) {
						password = sd.DecodeAuthKey(key);
						token = password.Token;
						email = password.Email;
						if (string.IsNullOrEmpty(email) == false) {
							user = securityHelper.UserManager.FindByEmail(email);
						}
					} else {
						if (string.IsNullOrEmpty(token) == false && token.Length > 20) {
							if (string.IsNullOrEmpty(email) == false) {
								user = securityHelper.UserManager.FindByEmail(email);
							}
							if (string.IsNullOrEmpty(userId) == false) {
								user = securityHelper.UserManager.FindById(userId);
							}
							if (user != null) {
								password.ValidToken = !string.IsNullOrEmpty(user.Email) && user.Email.Contains("@");
							} else {
								password.ValidToken = false;
							}
						}
					}

					if (password.ValidToken == false || user == null) {
						password.ValidToken = false;
						phExpired.Visible = true;
					}

					if (user != null && password.ValidToken) {
						userId = user.Id;
						email = user.Email;

						if (user != null) {
							password.ValidToken = sd.ValidatePasswordToken(user, password.Token);
							if (password.ValidToken == false) {
								FailureText.Text = "Reset link is no longer valid.  Please make a new password reset request.";
								password = null;
							} else {
								password.Email = user.Email ?? string.Empty;
							}
						}
					}

					if (user != null && password.ValidToken) {
						txtEmail.Text = password.Email;
						hdnToken.Value = password.Token;
					}

					phReset.Visible = password != null && password.ValidToken;
					phExpired.Visible = phReset.Visible == false;
				}
			}

			divMsg.Visible = (FailureText.Text.Length > 0);
		}

		protected void btnReset_Click(object sender, EventArgs e) {
			var user = new ApplicationUser();
			var sd = new SecurityData();
			bool validToken = false;

			FailureText.Text = string.Empty;

			if (Page.IsValid) {
				string token = hdnToken.Value;
				string email = txtEmail.Text;
				string newPassword = txtNewPassword.Text;
				string newPasswordConf = txtConfirmPassword.Text;

				if (string.IsNullOrEmpty(token) == false && token.Length > 10) {
					if (string.IsNullOrEmpty(email) == false) {
						user = securityHelper.UserManager.FindByEmail(email);
					}

					if (user != null) {
						if (newPassword == newPasswordConf) {
							validToken = user != null && sd.ValidatePasswordToken(user, token);

							if (validToken) {
								var result = sd.ResetPassword(user, token, newPassword);
								if (result.Succeeded) {
									phLogonLink.Visible = true;
									phReset.Visible = false;
								}
							}
						} else {
							validToken = false;
							FailureText.Text = "Passwords did not match.";
						}
					} else {
						phLogonLink.Visible = true;
						phReset.Visible = false;
					}
				} else {
					validToken = false;
					FailureText.Text = "Token was not provided.";
				}
			}

			divMsg.Visible = (FailureText.Text.Length > 0);

			if (validToken == false) {
				phLogonLink.Visible = false;
				phReset.Visible = true;
			}
		}

		protected string GetQueryValue(string keyName) {
			if (Request.QueryString[keyName] != null) {
				return Request.QueryString[keyName].ToString();
			}

			return string.Empty;
		}
	}
}