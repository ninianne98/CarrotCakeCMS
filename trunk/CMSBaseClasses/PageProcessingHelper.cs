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
	public class PageProcessingHelper {

		public PageProcessingHelper() {
			this.CurrentWebPage = null;
		}

		public PageProcessingHelper(Page webPage) {
			this.CurrentWebPage = webPage;
		}

		protected Page CurrentWebPage { get; set; }

		protected ContentContainer contCenter { get; set; }
		protected ContentContainer contRight { get; set; }
		protected ContentContainer contLeft { get; set; }

		public ContentPage ThePage { get { return pageContents; } }
		public SiteData TheSite { get { return theSite; } }
		public List<Widget> ThePageWidgets { get { return pageWidgets; } }

		protected ControlUtilities cu = new ControlUtilities();

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

		public void AssignContentZones(ContentContainer pageArea, ContentContainer pageSource) {

			pageArea.IsAdminMode = pageSource.IsAdminMode;

			pageArea.Text = pageSource.Text;

			pageArea.ZoneChar = pageSource.ZoneChar;

			pageArea.DatabaseKey = pageSource.DatabaseKey;

			pageArea.TextZone = pageSource.TextZone;

		}

		public void LoadData() {

			theSite = SiteData.CurrentSite;
			pageContents = null;
			pageWidgets = new List<Widget>();

			pageContents = SiteData.GetCurrentPage();

			if (pageContents != null) {
				guidContentID = pageContents.Root_ContentID;
				pageWidgets = SiteData.GetCurrentPageWidgets(guidContentID);
			}
		}

		public void LoadPageControls() {

			this.CurrentWebPage.Header.Controls.Add(new Literal { Text = "\r\n" });

			List<HtmlMeta> lstMD = GetHtmlMeta(this.CurrentWebPage.Header);

			HtmlMeta metaGenerator = new HtmlMeta();
			metaGenerator.Name = "generator";
			metaGenerator.Content = SiteData.CarrotCakeCMSVersion;
			this.CurrentWebPage.Header.Controls.Add(metaGenerator);
			this.CurrentWebPage.Header.Controls.Add(new Literal { Text = "\r\n" });

			if (guidContentID == SiteData.CurrentSiteID && SiteData.IsPageReal) {
				IsPageTemplate = true;
			}

			if (theSite != null && pageContents != null) {
				if (theSite.BlockIndex || pageContents.BlockIndex) {
					bool bCrawlExist = false;
					HtmlMeta metaNoCrawl = new HtmlMeta();
					metaNoCrawl.Name = "robots";

					if (lstMD.Where(x => x.Name == "robots").Count() > 0) {
						metaNoCrawl = lstMD.Where(x => x.Name == "robots").FirstOrDefault();
						bCrawlExist = true;
					}

					metaNoCrawl.Content = "noindex,nofollow,noarchive";

					if (!bCrawlExist) {
						this.CurrentWebPage.Header.Controls.Add(metaNoCrawl);
						this.CurrentWebPage.Header.Controls.Add(new Literal { Text = "\r\n" });
					}
				}
			}

			if (pageContents != null) {
				HtmlMeta metaDesc = new HtmlMeta();
				HtmlMeta metaKey = new HtmlMeta();
				bool bDescExist = false;
				bool bKeyExist = false;

				if (lstMD.Where(x => x.Name == "description").Count() > 0) {
					metaDesc = lstMD.Where(x => x.Name == "description").FirstOrDefault();
					bDescExist = true;
				}
				if (lstMD.Where(x => x.Name == "keywords").Count() > 0) {
					metaKey = lstMD.Where(x => x.Name == "keywords").FirstOrDefault();
					bKeyExist = true;
				}

				metaDesc.Name = "description";
				metaKey.Name = "keywords";
				metaDesc.Content = string.IsNullOrEmpty(pageContents.MetaDescription) ? theSite.MetaDescription : pageContents.MetaDescription;
				metaKey.Content = string.IsNullOrEmpty(pageContents.MetaKeyword) ? theSite.MetaKeyword : pageContents.MetaKeyword;

				int indexPos = 6;
				if (this.CurrentWebPage.Header.Controls.Count > indexPos) {
					if (!string.IsNullOrEmpty(metaDesc.Content) && !bDescExist) {
						this.CurrentWebPage.Header.Controls.AddAt(indexPos, new Literal { Text = "\r\n" });
						this.CurrentWebPage.Header.Controls.AddAt(indexPos, metaDesc);
					}
					if (!string.IsNullOrEmpty(metaKey.Content) && !bKeyExist) {
						this.CurrentWebPage.Header.Controls.AddAt(indexPos, new Literal { Text = "\r\n" });
						this.CurrentWebPage.Header.Controls.AddAt(indexPos, metaKey);
					}
				} else {
					if (!string.IsNullOrEmpty(metaDesc.Content) && !bDescExist) {
						this.CurrentWebPage.Header.Controls.Add(metaDesc);
						this.CurrentWebPage.Header.Controls.Add(new Literal { Text = "\r\n" });
					}
					if (!string.IsNullOrEmpty(metaKey.Content) && !bKeyExist) {
						this.CurrentWebPage.Header.Controls.Add(metaKey);
						this.CurrentWebPage.Header.Controls.Add(new Literal { Text = "\r\n" });
					}
				}

				metaDesc.Visible = !string.IsNullOrEmpty(metaDesc.Content);
				metaKey.Visible = !string.IsNullOrEmpty(metaKey.Content);
			}

			contCenter = new ContentContainer();
			contLeft = new ContentContainer();
			contRight = new ContentContainer();

			if (pageContents != null) {

				using (ContentPageHelper pageHelper = new ContentPageHelper()) {

					PageViewType pvt = pageHelper.GetBlogHeadingFromURL(theSite, SiteData.CurrentScriptName);
					string sTitleAddendum = pvt.ExtraTitle;

					if (!string.IsNullOrEmpty(sTitleAddendum)) {
						if (!string.IsNullOrEmpty(pageContents.PageHead)) {
							pageContents.PageHead = pageContents.PageHead.Trim() + ": " + sTitleAddendum;
						} else {
							pageContents.PageHead = sTitleAddendum;
						}
						pageContents.TitleBar = pageContents.TitleBar.Trim() + ": " + sTitleAddendum;
					}

					PagedDataSummary pd = (PagedDataSummary)cu.FindControl(typeof(PagedDataSummary), this.CurrentWebPage);

					if (pd != null) {
						PagedDataSummaryTitleOption titleOpts = pd.TypeLabelPrefixes.Where(x => x.KeyValue == pvt.CurrentViewType).FirstOrDefault();

						if (titleOpts == null
							&& (pvt.CurrentViewType == PageViewType.ViewType.DateDayIndex
							|| pvt.CurrentViewType == PageViewType.ViewType.DateMonthIndex
							|| pvt.CurrentViewType == PageViewType.ViewType.DateYearIndex)) {

							titleOpts = pd.TypeLabelPrefixes.Where(x => x.KeyValue == PageViewType.ViewType.DateIndex).FirstOrDefault();
						}

						if (titleOpts != null && !string.IsNullOrEmpty(titleOpts.FormatText)) {
							pvt.ExtraTitle = string.Format(titleOpts.FormatText, pvt.RawValue);
							sTitleAddendum = pvt.ExtraTitle;
						}

						if (titleOpts != null && !string.IsNullOrEmpty(titleOpts.LabelText)) {
							pageContents.PageHead = titleOpts.LabelText + " " + sTitleAddendum;
							pageContents.NavMenuText = pageContents.PageHead;
							pageContents.TitleBar = pageContents.PageHead;
						}
					}
				}

				this.CurrentWebPage.Title = SetPageTitle(pageContents);

				DateTime dtModified = theSite.ConvertSiteTimeToLocalServer(pageContents.EditDate);
				string strModifed = dtModified.ToString("r");
				HttpContext.Current.Response.AppendHeader("Last-Modified", strModifed);
				HttpContext.Current.Response.Cache.SetLastModified(dtModified);

				DateTime dtExpire = DateTime.Now.AddSeconds(15);

				contCenter.Text = pageContents.PageText;
				contLeft.Text = pageContents.LeftPageText;
				contRight.Text = pageContents.RightPageText;

				contCenter.DatabaseKey = pageContents.Root_ContentID;
				contLeft.DatabaseKey = pageContents.Root_ContentID;
				contRight.DatabaseKey = pageContents.Root_ContentID;

				pageContents = CMSConfigHelper.IdentifyLinkAsInactive(pageContents);

				if (this.CurrentWebPage.User.Identity.IsAuthenticated) {

					HttpContext.Current.Response.Cache.SetNoServerCaching();
					HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
					dtExpire = DateTime.Now.AddMinutes(-10);
					HttpContext.Current.Response.Cache.SetExpires(dtExpire);

					if (!SecurityData.AdvancedEditMode) {

						if (SecurityData.IsAdmin || SecurityData.IsSiteEditor) {
							if (!SiteData.IsPageSampler && !IsPageTemplate) {
								Control editor = this.CurrentWebPage.LoadControl(SiteFilename.EditNotifierControlPath);
								this.CurrentWebPage.Form.Controls.Add(editor);
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

						Control editor = this.CurrentWebPage.LoadControl(SiteFilename.AdvancedEditControlPath);
						this.CurrentWebPage.Form.Controls.Add(editor);

						MarkWidgets(this.CurrentWebPage, true);
					}
				} else {
					HttpContext.Current.Response.Cache.SetExpires(dtExpire);
				}


				if (pageWidgets.Count > 0) {
					CMSConfigHelper cmsHelper = new CMSConfigHelper();

					//find each placeholder in use ONCE!
					List<LabeledControl> lstPlaceholders = (from ph in pageWidgets
															where ph.Root_ContentID == pageContents.Root_ContentID
															select new LabeledControl {
																ControlLabel = ph.PlaceholderName,
																KeyControl = FindTheControl(ph.PlaceholderName, this.CurrentWebPage)
															}).Distinct().ToList();

					List<Widget> lstWidget = null;

					if (SecurityData.AdvancedEditMode) {
						lstWidget = (from w in pageWidgets
									 orderby w.WidgetOrder, w.EditDate
									 where w.Root_ContentID == pageContents.Root_ContentID
									   && w.IsWidgetActive == true
									   && w.IsWidgetPendingDelete == false
									 select w).ToList();
					} else {
						lstWidget = (from w in pageWidgets
									 orderby w.WidgetOrder, w.EditDate
									 where w.Root_ContentID == pageContents.Root_ContentID
									   && w.IsWidgetActive == true
									   && w.IsRetired == false && w.IsUnReleased == false
									   && w.IsWidgetPendingDelete == false
									 select w).ToList();
					}

					Assembly a = Assembly.GetExecutingAssembly();

					foreach (Widget theWidget in lstWidget) {

						WidgetContainer plcHolder = (WidgetContainer)(from d in lstPlaceholders
																	  where d.ControlLabel == theWidget.PlaceholderName
																	  select d.KeyControl).FirstOrDefault();
						if (plcHolder != null) {
							plcHolder.EnableViewState = true;
							Control widget = new Control();

							if (theWidget.ControlPath.EndsWith(".ascx")) {
								if (File.Exists(this.CurrentWebPage.Server.MapPath(theWidget.ControlPath))) {
									try {
										widget = this.CurrentWebPage.LoadControl(theWidget.ControlPath);
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

							Dictionary<string, string> lstMenus = new Dictionary<string, string>();
							if (widget is IWidgetMultiMenu) {
								IWidgetMultiMenu wmm = widget as IWidgetMultiMenu;
								lstMenus = wmm.JSEditFunctions;
							}

							if (SecurityData.AdvancedEditMode) {

								WidgetWrapper plcWrapper = plcHolder.AddWidget(widget, theWidget);

								CMSPlugin plug = (from p in cmsHelper.ToolboxPlugins
												  where p.FilePath.ToLower() == plcWrapper.ControlPath.ToLower()
												  select p).FirstOrDefault();

								if (plug != null) {
									plcWrapper.ControlTitle = plug.Caption;
								}

								plcWrapper.ID = WrapCtrlId;

								if (w != null) {
									if (w.EnableEdit) {
										if (lstMenus.Count < 1) {
											string sScript = w.JSEditFunction;
											if (string.IsNullOrEmpty(sScript)) {
												sScript = "cmsGenericEdit('" + pageContents.Root_ContentID + "','" + plcWrapper.DatabaseKey + "')";
											}

											plcWrapper.JSEditFunction = sScript;
										} else {
											plcWrapper.JSEditFunctions = lstMenus;
										}
									}
								}
							} else {
								plcHolder.AddWidget(widget);
							}

							widget.ID = CtrlId;

						}
					}

					cmsHelper.Dispose();
				}
			}
		}

		public string SetPageTitle(ContentPage pageData) {
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

			string sPageTitle = string.Format(sPattern, theSite.SiteName, theSite.SiteTagline, pageData.TitleBar, pageData.PageHead, pageData.NavMenuText, pageData.GoLiveDate, pageData.EditDate);

			return sPageTitle;
		}

		public void MarkWidgets(Control X, bool bAdmin) {
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

		public void AssignControls() {

			if (pageContents != null) {

				cu.ResetFind();
				Control ctrlHead = cu.FindControl("litPageHeading", this.CurrentWebPage);
				if (ctrlHead != null && ctrlHead is ITextControl) {
					((ITextControl)ctrlHead).Text = pageContents.PageHead;
				}

				cu.ResetFind();
				Control ctrlCenter = cu.FindControl("BodyCenter", this.CurrentWebPage);
				if (ctrlCenter != null && ctrlCenter is ContentContainer) {
					AssignContentZones((ContentContainer)ctrlCenter, contCenter);
				}

				cu.ResetFind();
				Control ctrlLeft = cu.FindControl("BodyLeft", this.CurrentWebPage);
				if (ctrlLeft != null && ctrlLeft is ContentContainer) {
					AssignContentZones((ContentContainer)ctrlLeft, contLeft);
				}

				cu.ResetFind();
				Control ctrlRight = cu.FindControl("BodyRight", this.CurrentWebPage);
				if (ctrlRight != null && ctrlRight is ContentContainer) {
					AssignContentZones((ContentContainer)ctrlRight, contRight);
				}
			}
		}

		bool bFound = false;
		WidgetContainer widgetCtrl = new WidgetContainer();
		protected WidgetContainer FindTheControl(string ControlName, Control X) {

			if (X is Page) {
				bFound = false;
				widgetCtrl = new WidgetContainer();
			}

			foreach (Control c in X.Controls) {
				if (c.ID == ControlName && c is WidgetContainer) {
					bFound = true;
					widgetCtrl = (WidgetContainer)c;
					return widgetCtrl;
				} else {
					if (!bFound) {
						FindTheControl(ControlName, c);
					}
				}
			}
			return widgetCtrl;
		}


		List<HtmlMeta> lstHtmlMeta = new List<HtmlMeta>();
		protected List<HtmlMeta> GetHtmlMeta(Control X) {
			lstHtmlMeta = new List<HtmlMeta>();

			FindHtmlMeta(X);

			return lstHtmlMeta;
		}
		protected void FindHtmlMeta(Control X) {

			foreach (Control c in X.Controls) {
				if (c is HtmlMeta) {
					lstHtmlMeta.Add((HtmlMeta)c);
				} else {
					FindHtmlMeta(c);
				}
			}
		}

	}
}
