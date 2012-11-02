using System;

namespace Carrotware.CMS.Data {
	public partial class CarrotCMSDataContext {

		public static CarrotCMSDataContext GetDataContext() {
			CarrotCMSDataContext _db = new CarrotCMSDataContext();
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

	}
}
