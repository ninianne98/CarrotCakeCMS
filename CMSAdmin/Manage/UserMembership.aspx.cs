using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;


namespace Carrotware.CMS.UI.Admin {
	public partial class UserMembership : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserAdmin);


			if (!IsPostBack || IsPageRefreshJavaScript) {
				LoadGrid(hdnSort.Value);
			}
		}


		protected void LoadGrid(string sSortKey) {
			List<MembershipUser> usrs = null;

			if (!IsPostBack) {
				usrs = GetUserList();
			}

			LoadGrid<MembershipUser>(dvMembers, hdnSort, usrs, sSortKey);

			FormatGrid();
		}

		protected void lblSort_Command(object sender, EventArgs e) {
			gs.DefaultSort = hdnSort.Value;
			gs.Sort = hdnSort.Value;
			LinkButton lb = (LinkButton)sender;
			string sSortField = "";
			try { sSortField = lb.CommandName.ToString(); } catch { }
			sSortField = gs.ResetSortToColumn(sSortField);
			LoadGrid(sSortField);
			hdnSort.Value = sSortField;
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

			gs.WalkGridForHeadings(dvMembers);
		}



	}
}
