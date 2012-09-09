using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

namespace Carrotware.CMS.UI.Admin {
	public partial class ucTextContent : BaseShellUserControl, IWidget, IWidgetRawData {

		#region IWidget Members

		public Guid PageWidgetID { get; set; }

		public Guid RootContentID { get; set; }

		public Guid SiteID { get; set; }

		public string JSEditFunction {
			get { return "cmsShowEditWidgetForm('" + this.PageWidgetID + "', 'plain');"; }
		}

		public bool EnableEdit { get { return true; } }

		#endregion
		#region IWidgetRawData Members

		public string RawWidgetData { get; set; }

		#endregion

		protected void Page_Load(object sender, EventArgs e) {

			//PageWidget pageWidget = null;

			//if (SecurityData.AdvancedEditMode) {
			//    pageWidget = (from w in cmsHelper.cmsAdminWidget
			//                  where (w.Root_WidgetID == PageWidgetID)
			//                  orderby w.WidgetOrder
			//                  select w).FirstOrDefault();

			//} else {
			//    pageWidget = widgetHelper.Get(PageWidgetID);
			//}

			//litContent.Text = pageWidget.ControlProperties;

			litContent.Text = RawWidgetData;

		}


	}
}