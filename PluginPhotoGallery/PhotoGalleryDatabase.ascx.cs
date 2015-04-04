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

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryDatabase : AdminModule {
		protected void Page_Load(object sender, EventArgs e) {
			DatabaseUpdate du = new DatabaseUpdate();
			DatabaseUpdateResponse dbRes = new DatabaseUpdateResponse();
			string sqlUpdate = "";
			string sqlTest = "";
			int iCt = 0;
			litMsg.Text = "";

			sqlUpdate = ReadEmbededScript("Carrotware.CMS.UI.Plugins.PhotoGallery.tblGallery.sql");

			sqlTest = "select * from [information_schema].[columns] where table_name in('tblGalleryImageMeta')";
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