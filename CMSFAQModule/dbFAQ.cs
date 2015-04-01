using System.Configuration;

namespace Carrotware.CMS.UI.Plugins.FAQModule {
	public partial class dbFAQDataContext {

		private static string connString = ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"].ConnectionString;

		public static dbFAQDataContext GetDataContext() {

			return GetDataContext(connString);
		}


		public static dbFAQDataContext GetDataContext(string connection) {

			return new dbFAQDataContext(connection);
		}

	}
}
