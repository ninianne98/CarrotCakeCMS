using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.IO;
using System.Reflection;


namespace Carrotware.CMS.Core {
	public class DatabaseUpdate {

		public SqlException LastSQLError { get; set; }

		private string _ConnectionString = "";

		public DatabaseUpdate() {
			LastSQLError = null;
			SetConn();
			testDB();
		}

		private void testDB() {
			LastSQLError = null;

			string query = "select distinct table_name, column_name from information_schema.columns " +
					" where table_name like 'carrot%' " +
					" order by table_name";

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

		private DataTable GetData(string sQuery) {
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


		public string CreateCMSDatabase() {
			if (!FailedSQL) {
				string query = "select distinct table_name, column_name from information_schema.columns " +
						" where table_name in ('tblSites', 'carrot_Sites') ";

				DataTable table1 = GetData(query);

				if (table1.Rows.Count < 1) {
					var ex = ProcessQuery("Carrotware.CMS.Core.DataScripts.CREATE01.sql", false);
					return "Created Database \r\n" + ex.Message.ToString();
				}

				return "Database Already Created";
			}

			return "Database Access Failed";
		}

		public bool DoDatabaseTablesExist() {
			if (!FailedSQL) {

				string query = "";
				DataTable table1 = null;

				query = "select distinct table_name from information_schema.columns  where table_name in ('aspnet_Membership', 'aspnet_Users', 'tblSites', 'tblRootContent', 'carrot_Sites', 'carrot_RootContent') ";
				table1 = GetData(query);
				if (table1.Rows.Count == 4) {
					return true;
				}
			}

			return false;
		}

		public bool DoesDBNeedUpdates() {
			if (!FailedSQL) {

				string query = "";
				DataTable table1 = null;

				query = "select distinct table_name, column_name from information_schema.columns  where table_name in ('tblSites', 'carrot_Sites') ";
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

			return false;
		}

		public bool DoUsersExist() {
			if (!FailedSQL) {
				string query = "select top 5 * from [aspnet_Membership]";
				DataTable table1 = GetData(query);

				if (table1.Rows.Count > 0) {
					return true;
				}
			}

			return false;
		}

		public string AlterStep01() {
			string query = "select * from information_schema.columns where table_name in ('tblContent', 'carrot_Content') and column_name = 'MetaKeyword'";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				var ex = ProcessQuery("Carrotware.CMS.Core.DataScripts.ALTER01.sql", false);
				return "Created MetaKeyword and MetaDescription \r\n" + ex.Message.ToString();
			}

			return "MetaKeyword and MetaDescription Already Exists";
		}

		public string AlterStep02() {
			string query = "select * from information_schema.columns where table_name in ('tblWidget', 'carrot_Widget') and column_name = 'Root_WidgetID'";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				var ex = ProcessQuery("Carrotware.CMS.Core.DataScripts.ALTER02.sql", false);
				return "Widget Schema Updated \r\n" + ex.Message.ToString();
			}

			return "Widget Schema Already Exists";
		}

		public string AlterStep03() {
			string query = "select * from information_schema.columns where table_name in ('tblRootContent', 'carrot_RootContent') and column_name = 'CreateDate'";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				var ex = ProcessQuery("Carrotware.CMS.Core.DataScripts.ALTER03.sql", false);
				return "RootContent.CreateDate Updated \r\n" + ex.Message.ToString();
			}

			return "RootContent.CreateDate Already Exists";
		}

		public string AlterStep04() {
			string query = "select * from information_schema.columns where table_name in ('carrot_Sites', 'carrot_RootContent')";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				var ex = ProcessQuery("Carrotware.CMS.Core.DataScripts.ALTER04.sql", false);
				return "CMS Table Names Updated \r\n" + ex.Message.ToString();
			}

			return "CMS Tables Already Renamed";
		}

		public Exception ProcessQuery(string sFileName, bool bIgnoreErr) {
			SetConn();

			string sScriptContents = ReadFile(sFileName);

			Exception response = RunNonQueryFile(_ConnectionString, sScriptContents, bIgnoreErr);

			return response;

		}


		public static string ReadFile(string filePath) {

			string sFile = "";

			Assembly _assembly = Assembly.GetExecutingAssembly();

			using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream(filePath))) {
				sFile = oTextStream.ReadToEnd();
			}

			return sFile;
		}


		private static List<string> SplitSQLCmds(string sSQLQuery) {
			sSQLQuery = sSQLQuery.Replace("\r\n", "\n");
			string[] splitcommands = sSQLQuery.Split(new string[] { "GO\n" }, StringSplitOptions.RemoveEmptyEntries);
			List<string> commandList = new List<string>(splitcommands);
			return commandList;
		}


		private Exception RunNonQueryFile(string sConnectionString, string sSQLQuery, bool bIgnoreErr) {
			string sConnString = sConnectionString;

			Exception exc = new Exception("");

			SqlConnection myConnection = new SqlConnection(sConnString);
			myConnection.Open();

			var cmdLst = SplitSQLCmds(sSQLQuery);

			if (!bIgnoreErr) {
				try {
					foreach (string cmdStr in cmdLst) {
						SqlCommand myCommand = myConnection.CreateCommand();
						myCommand.CommandText = cmdStr;
						myCommand.Connection = myConnection;
						myCommand.CommandTimeout = 360;
						int ret = myCommand.ExecuteNonQuery();
					}
				} catch (Exception ex) {
					exc = ex;
				} finally {
					myConnection.Close();
				}
			} else {
				string sErr = "";
				foreach (string cmdStr in cmdLst) {
					try {
						SqlCommand myCommand = myConnection.CreateCommand();
						myCommand.CommandText = cmdStr;
						myCommand.Connection = myConnection;
						int ret = myCommand.ExecuteNonQuery();
					} catch (Exception ex) {
						sErr += ex.Message.ToString() + "\n~~~~~~~~~~~~~~~~~~~~~~~~\n";
					}
				}
				exc = new Exception(sErr);
				myConnection.Close();
			}

			return exc;
		}



	}
}
