using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Data;
using Carrotware.CMS.UI.Base;


namespace Carrotware.CMS.UI.Admin {
	public partial class UserMembership : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserAdmin);

			LoadGrid();

		}


		protected void LoadGrid() {
			List<MembershipUser> usrs = SiteData.GetUserList();

			dvMembers.DataSource = usrs;
			dvMembers.DataBind();
		}

		protected void dvMembers_DataBound(object sender, EventArgs e) {
			FormatGrid();
		}

		protected void FormatGrid() {
			foreach (GridViewRow dgItem in dvMembers.Rows) {
				Image imgLocked = (Image)dgItem.FindControl("imgLocked");
				HiddenField hdnIsLockedOut = (HiddenField)dgItem.FindControl("hdnIsLockedOut");

				if (hdnIsLockedOut.Value.ToLower() == "true") {
					imgLocked.ImageUrl = hdnInactive.Value;
					imgLocked.AlternateText = "Locked Out";
				}
				imgLocked.ToolTip = imgLocked.AlternateText;
			}
		}

	}
}
