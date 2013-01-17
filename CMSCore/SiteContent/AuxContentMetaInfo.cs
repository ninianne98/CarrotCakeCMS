using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carrotware.CMS.Core {

	//======================================

	public class ContentDateTally : IContentMetaInfo {
		public Guid TallyID { get; set; }
		public SiteData TheSite { get; set; }
		public DateTime GoLiveDate { get; set; }
		public string DateCaption { get; set; }
		public string DateSlug { get; set; }
		public string DateURL { get; set; }
		public int? UseCount { get; set; }

		#region IContentMetaInfo Members

		public void SetValue(Guid ContentMetaInfoID) {
			this.TallyID = ContentMetaInfoID;
		}

		public Guid ContentMetaInfoID {
			get { return this.TallyID; }
		}

		public bool MetaIsPublic {
			get { return true; }
		}

		public string MetaInfoText {
			get {
				return this.GoLiveDate.ToString("MMMM yyyy");
			}
		}

		public string MetaInfoURL {
			get {
				this.DateURL = (this.TheSite.BuildMonthSearchLink(this.GoLiveDate));
				return this.DateURL;
			}
		}

		public int MetaInfoCount {
			get { return this.UseCount == null ? 0 : Convert.ToInt32(this.UseCount); }
		}
		#endregion
	}

	//======================================

	public class ContentDateLinks : IContentMetaInfo {

		private SiteData _site = new SiteData();
		public SiteData TheSite {
			get {
				return _site;
			}
			set {
				_site = value;
				setDate();
			}
		}

		private DateTime _postDate = DateTime.MinValue;
		public DateTime PostDate {
			get {
				setDate();
				return _postDate;
			}
			set { _postDate = value; }
		}

		private void setDate() {
			this.DateURL = (this.TheSite.BuildDateSearchLink(_postDate));
		}

		public string DateURL { get; set; }
		public int? UseCount { get; set; }

		#region IContentMetaInfo Members

		public void SetValue(Guid ContentMetaInfoID) {

		}

		public Guid ContentMetaInfoID {
			get { return Guid.Empty; }
		}

		public bool MetaIsPublic {
			get { return true; }
		}

		public string MetaInfoText {
			get {
				return this.PostDate.ToString("MMMM d, yyyy");
			}
		}

		public string MetaInfoURL {
			get {
				setDate();
				return this.DateURL;
			}
		}

		public int MetaInfoCount {
			get { return this.UseCount == null ? 0 : Convert.ToInt32(this.UseCount); }
		}
		#endregion
	}


}
