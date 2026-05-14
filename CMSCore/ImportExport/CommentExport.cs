using System;
using System.Collections.Generic;
using System.Linq;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.Core {

	public class CommentExport {

		public CommentExport() {
			CarrotCakeVersion = SiteData.CarrotCakeCMSVersion;
			ExportDate = DateTime.UtcNow;

			TheComment = new PostComment();
		}

		public static List<CommentExport> GetPageCommentExport(Guid rootContentID) {
			List<CommentExport> lst = PostComment.GetCommentsByContentPage(rootContentID, false).Select(x => new CommentExport(x)).ToList();

			return lst;
		}

		public CommentExport(PostComment pc) {
			SetVals(pc);
		}

		private void SetVals(PostComment pc) {
			CarrotCakeVersion = SiteData.CarrotCakeCMSVersion;
			ExportDate = DateTime.UtcNow;

			NewContentCommentID = Guid.NewGuid();

			TheComment = pc;

			if (TheComment == null) {
				TheComment = new PostComment();
				TheComment.Root_ContentID = Guid.NewGuid();
			}

			OriginalRootContentID = TheComment.Root_ContentID;
			OriginalContentCommentID = TheComment.ContentCommentID;

			TheComment.ContentCommentID = NewContentCommentID;
		}

		public string CarrotCakeVersion { get; set; }

		public DateTime ExportDate { get; set; }

		public Guid NewContentCommentID { get; set; }

		public Guid OriginalContentCommentID { get; set; }

		public Guid OriginalRootContentID { get; set; }

		public PostComment TheComment { get; set; }
	}
}