namespace Carrotware.CMS.Core {

	public class PageViewType {

		public PageViewType() { }

		public PageViewType(ViewType type) {
			this.CurrentViewType = type;
			this.ExtraTitle = string.Empty;
			this.RawValue = null;
		}

		public PageViewType(ViewType type, string extraTitle) {
			this.CurrentViewType = type;
			this.ExtraTitle = extraTitle;
			this.RawValue = null;
		}

		public PageViewType(ViewType type, string extraTitle, object value) {
			this.CurrentViewType = type;
			this.ExtraTitle = extraTitle;
			this.RawValue = value;
		}

		public enum ViewType {
			SinglePage,
			SearchResults,
			AuthorIndex,
			DateIndex,
			DateMonthIndex,
			DateDayIndex,
			DateYearIndex,
			TagIndex,
			CategoryIndex,
		}

		public ViewType CurrentViewType { get; set; }

		public string ExtraTitle { get; set; }

		public object RawValue { get; set; }
	}
}