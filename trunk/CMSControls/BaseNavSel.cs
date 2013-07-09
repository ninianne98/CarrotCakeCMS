using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
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

	public abstract class BaseNavSel : BaseNavCommon {

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
		[DefaultValue("")]
		public string CSSItem {
			get {
				string s = (string)ViewState["CSSItem"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSItem"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("parent")]
		public string CSSULClassTop {
			get {
				string s = (string)ViewState["ULClassTop"];
				return ((s == null) ? "parent" : s);
			}
			set {
				ViewState["ULClassTop"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("children")]
		public string CSSULClassLower {
			get {
				string s = (string)ViewState["ULClassLower"];
				return ((s == null) ? "children" : s);
			}
			set {
				ViewState["ULClassLower"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("sub")]
		public string CSSHasChildren {
			get {
				string s = (string)ViewState["CSSHasChildren"];
				return ((s == null) ? "sub" : s);
			}
			set {
				ViewState["CSSHasChildren"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public virtual bool MultiLevel {
			get {
				return false;
			}
		}

		public virtual List<SiteNav> GetTopNav() {
			if (this.MultiLevel) {
				return this.NavigationData.Where(ct => ct.Parent_ContentID == null).OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
			} else {
				return this.NavigationData.OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
			}
		}

		public virtual List<SiteNav> GetChildren(Guid rootContentID) {
			return this.NavigationData.Where(ct => ct.Parent_ContentID == rootContentID).OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected SiteNav IsContained(List<SiteNav> navCrumbs, Guid rootContentID) {
			return navCrumbs.Where(ct => ct.Root_ContentID == rootContentID && ct.NavOrder > 0).FirstOrDefault();
		}

		protected override void WriteListPrefix(HtmlTextWriter output) {
			string sCSS = (this.CSSULClassTop + " " + this.CssClass).Trim();
			if (!string.IsNullOrEmpty(sCSS)) {
				output.WriteLine("<ul id=\"" + this.HtmlClientID + "\" class=\"" + sCSS + "\">");
			} else {
				output.WriteLine("<ul id=\"" + this.HtmlClientID + "\" >");
			}
		}

		protected override void WriteListSuffix(HtmlTextWriter output) {
			output.WriteLine("</ul>");
		}

		protected string ParentFileName { get; set; }

		private int iItemNumber = 0;

		protected virtual void WriteTopLevel(HtmlTextWriter output) {
			int indent = output.Indent;
			output.Indent = indent + 2;

			List<SiteNav> lstNav = GetTopNav();
			SiteNav parentPageNav = GetParentPage();
			List<SiteNav> lstNavTree = GetPageNavTree().OrderByDescending(x => x.NavOrder).ToList();

			this.ParentFileName = parentPageNav.FileName.ToLower();

			if (lstNav != null && lstNav.Count > 0) {

				output.WriteLine();
				WriteListPrefix(output);

				int indent2 = output.Indent + 1;

				string sItemCSS = "";
				if (!string.IsNullOrEmpty(CSSItem)) {
					sItemCSS = string.Format(" {0} ", this.CSSItem);
				}

				string sThis1CSS = sItemCSS;

				foreach (SiteNav c1 in lstNav) {
					output.Indent = indent2;
					List<SiteNav> cc = GetChildren(c1.Root_ContentID);

					string sChild = " ";
					if (this.MultiLevel) {
						if (cc != null && cc.Count > 0) {
							sChild = " level1-haschildren " + this.CSSHasChildren + " ";
						}
						sThis1CSS = " level1 " + sItemCSS + sChild;
					} else {
						sThis1CSS = sItemCSS;
					}
					if (SiteData.IsFilenameCurrentPage(c1.FileName) || (IsContained(lstNavTree, c1.Root_ContentID) != null) || AreFilenamesSame(c1.FileName, this.ParentFileName)) {
						sThis1CSS = sThis1CSS + " " + this.CSSSelected;
					}
					if (lstNav.Where(x => x.NavOrder < 0).Count() > 0) {
						if (c1.NavOrder < 0) {
							sThis1CSS = sThis1CSS + " parent-nav";
						} else {
							sThis1CSS = sThis1CSS + " child-nav";
						}
					}
					sThis1CSS = sThis1CSS.Replace("   ", " ").Replace("  ", " ").Trim();

					iItemNumber++;
					output.WriteLine("<li id=\"listitem" + iItemNumber.ToString() + "\" class=\"" + sThis1CSS + "\"><a href=\"" + c1.FileName + "\">" + c1.NavMenuText + "</a>");

					int indent3 = output.Indent;
					if (this.MultiLevel && cc != null && cc.Count > 0) {
						LoadChildren(output, c1.Root_ContentID, sItemCSS, iItemNumber, 2);
					}
					output.Indent = indent3;
					output.WriteLine("</li>");
					output.WriteLine();
				}
				WriteListSuffix(output);
			} else {
				output.WriteLine("<span style=\"display: none;\" id=\"" + this.ClientID + "\"></span>");
			}

			output.Indent = indent;
		}

		protected virtual void LoadChildren(HtmlTextWriter output, Guid rootContentID, string sItemCSS, int iParent, int iLevel) {
			List<SiteNav> lstNav = GetChildren(rootContentID);
			int indent = output.Indent;
			output.Indent = indent + 1;

			string sThis2CSS = sItemCSS;

			if (lstNav != null && lstNav.Count > 0) {
				output.WriteLine();
				output.WriteLine("<ul id=\"listitem" + iParent.ToString() + "-childlist\" class=\"childlist childlevel" + iLevel + " " + this.CSSULClassLower + "\">");
				int indent2 = output.Indent + 1;
				foreach (SiteNav c2 in lstNav) {
					output.Indent = indent2;
					List<SiteNav> cc = GetChildren(c2.Root_ContentID);

					if (this.MultiLevel) {
						string sChild = " ";
						if (cc != null && cc.Count > 0) {
							sChild = " level" + iLevel + "-haschildren " + this.CSSHasChildren + " ";
						}
						sThis2CSS = " level" + iLevel + " " + sItemCSS + sChild;
					} else {
						sThis2CSS = sItemCSS;
					}

					if (SiteData.IsFilenameCurrentPage(c2.FileName) || AreFilenamesSame(c2.FileName, this.ParentFileName)) {
						sThis2CSS = sThis2CSS + " " + this.CSSSelected;
					}
					sThis2CSS = (sThis2CSS + " child-nav").Replace("   ", " ").Replace("  ", " ").Trim();

					iItemNumber++;
					output.WriteLine("<li id=\"listitem" + iItemNumber.ToString() + "\" class=\"" + sThis2CSS + "\"><a href=\"" + c2.FileName + "\">" + c2.NavMenuText + "</a>");
					int indent3 = output.Indent;
					if (cc != null && cc.Count > 0) {
						LoadChildren(output, c2.Root_ContentID, sItemCSS, iItemNumber, iLevel + 1);
					}
					output.Indent = indent3;
					output.Write("</li>");

					output.WriteLine();
				}
				output.Indent--;
				output.WriteLine("</ul> ");
			}

			output.Indent = indent;
		}

		protected override void RenderContents(HtmlTextWriter output) {
			LoadAndTweakData();

			WriteTopLevel(output);
		}

		protected override void OnPreRender(EventArgs e) {
			try {

				base.OnPreRender(e);

				if (PublicParmValues.Count > 0) {

					string sTmp = "";

					sTmp = GetParmValue("CSSSelected", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.CSSSelected = sTmp;
					}

					sTmp = GetParmValue("CSSHasChildren", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.CSSHasChildren = sTmp;
					}

					sTmp = GetParmValue("CSSULClassTop", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.CSSULClassTop = sTmp;
					}

					sTmp = GetParmValue("CSSULClassLower", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.CSSULClassLower = sTmp;
					}

					sTmp = GetParmValue("CSSHasChildren", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.CSSHasChildren = sTmp;
					}
				}
			} catch (Exception ex) {
			}

		}

	}
}
