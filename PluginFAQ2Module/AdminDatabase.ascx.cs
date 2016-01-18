using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using System.IO;
using System.Reflection;
using Carrotware.CMS.DBUpdater;


namespace Carrotware.CMS.UI.Plugins.FAQ2Module {

	public partial class AdminDatabase : AdminModule {

		protected void Page_Load(object sender, EventArgs e) {
			DatabaseUpdate du = new DatabaseUpdate();
			DatabaseUpdateResponse dbRes = new DatabaseUpdateResponse();
			string sqlUpdate = "";
			string sqlTest = "";
			int iCt = 0;
			litMsg.Text = "";


			sqlUpdate = ReadEmbededScript("Carrotware.CMS.UI.Plugins.FAQ2Module.carrot_FaqItem.sql");
			sqlTest = "select * from [INFORMATION_SCHEMA].[COLUMNS] where table_name in('carrot_FaqItem')";
			dbRes = du.ApplyUpdateIfNotFound(sqlTest, sqlUpdate, false);
			iCt++;

			if (dbRes.LastException != null && !string.IsNullOrEmpty(dbRes.LastException.Message)) {
				litMsg.Text += iCt.ToString() + ")  " + dbRes.LastException.Message + "<br />";
			} else {
				litMsg.Text += iCt.ToString() + ")  " + dbRes.Response + "<br />";
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