using Carrotware.CMS.DBUpdater;
using Carrotware.CMS.Interface;
using System;
using System.IO;
using System.Reflection;

namespace Carrotware.CMS.UI.Plugins.FAQ2Module {

	public partial class AdminDatabase : AdminModule {

		protected void Page_Load(object sender, EventArgs e) {
			DatabaseUpdate du = new DatabaseUpdate();
			DatabaseUpdateResponse dbRes = new DatabaseUpdateResponse();
			string sqlUpdate = string.Empty;
			string sqlTest = string.Empty;
			int iCt = 0;
			litMsg.Text = string.Empty;

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
			string sFile = string.Empty;

			Assembly _assembly = Assembly.GetExecutingAssembly();

			using (var stream = new StreamReader(_assembly.GetManifestResourceStream(filePath))) {
				sFile = stream.ReadToEnd();
			}

			return sFile;
		}
	}
}