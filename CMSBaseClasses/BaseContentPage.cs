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

		private const string JSSCOPE = "GlossySeaGreen";


		protected Guid guidContentID = Guid.Empty;
		protected ContentPage pageContents = new ContentPage();
		protected List<PageWidget> pageWidgets = new List<PageWidget>();


		protected SiteData GetSite() {
			return siteHelper.Get(SiteData.CurrentSiteID);
		}

		protected override void OnLoad(EventArgs e) {

			//Response.Cache.SetNoServerCaching();
			//Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
			//var dtExpire = System.DateTime.Now.AddMinutes(2);
			//Response.Cache.SetExpires(dtExpire);

			base.OnLoad(e);
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

		protected void LoadPageControls(Control page) {

			string sGen = "\r\n\t<meta name=\"generator\" content=\"Carrotware CMS {0}\" />\r\n";

			var generator = new Literal();
			generator.Text = string.Format(sGen, CurrentDLLVersion);
			Page.Header.Controls.Add(generator);

			theSite = GetSite();

			if (theSite != null) {
				if (theSite.BlockIndex) {
					HtmlMeta metaNoCrawl = new HtmlMeta();
					metaNoCrawl.Name = "robots";
					metaNoCrawl.Content = "noindex,nofollow";
					Page.Header.Controls.Add(metaNoCrawl);
				}
			}


			string path = HttpContext.Current.Request.Path;
			path = path.ToLower();

			ContentPage filePage = null;

			if (path.Length < 3) {
				if (IsAdmin || IsEditor) {
					filePage = pageHelper.FindHome(SiteID, null);
				} else {
					filePage = pageHelper.FindHome(SiteID, true);
				}
			} else {
				var pageName = path;
				if (IsAdmin || IsEditor) {
					filePage = pageHelper.GetLatestContent(SiteID, null, pageName);
				} else {
					filePage = pageHelper.GetLatestContent(SiteID, true, pageName);
				}
			}

			if (filePage != null) {
				guidContentID = filePage.Root_ContentID;
			}

			pageContents = pageHelper.GetLatestContent(SiteID, guidContentID);

			pageWidgets = widgetHelper.GetWidgets(guidContentID);

			if (pageContents != null) {
				HtmlMeta metaDesc = new HtmlMeta();
				HtmlMeta metaKey = new HtmlMeta();

				metaDesc.Name = "description";
				metaKey.Name = "keywords";
				metaDesc.Content = string.IsNullOrEmpty(theSite.MetaDescription) ? pageContents.MetaDescription : theSite.MetaDescription;
				metaKey.Content = string.IsNullOrEmpty(theSite.MetaKeyword) ? pageContents.MetaKeyword : theSite.MetaKeyword;

				if (!string.IsNullOrEmpty(metaDesc.Content)) {
					Page.Header.Controls.Add(metaDesc);
				}
				if (!string.IsNullOrEmpty(metaKey.Content)) {
					Page.Header.Controls.Add(metaKey);
				}
			}


			Page.Title = string.Format(PageTitlePattern, theSite.SiteName, pageContents.TitleBar);

			if (!pageContents.PageActive) {
				if (IsAdmin || IsEditor) {
					Page.Title = string.Format(PageTitlePattern, "* UNPUBLISHED * " + theSite.SiteName, pageContents.TitleBar);
				}
			}

			if (AdvancedEditMode) {
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


			contCenter = new ContentContainer();
			contLeft = new ContentContainer();
			contRight = new ContentContainer();

			if (pageContents != null) {

				DateTime dtModified = pageContents.EditDate;
				string strModifed = dtModified.ToString("r");
				Response.AppendHeader("Last-Modified", strModifed);
				Response.Cache.SetLastModified(dtModified);

				//Response.Cache.SetCacheability(System.Web.HttpCacheability.Private);
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


					if (!AdvancedEditMode) {
						//string editLink = "<div style=\"background:#CDE3D6; padding:5px; border: 2px dashed #676F6A;\">"
						//	+ "<a style=\"color:#676F6A;padding:5px;margin:0px;font-weight: bold;\" target=\"_blank\" href=\"/Manage/PageAddEdit.aspx?id=" + pageContents.Root_ContentID.ToString() + "\">EDIT</a></div>\r\n";

						//contCenter.Text = editLink + pageContents.PageText;
						//contLeft.Text = editLink + pageContents.LeftPageText;
						//contRight.Text = editLink + pageContents.RightPageText;

						if (IsAdmin || IsEditor) {
							//Literal litEd = new Literal();
							//litEd.Text = "\r\n<div style=\"clear: both;\">&nbsp;</div>\r\n<div style=\"text-align: center; background:#CDE3D6; padding:5px; margin:5px; border: 2px dashed #676F6A;\">\r\n"
							//        + "<a style=\"color:#676F6A;padding:5px;margin:2px;font-weight: bold;\" target=\"_top\" href=\"" + CurrentScriptName + "?carrotedit=true\">ADVANCED EDIT</a>\r\n"
							//        + "</div>\r\n";
							//Page.Form.Controls.Add(litEd);

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
						//link.JQVersion = "1.6";
						//Page.Header.Controls.Add(link);
						Page.Header.Controls.AddAt(0, link);

						MarkWidgets(page, JSSCOPE, true);

					}
				}


				if (pageWidgets.Count > 0) {
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
								w.SiteID = SiteID;
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
								wes.IsBeingEdited = AdvancedEditMode;
							}

							if (AdvancedEditMode) {
								WidgetWrapper plcWrapper = new WidgetWrapper();
								plcWrapper.JQueryUIScope = JSSCOPE;
								plcWrapper.IsAdminMode = true;
								plcWrapper.ControlPath = theWidget.ControlPath;
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
