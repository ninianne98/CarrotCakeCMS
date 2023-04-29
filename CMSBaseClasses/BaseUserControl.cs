﻿using Carrotware.CMS.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Base {

	public abstract class BaseUserControl : System.Web.UI.UserControl {
		protected ContentPageHelper pageHelper = new ContentPageHelper();
		protected WidgetHelper widgetHelper = new WidgetHelper();
		protected CMSConfigHelper cmsHelper = new CMSConfigHelper();

		protected string CurrentDLLVersion {
			get { return SiteData.CurrentDLLVersion; }
		}

		protected Guid SiteID {
			get {
				return SiteData.CurrentSiteID;
			}
		}

		private bool bFound = false;
		private PlaceHolder x = new PlaceHolder();

		protected PlaceHolder FindTheControl(string ControlName, Control X) {
			if (X is Page) {
				bFound = false;
				x = new PlaceHolder();
			}

			foreach (Control c in X.Controls) {
				if (c.ID == ControlName && c is PlaceHolder) {
					bFound = true;
					x = (PlaceHolder)c;
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