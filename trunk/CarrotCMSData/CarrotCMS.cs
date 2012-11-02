using System;
using System.Configuration;

namespace Carrotware.CMS.Data {
	public partial class CarrotCMSDataContext {

		public static CarrotCMSDataContext GetDataContext() {
			CarrotCMSDataContext _db = null;

			_db = GetDataContext(ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"].ConnectionString);

			//if (_db.Connection.ConnectionString.IndexOf("localhost") > 0) {
			//	_db = GetDataContext(ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"].ConnectionString);
			//} else {
			//    _db = new CarrotCMSDataContext();
			//}

#if DEBUG
			_db.Log = new DebugTextWriter();
#endif
			return _db;
		}


		public static CarrotCMSDataContext GetDataContext(string connection) {
			CarrotCMSDataContext _db = new CarrotCMSDataContext(connection);
#if DEBUG
			_db.Log = new DebugTextWriter();
#endif
			return _db;
		}


		public static CarrotCMSDataContext GetDataContext(System.Data.IDbConnection connection) {
			CarrotCMSDataContext _db = new CarrotCMSDataContext(connection);
#if DEBUG
			_db.Log = new DebugTextWriter();
#endif
			return _db;
		}



		//public CarrotCMSDataContext() :
		//    base(global::Carrotware.CMS.Data.Properties.Settings.Default.CarrotwareCMSConnectionString, mappingSource) {
		//    OnCreated();
		//}

		//public CarrotCMSDataContext() :
		//    base(global::System.Configuration.ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"].ConnectionString, mappingSource) {
		//    OnCreated();
		//}

	}
}
