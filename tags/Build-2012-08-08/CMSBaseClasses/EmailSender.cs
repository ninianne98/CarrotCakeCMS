using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Carrotware.CMS.UI.Base {
	public class EmailSender {
		public ListDictionary Dictionary { get; set; }
		public string Recepient { get; set; }
		public Control WebControl { get; set; }
		public string TemplateFile { get; set; }
		public string Subject { get; set; }
		public string From { get; set; }
		public string Body { get; set; }
		public bool IsHTML { get; set; }

		public string SmtpHost { get { return System.Configuration.ConfigurationManager.AppSettings["SmtpHost"].ToString(); } }
		public string SmtpPassword { get { return System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"].ToString(); } }
		public string SmtpUsername { get { return System.Configuration.ConfigurationManager.AppSettings["SmtpEmail"].ToString(); } }

		public EmailSender() {
			Dictionary = new ListDictionary();
		}

		public void SendMail() {
			var mailDefinition = new MailDefinition {
				BodyFileName = TemplateFile,
				From = From,
				Subject = Subject,
				IsBodyHtml = IsHTML
			};

			MailMessage mailMessage = null;
			if (!string.IsNullOrEmpty(Body)) {
				mailMessage = mailDefinition.CreateMailMessage(Recepient, Dictionary, Body, WebControl);
			} else {
				mailMessage = mailDefinition.CreateMailMessage(Recepient, Dictionary, WebControl);
			}

			var client = new SmtpClient();
			if (!string.IsNullOrEmpty(SmtpPassword)) {
				client.Credentials = new System.Net.NetworkCredential(SmtpUsername, SmtpPassword);
			}
			client.Send(mailMessage);
		}


		//public void SendMail(string recepient, Control webControl, string templateFile, string subject, string from, bool html) {
		//    Recepient = recepient;
		//    WebControl = webControl;
		//    TemplateFile = templateFile;
		//    Subject = subject;
		//    IsHTML = html;
		//    From = from;
		//    SendMail();
		//}

	}
}
