using System;
using System.IO;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;

namespace Carrotware.CMS.UI.Base {

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

		public ProfileManager(string username) {
			Load(username);
		}
		public ProfileManager(string username, bool anon) {
			Load(username, anon);
		}

		public static MembershipUser GetCurrentUser() {
			string username = HttpContext.Current.User.Identity.Name;
			return Membership.GetUser(username);
		}

		public static MembershipUser GetUserByGuid(Guid providerUserKey) {
			return Membership.GetUser(providerUserKey);
		}

		public static MembershipUser GetUserByName(string username) {
			return Membership.GetUser(username);
		}

		public bool ResetPassword(string Email, Control X) {

			MembershipUser user = null;

			if (!String.IsNullOrEmpty(Email)) {
				MembershipUserCollection membershipCollection = Membership.FindUsersByEmail(Email);
				foreach (MembershipUser userEnum in membershipCollection) {
					user = userEnum;
					break;
				}
			}

			if (user != null) {

				//string MailTemplate = "~/Forgot.txt";
				string MailTemplate = "";

				//string MailTemplate = "Carrotware.CMS.UI.Base.EmailForgotPassMsg.txt";
				var _assembly = System.Reflection.Assembly.GetExecutingAssembly();
				StreamReader _textStreamReader = null;
				_textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.UI.Base.EmailForgotPassMsg.txt"));
				string sBody = _textStreamReader.ReadToEnd();

				string SenderEmail = "";

				string tmpPassword = user.ResetPassword(); // set to known password
				string newPassword = GenerateSimplePassword(); // create simpler password

				try { SenderEmail = System.Configuration.ConfigurationManager.AppSettings["FormEmail"].ToString(); } catch { }
				try { MailTemplate = System.Configuration.ConfigurationManager.AppSettings["FormEmailForgotMessage"].ToString(); } catch { }

				user.ChangePassword(tmpPassword, newPassword); // set to simpler password

				var mailer = new EmailSender {
					From = SenderEmail,
					Recepient = user.Email,
					Subject = "Password Reset",
					TemplateFile = MailTemplate,
					Body = sBody,
					IsHTML = false,
					WebControl = X
				};

				string strHTTPHost = "";
				try { strHTTPHost = HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + ""; } catch { strHTTPHost = ""; }

				string strHTTPProto = "http://";
				try {
					strHTTPProto = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"] + "";
					if (strHTTPProto == "1") {
						strHTTPProto = "https://";
					} else {
						strHTTPProto = "http://";
					}
				} catch { }

				strHTTPHost = strHTTPProto + strHTTPHost.ToLower();

				mailer.Dictionary.Add("<%%UserName%%>", user.UserName);
				mailer.Dictionary.Add("<%%Password%%>", newPassword);
				mailer.Dictionary.Add("<%%SiteURL%%>", strHTTPHost);
				mailer.Dictionary.Add("<%%Time%%>", DateTime.Now.ToString());

				mailer.SendMail();

				return true;
			} else {
				return false;
			}

		}

		//create constant strings for each type of characters
		static string alphaCaps = "QWERTYUIOPASDFGHJKLZXCVBNM";
		static string alphaLow = "qwertyuiopasdfghjklzxcvbnm";
		static string numerics = "1234567890";
		static string special = "@#$";
		static string nonalphanum = "&%=:";
		//create another string which is a concatenation of all above
		string allChars = alphaCaps + alphaLow + numerics + special;
		string specialChars = special + nonalphanum;

		public string GenerateSimplePassword() {
			int length = 8;
			Random r = new Random();
			String generatedPassword = "";

			for (int i = 0; i < length; i++) {
				double rand = r.NextDouble();
				if (i == 0) {
					//First character is an upper case alphabet
					generatedPassword += alphaCaps.ToCharArray()[(int)Math.Floor(rand * alphaCaps.Length)];
				} else if (i == length - 2) {
					generatedPassword += specialChars.ToCharArray()[(int)Math.Floor(rand * specialChars.Length)];
				} else {
					generatedPassword += allChars.ToCharArray()[(int)Math.Floor(rand * allChars.Length)];
				}
			}

			return generatedPassword;
		}


	}
}