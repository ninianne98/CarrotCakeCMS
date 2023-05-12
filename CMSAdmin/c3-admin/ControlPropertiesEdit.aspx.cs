﻿using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class ControlPropertiesEdit : AdminBasePage {
		public Guid guidWidget = Guid.Empty;
		public Guid guidContentID = Guid.Empty;
		public List<WidgetProps> lstProps = null;
		public List<ObjectProperty> lstDefProps = null;

		protected void Page_Load(object sender, EventArgs e) {
			Master.UsesSaved = true;
			Master.HideSave();

			guidWidget = GetGuidIDFromQuery();

			guidContentID = GetGuidPageIDFromQuery();

			cmsHelper.OverrideKey(guidContentID);

			Widget w = (from aw in cmsHelper.cmsAdminWidget
						where aw.Root_WidgetID == guidWidget
						orderby aw.WidgetOrder, aw.EditDate
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

				if (w.ControlPath.ToLowerInvariant().StartsWith("class:")) {
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

				List<ObjectProperty> props = new List<ObjectProperty>();
				List<ObjectProperty> props_tmp = new List<ObjectProperty>();

				if (widget is BaseUserControl) {
					props_tmp = ObjectProperty.GetTypeProperties(typeof(BaseUserControl));
					props = props.Union(props_tmp).ToList();
				}

				if (widget is BaseShellUserControl) {
					props_tmp = ObjectProperty.GetTypeProperties(typeof(BaseShellUserControl));
					props = props.Union(props_tmp).ToList();
				}

				if (widget is UserControl) {
					props_tmp = ObjectProperty.GetTypeProperties(typeof(UserControl));
					props = props.Union(props_tmp).ToList();
				}

				if (widget is IAdminModule) {
					var w1 = (IAdminModule)widget;
					w1.SiteID = SiteData.CurrentSiteID;
					props_tmp = ObjectProperty.GetTypeProperties(typeof(IAdminModule));
					props = props.Union(props_tmp).ToList();
				}

				if (widget is IWidget) {
					var w1 = (IWidget)widget;
					w1.SiteID = SiteData.CurrentSiteID;
					w1.PageWidgetID = w.Root_WidgetID;
					w1.RootContentID = w.Root_ContentID;
					props_tmp = ObjectProperty.GetTypeProperties(typeof(IWidget));
					props = props.Union(props_tmp).ToList();
				}

				if (widget is IWidgetEditStatus) {
					props_tmp = ObjectProperty.GetTypeProperties(typeof(IWidgetEditStatus));
					props = props.Union(props_tmp).ToList();
				}

				if (widget is IWidgetParmData) {
					props_tmp = ObjectProperty.GetTypeProperties(typeof(IWidgetParmData));
					props = props.Union(props_tmp).ToList();
				}

				if (widget is IWidgetRawData) {
					var w1 = (IWidgetRawData)widget;
					w1.RawWidgetData = w.ControlProperties;
					props_tmp = ObjectProperty.GetTypeProperties(typeof(IWidgetRawData));
					props = props.Union(props_tmp).ToList();
				}

				lstDefProps = ObjectProperty.GetObjectProperties(widget);

				List<string> limitedPropertyList = new List<string>();
				if (widget is IWidgetLimitedProperties) {
					limitedPropertyList = ((IWidgetLimitedProperties)(widget)).LimitedPropertyList;
				} else {
					limitedPropertyList = (from p in lstDefProps
										   select p.Name.ToLowerInvariant()).ToList();
				}
				if (limitedPropertyList != null && limitedPropertyList.Any()) {
					limitedPropertyList = (from p in limitedPropertyList
										   select p.ToLowerInvariant()).ToList();
				}

				var defprops = (from p in lstDefProps
								join l in limitedPropertyList on p.Name.ToLowerInvariant() equals l.ToLowerInvariant()
								where p.CanRead == true
								&& p.CanWrite == true
								&& !props.Contains(p)
								select p).ToList();

				GeneralUtilities.BindRepeater(rpProps, defprops);
			}
		}

		public string GetSavedValue(string sDefVal, string sName) {
			var pp = (from p in lstProps
					  where p.KeyName.ToLowerInvariant() == sName.ToLowerInvariant()
					  select p).FirstOrDefault();

			if (pp == null) {
				var dp = (from p in lstDefProps
						  where p.Name.ToLowerInvariant() == sName.ToLowerInvariant()
						  select p).FirstOrDefault();

				if (dp.DefValue != null) {
					sDefVal = dp.DefValue.ToString();

					if (dp.PropertyType == typeof(bool)) {
						bool vB = Convert.ToBoolean(dp.DefValue.ToString());
						sDefVal = vB.ToString();
					}
					if (dp.PropertyType == typeof(System.Drawing.Color)) {
						System.Drawing.Color vC = (System.Drawing.Color)dp.DefValue;
						sDefVal = System.Drawing.ColorTranslator.ToHtml(vC);
					}
				} else {
					sDefVal = string.Empty;
				}

				return sDefVal;
			} else {
				return pp.KeyValue;
			}
		}

		protected void rpProps_Bind(object sender, RepeaterItemEventArgs e) {
			if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item) {
				HiddenField hdnName = (HiddenField)e.Item.FindControl("hdnName");
				TextBox txtValue = (TextBox)e.Item.FindControl("txtValue");
				DropDownList ddlValue = (DropDownList)e.Item.FindControl("ddlValue");
				CheckBox chkValue = (CheckBox)e.Item.FindControl("chkValue");
				CheckBoxList chkValues = (CheckBoxList)e.Item.FindControl("chkValues");

				txtValue.Visible = true;
				string sName = hdnName.Value;

				ObjectProperty ListSourceProperty = new ObjectProperty();

				string sListSourcePropertyName = (from p in lstDefProps
												  where p.Name.ToLowerInvariant() == sName.ToLowerInvariant()
														&& !string.IsNullOrEmpty(p.CompanionSourceFieldName)
												  select p.CompanionSourceFieldName).FirstOrDefault();

				if (string.IsNullOrEmpty(sListSourcePropertyName)) {
					sListSourcePropertyName = string.Empty;
				}

				ListSourceProperty = (from p in lstDefProps
									  where p.CanRead == true
									  && p.CanWrite == false
									  && p.Name.ToLowerInvariant() == sListSourcePropertyName.ToLowerInvariant()
									  select p).FirstOrDefault();

				var dp = (from p in lstDefProps
						  where p.Name.ToLowerInvariant() == sName.ToLowerInvariant()
						  select p).FirstOrDefault();

				if (ListSourceProperty != null) {
					if (ListSourceProperty.DefValue is Dictionary<string, string>) {
						txtValue.Visible = false;

						//work with a drop down list, only allow one item in the drop down.
						if (dp.FieldMode == WidgetAttribute.FieldMode.DropDownList) {
							ddlValue.Visible = true;

							ddlValue.DataTextField = "Value";
							ddlValue.DataValueField = "Key";

							GeneralUtilities.BindListDefaultText(ddlValue, ListSourceProperty.DefValue, null, "Select Value", "");

							if (!string.IsNullOrEmpty(txtValue.Text)) {
								try {
									GeneralUtilities.SelectListValue(ddlValue, txtValue.Text);
								} catch { }
							}
						}

						// work with a checkbox list, allow more than one value
						if (dp.FieldMode == WidgetAttribute.FieldMode.CheckBoxList) {
							chkValues.Visible = true;

							chkValues.DataTextField = "Value";
							chkValues.DataValueField = "Key";

							GeneralUtilities.BindList(chkValues, ListSourceProperty.DefValue);

							// since this is a multi selected capable field, look for anything that starts with the
							// field name and has the delimeter trailing
							var pp = (from p in lstProps
									  where p.KeyName.ToLowerInvariant().StartsWith(sName.ToLowerInvariant() + "|")
									  select p).ToList();

							if (pp.Any()) {
								foreach (ListItem v in chkValues.Items) {
									v.Selected = (from p in pp
												  where p.KeyValue == v.Value
												  select p.KeyValue).Count() < 1 ? false : true;
								}
							}
						}
					}
				}

				if (dp.FieldMode == WidgetAttribute.FieldMode.RichHTMLTextBox
						|| dp.FieldMode == WidgetAttribute.FieldMode.MultiLineTextBox) {
					txtValue.Visible = true;
					txtValue.TextMode = TextBoxMode.MultiLine;
					txtValue.Columns = 60;
					txtValue.Rows = 5;
					if (dp.FieldMode == WidgetAttribute.FieldMode.RichHTMLTextBox) {
						txtValue.CssClass = "mceEditor";
					}
				}

				if (dp.PropertyType == typeof(Color) || dp.FieldMode == WidgetAttribute.FieldMode.ColorBox) {
					txtValue.CssClass = "color-field";
					txtValue.Width = new Unit("90px");
				}

				if (dp.PropertyType == typeof(bool) || dp.FieldMode == WidgetAttribute.FieldMode.CheckBox) {
					txtValue.Visible = false;
					chkValue.Visible = true;

					chkValue.Checked = Convert.ToBoolean(txtValue.Text);
				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			Widget w = (from aw in cmsHelper.cmsAdminWidget
						where aw.Root_WidgetID == guidWidget
						orderby aw.WidgetOrder, aw.EditDate
						select aw).FirstOrDefault();

			var props = new List<WidgetProps>();

			foreach (RepeaterItem r in rpProps.Items) {
				HiddenField hdnName = (HiddenField)r.FindControl("hdnName");
				TextBox txtValue = (TextBox)r.FindControl("txtValue");
				DropDownList ddlValue = (DropDownList)r.FindControl("ddlValue");
				CheckBox chkValue = (CheckBox)r.FindControl("chkValue");
				CheckBoxList chkValues = (CheckBoxList)r.FindControl("chkValues");

				var p = new WidgetProps();
				p.KeyName = hdnName.Value;

				if (ddlValue.Visible) {
					// drop down list detected, save the selected item
					p.KeyValue = ddlValue.SelectedValue;
				} else if (chkValue.Visible) {
					//boolean detected
					p.KeyValue = chkValue.Checked.ToString();
				} else if (chkValues.Visible) {
					//multiple selections are possible, since dictionary is used, insure key is unique by appending the ordinal with a | delimeter.
					p = null;
					int CheckedPosition = 0;
					foreach (ListItem v in chkValues.Items) {
						if (v.Selected) {
							var pp = new WidgetProps();
							pp.KeyName = hdnName.Value + "|" + CheckedPosition.ToString();
							pp.KeyValue = v.Value.ToString();
							props.Add(pp);
							CheckedPosition++;
						}
					}
				} else {
					//default, free text field
					p.KeyValue = txtValue.Text;
				}

				if (p != null) {
					props.Add(p);
				}
			}

			w.SaveDefaultControlProperties(props);
			w.EditDate = SiteData.CurrentSite.Now;

			List<Widget> lstPageWidgets = cmsHelper.cmsAdminWidget;
			lstPageWidgets.RemoveAll(x => x.Root_WidgetID == guidWidget);
			lstPageWidgets.Add(w);

			cmsHelper.cmsAdminWidget = lstPageWidgets;

			Master.ShowSave();
		}
	}
}