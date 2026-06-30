using Carrotware.CMS.Core;
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
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Controls {

	[Designer(typeof(ContentCommentFormDesigner))]
	[ParseChildren(true, "CommentEntryTemplate"), PersistChildren(true)]
	[ToolboxData("<{0}:ContentCommentForm runat=server></{0}:ContentCommentForm>")]
	public class ContentCommentForm : BaseServerControl, INamingContainer {

		public class Fields {
			public const string ContentCommentFormMsg = "ContentCommentFormMsg";
			public const string ContentCommentCaptcha = "ContentCommentCaptcha";
			public const string CommenterName = "CommenterName";
			public const string CommenterEmail = "CommenterEmail";
			public const string VisitorComments = "VisitorComments";
			public const string CommenterURL = "CommenterURL";
			public const string SubmitCommentButton = "SubmitCommentButton";
		}

		//=================

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

		[Browsable(true)]
		[DefaultValue("ContentCommentForm")]
		public string ValidationGroup { get; set; }

		protected PlaceHolder _phEntry = new PlaceHolder();
		protected List<Control> _entryFormControls = new List<Control>();

		protected PlaceHolder _phThanks = new PlaceHolder();
		protected List<Control> _thanksControls = new List<Control>();

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			if (this.CommentEntryTemplate == null) {
				this.CommentEntryTemplate = new DefaultContentCommentEntryForm(this);
			}
			if (this.CommentThanksTemplate == null) {
				this.CommentThanksTemplate = new DefaultContentCommentFormThanks(this);
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
			if (this.CommentEntryTemplate != null || this.CommentThanksTemplate != null) {
				this.Controls.Clear();
			}

			if (string.IsNullOrWhiteSpace(this.ID) == false && this.ID.ToLowerInvariant().Contains("widget")
				&& this.PageWidgetID != Guid.Empty) {
				this.ValidationGroup = string.Format("{0}_ValidGroupW", this.ID);
			}

			if (string.IsNullOrWhiteSpace(this.ValidationGroup)) {
				this.ValidationGroup = string.Format("{0}_ValidGroup", this.ClientID);
			}

			_phEntry.Visible = true;
			_phEntry.Controls.Clear();
			this.Controls.Add(_phEntry);

			if (this.CommentEntryTemplate != null) {
				this.CommentEntryTemplate.InstantiateIn(_phEntry);
			}

			_phThanks.Visible = false;
			_phThanks.Controls.Clear();
			this.Controls.Add(_phThanks);

			if (this.CommentThanksTemplate != null) {
				this.CommentThanksTemplate.InstantiateIn(_phThanks);
			}

			var pnl = new Panel();
			pnl.Style.Add("display", "none");

			var btn = new Button();
			//btn.ID = "btn_" + Guid.NewGuid().ToString("N").Substring(0, 12);
			btn.ID = "btn_" + this.ValidationGroup;
			btn.ValidationGroup = this.ValidationGroup;

			pnl.Controls.Add(btn);

			_phEntry.Controls.AddAt(0, pnl);

			FindEntryFormCtrls(_phEntry);
			FindThanksFormCtrls(_phThanks);

			var lbl = (Label)GetEntryFormControl(Fields.ContentCommentFormMsg);

			if (lbl != null) {
				lbl.Text = "&nbsp;";
			}

			if (_entryFormControls.Select(x => x.GetType()).Contains(typeof(jsHelperLib)) == false) {
				var jsh = new jsHelperLib();

				_phEntry.Controls.AddAt(0, jsh);
				_entryFormControls.Insert(0, jsh);
			}

			if (_entryFormControls.Select(x => x.GetType()).Contains(typeof(jsHelperLib))) {
				var litj = new Literal();
				litj.Text = Environment.NewLine +
							Environment.NewLine + "		<script type=\"text/javascript\"> " +
							Environment.NewLine + "			__carrotware_PageValidate('" + this.ValidationGroup + "'); " +
							Environment.NewLine + "		</script>" + Environment.NewLine + Environment.NewLine;

				_phEntry.Controls.Add(litj);
				_entryFormControls.Add(litj);
			}

			if (this.RequireAuthentication) {
				_phEntry.Visible = false;
			}

			base.CreateChildControls();
		}

		private void FindEntryFormCtrls(Control ctrls) {
			foreach (Control c in ctrls.Controls) {
				_entryFormControls.Add(c);

				if (c is BaseValidator) {
					var ctrl = (BaseValidator)c;

					ctrl.ValidationGroup = this.ValidationGroup;
				}

				if (string.IsNullOrWhiteSpace(c.ID) == false) {
					if (c is TextBox) {
						var ctrl = (TextBox)c;

						ctrl.ValidationGroup = this.ValidationGroup;
					}

					if (c is Captcha) {
						var ctrl = (Captcha)c;

						ctrl.ValidationGroup = this.ValidationGroup;
					}

					if (c is Button) {
						var ctrl = (Button)c;
						switch (c.ID) {
							case Fields.SubmitCommentButton:
								ctrl.Click += new EventHandler(this.Submit_ContentCommentForm);
								break;

							default:
								break;
						}

						ctrl.ViewStateMode = ViewStateMode.Disabled;
						ctrl.ValidationGroup = this.ValidationGroup;
						ctrl.CausesValidation = true;
						ctrl.OnClientClick = "return __carrotware_IsPageValid('" + this.ValidationGroup + "')";
					}
				}

				FindEntryFormCtrls(c);
			}
		}

		private void FindThanksFormCtrls(Control ctrl) {
			foreach (Control c in ctrl.Controls) {
				_thanksControls.Add(c);
				FindThanksFormCtrls(c);
			}
		}

		protected Control GetEntryFormControl(string controlName) {
			return (from c in _entryFormControls
					where c.ID != null
							&& c.ID.ToLowerInvariant() == controlName.ToLowerInvariant()
					select c).FirstOrDefault();
		}

		protected void Submit_ContentCommentForm(object sender, EventArgs e) {
			bool isValidCaptcha = false;

			var captcha = (Captcha)GetEntryFormControl(Fields.ContentCommentCaptcha);
			if (captcha != null) {
				isValidCaptcha = captcha.Validate();
			} else {
				isValidCaptcha = true;
			}

			if (isValidCaptcha) {
				HttpRequest request = HttpContext.Current.Request;

				bool bIgnorePublishState = SecurityData.AdvancedEditMode || SecurityData.IsAdmin || SecurityData.IsSiteEditor;

				SiteNav navData = _navHelper.GetLatestVersion(SiteData.CurrentSiteID, !bIgnorePublishState, SiteData.CurrentScriptName);

				var lblContentCommentFormMsg = (Label)GetEntryFormControl(Fields.ContentCommentFormMsg);
				var txtCommenterName = (TextBox)GetEntryFormControl(Fields.CommenterName);
				var txtCommenterEmail = (TextBox)GetEntryFormControl(Fields.CommenterEmail);
				var txtVisitorComments = (TextBox)GetEntryFormControl(Fields.VisitorComments);
				var txtCommenterURL = (TextBox)GetEntryFormControl(Fields.CommenterURL);

				string addr = request.ServerVariables["REMOTE_ADDR"].ToString();

				var pc = new PostComment();
				pc.ContentCommentID = Guid.NewGuid();
				pc.Root_ContentID = navData.Root_ContentID;
				pc.CreateDate = SiteData.CurrentSite.Now;
				pc.IsApproved = false;
				pc.IsSpam = false;
				pc.CommenterIP = addr;

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
					if (!string.IsNullOrEmpty(this.AutoApproveGroupName)) {
						pc.IsApproved = SecurityData.IsUserInRole(this.AutoApproveGroupName);
					}
				}

				pc.Save();

				if (!string.IsNullOrEmpty(this.DirectEmail) || this.NotifyEditors || !string.IsNullOrEmpty(this.DirectEmailKeyName)) {
					List<string> emails = new List<string>();

					if (!string.IsNullOrEmpty(this.DirectEmail)) {
						emails.Add(this.DirectEmail);
					}
					if (!string.IsNullOrEmpty(this.DirectEmailKeyName)) {
						emails.Add(ConfigurationManager.AppSettings[this.DirectEmailKeyName].ToString());
					}
					if (this.NotifyEditors) {
						ContentPage page = navData.GetContentPage();
						emails.Add(page.CreateUser.Email);

						if (page.EditUser.UserId != page.CreateUser.UserId) {
							emails.Add(page.EditUser.Email);
						}
						if (page.CreditUserId.HasValue) {
							emails.Add(page.CreditUser.Email);
						}
					}

					string sEmail = string.Join(";", emails.ToArray());

					string host = string.Empty;
					try { host = request.ServerVariables["HTTP_HOST"].ToString().Trim(); } catch { host = string.Empty; }

					string hostName = host.ToLowerInvariant();

					string hostPrefix = "http://";
					try {
						hostPrefix = request.ServerVariables["SERVER_PORT_SECURE"] == "1" ? "https://" : "http://";
					} catch { hostPrefix = "http://"; }

					host = string.Format("{0}{1}", hostPrefix, hostName).ToLowerInvariant();

					string mailSubject = string.Format("Comment Form From {0}", hostName);

					string sBody = "Name:   " + pc.CommenterName
						+ Environment.NewLine + "Email:   " + pc.CommenterEmail
						+ Environment.NewLine + "URL:   " + pc.CommenterURL
						+ Environment.NewLine + " -----------------"
						+ Environment.NewLine + "Comment:" + Environment.NewLine + HttpUtility.HtmlEncode(pc.PostCommentText)
						+ Environment.NewLine + " ================= "
						+ Environment.NewLine + Environment.NewLine + "IP:   " + pc.CommenterIP
						+ Environment.NewLine + "Site URL:   " + string.Format("{0}{1}", host, request.ServerVariables["script_name"])
						+ Environment.NewLine + "Site Time:   " + SiteData.CurrentSite.Now.ToString()
						+ Environment.NewLine + "UTC Time:   " + DateTime.UtcNow.ToString();

					EmailHelper.SendMail(null, sEmail, mailSubject, sBody, false);
				}

				//if (lbl != null && txt1 != null && txt2 != null) {
				//    lbl.Text = "Clicked the button: " + txt1.Text + " - " + txt2.Text;
				//}

				_phEntry.Visible = false;
				_phThanks.Visible = true;
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

				group.AddTemplateDefinition(new TemplateDefinition(this, nameof(ContentCommentForm.CommentEntryTemplate), control, nameof(ContentCommentForm.CommentEntryTemplate), true));
				group.AddTemplateDefinition(new TemplateDefinition(this, nameof(ContentCommentForm.CommentThanksTemplate), control, nameof(ContentCommentForm.CommentThanksTemplate), true));

				collection.Add(group);

				return collection;
			}
		}
	}
}