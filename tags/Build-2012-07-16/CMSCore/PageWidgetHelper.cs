using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Carrotware.CMS.Data;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Core {

	public class PageWidgetHelper : IDisposable {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public PageWidgetHelper() { }

		public PageWidget Get(Guid pageWidgetID) {
			var w = (from r in db.tblPageWidgets
					 orderby r.WidgetOrder
					 where r.PageWidgetID == pageWidgetID
					 select new PageWidget(r)).FirstOrDefault();

			return w;
		}

		public List<PageWidget> GetWidgets(Guid rootContentID) {
			var w = (from r in db.tblPageWidgets
					 orderby r.WidgetOrder
					 where r.Root_ContentID == rootContentID
					 select new PageWidget(r)).ToList();

			return w;
		}


		public void Delete(Guid pageWidgetID) {
			var w = (from r in db.tblPageWidgets
					 where r.PageWidgetID == pageWidgetID
					 select r).FirstOrDefault();

			if (w != null) {
				db.tblPageWidgets.DeleteOnSubmit(w);
				db.SubmitChanges();
			}

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
