using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.DBUpdater {

	public class DatabaseUpdate {

		public DatabaseUpdate() { }

		public DatabaseUpdate(bool clearTest) {
			if (clearTest) {
				ClearTest();
			}
		}

		public void ClearTest() {
			_usersexist = false;
			_step04 = false;
			_step09 = false;
			_step10 = false;

			DatabaseSchemaState.LastSQLError = null;
			DatabaseSchemaState.ResetSQLState();
			TestDatabaseWithQuery();
		}

		private bool _step04 = false;

		public bool IsPostStep04 {
			get {
				if (!DatabaseSchemaState.FailedSQL && !_step04) {
					_step04 = SQLUpdateNugget.EvalNuggetKey("IsPostStep04");
				}
				return _step04;
			}
		}

		private bool _step09 = false;

		public bool IsPostStep09 {
			get {
				if (!DatabaseSchemaState.FailedSQL && !_step09) {
					_step09 = SQLUpdateNugget.EvalNuggetKey("IsPostStep09");
				}
				return _step09;
			}
		}

		private bool _step10 = false;

		public bool IsPostStep10 {
			get {
				if (!DatabaseSchemaState.FailedSQL && !_step10) {
					_step10 = SQLUpdateNugget.EvalNuggetKey("IsPostStep10");
				}
				return _step10;
			}
		}

		private void TestDatabaseWithQuery() {
			DatabaseSchemaState.LastSQLError = null;

			string query = "select top 10 table_name, column_name, ordinal_position from [INFORMATION_SCHEMA].[COLUMNS] as isc \n" +
					" where isc.table_name like 'carrot%' \n" +
					" order by isc.table_name, isc.ordinal_position, isc.column_name";

			DataTable table1 = DatabaseSchemaState.GetTestData(query);
		}

		public List<DatabaseUpdateResponse> CreateCMSDatabase() {
			var res = new List<DatabaseUpdateResponse>();

			if (!DatabaseSchemaState.FailedSQL) {
				bool bDbResult = SQLUpdateNugget.EvalNuggetKey("DoCMSTablesExist");
				bool bAuthResult = SQLUpdateNugget.EvalNuggetKey("DoAuthTablesExist");

				var res1 = new DatabaseUpdateResponse();

				if (!bAuthResult) {
					res1.LastException = ExecFileContents("MEMBER01.sql", false);
					res1.Response = "Created Membership";
					res1.RanUpdate = true;
				} else {
					res1.Response = "Membership Already Created";
				}
				res.Add(res1);

				var res2 = new DatabaseUpdateResponse();

				if (!bDbResult) {
					res2.LastException = ExecFileContents("CREATE01.sql", false);
					res2.Response = "Created Database";
					res2.RanUpdate = true;
					// change version key when the DB creation is re-scripted
					DatabaseSchemaState.SetDbSchemaVersion(DatabaseSchemaState.DbVersion20);
				} else {
					res2.Response = "Database Already Created";
				}
				res.Add(res2);

				return res;
			}

			var res3 = new DatabaseUpdateResponse();
			res3.Response = "*** Database Access Failed ***";
			res3.LastException = new ApplicationException(res3.Response);
			res.Add(res3);

			return res;
		}

		public bool DoCMSTablesExist() {
			if (!DatabaseSchemaState.FailedSQL) {
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

			if (lstMsgs2.Any()) {
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

			var execMessage = new DatabaseUpdateResponse();

			HandleResponse(lstMsgs, sMsg, execMessage);

			return lstMsgs;
		}

		public List<DatabaseUpdateMessage> HandleResponse(List<DatabaseUpdateMessage> lstMsgs, string sMsg, List<DatabaseUpdateResponse> lstExecMessages) {
			if (lstMsgs == null) {
				lstMsgs = new List<DatabaseUpdateMessage>();
			}

			int m = 1;
			if (lstExecMessages != null) {
				foreach (var msg in lstExecMessages) {
					lstMsgs = HandleResponse(lstMsgs, string.Format("{0}  [{1}]", sMsg, m), msg);
					m++;
				}
			}

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
						DatabaseSchemaState.WriteDebugException("handleresponse", execMessage.LastException);

						item.HasException = true;
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

		public List<DatabaseUpdateMessage> ResponseVersion(List<DatabaseUpdateMessage> lstMsgs) {
			var ver = DatabaseSchemaState.GetDbSchemaVersion();

			string sMsg = "Database version [" + ver.DataValue + "] ";

			if (ver.IsLatest()) {
				sMsg = "Database up-to-date [" + ver.DataValue + "] ";
			}

			if (lstMsgs == null) {
				lstMsgs = new List<DatabaseUpdateMessage>();
			}

			var execMessage = new DatabaseUpdateResponse();

			HandleResponse(lstMsgs, sMsg, execMessage);

			return lstMsgs;
		}

		public string BuildUpdateString(int iCount) {
			return "Update " + (iCount).ToString() + " ";
		}

		private static object _updateLocker = new object();

		public DatabaseUpdateStatus PerformUpdates() {
			DatabaseUpdateStatus status = new DatabaseUpdateStatus();
			bool update = true;
			var lst = new List<DatabaseUpdateMessage>();

			lock (_updateLocker) {
				var doTablesExist = DoCMSTablesExist();
				var ver = DatabaseSchemaState.GetDbSchemaVersion();

				if (ver.IsLatest() == false || ver.IsBlank()) {
					if (!doTablesExist) {
						HandleResponse(lst, "Create Database", CreateCMSDatabase());
					} else {
						HandleResponse(lst, "Database already exists");
					}
				}

				var needsUpdate = DatabaseNeedsUpdate();
				ver.GetDbSchema();
				update = needsUpdate && ver.IsLatest() == false;

				int updateCount = 1;

				if (update) {
					if (doTablesExist && !this.IsPostStep10) {
						if (!this.IsPostStep04) {
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep00());
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep01());
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep02());
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep03());
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep04());
						}

						if (!this.IsPostStep09) {
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep05());
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep06());
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep07());
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep08());
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep09());
						}
					}

					ver.GetDbSchema();

					if (ver.IsLatest() == false || ver.IsBlank()) {
						if (ver.IsBlank() || ver.IsYearOf("2013")) {
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep10());
							ver.GetDbSchema();
						}
						if (ver.Matches(DatabaseSchemaState.DbVersion10) || ver.IsYearOf("2013")) {
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep11());
							ver.GetDbSchema();
						}
						if (ver.Matches(DatabaseSchemaState.DbVersion11) || ver.IsYearOf("2013") || ver.IsYearOf("2014")) {
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep12());
							ver.GetDbSchema();
						}
						if (ver.Matches(DatabaseSchemaState.DbVersion12) || ver.IsYearOf("2014") || ver.IsYearOf("2015")) {
							HandleResponse(lst, BuildUpdateString(updateCount++), AlterStep13());
							ver.GetDbSchema();
						}

						if (ver.Matches(DatabaseSchemaState.DbVersion13) || ver.IsYearOf("2020")
									|| ver.IsYearOf("2025") || ver.IsYearOf("2026")) {
							HandleResponse(lst, BuildUpdateString(updateCount++), Migrate45());
							ver.GetDbSchema();
						}
					}
				}

				ResponseVersion(lst);

				DatabaseSchemaState.ResetFailedSQL();

				DatabaseSchemaState.ResetSQLState();

				needsUpdate = DatabaseNeedsUpdate();
				ver.GetDbSchema();
				update = needsUpdate && ver.IsLatest() == false;

				status.NeedsUpdate = update;
				status.Messages = lst;
			}

			return status;
		}

		public bool TableExists(string testTableName) {
			string testQuery = "select * from [INFORMATION_SCHEMA].[COLUMNS] where table_name = @TableName ";
			List<SqlParameter> parms = new List<SqlParameter>();

			SqlParameter parmKey = new SqlParameter();
			parmKey.ParameterName = "@TableName";
			parmKey.SqlDbType = SqlDbType.VarChar;
			parmKey.Size = 2000;
			parmKey.Direction = ParameterDirection.Input;
			parmKey.Value = testTableName;

			parms.Add(parmKey);

			DataTable table1 = DatabaseSchemaState.GetTestData(testQuery, parms);

			if (table1.Rows.Count < 1) {
				return false;
			}

			return true;
		}

		public List<string> GetTableColumns(string testTableName) {
			List<string> lst = new List<string>();

			string testQuery = "select * from [INFORMATION_SCHEMA].[COLUMNS] where table_name = @TableName ";

			List<SqlParameter> parms = new List<SqlParameter>();

			SqlParameter parmKey = new SqlParameter();
			parmKey.ParameterName = "@TableName";
			parmKey.SqlDbType = SqlDbType.VarChar;
			parmKey.Size = 2000;
			parmKey.Direction = ParameterDirection.Input;
			parmKey.Value = testTableName;

			parms.Add(parmKey);

			DataTable table1 = DatabaseSchemaState.GetTestData(testQuery, parms);

			if (table1.Rows.Count > 1) {
				lst = (from d in table1.AsEnumerable()
					   select d.Field<string>("column_name")).ToList();
			}

			return lst;
		}

		public DatabaseUpdateResponse ApplyUpdateIfNotFound(string testQuery, string updateStatement, bool bIgnore) {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();
			DataTable table1 = DatabaseSchemaState.GetTestData(testQuery);

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
			DataTable table1 = DatabaseSchemaState.GetTestData(testQuery);

			if (table1.Rows.Count > 0) {
				res.LastException = ExecScriptContents(updateStatement, bIgnore);
				res.Response = "Applied update";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "Did not apply any updates";
			return res;
		}

		public bool DatabaseNeedsUpdate() {
			if (!DatabaseSchemaState.FailedSQL) {
				var ver = DatabaseSchemaState.GetDbSchemaVersion();
				if (ver.IsBlank() || ver.IsLatest() == false) {
					return true;
				}
				if (ver.IsLatest()) {
					return false;
				}
				if (SQLUpdateNugget.EvalNuggetKey("AreCMSTablesIncomplete")) {
					return true;
				}
				if (SQLUpdateNugget.EvalNuggetKey("PreCarrotPrefix")) {
					return true;
				}
				if (SQLUpdateNugget.EvalManditoryChecks()) {
					return true;
				}
			}
			return false;
		}

		private bool _usersexist = false;

		public bool DoUsersExist() {
			if (!DatabaseSchemaState.FailedSQL && !_usersexist) {
				try {
					_usersexist = SQLUpdateNugget.EvalNuggetKey("DoUsersExist");
				} catch (Exception ex) {
					DatabaseSchemaState.WriteDebugException("usersexist", ex);
				}
			}
			return _usersexist;
		}

		#region Pre Blog alters

		public DatabaseUpdateResponse AlterStep01() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep01");

			if (bTestResult) {
				res.LastException = ExecFileContents("ALTER01.sql", false);
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
				res.LastException = ExecFileContents("ALTER01a.sql", false);
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
				res.LastException = ExecFileContents("ALTER02.sql", false);
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
				res.LastException = ExecFileContents("ALTER03.sql", false);
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
				res.LastException = ExecFileContents("ALTER04.sql", false);
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
				res.LastException = ExecFileContents("ALTER05.sql", false);
				res.Response = "CMS DB created vw_carrot_Content and vw_carrot_Widget";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "CMS DB vw_carrot_Content and vw_carrot_Widget already exist";
			return res;
		}

		#endregion Pre Blog alters

		#region Blog alters

		public DatabaseUpdateResponse AlterStep06() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep06");

			if (bTestResult) {
				res.LastException = ExecFileContents("ALTER06.sql", false);
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
				res.LastException = ExecFileContents("ALTER07.sql", false);
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
				res.LastException = ExecFileContents("ALTER08.sql", false);
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
				res.LastException = ExecFileContents("ALTER09.sql", false);
				res.Response = "CMS DB created vw_carrot_ContentChild and ShowInSiteNav";
				res.RanUpdate = true;
				return res;
			}

			res.Response = "CMS DB vw_carrot_ContentChild and ShowInSiteNav already exist";
			return res;
		}

		#endregion Blog alters

		public DatabaseUpdateResponse AlterStep10() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep10");

			if (bTestResult) {
				res.LastException = ExecFileContents("ALTER10.sql", false);
				res.Response = "CMS DB created TextWidget and Content Snippet, updated edit history";
				res.RanUpdate = true;
				DatabaseSchemaState.SetDbSchemaVersion(DatabaseSchemaState.DbVersion10);
				return res;
			}

			res.Response = "CMS DB TextWidget and Content Snippet already exist";
			return res;
		}

		public DatabaseUpdateResponse AlterStep11() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep11");

			if (bTestResult) {
				res.LastException = ExecFileContents("ALTER11.sql", false);
				res.Response = "CMS DB Updated archive tally";
				res.RanUpdate = true;
				DatabaseSchemaState.SetDbSchemaVersion(DatabaseSchemaState.DbVersion11);
				return res;
			}

			res.Response = "CMS DB archive tally already updated";
			return res;
		}

		public DatabaseUpdateResponse AlterStep12() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep12");
			var ver = DatabaseSchemaState.GetDbSchemaVersion();
			var minorUpdate = ver.IsMinorOf(DatabaseSchemaState.DbVersion12);

			if (bTestResult || minorUpdate) {
				res.LastException = ExecFileContents("ALTER12.sql", false);
				res.Response = minorUpdate ? "Reapply time zone sproc and tally updates"
											: "CMS DB Updated time zone sproc and tallies";
				res.RanUpdate = true;
				DatabaseSchemaState.SetDbSchemaVersion(DatabaseSchemaState.DbVersion12);
				return res;
			}

			res.Response = "CMS DB time zone sproc already updated";
			return res;
		}

		public DatabaseUpdateResponse AlterStep13() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("AlterStep13");
			var ver = DatabaseSchemaState.GetDbSchemaVersion();
			var minorUpdate = ver.IsMinorOf(DatabaseSchemaState.DbVersion13);

			if (bTestResult || minorUpdate) {
				res.LastException = ExecFileContents("ALTER13.sql", false);
				res.Response = minorUpdate ? "Reapply update timezone sproc"
											: "Update timezone sproc";
				res.RanUpdate = true;
				DatabaseSchemaState.SetDbSchemaVersion(DatabaseSchemaState.DbVersion13);
				return res;
			}

			res.Response = "Timezone sproc update already applied";
			return res;
		}

		public DatabaseUpdateResponse Migrate45() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			bool bTestResult = SQLUpdateNugget.EvalNuggetKey("Migrate45");
			var ver = DatabaseSchemaState.GetDbSchemaVersion();
			var minorUpdate = ver.IsMinorOf(DatabaseSchemaState.DbVersion20);

			if (bTestResult || minorUpdate) {
				res.LastException = ExecFileContents("MIGRATE01.sql", false);
				res.Response = minorUpdate ? "Reapply Schema update for Framework 4.5 / Owin"
										: "Migrated Schema to Framework 4.5 / Owin";
				res.RanUpdate = true;
				DatabaseSchemaState.SetDbSchemaVersion(DatabaseSchemaState.DbVersion20);
				return res;
			}

			res.Response = "Migration to Schema Framework 4.5 / Owin already applied";
			return res;
		}

		private List<string> SplitScriptAtGo(string sqlQuery) {
			sqlQuery += "\r\n\r\nGO\r\n\r\n";
			sqlQuery = sqlQuery.Replace("\r\n", "\n");

			string[] splitcommands = sqlQuery.Split(new string[] { "GO\n" }, StringSplitOptions.RemoveEmptyEntries);
			List<string> commandList = new List<string>(splitcommands);
			return commandList;
		}

		#region Execute SQL statements

		public Exception ExecScriptContents(string sScriptContents, bool bIgnoreErr) {
			string _connStr = DatabaseSchemaState.SetConn();

			return ExecScriptContents(_connStr, sScriptContents, bIgnoreErr);
		}

		public Exception ExecScriptContents(string connectionString, string sScriptContents, bool bIgnoreErr) {
			return ExecNonQuery(connectionString, sScriptContents, bIgnoreErr);
		}

		private Exception ExecFileContents(string resourceName, bool bIgnoreErr) {
			string _connStr = DatabaseSchemaState.SetConn();

			return ExecFileContents(_connStr, resourceName, bIgnoreErr);
		}

		private Exception ExecFileContents(string connectionString, string resourceName, bool bIgnoreErr) {
			string scriptContents = DatabaseSchemaState.ReadEmbededScript(resourceName);

			Exception response = ExecScriptContents(connectionString, scriptContents, bIgnoreErr);

			return response;
		}

		#endregion Execute SQL statements

		#region General database routines

		private Exception ExecNonQuery(string connectionString, string sqlQuery, bool bIgnoreErr) {
			var exc = new Exception("");
			var sb = new StringBuilder();

			using (SqlConnection cn = new SqlConnection(connectionString)) {
				List<string> cmdLst = SplitScriptAtGo(sqlQuery);

				foreach (string cmdStr in cmdLst) {
					cn.Open();

					try {
						using (SqlCommand cmd = cn.CreateCommand()) {
							cmd.CommandText = cmdStr;
							cmd.Connection = cn;
							cmd.CommandTimeout = 360;
							int ret = cmd.ExecuteNonQuery();
						}
					} catch (Exception ex) {
						exc = ex;
						if (!bIgnoreErr) {
							var extxt = ex.CombineMessage();
							sb.AppendLine("~~~~~~~~~~~~~~~~~~~~~~~~");
							sb.AppendLine(extxt);
						}
						DatabaseSchemaState.WriteDebugException("execnonquery", ex);
					}
					cn.Close();
				}

				if (!bIgnoreErr) {
					exc = new Exception(sb.ToString());
				}
			}

			return exc;
		}

		#endregion General database routines
	}

	//======================
	public class DatabaseUpdateStatus {
		public bool NeedsUpdate { get; set; } = true;

		public List<DatabaseUpdateMessage> Messages { get; set; } = new List<DatabaseUpdateMessage>();

		public DatabaseUpdateStatus() {
			this.Messages = new List<DatabaseUpdateMessage>();
			this.NeedsUpdate = true;
		}
	}

	//======================
	public class DataInfo {
		public string DataKey { get; set; } = "Key";
		public string DataValue { get; set; } = "00000000";

		public bool IsYearOf(string testVersion) {
			return IsSubVersionOf(testVersion, 4);
		}

		public bool IsMinorOf(string testVersion) {
			return IsSubVersionOf(testVersion, 6);
		}

		protected bool IsSubVersionOf(string testVersion, int len) {
			if (string.IsNullOrEmpty(this.DataValue) || string.IsNullOrEmpty(testVersion)) {
				return false;
			}
			if (this.DataValue.Length < len || testVersion.Length < len) {
				return false;
			}

			return this.DataValue.Substring(0, len) == testVersion.Substring(0, len);
		}

		public bool Matches(string testVersion) {
			if (string.IsNullOrWhiteSpace(this.DataValue) || string.IsNullOrWhiteSpace(testVersion)) {
				return false;
			}

			return this.DataValue.ToUpperInvariant() == testVersion.ToUpperInvariant();
		}

		public bool IsLatest() {
			return this.DataValue == DatabaseSchemaState.CurrentDbVersion;
		}

		public bool IsBlank() {
			return string.IsNullOrWhiteSpace(this.DataValue) || this.DataValue.Length < 4 || this.DataValue.StartsWith("0000");
		}

		public void GetDbSchema() {
			var ver = DatabaseSchemaState.GetDbSchemaVersion();
			this.DataKey = ver.DataKey;
			this.DataValue = ver.DataValue;
		}

		public static string DBSchema {
			get { return "DBSchema"; }
		}

		public static DataInfo CreateBlankSchema() {
			var di = new DataInfo();
			di.DataKey = DataInfo.DBSchema;
			di.DataValue = "000000";
			return di;
		}
	}

	//======================
	public class DatabaseUpdateMessage {
		public string Message { get; set; } = string.Empty;
		public string ExceptionText { get; set; }
		public string InnerExceptionText { get; set; }
		public string Response { get; set; } = string.Empty;
		public int Order { get; set; } = -1;
		public bool AlteredData { get; set; }
		public bool HasException { get; set; }

		public DatabaseUpdateMessage() {
			this.ExceptionText = null;
			this.InnerExceptionText = null;
			this.AlteredData = false;
			this.HasException = false;
			this.Message = string.Empty;
			this.Response = string.Empty;
			this.Order = -1;
		}
	}

	//======================
	public class DatabaseUpdateResponse {
		public Exception LastException { get; set; }
		public string Response { get; set; } = string.Empty;
		public bool RanUpdate { get; set; }

		public DatabaseUpdateResponse() {
			this.LastException = null;
			this.Response = string.Empty;
			this.RanUpdate = false;
		}

		public void Combine(DatabaseUpdateResponse res1, DatabaseUpdateResponse res2) {
			if (res1.LastException != null && res2.LastException != null) {
				var msg1 = res1.LastException.CombineMessage();
				var msg2 = res2.LastException.CombineMessage();

				this.LastException = new Exception(msg1, new Exception(msg2));
			} else {
				this.LastException = (res1.LastException != null) ? res1.LastException : res2.LastException;
			}

			this.Response = string.Join("; ", new string[] { res1.Response, res2.Response });
			this.RanUpdate = res1.RanUpdate || res2.RanUpdate;
		}
	}
}