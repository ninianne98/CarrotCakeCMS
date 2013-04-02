using System;
using System.Collections.Generic;
using System.Linq;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Core {

	public class ContentPageExport {

		public ContentPageExport() {
			CarrotCakeVersion = SiteData.CarrotCakeCMSVersion;
			ExportDate = DateTime.UtcNow;

			ThePage = new ContentPage();
			ThePageWidgets = new List<Widget>();
		}

		public ContentPageExport(Guid siteID, Guid rootContentID) {
			ContentPage cp = null;

			using (ContentPageHelper cph = new ContentPageHelper()) {
				cp = cph.FindContentByID(siteID, rootContentID);
			}

			List<Widget> widgets = cp.GetWidgetList();

			SetVals(cp, widgets);
		}

		public ContentPageExport(ContentPage cp, List<Widget> widgets) {

			SetVals(cp, widgets);
		}

		private void SetVals(ContentPage cp, List<Widget> widgets) {
			CarrotCakeVersion = SiteData.CarrotCakeCMSVersion;
			ExportDate = DateTime.UtcNow;
			Guid siteID = cp.SiteID;

			NewRootContentID = Guid.NewGuid();

			cp.LoadAttributes();

			ThePage = cp;
			ThePageWidgets = widgets;

			if (ThePage == null) {
				ThePage = new ContentPage();
				ThePage.Root_ContentID = Guid.NewGuid();
				ThePage.ContentID = ThePage.Root_ContentID;
			}
			if (ThePageWidgets == null) {
				ThePageWidgets = new List<Widget>();
			}

			OriginalRootContentID = ThePage.Root_ContentID;
			OriginalSiteID = ThePage.SiteID;
			OriginalParentContentID = Guid.Empty;
			ParentFileName = "";

			if (ThePage.Parent_ContentID != null) {
				ContentPage parent = new ContentPage();
				using (ContentPageHelper cph = new ContentPageHelper()) {
					parent = cph.FindContentByID(siteID, ThePage.Parent_ContentID.Value);
				}
				ParentFileName = parent.FileName;
				OriginalParentContentID = parent.Root_ContentID;
			}

			ThePage.Root_ContentID = NewRootContentID;
			ThePage.ContentID = NewRootContentID;

			foreach (var w in ThePageWidgets) {
				w.Root_ContentID = NewRootContentID;
				w.Root_WidgetID = Guid.NewGuid();
				w.WidgetDataID = Guid.NewGuid();
			}

			Guid userID = Guid.Empty;

			if (!cp.EditUserId.HasValue) {
				userID = cp.CreateUserId;
			} else {
				userID = cp.EditUserId.Value;
			}

			using (ExtendedUserData u = new ExtendedUserData(userID)) {
				this.TheUser = new SiteExportUser {
					ExportUserID = u.UserId,
					Email = u.EmailAddress,
					Login = u.UserName,
					FirstName = u.FirstName,
					LastName = u.LastName,
					UserNickname = u.UserNickName
				};
			}
		}

		public string CarrotCakeVersion { get; set; }

		public DateTime ExportDate { get; set; }

		public Guid NewRootContentID { get; set; }

		public Guid OriginalRootContentID { get; set; }

		public Guid OriginalSiteID { get; set; }

		public Guid OriginalParentContentID { get; set; }

		public string ParentFileName { get; set; }

		public ContentPage ThePage { get; set; }

		public List<Widget> ThePageWidgets { get; set; }

		public SiteExportUser TheUser { get; set; }

	}
}
