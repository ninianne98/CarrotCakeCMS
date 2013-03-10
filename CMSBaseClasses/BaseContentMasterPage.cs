using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.CMS.UI.Controls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Base {
	internal class BaseContentMasterPage : BaseMasterPage {

		protected ContentContainer contCenter { get; set; }
		protected ContentContainer contRight { get; set; }
		protected ContentContainer contLeft { get; set; }

		public ContentPage ThePage { get { return pageContents; } }
		public SiteData TheSite { get { return theSite; } }
		public List<Widget> ThePageWidgets { get { return pageWidgets; } }

		protected ControlUtilities cu = new ControlUtilities();
		protected ContentPageHelper pageHelper = new ContentPageHelper();
		protected SiteData siteHelper = new SiteData();
		protected WidgetHelper widgetHelper = new WidgetHelper();
		protected CMSConfigHelper cmsHelper = new CMSConfigHelper();

		protected ContentPage pageContents = null;
		protected SiteData theSite = null;
		protected List<Widget> pageWidgets = null;

		public Guid guidContentID = Guid.Empty;

		public bool IsPageTemplate = false;

		private int iCtrl = 0;
		public string CtrlId {
			get {
				return "WidgetCtrlId" + (iCtrl++);
			}
		}

		private int iCtrlW = 0;
		public string WrapCtrlId {
			get {
				return "WidgetWrapCtrlId" + (iCtrlW++);
			}
		}

		protected void AssignContentZones(ContentContainer pageArea, ContentContainer pageSource) {

			pageArea.IsAdminMode = pageSource.IsAdminMode;

			pageArea.Text = pageSource.Text;

			pageArea.ZoneChar = pageSource.ZoneChar;

			pageArea.DatabaseKey = pageSource.DatabaseKey;

			pageArea.TextZone = pageSource.TextZone;

		}

		protected void LoadData() {

			theSite = SiteData.CurrentSite;
			pageContents = null;
			pageWidgets = new List<Widget>();

			pageContents = SiteData.CurrentSite.GetCurrentPage();

			if (pageContents != null) {
				guidContentID = pageContents.Root_ContentID;
				pageWidgets = SiteData.CurrentSite.GetCurrentPageWidgets(guidContentID);
			}
		}

		protected void LoadPageControls(Control page) {

			this.Page.Header.Controls.Add(new Literal { Text = "\r\n" });

			HtmlMeta metaGenerator = new HtmlMeta();
			metaGenerator.Name = "generator";
			metaGenerator.Content = SiteData.CarrotCakeCMSVersion;
			this.Page.Header.Controls.Add(metaGenerator);
			this.Page.Header.Controls.Add(new Literal { Text = "\r\n" });

			if (guidContentID == SiteData.CurrentSiteID && SiteData.IsPageReal) {
				IsPageTemplate = true;
			}

			if (theSite != null && pageContents != null) {
				if (theSite.BlockIndex || pageContents.BlockIndex) {
					HtmlMeta metaNoCrawl = new HtmlMeta();
					metaNoCrawl.Name = "robots";
					metaNoCrawl.Content = "noindex,nofollow,noarchive";
					this.Page.Header.Controls.Add(metaNoCrawl);
					this.Page.Header.Controls.Add(new Literal { Text = "\r\n" });
				}
			}

			if (pageContents != null) {
				HtmlMeta metaDesc = new HtmlMeta();
				HtmlMeta metaKey = new HtmlMeta();

				metaDesc.Name = "description";
				metaKey.Name = "keywords";
				metaDesc.Content = string.IsNullOrEmpty(pageContents.MetaDescription) ? theSite.MetaDescription : pageContents.MetaDescription;
				metaKey.Content = string.IsNullOrEmpty(pageContents.MetaKeyword) ? theSite.MetaKeyword : pageContents.MetaKeyword;

				if (!string.IsNullOrEmpty(metaDesc.Content)) {
					this.Page.Header.Controls.Add(metaDesc);
					this.Page.Header.Controls.Add(new Literal { Text = "\r\n" });
				}

				if (!string.IsNullOrEmpty(metaKey.Content)) {
					this.Page.Header.Controls.Add(metaKey);
					this.Page.Header.Controls.Add(new Literal { Text = "\r\n" });
				}
			}

			contCenter = new ContentContainer();
			contLeft = new ContentContainer();
			contRight = new ContentContainer();

			if (pageContents != null) {

				//do stuff to page title
				string sTitleAddendum = pageHelper.GetBlogHeadingFromURL(theSite, SiteData.CurrentScriptName);
				if (!string.IsNullOrEmpty(sTitleAddendum)) {
					if (!string.IsNullOrEmpty(pageContents.PageHead)) {
						pageContents.PageHead = pageContents.PageHead.Trim() + ": " + sTitleAddendum;
					} else {
						pageContents.PageHead = sTitleAddendum;
					}
					pageContents.TitleBar = pageContents.TitleBar.Trim() + ": " + sTitleAddendum;
				}

				this.Page.Title = SetPageTitle(pageContents);

				DateTime dtModified = theSite.ConvertSiteTimeToLocalServer(pageContents.EditDate);
				string strModifed = dtModified.ToString("r");
				Response.AppendHeader("Last-Modified", strModifed);
				Response.Cache.SetLastModified(dtModified);

				DateTime dtExpire = DateTime.Now.AddMinutes(1);
				Response.Cache.SetExpires(dtExpire);

				contCenter.Text = pageContents.PageText;
				contLeft.Text = pageContents.LeftPageText;
				contRight.Text = pageContents.RightPageText;

				contCenter.DatabaseKey = pageContents.Root_ContentID;
				contLeft.DatabaseKey = pageContents.Root_ContentID;
				contRight.DatabaseKey = pageContents.Root_ContentID;

				pageContents = CMSConfigHelper.IdentifyLinkAsInactive(pageContents);

				if (this.Page.User.Identity.IsAuthenticated) {

					Response.Cache.SetNoServerCaching();
					Response.Cache.SetCacheability(HttpCacheability.NoCache);
					dtExpire = DateTime.Now.AddMinutes(-10);
					Response.Cache.SetExpires(dtExpire);

					if (!SecurityData.AdvancedEditMode) {

						if (SecurityData.IsAdmin || SecurityData.IsEditor) {
							if (!SiteData.IsPageSampler && !IsPageTemplate) {
								Control editor = this.Page.LoadControl(SiteFilename.EditNotifierControlPath);
								this.Page.Form.Controls.Add(editor);
							}
						}

					} else {

						contCenter.IsAdminMode = true;
						contLeft.IsAdminMode = true;
						contRight.IsAdminMode = true;

						contCenter.ZoneChar = "c";
						contLeft.ZoneChar = "l";
						contRight.ZoneChar = "r";

						contCenter.TextZone = ContentContainer.TextFieldZone.TextCenter;
						contLeft.TextZone = ContentContainer.TextFieldZone.TextLeft;
						contRight.TextZone = ContentContainer.TextFieldZone.TextRight;

						contCenter.Text = pageContents.PageText;
						contLeft.Text = pageContents.LeftPageText;
						contRight.Text = pageContents.RightPageText;

						Control editor = this.Page.LoadControl(SiteFilename.AdvancedEditControlPath);
						Page.Form.Controls.Add(editor);

						MarkWidgets(page, true);
					}
				}


				if (pageWidgets.Count > 0) {
					CMSConfigHelper cmsHelper = new CMSConfigHelper();

					//find each placeholder in use ONCE!
					List<LabeledControl> lstPlaceholders = (from d in pageWidgets
															where d.Root_ContentID == pageContents.Root_ContentID
															select new LabeledControl {
																ControlLabel = d.PlaceholderName,
																KeyControl = FindTheControl(d.PlaceholderName, page)
															}).Distinct().ToList();

					List<Widget> lstWidget = (from d in pageWidgets
											  where d.Root_ContentID == pageContents.Root_ContentID
												&& d.IsWidgetActive == true
												&& d.IsWidgetPendingDelete == false
											  select d).ToList();

					Assembly a = Assembly.GetExecutingAssembly();

					foreach (Widget theWidget in lstWidget) {

						WidgetContainer plcHolder = (WidgetContainer)(from d in lstPlaceholders
																	  where d.ControlLabel == theWidget.PlaceholderName
																	  select d.KeyControl).FirstOrDefault();
						if (plcHolder != null) {
							plcHolder.EnableViewState = true;
							Control widget = new Control();

							if (theWidget.ControlPath.EndsWith(".ascx")) {
								if (File.Exists(Server.MapPath(theWidget.ControlPath))) {
									try {
										widget = this.Page.LoadControl(theWidget.ControlPath);
									} catch (Exception ex) {
										Literal lit = new Literal();
										lit.Text = "<b>ERROR: " + theWidget.ControlPath + "</b> <br />\r\n" + ex.ToString();
										widget = lit;
									}
								} else {
									Literal lit = new Literal();
									lit.Text = "MISSING FILE: " + theWidget.ControlPath + "<br />\r\n";
									widget = lit;
								}
							}

							if (theWidget.ControlPath.ToLower().StartsWith("class:")) {
								try {
									string className = theWidget.ControlPath.Replace("CLASS:", "");
									Type t = Type.GetType(className);
									Object o = Activator.CreateInstance(t);

									if (o != null) {
										widget = o as Control;
									} else {
										Literal lit = new Literal();
										lit.Text = "OOPS: " + theWidget.ControlPath + "<br />\r\n";
										widget = lit;
									}
								} catch (Exception ex) {
									Literal lit = new Literal();
									lit.Text = "<b>ERROR: " + theWidget.ControlPath + "</b> <br />\r\n" + ex.ToString();
									widget = lit;
								}
							}

							widget.EnableViewState = true;

							IWidget w = null;
							if (widget is IWidget) {
								w = widget as IWidget;
								w.SiteID = SiteData.CurrentSiteID;
								w.PageWidgetID = theWidget.Root_WidgetID;
								w.RootContentID = theWidget.Root_ContentID;
							}

							if (widget is IWidgetParmData) {
								IWidgetParmData wp = widget as IWidgetParmData;
								List<WidgetProps> lstProp = theWidget.ParseDefaultControlProperties();

								wp.PublicParmValues = lstProp.ToDictionary(t => t.KeyName, t => t.KeyValue);
							}

							if (widget is IWidgetRawData) {
								IWidgetRawData wp = widget as IWidgetRawData;
								wp.RawWidgetData = theWidget.ControlProperties;
							}

							if (widget is IWidgetEditStatus) {
								IWidgetEditStatus wes = widget as IWidgetEditStatus;
								wes.IsBeingEdited = SecurityData.AdvancedEditMode;
							}

							if (SecurityData.AdvancedEditMode) {
								WidgetWrapper plcWrapper = new WidgetWrapper();

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
								plcWrapper.DatabaseKey = theWidget.Root_WidgetID;

								plcWrapper.Controls.Add(widget);
								plcHolder.Controls.Add(plcWrapper);

								plcWrapper.ID = WrapCtrlId;

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

							widget.ID = CtrlId;

						}
					}

					cmsHelper.Dispose();
				}
			}
		}

		private string SetPageTitle(ContentPage pageData) {
			string sPrefix = "";

			if (!pageData.PageActive) {
				sPrefix = "* UNPUBLISHED * ";
			}
			if (pageData.RetireDate < theSite.Now) {
				sPrefix = "* RETIRED * ";
			}
			if (pageData.GoLiveDate > theSite.Now) {
				sPrefix = "* UNRELEASED * ";
			}
			string sPattern = sPrefix + SiteData.CurrentTitlePattern;

			string sPageTitle = string.Format(sPattern, theSite.SiteName, pageData.TitleBar, pageData.PageHead, pageData.NavMenuText);

			return sPageTitle;
		}

		protected void MarkWidgets(Control X, bool bAdmin) {
			//add the command click event to the link buttons on the datagrid heading
			foreach (Control c in X.Controls) {
				if (c is WidgetContainer) {
					WidgetContainer ph = (WidgetContainer)c;
					ph.IsAdminMode = bAdmin;
				} else {
					MarkWidgets(c, bAdmin);
				}
			}
		}

		protected void AssignControls(Page ThePage) {

			if (pageContents != null) {

				cu.ResetFind();
				Control ctrlHead = cu.FindControl("litPageHeading", ThePage);
				if (ctrlHead != null && ctrlHead is ITextControl) {
					((ITextControl)ctrlHead).Text = pageContents.PageHead;
				}

				cu.ResetFind();
				Control ctrlCenter = cu.FindControl("BodyCenter", ThePage);
				if (ctrlCenter != null && ctrlCenter is ContentContainer) {
					AssignContentZones((ContentContainer)ctrlCenter, contCenter);
				}

				cu.ResetFind();
				Control ctrlLeft = cu.FindControl("BodyLeft", ThePage);
				if (ctrlLeft != null && ctrlLeft is ContentContainer) {
					AssignContentZones((ContentContainer)ctrlLeft, contLeft);
				}

				cu.ResetFind();
				Control ctrlRight = cu.FindControl("BodyRight", ThePage);
				if (ctrlRight != null && ctrlRight is ContentContainer) {
					AssignContentZones((ContentContainer)ctrlRight, contRight);
				}
			}
		}


	}
}
