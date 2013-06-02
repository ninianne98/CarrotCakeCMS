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

		public static string CurrentDbVersion { get { return "20130531"; } }

		private static string ConnectionString { get; set; }

		public DatabaseUpdate() {
			ConnectionString = "";
			LastSQLError = null;
			SetConn();
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

		private static void SetConn() {
			if (string.IsNullOrEmpty(ConnectionString)) {
				ConnectionString = "";
				if (ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"] != null) {
					ConnectionStringSettings conString = ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"];
					ConnectionString = conString.ConnectionString;
				}
			}
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

			//if ((ex is SqlException || ex is HttpUnhandledException) && ex != null) {
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
					res.Response = "Created Database *";
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
					item.Response = execMessage.Response;
					if (execMessage.LastException != null && !string.IsNullOrEmpty(execMessage.LastException.Message)) {
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

		public DatabaseUpdateStatus PerformUpdates() {
			DatabaseUpdateStatus status = new DatabaseUpdateStatus();

			bool bUpdate = true;
			List<DatabaseUpdateMessage> lst = new List<DatabaseUpdateMessage>();

			if (!DoCMSTablesExist()) {
				HandleResponse(lst, "Create Database *", CreateCMSDatabase());
				bUpdate = false;
			} else {
				HandleResponse(lst, "Database already exists");
			}

			bUpdate = DatabaseNeedsUpdate();

			int iUpdate = 1;

			if (bUpdate) {

				DataInfo ver = GetDbSchemaVersion();

				if (ver.DataValue != DatabaseUpdate.CurrentDbVersion) {

					if (!IsPostStep04) {
						HandleResponse(lst, "Update  " + (iUpdate++).ToString() + " ", AlterStep00());
						HandleResponse(lst, "Update  " + (iUpdate++).ToString() + " ", AlterStep01());
						HandleResponse(lst, "Update  " + (iUpdate++).ToString() + " ", AlterStep02());
						HandleResponse(lst, "Update  " + (iUpdate++).ToString() + " ", AlterStep03());
						HandleResponse(lst, "Update  " + (iUpdate++).ToString() + " ", AlterStep04());
					}

					if (!IsPostStep10) {
						HandleResponse(lst, "Update  " + (iUpdate++).ToString() + " ", AlterStep05());
						HandleResponse(lst, "Update  " + (iUpdate++).ToString() + " ", AlterStep06());
						HandleResponse(lst, "Update  " + (iUpdate++).ToString() + " ", AlterStep07());
						HandleResponse(lst, "Update  " + (iUpdate++).ToString() + " ", AlterStep08());
						HandleResponse(lst, "Update  " + (iUpdate++).ToString() + " ", AlterStep09());
						HandleResponse(lst, "Update  " + (iUpdate++).ToString() + " ", AlterStep10());
					}

				} else {
					HandleResponse(lst, "Database up-to-date [" + ver.DataValue + "] ");
				}

			} else {
				HandleResponse(lst, "Database up-to-date ");
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
			parmKey.Size = 1000;
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
			parmKey.Size = 1000;
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

		public DatabaseUpdateResponse ApplyUpdateIfNotFound(string testQuery, string updateStatement, bool bIg) {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();
			DataTable table1 = GetTestData(testQuery);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecScriptContents(updateStatement, bIg);
				res.Response = "Applied update *";
				return res;
			}

			res.Response = "Did not apply update";
			return res;
		}

		public DatabaseUpdateResponse ApplyUpdateIfFound(string testQuery, string updateStatement, bool bIg) {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();
			DataTable table1 = GetTestData(testQuery);

			if (table1.Rows.Count > 0) {
				res.LastException = ExecScriptContents(updateStatement, bIg);
				res.Response = "Applied update *";
				return res;
			}

			res.Response = "Did not apply update";
			return res;
		}

		public static bool AreCMSTablesIncomplete() {
			if (!FailedSQL) {
				bool bTestResult = false;

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
					string query = "select top 5 * from [aspnet_Membership]";
					DataTable table1 = GetTestData(query);

					if (table1.Rows.Count > 0) {
						return true;
					}
				}

				return false;
			}
		}

		public DatabaseUpdateResponse AlterStep01() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep01");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER01.sql", false);
				res.Response = "Created Content MetaKeyword and MetaDescription *";
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
				res.Response = "Created Table SerialCache *";
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
				res.Response = "Widget Schema Updated *";
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
				res.Response = "RootContent CreateDate Created *";
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
				res.Response = "CMS Table Names Changed *";
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
				res.Response = "CMS DB created vw_carrot_Content and vw_carrot_Widget *";
				return res;
			}

			res.Response = "CMS DB vw_carrot_Content and vw_carrot_Widget already exist";
			return res;
		}


		public DatabaseUpdateResponse AlterStep06() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep06");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER06.sql", false);
				res.Response = "CMS DB created carrot_ContentType, carrot_ContentTag, carrot_ContentCategory *";
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
				res.Response = "CMS DB created cols RetireDate, GoLiveDate, and GoLiveDateLocal in carrot_RootContent *";
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
				res.Response = "CMS DB created vw_carrot_Comment *";
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
				res.Response = "CMS DB created vw_carrot_ContentChild and ShowInSiteNav *";
				return res;
			}

			res.Response = "CMS DB vw_carrot_ContentChild and ShowInSiteNav already exist ";
			return res;
		}

		public DatabaseUpdateResponse AlterStep10() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep10");

			if (bTestResult) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER10.sql", false);
				res.Response = "CMS DB created TextWidget *";
				SetDbSchemaVersion("20130531");
				return res;
			}

			res.Response = "CMS DB TextWidget already exist ";
			return res;
		}

		public Exception ExecScriptContents(string sScriptContents, bool bIgnoreErr) {
			SetConn();

			Exception response = ExecNonQuery(ConnectionString, sScriptContents, bIgnoreErr);

			return response;

		}

		private Exception ExecFileContents(string sResourceName, bool bIgnoreErr) {
			SetConn();

			string sScriptContents = ReadEmbededScript(sResourceName);

			Exception response = ExecScriptContents(sScriptContents, bIgnoreErr);

			return response;

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


		public DataInfo GetDbSchemaVersion() {
			return GetDataKeyValue("DBSchema");
		}

		public void SetDbSchemaVersion(string dataKeyValue) {
			SetDataKeyValue("DBSchema", dataKeyValue);
		}

		public DataInfo GetDataKeyValue(string dataKeyName) {
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

				DataTable dt = ExecuteDataTableCommands(ConnectionString, n.SQLQuery, parms);

				if (dt.Rows.Count > 0) {
					DataRow dr = dt.Rows[0];

					d.DataKey = dr["DataKey"].ToString();
					d.DataValue = dr["DataValue"].ToString();
				}
			}

			return d;
		}

		public void SetDataKeyValue(string dataKeyName, string dataKeyValue) {
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

				ExecuteNonQueryCommands(ConnectionString, n.SQLQuery, parms);
			}
		}

		private void ExecuteNonQueryCommands(string sConnectionString, string sSQLQuery, List<SqlParameter> SqlParms) {
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

		private DataTable ExecuteDataTableCommands(string sConnectionString, string sSQLQuery, List<SqlParameter> SqlParms) {
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

		private DataTable GetDataTable(string sSQLQuery) {

			return GetDataTable(ConnectionString, sSQLQuery);
		}

		private DataTable GetDataTable(string sConnectionString, string sSQLQuery) {
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

		private DataSet GetDataSet(string sConnectionString, string sSQLQuery) {
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

		public static DataTable GetTestData(string sQuery) {
			return GetTestData(sQuery, null);
		}

		public static DataTable GetTestData(string sQuery, List<SqlParameter> SqlParms) {
			DataTable dt = new DataTable();
			try {
				SetConn();

				using (SqlConnection cn = new SqlConnection(ConnectionString)) {
					cn.Open(); // throws if invalid

					FailedSQL = false;

					using (SqlCommand cmd = cn.CreateCommand()) {
						cmd.CommandText = sQuery;

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

		private Exception ExecNonQuery(string sConnectionString, string sSQLQuery, bool bIgnoreErr) {
			string sConnString = sConnectionString;

			Exception exc = new Exception("");

			using (SqlConnection myConnection = new SqlConnection(sConnString)) {
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
	}

	//======================
	public class DatabaseUpdateStatus {
		public bool NeedsUpdate { get; set; }

		public List<DatabaseUpdateMessage> Messages { get; set; }


		public DatabaseUpdateStatus() {
			this.Messages = new List<DatabaseUpdateMessage>();
			NeedsUpdate = true;
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

		public DatabaseUpdateMessage() {
			this.ExceptionText = null;
			this.InnerExceptionText = null;
			this.Message = "";
			this.Response = "";
			this.Order = -1;
		}
	}

	//======================
	public class DatabaseUpdateResponse {

		public Exception LastException { get; set; }
		public string Response { get; set; }

		public DatabaseUpdateResponse() {
			LastException = null;
			Response = "";
		}
	}
}
