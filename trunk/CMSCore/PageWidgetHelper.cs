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

		public List<PageWidget> GetWidgets(Guid rootContentID, bool? bActiveOnly) {
			var w = (from r in db.carrot_WidgetDatas
					 join rr in db.carrot_Widgets on r.Root_WidgetID equals rr.Root_WidgetID
					 orderby rr.WidgetOrder
					 where rr.Root_ContentID == rootContentID
						&& r.IsLatestVersion == true
						&& (rr.WidgetActive == bActiveOnly || bActiveOnly == null)
					 select new PageWidget(r)).ToList();

			return w;
		}

		public List<PageWidget> GetWidgetVersionHistory(Guid rootWidgetID) {
			var w = (from r in db.carrot_WidgetDatas
					 join rr in db.carrot_Widgets on r.Root_WidgetID equals rr.Root_WidgetID
					 orderby r.EditDate descending
					 where rr.Root_WidgetID == rootWidgetID
					 select new PageWidget(r)).ToList();

			return w;
		}

		public PageWidget GetWidgetVersion(Guid widgetDataID) {
			var w = (from r in db.carrot_WidgetDatas
					 where r.WidgetDataID == widgetDataID
					 select new PageWidget(r)).FirstOrDefault();

			return w;
		}

		public void RemoveVersions(List<Guid> lstDel) {

			var oldW = (from w in db.carrot_WidgetDatas
						orderby w.EditDate descending
						where lstDel.Contains(w.WidgetDataID)
						&& w.IsLatestVersion != true
						select w).ToList();

			if (oldW.Count > 0) {
				foreach (var c in oldW) {
					db.carrot_WidgetDatas.DeleteOnSubmit(c);
				}
				db.SubmitChanges();
			}
		}

		public void Delete(Guid widgetDataID) {
			var w = (from r in db.carrot_WidgetDatas
					 where r.WidgetDataID == widgetDataID
					 select r).FirstOrDefault();

			if (w != null) {
				db.carrot_WidgetDatas.DeleteOnSubmit(w);
				db.SubmitChanges();
			}
		}

		public void Disable(Guid rootWidgetID) {
			var w = (from r in db.carrot_Widgets
					 where r.Root_WidgetID == rootWidgetID
					 select r).FirstOrDefault();

			if (w != null) {
				w.WidgetActive = false;
				db.SubmitChanges();
			}
		}

		public void DeleteAll(Guid rootWidgetID) {

			var w1 = (from r in db.carrot_WidgetDatas
					  where r.Root_WidgetID == rootWidgetID
					  select r).ToList();

			var w2 = (from r in db.carrot_Widgets
					  where r.Root_WidgetID == rootWidgetID
					  select r).ToList();

			bool bPendingDel = false;

			if (w1 != null) {
				foreach (var w in w1) {
					db.carrot_WidgetDatas.DeleteOnSubmit(w);
					bPendingDel = true;
				}
			}

			if (w2 != null) {
				foreach (var w in w2) {
					db.carrot_Widgets.DeleteOnSubmit(w);
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
