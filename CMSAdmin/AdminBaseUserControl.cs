using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

namespace Carrotware.CMS.UI.Admin {
	public class AdminBaseUserControl : BaseUserControl {

		protected Guid GetGuidPageIDFromQuery() {
			return GetGuidParameterFromQuery("pageid");
		}

		protected Guid GetGuidIDFromQuery() {
			return GetGuidParameterFromQuery("id");
		}

		protected Guid GetGuidParameterFromQuery(string ParmName) {
			Guid id = Guid.Empty;
			if (Request.QueryString[ParmName] != null) {
				id = new Guid(Request.QueryString[ParmName]);
			}
			return id;
		}

		protected void RedirectIfNoSite() {
			if (SiteData.CurrentSite == null) {
				Response.Redirect(SiteFilename.DashboardURL);
			}
		}


	}
}