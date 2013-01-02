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

	[ToolboxData("<{0}:MultiLevelNavigation runat=server></{0}:MultiLevelNavigation>")]
	public class MultiLevelNavigation : BaseServerControl {

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

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int LevelDepth {
			get {
				int s = 3;
				try { s = int.Parse(ViewState["LevelDepth"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["LevelDepth"] = value.ToString();
			}
		}


		private List<SiteNav> lstDeepLevelNav = new List<SiteNav>();


		protected List<SiteNav> GetTopNav() {
			return lstDeepLevelNav.Where(ct => ct.Parent_ContentID == null).OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected List<SiteNav> GetChildren(Guid rootContentID) {
			return lstDeepLevelNav.Where(ct => ct.Parent_ContentID == rootContentID).OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected SiteNav GetPageInfo(string sPath) {
			return lstDeepLevelNav.Where(ct => ct.FileName.ToLower() == sPath.ToLower()).FirstOrDefault();
		}

		protected void LoadData() {
			lstDeepLevelNav = navHelper.GetLevelDepthNavigation(SiteData.CurrentSiteID, LevelDepth, !SecurityData.IsAuthEditor);
		}


		protected override void OnInit(EventArgs e) {
			this.Controls.Clear();

			base.OnInit(e);

			LoadData();
		}


		private void LoadChildren(HtmlTextWriter output, Guid rootContentID, int iLevel) {
			List<SiteNav> cc = GetChildren(rootContentID);
			int indent = output.Indent;
			output.Indent = indent + 1;

			if (cc != null && cc.Count > 0) {
				output.WriteLine();
				output.WriteLine("<ul class=\"children level-" + iLevel + "\">");
				int indent2 = output.Indent + 1;
				foreach (SiteNav c1 in cc) {
					output.Indent = indent2;
					IdentifyLinkAsInactive(c1);
					if (SiteData.IsFilenameCurrentPage(c1.FileName) || AreFilenamesSame(c1.FileName, ParentFileName)) {
						output.Write("<li class=\"" + CSSSelected + " level-" + iLevel + "\">");
					} else {
						output.Write("<li class=\"level-" + iLevel + "\">");
					}
					output.Write(" <a href=\"" + c1.FileName + "\">" + c1.NavMenuText + "</a> ");
					int indent3 = output.Indent;
					LoadChildren(output, c1.Root_ContentID, iLevel + 1);
					output.Indent = indent3;
					output.Write("</li>");
					output.WriteLine();
				}
				output.Indent--;
				output.WriteLine("</ul> ");
			}

			output.Indent = indent;
		}

		private string ParentFileName = "";

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;
			output.Indent = indent + 2;

			List<SiteNav> lst = GetTopNav();
			SiteNav pageNav = GetParentPage();
			ParentFileName = pageNav.FileName.ToLower();

			output.WriteLine();
			output.WriteLine("<div name=\"" + this.UniqueID + "\" id=\"" + this.ClientID + "\">");
			output.Indent++;
			output.WriteLine("<div id=\"" + this.ClientID + "-inner\">");
			output.Indent++;
			output.WriteLine("<ul class=\"parent\">");

			int indent2 = output.Indent + 1;

			foreach (SiteNav c1 in lst) {
				output.Indent = indent2;
				IdentifyLinkAsInactive(c1);
				if (SiteData.IsFilenameCurrentPage(c1.FileName) || AreFilenamesSame(c1.FileName, ParentFileName)) {
					output.Write("<li class=\"level-0 " + CSSSelected + "\">");
				} else {
					output.Write("<li class=\"level-0\">");
				}

				output.Write(" <a href=\"" + c1.FileName + "\">" + c1.NavMenuText + "</a> ");
				int indent3 = output.Indent;
				LoadChildren(output, c1.Root_ContentID, 1);
				output.Indent = indent3;
				output.Write("</li>");
				output.WriteLine();
			}
			output.Indent--;
			output.WriteLine("</ul>");
			output.Indent--;
			output.WriteLine("</div>");
			output.Indent--;
			output.WriteLine("</div>");

			output.Indent = indent;
		}

	}
}
