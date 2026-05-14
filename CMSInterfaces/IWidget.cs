using System;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.Interface {

	public interface IWidget {
		Guid PageWidgetID { get; set; }

		Guid RootContentID { get; set; }

		Guid SiteID { get; set; }

		string JSEditFunction { get; }

		bool EnableEdit { get; }
	}
}