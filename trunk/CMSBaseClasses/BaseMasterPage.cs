using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
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

namespace Carrotware.CMS.UI.Base {
	public class BaseMasterPage : System.Web.UI.MasterPage {
		protected Guid SiteID {
			get {
				return SiteData.CurrentSiteID;
			}
		}

		protected string CurrentDLLVersion {
			get { return SiteData.CurrentDLLVersion; }
		}


	}
}
