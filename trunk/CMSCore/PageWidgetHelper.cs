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

		public PageWidget Get(Guid rootWidgetID) {
			return new PageWidget(rootWidgetID);
		}

		public List<PageWidget> GetWidgets(Guid rootContentID, bool? bActive) {
			var w = (from r in db.tblWidgetDatas
					 join rr in db.tblWidgets on r.Root_WidgetID equals rr.Root_WidgetID
					 orderby rr.WidgetOrder
					 where rr.Root_ContentID == rootContentID
						&& r.IsLatestVersion == true
						&& rr.WidgetActive == bActive || bActive == null
					 select new PageWidget(r)).ToList();

			return w;
		}

		public void Delete(Guid widgetDataID) {
			var w = (from r in db.tblWidgetDatas
					 where r.WidgetDataID == widgetDataID
					 select r).FirstOrDefault();

			if (w != null) {
				db.tblWidgetDatas.DeleteOnSubmit(w);
				db.SubmitChanges();
			}
		}

		public void Disable(Guid rootWidgetID) {
			var w = (from r in db.tblWidgets
					 where r.Root_WidgetID == rootWidgetID
					 select r).FirstOrDefault();

			if (w != null) {
				w.WidgetActive = false;
				db.SubmitChanges();
			}
		}

		public void DeleteAll(Guid rootWidgetID) {

			var w1 = (from r in db.tblWidgetDatas
					  where r.Root_WidgetID == rootWidgetID
					  select r).ToList();

			var w2 = (from r in db.tblWidgets
					  where r.Root_WidgetID == rootWidgetID
					  select r).ToList();

			bool bPendingDel = false;

			if (w1 != null) {
				foreach (var w in w1) {
					db.tblWidgetDatas.DeleteOnSubmit(w);
					bPendingDel = true;
				}
			}

			if (w2 != null) {
				foreach (var w in w2) {
					db.tblWidgets.DeleteOnSubmit(w);
					bPendingDel = true;
				}
			}

			if (bPendingDel) {
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
