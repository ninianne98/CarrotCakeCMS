using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;

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
	[ToolboxData("<{0}:ContentRichText runat=server></{0}:ContentRichText>")]
	public class ContentRichText : WidgetWebControl, IWidgetMultiMenu, IWidgetRawData, ITextControl {

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
			get { return null; }
		}

		#endregion IWidget Members

		#region IWidgetMultiMenu Members

		public Dictionary<string, string> JSEditFunctions {
			get {
				Dictionary<string, string> lst = new Dictionary<string, string>();
				lst.Add("Edit HTML", "cmsShowEditWidgetForm('" + this.PageWidgetID + "', '" + SiteData.HtmlMode + "');");
				lst.Add("Edit Text", "cmsShowEditWidgetForm('" + this.PageWidgetID + "', '" + SiteData.RawMode + "');");
				return lst;
			}
		}

		#endregion IWidgetMultiMenu Members

		#region IWidgetRawData Members

		public string RawWidgetData { get; set; }

		#endregion IWidgetRawData Members

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			RenderContents(writer);
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			output.Indent = indent + 3;
			output.WriteLine();

			this.Text = SiteData.CurrentSite.UpdateContentRichText(this.RawWidgetData);

			output.WriteLine();
			output.Write(this.Text);
			output.WriteLine();

			output.Indent = indent;
		}
	}
}