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

		private static string _ConnectionString = "";

		public DatabaseUpdate() {
			_ConnectionString = "";
			LastSQLError = null;
			SetConn();
			TestDatabaseWithQuery();
		}

		private void TestDatabaseWithQuery() {
			LastSQLError = null;

			string query = "select table_name, column_name, ordinal_position from [information_schema].[columns] as isc " +
					" where isc.table_name like 'carrot%' " +
					" order by isc.table_name, isc.ordinal_position, isc.column_name";

			DataTable table1 = GetData(query);
		}

		private static void SetConn() {
			if (string.IsNullOrEmpty(_ConnectionString)) {
				_ConnectionString = "";
				if (ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"] != null) {
					ConnectionStringSettings conString = ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"];
					_ConnectionString = conString.ConnectionString;
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

		public static DataTable GetData(string sQuery) {
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
				string query = "select * from [information_schema].[columns] " +
						" where table_name in ('tblSites', 'carrot_Sites') ";

				DataTable table1 = GetData(query);

				if (table1.Rows.Count < 1) {
					res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.CREATE01.sql", false);
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

				string query = "select distinct table_name from [information_schema].[columns] where table_name in ('aspnet_Membership', 'aspnet_Users', 'tblSites', 'tblRootContent', 'carrot_Sites', 'carrot_RootContent') ";
				DataTable table1 = GetData(query);

				if (table1.Rows.Count >= 4) {
					return true;
				}
			}

			return false;
		}


		public bool TableExists(string testTableName) {
			string testQuery = "select * from [information_schema].[columns] where table_name = '" + testTableName.Replace("'", "''") + "' ";

			DataTable table1 = GetData(testQuery);

			if (table1.Rows.Count < 1) {
				return false;
			}

			return true;
		}

		public List<string> GetTableColumns(string testTableName) {
			List<string> lst = new List<string>();

			string testQuery = "select * from [information_schema].[columns] where table_name = '" + testTableName.Replace("'", "''") + "' ";

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

		public bool IsPostStep04() {
			if (!FailedSQL) {
				string query = "";
				DataTable table1 = null;

				query = "select distinct table_name from [information_schema].[columns] where table_name in ('carrot_Sites', 'carrot_RootContent', 'carrot_Content', 'carrot_Widget') ";
				table1 = GetData(query);
				if (table1.Rows.Count >= 4) {
					return true;
				}
			}

			return false;
		}


		public static bool AreCMSTablesIncomplete() {
			if (!FailedSQL) {

				string query = "";
				DataTable table1 = null;

				//query = "select [specific_name], [ordinal_position], [parameter_name] from [information_schema].[parameters] where [specific_name] like 'carrot%' ";
				query = "SELECT * FROM sys.views WHERE name in ('vw_carrot_Comment') ";
				table1 = GetData(query);
				if (table1.Rows.Count < 1) {
					return true;
				}

				query = "select [table_schema], [table_name], [column_name], [ordinal_position] from [information_schema].[columns] where [table_name] in ('vw_carrot_Content') ";
				table1 = GetData(query);
				if (table1.Rows.Count < 31) {
					return true;
				}

				query = "select [table_schema], [table_name], [column_name], [ordinal_position] from [information_schema].[columns] \r\n " +
						"where [table_name] in ('carrot_Content', 'carrot_RootContent', 'carrot_SerialCache', 'carrot_Sites', 'carrot_UserSiteMapping', 'carrot_Widget', 'carrot_WidgetData') ";
				table1 = GetData(query);
				if (table1.Rows.Count < 52) {
					return true;
				}
			}

			return false;
		}

		public bool DatabaseNeedsUpdate() {
			if (!FailedSQL) {

				string query = "";
				DataTable table1 = null;

				//query = "select [specific_name], [ordinal_position], [parameter_name] from [information_schema].[parameters] where [specific_name] like 'carrot%' ";
				query = "SELECT * FROM sys.views WHERE name in ('vw_carrot_Comment') ";
				table1 = GetData(query);
				if (table1.Rows.Count < 1) {
					return true;
				}

				query = "select [table_schema], [table_name], [column_name], [ordinal_position] from [information_schema].[columns] where [table_name] in ('vw_carrot_Content') ";
				table1 = GetData(query);
				if (table1.Rows.Count < 31) {
					return true;
				}

				//query = "SELECT * FROM sys.views WHERE name in ( 'vw_carrot_Content', 'vw_carrot_Widget') ";
				query = "select distinct [view_name] , [table_name], [column_name] from [information_schema].[view_column_usage] where [view_name] in ( 'vw_carrot_Content', 'vw_carrot_Widget') ";
				table1 = GetData(query);
				if (table1.Rows.Count < 40) {
					return true;
				}

				//query = "select distinct table_name from [information_schema].[columns] where table_name in ('carrot_Sites', 'carrot_RootContent', 'carrot_Content', 'carrot_Widget') ";
				query = "select [table_schema], [table_name], [column_name], [ordinal_position] from [information_schema].[columns] \r\n " +
						"where [table_name] in ('carrot_Content', 'carrot_RootContent', 'carrot_SerialCache', 'carrot_Sites', 'carrot_UserSiteMapping', 'carrot_Widget', 'carrot_WidgetData') ";
				DataTable table2 = GetData(query);
				if (table2.Rows.Count < 52) {
					return true;
				}

				query = "select distinct table_name from [information_schema].[columns] where table_name in ('tblSites', 'tblRootContent', 'tblContent', 'tblWidget') ";
				table1 = GetData(query);
				if (table1.Rows.Count >= 4) {
					return true;
				}

				if (table2.Rows.Count < 1) {
					query = "select * from [information_schema].[columns] where table_name in ('tblSites', 'carrot_Sites') ";
					table1 = GetData(query);
					if (table1.Rows.Count < 1) {
						return true;
					}
					// update 01
					query = "select * from [information_schema].[columns] where table_name in ('tblContent', 'carrot_Content') and column_name = 'MetaKeyword'";
					table1 = GetData(query);
					if (table1.Rows.Count < 1) {
						return true;
					}

					// update 01 A
					query = "select column_name, table_name from [information_schema].[columns] where column_name='SerialCacheID' and table_name in ('tblSerialCache', 'carrot_SerialCache') ";
					table1 = GetData(query);
					if (table1.Rows.Count < 1) {
						return true;
					}

					// update 02
					query = "select * from [information_schema].[columns] where table_name in ('tblWidget', 'carrot_Widget') and column_name = 'Root_WidgetID'";
					table1 = GetData(query);
					if (table1.Rows.Count < 1) {
						return true;
					}
					// update 03
					query = "select * from [information_schema].[columns] where table_name in ('tblRootContent', 'carrot_RootContent') and column_name = 'CreateDate'";
					table1 = GetData(query);
					if (table1.Rows.Count < 1) {
						return true;
					}
					// update 04
					query = "select * from [information_schema].[columns] where table_name in ('carrot_Sites', 'carrot_RootContent')";
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

			string query = "select * from [information_schema].[columns] where table_name in ('tblContent', 'carrot_Content') and column_name = 'MetaKeyword'";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER01.sql", false);
				res.Response = "Created Content MetaKeyword and MetaDescription";
				return res;
			}

			res.Response = "Content MetaKeyword and MetaDescription Already Exists";
			return res;
		}

		public DatabaseUpdateResponse AlterStep00() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			string query = "select column_name, table_name from [information_schema].[columns] where column_name='SerialCacheID' and table_name in ('tblSerialCache', 'carrot_SerialCache') ";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER01a.sql", false);
				res.Response = "Created Table SerialCache";
				return res;
			}

			res.Response = "Table SerialCache Already Exists";
			return res;
		}

		public DatabaseUpdateResponse AlterStep02() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			string query = "select * from [information_schema].[columns] where table_name in ('tblWidget', 'carrot_Widget') and column_name = 'Root_WidgetID'";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER02.sql", false);
				res.Response = "Widget Schema Updated";
				return res;
			}

			res.Response = "Widget Schema Already Exists";
			return res;
		}

		public DatabaseUpdateResponse AlterStep03() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			string query = "select * from [information_schema].[columns] where table_name in ('tblRootContent', 'carrot_RootContent') and column_name = 'CreateDate'";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER03.sql", false);
				res.Response = "RootContent CreateDate Updated";
			}

			res.Response = "RootContent CreateDate Already Exists";
			return res;
		}

		public DatabaseUpdateResponse AlterStep04() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			string query = "select * from [information_schema].[columns] where table_name in ('carrot_Sites', 'carrot_RootContent')";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER04.sql", false);
				res.Response = "CMS Table Names Changed";
				return res;
			}

			res.Response = "CMS Tables Already Changed";
			return res;
		}

		public DatabaseUpdateResponse AlterStep05() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			//string query = "SELECT * FROM sys.views WHERE name in ( 'vw_carrot_Content', 'vw_carrot_Widget') ";
			string query = "select distinct [view_name], [table_name], [column_name] from [information_schema].[view_column_usage] where [view_name] in ( 'vw_carrot_Content', 'vw_carrot_Widget')  and column_name in ('Root_WidgetID', 'Root_ContentID') ";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 6) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER05.sql", false);
				res.Response = "CMS DB added vw_carrot_Content and vw_carrot_Widget";
				return res;
			}

			res.Response = "CMS DB vw_carrot_Content and vw_carrot_Widget already added";
			return res;
		}


		public DatabaseUpdateResponse AlterStep06() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			//string query = "SELECT * FROM sys.views WHERE name in ( 'vw_carrot_Content', 'vw_carrot_Widget') ";
			//string query = "select distinct table_name, column_name from information_schema.columns where table_name in('carrot_ContentType', 'carrot_ContentTag', 'carrot_ContentCategory') and column_name in('ContentTypeID', 'SiteID') ";
			//string query = "select distinct table_name, column_name from information_schema.columns where table_name in('carrot_Sites', 'carrot_RootContent') and column_name in('TimeZone', 'PageThumbnail') ";
			string query = "select [specific_name], [ordinal_position], [parameter_name] from [information_schema].[parameters] where [specific_name] like 'carrot%' ";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 5) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER06.sql", false);
				res.Response = "CMS DB created carrot_ContentType, carrot_ContentTag, carrot_ContentCategory";
				return res;
			}

			res.Response = "CMS DB carrot_ContentType, carrot_ContentTag, carrot_ContentCategory already created";
			return res;
		}

		public DatabaseUpdateResponse AlterStep07() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			//string query = "select distinct table_name, column_name from information_schema.columns where table_name in('carrot_RootContent') and column_name in('GoLiveDate', 'RetireDate') ";
			//string query = "select distinct table_name, column_name from information_schema.columns where table_name in('carrot_Sites') and column_name in('TimeZone') ";
			//string query = "select distinct table_name, column_name from information_schema.columns where table_name in('carrot_Sites', 'carrot_RootContent') and column_name in('TimeZone', 'PageThumbnail') ";
			string query = "select [specific_name], [ordinal_position], [parameter_name] from [information_schema].[parameters] where [specific_name] like 'carrot%' ";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 5) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER07.sql", false);
				res.Response = "CMS DB created cols RetireDate, GoLiveDate, and GoLiveDateLocal in carrot_RootContent";
				return res;
			}

			res.Response = "CMS DB cols RetireDate, GoLiveDate, and GoLiveDateLocal in carrot_RootContent already created";
			return res;
		}


		public DatabaseUpdateResponse AlterStep08() {
			DatabaseUpdateResponse res = new DatabaseUpdateResponse();

			string query = "SELECT * FROM sys.views WHERE name in ( 'vw_carrot_Comment') ";

			DataTable table1 = GetData(query);

			if (table1.Rows.Count < 1) {
				res.LastException = ExecFileContents("Carrotware.CMS.DBUpdater.DataScripts.ALTER08.sql", false);
				res.Response = "CMS DB added vw_carrot_Comment";
				return res;
			}

			res.Response = "CMS DB vw_carrot_Comment already added";
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
