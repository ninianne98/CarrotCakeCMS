using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using Carrotware.CMS.Core;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:SecondLevelNavigation runat=server></{0}:SecondLevelNavigation>")]
	public class SecondLevelNavigation : BaseNavSelHeaded {

		public bool IncludeParent { get; set; }

		protected override void LoadData() {
			base.LoadData();

			List<SiteNav> lstNav = navHelper.GetSiblingNavigation(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName, !SecurityData.IsAuthEditor);

			if (IncludeParent) {
				if (lstNav != null && lstNav.Count > 0) {
					SiteNav p = GetParent(lstNav.OrderByDescending(x => x.Parent_ContentID).FirstOrDefault().Parent_ContentID);
					if (p != null) {
						p.NavOrder = -100;
						lstNav.Add(p);
					}
				}
			}

			this.NavigationData = lstNav.OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected SiteNav GetParent(Guid? rootContentID) {
			SiteNav pageNav = null;
			if (rootContentID.HasValue) {
				pageNav = navHelper.GetPageNavigation(SiteData.CurrentSiteID, rootContentID.Value);
			}
			return pageNav;
		}

	}
}
