using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class UserGroupAddEdit : AdminBasePage {
		public Guid groupID = Guid.Empty;


		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.GroupAdmin);

			groupID = GetGuidIDFromQuery();

			btnApply.Visible = SecurityData.IsAdmin;
			btnAddUsers.Visible = SecurityData.IsAdmin;
			btnRemove.Visible = SecurityData.IsAdmin;

			if (groupID == Guid.Empty) {
				btnApply.Text = "Add";
			} else {
				pnlUsers.Visible = true;
			}

			if (!IsPostBack) {
				if (groupID != Guid.Empty) {
					MembershipRole role = getCurrentGroup();

					txtRoleName.Text = role.RoleName;
					txtRoleName.Enabled = CheckValidEditing(role.RoleName);

					btnApply.Visible = CheckValidEditing(role.RoleName);

					GetUserList(role.RoleName);
				}
			}

		}

		protected void GetUserList(string roleName) {

			List<MembershipUser> usrs = SecurityData.GetUsersInRole(roleName);
			GeneralUtilities.BindDataBoundControl(gvUsers, usrs);

			if (usrs.Count < 1) {
				btnRemove.Visible = false;
			}
		}

		protected MembershipRole getCurrentGroup() {
			MembershipRole role = SecurityData.FindMembershipRole(groupID);

			return role;
		}


		protected void btnAddUsers_Click(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(hdnUserID.Value)) {
				MembershipRole role = getCurrentGroup();

				ExtendedUserData exUsr = new ExtendedUserData(hdnUserID.Value);
				exUsr.AddToRole(role.RoleName);
			}

			Response.Redirect(SiteData.CurrentScriptName + "?id=" + groupID.ToString());
		}



		protected void btnRemove_Click(object sender, EventArgs e) {
			HiddenField hdnUserName = null;
			CheckBox chkSelected = null;

			MembershipRole role = getCurrentGroup();

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
			MembershipRole role = new MembershipRole(txtRoleName.Text, groupID);

			if (!Roles.RoleExists(txtRoleName.Text) && groupID == Guid.Empty) {
				Roles.CreateRole(txtRoleName.Text);
			}

			if (Roles.RoleExists(txtRoleName.Text) || groupID != Guid.Empty) {
				if (groupID == Guid.Empty) {
					role = SecurityData.FindMembershipRole(txtRoleName.Text);

					groupID = role.RoleId;
				} else {
					role = SecurityData.FindMembershipRole(groupID);
				}

				if (role != null && groupID != Guid.Empty) {
					if (CheckValidEditing(role.LoweredRoleName)
							&& CheckValidEditing(txtRoleName.Text)) {

						role.RoleName = txtRoleName.Text;
						role.Save();
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
						&& sLoweredRoleName != SecurityData.CMSGroup_Admins.ToLower()
						&& sLoweredRoleName != SecurityData.CMSGroup_Editors.ToLower()
						&& sLoweredRoleName != SecurityData.CMSGroup_Users.ToLower()) {

				return true;
			}

			return false;
		}


	}
}