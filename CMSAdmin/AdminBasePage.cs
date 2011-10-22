using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using Carrotware.CMS.Data;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;

/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin {
	public class AdminBasePage : BasePage {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		protected SiteNav navHelper = new SiteNav();

		public bool IsPageLocked(ContentPage cp) {

			bool bLock = false;

			if (cp.Heartbeat_UserId != null) {
				if (cp.Heartbeat_UserId != CurrentUserGuid
						&& cp.EditHeartbeat.Value > DateTime.Now.AddMinutes(-2)) {
					bLock = true;
				}
				if (cp.Heartbeat_UserId == CurrentUserGuid
					|| cp.Heartbeat_UserId == null) {
					bLock = false;
				}
			}
			return bLock;
		}


		protected override void OnInit(EventArgs e) {
			if (Page.User.Identity.IsAuthenticated) {
				LoadGuids();
				if (!IsAdmin) {
					var lstSites = (from l in db.tblUserSiteMappings
									where l.UserId == CurrentUserGuid
										 && l.SiteID == SiteID
									select l).ToList();

					if (lstSites.Count < 1) {
						FormsAuthentication.SignOut();
						Response.Redirect("./Logon.aspx");
					}
				}
			}

			Response.Cache.SetCacheability(System.Web.HttpCacheability.Private);
			DateTime dtExpire = System.DateTime.Now.AddMinutes(-5);
			Response.Cache.SetExpires(dtExpire);

			//base.OnLoad(e);

		}



	}
}
