﻿using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Controls {

	[Designer(typeof(ContentCommentFormDesigner))]
	[ParseChildren(true, "CommentEntryTemplate"), PersistChildren(true)]
	[ToolboxData("<{0}:ContentCommentForm runat=server></{0}:ContentCommentForm>")]
	public class ContentCommentForm : BaseServerControl, INamingContainer {

		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateInstance(TemplateInstance.Single)]
		[DefaultValue(null)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[TemplateContainer(typeof(ContentCommentForm))]
		public ITemplate CommentEntryTemplate { get; set; }

		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateInstance(TemplateInstance.Single)]
		[DefaultValue(null)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[TemplateContainer(typeof(ContentCommentForm))]
		public ITemplate CommentThanksTemplate { get; set; }

		[Browsable(true)]
		[DefaultValue(false)]
		public bool AutoApproveAdmin { get; set; }

		[Browsable(true)]
		[DefaultValue(false)]
		public bool RequireAuthentication { get; set; }

		[Browsable(true)]
		[DefaultValue(null)]
		public string AutoApproveGroupName { get; set; }

		[Browsable(true)]
		[DefaultValue(null)]
		public string DirectEmail { get; set; }

		[Browsable(true)]
		[DefaultValue(null)]
		public string DirectEmailKeyName { get; set; }

		[Browsable(true)]
		[DefaultValue(false)]
		public bool NotifyEditors { get; set; }

		protected PlaceHolder phEntry = new PlaceHolder();
		protected List<Control> EntryFormControls = new List<Control>();

		protected PlaceHolder phThanks = new PlaceHolder();
		protected List<Control> ThanksControls = new List<Control>();

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			if (CommentEntryTemplate == null) {
				CommentEntryTemplate = new DefaultContentCommentEntryForm(this);
			}
			if (CommentThanksTemplate == null) {
				CommentThanksTemplate = new DefaultContentCommentFormThanks(this);
			}
		}

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			base.BaseRender(writer);
		}

		protected override void RenderContents(HtmlTextWriter writer) {
			base.BaseRenderContents(writer);
		}

		protected override void CreateChildControls() {
			if (CommentEntryTemplate != null || CommentThanksTemplate != null) {
				this.Controls.Clear();
			}

			phEntry.Visible = true;
			phEntry.Controls.Clear();
			this.Controls.Add(phEntry);

			if (CommentEntryTemplate != null) {
				CommentEntryTemplate.InstantiateIn(phEntry);
			}

			phThanks.Visible = false;
			phThanks.Controls.Clear();
			this.Controls.Add(phThanks);

			if (CommentThanksTemplate != null) {
				CommentThanksTemplate.InstantiateIn(phThanks);
			}

			Literal lit1 = new Literal();
			lit1.Text = "<div style=\"display: none\">";
			Literal lit2 = new Literal();
			lit2.Text = "</div>";

			Button btnNone = new Button();
			btnNone.ID = "btn_" + DateTime.UtcNow.Ticks.ToString();

			phEntry.Controls.AddAt(0, lit2);
			phEntry.Controls.AddAt(0, btnNone);
			phEntry.Controls.AddAt(0, lit1);

			FindEntryFormCtrls(phEntry);
			FindThanksFormCtrls(phThanks);

			Label lbl = (Label)GetEntryFormControl("ContentCommentFormMsg");

			if (lbl != null) {
				lbl.Text = "&nbsp;";
			}

			if (this.RequireAuthentication) {
				phEntry.Visible = false;
			}

			base.CreateChildControls();
		}

		private void FindEntryFormCtrls(Control X) {
			foreach (Control c in X.Controls) {
				EntryFormControls.Add(c);

				if (c is TextBox && c.ID != null) {
					TextBox z = (TextBox)c;

					z.ValidationGroup = "ContentCommentForm";
				}

				if (c is Button && c.ID != null) {
					Button z = (Button)c;
					switch (c.ID) {
						case "SubmitCommentButton":
							z.Click += new EventHandler(this.Submit_ContentCommentForm);
							break;

						default:
							break;
					}
					z.ValidationGroup = "ContentCommentForm";
				}

				FindEntryFormCtrls(c);
			}
		}

		private void FindThanksFormCtrls(Control X) {
			foreach (Control c in X.Controls) {
				ThanksControls.Add(c);
				FindThanksFormCtrls(c);
			}
		}

		protected Control GetEntryFormControl(string ControlName) {
			return (from x in EntryFormControls
					where x.ID != null
					&& x.ID.ToLowerInvariant() == ControlName.ToLowerInvariant()
					select x).FirstOrDefault();
		}

		protected void Submit_ContentCommentForm(object sender, EventArgs e) {
			bool bCaptcha = false;

			Captcha captcha = (Captcha)GetEntryFormControl("ContentCommentCaptcha");
			if (captcha != null) {
				bCaptcha = captcha.Validate();
			} else {
				bCaptcha = true;
			}

			if (bCaptcha) {
				HttpRequest request = HttpContext.Current.Request;

				bool bIgnorePublishState = SecurityData.AdvancedEditMode || SecurityData.IsAdmin || SecurityData.IsSiteEditor;

				SiteNav navData = navHelper.GetLatestVersion(SiteData.CurrentSiteID, !bIgnorePublishState, SiteData.CurrentScriptName);

				Label lblContentCommentFormMsg = (Label)GetEntryFormControl("ContentCommentFormMsg");
				TextBox txtCommenterName = (TextBox)GetEntryFormControl("CommenterName");
				TextBox txtCommenterEmail = (TextBox)GetEntryFormControl("CommenterEmail");
				TextBox txtVisitorComments = (TextBox)GetEntryFormControl("VisitorComments");
				TextBox txtCommenterURL = (TextBox)GetEntryFormControl("CommenterURL");

				string sIP = request.ServerVariables["REMOTE_ADDR"].ToString();

				PostComment pc = new PostComment();
				pc.ContentCommentID = Guid.NewGuid();
				pc.Root_ContentID = navData.Root_ContentID;
				pc.CreateDate = SiteData.CurrentSite.Now;
				pc.IsApproved = false;
				pc.IsSpam = false;
				pc.CommenterIP = sIP;

				if (txtCommenterName != null) {
					pc.CommenterName = txtCommenterName.Text;
				}
				if (txtCommenterEmail != null) {
					pc.CommenterEmail = txtCommenterEmail.Text;
				}
				if (txtVisitorComments != null) {
					pc.PostCommentText = txtVisitorComments.Text;
				}
				if (txtCommenterURL != null) {
					pc.CommenterURL = txtCommenterURL.Text;
				}

				if (SiteData.IsWebView && SecurityData.IsAuthenticated) {
					if ((this.AutoApproveAdmin)) {
						pc.IsApproved = SecurityData.IsAdmin;
					}
					if (!String.IsNullOrEmpty(this.AutoApproveGroupName)) {
						pc.IsApproved = SecurityData.IsUserInRole(this.AutoApproveGroupName);
					}
				}

				pc.Save();

				if (!String.IsNullOrEmpty(this.DirectEmail) || this.NotifyEditors || !String.IsNullOrEmpty(this.DirectEmailKeyName)) {
					List<string> emails = new List<string>();

					if (!String.IsNullOrEmpty(this.DirectEmail)) {
						emails.Add(this.DirectEmail);
					}
					if (!String.IsNullOrEmpty(this.DirectEmailKeyName)) {
						emails.Add(ConfigurationManager.AppSettings[this.DirectEmailKeyName].ToString());
					}
					if (this.NotifyEditors) {
						ContentPage page = navData.GetContentPage();
						emails.Add(page.CreateUser.EmailAddress);

						if (page.EditUser.UserId != page.CreateUser.UserId) {
							emails.Add(page.EditUser.EmailAddress);
						}
						if (page.CreditUserId.HasValue) {
							emails.Add(page.CreditUser.EmailAddress);
						}
					}

					string sEmail = String.Join(";", emails.ToArray());

					string strHTTPHost = String.Empty;
					try { strHTTPHost = request.ServerVariables["HTTP_HOST"] + String.Empty; } catch { strHTTPHost = String.Empty; }

					string hostName = strHTTPHost.ToLowerInvariant();

					string strHTTPProto = "http://";
					try {
						strHTTPProto = request.ServerVariables["SERVER_PORT_SECURE"] + String.Empty;
						if (strHTTPProto == "1") {
							strHTTPProto = "https://";
						} else {
							strHTTPProto = "http://";
						}
					} catch { }

					string mailSubject = String.Format("Comment Form From {0}", hostName);

					strHTTPHost = String.Format("{0}{1}", strHTTPProto, strHTTPHost).ToLowerInvariant();

					string sBody = "Name:   " + pc.CommenterName
						+ "\r\nEmail:   " + pc.CommenterEmail
						+ "\r\nURL:   " + pc.CommenterURL
						+ "\r\n-----------------\r\nComment:\r\n" + HttpUtility.HtmlEncode(pc.PostCommentText)
						+ "\r\n=================\r\n\r\nIP:   " + pc.CommenterIP
						//+ "\r\nSite Page:   " + request.ServerVariables["script_name"].ToString()
						+ "\r\nSite URL:   " + String.Format("{0}{1}", strHTTPHost, request.ServerVariables["script_name"])
						+ "\r\nSite Time:   " + SiteData.CurrentSite.Now.ToString()
						+ "\r\nUTC Time:   " + DateTime.UtcNow.ToString();

					EmailHelper.SendMail(null, sEmail, mailSubject, sBody, false);
				}

				//if (lbl != null && txt1 != null && txt2 != null) {
				//    lbl.Text = "Clicked the button: " + txt1.Text + " - " + txt2.Text;
				//}

				phEntry.Visible = false;
				phThanks.Visible = true;
			}
		}
	}

	//======================================

	public class ContentCommentFormDesigner : ControlDesigner {

		public override void Initialize(IComponent Component) {
			base.Initialize(Component);
			SetViewFlags(ViewFlags.TemplateEditing, true);
		}

		public override string GetDesignTimeHtml() {
			Control myctrl = (Control)base.ViewControl;
			string sType = myctrl.GetType().ToString().Replace(myctrl.GetType().Namespace + ".", "CMS, ");
			string sID = myctrl.ID;

			string sTextOut = "[" + sType + " - " + sID + "]";

			return "<span>" + sTextOut + "</span>";
		}

		public override TemplateGroupCollection TemplateGroups {
			get {
				TemplateGroupCollection collection = new TemplateGroupCollection();
				TemplateGroup group;
				ContentCommentForm control;

				control = (ContentCommentForm)Component;
				group = new TemplateGroup("Item");

				group.AddTemplateDefinition(new TemplateDefinition(this, "CommentEntryTemplate", control, "CommentEntryTemplate", true));
				group.AddTemplateDefinition(new TemplateDefinition(this, "CommentThanksTemplate", control, "CommentThanksTemplate", true));

				collection.Add(group);

				return collection;
			}
		}
	}
}