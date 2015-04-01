using System.Configuration;

namespace Carrotware.CMS.UI.Plugins.CalendarModule {
	public partial class dbCalendarDataContext {

		private static string connString = ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"].ConnectionString;

		public static dbCalendarDataContext GetDataContext() {

			return GetDataContext(connString);
		}


		public static dbCalendarDataContext GetDataContext(string connection) {

			return new dbCalendarDataContext(connection);
		}

	}
}
