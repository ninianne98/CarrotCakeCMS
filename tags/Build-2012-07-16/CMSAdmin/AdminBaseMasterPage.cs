using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
	public class AdminBaseMasterPage : BaseMasterPage {

		protected SiteData siteHelper = new SiteData();

		public enum SectionID {
			Home,
			Content,
			UserAdmin,
			UserFn,
			Modules
		}
	}
}
