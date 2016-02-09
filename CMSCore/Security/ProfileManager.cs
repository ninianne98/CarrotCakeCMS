using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using Carrotware.Web.UI.Controls;

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

			if (!String.IsNullOrEmpty(Email)) {
				MembershipUserCollection membershipCollection = Membership.FindUsersByEmail(Email);
				foreach (MembershipUser userEnum in membershipCollection) {
					user = userEnum;
					break;
				}
			}

			if (user != null) {
				HttpRequest request = HttpContext.Current.Request;
				Assembly _assembly = Assembly.GetExecutingAssembly();

				string sBody = String.Empty;
				using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.Core.Security.EmailForgotPassMsg.txt"))) {
					sBody = oTextStream.ReadToEnd();
				}

				if (user.IsLockedOut && user.LastLockoutDate < DateTime.Now.AddMinutes(-45)) {
					user.UnlockUser();
				}

				string tmpPassword = user.ResetPassword(); // set to known password
				string newPassword = GenerateSimplePassword(); // create simpler password

				user.ChangePassword(tmpPassword, newPassword); // set to simpler password

				string strHTTPHost = String.Empty;
				try { strHTTPHost = request.ServerVariables["HTTP_HOST"].ToString().Trim(); } catch { strHTTPHost = String.Empty; }

				string hostName = strHTTPHost.ToLowerInvariant();

				string strHTTPPrefix = "http://";
				try {
					strHTTPPrefix = request.ServerVariables["SERVER_PORT_SECURE"] == "1" ? "https://" : "http://";
				} catch { strHTTPPrefix = "http://"; }

				strHTTPHost = String.Format("{0}{1}", strHTTPPrefix, strHTTPHost).ToLowerInvariant();

				sBody = sBody.Replace("{%%UserName%%}", user.UserName);
				sBody = sBody.Replace("{%%Password%%}", newPassword);
				sBody = sBody.Replace("{%%SiteURL%%}", strHTTPHost);
				sBody = sBody.Replace("{%%Version%%}", CurrentDLLVersion);
				sBody = sBody.Replace("{%%AdminFolderPath%%}", String.Format("{0}{1}", strHTTPHost, SiteData.AdminFolderPath));

				if (SiteData.CurretSiteExists) {
					sBody = sBody.Replace("{%%Time%%}", SiteData.CurrentSite.Now.ToString());
				} else {
					sBody = sBody.Replace("{%%Time%%}", DateTime.Now.ToString());
				}

				EmailHelper.SendMail(null, user.Email, String.Format("Reset Password {0}", hostName), sBody, false);

				return true;
			} else {
				return false;
			}
		}

		//create constant strings for each type of characters
		private static string alphaCaps = "QWERTYUIOPASDFGHJKLZXCVBNM";

		private static string alphaLow = "qwertyuiopasdfghjklzxcvbnm";
		private static string numerics = "1234567890";
		private static string special = "@#$";
		private static string nonalphanum = "&%=:";

		//create another string which is a concatenation of all above
		private static string allChars = alphaCaps + alphaLow + numerics + special;

		private static string specialChars = special + nonalphanum;

		public static string GenerateSimplePassword() {
			int length = 12;
			Random r = new Random();
			string generatedPassword = String.Empty;

			for (int i = 0; i < length; i++) {
				double rand = r.NextDouble();
				if (i == 0) {
					//First character is an upper case alphabet
					generatedPassword += alphaCaps.ToCharArray()[(int)Math.Floor(rand * alphaCaps.Length)];
				} else if (i == length - 3) {
					generatedPassword += specialChars.ToCharArray()[(int)Math.Floor(rand * specialChars.Length)];
				} else if (i == length - 5) {
					generatedPassword += numerics.ToCharArray()[(int)Math.Floor(rand * numerics.Length)];
				} else if (i == length - 7) {
					generatedPassword += alphaLow.ToCharArray()[(int)Math.Floor(rand * alphaLow.Length)];
				} else {
					generatedPassword += allChars.ToCharArray()[(int)Math.Floor(rand * allChars.Length)];
				}
			}

			return generatedPassword;
		}
	}
}