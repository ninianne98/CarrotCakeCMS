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

	[Designer(typeof(GeneralControlDesigner))]
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
			string sField = DataField.ToString();

			try {
				ContentPage cp = cu.GetContainerContentPage(this);

				if (cp != null) {

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
			} catch {
				if (HttpContext.Current == null) {
					sFieldValue = sField;
				}
			}

			this.Text = sFieldValue;

			base.OnPreRender(e);
		}

	}

	//========================================
	[ToolboxData("<{0}:ContentPageNext runat=server></{0}:ContentPageNext>")]
	public class ContentPageNext : HyperLink {

		public enum CaptionSource {
			TitleBar,
			NavMenuText,
			PageHead,
		}

		public enum NavDirection {
			Unknown,
			Prev,
			Next,
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("NavMenuText")]
		[Localizable(true)]
		public CaptionSource CaptionDataField {
			get {
				string s = (string)ViewState["CaptionDataField"];
				CaptionSource c = CaptionSource.NavMenuText;
				if (!string.IsNullOrEmpty(s)) {
					try {
						c = (CaptionSource)Enum.Parse(typeof(CaptionSource), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["CaptionDataField"] = value.ToString();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("Unknown")]
		[Localizable(true)]
		public NavDirection NavigationDirection {
			get {
				string s = (string)ViewState["NavigationDirection"];
				NavDirection c = NavDirection.Unknown;
				if (!string.IsNullOrEmpty(s)) {
					try {
						c = (NavDirection)Enum.Parse(typeof(NavDirection), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["NavigationDirection"] = value.ToString();
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
			}
		}


		protected override void Render(HtmlTextWriter output) {

			base.Render(output);

		}

		private ControlUtilities cu = new ControlUtilities();


		protected override void OnPreRender(EventArgs e) {
			string sFieldValue = string.Empty;

			ContentPage cp = cu.GetContainerContentPage(this);
			SiteNav navNext = new SiteNav();
			if (NavigationDirection != NavDirection.Unknown) {
				using (SiteNavHelper navHelper = new SiteNavHelper()) {

					if (NavigationDirection == NavDirection.Prev) {
						navNext = navHelper.GetPrevPost(SiteData.CurrentSiteID, cp.Root_ContentID, !SecurityData.IsAuthEditor);
					}
					if (NavigationDirection == NavDirection.Next) {
						navNext = navHelper.GetNextPost(SiteData.CurrentSiteID, cp.Root_ContentID, !SecurityData.IsAuthEditor);
					}

					if (navNext != null) {
						if (UseDefaultText) {
							string sField = this.CaptionDataField.ToString();

							object objData = ReflectionUtilities.GetPropertyValue(navNext, sField);
							if (objData != null) {
								sFieldValue = string.Format("{0}", objData);
							}

							this.Text = sFieldValue;
						}

						this.NavigateUrl = navNext.FileName;
					} else {
						this.Visible = false;
					}
				}
			} else {
				this.Visible = false;
			}

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


		ControlUtilities cu = new ControlUtilities();

		protected override void Render(HtmlTextWriter w) {

			ContentPage cp = cu.GetContainerContentPage(this);

			if (cp != null) {
				SetFileInfo(cp);
			}

			base.Render(w);
		}

		private void SetFileInfo(ContentPage cp) {
			string sFieldValue = cp.Thumbnail;

			if (string.IsNullOrEmpty(sFieldValue)) {
				// if page itself has no image, see if the image had been specified directly
				sFieldValue = this.ImageUrl;
			}
			if (string.IsNullOrEmpty(sFieldValue)) {
				this.Visible = false;
			}
			if (!string.IsNullOrEmpty(sFieldValue) && !sFieldValue.StartsWith("/")) {
				sFieldValue = cp.TemplateFolderPath + sFieldValue;
			}

			if (PerformURLResize && !string.IsNullOrEmpty(sFieldValue)) {
				sFieldValue = string.Format("/carrotwarethumb.axd?scale={0}&thumb={1}&square={2}", ScaleImage, HttpUtility.UrlEncode(sFieldValue), ThumbSize);
			}

			this.ImageUrl = sFieldValue;
		}

	}

	//========================================
	[Designer(typeof(GeneralControlDesigner))]
	[ToolboxData("<{0}:SiteCanonicalURL runat=server></{0}:SiteCanonicalURL>")]
	public class SiteCanonicalURL : Literal {

		protected override void OnPreRender(EventArgs e) {
			string sFieldValue = string.Empty;

			SiteData sd = SiteData.CurrentSite;

			if (sd != null) {
				sFieldValue = sd.DefaultCanonicalURL;
			} else {
				sFieldValue = SiteData.DefaultDirectoryFilename;
			}

			this.Text = string.Format("<link rel=\"canonical\" href=\"{0}\" />\r\n", sFieldValue);

			base.OnPreRender(e);
		}
	}

	//========================================
	[Designer(typeof(GeneralControlDesigner))]
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

		protected override void OnPreRender(EventArgs e) {
			string sFieldValue = string.Empty;

			SiteData sd = SiteData.CurrentSite;

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