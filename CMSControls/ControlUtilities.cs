using System;
using System.Web.UI;
using System.Web.UI.WebControls;
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
	public class ControlUtilities {

		public ControlUtilities() {
			bFoundPage = false;
			page = null;
			bFoundPlaceHolder = false;
			plcholder = null;
		}

		public ContentPage GetContainerContentPage(object X) {
			bFoundPage = false;
			page = null;

			ContentPage cp = null;
			Page foundPage = FindPage(X);

			try {
				object obj = ReflectionUtilities.GetPropertyValue(foundPage, "pageContents");

				if (foundPage != null && obj is ContentPage) {
					cp = obj as ContentPage;
				}
			} catch (Exception ex) { }

			return cp;
		}

		public SiteData GetContainerSiteData(object X) {
			bFoundPage = false;
			page = null;

			SiteData sd = null;
			Page foundPage = FindPage(X);

			try {
				object obj = ReflectionUtilities.GetPropertyValue(foundPage, "theSite");

				if (foundPage != null && obj is SiteData) {
					sd = obj as SiteData;
				}
			} catch (Exception ex) { }

			return sd;
		}

		bool bFoundPage = false;
		Page page = null;
		public Page FindPage(object X) {

			if (X is Page) {
				bFoundPage = true;
				page = (Page)X;
			} else {
				if (!bFoundPage) {
					if (X is Control) {
						Control c = (Control)X;
						FindPage(c.Parent);
					}
				}
			}

			return page;
		}


		bool bFoundPlaceHolder = false;
		PlaceHolder plcholder = null;
		public PlaceHolder FindPlaceHolder(string ControlName, Control X) {

			if (X is Page) {
				bFoundPlaceHolder = false;
				plcholder = new PlaceHolder();
			}

			foreach (Control c in X.Controls) {
				if (c.ID == ControlName && c is PlaceHolder) {
					bFoundPlaceHolder = true;
					plcholder = (PlaceHolder)c;
					return plcholder;
				} else {
					if (!bFoundPlaceHolder) {
						FindPlaceHolder(ControlName, c);
					}
				}
			}

			return plcholder;
		}


	}
}