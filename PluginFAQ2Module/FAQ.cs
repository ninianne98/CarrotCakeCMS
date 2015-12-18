using System.Configuration;

namespace Carrotware.CMS.UI.Plugins.FAQ2Module {
	partial class FAQDataContext {

		private static string connString = ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"].ConnectionString;

		public static FAQDataContext GetDataContext() {

			return GetDataContext(connString);
		}


		public static FAQDataContext GetDataContext(string connection) {

			return new FAQDataContext(connection);
		}

	}
}
