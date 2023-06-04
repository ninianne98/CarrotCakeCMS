using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

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

	public abstract class BaseNavSel : BaseNavCommon, IWidgetLimitedProperties {

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

		public virtual List<string> LimitedPropertyList {
			get {
				List<string> lst = new List<string>();
				lst.Add("CssClass");

				lst.Add("CSSSelected");
				lst.Add("CSSItem");
				lst.Add("CSSULClassTop");
				lst.Add("CSSULClassLower");
				lst.Add("CSSHasChildren");
				return lst;
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
			_topTag = new HtmlTag("ul");

			_topTag.SetAttribute("id", this.HtmlClientID);
			_topTag.MergeAttribute("class", this.CSSULClassTop);
			_topTag.MergeAttribute("class", this.CssClass);

			output.WriteLine(_topTag.OpenTag());
		}

		protected override void WriteListSuffix(HtmlTextWriter output) {
			output.WriteLine(_topTag.CloseTag());
		}

		protected string ParentFileName { get; set; }

		private int _itemNumber = 0;

		protected virtual void WriteTopLevel(HtmlTextWriter output) {
			int indent = output.Indent;
			output.Indent = indent + 2;

			List<SiteNav> lstNav = GetTopNav();
			SiteNav parentPageNav = GetParentPage();
			List<SiteNav> lstNavTree = GetPageNavTree().OrderByDescending(x => x.NavOrder).ToList();

			if (parentPageNav != null && parentPageNav.FileName != null) {
				this.ParentFileName = parentPageNav.FileName.ToLowerInvariant();
			}

			if (lstNav != null && lstNav.Any()) {
				output.WriteLine();
				WriteListPrefix(output);

				int indent2 = output.Indent + 1;

				foreach (SiteNav c1 in lstNav) {
					var item = new HtmlTag("li");
					var link = new HtmlTag("a");

					item.MergeAttribute("class", this.CSSItem);

					link.Uri = c1.FileName;
					link.InnerHtml = c1.NavMenuText;

					output.Indent = indent2;
					List<SiteNav> cc = GetChildren(c1.Root_ContentID);

					if (this.MultiLevel) {
						item.MergeAttribute("class", "level1");

						if (cc != null && cc.Any()) {
							item.MergeAttribute("class", string.Format(" level1-haschildren {0}", this.CSSHasChildren));
						}
					}

					if (SiteData.IsFilenameCurrentPage(c1.FileName) || (IsContained(lstNavTree, c1.Root_ContentID) != null)
								|| AreFilenamesSame(c1.FileName, this.ParentFileName)) {
						item.MergeAttribute("class", this.CSSSelected);
					}

					_itemNumber++;
					item.SetAttribute("id", string.Format("listitem{0}", _itemNumber));

					output.Write(item.OpenTag());

					link.Uri = c1.FileName;
					link.InnerHtml = c1.NavMenuText;

					output.WriteLine(link.RenderTag());

					int indent3 = output.Indent;
					if (this.MultiLevel && cc != null && cc.Any()) {
						LoadChildren(output, c1.Root_ContentID, _itemNumber, 2);
					}
					output.Indent = indent3;

					output.Write(item.CloseTag());
					output.WriteLine();
				}
				WriteListSuffix(output);
			} else {
#if DEBUG
				output.WriteLine("<span style=\"display: none;\" id=\"" + this.HtmlClientID + "\"></span>");
#endif
			}

			output.Indent = indent;
		}

		protected virtual void LoadChildren(HtmlTextWriter output, Guid rootContentID, int iParent, int iLevel) {
			List<SiteNav> lstNav = GetChildren(rootContentID);
			int indent = output.Indent;
			output.Indent = indent + 1;

			if (lstNav != null && lstNav.Any()) {
				var childList = new HtmlTag("ul");

				childList.SetAttribute("id", string.Format("listitem{0}-childlist", iParent));
				childList.MergeAttribute("class", string.Format("childlist childlevel{0}", iLevel));
				childList.MergeAttribute("class", this.CSSULClassLower);

				output.Write(childList.OpenTag());
				output.WriteLine();

				int indent2 = output.Indent + 1;
				foreach (SiteNav c2 in lstNav) {
					output.Indent = indent2;
					List<SiteNav> cc = GetChildren(c2.Root_ContentID);
					var childItem = new HtmlTag("li");
					var childLink = new HtmlTag("a");

					if (this.MultiLevel) {
						childItem.MergeAttribute("class", string.Format("level{0}", iLevel));

						if (cc != null && cc.Any()) {
							childItem.MergeAttribute("class", string.Format("level{0}-haschildren {1}", iLevel, this.CSSHasChildren));
						}
					}

					if (SiteData.IsFilenameCurrentPage(c2.FileName) || AreFilenamesSame(c2.FileName, this.ParentFileName)) {
						childItem.MergeAttribute("class", this.CSSSelected);
					}

					_itemNumber++;
					childItem.SetAttribute("id", string.Format("listitem{0}", _itemNumber));
					childItem.MergeAttribute("class", "child-nav");
					output.Write(childItem.OpenTag());

					childLink.Uri = c2.FileName;
					childLink.InnerHtml = c2.NavMenuText;
					output.WriteLine(childLink.RenderTag());

					int indent3 = output.Indent;
					if (cc != null && cc.Any()) {
						LoadChildren(output, c2.Root_ContentID, _itemNumber, iLevel + 1);
					}
					output.Indent = indent3;

					output.Write(childItem.CloseTag());
					output.WriteLine();
				}
				output.Indent--;
				output.WriteLine(childList.CloseTag());
			}

			output.Indent = indent;
		}

		protected override void RenderContents(HtmlTextWriter output) {
			LoadAndTweakData();

			WriteTopLevel(output);
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);

			try {
				if (this.PublicParmValues.Any()) {
					string sTmp = "";

					sTmp = GetParmValue("CssClass", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.CssClass = sTmp;
					}

					sTmp = GetParmValue("CSSItem", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.CSSItem = sTmp;
					}

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
				}
			} catch (Exception ex) {
			}
		}
	}
}