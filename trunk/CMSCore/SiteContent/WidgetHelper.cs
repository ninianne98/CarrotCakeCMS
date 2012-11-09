using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
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

	public class WidgetHelper : IDisposable {

		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();
		//private CarrotCMSDataContext db = CompiledQueries.dbConn;


		public WidgetHelper() {
			//#if DEBUG
			//            db.Log = new DebugTextWriter();
			//#endif
		}

		public Widget Get(Guid rootWidgetID) {
			return new Widget(rootWidgetID);
		}

		internal static Widget MakeWidget(vw_carrot_Widget ww) {

			Widget w = new Widget();

			if (ww != null) {
				w.IsWidgetPendingDelete = false;

				w.WidgetDataID = ww.WidgetDataID;
				w.EditDate = ww.EditDate;
				w.IsLatestVersion = ww.IsLatestVersion;
				w.ControlProperties = ww.ControlProperties;

				w.Root_WidgetID = ww.Root_WidgetID;
				w.Root_ContentID = ww.Root_ContentID;
				w.WidgetOrder = ww.WidgetOrder;
				w.ControlPath = ww.ControlPath;
				w.PlaceholderName = ww.PlaceholderName;
				w.IsWidgetActive = ww.WidgetActive;
			}

			return w;
		}


		public List<Widget> GetWidgets(Guid rootContentID, bool bActiveOnly) {

			IQueryable<vw_carrot_Widget> items = CompiledQueries.cqGetLatestWidgets(db, rootContentID, bActiveOnly);

			List<Widget> w = (from r in items
							  select MakeWidget(r)).ToList();
			return w;
		}


		public void UpdateContentWidgets(Guid rootContentID) {

			var ww = (from rr in db.carrot_Widgets
					  orderby rr.WidgetOrder
					  where rr.Root_ContentID == rootContentID
					  && (rr.ControlPath.ToLower().Contains("/manage/ucgenericcontent.ascx")
							 || rr.ControlPath.ToLower().Contains("/manage/uctextcontent.ascx"))
					  select rr).ToList();

			bool bEdit = false;

			foreach (var w in ww) {
				bEdit = true;
				if (w.ControlPath.ToLower().Contains("/manage/ucgenericcontent.ascx")) {
					w.ControlPath = "CLASS:Carrotware.CMS.UI.Controls.ContentRichText, Carrotware.CMS.UI.Controls";
				}
				if (w.ControlPath.ToLower().Contains("/manage/uctextcontent.ascx")) {
					w.ControlPath = "CLASS:Carrotware.CMS.UI.Controls.ContentPlainText, Carrotware.CMS.UI.Controls";
				}
			}

			if (bEdit) {
				db.SubmitChanges();
			}
		}


		public List<Widget> GetWidgetVersionHistory(Guid rootWidgetID) {
			IQueryable<vw_carrot_Widget> items = CompiledQueries.cqGetWidgetVersionHistory_VW(db, rootWidgetID);

			List<Widget> w = (from r in items
							  select MakeWidget(r)).ToList();

			return w;
		}


		public Widget GetWidgetVersion(Guid widgetDataID) {
			Widget w = MakeWidget(CompiledQueries.cqGetWidgetDataByID_VW(db, widgetDataID));

			return w;
		}

		public void RemoveVersions(List<Guid> lstDel) {

			IQueryable<carrot_WidgetData> oldW = (from w in db.carrot_WidgetDatas
												  orderby w.EditDate descending
												  where lstDel.Contains(w.WidgetDataID)
												  && w.IsLatestVersion != true
												  select w);

			if (oldW.Count() > 0) {
				foreach (carrot_WidgetData c in oldW) {
					db.carrot_WidgetDatas.DeleteOnSubmit(c);
				}
				db.SubmitChanges();
			}
		}

		public void Delete(Guid widgetDataID) {
			carrot_WidgetData w = (from r in db.carrot_WidgetDatas
								   where r.WidgetDataID == widgetDataID
								   select r).FirstOrDefault();

			if (w != null) {
				db.carrot_WidgetDatas.DeleteOnSubmit(w);
				db.SubmitChanges();
			}
		}

		public void Disable(Guid rootWidgetID) {
			carrot_Widget w = (from r in db.carrot_Widgets
							   where r.Root_WidgetID == rootWidgetID
							   select r).FirstOrDefault();

			if (w != null) {
				w.WidgetActive = false;
				db.SubmitChanges();
			}
		}

		public void DeleteAll(Guid rootWidgetID) {

			IQueryable<carrot_WidgetData> w1 = (from r in db.carrot_WidgetDatas
												where r.Root_WidgetID == rootWidgetID
												select r);

			IQueryable<carrot_Widget> w2 = (from r in db.carrot_Widgets
											where r.Root_WidgetID == rootWidgetID
											select r);

			bool bPendingDel = false;

			if (w1 != null) {
				foreach (carrot_WidgetData w in w1) {
					db.carrot_WidgetDatas.DeleteOnSubmit(w);
					bPendingDel = true;
				}
			}

			if (w2 != null) {
				foreach (carrot_Widget w in w2) {
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
