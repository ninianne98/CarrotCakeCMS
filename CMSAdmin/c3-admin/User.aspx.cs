using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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

	public partial class User : AdminBasePage {
		public Guid userID = Guid.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserAdmin);

			userID = GetGuidIDFromQuery();

			btnApply.Visible = SecurityData.IsAdmin;

			if (!IsPostBack) {
				if (userID != Guid.Empty) {
					var dsRoles = SecurityData.GetRoleListRestricted();
					ExtendedUserData exUsr = new ExtendedUserData(userID);

					CheckBox chkSelected = null;

					gvSites.Visible = false;

					if (SecurityData.IsAdmin) {
						gvSites.Visible = true;

						GeneralUtilities.BindDataBoundControl(gvSites, SiteData.GetSiteList());

						List<SiteData> lstSites = exUsr.GetSiteList();

						chkSelected = null;

						if (lstSites.Any()) {
							HiddenField hdnSiteID = null;
							foreach (GridViewRow dgItem in gvSites.Rows) {
								hdnSiteID = (HiddenField)dgItem.FindControl("hdnSiteID");
								if (hdnSiteID != null) {
									Guid locID = new Guid(hdnSiteID.Value);
									chkSelected = (CheckBox)dgItem.FindControl("chkSelected");
									int ct = (from l in lstSites where l.SiteID == locID select l).Count();
									if (ct > 0) {
										chkSelected.Checked = true;
									}
								}
							}
						}
					}

					var user = securityHelper.UserManager.FindById(exUsr.UserKey);
					txtEmail.Text = user.Email;
					lblUserName.Text = user.UserName;
					txtUserName.Text = user.UserName;
					lblUserName.Visible = true;
					txtUserName.Visible = false;

					chkLocked.Checked = (user.LockoutEndDateUtc.HasValue && user.LockoutEndDateUtc.Value > DateTime.UtcNow);

					txtNickName.Text = exUsr.UserNickName;
					txtFirstName.Text = exUsr.FirstName;
					txtLastName.Text = exUsr.LastName;
					reBody.Text = exUsr.UserBio;

					GeneralUtilities.BindDataBoundControl(gvRoles, dsRoles);

					chkSelected = null;

					var roles = exUsr.GetRoles();

					HiddenField hdnRoleId = null;
					foreach (GridViewRow dgItem in gvRoles.Rows) {
						hdnRoleId = (HiddenField)dgItem.FindControl("hdnRoleId");
						if (hdnRoleId != null) {
							chkSelected = (CheckBox)dgItem.FindControl("chkSelected");
							chkSelected.Checked = roles.Where(x => x.RoleName.ToLowerInvariant() == hdnRoleId.Value.ToLowerInvariant()).Any();
						}
					}
				}
			}
		}

		protected void btnApply_Click(object sender, EventArgs e) {
			if (userID != Guid.Empty) {
				ExtendedUserData exUser = new ExtendedUserData(userID);

				var user = securityHelper.UserManager.FindById(exUser.UserKey);
				user.Email = txtEmail.Text;
				exUser.Email = user.Email;

				exUser.UserNickName = txtNickName.Text;
				exUser.FirstName = txtFirstName.Text;
				exUser.LastName = txtLastName.Text;
				exUser.UserBio = reBody.Text;

				IdentityResult result = securityHelper.UserManager.SetEmail(exUser.UserKey, exUser.Email);

				exUser.Save();

				if (chkLocked.Checked == false) {
					user.LockoutEndDateUtc = null;
					user.AccessFailedCount = 0;
					securityHelper.UserManager.Update(user);
				} else {
					if (user.LockoutEndDateUtc.HasValue == false || user.LockoutEndDateUtc.Value < DateTime.UtcNow) {
						user.LockoutEndDateUtc = DateTime.UtcNow.Date.AddYears(2);
						user.AccessFailedCount = 25;
						securityHelper.UserManager.Update(user);
					}
				}

				exUser.AddToRole(SecurityData.CMSGroup_Users);

				CheckBox chkSelected = null;
				HiddenField hdnSiteID = null;

				foreach (GridViewRow dgItem in gvSites.Rows) {
					hdnSiteID = (HiddenField)dgItem.FindControl("hdnSiteID");

					if (hdnSiteID != null) {
						Guid guidSiteID = new Guid(hdnSiteID.Value);
						chkSelected = (CheckBox)dgItem.FindControl("chkSelected");

						if (chkSelected.Checked) {
							exUser.AddToSite(guidSiteID);
						} else {
							exUser.RemoveFromSite(guidSiteID);
						}
					}
				}

				HiddenField hdnRoleId = null;

				foreach (GridViewRow dgItem in gvRoles.Rows) {
					hdnRoleId = (HiddenField)dgItem.FindControl("hdnRoleId");
					if (hdnRoleId != null) {
						chkSelected = (CheckBox)dgItem.FindControl("chkSelected");

						if (chkSelected.Checked) {
							exUser.AddToRole(hdnRoleId.Value);
						} else {
							exUser.RemoveFromRole(hdnRoleId.Value);
						}
					}
				}
			}
		}
	}
}