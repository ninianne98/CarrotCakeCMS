using System;
using System.Collections.Generic;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Core {
	public interface ISiteNavHelper {

		SiteNav FindByFilename(Guid siteID, string urlFileName);
		SiteNav FindHome(Guid siteID);
		SiteNav FindHome(Guid siteID, bool bActiveOnly);

		List<SiteNav> GetChildNavigation(Guid siteID, Guid? ParentID, bool bActiveOnly);
		List<SiteNav> GetChildNavigation(Guid siteID, string sParentID, bool bActiveOnly);

		List<SiteNav> GetLatest(Guid siteID, int iUpdates, bool bActiveOnly);

		SiteNav GetLatestVersion(Guid siteID, Guid rootContentID);
		SiteNav GetLatestVersion(Guid siteID, bool bActiveOnly, string sPage);

		List<SiteNav> GetMasterNavigation(Guid siteID, bool bActiveOnly);

		SiteNav GetPageNavigation(Guid siteID, Guid rootContentID);
		SiteNav GetPageNavigation(Guid siteID, string sPage);

		SiteNav GetParentPageNavigation(Guid siteID, Guid rootContentID);
		SiteNav GetParentPageNavigation(Guid siteID, string sPage);

		List<SiteNav> GetSiblingNavigation(Guid siteID, Guid PageID, bool bActiveOnly);
		List<SiteNav> GetSiblingNavigation(Guid siteID, string sPage, bool bActiveOnly);

		List<SiteNav> GetTopNavigation(Guid siteID, bool bActiveOnly);

		List<SiteNav> GetTwoLevelNavigation(Guid siteID, bool bActiveOnly);

		List<SiteNav> GetLevelDepthNavigation(Guid siteID, int iDepth, bool bActiveOnly);

		List<SiteNav> GetPageCrumbNavigation(Guid siteID, Guid rootContentID, bool bActiveOnly);
		List<SiteNav> GetPageCrumbNavigation(Guid siteID, string sPage, bool bActiveOnly);

	}
}
