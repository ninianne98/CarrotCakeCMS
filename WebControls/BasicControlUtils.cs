using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Carrotware.Web.UI.Controls {
	public class BasicControlUtils {
		private Page _page;

		public BasicControlUtils() {
			bFoundPage = false;
			_page2 = null;

			_page = GetContainerPage(this);
		}

		public BasicControlUtils(object X) {
			bFoundPage = false;
			_page2 = null;

			if (X != null && X is Control && ((Control)X).Page != null) {
				_page = ((Control)X).Page;
			} else {
				_page = GetContainerPage(X);
			}
		}

		public string GetWebResourceUrl(Type type, string resource) {
			string sPath = "";

			try { sPath = _page.ClientScript.GetWebResourceUrl(type, resource); } catch { }

			return sPath;
		}

		public string GetWebResourceUrl(Page page, Type type, string resource) {
			string sPath = "";

			try { sPath = page.ClientScript.GetWebResourceUrl(type, resource); } catch { }

			return sPath;
		}

		public Page GetContainerPage(object X) {
			bFoundPage = false;
			_page2 = null;

			Page foundPage = FindPage(X);

			return foundPage;
		}

		private bool bFoundPage = false;
		private Page _page2 = null;
		private Page FindPage(object X) {

			if (X is Page) {
				bFoundPage = true;
				_page2 = (Page)X;
			} else {
				if (!bFoundPage) {
					if (X is Control) {
						Control c = (Control)X;
						FindPage(c.Parent);
					}
				}
			}

			return _page2;
		}

	}
}
