using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

#if DEBUG
		private static Stopwatch ThisWatch = new Stopwatch();

		private static void Connection_StateChange(object sender, StateChangeEventArgs e) {
			if (e.OriginalState == ConnectionState.Closed && e.CurrentState == ConnectionState.Open) {
				ThisWatch.Reset();
				ThisWatch.Start();
			} else if (e.OriginalState == ConnectionState.Open && e.CurrentState == ConnectionState.Closed) {
				ThisWatch.Stop();
				Debug.Write("--------------------------------------\r\n");
				Debug.Write(string.Format("SQL took {0}ms \r\n", ThisWatch.ElapsedMilliseconds));
			}
		}
#endif



		private static string connString = ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"].ConnectionString;

		public static CarrotCMSDataContext GetDataContext() {

			return GetDataContext(connString);
		}


		public static CarrotCMSDataContext GetDataContext(string connection) {
#if DEBUG
			CarrotCMSDataContext _db = new CarrotCMSDataContext(connection);
			if (Debugger.IsAttached) {
				string sKey = Guid.NewGuid().ToString();

				_db.Connection.StateChange += Connection_StateChange;
				_db.Log = new DebugTextWriter();
			}
			return _db;
#endif
			return new CarrotCMSDataContext(connection);
		}


		public static CarrotCMSDataContext GetDataContext(IDbConnection connection) {
#if DEBUG
			CarrotCMSDataContext _db = new CarrotCMSDataContext(connection);
			if (Debugger.IsAttached) {
				string sKey = Guid.NewGuid().ToString();

				_db.Connection.StateChange += Connection_StateChange;
				_db.Log = new DebugTextWriter();
			}
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
