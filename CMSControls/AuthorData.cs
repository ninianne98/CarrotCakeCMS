using System;
using System.ComponentModel;
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
				usr = cp.BylineUser;
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

				usr = content.BylineUser;

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

		public enum AuthorSource {
			Default,
			Editor,
			Created,
			Credited
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
				if (!String.IsNullOrEmpty(s)) {
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

		[Category("Appearance")]
		[DefaultValue("Editor")]
		public AuthorSource SourceField {
			get {
				string s = (string)ViewState["SourceField"];
				AuthorSource c = AuthorSource.Editor;
				if (!String.IsNullOrEmpty(s)) {
					try {
						c = (AuthorSource)Enum.Parse(typeof(AuthorSource), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["SourceField"] = value.ToString();
			}
		}

		private ControlUtilities cu = new ControlUtilities();
		private ExtendedUserData _usr = null;

		protected override void OnPreRender(EventArgs e) {
			if (_usr == null) {
				_usr = ExtendedUserData.GetEditorFromURL();
			}
			if (_usr == null) {
				ContentPage cp = cu.GetContainerContentPage(this);

				switch (this.SourceField) {
					case AuthorSource.Editor:
						_usr = cp.GetUserInfo();
						break;

					case AuthorSource.Created:
						_usr = cp.GetCreateUserInfo();
						break;

					case AuthorSource.Credited:
						_usr = cp.GetCreditUserInfo();
						break;

					default:
						_usr = cp.BylineUser;
						break;
				}
			}

			AssignUser();

			base.OnPreRender(e);
		}

		private void AssignUser() {
			string sFieldValue = String.Empty;

			if (_usr != null) {
				object objData = ReflectionUtilities.GetPropertyValue(_usr, DataField.ToString());
				if (objData != null) {
					sFieldValue = String.Format(FieldFormat, objData);
				}

				this.Text = sFieldValue;
			}
		}

		protected override void OnDataBinding(EventArgs e) {
			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			object contentData = DataBinder.GetDataItem(container);

			if (contentData is ISiteContent) {
				ISiteContent content = (ISiteContent)(contentData);

				_usr = content.BylineUser;

				AssignUser();
			}

			base.OnDataBinding(e);
		}
	}
}