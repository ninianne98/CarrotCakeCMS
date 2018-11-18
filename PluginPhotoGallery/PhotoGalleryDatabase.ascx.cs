using Carrotware.CMS.DBUpdater;
using Carrotware.CMS.Interface;
using System;
using System.IO;
using System.Reflection;

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {

	public partial class PhotoGalleryDatabase : AdminModule {

		protected void Page_Load(object sender, EventArgs e) {
			DatabaseUpdate du = new DatabaseUpdate();
			DatabaseUpdateResponse dbRes = new DatabaseUpdateResponse();
			string sqlUpdate = String.Empty;
			string sqlTest = String.Empty;
			int iCt = 0;
			litMsg.Text = String.Empty;

			sqlUpdate = ReadEmbededScript("Carrotware.CMS.UI.Plugins.PhotoGallery.tblGallery.sql");

			sqlTest = "select * from [INFORMATION_SCHEMA].[COLUMNS] where table_name in('tblGalleryImageMeta')";
			dbRes = du.ApplyUpdateIfNotFound(sqlTest, sqlUpdate, false);
			iCt++;

			if (dbRes.LastException != null && !string.IsNullOrEmpty(dbRes.LastException.Message)) {
				litMsg.Text += iCt.ToString() + ")  " + dbRes.LastException.Message + "<br />";
			} else {
				litMsg.Text += iCt.ToString() + ")  " + dbRes.Response + "<br />";
			}
		}

		private string ReadEmbededScript(string filePath) {
			string sFile = String.Empty;

			Assembly _assembly = Assembly.GetExecutingAssembly();

			using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream(filePath))) {
				sFile = oTextStream.ReadToEnd();
			}

			return sFile;
		}
	}
}