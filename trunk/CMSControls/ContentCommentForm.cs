using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Controls {
	[ToolboxData("<{0}:ContentCommentForm runat=server></{0}:ContentCommentForm>")]
	public class ContentCommentForm : BaseServerControl, INamingContainer {

		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateInstance(TemplateInstance.Single)]
		[MergableProperty(false)]
		[Browsable(false)]
		[TemplateContainer(typeof(ContentCommentForm))]
		public ITemplate CommentEntryTemplate { get; set; }


		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateInstance(TemplateInstance.Single)]
		[MergableProperty(false)]
		[Browsable(false)]
		[TemplateContainer(typeof(ContentCommentForm))]
		public ITemplate CommentThanksTemplate { get; set; }


		protected PlaceHolder phEntry = new PlaceHolder();
		protected List<Control> EntryFormControls = new List<Control>();

		protected PlaceHolder phThanks = new PlaceHolder();
		protected List<Control> ThanksControls = new List<Control>();

		protected override void OnInit(EventArgs e) {

			base.OnInit(e);

			if (CommentEntryTemplate == null) {
				CommentEntryTemplate = new DefaultContentCommentEntryForm();
			}
			if (CommentThanksTemplate == null) {
				CommentThanksTemplate = new DefaultContentCommentFormThanks();
			}
		}

		protected override void Render(HtmlTextWriter writer) {
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
			if (CommentEntryTemplate != null) {
				CommentEntryTemplate.InstantiateIn(phEntry);
			}

			phThanks.Visible = false;
			phThanks.Controls.Clear();
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

			this.Controls.Add(phEntry);

			this.Controls.Add(phThanks);

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
					&& x.ID.ToLower() == ControlName.ToLower()
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

				bool bIgnorePublishState = SecurityData.AdvancedEditMode || SecurityData.IsAdmin || SecurityData.IsEditor;

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
				pc.CreateDate = DateTime.UtcNow;
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

				pc.Save();

				//if (lbl != null && txt1 != null && txt2 != null) {
				//    lbl.Text = "Clicked the button: " + txt1.Text + " - " + txt2.Text;
				//}

				phEntry.Visible = false;
				phThanks.Visible = true;
			}

		}

	}
}