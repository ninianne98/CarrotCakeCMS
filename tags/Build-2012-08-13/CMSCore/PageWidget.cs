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

	public class PageWidget : IDisposable {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public PageWidget() { }

		public PageWidget(Guid rootWidgetID) {

			var w = (from r in db.carrot_WidgetDatas
					 where r.Root_WidgetID == rootWidgetID
						&& r.IsLatestVersion == true
					 select r).FirstOrDefault();

			SetVals(w);
		}

		public void LoadPageWidgetVersion(Guid widgetDataID) {

			var w = (from r in db.carrot_WidgetDatas
					 where r.WidgetDataID == widgetDataID
					 select r).FirstOrDefault();

			SetVals(w);
		}

		public PageWidget(carrot_WidgetData w) {
			SetVals(w);
		}

		private void SetVals(carrot_WidgetData w) {

			var ww = db.carrot_Widgets.Where(x => x.Root_WidgetID == w.Root_WidgetID).FirstOrDefault();

			this.IsWidgetPendingDelete = false;

			this.WidgetDataID = w.WidgetDataID;
			this.EditDate = w.EditDate;
			this.IsLatestVersion = w.IsLatestVersion;
			this.ControlProperties = w.ControlProperties;

			if (ww != null) {
				this.Root_WidgetID = ww.Root_WidgetID;
				this.Root_ContentID = ww.Root_ContentID;
				this.WidgetOrder = ww.WidgetOrder;
				this.ControlPath = ww.ControlPath;
				this.PlaceholderName = ww.PlaceholderName;
				this.IsWidgetActive = ww.WidgetActive;
			}
		}


		public string ControlPath { get; set; }
		public string ControlProperties { get; set; }
		public Guid WidgetDataID { get; set; }
		public Guid Root_WidgetID { get; set; }
		public string PlaceholderName { get; set; }
		public Guid Root_ContentID { get; set; }
		public int WidgetOrder { get; set; }
		public bool? IsLatestVersion { get; set; }
		public bool IsWidgetActive { get; set; }
		public bool IsWidgetPendingDelete { get; set; }
		public DateTime EditDate { get; set; }


		public void Save() {

			if (!this.IsWidgetPendingDelete) {
				var w = (from r in db.carrot_Widgets
						 orderby r.WidgetOrder
						 where r.Root_WidgetID == this.Root_WidgetID
						 select r).FirstOrDefault();

				bool bAdd = false;
				if (w == null) {
					bAdd = true;
					w = new carrot_Widget();
				}

				if (this.Root_WidgetID == Guid.Empty) {
					this.Root_WidgetID = Guid.NewGuid();
				}

				w.Root_WidgetID = this.Root_WidgetID;

				w.WidgetOrder = this.WidgetOrder;
				w.Root_ContentID = this.Root_ContentID;
				w.PlaceholderName = this.PlaceholderName;
				w.ControlPath = this.ControlPath;
				w.WidgetActive = this.IsWidgetActive;

				var wd = new carrot_WidgetData();
				wd.Root_WidgetID = w.Root_WidgetID;
				wd.WidgetDataID = Guid.NewGuid();
				wd.IsLatestVersion = true;
				wd.ControlProperties = this.ControlProperties;
				wd.EditDate = DateTime.Now;

				var oldWD = (from ww in db.carrot_WidgetDatas
							 where ww.Root_WidgetID == w.Root_WidgetID
								&& ww.IsLatestVersion == true
							 select ww).FirstOrDefault();

				//only add a new entry if the widget has some sort of change in the data stored.
				if (oldWD != null) {
					if (oldWD.ControlProperties != wd.ControlProperties) {
						oldWD.IsLatestVersion = false;
						db.carrot_WidgetDatas.InsertOnSubmit(wd);
					}
				} else {
					db.carrot_WidgetDatas.InsertOnSubmit(wd);
				}

				if (bAdd) {
					db.carrot_Widgets.InsertOnSubmit(w);
				}

				db.SubmitChanges();

			} else {

				DeleteAll();

			}
		}


		public void Delete() {
			var w = (from r in db.carrot_WidgetDatas
					 where r.WidgetDataID == this.WidgetDataID
					 select r).FirstOrDefault();

			if (w != null) {
				db.carrot_WidgetDatas.DeleteOnSubmit(w);
				db.SubmitChanges();
			}
		}

		public void DeleteAll() {

			var w1 = (from r in db.carrot_WidgetDatas
					  where r.Root_WidgetID == this.Root_WidgetID
					  select r).ToList();

			var w2 = (from r in db.carrot_Widgets
					  where r.Root_WidgetID == this.Root_WidgetID
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


		public void Disable() {
			var w = (from r in db.carrot_Widgets
					 where r.Root_WidgetID == this.Root_WidgetID
					 select r).FirstOrDefault();

			if (w != null) {
				w.WidgetActive = false;
				db.SubmitChanges();
			}
		}

		public List<WidgetProps> ParseDefaultControlProperties() {
			List<WidgetProps> props = new List<WidgetProps>();
			var sProps = this.ControlProperties;

			if (!string.IsNullOrEmpty(sProps)) {
				if (sProps.StartsWith("<?xml")) {
					System.IO.StringReader stream = new System.IO.StringReader(sProps);

					DataSet ds = new DataSet();
					ds.ReadXml(stream);

					props = (from d in ds.Tables[0].AsEnumerable()
							 select new WidgetProps {
								 KeyName = d.Field<string>("KeyName"),
								 KeyValue = d.Field<string>("KeyValue")
							 }).ToList();
				}
			}

			return props;
		}


		public void SaveDefaultControlProperties(List<WidgetProps> props) {

			DataSet ds = new DataSet("DefaultControlProperties");
			DataTable dt = new DataTable("ControlProperties");
			DataColumn dc1 = new DataColumn("KeyName", typeof(System.String));
			DataColumn dc2 = new DataColumn("KeyValue", typeof(System.String));
			dt.Columns.Add(dc1);
			dt.Columns.Add(dc2);
			ds.Tables.Add(dt);

			foreach (var p in props) {
				DataRow newRow = ds.Tables["ControlProperties"].NewRow();
				newRow["KeyName"] = p.KeyName;
				newRow["KeyValue"] = p.KeyValue;
				ds.Tables["ControlProperties"].Rows.Add(newRow);
			}

			ds.AcceptChanges();

			this.ControlProperties = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>     " + ds.GetXml();

		}

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion
	}

	public class WidgetProps {

		public WidgetProps() { }

		public string KeyName { get; set; }
		public string KeyValue { get; set; }

	}


}
