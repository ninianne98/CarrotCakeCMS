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


		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["pageid"])) {
				guidContentID = new Guid(Request.QueryString["pageid"].ToString());
			}

			if (!IsPostBack) {
				DoDataBind();
			}

		}

		public string MakeStar(bool bFlag) {
			if (bFlag) {
				return "  ";
			} else {
				return "&#9746; ";
			}
		}

		protected void DoDataBind() {
			List<SiteNav> lst = null;

			using (SiteNavHelper navHelper = new SiteNavHelper()) {
				lst = navHelper.GetChildNavigation(SiteData.CurrentSiteID, guidContentID, !SiteData.IsAuthEditor);
			}

			rpPages.DataSource = lst;
			rpPages.DataBind();

			if (lst.Count < 2) {
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




	}
}