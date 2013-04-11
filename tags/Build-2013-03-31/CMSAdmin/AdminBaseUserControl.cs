using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

namespace Carrotware.CMS.UI.Admin {
	public class AdminBaseUserControl : BaseUserControl {

		protected Guid GetGuidPageIDFromQuery() {
			return GeneralUtilities.GetGuidPageIDFromQuery();
		}

		protected Guid GetGuidIDFromQuery() {
			return GeneralUtilities.GetGuidIDFromQuery();
		}

		protected Guid GetGuidParameterFromQuery(string ParmName) {
			return GeneralUtilities.GetGuidParameterFromQuery(ParmName);
		}

		protected void RedirectIfNoSite() {
			if (SiteData.CurrentSite == null) {
				Response.Redirect(SiteFilename.DashboardURL);
			}
		}


	}
}