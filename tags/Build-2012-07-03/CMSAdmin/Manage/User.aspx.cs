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
	public partial class User : AdminBasePage {

		public Guid id = Guid.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserAdmin);

			if (Request.QueryString["id"] != null) {
				id = new Guid(Request.QueryString["id"]);
			}



			if (!IsPostBack) {
				if (id != Guid.Empty) {
					var dsRoles = new List<aspnet_Role>();

					if (!IsAdmin) {
						dsRoles = (from l in db.aspnet_Roles
								   where l.RoleName != "CarrotCMS Users" && l.RoleName != "CarrotCMS Administrators"
								   orderby l.RoleName
								   select l).ToList();
					} else {
						dsRoles = (from l in db.aspnet_Roles
								   where l.RoleName != "CarrotCMS Users"
								   orderby l.RoleName
								   select l).ToList();
					}

					CheckBox chkSelected = null;

					gvSites.Visible = false;
					if (IsAdmin) {
						gvSites.Visible = true;
						gvSites.DataSource = (from l in db.tblSites
											  orderby l.SiteName
											  select l).ToList();
						gvSites.DataBind();


						var dsLocs = (from l in db.tblUserSiteMappings
									  where l.UserId == id
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

					MembershipUser usr = Membership.GetUser(id);
					Email.Text = usr.Email;
					lblUserName.Text = usr.UserName;
					UserName.Text = usr.UserName;
					lblUserName.Visible = true;
					UserName.Visible = false;

					chkLocked.Checked = usr.IsLockedOut;


					gvRoles.DataSource = dsRoles;
					gvRoles.DataBind();

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

			if (id != Guid.Empty) {
				CheckBox chkSelected = null;

				MembershipUser usr = Membership.GetUser(id);
				usr.Email = Email.Text;
				Membership.UpdateUser(usr);

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


				if (!Roles.IsUserInRole(usr.UserName, "CarrotCMS Users")) {
					Roles.AddUserToRole(usr.UserName, "CarrotCMS Users");
				}


				if (IsAdmin) {
					var dsLocs = (from l in db.tblUserSiteMappings
								  where l.UserId == id
								  select l).ToList();

					HiddenField hdnSiteID = null;
					foreach (GridViewRow dgItem in gvSites.Rows) {
						hdnSiteID = (HiddenField)dgItem.FindControl("hdnSiteID");
						if (hdnSiteID != null) {
							var iLoc = new Guid(hdnSiteID.Value);
							chkSelected = (CheckBox)dgItem.FindControl("chkSelected");

							int ct = (from l in dsLocs
									  where l.SiteID == iLoc
									  select l).Count();

							tblUserSiteMapping map = new tblUserSiteMapping();
							map.UserSiteMappingID = Guid.NewGuid();
							map.SiteID = iLoc;
							map.UserId = id;

							if (chkSelected.Checked) {
								if (ct < 1) {
									db.tblUserSiteMappings.InsertOnSubmit(map);
									db.SubmitChanges();
								}
							} else {
								if (ct > 0) {
									var loc = (from l in dsLocs
											   where l.SiteID == iLoc
											   select l).First();
									db.tblUserSiteMappings.DeleteOnSubmit(loc);
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
