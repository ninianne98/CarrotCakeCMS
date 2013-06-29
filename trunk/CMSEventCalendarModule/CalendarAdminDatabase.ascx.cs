using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.DBUpdater;
using Carrotware.CMS.Interface;
/*
* CarrotCake CMS - Event Calendar
* http://www.carrotware.com/
*
* Copyright 2013, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: June 2013
*/


namespace Carrotware.CMS.UI.Plugins.EventCalendarModule {
	public partial class CalendarAdminDatabase : AdminModule {
		protected void Page_Load(object sender, EventArgs e) {
			DatabaseUpdate du = new DatabaseUpdate();

			string sqlUpdate = ReadEmbededScript("Carrotware.CMS.UI.Plugins.EventCalendarModule.carrot_CalendarEvent.sql");
			string sqlTest = "select * from [information_schema].[columns] where table_name in('carrot_CalendarEvent')";

			var dbRes = du.ApplyUpdateIfNotFound(sqlTest, sqlUpdate, false);

			if (dbRes.LastException != null && !string.IsNullOrEmpty(dbRes.LastException.Message)) {
				litMsg.Text = dbRes.LastException.Message;
			} else {
				litMsg.Text = dbRes.Response;
			}
		}

		private string ReadEmbededScript(string filePath) {

			string sFile = "";

			Assembly _assembly = Assembly.GetExecutingAssembly();

			using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream(filePath))) {
				sFile = oTextStream.ReadToEnd();
			}

			return sFile;
		}
	}
}