﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.FAQModule {
	public partial class FAQDisplay : WidgetUserControl {

		protected int iFaq = 0;

		protected string FaqCounter() {
			iFaq++;
			return iFaq.ToString();
		}

		protected void Page_Load(object sender, EventArgs e) {

			using (dbFAQDataContext db = dbFAQDataContext.GetDataContext()) {

				var lstFAQ = (from f in db.tblFAQs
							  where f.SiteID == SiteID
							  && f.IsActive == true
							  orderby f.SortOrder
							  select f).ToList();

				if (lstFAQ == null) {
					dgFAQ.Visible = false;
				} else {
					dgFAQ.DataSource = lstFAQ;
					dgFAQ.DataBind();
					dgFAQ.Visible = true;
				}
			}
		}

	}
}