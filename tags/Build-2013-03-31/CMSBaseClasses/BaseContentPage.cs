using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
	public class BaseContentPage : System.Web.UI.Page {

		protected PageProcessingHelper pph = new PageProcessingHelper();

		public ContentPage ThePage { get { return pageContents; } }
		public SiteData TheSite { get { return theSite; } }
		public List<Widget> ThePageWidgets { get { return pageWidgets; } }

		protected ContentPage pageContents = null;
		protected SiteData theSite = null;
		protected List<Widget> pageWidgets = null;



	}
}