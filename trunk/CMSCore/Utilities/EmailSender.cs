using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Carrotware.CMS.Core {
	public class EmailSender {

		public class EMailSettings {
			public SmtpDeliveryMethod DeliveryMethod { get; set; }
			public string MailDomainName { get; set; }
			public string MailUserName { get; set; }
			public string MailPassword { get; set; }
			public string ReturnAddress { get; set; }
		}


		public Dictionary<string, string> ContentPlaceholders { get; set; }

		private string CurrentDLLVersion {
			get { return SiteData.CurrentDLLVersion; }
		}

		public string Recepient { get; set; }
		public Control WebControl { get; set; }
		public string TemplateFile { get; set; }
		public string MailSubject { get; set; }
		public string MailFrom { get; set; }
		public string Body { get; set; }
		public bool IsHTML { get; set; }

		public EmailSender() {
			ContentPlaceholders = new Dictionary<string, string>();
		}

		public void SendMail() {
			HttpContext context = HttpContext.Current;

			if (!string.IsNullOrEmpty(TemplateFile)) {
				string sFullFilePath = context.Server.MapPath(TemplateFile);
				if (File.Exists(sFullFilePath)) {
					using (StreamReader sr = new StreamReader(sFullFilePath)) {
						Body = sr.ReadToEnd();
					}
				}
			}

			EMailSettings mailSettings = new EMailSettings();
			mailSettings.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
			mailSettings.MailDomainName = "";
			mailSettings.MailUserName = "";
			mailSettings.MailPassword = "";
			mailSettings.ReturnAddress = "";

			//parse web.config as XML because of medium trust issues

			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(context.Server.MapPath("~/Web.config"));

			XmlElement xmlMailSettings = xDoc.SelectSingleNode("//system.net/mailSettings/smtp") as XmlElement;

			if (xmlMailSettings != null) {
				if (xmlMailSettings.Attributes["from"] != null) {
					mailSettings.ReturnAddress = xmlMailSettings.Attributes["from"].Value;
				}
				if (xmlMailSettings.Attributes["deliveryMethod"] != null && xmlMailSettings.Attributes["deliveryMethod"].Value.ToLower() == "network") {
					mailSettings.DeliveryMethod = SmtpDeliveryMethod.Network;
					if (xmlMailSettings.HasChildNodes) {
						XmlNode xmlNetSettings = xmlMailSettings.SelectSingleNode("//system.net/mailSettings/smtp/network");
						if (xmlNetSettings != null && xmlNetSettings.Attributes["password"] != null) {
							mailSettings.MailUserName = xmlNetSettings.Attributes["userName"].Value;
							mailSettings.MailPassword = xmlNetSettings.Attributes["password"].Value;
							mailSettings.MailDomainName = xmlNetSettings.Attributes["host"].Value;
						}
					}
				}
			}

			if (string.IsNullOrEmpty(mailSettings.MailDomainName)) {
				mailSettings.MailDomainName = context.Request.ServerVariables["SERVER_NAME"];
			}

			if (string.IsNullOrEmpty(mailSettings.ReturnAddress)) {
				mailSettings.ReturnAddress = "no-reply@" + mailSettings.MailDomainName;
			}

			MailFrom = mailSettings.ReturnAddress;

			MailDefinition mailDefinition = new MailDefinition {
				BodyFileName = TemplateFile,
				From = MailFrom,
				Subject = MailSubject,
				IsBodyHtml = IsHTML
			};

			MailMessage mailMessage = null;

			if (!string.IsNullOrEmpty(Body)) {
				mailMessage = mailDefinition.CreateMailMessage(Recepient, ContentPlaceholders, Body, WebControl);
			} else {
				mailMessage = mailDefinition.CreateMailMessage(Recepient, ContentPlaceholders, WebControl);
			}

			mailMessage.Priority = MailPriority.Normal;
			mailMessage.Headers.Add("X-Originating-IP", context.Request.ServerVariables["REMOTE_ADDR"].ToString());
			mailMessage.Headers.Add("X-Application", "CarrotCake CMS " + CurrentDLLVersion);
			mailMessage.Headers.Add("User-Agent", "CarrotCake CMS " + CurrentDLLVersion);
			mailMessage.Headers.Add("Message-ID", "<" + Guid.NewGuid().ToString().ToLower() + "@" + mailSettings.MailDomainName + ">");

			SmtpClient client = new SmtpClient();
			mailMessage.From = new MailAddress(mailSettings.ReturnAddress);

			if (mailSettings.DeliveryMethod == SmtpDeliveryMethod.Network
					&& !string.IsNullOrEmpty(mailSettings.MailUserName)
					&& !string.IsNullOrEmpty(mailSettings.MailPassword)) {
				client.Host = mailSettings.MailDomainName;
				client.Credentials = new NetworkCredential(mailSettings.MailUserName, mailSettings.MailPassword);
			} else {
				client.Credentials = new NetworkCredential();
			}

			client.Send(mailMessage);
		}

	}
}
