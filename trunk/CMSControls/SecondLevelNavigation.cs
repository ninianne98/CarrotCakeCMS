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

	[ToolboxData("<{0}:SecondLevelNavigation runat=server></{0}:SecondLevelNavigation>")]
	public class SecondLevelNavigation : BaseServerControl, IHeadedList {

		public bool IncludeParent { get; set; }

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

		protected List<SiteNav> GetSubNav() {
			return navHelper.GetSiblingNavigation(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName, !SecurityData.IsAuthEditor);
		}

		protected SiteNav GetParent(Guid? Root_ContentID) {
			SiteNav pageNav = null;
			if (Root_ContentID != null) {
				pageNav = navHelper.GetPageNavigation(SiteData.CurrentSiteID, Root_ContentID.Value);
			}
			return pageNav;
		}


		protected override void RenderContents(HtmlTextWriter output) {
			var lstNav = GetSubNav();

			if (lstNav != null && lstNav.Count > 0 && !string.IsNullOrEmpty(this.MetaDataTitle)) {
				output.WriteLine("<" + this.HeadWrapTag.ToString().ToLower() + ">" + this.MetaDataTitle + "</" + this.HeadWrapTag.ToString().ToLower() + ">\r\n");
			}

			if (lstNav != null && lstNav.Count > 0) {

				output.Write("<ul id=\"" + this.ClientID + "\">");

				if (IncludeParent) {
					if (lstNav != null && lstNav.Count > 0) {
						var p = GetParent(lstNav.OrderByDescending(x => x.Parent_ContentID).FirstOrDefault().Parent_ContentID);
						IdentifyLinkAsInactive(p);
						if (p != null) {
							output.Write("<li class=\"parent-nav\"><a href=\"" + p.FileName + "\">" + p.NavMenuText + "</a></li>\r\n");
						}
					}
				}

				foreach (var c in lstNav) {
					IdentifyLinkAsInactive(c);
					if (SiteData.IsFilenameCurrentPage(c.FileName)) {
						output.Write("<li class=\"selected\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li>\r\n");
					} else {
						output.Write("<li><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li>\r\n");
					}
				}
				output.Write("</ul>");
			} else {
				output.WriteLine("<span style=\"display: none;\" id=\"" + this.ClientID + "\"></span>");
			}
		}

	}
}
