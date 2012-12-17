using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.CMS.UI.Base;


namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class PageChildSort : AdminBasePage {

		public Guid guidContentID = Guid.Empty;
		bool bClickedSort = false;

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["pageid"])) {
				guidContentID = new Guid(Request.QueryString["pageid"].ToString());
			}

			if (!IsPostBack) {
				DoDataBind();
			}

		}

		protected void DoDataBind() {
			List<SiteNav> lstNav = null;

			using (SiteNavHelper navHelper = new SiteNavHelper()) {
				lstNav = navHelper.GetChildNavigation(SiteData.CurrentSiteID, guidContentID, !SecurityData.IsAuthEditor);
			}

			lstNav.ToList().ForEach(q => CMSConfigHelper.IdentifyLinkAsInactive(q));

			if (bClickedSort && ddlAlternateSort.SelectedIndex > 0) {
				switch (ddlAlternateSort.SelectedValue) {
					case "alpha":
						lstNav = lstNav.OrderBy(x => x.NavMenuText).ToList();
						break;
					case "datecreated":
						lstNav = lstNav.OrderBy(x => x.CreateDate).ToList();
						break;
					case "dateupdated":
						lstNav = lstNav.OrderBy(x => x.EditDate).ToList();
						break;
					case "alpha2":
						lstNav = lstNav.OrderByDescending(x => x.NavMenuText).ToList();
						break;
					case "datecreated2":
						lstNav = lstNav.OrderByDescending(x => x.CreateDate).ToList();
						break;
					case "dateupdated2":
						lstNav = lstNav.OrderByDescending(x => x.EditDate).ToList();
						break;

					default:
						lstNav = lstNav.OrderBy(x => x.NavOrder).ToList();
						break;
				}
			}

			rpPages.DataSource = lstNav;
			rpPages.DataBind();

			if (lstNav.Count < 2) {
				btnSave.Visible = false;
			}

		}

		protected void btnSave_Click(object sender, EventArgs e) {
			using (SiteMapOrderHelper orderHelper = new SiteMapOrderHelper()) {
				var lst = orderHelper.ParseChildPageData(txtSort.Text, guidContentID);
				orderHelper.UpdateSiteMap(SiteData.CurrentSiteID, lst);
			}

			Response.Redirect(SiteData.CurrentScriptName + "?" + Request.QueryString.ToString());
		}

		protected void btnSort_Click(object sender, EventArgs e) {
			bClickedSort = true;

			DoDataBind();
		}


	}
}