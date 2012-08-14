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

namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class UserGroupAddEdit : AdminBasePage {
		public Guid groupID = Guid.Empty;


		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.GroupAdmin);

			if (Request.QueryString["id"] != null) {
				groupID = new Guid(Request.QueryString["id"]);
			}

			if (groupID == Guid.Empty) {
				btnApply.Text = "Add";
			} else {
				pnlUsers.Visible = true;
			}

			if (!IsPostBack) {
				if (groupID != Guid.Empty) {
					aspnet_Role role = getCurrentGroup();

					txtRoleName.Text = role.RoleName;

					btnApply.Visible = CheckValidEditing(role.RoleName);

					GetUserList(role.RoleName);

				}
			}

		}

		protected void GetUserList(string roleName) {

			List<MembershipUser> usrs = SiteData.GetUsersInRole(roleName);
			gvUsers.DataSource = usrs;
			gvUsers.DataBind();

			if (usrs.Count < 1) {
				btnRemove.Visible = false;
			}
		}

		protected aspnet_Role getCurrentGroup() {
			aspnet_Role role = (from l in db.aspnet_Roles
								where l.RoleId == groupID
								select l).FirstOrDefault();

			return role;
		}


		protected void btnAddUsers_Click(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(hdnUserID.Value)) {
				aspnet_Role role = getCurrentGroup();
				if (!Roles.IsUserInRole(hdnUserID.Value, role.RoleName)) {
					Roles.AddUserToRole(hdnUserID.Value, role.RoleName);
				}
			}

			Response.Redirect(SiteData.CurrentScriptName + "?id=" + groupID.ToString());
		}



		protected void btnRemove_Click(object sender, EventArgs e) {
			HiddenField hdnUserName = null;
			CheckBox chkSelected = null;

			aspnet_Role role = getCurrentGroup();

			string currentRoleName = role.RoleName;

			foreach (GridViewRow dgItem in gvUsers.Rows) {
				hdnUserName = (HiddenField)dgItem.FindControl("hdnUserName");

				if (!string.IsNullOrEmpty(hdnUserName.Value)) {

					chkSelected = (CheckBox)dgItem.FindControl("chkSelected");
					if (chkSelected.Checked) {
						Roles.RemoveUserFromRole(hdnUserName.Value, currentRoleName);
					}
				}
			}

			Response.Redirect(SiteData.CurrentScriptName + "?id=" + groupID.ToString());
		}

		protected void btnApply_Click(object sender, EventArgs e) {
			aspnet_Role role = null;

			if (!Roles.RoleExists(txtRoleName.Text) && groupID == Guid.Empty) {
				Roles.CreateRole(txtRoleName.Text);
			}

			if (Roles.RoleExists(txtRoleName.Text) || groupID != Guid.Empty) {
				if (groupID == Guid.Empty) {
					role = (from l in db.aspnet_Roles
							where l.LoweredRoleName.ToLower() == txtRoleName.Text.ToLower()
							select l).FirstOrDefault();

					groupID = role.RoleId;

				} else {
					role = (from l in db.aspnet_Roles
							where l.RoleId == groupID
							select l).FirstOrDefault();
				}

				if (role != null && groupID != Guid.Empty) {
					if (CheckValidEditing(role.LoweredRoleName)
							&& CheckValidEditing(txtRoleName.Text)) {

						role.RoleName = txtRoleName.Text;
						role.LoweredRoleName = txtRoleName.Text.ToLower();

						db.SubmitChanges();
					}

					if (CheckValidEditing(role.LoweredRoleName)
							&& !CheckValidEditing(txtRoleName.Text)) {

						txtRoleName.Text = role.RoleName;
					}
				}

				Response.Redirect(SiteData.CurrentScriptName + "?id=" + groupID.ToString());
			}

		}

		private bool CheckValidEditing(string sLoweredRoleName) {
			sLoweredRoleName = sLoweredRoleName.ToLower();

			if (groupID != Guid.Empty
						&& sLoweredRoleName != SiteData.CMSGroup_Admins.ToLower()
						&& sLoweredRoleName != SiteData.CMSGroup_Editors.ToLower()
						&& sLoweredRoleName != SiteData.CMSGroup_Users.ToLower()) {

				return true;
			}

			return false;
		}


	}
}