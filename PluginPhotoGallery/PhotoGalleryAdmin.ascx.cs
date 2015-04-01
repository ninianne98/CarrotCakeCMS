using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;



namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryAdmin : AdminModule {

		Guid gTheID = Guid.Empty;

		protected FileDataHelper fileHelper = new FileDataHelper();

		protected void Page_Load(object sender, EventArgs e) {
			gTheID = ParmParser.GetGuidIDFromQuery();

			if (!IsPostBack) {
				txtDate.Text = DateTime.Now.ToShortDateString();
				LoadLists();
				BuildFolderList();
			}
		}

		protected string SetSitePath(string sPath) {
			return FileDataHelper.MakeFileFolderPath(sPath);
		}

		protected void BuildFolderList() {
			List<FileData> lstFolders = new List<FileData>();

			string sRoot = HttpContext.Current.Server.MapPath("~/");

			string[] subdirs;
			try {
				subdirs = Directory.GetDirectories(sRoot);
			} catch {
				subdirs = null;
			}

			lstFolders.Add(new FileData { FileName = "  -- whole site --  ", FolderPath = "/", FileDate = DateTime.Now });

			if (subdirs != null) {
				foreach (string theDir in subdirs) {
					string w = FileDataHelper.MakeWebFolderPath(theDir);
					lstFolders.Add(new FileData { FileName = w, FolderPath = w, FileDate = DateTime.Now });

					string[] subdirs2;
					try {
						subdirs2 = Directory.GetDirectories(theDir);
					} catch {
						subdirs2 = null;
					}

					if (subdirs2 != null) {
						foreach (string theDir2 in subdirs2) {
							string w2 = FileDataHelper.MakeWebFolderPath(theDir2);
							lstFolders.Add(new FileData { FileName = w2, FolderPath = w2, FileDate = DateTime.Now });
						}
					}
				}
			}

			lstFolders.RemoveAll(f => f.FileName.ToLower().StartsWith(SiteData.AdminFolderPath));
			lstFolders.RemoveAll(f => f.FileName.ToLower().StartsWith("/bin/"));
			lstFolders.RemoveAll(f => f.FileName.ToLower().StartsWith("/obj/"));
			lstFolders.RemoveAll(f => f.FileName.ToLower().StartsWith("/app_data/"));

			ddlFolders.DataSource = lstFolders.OrderBy(f => f.FileName);
			ddlFolders.DataBind();
		}

		protected void LoadLists() {
			GalleryHelper gh = new GalleryHelper(SiteID);

			var gal = gh.GalleryGroupGetByID(gTheID);

			if (gal != null) {
				litGalleryName.Text = gal.GalleryTitle;

				rpGallery.DataSource = (from g in gal.GalleryImages
										orderby g.ImageOrder ascending
										select fileHelper.GetFileInfo(g.GalleryImage, g.GalleryImage)).ToList();

				rpGallery.DataBind();
			}

			SetSourceFiles(null, "/");

		}

		protected Dictionary<int, string> ParseGalleryImages() {

			var sImageList = txtGalleryOrder.Text;
			Dictionary<int, string> lstImages = new Dictionary<int, string>();

			sImageList = sImageList.Replace("\r\n", "\n");
			sImageList = sImageList.Replace("\r", "\n");
			var arrImageRows = sImageList.Split('\n');

			int iRow = 0;
			foreach (string arrImgCell in arrImageRows) {
				if (!string.IsNullOrEmpty(arrImgCell)) {
					var w = arrImgCell.Split('\t');
					var img = w[1];
					if (!string.IsNullOrEmpty(img)) {
						lstImages.Add(iRow, img);
					}
				}
				iRow++;
			}

			return lstImages;
		}


		protected void btnApply_Click(object sender, EventArgs e) {
			SetSrcFiles();
		}


		protected void chkFilter_CheckedChanged(object sender, EventArgs e) {
			SetSrcFiles();
		}

		protected void SetSrcFiles() {

			Dictionary<int, string> lstImages = ParseGalleryImages();

			DateTime? dtFilter = null;

			if (chkFilter.Checked) {
				dtFilter = DateTime.Now.Date;
				try {
					dtFilter = Convert.ToDateTime(txtDate.Text);
				} catch {
				}
			}

			rpGallery.DataSource = (from g in lstImages
									orderby g.Key ascending
									select fileHelper.GetFileInfo(g.Value, g.Value)).ToList();
			rpGallery.DataBind();

			string sPath = "/";

			if (chkPath.Checked) {
				sPath = ddlFolders.SelectedValue;
			}

			SetSourceFiles(dtFilter, sPath);
		}


		protected void SetSourceFiles(DateTime? dtFilter, string sPath) {

			List<FileData> flsWorking = new List<FileData>();
			List<FileData> fldrWorking = new List<FileData>();

			fldrWorking = fileHelper.SpiderDeepFoldersFD(sPath);

			if (Directory.Exists(FileDataHelper.MakeFileFolderPath(sPath))) {
				var fls = fileHelper.GetFiles(sPath);

				var imgs = (from m in flsWorking.Union(fls).ToList()
							where m.MimeType.StartsWith("image")
							select m).ToList();

				flsWorking = flsWorking.Union(imgs).ToList();
			}

			foreach (var f in fldrWorking) {
				var fls = fileHelper.GetFiles(f.FolderPath);

				var imgs = (from m in flsWorking.Union(fls).ToList()
							where m.MimeType.StartsWith("image")
							select m).ToList();

				flsWorking = flsWorking.Union(imgs).ToList();
			}

			flsWorking = flsWorking.Where(x => x.MimeType.StartsWith("image") && (x.FolderPath.ToLower().StartsWith(SiteData.AdminFolderPath) == false)).ToList();

			if (dtFilter != null) {
				DateTime dtFlt = Convert.ToDateTime(dtFilter);
				flsWorking = flsWorking.Where(x => x.FileDate >= dtFlt.AddDays(-14) && x.FileDate <= dtFlt.AddDays(14)).ToList();
			}

			rpFiles.DataSource = flsWorking.OrderBy(x => x.FileName).OrderBy(x => x.FolderPath).ToList();
			rpFiles.DataBind();
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			GalleryHelper gh = new GalleryHelper(SiteID);

			Dictionary<int, string> lstImages = ParseGalleryImages();
			int iPos = 0;

			foreach (var img in lstImages) {

				if (!string.IsNullOrEmpty(img.Value)) {

					var theImg = gh.GalleryImageEntryGetByFilename(gTheID, img.Value);

					if (theImg == null) {
						theImg = new GalleryImageEntry();
						theImg.GalleryImage = img.Value;
						theImg.GalleryImageID = Guid.NewGuid();
						theImg.GalleryID = gTheID;
					}

					theImg.ImageOrder = iPos;

					theImg.Save();
				}

				iPos++;

				List<string> lst = (from l in lstImages
									select l.Value.ToLower()).ToList();

				gh.GalleryImageCleanup(gTheID, lst);

			}

			var stringFile = CreateLink(ModuleName, string.Format("id={0}", gTheID));

			Response.Redirect(stringFile);
		}


	}
}