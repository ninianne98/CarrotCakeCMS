using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Profile;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using Carrotware.Web.UI.Controls;
using Carrotware.CMS.Interface;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/
namespace Carrotware.CMS.UI.Base {
	public class BaseContentPage : BasePage {

		protected ContentContainer contCenter { get; set; }
		protected ContentContainer contRight { get; set; }
		protected ContentContainer contLeft { get; set; }

		protected SiteData theSite { get; set; }

		protected Guid guidContentID = Guid.Empty;

		protected ContentPage pageContents = new ContentPage();
		protected List<PageWidget> pageWidgets = new List<PageWidget>();


		protected string PageTitlePattern {
			get {
				string x = "{0} - {1}";
				try { x = System.Configuration.ConfigurationManager.AppSettings["PageTitlePattern"].ToString(); } catch { }
				return x;
			}
		}

		private int iCtrl = 0;

		private string CtrlId {
			get {
				return "WidgetID_" + (iCtrl++);
			}
		}

		private const string JSSCOPE = "cmsGlossySeaGreen";

		protected SiteData GetSite() {
			return siteHelper.Get(SiteData.CurrentSiteID);
		}


		protected override void OnInit(EventArgs e) {

			this.PreRenderComplete += new EventHandler(hand_PreRenderComplete);
			base.OnInit(e);
		}

		void hand_PreRenderComplete(object sender, EventArgs e) {
			try {
				HttpContext.Current.RewritePath(HttpContext.Current.Request.Path,
						HttpContext.Current.Request.PathInfo,
						HttpContext.Current.Request.QueryString.ToString());
			} catch (Exception ex) { }
		}


		protected void AssignContentZones(ContentContainer pageArea, ContentContainer pageSource) {

			pageArea.JQueryUIScope = pageSource.JQueryUIScope;

			pageArea.IsAdminMode = pageSource.IsAdminMode;

			pageArea.Text = pageSource.Text;

			pageArea.ZoneChar = pageSource.ZoneChar;

			pageArea.DatabaseKey = pageSource.DatabaseKey;
		}


