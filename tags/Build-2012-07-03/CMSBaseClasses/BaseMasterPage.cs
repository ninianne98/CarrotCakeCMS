using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Profile;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Base {
	public class BaseMasterPage : System.Web.UI.MasterPage {

		protected Guid CurrentUserGuid = Guid.Empty;
		protected MembershipUser CurrentUser { get; set; }


		public string CurrentScriptName {
			get { return Request.ServerVariables["script_name"].ToString(); }
		}

		public string ReferringPage {
			get {
				var r = CurrentScriptName;
				try { r = Request.ServerVariables["http_referer"].ToString(); } catch { }
				if (string.IsNullOrEmpty(r))
					r = "./default.aspx";
				return r;
			}
		}

		public bool IsAdmin {
			get { return Roles.IsUserInRole("CarrotCMS Administrators"); }
		}
		public bool IsEditor {
			get { return Roles.IsUserInRole("CarrotCMS Editors"); }
		}
		public bool IsUsers {
			get { return Roles.IsUserInRole("CarrotCMS Users"); }
		}

		protected override void OnLoad(EventArgs e) {

			if (!String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)) {
				CurrentUser = Membership.GetUser(HttpContext.Current.User.Identity.Name);
				CurrentUserGuid = new Guid(CurrentUser.ProviderUserKey.ToString());

			}

			//SetPageButtons(this);
			base.OnLoad(e);
		}


		//protected void SetPageButtons(Control X) {
		//    //add the command click event to the link buttons on the datagrid heading
		//    foreach (Control c in X.Controls) {
		//        if (c is Button) {
		//            Button btn = (Button)c;
		//            btn.Attributes["class"] = "staticButton";
		//        } else if (c is HtmlInputButton) {
		//            HtmlInputButton btn = (HtmlInputButton)c;
		//            btn.Attributes["class"] = "staticButton";
		//        } else {
		//            SetPageButtons(c);
		//        }
		//    }
		//}


	}
}
