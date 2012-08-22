using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Carrotware.CMS.UI.Base {
	public class EmailSender {

		public Dictionary<string, string> ContentPlaceholders { get; set; }

		private string CurrentDLLVersion {
			get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
		}

		public string Recepient { get; set; }
		public Control WebControl { get; set; }
		public string TemplateFile { get; set; }
		public string Subject { get; set; }
		public string From { get; set; }
		public string Body { get; set; }
		public bool IsHTML { get; set; }

		public static string SmtpSender { get { return ConfigurationManager.AppSettings["CarrotSenderEmail"].ToString(); } }
		public static string SmtpHost { get { return ConfigurationManager.AppSettings["CarrotSmtpHost"].ToString(); } }
		public static string SmtpPassword { get { return ConfigurationManager.AppSettings["CarrotSmtpPassword"].ToString(); } }
		public static string SmtpUsername { get { return ConfigurationManager.AppSettings["CarrotSmtpEmail"].ToString(); } }

		public EmailSender() {
			ContentPlaceholders = new Dictionary<string, string>();
		}

		public void SendMail() {
			MailDefinition mailDefinition = new MailDefinition {
				BodyFileName = TemplateFile,
				From = From,
				Subject = Subject,
				IsBodyHtml = IsHTML
			};

			if (!string.IsNullOrEmpty(TemplateFile)) {
				string sFullFilePath = HttpContext.Current.Server.MapPath(TemplateFile);
				if (File.Exists(sFullFilePath)) {
					using (StreamReader sr = new StreamReader(sFullFilePath)) {
						Body = sr.ReadToEnd();
					}
				}
			}

			MailMessage mailMessage = null;

			if (!string.IsNullOrEmpty(Body)) {
				mailMessage = mailDefinition.CreateMailMessage(Recepient, ContentPlaceholders, Body, WebControl);
			} else {
				mailMessage = mailDefinition.CreateMailMessage(Recepient, ContentPlaceholders, WebControl);
			}

			mailMessage.Priority = MailPriority.Normal;
			mailMessage.Headers.Add("X-Application", "CarrotCake CMS " + CurrentDLLVersion);
			mailMessage.Headers.Add("X-Originating-IP", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString());

			SmtpClient client = new SmtpClient();
			if (!string.IsNullOrEmpty(SmtpPassword)) {
				client.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
			} else {
				client.Credentials = new NetworkCredential();
			}
			client.Send(mailMessage);
		}

	}
}
