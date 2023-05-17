﻿using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
using System;
using static Carrotware.CMS.UI.Controls.CmsSkin;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin {

	public class AdminBaseUserControl : BaseUserControl {

		public SkinOption SiteSkin {
			get {
				return AdminBaseMasterPage.SiteSkin;
			}
		}

		public string MainColorCode {
			get {
				return AdminBaseMasterPage.MainColorCode;
			}
		}

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
			if (!SiteData.CurretSiteExists) {
				Response.Redirect(SiteFilename.SiteInfoURL);
			}
		}
	}
}