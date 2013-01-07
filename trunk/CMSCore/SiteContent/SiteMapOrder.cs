using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Carrotware.CMS.Data;
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

		public int NavOrder { get; set; }
		public Guid? Parent_ContentID { get; set; }
		public Guid Root_ContentID { get; set; }
		public string NavMenuText { get; set; }
		public bool PageActive { get; set; }
		public int NavLevel { get; set; }

		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is SiteMapOrder) {
				SiteMapOrder p = (SiteMapOrder)obj;
				return (this.Root_ContentID == p.Root_ContentID);
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return Root_ContentID.GetHashCode();
		}
	}

}
