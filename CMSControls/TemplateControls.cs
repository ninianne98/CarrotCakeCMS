using System;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;

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

	[ToolboxData("<{0}:ListItemImageThumb runat=server></{0}:ListItemImageThumb>")]
	public class ListItemImageThumb : Image {

		[Category("Appearance")]
		[DefaultValue(null)]
		public Guid ContentID {
			get {
				Guid s = Guid.Empty;
				try { s = new Guid(ViewState["ContentID"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["ContentID"] = value;
			}
		}

		public override Unit BorderWidth {
			get {
				if (base.BorderWidth.IsEmpty) {
					return Unit.Pixel(0);
				} else {
					return base.BorderWidth;
				}
			}

			set {
				base.BorderWidth = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(100)]
		public int ThumbSize {
			get {
				int s = 100;
				try { s = int.Parse(ViewState["ThumbSize"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["ThumbSize"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool ScaleImage {
			get {
				bool s = true;
				if (ViewState["ScaleImage"] != null) {
					try { s = (bool)ViewState["ScaleImage"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["ScaleImage"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool PerformURLResize {
			get {
				bool s = false;
				if (ViewState["PerformURLResize"] != null) {
					try { s = (bool)ViewState["PerformURLResize"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["PerformURLResize"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			base.Render(writer);
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);
		}

		private ControlUtilities cu = new ControlUtilities();

		private void SetFileInfo(string sImageName, string sTemplateFolderPath) {
			string sFieldValue = sImageName;

			if (String.IsNullOrEmpty(sFieldValue)) {
				// if page itself has no image, see if the image had been specified directly
				sFieldValue = this.ImageUrl;
			}
			if (String.IsNullOrEmpty(sFieldValue)) {
				this.Visible = false;
			}
			if (!String.IsNullOrEmpty(sFieldValue) && !sFieldValue.StartsWith("/")) {
				sFieldValue = sTemplateFolderPath + sFieldValue;
			}

			if (!String.IsNullOrEmpty(sFieldValue) && !File.Exists(HttpContext.Current.Server.MapPath(sFieldValue))) {
				ContentPage cp = cu.GetContainerContentPage(this);
				sFieldValue = sImageName;
				if (String.IsNullOrEmpty(sFieldValue)) {
					sFieldValue = this.ImageUrl;
				}

				if (cp != null) {
					if (!String.IsNullOrEmpty(sFieldValue) && !sFieldValue.StartsWith("/")) {
						sFieldValue = cp.TemplateFolderPath + sFieldValue;
					}
				}
			}

			if (PerformURLResize && !String.IsNullOrEmpty(sFieldValue)) {
				sFieldValue = String.Format("/carrotwarethumb.axd?scale={0}&thumb={1}&square={2}", ScaleImage, HttpUtility.UrlEncode(sFieldValue), ThumbSize);
			}

			this.ImageUrl = sFieldValue;
		}

		protected override void OnDataBinding(EventArgs e) {
			RepeaterItem container = (RepeaterItem)this.NamingContainer;
			string sFieldValue = string.Empty;

			if (DataBinder.Eval(container, "DataItem.Thumbnail") != null) {
				sFieldValue = DataBinder.Eval(container, "DataItem.Thumbnail").ToString();
			}
			string sTemplatePath = DataBinder.Eval(container, "DataItem.TemplateFolderPath").ToString();

			SetFileInfo(sFieldValue, sTemplatePath);

			Guid pageID = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());

			this.ContentID = pageID;

			base.OnDataBinding(e);
		}
	}

	//========================================
	[ToolboxData("<{0}:ListItemNavText runat=server></{0}:ListItemNavText>")]
	public class ListItemNavText : Control, ITextControl {

		public enum NavTextFieldName {
			Root_ContentID,
			SiteID,
			FileName,
			PageActive,
			CreateDate,
			ContentID,
			Parent_ContentID,
			TitleBar,
			NavMenuText,
			PageHead,
			PageTextPlainSummaryMedium,
			PageTextPlainSummary,
			CommentCount,
			NavOrder,
			EditUserId,
			EditDate,
			GoLiveDate,
			RetireDate,
			Author_UserName,
			Author_EditorURL,
			Author_UserBio,
			Author_EmailAddress,
			Author_UserNickName,
			Author_FirstName,
			Author_LastName,
			Author_FullName_FirstLast,
			Author_FullName_LastFirst,
			Credit_UserName,
			Credit_EditorURL,
			Credit_UserBio,
			Credit_EmailAddress,
			Credit_UserNickName,
			Credit_FirstName,
			Credit_LastName,
			Credit_FullName_FirstLast,
			Credit_FullName_LastFirst,
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public Guid ContentID {
			get {
				Guid s = Guid.Empty;
				try { s = new Guid(ViewState["ContentID"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["ContentID"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string NavigateUrl {
			get {
				string s = (string)ViewState["NavigateUrl"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["NavigateUrl"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string Text {
			get {
				string s = (string)ViewState["Text"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["Text"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("{0}")]
		public string FieldFormat {
			get {
				String s = ViewState["FieldFormat"] as String;
				return ((s == null) ? "{0}" : s);
			}
			set {
				ViewState["FieldFormat"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("NavMenuText")]
		public NavTextFieldName DataField {
			get {
				string s = (string)ViewState["DataField"];
				NavTextFieldName c = NavTextFieldName.NavMenuText;
				if (!String.IsNullOrEmpty(s)) {
					try {
						c = (NavTextFieldName)Enum.Parse(typeof(NavTextFieldName), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["DataField"] = value.ToString();
			}
		}

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			writer.Write(this.Text);
		}

		protected override void OnDataBinding(EventArgs e) {
			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			string sFieldValue = string.Empty;
			string sValue = "";
			string sField = DataField.ToString();

			try {
				if (sField.StartsWith("Author_") || sField.StartsWith("Credit_")) {
					SiteNav sn = (SiteNav)DataBinder.GetDataItem(container);
					if (sn != null) {
						ExtendedUserData usr = null;
						if (sField.StartsWith("Credit_")) {
							sField = DataField.ToString().Replace("Credit_", String.Empty);
							usr = sn.GetCreditUserInfo();
						}

						if (sField.StartsWith("Author_") || usr == null) {
							sField = DataField.ToString().Replace("Credit_", String.Empty).Replace("Author_", String.Empty);
							usr = sn.GetUserInfo();
						}

						if (usr != null) {
							object obj = ReflectionUtilities.GetPropertyValue(usr, sField);
							if (obj != null) {
								sValue = obj.ToString();
							}
						}
					}

					sFieldValue = String.Format(FieldFormat, sValue);
				} else {
					sFieldValue = String.Format(FieldFormat, DataBinder.Eval(container, "DataItem." + sField));
				}
			} catch {
				if (!SiteData.IsWebView) {
					sFieldValue = sField;
				}
			}

			this.Text = sFieldValue;

			string sFileName = DataBinder.Eval(container, "DataItem.FileName").ToString();
			Guid pageID = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());

			this.NavigateUrl = sFileName;
			this.ContentID = pageID;

			base.OnDataBinding(e);
		}
	}

	//========================================
	public class DefaultContentCommentFormThanks : ITemplate {

		public DefaultContentCommentFormThanks() { }

		public void InstantiateIn(Control container) {
			PlaceHolder ph = new PlaceHolder();
			ph.ID = "DefaultContentCommentFormThanks";

			ph.Controls.Add(new Literal { Text = "<p> Thank you for your comment </p>" });

			container.Controls.Add(ph);
		}
	}

	//========================================
	public class DefaultContentCommentEntryForm : ITemplate {

		private Control GetCtrl(Control X) {
			ControlUtilities cu = new ControlUtilities(X);
			Control userControl = cu.CreateControlFromResource("Carrotware.CMS.UI.Controls.ucContactForm.ascx");

			return userControl;
		}

		public DefaultContentCommentEntryForm() { }

		public void InstantiateIn(Control container) {
			PlaceHolder ph = new PlaceHolder();
			ph.ID = "DefaultContentCommentForm";
			container.Controls.Add(ph);

			Control control = GetCtrl(ph);

			ph.Controls.Add(control);
		}
	}

	//========================================
	/*
	public class DefaultContentCommentEntryFormB : ITemplate {
		public DefaultContentCommentEntryFormB() { }

		public void InstantiateIn(Control container) {
			PlaceHolder ph = new PlaceHolder();
			ph.ID = "DefaultContentCommentForm";

			string sValidationMethodName = "__carrotware_ValidateLongText";

			string sVG = "ContentCommentForm";

			ph.Controls.Add(new Literal { Text = "<div>\r\n " });
			ph.Controls.Add(new Label { ID = "ContentCommentFormMsg", Text = " " });
			ph.Controls.Add(new Literal { Text = "</div>\r\n" });

			ph.Controls.Add(new Literal { Text = "<div class=\"comment-form-outer\">\r\n" });

			// CommenterName
			ph.Controls.Add(new Literal { Text = "<div class=\"comment-form\"> <label class=\"comment-form-caption\"> Name: " });
			ph.Controls.Add(new RequiredFieldValidator {
				ID = "CommenterNameValid",
				ControlToValidate = "CommenterName",
				ErrorMessage = "*",
				ValidationGroup = sVG
			});
			ph.Controls.Add(new Literal { Text = " </label> " });
			ph.Controls.Add(new TextBox { ID = "CommenterName", Columns = 40, MaxLength = 100, ValidationGroup = sVG });
			ph.Controls.Add(new Literal { Text = "<br />\r\n</div>\r\n" });

			// CommenterEmail
			ph.Controls.Add(new Literal { Text = "<div class=\"comment-form\"> <label class=\"comment-form-caption\">  E-mail: " });
			ph.Controls.Add(new RequiredFieldValidator {
				ID = "CommenterEmailValid",
				ControlToValidate = "CommenterEmail",
				ErrorMessage = "*",
				ValidationGroup = sVG
			});
			ph.Controls.Add(new Literal { Text = " </label> " });
			ph.Controls.Add(new TextBox { ID = "CommenterEmail", Columns = 40, MaxLength = 100, ValidationGroup = sVG });
			ph.Controls.Add(new Literal { Text = "<br />\r\n</div>\r\n" });

			// CommenterURL
			ph.Controls.Add(new Literal { Text = "<div class=\"comment-form\"> <label class=\"comment-form-caption\">  Website: " });
			ph.Controls.Add(new Literal { Text = " </label> " });
			ph.Controls.Add(new TextBox { ID = "CommenterURL", Columns = 40, MaxLength = 100, ValidationGroup = sVG });
			ph.Controls.Add(new Literal { Text = "<br />\r\n</div>\r\n" });

			// VisitorComments
			ph.Controls.Add(new Literal { Text = "<div class=\"comment-form\"> <label class=\"comment-form-caption\"> Comment:  " });
			ph.Controls.Add(new CustomValidator {
				ID = "VisitorCommentsValid",
				ControlToValidate = "VisitorComments",
				ErrorMessage = "**",
				ClientValidationFunction = sValidationMethodName,
				ValidationGroup = sVG
			});
			ph.Controls.Add(new Literal { Text = " </label> " });
			ph.Controls.Add(new TextBox { ID = "VisitorComments", Columns = 40, MaxLength = 1024, Rows = 8, TextMode = TextBoxMode.MultiLine });
			ph.Controls.Add(new Literal { Text = "<br />\r\n</div>\r\n" });

			// ContentCommentCaptcha
			ph.Controls.Add(new Literal { Text = "<div style=\"clear: both;\"></div>\r\n<div class=\"comment-form-captcha\"> " });
			ph.Controls.Add(new RequiredFieldValidator {
				ID = "ContentCommentCaptchaValid",
				ControlToValidate = "ContentCommentCaptcha",
				ErrorMessage = "**",
				ValidationGroup = sVG
			});
			ph.Controls.Add(new Captcha { ID = "ContentCommentCaptcha", ValidationGroup = sVG });
			ph.Controls.Add(new Literal { Text = "</div>\r\n" });

			// SubmitCommentButton
			ph.Controls.Add(new Literal { Text = "<div style=\"clear: both;\"></div><br />\r\n<div class=\"comment-form-button\">" });
			ph.Controls.Add(new Button { ID = "SubmitCommentButton", Text = "Submit Comment", ValidationGroup = sVG });
			ph.Controls.Add(new Literal { Text = "</div>\r\n" });

			ph.Controls.Add(new Literal { Text = "</div>\r\n" });

			jsHelperLib js = new jsHelperLib();
			container.Controls.Add(js);
			container.Controls.Add(ph);
		}
	}
	*/

	//========================================
	public class DefaultSearchBoxForm : ITemplate {

		public DefaultSearchBoxForm() { }

		private Control GetCtrl(Control X) {
			ControlUtilities cu = new ControlUtilities(X);
			Control userControl = cu.CreateControlFromResource("Carrotware.CMS.UI.Controls.ucSearchForm.ascx");

			return userControl;
		}

		public void InstantiateIn(Control container) {
			PlaceHolder ph = new PlaceHolder();
			ph.ID = "DefaultSearchBoxForm";
			container.Controls.Add(ph);

			Control c = GetCtrl(ph);
			ph.Controls.Add(c);

			//ph.Controls.Add(new Literal { Text = "<div class=\"search-form-outer\">\r\n" });

			//// SearchText
			//ph.Controls.Add(new Literal { Text = "<div class=\"search-form-text\"> " });
			//ph.Controls.Add(new TextBox { ID = "SearchText", Columns = 16, MaxLength = 40 });
			//ph.Controls.Add(new Literal { Text = " </div>\r\n" });

			//// btnSiteSearch
			//ph.Controls.Add(new Literal { Text = "<div class=\"search-form-button\"> " });
			//ph.Controls.Add(new Button { ID = "btnSiteSearch", Text = "Search" });
			//ph.Controls.Add(new Literal { Text = "</div>\r\n" });

			//ph.Controls.Add(new Literal { Text = " </div>\r\n" });
		}
	}

	//========================================
	public class DefaultSummaryTemplate : ITemplate {

		public DefaultSummaryTemplate() { }

		private Control GetCtrl(Control X) {
			cu = new ControlUtilities(X);
			Control userControl = cu.CreateControlFromResource("Carrotware.CMS.UI.Controls.ucSummaryDisplay.ascx");

			return userControl;
		}

		private ControlUtilities cu = new ControlUtilities();

		public void InstantiateIn(Control container) {
			PlaceHolder ph = new PlaceHolder();
			container.Controls.Add(ph);

			Control c = GetCtrl(ph);

			PostMetaWordList wplCat = (PostMetaWordList)cu.FindControl("wplCat", c);
			PostMetaWordList wpltag = (PostMetaWordList)cu.FindControl("wpltag", c);

			wplCat.DataBinding += new EventHandler(pmwlList_DataBinding);
			wpltag.DataBinding += new EventHandler(pmwlList_DataBinding);

			ph.Controls.Add(c);
		}

		private void pmwlList_DataBinding(object sender, EventArgs e) {
			PostMetaWordList pmContent = (PostMetaWordList)sender;
			RepeaterItem container = (RepeaterItem)pmContent.NamingContainer;
			Guid guidSender = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());
			pmContent.AssignedRootContentID = guidSender;
		}
	}

	public class DefaultCommentTemplate : ITemplate {

		public DefaultCommentTemplate() { }

		private Control GetCtrl(Control X) {
			cu = new ControlUtilities(X);
			Control userControl = cu.CreateControlFromResource("Carrotware.CMS.UI.Controls.ucCommentDisplay.ascx");

			return userControl;
		}

		private ControlUtilities cu = new ControlUtilities();

		public void InstantiateIn(Control container) {
			PlaceHolder ph = new PlaceHolder();
			container.Controls.Add(ph);

			Control c = GetCtrl(ph);

			ph.Controls.Add(c);
		}
	}

	//========================================
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:ListItemCommentText runat=server></{0}:ListItemCommentText>")]
	public class ListItemCommentText : Control, ITextControl {

		public enum CommentTextFieldName {
			ContentCommentID,
			Root_ContentID,
			CreateDate,
			CommenterIP,
			CommenterName,
			CommenterEmail,
			PostCommentText,
			PostCommentEscaped,
			IsApproved,
			IsSpam
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public Guid ContentCommentID {
			get {
				Guid s = Guid.Empty;
				try { s = new Guid(ViewState["ContentCommentID"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["ContentCommentID"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string Text {
			get {
				string s = (string)ViewState["Text"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["Text"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("{0}")]
		public string FieldFormat {
			get {
				String s = ViewState["FieldFormat"] as String;
				return ((s == null) ? "{0}" : s);
			}
			set {
				ViewState["FieldFormat"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("CommenterName")]
		public CommentTextFieldName DataField {
			get {
				string s = (string)ViewState["DataField"];
				CommentTextFieldName c = CommentTextFieldName.CommenterName;
				if (!String.IsNullOrEmpty(s)) {
					try {
						c = (CommentTextFieldName)Enum.Parse(typeof(CommentTextFieldName), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["DataField"] = value.ToString();
			}
		}

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			writer.Write(this.Text);
		}

		protected override void OnDataBinding(EventArgs e) {
			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			bool IsApproved = Convert.ToBoolean(DataBinder.Eval(container, "DataItem.IsApproved").ToString());
			Guid pageID = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());

			this.ContentCommentID = pageID;

			string sTxt1 = String.Format(FieldFormat, DataBinder.Eval(container, "DataItem." + DataField.ToString()));
			if (!IsApproved) {
				sTxt1 = String.Format("{0} {1}", CMSConfigHelper.InactivePagePrefix, sTxt1);
			}
			this.Text = sTxt1;

			base.OnDataBinding(e);
		}
	}
}

/*
<div>
 	<carrot:TwoLevelNavigationTemplate runat="server" ID="TwoLevelNavigationTemplate1" ShowSecondLevel="true">
		<%--<TopNavTemplate>
			<carrot:ListItemWrapper runat="server" ID="wrap">
				~
				<carrot:NavLinkForTemplate ID="lnk" runat="server" NavigateUrl='<%# Eval("FileName").ToString()%>' Text='<%# Eval("NavMenuText").ToString()%>' />
			</carrot:ListItemWrapper>
		</TopNavTemplate>--%>
		<%--<TopNavTemplate>--%>
		<%--<li>
				<asp:HyperLink ID="lnkNav" NavigateUrl='<%# Eval("FileName").ToString()%>' runat="server">
					<%# Eval("NavMenuText").ToString()%></asp:HyperLink>
				<asp:ListItemPlaceHolder runat="server" ID="ph"></asp:ListItemPlaceHolder>
			</li>--%>
		<%--<li><a href='<%# String.Format("{0}", Eval( "FileName"))%>'>
				<%# String.Format("{0}", Eval("NavMenuText"))%></a>
			</li>--%>
		<%--<li>
				<carrot:NavLinkForTemplate ID="lnk" runat="server" Target="_blank"></carrot:NavLinkForTemplate>
			</li>
		</TopNavTemplate>--%>
		<TopNavFooterTemplate>
			</ol>
		</TopNavFooterTemplate>
		<TopNavHeaderTemplate>
			<ol>
		</TopNavHeaderTemplate>
		<%--<SubNavFooterTemplate>
			</div>
		</SubNavFooterTemplate>
		<SubNavHeaderTemplate>
			<div>
		</SubNavHeaderTemplate>--%>
		<%--<SubNavTemplate>
			<carrot:ListItemWrapper runat="server" ID="wrap">
				~~~~
				<carrot:NavLinkForTemplate ID="lnk" runat="server" NavigateUrl='<%# Eval("FileName").ToString()%>' Text='<%# Eval("NavMenuText").ToString()%>' />
			</carrot:ListItemWrapper>
		</SubNavTemplate>--%>
		<SubNavTemplate>
			<li>--
				<%--<asp:HyperLink ID="lnkNav" NavigateUrl='<%# Eval("FileName").ToString()%>' runat="server">
						<%# Eval("NavMenuText").ToString()%></asp:HyperLink>--%>
				<%--<asp:HyperLink ID="lnkNav" NavigateUrl='<%# Eval("FileName").ToString()%>' Text='<%# Eval("NavMenuText").ToString()%>' runat="server">
				</asp:HyperLink>--%>
				<carrot:NavLinkForTemplate ID="lnk" runat="server" NavigateUrl='<%# Eval("FileName").ToString()%>' Text='<%# Eval("NavMenuText").ToString()%>' />
				<%--<li> -- <a href='<%# String.Format("{0}", Eval( "FileName"))%>'>
				<%# String.Format("{0}", Eval("NavMenuText"))%></a>
			</li>--%>
				-- </li>
		</SubNavTemplate>
		<SubNavFooterTemplate>
			</ol>
		</SubNavFooterTemplate>
		<SubNavHeaderTemplate>
			<ol type="i">
		</SubNavHeaderTemplate>
	</carrot:TwoLevelNavigationTemplate>
</div>
*/