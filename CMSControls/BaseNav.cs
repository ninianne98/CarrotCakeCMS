using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Controls {

	public abstract class BaseNav : BaseNavCommon {

		protected override void RenderContents(HtmlTextWriter writer) {
			LoadAndTweakData();

			int indent = writer.Indent;

			List<SiteNav> lstNav = this.NavigationData;

			writer.Indent = indent + 3;
			writer.WriteLine();

			WriteListPrefix(writer);

			if (lstNav != null && lstNav.Any()) {
				writer.Indent++;

				foreach (SiteNav c in lstNav) {
					var childItem = new HtmlTag("li");
					var childLink = new HtmlTag("a");

					childLink.Uri = c.FileName;
					childLink.InnerHtml = c.NavMenuText;

					childItem.InnerHtml = childLink.RenderTag();

					if (c.Parent_ContentID.HasValue) {
						childItem.MergeAttribute("class", "child-nav");
					} else {
						childItem.MergeAttribute("class", "parent-nav");
					}

					writer.WriteLine(childItem.RenderTag());
				}
				writer.Indent--;
			} else {
#if DEBUG
				writer.WriteLine("<span style=\"display: none;\" id=\"" + this.HtmlClientID + "\"></span>");
#endif
			}

			WriteListSuffix(writer);

			writer.Indent = indent;
		}
	}
}