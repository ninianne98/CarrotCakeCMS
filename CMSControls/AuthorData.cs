using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using System.ComponentModel;

namespace Carrotware.CMS.UI.Controls {
	//================================================
	[ToolboxData("<{0}:AuthorLink runat=server></{0}:AuthorLink>")]
	public class AuthorLink : HyperLink, INamingContainer {

		[Category("Appearance")]
		[DefaultValue(true)]
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
			}
		}

		private ControlUtilities cu = new ControlUtilities();
		private ExtendedUserData usr = null;

		protected override void OnPreRender(EventArgs e) {

			if (usr == null) {
				usr = ExtendedUserData.GetEditorFromURL();
			}
			if (usr == null) {
				ContentPage cp = cu.GetContainerContentPage(this);
				usr = cp.GetUserInfo();
			}

			AssignUser();

			base.OnPreRender(e);
		}

		private void AssignUser() {
			if (usr != null) {
				if (this.UseDefaultText) {
					this.Controls.Clear();
					this.Text = usr.ToString();
				}
				this.NavigateUrl = usr.EditorURL;
			}
		}

		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			object contentData = DataBinder.GetDataItem(container);

			if (contentData is ISiteContent) {
				ISiteContent content = (ISiteContent)(contentData);

				usr = content.GetUserInfo();

				AssignUser();
			}

			base.OnDataBinding(e);
		}

	}


	//================================================
	[ToolboxData("<{0}:AuthorTextLabel runat=server></{0}:AuthorTextLabel>")]
	public class AuthorTextLabel : Literal, INamingContainer {

		public enum AuthorTextFieldName {
			UserName,
			EditorURL,
			UserBio,
			EmailAddress,
			UserNickName,
			FirstName,
			LastName,
			FullName_FirstLast,
			FullName_LastFirst,
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
		[DefaultValue("FullName_FirstLast")]
		public AuthorTextFieldName DataField {
			get {
				string s = (string)ViewState["DataField"];
				AuthorTextFieldName c = AuthorTextFieldName.FullName_FirstLast;
				if (!string.IsNullOrEmpty(s)) {
					try {
						c = (AuthorTextFieldName)Enum.Parse(typeof(AuthorTextFieldName), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["DataField"] = value.ToString();
			}
		}

		private ControlUtilities cu = new ControlUtilities();
		private ExtendedUserData usr = null;

		protected override void OnPreRender(EventArgs e) {


			if (usr == null) {
				usr = ExtendedUserData.GetEditorFromURL();
			}
			if (usr == null) {
				ContentPage cp = cu.GetContainerContentPage(this);
				usr = cp.GetUserInfo();
			}

			AssignUser();

			base.OnPreRender(e);
		}

		private void AssignUser() {
			string sFieldValue = string.Empty;

			if (usr != null) {
				object objData = ReflectionUtilities.GetPropertyValue(usr, DataField.ToString());
				if (objData != null) {
					sFieldValue = string.Format(FieldFormat, objData);
				}

				this.Text = sFieldValue;
			}
		}

		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			object contentData = DataBinder.GetDataItem(container);

			if (contentData is ISiteContent) {
				ISiteContent content = (ISiteContent)(contentData);

				usr = content.GetUserInfo();

				AssignUser();
			}

			base.OnDataBinding(e);
		}

	}


}