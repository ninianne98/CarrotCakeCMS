using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
using System;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Admin {

	public class AdminBaseUserControl : BaseUserControl {

		public CmsSkin.SkinOption SiteSkin {
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

		protected Guid GetGuidVersionFromQuery() {
			return GeneralUtilities.GetGuidVersionFromQuery();
		}

		protected Guid GetGuidImportFromQuery() {
			return GeneralUtilities.GetGuidImportFromQuery();
		}

		protected Guid GetGuidParameterFromQuery(string parmName) {
			return GeneralUtilities.GetGuidParameterFromQuery(parmName);
		}

		protected string GetStringParameterFromQuery(string parmName) {
			return GeneralUtilities.GetStringParameterFromQuery(parmName);
		}

		protected void RedirectIfNoSite() {
			if (!SiteData.CurrentSiteExists) {
				Response.Redirect(SiteFilename.SiteInfoURL);
			}
		}
	}
}