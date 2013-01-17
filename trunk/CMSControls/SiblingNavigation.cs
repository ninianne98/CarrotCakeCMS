using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
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

	[ToolboxData("<{0}:SiblingNavigation runat=server></{0}:SiblingNavigation>")]
	public class SiblingNavigation : BaseServerControl, IHeadedList {

		public int ItemCount { get; set; }

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "selected" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
			}
		}


		[Obsolete("This property is obsolete, do not use.")]
		public string SectionTitle {
			get {
				string s = (string)ViewState["SectionTitle"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["SectionTitle"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string MetaDataTitle {
			get {
				string s = (string)ViewState["MetaDataTitle"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["MetaDataTitle"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		[Localizable(true)]
		public TagType HeadWrapTag {
			get {
				String s = (String)ViewState["HeadWrapTag"];
				TagType c = TagType.H2;
				if (!string.IsNullOrEmpty(s)) {
					c = (TagType)Enum.Parse(typeof(TagType), s, true);
				}
				return c;
			}

			set {
				ViewState["HeadWrapTag"] = value.ToString();
			}
		}


		[DefaultValue(false)]
		[Themeable(false)]
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

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;
			List<SiteNav> lstNav = navHelper.GetSiblingNavigation(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName, !SecurityData.IsAuthEditor);
			lstNav.RemoveAll(x => x.ShowInSiteNav == false);
			lstNav.ToList().ForEach(q => IdentifyLinkAsInactive(q));

			if (lstNav != null) {
				this.ItemCount = lstNav.Count;
			}

			output.Indent = indent + 3;
			output.WriteLine();

			if (lstNav != null && lstNav.Count > 0 && !string.IsNullOrEmpty(this.MetaDataTitle)) {
				output.WriteLine("<" + this.HeadWrapTag.ToString().ToLower() + ">" + this.MetaDataTitle + "</" + this.HeadWrapTag.ToString().ToLower() + ">\r\n");
			}

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			if (lstNav != null && lstNav.Count > 0) {
				output.WriteLine("<ul" + sCSS + " id=\"" + this.ClientID + "\">");
				output.Indent++;
				foreach (SiteNav c in lstNav) {
					if (SiteData.IsFilenameCurrentPage(c.FileName)) {
						output.WriteLine("<li class=\"" + CSSSelected + "\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
					} else {
						output.WriteLine("<li><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
					}
				}
				output.Indent--;
				output.WriteLine("</ul>");
			} else {
				output.WriteLine("<span style=\"display: none;\" id=\"" + this.ClientID + "\"></span>");
			}

			output.Indent = indent;
		}

	}
}
