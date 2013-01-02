using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

	public class WordPressPost {
		public enum WPPostType {
			Unknown,
			Attachment,
			BlogPost,
			Page
		}

		public WordPressPost() { }

		public string PostTitle { get; set; }
		public string PostName { get; set; }
		public string PostContent { get; set; }
		public DateTime PostDateUTC { get; set; }
		public bool IsPublished { get; set; }

		public int PostOrder { get; set; }
		public int PostID { get; set; }
		public int ParentPostID { get; set; }

		public WPPostType PostType { get; set; }

		public List<string> Categories { get; set; }
		public List<string> Tags { get; set; }

		public Guid ImportRootID { get; set; }
		public string ImportFileSlug { get; set; }
		public string ImportFileName { get; set; }

		public string AttachmentURL { get; set; }

		public override string ToString() {
			return PostTitle + " : " + PostType.ToString() + " , #" + PostID;
		}

		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is WordPressPost) {
				WordPressPost p = (WordPressPost)obj;
				return (this.PostID == p.PostID)
						&& (this.PostDateUTC == p.PostDateUTC);
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return PostID.GetHashCode() ^ PostDateUTC.GetHashCode();
		}

		public void CleanBody() {
			if (string.IsNullOrEmpty(this.PostContent)) {
				this.PostContent = "";
			}

			this.PostContent = this.PostContent.Replace("\r\n", "\n");
			this.PostContent = this.PostContent.Replace('\u00A0', ' ').Replace("\n\n\n\n", "\n\n\n").Replace("\n\n\n\n", "\n\n\n");
			this.PostContent = this.PostContent.Trim();
		}

		public void RepairBody() {
			this.CleanBody();

			this.PostContent = "<p>" + this.PostContent.Replace("\n\n", "</p><p>") + "</p>";
			this.PostContent = this.PostContent.Replace("\n", "<br />\n");
			this.PostContent = this.PostContent.Replace("</p><p>", "</p>\n<p>");
		}

	}
}
