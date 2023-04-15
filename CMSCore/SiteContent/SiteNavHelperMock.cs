using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Core {

	public class SiteNavHelperMock : ISiteNavHelper {

		public SiteNavHelperMock() { }

		internal static string SampleBody {
			get {
				return "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus mi arcu, lacinia scelerisque blandit nec, mattis non nibh.</p> \r\n <p> Curabitur quis urna at massa placerat auctor. Quisque et mauris sapien, a consectetur nulla.</p>\r\n" +
							"<p>Etiam a quam lacus. Etiam urna sapien, porttitor at rhoncus sed, tristique sed sapien. Nam felis nulla, sodales a tincidunt ac, fermentum eu nisi.</p>\r\n";
			}
		}

		private static string[] _captions = new string[] { "Felis eget velit", "Dui accumsan", "Sed tempus urna",
							"Consectetur", "Adipiscing", "Leo urna molestie", "Nulla facilisi", "Lorem sed risus",
							"Vitae nunc sed", "Convallis convallis", "Morbi leo urna", "Nunc consequat" };

		private static int _captionPos = -1;
		private static List<string> _captionsUsed = new List<string>();

		internal static void ResetCaption() {
			_captionsUsed = new List<string>();
			_captionPos = -1;
		}

		internal static string GetCaption(int idx) {
			if (idx >= _captions.Length || idx < 0) {
				idx = 0;
			}

			var caption = _captions[idx];

			_captionPos = idx + 1;

			return string.Format("{0} {1}", caption, idx);
		}

		internal static string GetRandomCaption() {
			var caption = _captions.OrderBy(x => Guid.NewGuid())
							.Where(x => !_captionsUsed.Contains(x))
							.FirstOrDefault();

			if (_captionsUsed.Count >= 7) {
				_captionsUsed.RemoveAt(0);
				_captionsUsed.RemoveAt(0);
				_captionsUsed.RemoveAt(0);
				_captionsUsed.RemoveAt(0);
			}

			_captionsUsed.Add(caption);

			return string.Format("{0}", caption);
		}

		internal static string GetNextCaption() {
			Interlocked.Increment(ref _captionPos);
			if (_captionPos >= _captions.Length || _captionPos < 0) {
				_captionPos = 0;
			}

			var caption = _captions[_captionPos];
			_captionsUsed.Add(caption);

			return string.Format("{0} {1}", caption, _captionPos);
		}

		public static string ReadEmbededScript(string sResouceName) {
			string body = string.Empty;

			Assembly _assembly = Assembly.GetExecutingAssembly();
			using (var stream = new StreamReader(_assembly.GetManifestResourceStream(sResouceName))) {
				body = stream.ReadToEnd();
			}

			return body;
		}

		public List<SiteNav> GetMasterNavigation(Guid siteID, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav();
		}

		public List<SiteNav> GetTopNavigation(Guid siteID, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav();
		}

		public List<SiteNav> GetTwoLevelNavigation(Guid siteID, bool bActiveOnly) {
			List<SiteNav> lstNav = SiteNavHelper.GetSamplerFakeNav();
			List<SiteNav> lstNav2 = new List<SiteNav>();

			foreach (SiteNav l in lstNav) {
				lstNav2 = lstNav2.Union(SiteNavHelper.GetSamplerFakeNav(l.Root_ContentID)).ToList();
			}

			lstNav = lstNav.Union(lstNav2).ToList();
			return lstNav;
		}

		public List<SiteNav> GetLevelDepthNavigation(Guid siteID, int iDepth, bool bActiveOnly) {
			List<SiteNav> lstNav = SiteNavHelper.GetSamplerFakeNav();
			List<SiteNav> lstNav2 = new List<SiteNav>();

			foreach (SiteNav l1 in lstNav) {
				List<SiteNav> lst = SiteNavHelper.GetSamplerFakeNav(l1.Root_ContentID);
				lstNav2 = lstNav2.Union(lst).ToList();

				foreach (SiteNav l2 in lst) {
					List<SiteNav> lst2 = SiteNavHelper.GetSamplerFakeNav(l2.Root_ContentID);
					lstNav2 = lstNav2.Union(lst2).ToList();
				}
			}

			lstNav = lstNav.Union(lstNav2).ToList();
			return lstNav;
		}

		public List<IContentMetaInfo> GetCategoryList(Guid siteID, int iUpdates) {
			List<int> pagelist = Enumerable.Range(1, iUpdates).ToList();

			List<IContentMetaInfo> lstContent = (from ct in pagelist
												 orderby ct descending
												 select new ContentCategory {
													 SiteID = Guid.NewGuid(),
													 CategoryURL = string.Format("#/archive/keyword/cat{0}.aspx", ct),
													 CategoryText = string.Format("Meta Info Cat {0}", ct),
													 UseCount = ct + 2,
													 PublicUseCount = ct + 3
												 }).Cast<IContentMetaInfo>().ToList();

			return lstContent;
		}

		public List<IContentMetaInfo> GetTagList(Guid siteID, int iUpdates) {
			List<int> pagelist = Enumerable.Range(1, iUpdates).ToList();

			List<IContentMetaInfo> lstContent = (from ct in pagelist
												 orderby ct descending
												 select new ContentTag {
													 SiteID = Guid.NewGuid(),
													 TagURL = string.Format("#/archive/keyword/tag{0}.aspx", ct),
													 TagText = string.Format("Meta Info Tag {0}", ct),
													 UseCount = ct + 2,
													 PublicUseCount = ct + 3
												 }).Cast<IContentMetaInfo>().ToList();

			return lstContent;
		}

		public List<IContentMetaInfo> GetTagListForPost(Guid siteID, int iUpdates, string urlFileName) {
			return GetTagList(siteID, 3);
		}

		public List<IContentMetaInfo> GetCategoryListForPost(Guid siteID, int iUpdates, string urlFileName) {
			return GetCategoryList(siteID, 5);
		}

		public List<IContentMetaInfo> GetTagListForPost(Guid siteID, int iUpdates, Guid rootContentID) {
			return GetTagList(siteID, 3);
		}

		public List<IContentMetaInfo> GetCategoryListForPost(Guid siteID, int iUpdates, Guid rootContentID) {
			return GetCategoryList(siteID, 5);
		}

		public List<ContentDateLinks> GetSingleMonthBlogUpdateList(SiteData currentSite, DateTime monthDate, bool bActiveOnly) {
			List<ContentDateLinks> lstContent = new List<ContentDateLinks>();
			int n = 0;
			monthDate = monthDate.AddDays(0 - monthDate.Day).AddDays(1);
			DateTime dateNow = monthDate;

			while (n < 28) {
				dateNow = monthDate.AddDays(n);

				ContentDateLinks cc = new ContentDateLinks();
				cc.TheSite = currentSite;
				cc.UseCount = n;
				cc.PostDate = dateNow;
				lstContent.Add(cc);

				n = n + 3;
			}

			return lstContent;
		}

		public List<IContentMetaInfo> GetMonthBlogUpdateList(Guid siteID, int iUpdates, bool bActiveOnly) {
			List<IContentMetaInfo> lstContent = new List<IContentMetaInfo>();
			int n = 0;
			DateTime dateNow = DateTime.UtcNow.Date;

			while (n < iUpdates) {
				dateNow = DateTime.UtcNow.Date.AddMonths(0 - n);

				ContentCategory cc = new ContentCategory();
				cc.SiteID = siteID;
				cc.UseCount = n * 3;
				cc.PublicUseCount = n * 2;
				cc.ContentCategoryID = Guid.NewGuid();
				cc.EditDate = dateNow;
				cc.CategoryText = dateNow.ToString("MMMM yyyy");
				cc.CategoryURL = "#" + SiteData.PreviewTemplateFilePage;

				lstContent.Add(cc);
				n++;
			}

			return lstContent;
		}

		public List<SiteNav> GetPageCrumbNavigation(Guid siteID, Guid rootContentID, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(rootContentID);
		}

		public List<SiteNav> GetPageCrumbNavigation(Guid siteID, string sPage, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(Guid.NewGuid());
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, string sparentPageID, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(Guid.NewGuid());
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, string sPage, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(Guid.NewGuid());
		}

		public int GetChildNavigationCount(Guid siteID, Guid? parentPageID, bool bActiveOnly) {
			return 15;
		}

		public int GetChildNavigationCount(Guid siteID, string sParentPage, bool bActiveOnly) {
			return 15;
		}

		public SiteNav GetPageNavigation(Guid siteID, string sPage) {
			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav GetPageNavigation(Guid siteID, Guid rootContentID) {
			return SiteNavHelper.GetSamplerView(rootContentID);
		}

		public SiteNav GetParentPageNavigation(Guid siteID, string sPage) {
			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav GetParentPageNavigation(Guid siteID, Guid rootContentID) {
			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav GetPrevPost(Guid siteID, Guid rootContentID, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav GetNextPost(Guid siteID, Guid rootContentID, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerView();
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, Guid? parentPageID, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(parentPageID);
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, Guid PageID, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(PageID);
		}

		public List<SiteNav> GetLatest(Guid siteID, int iUpdates, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(iUpdates);
		}

		public List<SiteNav> GetLatestPosts(Guid siteID, int iUpdates, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(iUpdates);
		}

		public List<SiteNav> GetLatestUpdates(Guid siteID, int iUpdates, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(iUpdates);
		}

		public List<SiteNav> GetLatestPostUpdates(Guid siteID, int iUpdates, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(iUpdates);
		}

		public SiteNav GetLatestVersion(Guid siteID, Guid rootContentID) {
			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav GetLatestVersion(Guid siteID, bool bActiveOnly, string sPage) {
			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav FindHome(Guid siteID) {
			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav FindByFilename(Guid siteID, string urlFileName) {
			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav FindHome(Guid siteID, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerView();
		}

		public int GetFilteredContentPagedCount(SiteData currentSite, string sFilterPath, bool bActiveOnly) {
			return 50;
		}

		public int GetFilteredContentByIDPagedCount(SiteData currentSite, List<Guid> lstCategories, bool bActiveOnly) {
			return 50;
		}

		public int GetFilteredContentByIDPagedCount(SiteData currentSite, List<Guid> lstCategoryGUIDs, List<string> lstCategorySlugs, bool bActiveOnly) {
			return 50;
		}

		public int GetSiteSearchCount(Guid siteID, string searchTerm, bool bActiveOnly) {
			return 50;
		}

		public List<SiteNav> GetLatestContentSearchList(Guid siteID, string searchTerm, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public string GetBlogHeadingFromURL(SiteData currentSite, string sFilterPath) {
			string sTitle = string.Empty;

			if (currentSite.CheckIsBlogCategoryPath(sFilterPath)) {
				sTitle = "Category 1";
			}
			if (currentSite.CheckIsBlogTagPath(sFilterPath)) {
				sTitle = "Tag 1";
			}
			if (currentSite.CheckIsBlogEditorFolderPath(sFilterPath)) {
				sTitle = "Editor 1";
			}
			if (currentSite.CheckIsBlogDateFolderPath(sFilterPath)) {
				sTitle = DateTime.UtcNow.ToString("MMMM yyyy");
			}
			if (currentSite.CheckIsSiteSearchPath(sFilterPath)) {
				sTitle = "Search Results";
			}

			return sTitle;
		}

		public List<SiteNav> GetFilteredContentPagedList(SiteData currentSite, string sFilterPath, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public List<SiteNav> GetFilteredContentByIDPagedList(SiteData currentSite, List<Guid> lstCategories, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public List<SiteNav> GetFilteredContentByIDPagedList(SiteData currentSite, List<Guid> lstCategoryGUIDs, List<string> lstCategorySlugs, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public List<SiteNav> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageNumber) {
			return SiteNavHelper.GetSamplerFakeNav(10);
		}

		public List<SiteNav> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(10);
		}

		public List<SiteNav> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageSize, int pageNumber) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public List<SiteNav> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public List<SiteNav> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageNumber) {
			return SiteNavHelper.GetSamplerFakeNav(10);
		}

		public List<SiteNav> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(10);
		}

		public List<SiteNav> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageSize, int pageNumber) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public List<SiteNav> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public List<SiteNav> GetLatestChildContentPagedList(Guid siteID, Guid? parentContentID, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize, parentContentID);
		}

		public List<SiteNav> GetLatestChildContentPagedList(Guid siteID, string parentPage, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize, Guid.NewGuid());
		}

		public int GetSiteContentCount(Guid siteID) {
			return 50;
		}

		public int GetSitePageCount(Guid siteID, ContentPageType.PageType entryType) {
			return 50;
		}

		public int GetSitePageCount(Guid siteID, ContentPageType.PageType entryType, bool bActiveOnly) {
			return 50;
		}

		public List<SiteNav> PerformDataPagingQueryableContent(Guid siteID, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir, IQueryable<Data.vw_carrot_Content> QueryInput) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public void Dispose() { }
	}

	//=====================
	internal class SequentialGuid {
		private int _b0 = 1;
		private int _b1 = 1;
		private int _b2 = 1;
		private int _b3 = 1;
		private int _b4 = 1;
		private int _b5 = 1;

		internal SequentialGuid() { }

		internal Guid NextGuid {
			get {
				var tempGuid = Guid.Empty;
				var bytes = tempGuid.ToByteArray();

				bytes[0] = (byte)_b0;
				bytes[1] = (byte)_b1;
				bytes[2] = (byte)_b2;
				bytes[3] = (byte)_b3;
				bytes[4] = (byte)_b4;
				bytes[5] = (byte)_b5;

				_b0 = _b0 + 3;
				_b1 = _b1 + 5;
				_b2 = _b2 + 11;
				_b3 = _b3 + 9;
				_b4 = _b4 + 7;
				_b5 = _b5 + 13;

				return new Guid(bytes);
			}
		}
	}
}