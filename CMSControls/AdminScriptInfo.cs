using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.ComponentModel;
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

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:AdminScriptInfo runat=server></{0}:AdminScriptInfo>")]
	public class AdminScriptInfo : BaseWebControl {

		protected override void RenderContents(HtmlTextWriter writer) {
			var versionKey = string.Format("cms={0}", SiteData.CurrentDLLVersion);
			var tag = new HtmlTag(HtmlTag.EasyTag.JavaScript);
			var key = SecurityData.IsAuthenticated ? DateTime.UtcNow.Ticks.ToString().Substring(0, 8) : WebControlHelper.DateKey();

			tag.Uri = SiteFilename.AdminScriptValues + "?ts=" + key + (SecurityData.IsAuthenticated ? ("&a=true&" + versionKey) : string.Empty);

			writer.WriteLine(tag.RenderTag());
		}

		protected override void OnPreRender(EventArgs e) { }
	}
}