using Carrotware.CMS.DBUpdater;
using Carrotware.CMS.Interface;
using System;
using System.IO;
using System.Linq;

using System.Linq;
using System.Collections.Generic;
using System;

using System.Reflection;

/*
* CarrotCake CMS - Event Calendar
* http://www.carrotware.com/
*
* Copyright 2013, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: June 2013
*/

namespace Carrotware.CMS.UI.Plugins.EventCalendarModule {

	public partial class CalendarAdminDatabase : AdminModule {

		protected void Page_Load(object sender, EventArgs e) {
			DatabaseUpdate du = new DatabaseUpdate();
			DatabaseUpdateResponse dbRes = new DatabaseUpdateResponse();
			string sqlUpdate = string.Empty;
			string sqlTest = string.Empty;
			int iCt = 0;
			litMsg.Text = string.Empty;

			sqlUpdate = ReadEmbededScript("Carrotware.CMS.UI.Plugins.EventCalendarModule.carrot_CalendarEvent.sql");
			sqlTest = "select * from [INFORMATION_SCHEMA].[COLUMNS] where table_name = 'carrot_CalendarEventProfile' and column_name = 'RecursEvery'";
			dbRes = du.ApplyUpdateIfNotFound(sqlTest, sqlUpdate, false);
			iCt++;

			if (dbRes.LastException != null && !string.IsNullOrEmpty(dbRes.LastException.Message)) {
				litMsg.Text += iCt.ToString() + ")  " + dbRes.LastException.Message + "<br />";
			} else {
				litMsg.Text += iCt.ToString() + ")  " + dbRes.Response + "<br />";
			}

			var lst = CalendarHelper.GetCalendarCategories(SiteID);

			if (!lst.Any()) {
				using (CalendarDataContext db = CalendarDataContext.GetDataContext()) {
					var itm = new carrot_CalendarEventCategory();
					itm.CalendarEventCategoryID = Guid.NewGuid();
					itm.SiteID = SiteID;
					itm.CategoryName = "Default";
					itm.CategoryFGColor = "#8FBC8F";
					itm.CategoryBGColor = "#FFFFFF";

					db.carrot_CalendarEventCategories.InsertOnSubmit(itm);
					db.SubmitChanges();
				}
			}
		}

		private string ReadEmbededScript(string filePath) {
			string sFile = string.Empty;

			Assembly _assembly = Assembly.GetExecutingAssembly();

			using (var stream = new StreamReader(_assembly.GetManifestResourceStream(filePath))) {
				sFile = stream.ReadToEnd();
			}

			return sFile;
		}
	}
}