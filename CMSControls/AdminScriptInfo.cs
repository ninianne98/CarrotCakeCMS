using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.ComponentModel;
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

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:AdminScriptInfo runat=server></{0}:AdminScriptInfo>")]
	public class AdminScriptInfo : BaseWebControl {

		protected override void RenderContents(HtmlTextWriter output) {
			var tag = new HtmlTag(HtmlTag.EasyTag.JavaScript);
			var key = SecurityData.IsAuthenticated ? DateTime.UtcNow.Ticks.ToString().Substring(0, 8) : WebControlHelper.DateKey();
			tag.Uri = SiteFilename.AdminScriptValues + "?ts=" + key;

			output.WriteLine(tag.RenderTag());
		}

		protected override void OnPreRender(EventArgs e) { }
	}
}