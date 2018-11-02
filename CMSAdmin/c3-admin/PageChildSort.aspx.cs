using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class PageChildSort : AdminBasePage {
		public Guid guidContentID = Guid.Empty;
		private bool bClickedSort = false;

		protected void Page_Load(object sender, EventArgs e) {
			Master.UsesSaved = true;
			Master.HideSave();

			guidContentID = GetGuidPageIDFromQuery();

			if (!IsPostBack) {
				DoDataBind();
			}
		}

		protected void DoDataBind() {
			List<SiteNav> lstNav = null;

			using (ISiteNavHelper navHelper = SiteNavFactory.GetSiteNavHelper()) {
				lstNav = navHelper.GetChildNavigation(SiteData.CurrentSiteID, guidContentID, !SecurityData.IsAuthEditor);
			}

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

			lstNav.ToList().ForEach(q => CMSConfigHelper.IdentifyLinkAsInactive(q));

			GeneralUtilities.BindRepeater(rpPages, lstNav);

			if (lstNav.Count < 2) {
				btnSave.Visible = false;
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			using (SiteMapOrderHelper orderHelper = new SiteMapOrderHelper()) {
				var lst = orderHelper.ParseChildPageData(txtSort.Text, guidContentID);
				orderHelper.UpdateSiteMap(SiteData.CurrentSiteID, lst);
			}

			Master.ShowSave();

			Response.Redirect(SiteData.CurrentScriptName + "?" + Request.QueryString.ToString() + Master.SavedSuffix);
		}

		protected void btnSort_Click(object sender, EventArgs e) {
			bClickedSort = true;

			DoDataBind();
		}
	}
}