using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System.Collections.Generic;
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

	public abstract class BaseNav : BaseNavCommon {

		protected override void RenderContents(HtmlTextWriter output) {
			LoadAndTweakData();

			int indent = output.Indent;

			List<SiteNav> lstNav = this.NavigationData;

			output.Indent = indent + 3;
			output.WriteLine();

			WriteListPrefix(output);

			if (lstNav != null && lstNav.Any()) {
				output.Indent++;

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

					output.WriteLine(childItem.RenderTag());
				}
				output.Indent--;
			} else {
#if DEBUG
				output.WriteLine("<span style=\"display: none;\" id=\"" + this.HtmlClientID + "\"></span>");
#endif
			}

			WriteListSuffix(output);

			output.Indent = indent;
		}
	}
}