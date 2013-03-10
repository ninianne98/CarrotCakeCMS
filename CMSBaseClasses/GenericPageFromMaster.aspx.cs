using System;
using System.Web.UI;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
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


namespace Carrotware.CMS.UI.Base {
	public partial class GenericPageFromMaster : BaseContentPage {

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			LoadMasterPageInfo();
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			LoadMasterPageInfo();
		}

		protected void LoadMasterPageInfo() {
			if (Master.ThePage != null) {
				theSite = Master.TheSite;
				pageContents = Master.ThePage;
				pageWidgets = Master.ThePageWidgets;
			} else {
				if (pageContents == null) {
					theSite = SiteData.CurrentSite;
					pageContents = theSite.GetCurrentPage();
					if (pageContents != null) {
						pageWidgets = theSite.GetCurrentPageWidgets(pageContents.Root_ContentID);
					}
				}
			}
		}

	}
}