using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
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
	public partial class UserAdd : AdminBasePage {

		protected HtmlGenericControl divMsg {
			get {
				return ((HtmlGenericControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divMsg"));
			}
		}
		protected Literal FailureText {
			get {
				return ((Literal)CreateUserWizardStep1.ContentTemplateContainer.FindControl("FailureText"));
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserAdmin);
			createWizard.Visible = SecurityData.IsAdmin;

			divMsg.Visible = false;
		}

		protected void createWizard_CreatedUser(object sender, EventArgs e) {
			var pb = ProfileBase.Create(createWizard.UserName, false);
			Roles.AddUserToRole(createWizard.UserName, SecurityData.CMSGroup_Users);

			try {
				MembershipUser usr = Membership.GetUser(createWizard.UserName);
				Guid userID = new Guid(usr.ProviderUserKey.ToString());

				siteHelper.MapUserToSite(SiteID, userID);

				Response.Redirect(string.Format("{0}?id={1}", SiteFilename.UserURL, userID));
			} catch (Exception ex) {
			}
		}

		protected void createWizard_CreatingUser(object sender, LoginCancelEventArgs e) {

		}

		protected void createWizard_CreateUserError(object sender, CreateUserErrorEventArgs e) {
			divMsg.Visible = true;
			string sFieldValue = e.CreateUserError.ToString();

			CreateUserWizard cw = (CreateUserWizard)sender;
			if (cw != null) {
				object objData = ReflectionUtilities.GetPropertyValue(cw, e.CreateUserError.ToString() + "ErrorMessage");
				if (objData != null) {
					sFieldValue = string.Format("{0}", objData);
				}
			}

			FailureText.Text = sFieldValue;
		}

	}
}
