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
	public class PostComment : IDisposable {

		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();

		public PostComment() { }

		public Guid ContentCommentID { get; set; }
		public Guid Root_ContentID { get; set; }
		public DateTime CreateDate { get; set; }
		public string CommenterIP { get; set; }
		public string CommenterName { get; set; }
		public string CommenterEmail { get; set; }
		public string CommenterURL { get; set; }
		public string PostCommentText { get; set; }
		public bool IsApproved { get; set; }
		public bool IsSpam { get; set; }
		public string NavMenuText { get; internal set; }
		public string FileName { get; internal set; }


		internal PostComment(vw_carrot_Comment c) {

			if (c != null) {
				this.ContentCommentID = c.ContentCommentID;
				this.Root_ContentID = c.Root_ContentID;
				this.CreateDate = SiteData.CurrentSite.ConvertUTCToSiteTime(c.CreateDate);
				this.CommenterIP = c.CommenterIP;
				this.CommenterName = c.CommenterName;
				this.CommenterEmail = c.CommenterEmail;
				this.CommenterURL = c.CommenterURL;
				this.PostCommentText = c.PostComment;
				this.IsApproved = c.IsApproved;
				this.IsSpam = c.IsSpam;
				this.NavMenuText = c.NavMenuText;
				this.FileName = c.FileName;
			}
		}


		public void Save() {
			bool bNew = false;
			carrot_ContentComment c = CompiledQueries.cqGetContentCommentsTblByID(db, this.ContentCommentID);

			if (c == null) {
				c = new carrot_ContentComment();
				c.CreateDate = DateTime.UtcNow;
				bNew = true;
			}

			if (this.ContentCommentID == Guid.Empty) {
				this.ContentCommentID = Guid.NewGuid();
			}

			c.ContentCommentID = this.ContentCommentID;
			c.Root_ContentID = this.Root_ContentID;
			c.CommenterIP = this.CommenterIP;
			c.CommenterName = this.CommenterName;
			c.CommenterEmail = this.CommenterEmail;
			c.CommenterURL = this.CommenterURL;
			c.PostComment = this.PostCommentText;
			c.IsApproved = this.IsApproved;
			c.IsSpam = this.IsSpam;

			if (bNew) {
				db.carrot_ContentComments.InsertOnSubmit(c);
			}

			db.SubmitChanges();

			this.ContentCommentID = c.ContentCommentID;
			this.CreateDate = c.CreateDate;
		}


		public static List<PostComment> GetCommentsBySitePageNumber(Guid siteID, int iPageNbr, int iPageSize, string SortBy, ContentPageType.PageType pageType) {
			int startRec = iPageNbr * iPageSize;
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<vw_carrot_Comment> lstComments = (from c in CannedQueries.GetSiteContentCommentsByPostType(_db, siteID, pageType)
															 select c);

				return PaginateComments(lstComments, iPageNbr, iPageSize, SortBy).ToList();
			}
		}

		public static List<PostComment> GetCommentsByContentPageNumber(Guid rootContentID, int iPageNbr, int iPageSize, string SortBy, bool bActiveOnly) {
			int startRec = iPageNbr * iPageSize;
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<vw_carrot_Comment> lstComments = (from c in CannedQueries.GetContentPageComments(_db, rootContentID, bActiveOnly)
															 select c);

				return PaginateComments(lstComments, iPageNbr, iPageSize, SortBy).ToList();
			}
		}

		public static List<PostComment> PaginateComments(IQueryable<vw_carrot_Comment> lstComments, int iPageNbr, int iPageSize, string SortBy) {
			int startRec = iPageNbr * iPageSize;

			string sortField = "";
			string sortDir = "";

			if (!string.IsNullOrEmpty(SortBy)) {
				int pos = SortBy.LastIndexOf(" ");
				sortField = SortBy.Substring(0, pos).Trim();
				sortDir = SortBy.Substring(pos).Trim();
			}

			if (string.IsNullOrEmpty(sortField)) {
				sortField = "CreateDate";
			}

			if (string.IsNullOrEmpty(sortDir)) {
				sortDir = "DESC";
			}

			lstComments = ReflectionUtilities.SortByParm<vw_carrot_Comment>(lstComments, sortField, sortDir);

			return lstComments.Skip(startRec).Take(iPageSize).ToList().Select(v => new PostComment(v)).ToList();
		}


		public static int GetCommentCountBySiteAndType(Guid siteID, ContentPageType.PageType pageType) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				return (from c in CannedQueries.GetSiteContentCommentsByPostType(_db, siteID, pageType)
						select c).Count();
			}
		}


		public static int GetCommentCountByContent(Guid rootContentID, bool bActiveOnly) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				return (from c in CannedQueries.GetContentPageComments(_db, rootContentID, bActiveOnly)
						select c).Count();
			}
		}


		public static PostComment GetContentCommentByID(Guid contentCommentID) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				return new PostComment(CompiledQueries.cqGetContentCommentByID(_db, contentCommentID));
			}
		}


		public static int GetAllCommentCountBySite(Guid siteID) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				return (from c in CannedQueries.GetSiteContentComments(_db, siteID)
						select c).Count();
			}
		}


		public static List<PostComment> GetAllCommentsBySite(Guid siteID) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<PostComment> s = (from c in CannedQueries.GetSiteContentComments(_db, siteID)
											 select new PostComment(c));

				return s.ToList();
			}
		}


		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is PostComment) {
				PostComment p = (PostComment)obj;
				return (this.ContentCommentID == p.ContentCommentID)
					&& (this.Root_ContentID == p.Root_ContentID)
					&& (this.CommenterIP == p.CommenterIP);
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return ContentCommentID.GetHashCode() ^ Root_ContentID.GetHashCode();
		}


		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion
	}
}
