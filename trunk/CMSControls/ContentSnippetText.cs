using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
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
	[Designer(typeof(GeneralControlDesigner))]
	[ToolboxData("<{0}:ContentSnippetText runat=server></{0}:ContentSnippetText>")]
	public class ContentSnippetText : WidgetParmDataWebControl, IWidgetLimitedProperties, ITextControl {

		[Category("Appearance")]
		[DefaultValue("")]
		public string Text {
			get {
				String s = (String)ViewState["Text"];
				return ((s == null) ? String.Empty : s);
			}

			set {
				ViewState["Text"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public override bool EnableViewState {
			get {
				String s = (String)ViewState["EnableViewState"];
				bool b = ((s == null) ? false : Convert.ToBoolean(s));
				base.EnableViewState = b;
				return b;
			}

			set {
				ViewState["EnableViewState"] = value.ToString();
				base.EnableViewState = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string SnippetSlug {
			get {
				String s = ViewState["SnippetSlug"] as String;
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["SnippetSlug"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		[Description("Select a content snippet to display in the widget area")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstSnippetID")]
		public Guid SnippetID {
			get {
				String s = ViewState["SnippetID"] as String;
				return ((s == null) ? Guid.Empty : new Guid(s));
			}
			set {
				ViewState["SnippetID"] = value.ToString();
			}
		}

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstSnippetID {
			get {
				if (SiteID == Guid.Empty) {
					SiteID = SiteData.CurrentSiteID;
				}
				Dictionary<string, string> _dict = (from c in SiteData.CurrentSite.GetContentSnippetList()
													orderby c.ContentSnippetName
													where c.SiteID == SiteID
													select c).ToList().ToDictionary(k => k.Root_ContentSnippetID.ToString(),
													v => string.Format("{0} - {1} ({2})", v.ContentSnippetSlug, v.ContentSnippetName, (v.ContentSnippetActive ? "active" : "inactive")));
				return _dict;
			}
		}

		public List<string> LimitedPropertyList {
			get {
				List<string> lst = new List<string>();
				lst.Add("SnippetID");
				lst.Add("lstSnippetID");
				lst.Add("EnableViewState");
				return lst;
			}
		}


		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			RenderContents(writer);
		}

		protected override void RenderContents(HtmlTextWriter output) {

			string sBody = string.Empty;

			ContentSnippet cs = null;

			try {
				bool bIsEditor = SecurityData.IsAdmin || SecurityData.IsEditor;

				if (this.SnippetID != Guid.Empty) {
					cs = ContentSnippet.GetSnippetByID(SiteData.CurrentSiteID, this.SnippetID, !bIsEditor);
				} else {
					cs = ContentSnippet.GetSnippetBySlug(SiteData.CurrentSiteID, this.SnippetSlug, !bIsEditor);
				}

				string sBodyNote = string.Empty;
				string sIdent = String.Empty;

				if (cs != null) {

					if (bIsEditor && (cs.IsRetired || cs.IsUnReleased || !cs.ContentSnippetActive)) {
						string sBodyFlags = string.Empty;
						if (!cs.ContentSnippetActive) {
							sBodyFlags += CMSConfigHelper.InactivePagePrefix + " - Status : " + cs.ContentSnippetActive.ToString() + " ";
						}
						if (cs.IsRetired) {
							sBodyFlags += CMSConfigHelper.RetiredPagePrefix + " - Retired : " + cs.RetireDate.ToString() + " ";
						}
						if (cs.IsUnReleased) {
							sBodyFlags += CMSConfigHelper.UnreleasedPagePrefix + " - Unreleased : " + cs.GoLiveDate.ToString() + " ";
						}

						if (SecurityData.AdvancedEditMode) {
							sBodyNote = "<div class=\"cmsSnippetOuter\"> <div class=\"cmsSnippetInner\">\r\n" + cs.ContentSnippetSlug + ": " + sBodyFlags.Trim() + "\r\n<br style=\"clear: both;\" /></div></div>";
						} else {
							sBodyNote = "<div>\r\n" + cs.ContentSnippetSlug + ": " + sBodyFlags.Trim() + "\r\n<br style=\"clear: both;\" /></div>";
						}
					}

					if (SecurityData.AdvancedEditMode) {
						sIdent = "<div class=\"cmsSnippetOuter\"> <div class=\"cmsSnippetInner\">\r\n" + cs.ContentSnippetSlug + ": " + cs.ContentSnippetName + "\r\n<br style=\"clear: both;\" /></div></div>";
					}

					sBody = string.Format("{0}\r\n{1}\r\n{2}", sIdent, cs.ContentBody, sBodyNote);
				}

			} catch {
				if (!SiteData.IsWebView) {
					if (this.SnippetID != Guid.Empty) {
						sBody = this.SnippetID.ToString();
					} else {
						sBody = this.SnippetSlug;
					}
				}
			}

			this.Text = sBody;

			int indent = output.Indent;

			output.Indent = indent + 3;
			output.WriteLine();

			output.WriteLine();
			output.Write(this.Text);
			output.WriteLine();

			output.Indent = indent;
		}

		protected override void OnPreRender(EventArgs e) {

			base.OnPreRender(e);

			try {

				if (PublicParmValues.Count > 0) {

					//this.SnippetSlug = GetParmValue("SnippetSlug", "");
					this.SnippetID = new Guid(GetParmValue("SnippetID", Guid.Empty.ToString()));
					this.EnableViewState = Convert.ToBoolean(GetParmValue("EnableViewState", "false"));

				}
			} catch (Exception ex) {
			}
		}


	}

}