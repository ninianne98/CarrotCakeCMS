using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;

namespace Carrotware.CMS.UI.Admin {
	public partial class PageHistory : AdminBasePage {

		public Guid guidRootID = Guid.Empty;
		public Guid guidContentID = Guid.Empty;
		string sPageMode = String.Empty;

		protected void Page_Load(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				guidRootID = new Guid(Request.QueryString["id"].ToString());
			}
			if (!string.IsNullOrEmpty(Request.QueryString["version"])) {
				guidContentID = new Guid(Request.QueryString["version"].ToString());
			}

			ContentPage p = null;

			if (guidRootID != Guid.Empty) {
				p = pageHelper.GetLatestContent(SiteID, guidRootID);
				if (!IsPostBack) {
					LoadGrid();
				}
				pnlDetail.Visible = false;
				pnlHistory.Visible = true;
			}

			if (guidContentID != Guid.Empty) {
				p = pageHelper.GetVersion(SiteID, guidContentID);

				litLeft.Text = p.LeftPageText;
				litCenter.Text = p.PageText;
				litRight.Text = p.RightPageText;
				guidRootID = p.Root_ContentID;

				lblEditDate.Text = p.EditDate.ToString();

				pnlDetail.Visible = true;
				pnlHistory.Visible = false;
			}

			if (p != null) {
				lblFilename.Text = p.FileName;
				lblCreated.Text = p.CreateDate.ToString();
				if (p.PageActive != true) {
					imgStatus.ImageUrl = hdnInactive.Value;
					imgStatus.AlternateText = "Inactive";
				}
				imgStatus.ToolTip = imgStatus.AlternateText;
			}


			lnkReturn.NavigateUrl = "./PageHistory.aspx?id=" + guidRootID.ToString();

		}



		protected void LoadGrid() {

			var lstCont = pageHelper.GetVersionHistory(SiteID, guidRootID);

			gvPages.DataSource = lstCont;
			gvPages.DataBind();

		}

		protected void gvPages_DataBound(object sender, EventArgs e) {

			var current = pageHelper.GetLatestContent(SiteID, guidRootID);

			foreach (GridViewRow dgItem in gvPages.Rows) {
				Image imgActive = (Image)dgItem.FindControl("imgActive");
				HiddenField hdnIsActive = (HiddenField)dgItem.FindControl("hdnIsActive");
				CheckBox chkContent = (CheckBox)dgItem.FindControl("chkContent");

				if (hdnIsActive != null && imgActive != null) {
					if (hdnIsActive.Value.ToLower() != "true") {
						imgActive.ImageUrl = hdnInactive.Value;
						imgActive.AlternateText = "Inactive";
					}
					imgActive.ToolTip = imgActive.AlternateText;
				}

				if (chkContent != null) {
					if (chkContent.Attributes["value"].ToString() == current.ContentID.ToString()) {
						chkContent.Visible = false;
					}
				}
			}
		}

		protected void btnRemove_Click(object sender, EventArgs e) {
			List<Guid> lstDel = new List<Guid>();
			foreach (GridViewRow dgItem in gvPages.Rows) {
				CheckBox chkContent = (CheckBox)dgItem.FindControl("chkContent");
				if (chkContent != null) {
					if (chkContent.Checked) {
						lstDel.Add(new Guid(chkContent.Attributes["value"].ToString()));
					}
				}
			}

			pageHelper.RemoveVersions(SiteID, lstDel);

			LoadGrid();
		}


	}
}
