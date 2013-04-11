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

			string sqlUpdate = ReadEmbededScript("Carrotware.CMS.UI.Plugins.PhotoGallery.scripts.CreateGallery.sql");

			var ex = du.ExecScriptContents(sqlUpdate, false);

			if (!string.IsNullOrEmpty(ex.Message)) {
				litMsg.Text = ex.ToString();
			} else {
				litMsg.Text = "Gallery Database is up to date";
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