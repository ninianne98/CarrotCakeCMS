﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.FAQModule {
	public partial class FAQAdmin : AdminModule {

		protected void Page_Load(object sender, EventArgs e) {

			if (!IsPostBack) {
				LoadData();
			}
		}

		protected void LoadData() {
			using (dbFAQDataContext db = dbFAQDataContext.GetDataContext()) {
				var lst = (from c in db.tblFAQs
						   where c.SiteID == SiteID
						   orderby c.SortOrder
						   select c).ToList();


				dgMenu.DataSource = lst;
				dgMenu.DataBind();
			}
		}

	}
}