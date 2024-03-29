﻿using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.CMS.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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

		public ContentPage ThePage { get { return _pageContents; } }
		public SiteData TheSite { get { return _theSite; } }
		public List<Widget> ThePageWidgets { get { return _pageWidgets; } }

		protected ControlUtilities cu = new ControlUtilities();

		protected ContentPage _pageContents = null;
		protected SiteData _theSite = null;
		protected List<Widget> _pageWidgets = null;

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

		protected enum ControlLocation {
			Footer,
			Header,
		}

		protected void InsertSpecialCtrl(Control control, ControlLocation CtrlKey) {
			string sControlPath = string.Empty;
			CarrotCakeConfig config = CarrotCakeConfig.GetConfig();

			switch (CtrlKey) {
				case ControlLocation.Header:
					sControlPath = config.PublicSiteControls.ControlPathHeader;
					break;

				case ControlLocation.Footer:
					sControlPath = config.PublicSiteControls.ControlPathFooter;
					break;
			}

			if (!string.IsNullOrEmpty(sControlPath)) {
				if (File.Exists(HttpContext.Current.Server.MapPath(sControlPath))) {
					Control ctrl = new Control();
					ctrl = CurrentWebPage.LoadControl(sControlPath);
					control.Controls.Add(ctrl);
				}
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
			_theSite = SiteData.CurrentSite;
			_pageContents = null;
			_pageWidgets = new List<Widget>();

			_pageContents = SiteData.GetCurrentPage();

			if (_pageContents != null) {
				guidContentID = _pageContents.Root_ContentID;
				_pageWidgets = SiteData.GetCurrentPageWidgets(guidContentID);
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

			if (_theSite != null && _pageContents != null) {
				if (_theSite.BlockIndex || _pageContents.BlockIndex) {
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

			InsertSpecialCtrl(this.CurrentWebPage.Header, ControlLocation.Header);

			if (_pageContents != null) {
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
				metaDesc.Content = string.IsNullOrEmpty(_pageContents.MetaDescription) ? _theSite.MetaDescription : _pageContents.MetaDescription;
				metaKey.Content = string.IsNullOrEmpty(_pageContents.MetaKeyword) ? _theSite.MetaKeyword : _pageContents.MetaKeyword;

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

			if (_pageContents != null) {
				using (ContentPageHelper pageHelper = new ContentPageHelper()) {
					PageViewType pvt = pageHelper.GetBlogHeadingFromURL(_theSite, SiteData.CurrentScriptName);
					string sTitleAddendum = pvt.ExtraTitle;

					if (!string.IsNullOrEmpty(sTitleAddendum)) {
						if (!string.IsNullOrEmpty(_pageContents.PageHead)) {
							_pageContents.PageHead = _pageContents.PageHead.Trim() + ": " + sTitleAddendum;
						} else {
							_pageContents.PageHead = sTitleAddendum;
						}
						_pageContents.TitleBar = _pageContents.TitleBar.Trim() + ": " + sTitleAddendum;
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
							_pageContents.PageHead = titleOpts.LabelText + " " + sTitleAddendum;
							_pageContents.NavMenuText = _pageContents.PageHead;
							_pageContents.TitleBar = _pageContents.PageHead;
						}
					}
				}

				this.CurrentWebPage.Title = SetPageTitle(_pageContents);

				DateTime dtModified = _theSite.ConvertSiteTimeToLocalServer(_pageContents.EditDate);
				string strModifed = dtModified.ToString("r");
				HttpContext.Current.Response.AppendHeader("Last-Modified", strModifed);
				HttpContext.Current.Response.Cache.SetLastModified(dtModified);

				DateTime dtExpire = DateTime.Now.AddSeconds(15);

				contCenter.Text = _pageContents.PageText;
				contLeft.Text = _pageContents.LeftPageText;
				contRight.Text = _pageContents.RightPageText;

				contCenter.DatabaseKey = _pageContents.Root_ContentID;
				contLeft.DatabaseKey = _pageContents.Root_ContentID;
				contRight.DatabaseKey = _pageContents.Root_ContentID;

				_pageContents = CMSConfigHelper.FixNavLinkText(_pageContents);

				if (SecurityData.IsAuthenticated) {
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

						contCenter.Text = _pageContents.PageText;
						contLeft.Text = _pageContents.LeftPageText;
						contRight.Text = _pageContents.RightPageText;

						Control editor = this.CurrentWebPage.LoadControl(SiteFilename.AdvancedEditControlPath);
						this.CurrentWebPage.Form.Controls.Add(editor);

						MarkWidgets(this.CurrentWebPage, true);
					}
				} else {
					HttpContext.Current.Response.Cache.SetExpires(dtExpire);
				}

				InsertSpecialCtrl(this.CurrentWebPage.Form, ControlLocation.Footer);

				if (_pageWidgets.Any()) {
					CMSConfigHelper cmsHelper = new CMSConfigHelper();

					//find each placeholder in use ONCE!
					List<LabeledControl> lstPlaceholders = (from ph in _pageWidgets
															where ph.Root_ContentID == _pageContents.Root_ContentID
															select new LabeledControl(ph.PlaceholderName, FindTheControl(ph.PlaceholderName, this.CurrentWebPage))).Distinct().ToList();

					List<Widget> lstWidget = null;

					if (SecurityData.AdvancedEditMode) {
						lstWidget = (from w in _pageWidgets
									 orderby w.WidgetOrder, w.EditDate
									 where w.Root_ContentID == _pageContents.Root_ContentID
									   //&& w.IsWidgetActive == true
									   && w.IsWidgetPendingDelete == false
									 select w).ToList();
					} else {
						lstWidget = (from w in _pageWidgets
									 orderby w.WidgetOrder, w.EditDate
									 where w.Root_ContentID == _pageContents.Root_ContentID
									   && w.IsWidgetActive == true
									   && w.IsRetired == false && w.IsUnReleased == false
									   && w.IsWidgetPendingDelete == false
									 select w).ToList();
					}

					foreach (Widget theWidget in lstWidget) {
						WidgetContainer plcHolder = (WidgetContainer)(from d in lstPlaceholders
																	  where d.ControlLabel == theWidget.PlaceholderName
																	  select d.KeyControl).FirstOrDefault();
						if (plcHolder != null) {
							plcHolder.EnableViewState = true;
							Control widget = new Control();

							if (theWidget.ControlPath.ToLowerInvariant().EndsWith(".ascx")) {
								if (File.Exists(this.CurrentWebPage.Server.MapPath(theWidget.ControlPath))) {
									try {
										widget = this.CurrentWebPage.LoadControl(theWidget.ControlPath);
									} catch (Exception ex) {
										SiteData.WriteDebugException("renderwidget-ascx", ex);
										var lit = new Literal();
										lit.Text = "<b>ERROR: " + theWidget.ControlPath + "</b> <br />\r\n" + ex.ToString();
										widget = lit;
									}
								} else {
									SiteData.WriteDebugException("renderwidget-ascx", new Exception("MISSING FILE: " + theWidget.ControlPath));
									var lit = new Literal();
									lit.Text = "MISSING FILE: " + theWidget.ControlPath + "<br />\r\n";
									widget = lit;
								}
							}

							if (theWidget.ControlPath.ToLowerInvariant().StartsWith("class:")) {
								try {
									string className = theWidget.ControlPath.Replace("CLASS:", "");
									Type t = Type.GetType(className);
									object o = Activator.CreateInstance(t);

									if (o != null) {
										widget = o as Control;
									} else {
										SiteData.WriteDebugException("renderwidget-class", new Exception("OOPS: " + theWidget.ControlPath));
										var lit = new Literal();
										lit.Text = "OOPS: " + theWidget.ControlPath + "<br />\r\n";
										widget = lit;
									}
								} catch (Exception ex) {
									SiteData.WriteDebugException("renderwidget-class", ex);
									var lit = new Literal();
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
												  where p.FilePath.ToLowerInvariant() == plcWrapper.ControlPath.ToLowerInvariant()
												  select p).FirstOrDefault();

								if (plug != null) {
									plcWrapper.ControlTitle = plug.Caption;
								} else {
									plcWrapper.ControlTitle = "UNTITLED";
								}

								plcWrapper.ID = WrapCtrlId;

								if (w != null) {
									if (w.EnableEdit) {
										if (lstMenus.Count < 1) {
											string sScript = w.JSEditFunction;
											if (string.IsNullOrEmpty(sScript)) {
												sScript = "cmsGenericEdit('" + _pageContents.Root_ContentID + "','" + plcWrapper.DatabaseKey + "')";
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
			string sPrefix = string.Empty;

			if (!pageData.PageActive) {
				sPrefix = "* UNPUBLISHED * ";
			}
			if (pageData.RetireDate < _theSite.Now) {
				sPrefix = "* RETIRED * ";
			}
			if (pageData.GoLiveDate > _theSite.Now) {
				sPrefix = "* UNRELEASED * ";
			}
			string sPattern = sPrefix + SiteData.CurrentTitlePattern;

			string sPageTitle = string.Format(sPattern, _theSite.SiteName, _theSite.SiteTagline, pageData.TitleBar, pageData.PageHead, pageData.NavMenuText, pageData.GoLiveDate, pageData.EditDate);

			return sPageTitle;
		}

		public void MarkWidgets(Control control, bool bAdmin) {
			//add the command click event to the link buttons on the datagrid heading
			foreach (Control c in control.Controls) {
				if (c is WidgetContainer) {
					WidgetContainer ph = (WidgetContainer)c;
					ph.IsAdminMode = bAdmin;
				} else {
					MarkWidgets(c, bAdmin);
				}
			}
		}

		public void AssignControls() {
			if (_pageContents != null) {
				cu.ResetFind();
				Control ctrlHead = cu.FindControl("litPageHeading", this.CurrentWebPage);
				if (ctrlHead != null && ctrlHead is ITextControl) {
					((ITextControl)ctrlHead).Text = _pageContents.PageHead;
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

		private bool bFound = false;
		private WidgetContainer widgetCtrl = new WidgetContainer();

		protected WidgetContainer FindTheControl(string ControlName, Control control) {
			if (control is Page) {
				bFound = false;
				widgetCtrl = new WidgetContainer();
			}

			foreach (Control c in control.Controls) {
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

		private List<HtmlMeta> lstHtmlMeta = new List<HtmlMeta>();

		protected List<HtmlMeta> GetHtmlMeta(Control control) {
			lstHtmlMeta = new List<HtmlMeta>();

			FindHtmlMeta(control);

			return lstHtmlMeta;
		}

		protected void FindHtmlMeta(Control control) {
			foreach (Control c in control.Controls) {
				if (c is HtmlMeta) {
					lstHtmlMeta.Add((HtmlMeta)c);
				} else {
					FindHtmlMeta(c);
				}
			}
		}
	}
}