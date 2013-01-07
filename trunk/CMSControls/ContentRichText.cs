using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
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

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:ContentRichText runat=server></{0}:ContentRichText>")]
	public class ContentRichText : WebControl, IWidget, IWidgetRawData, ITextControl {

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

		public Guid PageWidgetID { get; set; }

		public Guid RootContentID { get; set; }

		public Guid SiteID { get; set; }

		public string JSEditFunction {
			get { return "cmsShowEditWidgetForm('" + this.PageWidgetID + "', 'html');"; }
		}

		public bool EnableEdit { get { return true; } }

		#endregion
		#region IWidgetRawData Members

		public string RawWidgetData { get; set; }

		#endregion

		protected override void Render(HtmlTextWriter writer) {
			RenderContents(writer);
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			output.Indent = indent + 3;
			output.WriteLine();

			this.Text = RawWidgetData;

			output.WriteLine();
			output.Write(RawWidgetData);
			output.WriteLine();

			output.Indent = indent;
		}


	}
}
