using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
using Carrotware.CMS.Data;


namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class UserAdd : AdminBasePage {
		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.UserAdmin);

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
	}
}
