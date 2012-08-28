using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Text;


namespace Carrotware.CMS.Core {


	public class DatabaseUpdate {

		public SqlException LastSQLError { get; set; }

		private string _ConnectionString = "";

		public DatabaseUpdate() {
			LastSQLError = null;
			SetConn();
			TestDatabaseWithQuery();
		}

		private void TestDatabaseWithQuery() {
			LastSQLError = null;

			string query = "select table_name, column_name, ordinal_position from information_schema.columns as isc " +
					" where isc.table_name like 'carrot%' " +
					" order by isc.table_name, isc.ordinal_position, isc.column_name";

			DataTable table1 = GetData(query);
		}

		private void SetConn() {
			if (string.IsNullOrEmpty(_ConnectionString)) {
				_ConnectionString = "";
				if (ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"] != null) {
					ConnectionStringSettings conString = ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"];
					_ConnectionString = conString.ConnectionString;
				}
			}
		}

		private string ContentKey = "cms_SiteSetUpSQLState";
		public bool FailedSQL {
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
				if (msg.Contains("invalid object name")
					//|| msg.Contains("no process is on the other end of the pipe")
					|| msg.Contains("not found")) {

					return true;
				}
			}

			return false;
		}

		public DataTable GetData(string sQuery) {
			DataTable tables = new DataTable();
			try {
				SetConn();

				using (SqlConnection conn = new SqlConnection(_ConnectionString)) {
					conn.Open(); // throws if invalid

					FailedSQL = false;

					using (SqlCommand command = conn.CreateCommand()) {
						command.CommandText = sQuery;
						tables.Load(command.ExecuteReader(CommandBehavior.CloseConnection));
					}
					conn.Close();
				}
				LastSQLError = null;
			} catch (SqlException sqlEx) {
				LastSQLError = sqlEx;
				FailedSQL = true;
			}

			return tables;
		}


		public DatabaseUpdateResponse CreateCMSDatabase() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			if (!FailedSQL) {
				string query = "select * from information_schema.columns " +
						" where table_name in ('tblSites', 'carrot_Sites') ";

				DataTable table1 = GetData(query);

				if (table1.Rows.Count < 1) {
					res.LastException = ExecFileContents("Carrotware.CMS.Core.DataScripts.CREATE01.sql", false);
					res.Response = "Created Database";
					return res;
				}

				res.Response = "Database Already Created";
				return res;
			}

