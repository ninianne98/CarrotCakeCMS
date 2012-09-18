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

	public class SiteNavHelperMock : ISiteNavHelper {

		public SiteNavHelperMock() { }

		public List<SiteNav> GetMasterNavigation(Guid siteID, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav();
		}

		public List<SiteNav> GetTopNavigation(Guid siteID, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav();
		}

		public List<SiteNav> GetPathNavigation(Guid siteID, Guid rootContentID, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(rootContentID);
		}

		public List<SiteNav> GetPathNavigation(Guid siteID, string sPage, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(Guid.NewGuid());
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, string sParentID, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav(Guid.NewGuid());
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, string sPage, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav(Guid.NewGuid());
		}

		public SiteNav GetPageCrumbNavigation(Guid siteID, string sPage) {

			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav GetPageCrumbNavigation(Guid siteID, Guid rootContentID) {

			return SiteNavHelper.GetSamplerView(rootContentID);
		}

		public SiteNav GetParentPageNavigation(Guid siteID, string sPage) {

			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav GetParentPageNavigation(Guid siteID, Guid rootContentID) {

			return SiteNavHelper.GetSamplerView();
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, Guid ParentID, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav(ParentID);
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, Guid PageID, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav(PageID);
		}

		public List<SiteNav> GetLatest(Guid siteID, int iUpdates, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav();
		}

		public SiteNav GetLatestVersion(Guid siteID, Guid rootContentID) {

			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav GetLatestVersion(Guid siteID, bool? active, string sPage) {

			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav FindHome(Guid siteID) {

			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav FindByFilename(Guid siteID, string urlFileName) {

			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav FindHome(Guid siteID, bool? active) {

			return SiteNavHelper.GetSamplerView();
		}


	}

}
