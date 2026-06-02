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
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.DBUpdater {

	public static class DatabaseSchemaState {
		public static SqlException LastSQLError { get; set; }

		public static string CurrentDbVersion { get { return DbVersion20; } }

		public static string DbVersion10 { get { return "20130615"; } }

		public static string DbVersion11 { get { return "20130926"; } }

		public static string DbVersion12 { get { return "20141025"; } }

		public static string DbVersion13 { get { return "20200915"; } }

		public static string DbVersion20 { get { return "20260510"; } }

		internal static string ReadEmbededScript(string resourceName) {
			var sb = new StringBuilder();

			var assembly = Assembly.GetExecutingAssembly();
			var a_name = assembly.GetName().Name;

			if (resourceName.ToLowerInvariant().StartsWith(a_name.ToLowerInvariant()) == false) {
				resourceName = string.Format("{0}.DataScripts.{1}", a_name, resourceName);
			}

			using (var stream = new StreamReader(assembly.GetManifestResourceStream(resourceName))) {
				sb.Append(stream.ReadToEnd());
			}

			return sb.ToString();
		}

		public static string SetConn() {
			string connectionString = string.Empty;
			string keyName = "CarrotwareCMSConnectionString";

			if (ConfigurationManager.ConnectionStrings[keyName] != null) {
				var csSetting = ConfigurationManager.ConnectionStrings[keyName];
				connectionString = csSetting.ConnectionString;
			}

			return connectionString;
		}

		private static string _contentKey = "cms_SiteSetUpSQLState";

		public static bool FailedSQL {
			get {
				bool c = false;
				var ret = GetCacheItem(_contentKey);
				try { c = Convert.ToBoolean(ret); } catch { }
				return c;
			}
			set {
				HttpContext.Current.Cache.Insert(_contentKey, value, null, DateTime.Now.AddMinutes(3), Cache.NoSlidingExpiration);
			}
		}

		public static void ResetFailedSQL() {
			HttpContext.Current.Cache.Insert(_contentKey, "False", null, DateTime.Now.AddMilliseconds(10), Cache.NoSlidingExpiration);
			HttpContext.Current.Cache.Remove(_contentKey);
		}

		public static bool SystemNeedsChecking(Exception ex) {
			if (ex is SqlException && ex != null) {
				string msg = ex.Message.ToLowerInvariant();
				if (ex.InnerException != null) {
					msg += "\r\n" + ex.InnerException.Message.ToLowerInvariant();
				}
				if (msg.Contains("the server was not found")) {
					return false;
				}

				if (msg.Contains("invalid object name")
					|| msg.Contains("invalid column name")
					|| msg.Contains("could not find stored procedure")
					|| msg.Contains("not found")) {
					return true;
				}
			}

			return false;
		}

		public static object GetCacheItem(string key) {
			if (HttpContext.Current.Cache[key] != null) {
				return HttpContext.Current.Cache[key];
			}
			return null;
		}

		public static string GetCacheItemString(string key) {
			var item = GetCacheItem(key);
			return item != null ? item.ToString() : null;
		}

		public static object UpdateLocker = new object();
		private static object logLocker = new object();

		public static void WriteDebugException(string debugSource, Exception objErr) {
			WriteDebugException(false, debugSource, objErr);
		}

		public static void WriteDebugException(bool bWriteError, string debugSource, Exception objErr) {
#if DEBUG
			bWriteError = true;
#endif

			if (bWriteError && objErr != null) {
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("----------------  " + debugSource.ToUpperInvariant() + " - " + DateTime.Now.ToString() + "  ----------------");
				sb.AppendLine("[" + objErr.GetType().ToString() + "] " + objErr.Message);
				if (objErr.StackTrace != null) {
					sb.AppendLine(objErr.StackTrace);
				}
				if (objErr.InnerException != null) {
					sb.AppendLine(objErr.InnerException.Message);
				}

				string filePath = HttpContext.Current.Server.MapPath("~/carrot_errors.txt");
				Encoding encode = Encoding.Default;
				lock (logLocker) {
					using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)) {
						using (var oWriter = new StreamWriter(fs, encode)) {
							oWriter.Write(sb.ToString());
						}
					}
				}
			}
		}

		private static string _contentSqlStateKey = "cms_SqlTablesIncomplete";

		public static bool TablesIncomplete {
			get {
				string tablesIncomplete = string.Empty;
				bool c = true;
				var ret = GetCacheItemString(_contentSqlStateKey);

				if (ret != null) {
					tablesIncomplete = ret;
				} else {
					try {
						c = AreCMSTablesIncomplete();
					} catch (Exception ex) {
						c = false;
						WriteDebugException("tablesincomplete", ex);
					}
					tablesIncomplete = c.ToString();
					HttpContext.Current.Cache.Insert(_contentSqlStateKey, tablesIncomplete, null, DateTime.Now.AddMinutes(3), Cache.NoSlidingExpiration);
				}

				c = Convert.ToBoolean(tablesIncomplete);
				return c;
			}
		}

		public static void ResetSQLState() {
			var ret = GetCacheItem(_contentSqlStateKey);
			if (ret != null) {
				HttpContext.Current.Cache.Remove(_contentSqlStateKey);
			}
		}

		public static bool AreCMSTablesIncomplete() {
			if (!DatabaseSchemaState.FailedSQL) {
				DataInfo ver = GetDbSchemaVersion();
				if (ver.DataValue != DatabaseSchemaState.CurrentDbVersion) {
					return true;
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

		public static bool UsersExist {
			get {
				if (!DatabaseSchemaState.FailedSQL) {
					try {
						return SQLUpdateNugget.EvalNuggetKey("DoUsersExist");
					} catch (Exception ex) {
						WriteDebugException("usersexist", ex);
					}
				}
				return false;
			}
		}

		private static object schemaCheckLocker = new object();

		public static DataInfo GetDbSchemaVersion() {
			var di = DataInfo.CreateBlankSchema();
			lock (schemaCheckLocker) {
				try {
					di = GetDataKeyValue(DataInfo.DBSchema);
				} catch (Exception ex) {
					di = DataInfo.CreateBlankSchema();
				}
			}
			return di;
		}

		public static void SetDbSchemaVersion(string dataKeyValue) {
			SetDataKeyValue(DataInfo.DBSchema, dataKeyValue);
		}

		public static DataInfo GetDataKeyValue(string dataKeyName) {
			string _connStr = SetConn();
			DataInfo d = new DataInfo();
			SQLUpdateNugget n = SQLUpdateNugget.GetNuggets("SchemaVersionCheck").FirstOrDefault();
			if (n != null) {
				List<SqlParameter> parms = new List<SqlParameter>();
				parms.Add(new SqlParameter("@DataKey", dataKeyName));
				DataTable dt = ExecuteDataTableCommands(_connStr, n.SQLQuery, parms);
				if (dt.Rows.Count > 0) {
					DataRow dr = dt.Rows[0];
					d.DataKey = dr["DataKey"].ToString();
					d.DataValue = dr["DataValue"].ToString();
				}
			}
			if (d != null && string.IsNullOrEmpty(d.DataValue)) {
				d.DataValue = string.Empty;
			}
			return d;
		}

		public static void SetDataKeyValue(string dataKeyName, string dataKeyValue) {
			string _connStr = SetConn();
			SQLUpdateNugget n = SQLUpdateNugget.GetNuggets("SchemaVersionUpdate").FirstOrDefault();
			if (n != null) {
				List<SqlParameter> parms = new List<SqlParameter>();
				parms.Add(new SqlParameter("@DataValue", dataKeyValue));
				parms.Add(new SqlParameter("@DataKey", dataKeyName));
				ExecuteNonQueryCommands(_connStr, n.SQLQuery, parms);
			}
		}

		public static void ExecuteNonQueryCommands(string connectionString, string sqlQuery, List<SqlParameter> SqlParms) {
			using (SqlConnection cn = new SqlConnection(connectionString)) {
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(sqlQuery, cn)) {
					cmd.CommandType = CommandType.Text;
					foreach (var p in SqlParms) { cmd.Parameters.Add(p); }
					cmd.ExecuteNonQuery();
				}
				cn.Close();
			}
		}

		public static DataTable ExecuteDataTableCommands(string connectionString, string sqlQuery, List<SqlParameter> SqlParms) {
			DataTable dt = new DataTable();
			using (SqlConnection cn = new SqlConnection(connectionString)) {
				using (SqlCommand cmd = new SqlCommand(sqlQuery, cn)) {
					cn.Open();
					cmd.CommandType = CommandType.Text;
					if (SqlParms != null) {
						foreach (var p in SqlParms) { cmd.Parameters.Add(p); }
					}
					using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { da.Fill(dt); }
				}
				cn.Close();
			}
			return dt;
		}

		public static DataTable GetDataTable(string sqlQuery) {
			return GetDataTable(SetConn(), sqlQuery);
		}

		private static DataTable GetDataTable(string connectionString, string sqlQuery) {
			DataTable dt = new DataTable();
			using (SqlConnection cn = new SqlConnection(connectionString)) {
				using (SqlCommand cmd = new SqlCommand(sqlQuery, cn)) {
					cn.Open();
					cmd.CommandType = CommandType.Text;
					using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { da.Fill(dt); }
					cn.Close();
				}
			}
			return dt;
		}

		public static DataTable GetTestData(string sqlQuery) {
			return GetTestData(sqlQuery, null);
		}

		public static DataTable GetTestData(string sqlQuery, List<SqlParameter> SqlParms) {
			return GetTestData(SetConn(), sqlQuery, SqlParms);
		}

		public static DataTable GetTestData(string connectionString, string sqlQuery, List<SqlParameter> SqlParms) {
			DataTable dt = new DataTable();
			try {
				using (SqlConnection cn = new SqlConnection(connectionString)) {
					cn.Open();
					DatabaseSchemaState.FailedSQL = false;
					using (SqlCommand cmd = cn.CreateCommand()) {
						cmd.CommandText = sqlQuery;
						if (SqlParms != null) {
							foreach (var p in SqlParms) { cmd.Parameters.Add(p); }
						}
						using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { da.Fill(dt); }
					}
					cn.Close();
				}
				DatabaseSchemaState.LastSQLError = null;
			} catch (SqlException sqlEx) {
				DatabaseSchemaState.LastSQLError = sqlEx;
				DatabaseSchemaState.FailedSQL = true;
				WriteDebugException("gettestdata", sqlEx);
			}
			return dt;
		}

		private static DataSet GetDataSet(string connectionString, string sqlQuery) {
			DataSet ds = new DataSet();
			using (SqlConnection cn = new SqlConnection(connectionString)) {
				using (SqlCommand cmd = new SqlCommand(sqlQuery, cn)) {
					cn.Open();
					cmd.CommandType = CommandType.Text;
					using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { da.Fill(ds); }
					cn.Close();
				}
			}
			return ds;
		}
	}

	//======================

	public static class DatabaseUpdateResponseExtensions {

		public static string CombineMessage(this Exception ex) {
			var msgInner = string.Empty;
			var msgTop = ex.Message + "\n" + ex.StackTrace;

			if (ex.InnerException != null) {
				msgInner = ex.InnerException.Message + "\n" + ex.InnerException.StackTrace;
				msgInner = "\n" + msgInner;
			}

			return msgTop + msgInner;
		}
	}
}