			res.Response = "Database Access Failed";
			return res;
		}

		public bool DoCMSTablesExist() {
			if (!FailedSQL) {

				string query = "select distinct table_name from information_schema.columns where table_name in ('aspnet_Membership', 'aspnet_Users', 'tblSites', 'tblRootContent', 'carrot_Sites', 'carrot_RootContent') ";
				DataTable table1 = GetData(query);

				if (table1.Rows.Count >= 4) {
					return true;
				}
			}

			return false;
		}


		public bool TableExists(string testTableName) {
			string testQuery = "select * from information_schema.columns where table_name = '" + testTableName.Replace("'", "''") + "' ";

			DataTable table1 = GetData(testQuery);

			if (table1.Rows.Count < 1) {
				return false;
			}

			return true;
		}

		public List<string> GetTableColumns(string testTableName) {
			List<string> lst = new List<string>();

			string testQuery = "select * from information_schema.columns where table_name = '" + testTableName.Replace("'", "''") + "' ";

			DataTable table1 = GetData(testQuery);

			if (table1.Rows.Count > 1) {
				lst = (from d in table1.AsEnumerable()
					   select d.Field<string>("column_name")).ToList();
			}

			return lst;
		}

		public DatabaseUpdateResponse ApplyUpdateIfNotFound(string testQuery, string updateStatement, bool bIg) {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();
			DataTable table1 = GetData(testQuery);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecScriptContents(updateStatement, bIg);
				res.Response = "Applied update";
				return res;
			}

			res.Response = "Did not apply update";
			return res;
		}

		public DatabaseUpdateResponse ApplyUpdateIfFound(string testQuery, string updateStatement, bool bIg) {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();
			DataTable table1 = GetData(testQuery);

			if (table1.Rows.Count > 0) {
				res.LastException = ExecScriptContents(updateStatement, bIg);
				res.Response = "Applied update";
				return res;
			}

			res.Response = "Did not apply update";
			return res;
		}


		public bool DatabaseNeedsUpdate() {
			if (!FailedSQL) {

				string query = "";
				DataTable table1 = null;

				query = "select distinct table_name from information_schema.columns where table_name in ('carrot_Sites', 'carrot_RootContent', 'carrot_Content', 'carrot_Widget') ";
				DataTable table2 = GetData(query);
				if (table2.Rows.Count >= 4) {
					return false;
				}

				query = "select distinct table_name from information_schema.columns where table_name in ('tblSites', 'tblRootContent', 'tblContent', 'tblWidget') ";
				table1 = GetData(query);
				if (table1.Rows.Count >= 4) {
					return true;
				}

				if (table2.Rows.Count < 1) {
					query = "select * from information_schema.columns where table_name in ('tblSites', 'carrot_Sites') ";
					table1 = GetData(query);
					if (table1.Rows.Count < 1) {
						return true;
					}
					// update 01
					query = "select * from information_schema.columns where table_name in ('tblContent', 'carrot_Content') and column_name = 'MetaKeyword'";
					table1 = GetData(query);
					if (table1.Rows.Count < 1) {
						return true;
					}
					// update 02
					query = "select * from information_schema.columns where table_name in ('tblWidget', 'carrot_Widget') and column_name = 'Root_WidgetID'";
					table1 = GetData(query);
					if (table1.Rows.Count < 1) {
						return true;
					}
					// update 03
					query = "select * from information_schema.columns where table_name in ('tblRootContent', 'carrot_RootContent') and column_name = 'CreateDate'";
					table1 = GetData(query);
					if (table1.Rows.Count < 1) {
						return true;
					}
					// update 04
					query = "select * from information_schema.columns where table_name in ('carrot_Sites', 'carrot_RootContent')";
					table1 = GetData(query);
					if (table1.Rows.Count < 1) {
						return true;
					}

				}
			}

			return false;
		}

		public bool UsersExist() {
			if (!FailedSQL) {
				string query = "select top 5 * from [aspnet_Membership]";
				DataTable table1 = GetData(query);

				if (table1.Rows.Count > 0) {
					return true;
				}
			}

			return false;
		}

		public DatabaseUpdateResponse AlterStep01() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			string query = "select * from information_schema.columns where table_name in ('tblContent', 'carrot_Content') and column_name = 'MetaKeyword'";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecFileContents("Carrotware.CMS.Core.DataScripts.ALTER01.sql", false);
				res.Response = "Created Content MetaKeyword and MetaDescription";
				return res;
			}

			res.Response = "Content MetaKeyword and MetaDescription Already Exists";
			return res;
		}

		public DatabaseUpdateResponse AlterStep02() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			string query = "select * from information_schema.columns where table_name in ('tblWidget', 'carrot_Widget') and column_name = 'Root_WidgetID'";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecFileContents("Carrotware.CMS.Core.DataScripts.ALTER02.sql", false);
				res.Response = "Widget Schema Updated";
				return res;
			}

			res.Response = "Widget Schema Already Exists";
			return res;
		}

		public DatabaseUpdateResponse AlterStep03() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			string query = "select * from information_schema.columns where table_name in ('tblRootContent', 'carrot_RootContent') and column_name = 'CreateDate'";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecFileContents("Carrotware.CMS.Core.DataScripts.ALTER03.sql", false);
				res.Response = "RootContent CreateDate Updated";
			}

			res.Response = "RootContent CreateDate Already Exists";
			return res;
		}

		public DatabaseUpdateResponse AlterStep04() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			string query = "select * from information_schema.columns where table_name in ('carrot_Sites', 'carrot_RootContent')";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecFileContents("Carrotware.CMS.Core.DataScripts.ALTER04.sql", false);
				res.Response = "CMS Table Names Changed";
				return res;
			}

			res.Response = "CMS Tables Already Changed";
			return res;
		}

		public Exception ExecScriptContents(string sScriptContents, bool bIgnoreErr) {
			SetConn();

			Exception response = ExecNonQuery(_ConnectionString, sScriptContents, bIgnoreErr);

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


	public class DatabaseUpdateResponse {

		public Exception LastException { get; set; }
		public string Response { get; set; }

		public DatabaseUpdateResponse() {
			LastException = null;
			Response = "";
		}
	}
}
