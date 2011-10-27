﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.CMS.UI.Base;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class ControlPropertiesEdit : AdminBasePage {

		public Guid guidWidget = Guid.Empty;
		public Guid guidPage = Guid.Empty;
		public List<WidgetProps> lstProps = null;
		public List<ObjectProperty> lstDefProps = null;


		protected void Page_Load(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				guidWidget = new Guid(Request.QueryString["id"].ToString());
			}

			if (!string.IsNullOrEmpty(Request.QueryString["pageid"])) {
				guidPage = new Guid(Request.QueryString["pageid"].ToString());
			}

			var pageContents = pageHelper.GetLatestContent(SiteID, guidPage);
			cmsHelper.OverrideKey(pageContents.FileName);


			PageWidget w = (from aw in cmsHelper.cmsAdminWidget
							where aw.PageWidgetID == guidWidget
							orderby aw.WidgetOrder
							select aw).FirstOrDefault();


			if (!IsPostBack) {

				lstProps = w.ParseDefaultControlProperties();

				Control widget = new Control();

				if (w.ControlPath.EndsWith(".ascx")) {
					try {
						widget = Page.LoadControl(w.ControlPath);
					} catch (Exception ex) {
					}
				}

				if (w.ControlPath.ToLower().StartsWith("class:")) {
					try {
						Assembly a = Assembly.GetExecutingAssembly();
						var className = w.ControlPath.Replace("CLASS:", "");
						Type t = Type.GetType(className);
						Object o = Activator.CreateInstance(t);
						if (o != null) {
							widget = o as Control;
						}
					} catch (Exception ex) {
					}
				}

				lstDefProps = cmsHelper.GetProperties(widget);
				List<ObjectProperty> props1 = new List<ObjectProperty>();
				List<ObjectProperty> props2 = new List<ObjectProperty>();
				List<ObjectProperty> props3 = new List<ObjectProperty>();
				List<ObjectProperty> props4 = new List<ObjectProperty>();
				List<ObjectProperty> props5 = new List<ObjectProperty>();

				if (widget is Carrotware.CMS.UI.Base.BaseUserControl) {
					props1 = cmsHelper.GetTypeProperties(typeof(Carrotware.CMS.UI.Base.BaseUserControl));
				}
				if (widget is Carrotware.CMS.Interface.IWidget) {
					props2 = cmsHelper.GetTypeProperties(typeof(Carrotware.CMS.Interface.IWidget));
				}

				if (widget is Carrotware.CMS.Interface.BaseShellUserControl) {
					props3 = cmsHelper.GetTypeProperties(typeof(Carrotware.CMS.Interface.BaseShellUserControl));
				}
				if (widget is System.Web.UI.UserControl) {
					props4 = cmsHelper.GetTypeProperties(typeof(System.Web.UI.UserControl));
				}
				if (widget is Carrotware.CMS.Interface.IWidgetParmData) {
					props5 = cmsHelper.GetTypeProperties(typeof(Carrotware.CMS.Interface.IWidgetParmData));
				}

				rpProps.DataSource = (from p in lstDefProps
									  where p.CanRead == true
									  && p.CanWrite == true
									  && !props1.Contains(p)
									  && !props2.Contains(p)
									  && !props3.Contains(p)
									  && !props4.Contains(p)
									  && !props5.Contains(p)
									  select p).ToList();

				rpProps.DataBind();
			}


		}

		public string GetSavedValue(string sDefVal, string sName) {

			var pp = (from p in lstProps
					  where p.KeyName.ToLower() == sName.ToLower()
					  select p).FirstOrDefault();

			if (pp == null) {

				var dp = (from p in lstDefProps
						  where p.Name.ToLower() == sName.ToLower()
						  select p).FirstOrDefault();

				string sType = dp.PropertyType.ToString().ToLower();
				if (dp.DefValue != null) {
					sDefVal = dp.DefValue.ToString();
					switch (sType) {
						case "system.boolean":
							bool vB = Convert.ToBoolean(dp.DefValue.ToString());
							sDefVal = vB.ToString();
							break;
						case "system.drawing.color":
							System.Drawing.Color vC = (System.Drawing.Color)dp.DefValue;
							sDefVal = System.Drawing.ColorTranslator.ToHtml(vC);
							break;
						default:
							sDefVal = dp.DefValue.ToString();
							break;
					}
				} else {
					sDefVal = "";
				}

				return sDefVal;
			} else {
				return pp.KeyValue;
			}
		}


		protected void btnSave_Click(object sender, EventArgs e) {

			PageWidget w = (from aw in cmsHelper.cmsAdminWidget
							where aw.PageWidgetID == guidWidget
							orderby aw.WidgetOrder
							select aw).FirstOrDefault();

			var props = new List<WidgetProps>();

			foreach (RepeaterItem r in rpProps.Items) {
				HiddenField hdnName = (HiddenField)r.FindControl("hdnName");
				TextBox txtValue = (TextBox)r.FindControl("txtValue");

				var p = new WidgetProps();
				p.KeyName = hdnName.Value;
				p.KeyValue = txtValue.Text;
				props.Add(p);
			}

			w.SaveDefaultControlProperties(props);

			var lst = cmsHelper.cmsAdminWidget;
			lst.RemoveAll(x => x.PageWidgetID == guidWidget);
			lst.Add(w);

			cmsHelper.cmsAdminWidget = lst;

		}


	}
}
