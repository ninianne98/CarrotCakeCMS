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

	[ToolboxData("<{0}:TopLevelNavigation runat=server></{0}:TopLevelNavigation>")]
	public class TopLevelNavigation : BaseServerControl {

		[Category("Appearance")]
		[DefaultValue("selected")]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "selected" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
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
		[DefaultValue(false)]
		public bool RenderHTMLWithID {
			get {
				String s = (String)ViewState["RenderHTMLWithID"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}

			set {
				ViewState["RenderHTMLWithID"] = value.ToString();
			}
		}

		public string HtmlClientID {
			get {
				if (RenderHTMLWithID) {
					return this.ID;
				} else {
					return this.ClientID;
				}
			}
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			SiteNav pageNav = GetParentPage();
			string sParent = pageNav.FileName.ToLower();

			List<SiteNav> lstNav = navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);
			lstNav.RemoveAll(x => x.ShowInSiteNav == false);
			lstNav.ToList().ForEach(q => IdentifyLinkAsInactive(q));

			output.Indent = indent + 3;
			output.WriteLine();

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			output.WriteLine("<ul" + sCSS + " id=\"" + this.HtmlClientID + "\">");
			output.Indent++;

			foreach (SiteNav c in lstNav) {
				if (SiteData.IsFilenameCurrentPage(c.FileName) || AreFilenamesSame(c.FileName, sParent)) {
					output.WriteLine("<li class=\"" + CSSSelected + "\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
				} else {
					output.WriteLine("<li><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
				}
			}

			output.Indent--;
			output.WriteLine("</ul>");

			output.Indent = indent;
		}

	}
}
