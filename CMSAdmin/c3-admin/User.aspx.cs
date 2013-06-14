using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
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

						if (lstSites.Count > 0) {
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

					MembershipUser usr = Membership.GetUser(userID);
					Email.Text = usr.Email;
					lblUserName.Text = usr.UserName;
					UserName.Text = usr.UserName;
					lblUserName.Visible = true;
					UserName.Visible = false;

					chkLocked.Checked = usr.IsLockedOut;

					txtNickName.Text = exUsr.UserNickName;
					txtFirstName.Text = exUsr.FirstName;
					txtLastName.Text = exUsr.LastName;
					reBody.Text = exUsr.UserBio;

					GeneralUtilities.BindDataBoundControl(gvRoles, dsRoles);

					chkSelected = null;

					HiddenField hdnRoleId = null;
					foreach (GridViewRow dgItem in gvRoles.Rows) {
						hdnRoleId = (HiddenField)dgItem.FindControl("hdnRoleId");
						if (hdnRoleId != null) {
							chkSelected = (CheckBox)dgItem.FindControl("chkSelected");
							if (Roles.IsUserInRole(usr.UserName, hdnRoleId.Value)) {
								chkSelected.Checked = true;
							}
						}
					}
				}
			}

		}



		protected void btnApply_Click(object sender, EventArgs e) {

			if (userID != Guid.Empty) {

				MembershipUser usr = Membership.GetUser(userID);
				usr.Email = Email.Text;
				Membership.UpdateUser(usr);

				ExtendedUserData exUsr = new ExtendedUserData(userID);
				exUsr.UserNickName = txtNickName.Text;
				exUsr.FirstName = txtFirstName.Text;
				exUsr.LastName = txtLastName.Text;
				exUsr.UserBio = reBody.Text;

				exUsr.Save();

				if (!chkLocked.Checked) {
					usr.UnlockUser();
					Membership.UpdateUser(usr);
				} else {
					if (!usr.IsLockedOut) {
						int iCount = 0;
						while (iCount < 10) {
							Membership.ValidateUser(usr.UserName, "zzBadPassWord123a" + iCount.ToString());
							Membership.ValidateUser(usr.UserName, "zzBadPassWord123b" + iCount.ToString());
							iCount++;
						}
					}
				}

				exUsr.AddToRole(SecurityData.CMSGroup_Users);

				CheckBox chkSelected = null;
				HiddenField hdnSiteID = null;

				foreach (GridViewRow dgItem in gvSites.Rows) {
					hdnSiteID = (HiddenField)dgItem.FindControl("hdnSiteID");

					if (hdnSiteID != null) {
						Guid guidSiteID = new Guid(hdnSiteID.Value);
						chkSelected = (CheckBox)dgItem.FindControl("chkSelected");

						if (chkSelected.Checked) {
							exUsr.AddToSite(guidSiteID);
						} else {
							exUsr.RemoveFromSite(guidSiteID);
						}
					}
				}

				HiddenField hdnRoleId = null;

				foreach (GridViewRow dgItem in gvRoles.Rows) {
					hdnRoleId = (HiddenField)dgItem.FindControl("hdnRoleId");
					if (hdnRoleId != null) {
						chkSelected = (CheckBox)dgItem.FindControl("chkSelected");

						if (chkSelected.Checked) {
							exUsr.AddToRole(hdnRoleId.Value);
						} else {
							exUsr.RemoveFromRole(hdnRoleId.Value);
						}
					}
				}
			}

		}


	}
}
