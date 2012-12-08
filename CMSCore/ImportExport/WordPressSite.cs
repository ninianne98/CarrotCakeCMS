using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

	public class WordPressSite {
		public WordPressSite() { }

		public Guid NewSiteID { get; set; }

		public string SiteTitle { get; set; }
		public string SiteDescription { get; set; }
		public string SiteURL { get; set; }

		public string ImportSource { get; set; }
		public string wxrVersion { get; set; }

		public DateTime ExtractDate { get; set; }

		public List<InfoKVP> Categories { get; set; }
		public List<InfoKVP> Tags { get; set; }

		public List<WordPressPost> Content { get; set; }

		public override string ToString() {
			return SiteTitle + " : " + SiteDescription;
		}
	}
}
