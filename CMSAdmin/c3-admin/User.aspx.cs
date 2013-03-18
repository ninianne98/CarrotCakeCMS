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
using Carrotware.CMS.UI.Controls;


namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class User : AdminBasePage {

		public Guid userID = Guid.Empty;
		protected CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserAdmin);

			userID = GetGuidIDFromQuery();

			if (!IsPostBack) {
				if (userID != Guid.Empty) {
					var dsRoles = SecurityData.GetRoleListRestricted();

					CheckBox chkSelected = null;

					gvSites.Visible = false;
					if (SecurityData.IsAdmin) {
						gvSites.Visible = true;

						GeneralUtilities.BindDataBoundControl(gvSites, SiteData.GetSiteList());

						var dsLocs = (from l in db.carrot_UserSiteMappings
									  where l.UserId == userID
									  select l).ToList();

						chkSelected = null;

						if (dsLocs.Count > 0) {
							HiddenField hdnSiteID = null;
							foreach (GridViewRow dgItem in gvSites.Rows) {
								hdnSiteID = (HiddenField)dgItem.FindControl("hdnSiteID");
								if (hdnSiteID != null) {
									var iLoc = new Guid(hdnSiteID.Value);
									chkSelected = (CheckBox)dgItem.FindControl("chkSelected");
									int ct = (from l in dsLocs
											  where l.SiteID == iLoc
											  select l).Count();
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

					ExtendedUserData ud = new ExtendedUserData(usr.UserName);
					txtNickName.Text = ud.UserNickName;
					txtFirstName.Text = ud.FirstName;
					txtLastName.Text = ud.LastName;

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
				CheckBox chkSelected = null;

				MembershipUser usr = Membership.GetUser(userID);
				usr.Email = Email.Text;
				Membership.UpdateUser(usr);

				ExtendedUserData ud = new ExtendedUserData(usr.UserName);
				ud.UserNickName = txtNickName.Text;
				ud.FirstName = txtFirstName.Text;
				ud.LastName = txtLastName.Text;
				ud.Save();

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


				if (!Roles.IsUserInRole(usr.UserName, SecurityData.CMSGroup_Users)) {
					Roles.AddUserToRole(usr.UserName, SecurityData.CMSGroup_Users);
				}


				if (SecurityData.IsAdmin) {
					var dsLocs = (from l in db.carrot_UserSiteMappings
								  where l.UserId == userID
								  select l).ToList();

					HiddenField hdnSiteID = null;
					foreach (GridViewRow dgItem in gvSites.Rows) {
						hdnSiteID = (HiddenField)dgItem.FindControl("hdnSiteID");
						if (hdnSiteID != null) {
							var guidSiteID = new Guid(hdnSiteID.Value);
							chkSelected = (CheckBox)dgItem.FindControl("chkSelected");

							int ct = (from l in dsLocs
									  where l.SiteID == guidSiteID
									  select l).Count();

							carrot_UserSiteMapping map = new carrot_UserSiteMapping();
							map.UserSiteMappingID = Guid.NewGuid();
							map.SiteID = guidSiteID;
							map.UserId = userID;

							if (chkSelected.Checked) {
								if (ct < 1) {
									db.carrot_UserSiteMappings.InsertOnSubmit(map);
									db.SubmitChanges();
								}
							} else {
								if (ct > 0) {
									var loc = (from l in dsLocs
											   where l.SiteID == guidSiteID
											   select l).First();
									db.carrot_UserSiteMappings.DeleteOnSubmit(loc);
									db.SubmitChanges();
								}
							}
						}
					}
				}


				HiddenField hdnRoleId = null;
				foreach (GridViewRow dgItem in gvRoles.Rows) {
					hdnRoleId = (HiddenField)dgItem.FindControl("hdnRoleId");
					if (hdnRoleId != null) {
						chkSelected = (CheckBox)dgItem.FindControl("chkSelected");

						if (chkSelected.Checked) {
							if (!Roles.IsUserInRole(usr.UserName, hdnRoleId.Value)) {
								Roles.AddUserToRole(usr.UserName, hdnRoleId.Value);
							}
						} else {
							if (Roles.IsUserInRole(usr.UserName, hdnRoleId.Value)) {
								Roles.RemoveUserFromRole(usr.UserName, hdnRoleId.Value);
							}
						}
					}
				}
			}

		}




	}
}
