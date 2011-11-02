using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin {
	public partial class SiteMap : AdminBasePage {
		List<ContentPage> lstSite = new List<ContentPage>();

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.Content);

			if (!IsPostBack) {
				lstSite = (from c in pageHelper.GetLatestContentList(SiteID)
								  orderby c.TemplateFile
								  select c).ToList();
				rpTop.DataSource = (from l in lstSite
									orderby l.NavOrder, l.NavMenuText
									where l.Parent_ContentID == null
									select l).ToList();
				rpTop.DataBind();

			}

		}

		protected void rpMap_ItemDataBound(object sender, RepeaterItemEventArgs e) {
			PlaceHolder ph = (PlaceHolder)e.Item.FindControl("ph");

			if (ph != null) {
				var d = (ContentPage)e.Item.DataItem;
				var lst = (from l in lstSite
									orderby l.NavOrder, l.NavMenuText
									where l.Parent_ContentID == d.Root_ContentID
									select l).ToList();

				if (lst.Count > 0) {
					Repeater rp = new Repeater();
					
					rp.HeaderTemplate = rpSub.HeaderTemplate;
					rp.ItemTemplate = rpSub.ItemTemplate;
					rp.FooterTemplate = rpSub.FooterTemplate;
					ph.Controls.Add(rp);

					rp.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rpMap_ItemDataBound);
					
					rp.DataSource = lst;
					rp.DataBind();
				}
			}

		}

		protected void btnSave_Click(object sender, EventArgs e) {
			SiteMapOrder orderHelper = new SiteMapOrder();
			var lst = orderHelper.CreateSiteMapList(txtMap.Text);

			orderHelper.UpdateSiteMap(SiteID, lst);

			Response.Redirect(CurrentScriptName);
		}





	}
}
