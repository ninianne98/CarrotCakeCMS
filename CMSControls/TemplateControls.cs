using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
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

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:NavLinkForTemplate runat=server></{0}:NavLinkForTemplate>")]

	public class NavLinkForTemplate : Control, IActivateNavItem {

		public string NavigateUrl {
			get {
				return lnk.NavigateUrl;
			}

			set {
				lnk.NavigateUrl = value;
			}
		}

		public string LinkText {
			get {
				return lnk.Text;
			}

			set {
				lnk.Text = value;
			}
		}

		public string Target {
			get {
				return lnk.Target;
			}

			set {
				lnk.Target = value;
			}
		}

		public string ToolTip {
			get {
				return lnk.ToolTip;
			}

			set {
				lnk.ToolTip = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CssClassNormal {
			get {
				string s = (string)ViewState["CssClassNormal"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CssClassNormal"] = value;
				SetCSS();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
				SetCSS();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CssClassHasChild {
			get {
				string s = (string)ViewState["HasChildCssClass"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["HasChildCssClass"] = value;
				SetCSS();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool IsSelected {
			get {
				bool s = false;
				if (ViewState["IsSelected"] != null) {
					try { s = (bool)ViewState["IsSelected"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["IsSelected"] = value;
				SetCSS();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		[Localizable(true)]
		public bool UseDefaultText {
			get {
				bool s = true;
				if (ViewState["UseDefaultText"] != null) {
					try { s = (bool)ViewState["UseDefaultText"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["UseDefaultText"] = value;
				SetCSS();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Guid ContentID {
			get {
				Guid s = Guid.Empty;
				if (ViewState["ContentID"] != null) {
					try { s = new Guid(ViewState["ContentID"].ToString()); } catch { }
				}
				return s;
			}
			set {
				ViewState["ContentID"] = value;
			}
		}


		private void SetCSS() {

			if (!string.IsNullOrEmpty(CssClassNormal) || !string.IsNullOrEmpty(CSSSelected)) {
				string sCSS = "";
				string sSelCss = "";

				if (IsSelected) {
					sSelCss = CSSSelected.Trim();
				}

				sCSS = string.Format("{0} {1}", CssClassNormal.Trim(), sSelCss);

				lnk.CssClass = sCSS.Trim();
			}
		}


		private string _linkPage = string.Empty;
		private string _linkText = string.Empty;

		private HyperLink lnk = new HyperLink();
		private ListItemPlaceHolder ph = new ListItemPlaceHolder();


		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			_linkPage = DataBinder.Eval(container, "DataItem.FileName").ToString();
			_linkText = DataBinder.Eval(container, "DataItem.NavMenuText").ToString();

			Guid pageID = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());

			this.ContentID = pageID;

			LoadCtrsl();

			base.OnDataBinding(e);
		}


		protected override void OnPreRender(EventArgs e) {

			base.OnPreRender(e);

			AssignVals();
		}


		private void AssignVals() {

			SetCSS();

			if (!string.IsNullOrEmpty(_linkPage) && string.IsNullOrEmpty(this.NavigateUrl)) {
				this.NavigateUrl = _linkPage;
			}

			if (!string.IsNullOrEmpty(_linkText) && string.IsNullOrEmpty(this.LinkText)) {
				//var ctrl = FindSubControl(this, _linkText);
				//if (ctrl == null) {
				//    this.Text = _linkText;
				//}
				//if (!this.HasControls()) {
				//    this.Text = _linkText;
				//}
				if (UseDefaultText) {
					this.LinkText = _linkText;
				}
			}
		}


		private void LoadCtrsl() {

			int iMax = this.Controls.Count;
			lnk.Controls.Clear();

			for (int i = 0; i < iMax; i++) {
				lnk.Controls.Add(this.Controls[0]);
			}

			this.Controls.Add(lnk);
			this.Controls.Add(ph);

		}

	}


	//========================================

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:ListItemWrapper runat=server></{0}:ListItemWrapper>")]

	public class ListItemWrapper : Control, IActivateNavItem {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CssClassNormal {
			get {
				string s = (string)ViewState["CssClassNormal"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CssClassNormal"] = value;
				SetTag();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
				SetTag();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool IsSelected {
			get {
				bool s = false;
				if (ViewState["IsSelected"] != null) {
					try { s = (bool)ViewState["IsSelected"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["IsSelected"] = value;
				SetTag();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CssClassHasChild {
			get {
				string s = (string)ViewState["HasChildCssClass"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["HasChildCssClass"] = value;
				SetTag();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string NavigateUrl {
			get {
				string s = (string)ViewState["NavigateUrl"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["NavigateUrl"] = value;
				SetTag();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
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


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string HtmlTagName {
			get {
				string s = (string)ViewState["HtmlTagName"];
				return ((s == null) ? "li" : s);
			}
			set {
				ViewState["HtmlTagName"] = value;
				SetTag();
			}
		}


		private Literal litOpen = new Literal();
		private Literal litClose = new Literal();
		private Control ctrlAll = new Control();


		private void LoadCtrsl() {
			int iMax = this.Controls.Count;

			SetTag();

			ctrlAll.Controls.Clear();
			ctrlAll.Controls.Add(litOpen);

			//instead of wind/unwind, pop stack X times
			for (int i = 0; i < iMax; i++) {
				ctrlAll.Controls.Add(this.Controls[0]);
			}

			ListItemPlaceHolder ph = new ListItemPlaceHolder();

			ctrlAll.Controls.Add(ph);
			ctrlAll.Controls.Add(litClose);

			this.Controls.Clear();
			this.Controls.Add(ctrlAll);

		}


		private void SetTag() {

			litOpen.Text = HtmlTextWriter.TagLeftChar + HtmlTagName + HtmlTextWriter.TagRightChar;
			litClose.Text = HtmlTextWriter.EndTagLeftChars + HtmlTagName + HtmlTextWriter.TagRightChar;

			if (!string.IsNullOrEmpty(CssClassNormal) || !string.IsNullOrEmpty(CSSSelected)) {
				string sCSS = "";
				string sSelCss = "";

				if (IsSelected) {
					sSelCss = CSSSelected.Trim();
				}

				if (!string.IsNullOrEmpty(CssClassNormal) || !string.IsNullOrEmpty(sSelCss)) {
					sCSS = string.Format(" class=\"{0} {1}\"", CssClassNormal.Trim(), sSelCss);
				}

				litOpen.Text = HtmlTextWriter.TagLeftChar + HtmlTagName + sCSS + HtmlTextWriter.TagRightChar;
			}
		}

		//protected override void RenderChildren(HtmlTextWriter writer) {
		//    int indent = writer.Indent++;

		//    writer.Indent = indent + 2;

		//    //writer.WriteLine();
		//    base.RenderChildren(writer);
		//    //writer.WriteLine();

		//    writer.Indent = indent;
		//    writer.Indent--;
		//}

		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			string sFileName = DataBinder.Eval(container, "DataItem.FileName").ToString();
			Guid pageID = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());

			this.NavigateUrl = sFileName;
			this.ContentID = pageID;

			SetTag();

			LoadCtrsl();

			base.OnDataBinding(e);
		}

	}

	//========================================
	[DefaultProperty("Text")]
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
			Author_UserName,
			Author_EmailAddress,
			Author_UserNickName,
			Author_FirstName,
			Author_LastName,
			Author_FullName_FirstLast,
			Author_FullName_LastFirst,
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
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


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string NavigateUrl {
			get {
				string s = (string)ViewState["NavigateUrl"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["NavigateUrl"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string Text {
			get {
				string s = (string)ViewState["Text"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["Text"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("{0}")]
		[Localizable(true)]
		public string FieldFormat {
			get {
				String s = ViewState["FieldFormat"] as String;
				return ((s == null) ? "{0}" : s);
			}
			set {
				ViewState["FieldFormat"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("NavMenuText")]
		[Localizable(true)]
		public NavTextFieldName DataField {
			get {
				string s = (string)ViewState["DataField"];
				NavTextFieldName c = NavTextFieldName.NavMenuText;
				if (!string.IsNullOrEmpty(s)) {
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


		protected override void Render(HtmlTextWriter output) {

			output.Write(this.Text);

		}


		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;
			string sFieldValue = string.Empty;

			if (DataField.ToString().StartsWith("Author_")) {
				string sField = DataField.ToString().Replace("Author_", "");

				string sValue = "";
				SiteNav sn = (SiteNav)DataBinder.GetDataItem(container);
				if (sn != null) {
					using (ExtendedUserData c = sn.GetUserInfo()) {
						object obj = ReflectionUtilities.GetPropertyValue(c, sField);
						if (obj != null) {
							sValue = obj.ToString();
						}
					}
				}

				sFieldValue = string.Format(FieldFormat, sValue);

			} else {
				sFieldValue = string.Format(FieldFormat, DataBinder.Eval(container, "DataItem." + DataField.ToString()));

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
	public class ListItemRepeater : Repeater {


	}

	//========================================
	public class ListItemPlaceHolder : PlaceHolder {


		protected override void RenderChildren(HtmlTextWriter writer) {
			int indent1 = writer.Indent;

			foreach (Control c in this.Controls) {
				writer.Indent = indent1;
				if (c is ListItemRepeater) {
					writer.Write("\t");
					writer.Write(BaseServerControl.GetCtrlText(c).Replace("\r\n", "\r\n\t\t"));
					writer.WriteLine();
					writer.Write("\t\t");
				} else {
					c.RenderControl(writer);
				}
			}

			writer.Indent = indent1;
		}
	}

	//========================================
	public class DefaultListOpenNavTemplate : ITemplate {


		public DefaultListOpenNavTemplate() {

		}

		public void InstantiateIn(Control container) {

			Literal litL = new Literal();

			litL.Text = HtmlTextWriter.TagLeftChar + "ul" + HtmlTextWriter.TagRightChar;

			container.Controls.Add(litL);
		}

	}

	//========================================
	public class DefaultListCloseNavTemplate : ITemplate {

		public DefaultListCloseNavTemplate() {

		}

		public void InstantiateIn(Control container) {

			Literal litL = new Literal();
			litL.Text = HtmlTextWriter.EndTagLeftChars + "ul" + HtmlTextWriter.TagRightChar;

			container.Controls.Add(litL);
		}

	}

	//========================================
	public class DefaultLinkNavTemplate : ITemplate {

		public DefaultLinkNavTemplate() {

		}

		public void InstantiateIn(Control container) {

			ListItemPlaceHolder phAll = new ListItemPlaceHolder();

			NavLinkForTemplate lnk = new NavLinkForTemplate();
			lnk.LinkText = " LINK ";
			lnk.NavigateUrl = "#";

			ListItemWrapper wrap = new ListItemWrapper();
			wrap.Controls.Add(lnk);

			lnk.DataBinding += new EventHandler(lnkContent_DataBinding);

			phAll.Controls.Add(wrap);

			container.Controls.Add(phAll);
		}


		private void lnkContent_DataBinding(object sender, EventArgs e) {
			NavLinkForTemplate lnk = (NavLinkForTemplate)sender;
			RepeaterItem container = (RepeaterItem)lnk.NamingContainer;

			string sFileName = DataBinder.Eval(container, "DataItem.FileName").ToString();
			string sNavMenuText = DataBinder.Eval(container, "DataItem.NavMenuText").ToString();

			lnk.NavigateUrl = sFileName;
			lnk.LinkText = sNavMenuText;
		}
	}

	//========================================
	public class DefaultContentCommentFormThanks : ITemplate {

		public DefaultContentCommentFormThanks() {

		}

		public void InstantiateIn(Control container) {

			PlaceHolder ph = new PlaceHolder();
			ph.ID = "DefaultContentCommentFormThanks";

			ph.Controls.Add(new Literal { Text = "<p> Thank you for your comment </p>" });

			container.Controls.Add(ph);
		}
	}

	//========================================
	public class DefaultContentCommentEntryForm : ITemplate {

		public DefaultContentCommentEntryForm() {

		}

		public void InstantiateIn(Control container) {

			PlaceHolder ph = new PlaceHolder();
			ph.ID = "DefaultContentCommentForm";

			string sValidationScript = String.Empty;
			string sScriptPrefix = "Carrot" + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();

			Assembly _assembly = Assembly.GetExecutingAssembly();

			using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.UI.Controls.ContentCommentFormScripts.txt"))) {
				sValidationScript = oTextStream.ReadToEnd();
			}

			sValidationScript = sValidationScript.Replace("{CONTROL_PREFIX}", sScriptPrefix);

			string sVG = "ContentCommentForm";

			ph.Controls.Add(new Literal { Text = sValidationScript });

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
				ClientValidationFunction = sScriptPrefix + "_ValidateComments",
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

			container.Controls.Add(ph);
		}

	}


	//========================================
	public class DefaultPagerTemplate : ITemplate {

		public DefaultPagerTemplate() {

		}

		public void InstantiateIn(Control container) {
			Literal litL = new Literal();
			litL.Text = " [ ";
			Literal litR = new Literal();
			litR.Text = " ]   ";

			NavLinkForPagerTemplate lnkBtn = new NavLinkForPagerTemplate();
			lnkBtn.ID = "lnkBtn";
			lnkBtn.CSSSelected = "selected";

			lnkBtn.DataBinding += new EventHandler(lnkBtn_DataBinding);

			container.Controls.Add(litL);
			container.Controls.Add(lnkBtn);
			container.Controls.Add(litR);
		}

		private void lnkBtn_DataBinding(object sender, EventArgs e) {
			NavLinkForPagerTemplate lnkBtn = (NavLinkForPagerTemplate)sender;
			RepeaterItem container = (RepeaterItem)lnkBtn.NamingContainer;

			string sTxt = DataBinder.Eval(container, "DataItem").ToString();
			lnkBtn.LinkText = sTxt;
			lnkBtn.PageNumber = int.Parse(sTxt);
		}

	}

	//========================================
	public class DefaultSummaryTemplate : ITemplate {

		public DefaultSummaryTemplate() {

		}

		public void InstantiateIn(Control container) {

			Literal litContent0 = new Literal();
			litContent0.Text = "\r\n<div>\r\n<p>\r\n<b><a href='{0}'>{1}</a></b> <br />\r\n{2} <br />\r\nPosted On: {3} \r\n</p> \r\n<p>";
			litContent0.DataBinding += new EventHandler(litContent_DataBinding);

			Literal litContent1 = new Literal();
			litContent1.Text = " <br /> ";

			Literal litContent2 = new Literal();
			litContent2.Text = " </p> \r\n</div> \r\n";

			PostMetaWordList pc = new PostMetaWordList();
			pc.ContentType = PostMetaWordList.MetaDataType.Category;
			pc.DataBinding += new EventHandler(pmwlList_DataBinding);
			pc.MetaDataTitle = "Categories: ";
			pc.HtmlTagNameOuter = "span";
			pc.HtmlTagNameInner = "span";

			PostMetaWordList pt = new PostMetaWordList();
			pt.ContentType = PostMetaWordList.MetaDataType.Tag;
			pt.DataBinding += new EventHandler(pmwlList_DataBinding);
			pt.MetaDataTitle = "Tags: ";
			pt.HtmlTagNameOuter = "span";
			pt.HtmlTagNameInner = "span";

			container.Controls.Add(litContent0);
			container.Controls.Add(pc);
			container.Controls.Add(litContent1);
			container.Controls.Add(pt);
			container.Controls.Add(litContent2);
		}

		private void pmwlList_DataBinding(object sender, EventArgs e) {
			PostMetaWordList pmContent = (PostMetaWordList)sender;
			RepeaterItem container = (RepeaterItem)pmContent.NamingContainer;
			Guid guidSender = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());
			pmContent.AssignedRootContentID = guidSender;
		}

		private void litContent_DataBinding(object sender, EventArgs e) {
			Literal litContent = (Literal)sender;
			RepeaterItem container = (RepeaterItem)litContent.NamingContainer;
			string sTxt0 = litContent.Text;

			string sTxt1 = DataBinder.Eval(container, "DataItem.FileName").ToString();
			string sTxt2 = DataBinder.Eval(container, "DataItem.NavMenuText").ToString();
			string sTxt3 = DataBinder.Eval(container, "DataItem.PageTextPlainSummary").ToString();
			string sTxt4 = DataBinder.Eval(container, "DataItem.CreateDate").ToString();

			litContent.Text = String.Format(sTxt0, sTxt1, sTxt2, sTxt3, sTxt4);
		}
	}

	public class DefaultCommentTemplate : ITemplate {

		public DefaultCommentTemplate() {

		}

		public void InstantiateIn(Control container) {

			Literal litContent0 = new Literal();
			litContent0.Text = "\r\n<div>\r\n <p><b>{0} </b> <br>\r\n Commented On: {1} <br />\r\n{2} <br />\r\n </p> </div> \r\n";
			litContent0.DataBinding += new EventHandler(litContent_DataBinding);

			container.Controls.Add(litContent0);
		}

		private void litContent_DataBinding(object sender, EventArgs e) {
			Literal litContent = (Literal)sender;
			RepeaterItem container = (RepeaterItem)litContent.NamingContainer;
			string sTxt0 = litContent.Text;

			bool IsApproved = Convert.ToBoolean(DataBinder.Eval(container, "DataItem.IsApproved").ToString());

			string sTxt1 = DataBinder.Eval(container, "DataItem.CommenterName").ToString();
			string sTxt2 = DataBinder.Eval(container, "DataItem.CreateDate").ToString();
			string sTxt3 = DataBinder.Eval(container, "DataItem.PostCommentText").ToString();

			if (!IsApproved) {
				sTxt1 = String.Format("{0}{0} {1}", BaseServerControl.InactivePagePrefix, sTxt1);
			}

			litContent.Text = String.Format(sTxt0, sTxt1, sTxt2, sTxt3);
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
			IsApproved,
			IsSpam
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
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


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string Text {
			get {
				string s = (string)ViewState["Text"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["Text"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("{0}")]
		[Localizable(true)]
		public string FieldFormat {
			get {
				String s = ViewState["FieldFormat"] as String;
				return ((s == null) ? "{0}" : s);
			}
			set {
				ViewState["FieldFormat"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("NavMenuText")]
		[Localizable(true)]
		public CommentTextFieldName DataField {
			get {
				string s = (string)ViewState["DataField"];
				CommentTextFieldName c = CommentTextFieldName.CommenterName;
				if (!string.IsNullOrEmpty(s)) {
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


		protected override void Render(HtmlTextWriter output) {

			output.Write(this.Text);

		}


		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			bool IsApproved = Convert.ToBoolean(DataBinder.Eval(container, "DataItem.IsApproved").ToString());
			Guid pageID = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());

			this.ContentCommentID = pageID;

			string sTxt1 = string.Format(FieldFormat, DataBinder.Eval(container, "DataItem." + DataField.ToString()));
			if (!IsApproved) {
				sTxt1 = String.Format("{0} {1}", BaseServerControl.InactivePagePrefix, sTxt1);
			}
			this.Text = sTxt1;

			base.OnDataBinding(e);
		}


	}


	[DefaultProperty("Text")]
	[ToolboxData("<{0}:NavPageNumberDisplay runat=server></{0}:NavPageNumberDisplay>")]

	public class NavPageNumberDisplay : Control {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int PageNumber {
			get {
				int s = 0;
				try { s = int.Parse(ViewState["PageNumber"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["PageNumber"] = value.ToString();
			}
		}

		private Literal litPageNbr = new Literal();

		private void LoadCtrsl() {

			litPageNbr.Text = PageNumber.ToString();

			this.Controls.Clear();
			this.Controls.Add(litPageNbr);
		}

		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			int PageNbr = int.Parse(DataBinder.Eval(container, "DataItem").ToString());

			this.PageNumber = PageNbr;

			LoadCtrsl();

			base.OnDataBinding(e);
		}

	}


	//========================================

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:NavLinkForPagerTemplate runat=server></{0}:NavLinkForPagerTemplate>")]

	public class NavLinkForPagerTemplate : Control, IActivatePageNavItem {

		public string LinkText {
			get {
				return lnkBtn.Text;
			}

			set {
				lnkBtn.Text = value;
			}
		}

		public string ToolTip {
			get {
				return lnkBtn.ToolTip;
			}

			set {
				lnkBtn.ToolTip = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CssClassNormal {
			get {
				string s = (string)ViewState["CssClassNormal"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CssClassNormal"] = value;
				SetCSS();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
				SetCSS();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool IsSelected {
			get {
				bool s = false;
				if (ViewState["IsSelected"] != null) {
					try { s = (bool)ViewState["IsSelected"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["IsSelected"] = value;
				SetCSS();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		[Localizable(true)]
		public bool UseDefaultText {
			get {
				bool s = true;
				if (ViewState["UseDefaultText"] != null) {
					try { s = (bool)ViewState["UseDefaultText"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["UseDefaultText"] = value;
				SetCSS();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int PageNumber {
			get {
				int s = 0;
				try { s = int.Parse(ViewState["PageNumber"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["PageNumber"] = value.ToString();
			}
		}


		private void SetCSS() {

			if (!string.IsNullOrEmpty(CssClassNormal) || !string.IsNullOrEmpty(CSSSelected)) {
				string sCSS = "";
				string sSelCss = "";

				if (IsSelected) {
					sSelCss = CSSSelected.Trim();
				}

				sCSS = string.Format("{0} {1}", CssClassNormal.Trim(), sSelCss);

				lnkBtn.CssClass = sCSS.Trim();
			}
		}

		private string sBtnName = "lnkPagerBtn";

		private string _linkText = string.Empty;

		private LinkButton lnkBtn = new LinkButton();
		private ListItemPlaceHolder ph = new ListItemPlaceHolder();

		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			int PageNbr = int.Parse(DataBinder.Eval(container, "DataItem").ToString());

			lnkBtn.ID = sBtnName + PageNbr.ToString();
			lnkBtn.Click += new EventHandler(this.lnkBtn_Click);

			_linkText = PageNbr.ToString();

			this.PageNumber = PageNbr;

			LoadCtrsl();

			base.OnDataBinding(e);
		}

		protected void lnkBtn_Click(object sender, EventArgs e) {

		}

		protected override void OnPreRender(EventArgs e) {

			base.OnPreRender(e);

			AssignVals();
		}


		private void AssignVals() {

			SetCSS();

			if (!string.IsNullOrEmpty(_linkText) && string.IsNullOrEmpty(this.LinkText)) {
				if (UseDefaultText) {
					this.LinkText = _linkText;
				}
			}
		}


		private void LoadCtrsl() {

			int iMax = this.Controls.Count;
			lnkBtn.Controls.Clear();

			for (int i = 0; i < iMax; i++) {
				lnkBtn.Controls.Add(this.Controls[0]);
			}

			this.Controls.Add(lnkBtn);
			this.Controls.Add(ph);

		}


	}

	//========================================

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:ListItemWrapperForPager runat=server></{0}:ListItemWrapperForPager>")]

	public class ListItemWrapperForPager : Control, IActivatePageNavItem {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CssClassNormal {
			get {
				string s = (string)ViewState["CssClassNormal"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CssClassNormal"] = value;
				SetTag();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
				SetTag();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool IsSelected {
			get {
				bool s = false;
				if (ViewState["IsSelected"] != null) {
					try { s = (bool)ViewState["IsSelected"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["IsSelected"] = value;
				SetTag();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int PageNumber {
			get {
				int s = 0;
				try { s = int.Parse(ViewState["PageNumber"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["PageNumber"] = value.ToString();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string HtmlTagName {
			get {
				string s = (string)ViewState["HtmlTagName"];
				return ((s == null) ? "div" : s);
			}
			set {
				ViewState["HtmlTagName"] = value;
				SetTag();
			}
		}

		private Literal litOpen = new Literal();
		private Literal litClose = new Literal();
		private Control ctrlAll = new Control();

		private void LoadCtrsl() {
			int iMax = this.Controls.Count;

			SetTag();

			ctrlAll.Controls.Clear();
			ctrlAll.Controls.Add(litOpen);

			//instead of wind/unwind, pop stack X times
			for (int i = 0; i < iMax; i++) {
				ctrlAll.Controls.Add(this.Controls[0]);
			}

			ListItemPlaceHolder ph = new ListItemPlaceHolder();

			ctrlAll.Controls.Add(ph);
			ctrlAll.Controls.Add(litClose);

			this.Controls.Clear();
			this.Controls.Add(ctrlAll);

		}


		private void SetTag() {

			litOpen.Text = HtmlTextWriter.TagLeftChar + HtmlTagName + HtmlTextWriter.TagRightChar;
			litClose.Text = HtmlTextWriter.EndTagLeftChars + HtmlTagName + HtmlTextWriter.TagRightChar;

			if (!string.IsNullOrEmpty(CssClassNormal) || !string.IsNullOrEmpty(CSSSelected)) {
				string sCSS = "";
				string sSelCss = "";

				if (IsSelected) {
					sSelCss = CSSSelected.Trim();
				}

				if (!string.IsNullOrEmpty(CssClassNormal) || !string.IsNullOrEmpty(sSelCss)) {
					sCSS = string.Format(" class=\"{0} {1}\"", CssClassNormal.Trim(), sSelCss);
				}

				litOpen.Text = HtmlTextWriter.TagLeftChar + HtmlTagName + sCSS + HtmlTextWriter.TagRightChar;
			}
		}


		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			int PageNbr = int.Parse(DataBinder.Eval(container, "DataItem").ToString());

			this.PageNumber = PageNbr;

			SetTag();

			LoadCtrsl();

			base.OnDataBinding(e);
		}

	}



	//========================================
	public interface IActivateNavItem {

		string CSSSelected { get; set; }

		string CssClassNormal { get; set; }

		string CssClassHasChild { get; set; }

		bool IsSelected { get; set; }

		string NavigateUrl { get; set; }

		Guid ContentID { get; set; }

	}

	public interface IActivatePageNavItem {

		string CSSSelected { get; set; }

		string CssClassNormal { get; set; }

		bool IsSelected { get; set; }

		int PageNumber { get; set; }

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