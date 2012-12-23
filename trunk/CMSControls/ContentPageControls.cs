using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
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


	/*
		<carrot:ContentPageImageThumb runat="server" ID="ContentPageImageThumb1" PerformURLResize="true" ScaleImage="true" ThumbSize="180" />
	 
		<p>
			By <carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="Author_FullName_FirstLast" />
			on <carrot:ContentPageProperty runat="server" ID="ContentPageProperty2" DataField="GoLiveDate" FieldFormat="{0:MMMM d, yyyy}" />
		</p>

		<h1 id="logo">
			<a href="/">
				<carrot:SiteDataProperty runat="server" ID="SiteDataProperty1" DataField="SiteName" /></a>
		</h1>
		<h2 id="slogan">
			<carrot:SiteDataProperty runat="server" ID="SiteDataProperty2" DataField="SiteTagline" />
		</h2>
	 */
	[ToolboxData("<{0}:ContentPageProperty runat=server></{0}:ContentPageProperty>")]
	public class ContentPageProperty : Literal {

		public enum ContentPageFieldName {
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
			CommentCount,
			NavOrder,
			EditUserId,
			EditDate,
			GoLiveDate,
			RetireDate,
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
		public ContentPageFieldName DataField {
			get {
				string s = (string)ViewState["DataField"];
				ContentPageFieldName c = ContentPageFieldName.NavMenuText;
				if (!string.IsNullOrEmpty(s)) {
					try {
						c = (ContentPageFieldName)Enum.Parse(typeof(ContentPageFieldName), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["DataField"] = value.ToString();
			}
		}

		protected override void Render(HtmlTextWriter output) {

			base.Render(output);

		}

		ControlUtilities cu = new ControlUtilities();

		protected override void OnPreRender(EventArgs e) {
			string sFieldValue = string.Empty;

			ContentPage cp = cu.GetContainerContentPage(this);

			if (cp != null) {
				string sField = DataField.ToString();
				if (sField.StartsWith("Author_")) {
					sField = DataField.ToString().Replace("Author_", "");

					using (ExtendedUserData usr = cp.GetUserInfo()) {
						if (usr != null) {
							object objData = ReflectionUtilities.GetPropertyValue(usr, sField);
							if (objData != null) {
								sFieldValue = string.Format(FieldFormat, objData);
							}
						}
					}
				} else {
					object objData = ReflectionUtilities.GetPropertyValue(cp, sField);
					if (objData != null) {
						sFieldValue = string.Format(FieldFormat, objData);
					}
				}

			}

			this.Text = sFieldValue;

			base.OnPreRender(e);
		}

	}

	//========================================

	[ToolboxData("<{0}:ListItemImageThumb runat=server></{0}:ListItemImageThumb>")]
	public class ContentPageImageThumb : Image {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
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

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
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

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
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

		protected override void Render(HtmlTextWriter output) {

			base.Render(output);

		}

		ControlUtilities cu = new ControlUtilities();

		protected override void OnPreRender(EventArgs e) {

			ContentPage cp = cu.GetContainerContentPage(this);

			if (cp != null) {
				SetFileInfo(cp.Thumbnail);
			}

			base.OnPreRender(e);
		}

		private void SetFileInfo(string sFieldValue) {
			if (string.IsNullOrEmpty(sFieldValue)) {
				this.Visible = false;
			}

			if (PerformURLResize && !string.IsNullOrEmpty(sFieldValue)) {
				sFieldValue = string.Format("/carrotwarethumb.axd?scale={0}&thumb={1}&square={2}", ScaleImage, HttpUtility.UrlEncode(sFieldValue), ThumbSize);
			}

			this.ImageUrl = sFieldValue;
		}

	}

	//========================================
	[ToolboxData("<{0}:SiteDataProperty runat=server></{0}:SiteDataProperty>")]
	public class SiteDataProperty : Literal {

		public enum SiteDataFieldName {
			SiteID,
			MetaKeyword,
			MetaDescription,
			SiteName,
			MainURL,
			BlockIndex,
			SiteTagline,
			SiteTitlebarPattern,
			Blog_Root_ContentID,
			Blog_FolderPath,
			Blog_CategoryPath,
			Blog_TagPath,
			Blog_DatePattern,
			TimeZone,
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
		public SiteDataFieldName DataField {
			get {
				string s = (string)ViewState["DataField"];
				SiteDataFieldName c = SiteDataFieldName.SiteName;
				if (!string.IsNullOrEmpty(s)) {
					try {
						c = (SiteDataFieldName)Enum.Parse(typeof(SiteDataFieldName), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["DataField"] = value.ToString();
			}
		}


		protected override void Render(HtmlTextWriter output) {

			base.Render(output);

		}

		ControlUtilities cu = new ControlUtilities();

		protected override void OnPreRender(EventArgs e) {
			string sFieldValue = string.Empty;

			SiteData sd = cu.GetContainerSiteData(this);

			if (sd != null) {
				string sField = DataField.ToString();

				object objData = ReflectionUtilities.GetPropertyValue(sd, sField);
				if (objData != null) {
					sFieldValue = string.Format(FieldFormat, objData);
				}
			}

			this.Text = sFieldValue;

			base.OnPreRender(e);
		}

	}

}