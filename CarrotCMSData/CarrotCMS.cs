using System;
using System.Configuration;
using System.Data;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Data {
	public partial class CarrotCMSDataContext {

		private static string connString = ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"].ConnectionString;

		public static CarrotCMSDataContext GetDataContext() {

			return GetDataContext(connString);
		}


		public static CarrotCMSDataContext GetDataContext(string connection) {
#if DEBUG
			CarrotCMSDataContext _db = new CarrotCMSDataContext(connection);
			_db.Log = new DebugTextWriter();
			return _db;
#endif
			return new CarrotCMSDataContext(connection);
		}


		public static CarrotCMSDataContext GetDataContext(IDbConnection connection) {
#if DEBUG
			CarrotCMSDataContext _db = new CarrotCMSDataContext(connection);
			_db.Log = new DebugTextWriter();
			return _db;
#endif
			return new CarrotCMSDataContext(connection);
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
