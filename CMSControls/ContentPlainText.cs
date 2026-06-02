using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
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
	[ToolboxData("<{0}:ContentPlainText runat=server></{0}:ContentPlainText>")]
	public class ContentPlainText : WidgetWebControl, IWidgetRawData, ITextControl {

		public string Text {
			get {
				String s = (String)ViewState["Text"];
				return ((s == null) ? String.Empty : s);
			}

			set {
				ViewState["Text"] = value;
			}
		}

		#region IWidget Members

		public override string JSEditFunction {
			get { return "cmsShowEditWidgetForm('" + this.PageWidgetID + "', '" + SiteData.RawMode + "');"; }
		}

		#endregion IWidget Members

		#region IWidgetRawData Members

		public string RawWidgetData { get; set; }

		#endregion IWidgetRawData Members

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			RenderContents(writer);
		}

		protected override void RenderContents(HtmlTextWriter writer) {
			int indent = writer.Indent;

			writer.Indent = indent + 3;
			writer.WriteLine();

			this.Text = SiteData.CurrentSite.UpdateContentPlainText(this.RawWidgetData);

			writer.WriteLine();
			writer.Write(this.Text);
			writer.WriteLine();

			writer.Indent = indent;
		}
	}
}