using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carrotware.CMS.Core {
	public class PageViewType {

		public enum ViewType {
			SinglePage,
			SearchResults,
			DateIndex,
			DateMonthIndex,
			DateDayIndex,
			DateYearIndex,
			TagIndex,
			CategoryIndex,
		}

		public ViewType CurrentViewType { get; set; }

		public string ExtraTitle { get; set; }

	}
}
