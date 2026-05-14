using Carrotware.CMS.Core;
using Carrotware.CMS.Security.Models;
using Carrotware.CMS.UI.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
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
					UserRole role = getCurrentGroup();

					txtRoleName.Text = role.RoleName;
					txtRoleName.Enabled = CheckValidEditing(role.RoleName);

					btnApply.Visible = CheckValidEditing(role.RoleName);

					GetUserList(role.RoleName);
				}
			}
		}

		protected void GetUserList(string roleName) {
			List<ApplicationUser> usrs = SecurityData.GetUsersInRole(roleName);
			GeneralUtilities.BindDataBoundControl(gvUsers, usrs);

			if (usrs.Count < 1) {
				btnRemove.Visible = false;
			}
		}

		protected UserRole getCurrentGroup() {
			var role = SecurityData.FindRoleByID(groupID);

			return role;
		}

		protected void btnAddUsers_Click(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(hdnUserID.Value)) {
				UserRole role = getCurrentGroup();

				var exUsr = new ExtendedUserData(hdnUserID.Value);
				exUsr.AddToRole(role.RoleName);
			}

			Response.Redirect(SiteData.CurrentScriptName + "?id=" + groupID.ToString());
		}

		protected void btnRemove_Click(object sender, EventArgs e) {
			HiddenField hdnUserName = null;
			CheckBox chkSelected = null;

			UserRole role = getCurrentGroup();

			string currentRoleName = role.RoleName;

			foreach (GridViewRow dgItem in gvUsers.Rows) {
				hdnUserName = (HiddenField)dgItem.FindControl("hdnUserName");

				if (!string.IsNullOrEmpty(hdnUserName.Value)) {
					chkSelected = (CheckBox)dgItem.FindControl("chkSelected");
					if (chkSelected.Checked) {
						SecurityData.RemoveUserFromRole(hdnUserName.Value, currentRoleName);
					}
				}
			}

			Response.Redirect(SiteData.CurrentScriptName + "?id=" + groupID.ToString());
		}

		protected void btnApply_Click(object sender, EventArgs e) {
			bool bAdd = false;
			if (groupID == Guid.Empty) {
				groupID = Guid.NewGuid();
				bAdd = true;
			}

			UserRole role = new UserRole(txtRoleName.Text, groupID);
			UserRole item = SecurityData.FindRole(role.RoleName);

			if (item == null && bAdd == false) {
				item = SecurityData.FindRoleByID(role.RoleId);
			}

			if (item == null || bAdd) {
				item = new UserRole();
				item.RoleId = role.RoleId;
			}

			item.RoleName = role.RoleName.Trim();
			item.Save();

			if (item.RoleId.Length > 10) {
				Response.Redirect(SiteData.CurrentScriptName + "?id=" + item.RoleId);
			}
		}

		private bool CheckValidEditing(string sLoweredRoleName) {
			sLoweredRoleName = sLoweredRoleName.ToLowerInvariant();

			if (groupID != Guid.Empty
						&& sLoweredRoleName != SecurityData.CMSGroup_Admins.ToLowerInvariant()
						&& sLoweredRoleName != SecurityData.CMSGroup_Editors.ToLowerInvariant()
						&& sLoweredRoleName != SecurityData.CMSGroup_Users.ToLowerInvariant()) {
				return true;
			}

			return false;
		}
	}
}