using Carrotware.Web.UI.Controls;
using System.Linq;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web;
using System;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Core {

	public class ProfileManager {
		private ProfileBase _profile = new ProfileBase();

		public ProfileManager() { }

		public void Load(string username) {
			_profile = ProfileBase.Create(username);
		}

		public void Load(string username, bool anon) {
			_profile = ProfileBase.Create(username, anon);
		}

		public void Update() {
			_profile.Save();
		}

		private static string CurrentDLLVersion {
			get { return SiteData.CurrentDLLVersion; }
		}

		public ProfileManager(string username) {
			Load(username);
		}

		public ProfileManager(string username, bool anon) {
			Load(username, anon);
		}

		public bool ResetPassword(string Email, Control theControl) {
			MembershipUser user = null;

			if (!string.IsNullOrEmpty(Email)) {
				MembershipUserCollection membershipCollection = Membership.FindUsersByEmail(Email);
				foreach (MembershipUser userEnum in membershipCollection) {
					user = userEnum;
					break;
				}
			}

			if (user != null) {
				HttpRequest request = HttpContext.Current.Request;

				string sBody = SiteNavHelperMock.ReadEmbededScript("Carrotware.CMS.Core.Security.EmailForgotPassMsg.txt");

				if (user.IsLockedOut && user.LastLockoutDate < DateTime.Now.AddMinutes(-45)) {
					user.UnlockUser();
				}

				string tmpPassword = user.ResetPassword(); // set to known password
				string newPassword = GenerateSimplePassword(); // create simpler password

				user.ChangePassword(tmpPassword, newPassword); // set to simpler password

				string strHTTPHost = string.Empty;
				try { strHTTPHost = request.ServerVariables["HTTP_HOST"].ToString().Trim(); } catch { strHTTPHost = string.Empty; }
				string hostName = strHTTPHost.ToLowerInvariant();

				string strHTTPPrefix = "http://";
				try {
					strHTTPPrefix = request.ServerVariables["SERVER_PORT_SECURE"] == "1" ? "https://" : "http://";
				} catch { strHTTPPrefix = "http://"; }

				strHTTPHost = string.Format("{0}{1}", strHTTPPrefix, hostName).ToLowerInvariant();

				sBody = sBody.Replace("{%%UserName%%}", user.UserName);
				sBody = sBody.Replace("{%%Password%%}", newPassword);
				sBody = sBody.Replace("{%%SiteURL%%}", strHTTPHost);
				sBody = sBody.Replace("{%%Version%%}", CurrentDLLVersion);
				sBody = sBody.Replace("{%%AdminFolderPath%%}", string.Format("{0}{1}", strHTTPHost, SiteData.AdminFolderPath));

				if (SiteData.CurretSiteExists) {
					sBody = sBody.Replace("{%%Time%%}", SiteData.CurrentSite.Now.ToString());
				} else {
					sBody = sBody.Replace("{%%Time%%}", DateTime.Now.ToString());
				}

				EmailHelper.SendMail(null, user.Email, string.Format("Reset Password {0}", hostName), sBody, false);

				return true;
			} else {
				return false;
			}
		}

		private static string alphaUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private static string alphaLower = "abcdefghijklmnopqrstuvwxyz";
		private static string numericChars = "1234567890";
		private static string specialChars = "@!$}{";

		private static string allChars = alphaUpper + alphaLower + numericChars + specialChars;

		private static int _length = -1;

		private static int GeneratedPasswordLength {
			get {
				if (_length <= 3) {
					_length = Membership.MinRequiredPasswordLength;
					if (_length <= 8) {
						_length = 12;
					}
				}

				return _length;
			}
		}

		public static string GenerateSimplePassword() {
			int length = GeneratedPasswordLength;

			string generatedPassword = SelectRandomString(allChars, 4);

			for (int i = 0; i < length; i++) {
				if (i == 0 || i == 7) {
					generatedPassword += SelectRandomChar(alphaUpper);
				} else if (i == 2 || i == 5) {
					generatedPassword += SelectRandomChar(alphaLower);
				} else if (i == 4 || i == 3) {
					generatedPassword += SelectRandomChar(numericChars);
				} else if (i == 6 || i == 1) {
					generatedPassword += SelectRandomChar(specialChars);
				} else {
					generatedPassword += SelectRandomString(allChars, 3);
				}
			}

			return generatedPassword;
		}

		private static string SelectRandomString(string sourceString, int take) {
			return new string(sourceString.OrderBy(x => Guid.NewGuid()).Take(take).ToArray());
		}

		private static char SelectRandomChar(string sourceString) {
			return SelectRandomString(sourceString, 1).FirstOrDefault();
			//var rand = new Random();
			//int index = rand.Next(sourceString.Length - 1);
			//return sourceString.ToCharArray()[index];
		}
	}
}