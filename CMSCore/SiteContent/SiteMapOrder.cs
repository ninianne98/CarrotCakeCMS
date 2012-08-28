using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carrotware.CMS.Data;
using System.Text.RegularExpressions;
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

	public class SiteMapOrder {

		public SiteMapOrder() { }

		public int? NavOrder { get; set; }
		public Guid? Parent_ContentID { get; set; }
		public Guid Root_ContentID { get; set; }
		public string NavMenuText { get; set; }
		public bool PageActive { get; set; }
		public int NavLevel { get; set; }

	}

}
