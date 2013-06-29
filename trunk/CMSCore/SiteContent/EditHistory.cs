using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Carrotware.CMS.Data;
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
	public class EditHistory {

		public EditHistory() { }

		public EditHistory(vw_carrot_EditHistory p) {
			if (p != null) {
				SiteData site = SiteData.GetSiteFromCache(p.SiteID);

				this.SiteID = p.SiteID;
				this.ContentID = p.ContentID;
				this.Root_ContentID = p.Root_ContentID;
				this.IsLatestVersion = p.IsLatestVersion;
				this.TitleBar = p.TitleBar;
				this.NavMenuText = p.NavMenuText;
				this.PageHead = p.PageHead;
				this.EditUserId = p.EditUserId;
				this.EditDate = site.ConvertUTCToSiteTime(p.EditDate);
				this.FileName = p.FileName;
				this.ContentTypeID = p.ContentTypeID;
				this.ContentTypeValue = p.ContentTypeValue;
				this.PageActive = p.PageActive;
				this.GoLiveDate = site.ConvertUTCToSiteTime(p.GoLiveDate);
				this.RetireDate = site.ConvertUTCToSiteTime(p.RetireDate);
				this.EditUserName = p.EditUserName;
				this.EditEmail = p.EditEmail;
				this.IsLockedOut = p.IsLockedOut;
				this.CreateDate = site.ConvertUTCToSiteTime(p.CreateDate);
				this.LastLoginDate = p.LastLoginDate;
				this.LastPasswordChangedDate = p.LastPasswordChangedDate;
				this.LastLockoutDate = p.LastLockoutDate;
				this.CreateUserName = p.CreateUserName;
				this.CreateEmail = p.CreateEmail;
			}
		}

		public Guid SiteID { get; set; }
		public Guid ContentID { get; set; }
		public Guid Root_ContentID { get; set; }
		public bool IsLatestVersion { get; set; }
		public string TitleBar { get; set; }
		public string NavMenuText { get; set; }
		public string PageHead { get; set; }
		public Guid? EditUserId { get; set; }
		public DateTime EditDate { get; set; }
		public string FileName { get; set; }
		public Guid ContentTypeID { get; set; }
		public string ContentTypeValue { get; set; }
		public bool PageActive { get; set; }
		public DateTime GoLiveDate { get; set; }
		public DateTime RetireDate { get; set; }
		public string EditUserName { get; set; }
		public string EditEmail { get; set; }
		public bool IsLockedOut { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime LastLoginDate { get; set; }
		public DateTime LastPasswordChangedDate { get; set; }
		public DateTime LastLockoutDate { get; set; }
		public string CreateUserName { get; set; }
		public string CreateEmail { get; set; }


		public static int GetHistoryListCount(Guid siteID, bool showLatestOnly, DateTime? editDate, Guid? editUserID) {

			Guid userID = Guid.Empty;
			if (editUserID.HasValue) {
				userID = editUserID.Value;
			}

			DateTime dateStart = DateTime.UtcNow.Date.AddDays(-2);
			DateTime dateEnd = DateTime.UtcNow.Date.AddDays(1);

			if (editDate.HasValue) {
				dateStart = editDate.Value.Date.AddDays(-8);
				dateEnd = editDate.Value.Date.AddDays(1);
			}

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {

				return (from h in _db.vw_carrot_EditHistories
						where h.SiteID == siteID
							&& (!showLatestOnly || h.IsLatestVersion == true)
							&& (!editDate.HasValue
								  || (h.EditDate.Date >= dateStart.Date && h.EditDate.Date <= dateEnd.Date))
							&& (!editUserID.HasValue || h.EditUserId == userID)
						select h).Count();

			}
		}

		public static List<EditHistory> GetHistoryList(string orderBy, int pageNumber, int pageSize,
				Guid siteID, bool showLatestOnly, DateTime? editDate, Guid? editUserID) {

			string sortField = string.Empty;
			string sortDir = string.Empty;

			if (!string.IsNullOrEmpty(orderBy)) {
				int pos = orderBy.LastIndexOf(" ");

				sortField = orderBy.Substring(0, pos).Trim();
				sortDir = orderBy.Substring(pos).Trim();
			}

			if (string.IsNullOrEmpty(sortField)) {
				sortField = "EditDate";
			}

			if (string.IsNullOrEmpty(sortDir)) {
				sortDir = "DESC";
			}

			Guid userID = Guid.Empty;
			if (editUserID.HasValue) {
				userID = editUserID.Value;
			}

			DateTime dateStart = DateTime.UtcNow.Date.AddDays(-2);
			DateTime dateEnd = DateTime.UtcNow.Date.AddDays(1);

			if (editDate.HasValue) {
				dateStart = editDate.Value.Date.AddDays(-8);
				dateEnd = editDate.Value.Date.AddDays(1);
			}

			int startRec = pageNumber * pageSize;

			if (pageSize < 0 || pageSize > 200) {
				pageSize = 25;
			}

			if (pageNumber < 0 || pageNumber > 10000) {
				pageNumber = 0;
			}

			bool IsContentProp = false;

			sortDir = sortDir.ToUpper();

			sortField = (from p in ReflectionUtilities.GetPropertyStrings(typeof(vw_carrot_EditHistory))
						 where p.ToLower().Trim() == sortField.ToLower().Trim()
						 select p).FirstOrDefault();

			if (!string.IsNullOrEmpty(sortField)) {
				IsContentProp = ReflectionUtilities.DoesPropertyExist(typeof(vw_carrot_EditHistory), sortField);
			}


			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				List<EditHistory> _history = null;

				IQueryable<vw_carrot_EditHistory> QueryInput = (from h in _db.vw_carrot_EditHistories
																where h.SiteID == siteID
																	&& (!showLatestOnly || h.IsLatestVersion == true)
																	&& (!editDate.HasValue
																		  || (h.EditDate.Date >= dateStart.Date && h.EditDate.Date <= dateEnd.Date))
																	&& (!editUserID.HasValue || h.EditUserId == userID)
																select h);

				if (IsContentProp) {
					QueryInput = ReflectionUtilities.SortByParm<vw_carrot_EditHistory>(QueryInput, sortField, sortDir);
				} else {
					QueryInput = (from c in QueryInput
								  orderby c.EditDate descending
								  where c.SiteID == siteID
								  select c).AsQueryable();
				}

				_history = (from h in QueryInput.Skip(startRec).Take(pageSize) select new EditHistory(h)).ToList();

				return _history;
			}

		}


	}
}
