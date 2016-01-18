using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carrotware.CMS.Core;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public class FileUp : IHttpHandler {

		public void ProcessRequest(HttpContext context) {
			context.Response.ContentType = "text/plain";

			try {
				if (context.Request.Files != null && context.Request.Files.Count > 0) {
					FileDataHelper helpFile = new FileDataHelper();

					for (int i = 0; i < context.Request.Files.Count; i++) {
						HttpPostedFile file = context.Request.Files[i];

						string dir = context.Request["FileDirectory"].ToString() ?? @"/";
						bool esc = Convert.ToBoolean(context.Request["EscapeSpaces"].ToString() ?? "false");
						string path = context.Server.MapPath(dir).Replace(@"\", @"/");
						string uploadedFileName = file.FileName.Replace(@"\", @"/").Replace(@"//", @"/");

						if (uploadedFileName.IndexOf("/") > 0) {
							uploadedFileName = uploadedFileName.Substring(uploadedFileName.LastIndexOf("/"));
						}

						if ((from b in helpFile.BlockedTypes
							 where uploadedFileName.ToLowerInvariant().EndsWith(String.Format(".{0}", b).ToLowerInvariant())
							 select b).Count() < 1) {
							if (esc) {
								uploadedFileName = uploadedFileName.Replace(" ", "-");
								uploadedFileName = uploadedFileName.Replace("_", "-");
								uploadedFileName = uploadedFileName.Replace("+", "-");
								uploadedFileName = uploadedFileName.Replace("%20", "-");
							}

							string filename = (path + "/" + uploadedFileName).Replace(@"//", @"/");
							file.SaveAs(filename);
						} else {
							throw new Exception("Blocked File Type");
						}

						var serial = new System.Web.Script.Serialization.JavaScriptSerializer();
						var res = new { name = file.FileName };
						context.Response.Write(serial.Serialize(res));
					}
				}
			} catch (Exception ex) {
				SiteData.WriteDebugException("fileupload", ex);
				throw;
			}
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}