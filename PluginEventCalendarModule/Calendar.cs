using System.Configuration;

namespace Carrotware.CMS.UI.Plugins.EventCalendarModule {
	public partial class CalendarDataContext {

		private static string connString = ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"].ConnectionString;

		public static CalendarDataContext GetDataContext() {

			return GetDataContext(connString);
		}


		public static CalendarDataContext GetDataContext(string connection) {

			return new CalendarDataContext(connection);
		}

	}
}
