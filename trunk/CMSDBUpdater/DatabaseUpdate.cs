using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.DBUpdater {

	public class DatabaseUpdate {

		public static SqlException LastSQLError { get; set; }

		public static string CurrentDbVersion { get { return "20130926"; } }

		public DatabaseUpdate() {
			LastSQLError = null;
			TestDatabaseWithQuery();
		}


		public bool IsPostStep04 {
			get {
				if (!FailedSQL) {
					return SQLUpdateNugget.EvalNuggetKey("IsPostStep04");
				}
				return false;
			}
		}
		public bool IsPostStep09 {
			get {
				if (!FailedSQL) {
					return SQLUpdateNugget.EvalNuggetKey("IsPostStep09");
				}
				return false;
			}
		}
		public bool IsPostStep10 {
			get {
				if (!FailedSQL) {
					return SQLUpdateNugget.EvalNuggetKey("IsPostStep10");
				}
				return false;
			}
		}

		private void TestDatabaseWithQuery() {
			LastSQLError = null;

			string query = "select top 10 table_name, column_name, ordinal_position from [information_schema].[columns] as isc " +
					" where isc.table_name like 'carrot%' " +
					" order by isc.table_name, isc.ordinal_position, isc.column_name";

			DataTable table1 = GetTestData(query);
		}

		private static string SetConn() {
			string _connStr = String.Empty;

			if (ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"] != null) {
				ConnectionStringSettings conString = ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"];
				_connStr = conString.ConnectionString;
			}

			return _connStr;
		}

		private static string ContentKey = "cms_SiteSetUpSQLState";
		public static bool FailedSQL {
			get {
				bool c = false;
				try { c = (bool)HttpContext.Current.Cache[ContentKey]; } catch { }
				return c;
			}
			set {
				HttpContext.Current.Cache.Insert(ContentKey, value, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
			}
		}

		public static bool SystemNeedsChecking(Exception ex) {
			//assumption is database is probably empty / needs updating, so trigger the under construction view

			if (ex is SqlException && ex != null) {
				string msg = ex.Message.ToLower();
				if (ex.InnerException != null) {
					msg += "\r\n" + ex.InnerException.Message.ToLower();
				}
				if (msg.Contains("the server was not found")) {
					return false;
				}

				if (msg.Contains("invalid object name")
					//|| msg.Contains("no process is on the other end of the pipe")
					|| msg.Contains("invalid column name")
					|| msg.Contains("could not find stored procedure")
					|| msg.Contains("not found")) {

					return true;
				}
			}

			return false;
		}

		public DatabaseUpdateResponse CreateCMSDatabase() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			if (!FailedSQL) {
				bool bTestResult = SQLUpdateNugget.EvalNuggetKey("DoCMSTablesExist");

				if (!bTestResult) {
					res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.CREATE01.sql", false);
					res.Response = "Created Database";
					res.RanUpdate = true;
					SetDbSchemaVersion(DatabaseUpdate.CurrentDbVersion);
					return res;
				}

				res.Response = "Database Already Created";
				return res;
			}

			res.Response = "*** Database Access Failed ***";
			return res;
		}

		public bool DoCMSTablesExist() {
			if (!FailedSQL) {
				bool bTestResult = SQLUpdateNugget.EvalNuggetKey("DoCMSTablesExist");

				if (bTestResult) {
					return bTestResult;
				}
			}

			return false;
		}

		public List<DatabaseUpdateMessage> MergeMessages(List<DatabaseUpdateMessage> lstMsgs1, List<DatabaseUpdateMessage> lstMsgs2) {
			if (lstMsgs1 == null) {
				lstMsgs1 = new List<DatabaseUpdateMessage>();
			}

			if (lstMsgs2 == null) {
				lstMsgs2 = new List<DatabaseUpdateMessage>();
			}

			if (lstMsgs2.Count > 0) {
				int iPad = lstMsgs1.Count;
				lstMsgs2.ToList().ForEach(x => x.Order = (x.Order + iPad));

				lstMsgs1 = lstMsgs1.Union(lstMsgs2).ToList();
			}

			return lstMsgs1;
		}

		public List<DatabaseUpdateMessage> HandleResponse(List<DatabaseUpdateMessage> lstMsgs, Exception ex) {
			if (lstMsgs == null) {
				lstMsgs = new List<DatabaseUpdateMessage>();
			}

			DatabaseUpdateResponse execMessage = new DatabaseUpdateResponse();
			execMessage.LastException = ex;
			execMessage.Response = "An error occurred.";

			HandleResponse(lstMsgs, "Error: ", execMessage);

			return lstMsgs;
		}

		public List<DatabaseUpdateMessage> HandleResponse(List<DatabaseUpdateMessage> lstMsgs, string sMsg) {
			if (lstMsgs == null) {
				lstMsgs = new List<DatabaseUpdateMessage>();
			}

			HandleResponse(lstMsgs, sMsg, null);

			return lstMsgs;
		}

		public List<DatabaseUpdateMessage> HandleResponse(List<DatabaseUpdateMessage> lstMsgs, string sMsg, DatabaseUpdateResponse execMessage) {

			if (lstMsgs == null) {
				lstMsgs = new List<DatabaseUpdateMessage>();
			}

			DatabaseUpdateMessage item = new DatabaseUpdateMessage();

			if (!string.IsNullOrEmpty(sMsg)) {

				item.Message = sMsg;

				if (execMessage != null) {
					item.AlteredData = execMessage.RanUpdate;
					item.Response = execMessage.Response;

					if (execMessage.LastException != null && !string.IsNullOrEmpty(execMessage.LastException.Message)) {
						item.HasExceoption = true;
						item.ExceptionText = execMessage.LastException.Message;
						if (execMessage.LastException.InnerException != null && !string.IsNullOrEmpty(execMessage.LastException.InnerException.Message)) {
							item.InnerExceptionText = execMessage.LastException.InnerException.Message;
						}
					}
				}
			}

			item.Order = lstMsgs.Count + 1;

			lstMsgs.Add(item);

			return lstMsgs;
		}

		public string BuildUpdateString(int iCount) {
			return "Update " + (iCount).ToString() + " ";
		}

		public DatabaseUpdateStatus PerformUpdates() {
			DatabaseUpdateStatus status = new DatabaseUpdateStatus();

			bool bUpdate = true;
			List<DatabaseUpdateMessage> lst = new List<DatabaseUpdateMessage>();

			if (!DoCMSTablesExist()) {
				HandleResponse(lst, "Create Database", CreateCMSDatabase());
			} else {
				HandleResponse(lst, "Database already exists");
			}

			bUpdate = DatabaseNeedsUpdate();

			DataInfo ver = GetDbSchemaVersion();

			int iUpdate = 1;

			if (bUpdate || (ver.DataValue != DatabaseUpdate.CurrentDbVersion)) {

				if (ver.DataValue != DatabaseUpdate.CurrentDbVersion) {

					if (!IsPostStep04) {
						HandleResponse(lst, BuildUpdateString(iUpdate++), AlterStep00());
						HandleResponse(lst, BuildUpdateString(iUpdate++), AlterStep01());
						HandleResponse(lst, BuildUpdateString(iUpdate++), AlterStep02());
						HandleResponse(lst, BuildUpdateString(iUpdate++), AlterStep03());
						HandleResponse(lst, BuildUpdateString(iUpdate++), AlterStep04());
					}

					if (!IsPostStep09) {
						HandleResponse(lst, BuildUpdateString(iUpdate++), AlterStep05());
						HandleResponse(lst, BuildUpdateString(iUpdate++), AlterStep06());
						HandleResponse(lst, BuildUpdateString(iUpdate++), AlterStep07());
						HandleResponse(lst, BuildUpdateString(iUpdate++), AlterStep08());
						HandleResponse(lst, BuildUpdateString(iUpdate++), AlterStep09());
					}

					ver = GetDbSchemaVersion();

					if (ver.DataValue != DatabaseUpdate.CurrentDbVersion) {
						if (ver.DataValue.Length < 2 || ver.DataValue.StartsWith("201305") || ver.DataValue.StartsWith("201306")) {
							HandleResponse(lst, BuildUpdateString(iUpdate++), AlterStep10());
						}
						ver = GetDbSchemaVersion();
						if (ver.DataValue.Length < 2 || ver.DataValue.StartsWith("201306") || ver.DataValue.StartsWith("201309")) {
							HandleResponse(lst, BuildUpdateString(iUpdate++), AlterStep11());
						}
					}

				} else {
					HandleResponse(lst, "Database up-to-date [" + ver.DataValue + "] ");
				}

			} else {
				HandleResponse(lst, "Database up-to-date [" + ver.DataValue + "] ");
			}

			bUpdate = DatabaseNeedsUpdate();

			status.NeedsUpdate = bUpdate;
			status.Messages = lst;

			return status;
		}

		public bool TableExists(string testTableName) {
			string testQuery = "select * from [information_schema].[columns] where table_name = @TableName ";
			List<SqlParameter> parms = new List<SqlParameter>();

			SqlParameter parmKey = new SqlParameter();
			parmKey.ParameterName = "@TableName";
			parmKey.SqlDbType = SqlDbType.VarChar;
			parmKey.Size = 2000;
			parmKey.Direction = ParameterDirection.Input;
			parmKey.Value = testTableName;

			parms.Add(parmKey);

			DataTable table1 = GetTestData(testQuery, parms);

			if (table1.Rows.Count < 1) {
				return false;
			}

			return true;
		}

		public List<string> GetTableColumns(string testTableName) {
			List<string> lst = new List<string>();

			string testQuery = "select * from [information_schema].[columns] where table_name = @TableName ";

			List<SqlParameter> parms = new List<SqlParameter>();

			SqlParameter parmKey = new SqlParameter();
			parmKey.ParameterName = "@TableName";
			parmKey.SqlDbType = SqlDbType.VarChar;
			parmKey.Size = 2000;
			parmKey.Direction = ParameterDirection.Input;
			parmKey.Value = testTableName;

			parms.Add(parmKey);

			DataTable table1 = GetTestData(testQuery, parms);

			if (table1.Rows.Count > 1) {
				lst = (from d in table1.AsEnumerable()
					   select d.Field<string>("column_name")).ToList();
			}

			return lst;
		}

		public DatabaseUpdateResponse ApplyUpdateIfNotFound(string testQuery, string updateStatement, bool bIgnore) {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();
			DataTable table1 = GetTestData(testQuery);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecScriptContents(updateStatement, bIgnore);
				res.Response = "Applied update";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "Did not apply any updates";
			return res;
		}

		public DatabaseUpdateResponse ApplyUpdateIfFound(string testQuery, string updateStatement, bool bIgnore) {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();
			DataTable table1 = GetTestData(testQuery);

			if (table1.Rows.Count > 0) {
				res.LastException = ExecScriptContents(updateStatement, bIgnore);
				res.Response = "Applied update";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "Did not apply any updates";
			return res;
		}

		public static bool AreCMSTablesIncomplete() {
			if (!FailedSQL) {
				bool bTestResult = false;

				DataInfo ver = GetDbSchemaVersion();
				bTestResult = ver.DataValue != DatabaseUpdate.CurrentDbVersion;
				if (bTestResult) {
					return true;
				}

				bTestResult = SQLUpdateNugget.EvalNuggetKey("AreCMSTablesIncomplete");
				if (bTestResult) {
					return true;
				}

				bTestResult = SQLUpdateNugget.EvalNuggetKey("PreCarrotPrefix");
				if (bTestResult) {
					return true;
				}

				bTestResult = SQLUpdateNugget.EvalManditoryChecks();
				if (bTestResult) {
					return true;
				}
			}

			return false;
		}

		public bool DatabaseNeedsUpdate() {
			if (!FailedSQL) {
				bool bTestResult = false;

				DataInfo ver = GetDbSchemaVersion();
				bTestResult = ver.DataValue != DatabaseUpdate.CurrentDbVersion;
				if (bTestResult) {
					return true;
				}

				bTestResult = SQLUpdateNugget.EvalNuggetKey("DatabaseNeedsUpdate");
				if (bTestResult) {
					return true;
				}

				bTestResult = SQLUpdateNugget.EvalNuggetKey("PreCarrotPrefix");
				if (bTestResult) {
					return true;
				}

				bTestResult = SQLUpdateNugget.EvalManditoryChecks();
				if (bTestResult) {
					return true;
				}
			}

			return false;
		}

		public bool UsersExist {
			get {
				if (!FailedSQL) {
					string query = "select top 3 * from [dbo].[aspnet_Membership]";
					DataTable table1 = GetTestData(query);

					if (table1.Rows.Count > 0) {
						return true;
					}
				}

				return false;
			}
		}

		#region Pre Blog alters

		public DatabaseUpdateResponse AlterStep01() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep01");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER01.sql", false);
				res.Response = "Created Content MetaKeyword and MetaDescription";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "Content MetaKeyword and MetaDescription Already Exists";
			return res;
		}

		public DatabaseUpdateResponse AlterStep00() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep00");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER01a.sql", false);
				res.Response = "Created Table SerialCache";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "Table SerialCache Already Exists";
			return res;
		}

		public DatabaseUpdateResponse AlterStep02() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep02");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER02.sql", false);
				res.Response = "Widget Schema Updated";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "Widget Schema Already Exists";
			return res;
		}

		public DatabaseUpdateResponse AlterStep03() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep03");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER03.sql", false);
				res.Response = "RootContent CreateDate Created";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "RootContent CreateDate Already Exists";
			return res;
		}

		public DatabaseUpdateResponse AlterStep04() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep04");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER04.sql", false);
				res.Response = "CMS Table Names Changed";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "CMS Tables Already Changed";
			return res;
		}

		public DatabaseUpdateResponse AlterStep05() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep05");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER05.sql", false);
				res.Response = "CMS DB created vw_carrot_Content and vw_carrot_Widget";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "CMS DB vw_carrot_Content and vw_carrot_Widget already exist";
			return res;
		}

		#endregion

		#region Blog alters

		public DatabaseUpdateResponse AlterStep06() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep06");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER06.sql", false);
				res.Response = "CMS DB created carrot_ContentType, carrot_ContentTag, carrot_ContentCategory";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "CMS DB carrot_ContentType, carrot_ContentTag, carrot_ContentCategory already exist";
			return res;
		}

		public DatabaseUpdateResponse AlterStep07() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep07");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER07.sql", false);
				res.Response = "CMS DB created cols RetireDate, GoLiveDate, and GoLiveDateLocal in carrot_RootContent";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "CMS DB cols RetireDate, GoLiveDate, and GoLiveDateLocal in carrot_RootContent already exist";
			return res;
		}

		public DatabaseUpdateResponse AlterStep08() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep08");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER08.sql", false);
				res.Response = "CMS DB created vw_carrot_Comment";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "CMS DB vw_carrot_Comment already created";
			return res;
		}

		public DatabaseUpdateResponse AlterStep09() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep09");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER09.sql", false);
				res.Response = "CMS DB created vw_carrot_ContentChild and ShowInSiteNav";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "CMS DB vw_carrot_ContentChild and ShowInSiteNav already exist";
			return res;
		}

		#endregion

		public DatabaseUpdateResponse AlterStep10() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep10");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER10.sql", false);
				res.Response = "CMS DB created TextWidget and Content Snippet, updated edit history";
				res.RanUpdate = true;
				SetDbSchemaVersion("20130615");
				return res;
			}

			res.Response = "CMS DB TextWidget and Content Snippet already exist";
			return res;
		}

		public DatabaseUpdateResponse AlterStep11() {

			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep11");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER11.sql", false);
				res.Response = "CMS DB Updated archive tally";
				res.RanUpdate = true;
				SetDbSchemaVersion("20130926");
				return res;
			}

			res.Response = "CMS DB archive tally already updated";
			return res;
		}

		private string ReadEmbededScript(string filePath) {

			string sFile = "";

			Assembly _assembly = Assembly.GetExecutingAssembly();

			using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream(filePath))) {
				sFile = oTextStream.ReadToEnd();
			}

			return sFile;
		}

		private List<string> SplitScriptAtGo(string sSQLQuery) {
			sSQLQuery += "\r\n\r\nGO\r\n\r\n";
			sSQLQuery = sSQLQuery.Replace("\r\n", "\n");

			string[] splitcommands = sSQLQuery.Split(new string[] { "GO\n" }, StringSplitOptions.RemoveEmptyEntries);
			List<string> commandList = new List<string>(splitcommands);
			return commandList;
		}

		#region Execute SQL statements

		public Exception ExecScriptContents(string sScriptContents, bool bIgnoreErr) {
			string _connStr = SetConn();

			return ExecScriptContents(_connStr, sScriptContents, bIgnoreErr);
		}

		public Exception ExecScriptContents(string sConnectionString, string sScriptContents, bool bIgnoreErr) {

			return ExecNonQuery(sConnectionString, sScriptContents, bIgnoreErr);
		}

		private Exception ExecFileContents(string sResourceName, bool bIgnoreErr) {
			string _connStr = SetConn();

			return ExecFileContents(_connStr, sResourceName, bIgnoreErr);
		}

		private Exception ExecFileContents(string sConnectionString, string sResourceName, bool bIgnoreErr) {

			string sScriptContents = ReadEmbededScript(sResourceName);

			Exception response = ExecScriptContents(sConnectionString, sScriptContents, bIgnoreErr);

			return response;
		}

		#endregion

		#region Work with data keys

		public static DataInfo GetDbSchemaVersion() {
			return GetDataKeyValue("DBSchema");
		}

		public static void SetDbSchemaVersion(string dataKeyValue) {
			SetDataKeyValue("DBSchema", dataKeyValue);
		}

		public static DataInfo GetDataKeyValue(string dataKeyName) {
			string _connStr = SetConn();

			DataInfo d = new DataInfo();

			SQLUpdateNugget n = SQLUpdateNugget.GetNuggets("SchemaVersionCheck").FirstOrDefault();
			if (n != null) {
				List<SqlParameter> parms = new List<SqlParameter>();

				SqlParameter parmKey = new SqlParameter();
				parmKey.ParameterName = "@DataKey";
				parmKey.SqlDbType = SqlDbType.VarChar;
				parmKey.Size = 100;
				parmKey.Direction = ParameterDirection.Input;
				parmKey.Value = dataKeyName;

				parms.Add(parmKey);

				DataTable dt = ExecuteDataTableCommands(_connStr, n.SQLQuery, parms);

				if (dt.Rows.Count > 0) {
					DataRow dr = dt.Rows[0];

					d.DataKey = dr["DataKey"].ToString();
					d.DataValue = dr["DataValue"].ToString();
				}
			}

			if (d != null && string.IsNullOrEmpty(d.DataValue)) {
				d.DataValue = String.Empty;
			}

			return d;
		}

		public static void SetDataKeyValue(string dataKeyName, string dataKeyValue) {
			string _connStr = SetConn();

			SQLUpdateNugget n = SQLUpdateNugget.GetNuggets("SchemaVersionUpdate").FirstOrDefault();

			if (n != null) {
				List<SqlParameter> parms = new List<SqlParameter>();

				SqlParameter parmNewVal = new SqlParameter();
				parmNewVal.ParameterName = "@DataValue";
				parmNewVal.SqlDbType = SqlDbType.VarChar;
				parmNewVal.Size = 100;
				parmNewVal.Direction = ParameterDirection.Input;
				parmNewVal.Value = dataKeyValue;

				parms.Add(parmNewVal);

				SqlParameter parmKey = new SqlParameter();
				parmKey.ParameterName = "@DataKey";
				parmKey.SqlDbType = SqlDbType.VarChar;
				parmKey.Size = 100;
				parmKey.Direction = ParameterDirection.Input;
				parmKey.Value = dataKeyName;

				parms.Add(parmKey);

				ExecuteNonQueryCommands(_connStr, n.SQLQuery, parms);
			}
		}

		#endregion

		#region General database routines

		private Exception ExecNonQuery(string sConnectionString, string sSQLQuery, bool bIgnoreErr) {

			Exception exc = new Exception("");

			using (SqlConnection myConnection = new SqlConnection(sConnectionString)) {
				myConnection.Open();

				List<string> cmdLst = SplitScriptAtGo(sSQLQuery);

				if (!bIgnoreErr) {
					try {
						foreach (string cmdStr in cmdLst) {
							using (SqlCommand myCommand = myConnection.CreateCommand()) {
								myCommand.CommandText = cmdStr;
								myCommand.Connection = myConnection;
								myCommand.CommandTimeout = 360;
								int ret = myCommand.ExecuteNonQuery();
							}
						}
					} catch (Exception ex) {
						exc = ex;
					} finally {
						myConnection.Close();
					}
				} else {
					StringBuilder sb = new StringBuilder();
					foreach (string cmdStr in cmdLst) {
						try {
							using (SqlCommand myCommand = myConnection.CreateCommand()) {
								myCommand.CommandText = cmdStr;
								myCommand.Connection = myConnection;
								myCommand.CommandTimeout = 360;
								int ret = myCommand.ExecuteNonQuery();
							}
						} catch (Exception ex) {
							sb.Append(ex.Message.ToString() + "\n~~~~~~~~~~~~~~~~~~~~~~~~\n");
						}
					}
					exc = new Exception(sb.ToString());
					myConnection.Close();
				}
			}

			return exc;
		}

		private static void ExecuteNonQueryCommands(string sConnectionString, string sSQLQuery, List<SqlParameter> SqlParms) {
			DataTable dt = new DataTable();

			using (SqlConnection cn = new SqlConnection(sConnectionString)) {
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(sSQLQuery, cn)) {
					cmd.CommandType = CommandType.Text;

					foreach (var p in SqlParms) {
						cmd.Parameters.Add(p);
					}

					int ret = cmd.ExecuteNonQuery();
				}
				cn.Close();
			}
		}

		private static DataTable ExecuteDataTableCommands(string sConnectionString, string sSQLQuery, List<SqlParameter> SqlParms) {
			DataTable dt = new DataTable();

			using (SqlConnection cn = new SqlConnection(sConnectionString)) {
				using (SqlCommand cmd = new SqlCommand(sSQLQuery, cn)) {
					cn.Open();
					cmd.CommandType = CommandType.Text;

					if (SqlParms != null) {
						foreach (var p in SqlParms) {
							cmd.Parameters.Add(p);
						}
					}

					using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
						da.Fill(dt);
					}
				}
				cn.Close();
			}

			return dt;
		}

		public static DataTable GetDataTable(string sSQLQuery) {
			string _connStr = SetConn();

			return GetDataTable(_connStr, sSQLQuery);
		}

		private static DataTable GetDataTable(string sConnectionString, string sSQLQuery) {
			DataTable dt = new DataTable();

			using (SqlConnection cn = new SqlConnection(sConnectionString)) {
				using (SqlCommand cmd = new SqlCommand(sSQLQuery, cn)) {
					cn.Open();
					cmd.CommandType = CommandType.Text;
					using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
						da.Fill(dt);
					}
					cn.Close();
				}
			}

			return dt;
		}

		public static DataTable GetTestData(string sSQLQuery) {
			return GetTestData(sSQLQuery, null);
		}

		public static DataTable GetTestData(string sSQLQuery, List<SqlParameter> SqlParms) {
			string _connStr = SetConn();

			return GetTestData(_connStr, sSQLQuery, SqlParms);
		}

		public static DataTable GetTestData(string sConnectionString, string sSQLQuery, List<SqlParameter> SqlParms) {
			DataTable dt = new DataTable();

			try {

				using (SqlConnection cn = new SqlConnection(sConnectionString)) {
					cn.Open(); // throws if invalid

					FailedSQL = false;

					using (SqlCommand cmd = cn.CreateCommand()) {
						cmd.CommandText = sSQLQuery;

						if (SqlParms != null) {
							foreach (var p in SqlParms) {
								cmd.Parameters.Add(p);
							}
						}

						using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
							da.Fill(dt);
						}
					}

					cn.Close();
				}
				LastSQLError = null;
			} catch (SqlException sqlEx) {
				LastSQLError = sqlEx;
				FailedSQL = true;
			}

			return dt;
		}

		private static DataSet GetDataSet(string sSQLQuery) {
			string _connStr = SetConn();

			return GetDataSet(_connStr, sSQLQuery);
		}

		private static DataSet GetDataSet(string sConnectionString, string sSQLQuery) {
			DataSet ds = new DataSet();

			using (SqlConnection cn = new SqlConnection(sConnectionString)) {
				using (SqlCommand cmd = new SqlCommand(sSQLQuery, cn)) {
					cn.Open();
					cmd.CommandType = CommandType.Text;
					using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
						da.Fill(ds);
					}
					cn.Close();
				}
			}

			return ds;
		}

		#endregion
	}

	//======================
	public class DatabaseUpdateStatus {
		public bool NeedsUpdate { get; set; }

		public List<DatabaseUpdateMessage> Messages { get; set; }


		public DatabaseUpdateStatus() {
			this.Messages = new List<DatabaseUpdateMessage>();
			this.NeedsUpdate = true;
		}
	}

	//======================
	public class DataInfo {

		public string DataKey { get; set; }
		public string DataValue { get; set; }
	}

	//======================
	public class DatabaseUpdateMessage {

		public string Message { get; set; }
		public string ExceptionText { get; set; }
		public string InnerExceptionText { get; set; }
		public string Response { get; set; }
		public int Order { get; set; }
		public bool AlteredData { get; set; }
		public bool HasExceoption { get; set; }

		public DatabaseUpdateMessage() {
			this.ExceptionText = null;
			this.InnerExceptionText = null;
			this.AlteredData = false;
			this.HasExceoption = false;
			this.Message = "";
			this.Response = "";
			this.Order = -1;
		}
	}

	//======================
	public class DatabaseUpdateResponse {

		public Exception LastException { get; set; }
		public string Response { get; set; }
		public bool RanUpdate { get; set; }

		public DatabaseUpdateResponse() {
			this.LastException = null;
			this.Response = "";
			this.RanUpdate = false;
		}
	}
}
