using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using System.Web.UI;
using Carrotware.CMS.Core;
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

		bool bFound = false;
		WidgetContainer x = new WidgetContainer();
		protected WidgetContainer FindTheControl(string ControlName, Control X) {

			if (X is Page) {
				bFound = false;
				x = new WidgetContainer();
			}

			foreach (Control c in X.Controls) {
				if (c.ID == ControlName && c is WidgetContainer) {
					bFound = true;
					x = (WidgetContainer)c;
					return x;
				} else {
					if (!bFound) {
						FindTheControl(ControlName, c);
					}
				}
			}
			return x;
		}


	}
}