		protected void LoadPageControls(Control page) {

			HtmlMeta metaGenerator = new HtmlMeta();
			metaGenerator.Name = "generator";
			metaGenerator.Content = string.Format("CarrotCake CMS {0}", CurrentDLLVersion);
			Page.Header.Controls.Add(metaGenerator);

			theSite = GetSite();

			if (theSite != null) {
				if (theSite.BlockIndex) {
					HtmlMeta metaNoCrawl = new HtmlMeta();
					metaNoCrawl.Name = "robots";
					metaNoCrawl.Content = "noindex,nofollow";
					Page.Header.Controls.Add(metaNoCrawl);
				}
			}

			string path = SiteData.CurrentScriptName.ToLower();

			pageContents = null;

			if (path.Length < 3) {
				if (SiteData.IsAdmin || SiteData.IsEditor) {
					pageContents = pageHelper.FindHome(SiteData.CurrentSiteID, null);
				} else {
					pageContents = pageHelper.FindHome(SiteData.CurrentSiteID, true);
				}
			} else {
				var pageName = path;
				if (SiteData.IsAdmin || SiteData.IsEditor) {
					pageContents = pageHelper.GetLatestContent(SiteData.CurrentSiteID, null, pageName);
				} else {
					pageContents = pageHelper.GetLatestContent(SiteData.CurrentSiteID, true, pageName);
				}
			}

			if (pageContents != null) {
				guidContentID = pageContents.Root_ContentID;
			}

			pageWidgets = widgetHelper.GetWidgets(guidContentID);

			if (pageContents != null) {
				HtmlMeta metaDesc = new HtmlMeta();
				HtmlMeta metaKey = new HtmlMeta();

				metaDesc.Name = "description";
				metaKey.Name = "keywords";
				metaDesc.Content = string.IsNullOrEmpty(pageContents.MetaDescription) ? theSite.MetaDescription : pageContents.MetaDescription;
				metaKey.Content = string.IsNullOrEmpty(pageContents.MetaKeyword) ? theSite.MetaKeyword : pageContents.MetaKeyword;

				if (!string.IsNullOrEmpty(metaDesc.Content)) {
					Page.Header.Controls.Add(metaDesc);
				}
				if (!string.IsNullOrEmpty(metaKey.Content)) {
					Page.Header.Controls.Add(metaKey);
				}
			}

			if (SiteData.AdvancedEditMode) {
				if (cmsHelper.cmsAdminContent == null) {
					cmsHelper.cmsAdminContent = pageContents;
					cmsHelper.cmsAdminWidget = (from w in pageWidgets
												orderby w.WidgetOrder
												select w).ToList();
				} else {
					pageContents = cmsHelper.cmsAdminContent;
					pageWidgets = (from w in cmsHelper.cmsAdminWidget
								   orderby w.WidgetOrder
								   select w).ToList();
				}
			} else {
				cmsHelper.cmsAdminContent = null;
			}

			SetPageTitle(pageContents);

			contCenter = new ContentContainer();
			contLeft = new ContentContainer();
			contRight = new ContentContainer();

			if (pageContents != null) {

				DateTime dtModified = pageContents.EditDate;
				string strModifed = dtModified.ToString("r");
				Response.AppendHeader("Last-Modified", strModifed);
				Response.Cache.SetLastModified(dtModified);

				DateTime dtExpire = System.DateTime.Now.AddMinutes(1);
				Response.Cache.SetExpires(dtExpire);

				contCenter.Text = pageContents.PageText;
				contLeft.Text = pageContents.LeftPageText;
				contRight.Text = pageContents.RightPageText;

				contCenter.DatabaseKey = pageContents.Root_ContentID;
				contLeft.DatabaseKey = pageContents.Root_ContentID;
				contRight.DatabaseKey = pageContents.Root_ContentID;

				if (Page.User.Identity.IsAuthenticated) {

					Response.Cache.SetNoServerCaching();
					Response.Cache.SetCacheability(HttpCacheability.NoCache);
					dtExpire = DateTime.Now.AddMinutes(-10);
					Response.Cache.SetExpires(dtExpire);

					if (!SiteData.AdvancedEditMode) {

						if (SiteData.IsAdmin || SiteData.IsEditor) {

							Control editor = Page.LoadControl("~/Manage/ucEditNotifier.ascx");
							Page.Form.Controls.Add(editor);
						}

					} else {

						contCenter.IsAdminMode = true;
						contLeft.IsAdminMode = true;
						contRight.IsAdminMode = true;

						contCenter.JQueryUIScope = JSSCOPE;
						contLeft.JQueryUIScope = JSSCOPE;
						contRight.JQueryUIScope = JSSCOPE;

						contCenter.ZoneChar = "c";
						contLeft.ZoneChar = "l";
						contRight.ZoneChar = "r";

						contCenter.Text = pageContents.PageText;
						contLeft.Text = pageContents.LeftPageText;
						contRight.Text = pageContents.RightPageText;

						Control editor = Page.LoadControl("~/Manage/ucAdvancedEdit.ascx");
						Page.Form.Controls.Add(editor);

						jquery link = new jquery();
						Page.Header.Controls.AddAt(0, link);

						MarkWidgets(page, JSSCOPE, true);
					}
				}


				if (pageWidgets.Count > 0) {
					CMSConfigHelper cmsHelper = new CMSConfigHelper();

					//find each placeholder in use ONCE!
					List<KeyedControl> lstPlaceholders = (from d in pageWidgets
														  where d.Root_ContentID == pageContents.Root_ContentID
														  select new KeyedControl {
															  KeyName = d.PlaceholderName,
															  KeyControl = FindTheControl(d.PlaceholderName, page)
														  }).Distinct().ToList();

					List<PageWidget> lstWidget = (from d in pageWidgets
												  where d.Root_ContentID == pageContents.Root_ContentID
												  select d).ToList();

					foreach (var theWidget in lstWidget) {

						//WidgetContainer plcHolder = (WidgetContainer)FindTheControl(theWidget.PlaceholderName, page);
						WidgetContainer plcHolder = (WidgetContainer)(from d in lstPlaceholders
																	  where d.KeyName == theWidget.PlaceholderName
																	  select d.KeyControl).FirstOrDefault();

						if (plcHolder != null) {
							Control widget = new Control();

							if (theWidget.ControlPath.EndsWith(".ascx")) {
								if (File.Exists(Server.MapPath(theWidget.ControlPath))) {
									try {
										widget = Page.LoadControl(theWidget.ControlPath);
									} catch (Exception ex) {
										var lit = new Literal();
										lit.Text = "<b>ERROR: " + theWidget.ControlPath + "</b> <br />\r\n" + ex.ToString();
										widget = lit;
									}
								} else {
									var lit = new Literal();
									lit.Text = "MISSING FILE: " + theWidget.ControlPath;
									widget = lit;
								}
							}

							if (theWidget.ControlPath.ToLower().StartsWith("class:")) {
								try {
									Assembly a = Assembly.GetExecutingAssembly();
									var className = theWidget.ControlPath.Replace("CLASS:", "");
									Type t = Type.GetType(className);
									Object o = Activator.CreateInstance(t);

									if (o != null) {
										widget = o as Control;
									} else {
										var lit = new Literal();
										lit.Text = "OOPS: " + theWidget.ControlPath;
										widget = lit;
									}
								} catch (Exception ex) {
									var lit = new Literal();
									lit.Text = "<b>ERROR: " + theWidget.ControlPath + "</b> <br />\r\n" + ex.ToString();
									widget = lit;
								}
							}

							widget.ID = CtrlId;

							IWidget w = null;
							if (widget is IWidget) {
								w = widget as IWidget;
								w.SiteID = SiteData.CurrentSiteID;
								w.PageWidgetID = theWidget.PageWidgetID;
								w.RootContentID = theWidget.Root_ContentID;
							}

							if (widget is IWidgetParmData) {
								var wp = widget as IWidgetParmData;

								var lstProp = theWidget.ParseDefaultControlProperties();

								wp.PublicParmValues = lstProp.ToDictionary(t => t.KeyName, t => t.KeyValue);
							}

							if (widget is IWidgetEditStatus) {
								var wes = widget as IWidgetEditStatus;
								wes.IsBeingEdited = SiteData.AdvancedEditMode;
							}

							if (SiteData.AdvancedEditMode) {
								WidgetWrapper plcWrapper = new WidgetWrapper();
								plcWrapper.JQueryUIScope = JSSCOPE;
								plcWrapper.IsAdminMode = true;
								plcWrapper.ControlPath = theWidget.ControlPath;
								plcWrapper.ControlTitle = theWidget.ControlPath;

								CMSPlugin plug = (from p in cmsHelper.ToolboxPlugins
												  where p.FilePath.ToLower() == plcWrapper.ControlPath.ToLower()
												  select p).FirstOrDefault();

								if (plug != null) {
									plcWrapper.ControlTitle = plug.Caption;
								}

								plcWrapper.Order = theWidget.WidgetOrder;
								plcWrapper.DatabaseKey = theWidget.PageWidgetID;

								plcWrapper.Controls.Add(widget);
								plcHolder.Controls.Add(plcWrapper);

								if (w != null) {
									if (w.EnableEdit) {
										string sScript = w.JSEditFunction;
										if (string.IsNullOrEmpty(sScript)) {
											sScript = "cmsGenericEdit('" + pageContents.Root_ContentID + "','" + plcWrapper.DatabaseKey + "')";
										}

										plcWrapper.JSEditFunction = sScript;
									}
								}
							} else {
								plcHolder.Controls.Add(widget);
							}

						}
					}

					cmsHelper.Dispose();
				}
			}
		}


		private void SetPageTitle(ContentPage pageData) {

			Page.Title = string.Format(PageTitlePattern, theSite.SiteName, pageData.TitleBar);

			if (!pageData.PageActive) {
				if (SiteData.IsAdmin || SiteData.IsEditor) {
					Page.Title = string.Format(PageTitlePattern, "* UNPUBLISHED * " + theSite.SiteName, pageData.TitleBar);
				}
			}
		}

		protected void MarkWidgets(Control X, string sJQScope, bool bAdmin) {
			//add the command click event to the link buttons on the datagrid heading
			foreach (Control c in X.Controls) {
				if (c is WidgetContainer) {
					WidgetContainer ph = (WidgetContainer)c;
					ph.IsAdminMode = bAdmin;
					ph.JQueryUIScope = sJQScope;
				} else {
					MarkWidgets(c, sJQScope, bAdmin);
				}
			}
		}


	}
}
