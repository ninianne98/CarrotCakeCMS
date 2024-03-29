﻿using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
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

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			base.Render(writer);
		}

		private ControlUtilities cu = new ControlUtilities();

		protected override void OnPreRender(EventArgs e) {
			string sFieldValue = string.Empty;
			string sField = DataField.ToString();

			try {
				ContentPage cp = cu.GetContainerContentPage(this);

				if (cp != null) {
					if (sField.StartsWith("Author_") || sField.StartsWith("Credit_")) {
						ExtendedUserData usr = null;
						if (sField.StartsWith("Credit_")) {
							sField = DataField.ToString().Replace("Credit_", string.Empty);
							usr = cp.GetCreditUserInfo();
						}

						if (sField.StartsWith("Author_") || usr == null) {
							sField = DataField.ToString().Replace("Credit_", string.Empty).Replace("Author_", string.Empty);
							usr = cp.GetUserInfo();
						}

						if (usr == null) {
							usr = cp.BylineUser;
						}

						if (usr != null) {
							object objData = ReflectionUtilities.GetPropertyValue(usr, sField);
							if (objData != null) {
								sFieldValue = string.Format(FieldFormat, objData);
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
				if (!SiteData.IsWebView) {
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

		[Category("Appearance")]
		[DefaultValue("NavMenuText")]
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

		[Category("Appearance")]
		[DefaultValue("Unknown")]
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

		protected override void Render(HtmlTextWriter output) {
			this.EnsureChildControls();

			base.Render(output);
		}

		private ControlUtilities cu = new ControlUtilities();

		protected override void OnPreRender(EventArgs e) {
			string sFieldValue = string.Empty;

			ContentPage cp = cu.GetContainerContentPage(this);
			SiteNav navNext = new SiteNav();
			if (NavigationDirection != NavDirection.Unknown) {
				using (ISiteNavHelper navHelper = SiteNavFactory.GetSiteNavHelper()) {
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

		private ControlUtilities cu = new ControlUtilities();

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			ContentPage cp = cu.GetContainerContentPage(this);

			if (cp != null) {
				SetFileInfo(cp);
			}
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
				sFieldValue = string.Format(UrlPaths.ThumbnailPath + "?scale={0}&thumb={1}&square={2}", ScaleImage, HttpUtility.UrlEncode(sFieldValue), ThumbSize);
			}

			this.ImageUrl = sFieldValue;
		}
	}

	//========================================
	[Designer(typeof(GeneralControlDesigner))]
	[ToolboxData("<{0}:SiteCanonicalURL runat=server></{0}:SiteCanonicalURL>")]
	public class SiteCanonicalURL : Literal {

		[DefaultValue(false)]
		public bool Enable301Redirect {
			get {
				String s = (String)ViewState["Enable301Redirect"];
				bool b = ((s == null) ? false : Convert.ToBoolean(s));
				return b;
			}

			set {
				ViewState["Enable301Redirect"] = value.ToString();
			}
		}

		protected override void OnPreRender(EventArgs e) {
			string sFieldValue = string.Empty;

			SiteData sd = SiteData.CurrentSite;

			if (sd != null) {
				sFieldValue = sd.DefaultCanonicalURL;

				ControlUtilities cu = new ControlUtilities();
				ContentPage cp = cu.GetContainerContentPage(this);

				if (cp != null) {
					if (cp.NavOrder == 0) {
						sFieldValue = sd.MainCanonicalURL;
					} else {
						sFieldValue = sd.DefaultCanonicalURL;
					}
				}
			} else {
				sFieldValue = SiteData.DefaultDirectoryFilename;
			}

			this.Text = string.Format("<link rel=\"canonical\" href=\"{0}\" />\r\n", sFieldValue);

			if (this.Enable301Redirect) {
				HttpContext ctx = HttpContext.Current;

				if (!SiteData.CurrentSite.MainCanonicalURL.ToLowerInvariant().Contains(@"://" + CMSConfigHelper.DomainName.ToLowerInvariant())) {
					ctx.Response.Status = "301 Moved Permanently";
					ctx.Response.AddHeader("Location", sFieldValue);
				}
			}

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
		[DefaultValue("SiteName")]
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

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			base.Render(writer);
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