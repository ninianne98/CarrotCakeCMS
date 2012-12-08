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
		public string PostCommentText { get; set; }
		public bool IsApproved { get; set; }
		public bool IsSpam { get; set; }


		internal static PostComment CreatePostComment(carrot_ContentComment c) {
			PostComment cont = null;

			if (c != null) {
				cont = new PostComment();

				cont.ContentCommentID = c.ContentCommentID;
				cont.Root_ContentID = c.Root_ContentID;
				cont.CommenterIP = c.CommenterIP;
				cont.CommenterName = c.CommenterName;
				cont.CommenterEmail = c.CommenterEmail;
				cont.PostCommentText = c.PostComment;
				cont.CreateDate = c.CreateDate;
				cont.IsApproved = c.IsApproved;
				cont.IsSpam = c.IsSpam;

			}

			return cont;
		}



		public void Save() {
			bool bNew = false;
			carrot_ContentComment c = CompiledQueries.cqGetContentCommentByID(db, this.ContentCommentID);

			if (c == null) {
				c = new carrot_ContentComment();
				c.CreateDate = DateTime.Now;
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
				IQueryable<carrot_ContentComment> lstComments = (from c in CannedQueries.GetSiteContentCommentsByPostType(_db, siteID, pageType)
																 select c);

				return GetCommentsBySitePageNumber(lstComments, iPageNbr, iPageSize, SortBy).ToList();
			}
		}

		public static int GetCommentCountBySiteAndType(Guid siteID, ContentPageType.PageType pageType) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				return (from c in CannedQueries.GetSiteContentCommentsByPostType(_db, siteID, pageType)
						select c).Count();
			}
		}

		public static List<PostComment> GetCommentsBySitePageNumber(IQueryable<carrot_ContentComment> lstComments, int iPageNbr, int iPageSize, string SortBy) {
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

			lstComments = ReflectionUtilities.SortByParm<carrot_ContentComment>(lstComments, sortField, sortDir);

			return lstComments.Skip(startRec).Take(iPageSize).ToList().Select(v => CreatePostComment(v)).ToList();
		}


		public static int GetCommentCountBySite(Guid siteID) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				return (from c in CannedQueries.GetSiteContentComments(_db, siteID)
						select c).Count();
			}
		}

		public static PostComment GetContentCommentByID(Guid contentCommentID) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				return CreatePostComment(CompiledQueries.cqGetContentCommentByID(_db, contentCommentID));
			}
		}


		public static List<PostComment> GetCommentsBySite(Guid siteID) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<PostComment> s = (from c in CannedQueries.GetSiteContentComments(_db, siteID)
											 select CreatePostComment(c));

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
