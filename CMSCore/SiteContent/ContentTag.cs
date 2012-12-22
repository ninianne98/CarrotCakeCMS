using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	public class ContentTag : IDisposable, IContentMetaInfo {

		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();
		//private CarrotCMSDataContext db = CompiledQueries.dbConn;

		public ContentTag() { }

		public Guid ContentTagID { get; set; }
		public Guid SiteID { get; set; }
		public string TagText { get; set; }
		public string TagSlug { get; set; }
		public string TagURL { get; set; }
		public int? UseCount { get; set; }

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion

		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is ContentTag) {
				ContentTag p = (ContentTag)obj;
				return (this.SiteID == p.SiteID
						&& this.TagSlug.ToLower() == p.TagSlug.ToLower());
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return TagSlug.GetHashCode() ^ SiteID.GetHashCode();
		}

		internal static ContentTag CreateTag(vw_carrot_TagCounted c) {
			ContentTag cc = null;
			if (c != null) {
				cc = new ContentTag();
				cc.ContentTagID = c.ContentTagID;
				cc.SiteID = c.SiteID;
				cc.TagSlug = c.TagSlug;
				cc.TagText = c.TagText;
				cc.UseCount = c.UseCount;
			}

			return cc;
		}

		internal static ContentTag CreateTag(vw_carrot_TagURL c) {
			ContentTag cc = null;
			if (c != null) {
				cc = new ContentTag();
				cc.ContentTagID = c.ContentTagID;
				cc.SiteID = c.SiteID;
				cc.TagURL = c.TagUrl;
				cc.TagText = c.TagText;
				cc.UseCount = c.UseCount;
			}

			return cc;
		}

		internal static ContentTag CreateTag(carrot_ContentTag c) {
			ContentTag cc = null;
			if (c != null) {
				cc = new ContentTag();
				cc.ContentTagID = c.ContentTagID;
				cc.SiteID = c.SiteID;
				cc.TagSlug = c.TagSlug;
				cc.TagText = c.TagText;
			}

			return cc;
		}

		public static ContentTag Get(Guid TagID) {
			ContentTag _item = null;
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				carrot_ContentTag query = CompiledQueries.cqGetContentTagByID(_db, TagID);
				_item = CreateTag(query);
			}

			return _item;
		}


		public static int GetSimilar(Guid SiteID, Guid TagID, string tagSlug) {

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<carrot_ContentTag> query = CompiledQueries.cqGetContentTagNoMatch(_db, SiteID, TagID, tagSlug);

				return query.Count();
			}
		}


		public static List<ContentTag> BuildTagList(Guid rootContentID) {

			List<ContentTag> _types = null;

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<carrot_ContentTag> query = CompiledQueries.cqGetContentTagByContentID(_db, rootContentID);

				_types = (from d in query.ToList()
						  select CreateTag(d)).ToList();
			}

			return _types;
		}

		public void Save() {
			bool bNew = false;
			carrot_ContentTag s = CompiledQueries.cqGetContentTagByID(db, this.ContentTagID);

			if (s == null) {
				s = new carrot_ContentTag();
				s.ContentTagID = Guid.NewGuid();
				s.SiteID = this.SiteID;
				bNew = true;
			}

			s.TagSlug = ContentPageHelper.ScrubSlug(this.TagSlug);
			s.TagText = this.TagText;

			if (bNew) {
				db.carrot_ContentTags.InsertOnSubmit(s);
			}

			this.ContentTagID = s.ContentTagID;

			db.SubmitChanges();
		}


		#region IContentMetaInfo Members

		public void SetValue(Guid ContentMetaInfoID) {
			this.ContentTagID = ContentMetaInfoID; 
		}

		public Guid ContentMetaInfoID {
			get { return this.ContentTagID; }
		}

		public string MetaInfoText {
			get { return this.TagText; }
		}

		public string MetaInfoURL {
			get { return this.TagURL; }
		}

		public int MetaInfoCount {
			get { return this.UseCount == null ? 0 : Convert.ToInt32(this.UseCount); }
		}
		#endregion
	}
}
