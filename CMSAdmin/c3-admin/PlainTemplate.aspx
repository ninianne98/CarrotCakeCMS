<%@ Page Language="C#" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPage" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ Import Namespace="Carrotware.CMS.UI.Admin" %>

<!DOCTYPE HTML>
<script runat="server">
	string main = string.Empty;
	string mainXDk = string.Empty;
	string mainDk = string.Empty;
	string mainLt = string.Empty;
	string styleSheetPath = "";

	protected override void OnLoad(EventArgs e) {

		base.OnLoad(e);

		styleSheetPath = ControlUtilities.GetWebResourceUrl(typeof(AdminBaseMasterPage), "Carrotware.CMS.UI.Admin.cmsTemplates.plain.css");

		main = AdminBaseMasterPage.MainColorCode;
		mainXDk = CmsSkin.DarkenColor(main, 0.35);
		mainDk = CmsSkin.DarkenColor(main, 0.15);
		mainLt = CmsSkin.LightenColor(main, 0.15);

		TwoLevelNavigation1.SetThemeColor(main);

		if (this.IsSiteIndex) {
			PagedDataSummary2.Visible = true;
		} else {
			PagedDataSummary2.Visible = false;
			PagedDataSummary2.EnableViewState = false;
			PagedDataSummary2.PageSize = 3;
			PagedDataSummary2.ContentType = PagedDataSummary.SummaryContentType.Unknown;
		}

		if (ThePage.ContentType != ContentPageType.PageType.BlogEntry) {
			phMeta.Visible = false;
		}
	}
</script>
<html>
<head id="Head1" runat="server">
	<meta charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=no" />
	<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="GlossyBlack" JQVersion="1.11" />
	<script type="text/javascript">
		$(document).ready(function () {
			$("input:button, input:submit, input:reset").button();
		});
	</script>
	<title>plain</title>

	<asp:PlaceHolder runat="server" ID="phCss">
		<link href="<%=styleSheetPath %>" rel="stylesheet" type="text/css" />
		<style type="text/css">
			a {
				color: <%=String.Format("{0}", mainDk)%>;
			}

				a:hover {
					color: <%=String.Format("{0}", mainLt)%>;
				}

			#logo a {
				color: <%=String.Format("{0}", mainXDk)%>;
			}

				#logo a:hover {
					color: <%=String.Format("{0}", mainDk)%>;
				}

			h1, h2, h3, h4 {
				color: <%=String.Format("{0}", mainXDk)%>;
			}
		</style>
	</asp:PlaceHolder>
</head>
<body>
	<form id="form1" runat="server">
		<div id="wrapper">
			<h1 id="logo">
				<a href="/">
					<carrot:SiteDataProperty runat="server" ID="SiteDataProperty1" DataField="SiteName" />
				</a>
			</h1>
			<p>
				<carrot:SiteDataProperty runat="server" ID="SiteDataProperty2" DataField="SiteTagline" />
			</p>
			<div id="menu">
				<carrot:TwoLevelNavigation runat="server" ID="TwoLevelNavigation1" WrapList="false" FontSize="14px" />
			</div>
			<div style="clear: both;">
			</div>
			<!-- end #menu -->
			<div id="page">
				<carrot:BreadCrumbNavigation runat="server" ID="BreadCrumbNavigation1" />
				<div id="page-bgtop">
					<div id="page-bgbtm">
						<div id="sidebar">
							<ul>
								<li>
									<carrot:SearchBox ID="search1" runat="server">
										<SearchTemplate>
											<p>
												<asp:TextBox ID="SearchText" runat="server" CssClass="searchbox" MaxLength="40" />
											</p>
										</SearchTemplate>
									</carrot:SearchBox>
								</li>
								<li id="calendar_wrap">
									<carrot:PostCalendar runat="server" ID="calendar" CssClass="calendar" RenderHTMLWithID="true" />
									<br />
								</li>
								<li>
									<carrot:WidgetContainer ID="phLeftTop" runat="server" />
									<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" runat="server" />
									<carrot:WidgetContainer ID="phLeftBottom" runat="server" />
								</li>
								<li>
									<carrot:ChildNavigation MetaDataTitle="Child Pages" runat="server" ID="SecondLevelNavigation2" />
								</li>
								<li>
									<carrot:SecondLevelNavigation MetaDataTitle="Section Pages" runat="server" ID="SecondLevelNavigation1" />
								</li>
								<li>
									<carrot:MostRecentUpdated MetaDataTitle="Recent Updates" runat="server" ID="MostRecentUpdated1" />
								</li>
							</ul>
						</div>
						<!-- end #sidebar -->
						<div id="content">
							<div class="post">
								<h2>
									<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="PageHead" /></h2>
								<div class="entry">
									<carrot:WidgetContainer ID="phCenterTop" runat="server" />
									<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
									<asp:PlaceHolder ID="phMeta" runat="server">
										<div>
											<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl1" runat="server" ContentType="Category"
												MetaDataTitle="Categories:" CssClass="meta" />
											<br />
											<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl2" runat="server" ContentType="Tag"
												MetaDataTitle="Tags:" CssClass="meta" />
										</div>
									</asp:PlaceHolder>
									<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
								</div>
								<div style="clear: both;">
								</div>
								<div class="entry">
									<carrot:WidgetContainer ID="phRightTop" runat="server" />
									<carrot:ContentContainer EnableViewState="false" ID="BodyRight" runat="server" />
									<carrot:WidgetContainer ID="phRightBottom" runat="server" />
								</div>
							</div>
							<carrot:PagedDataSummary ID="PagedDataSummary2" runat="server" ContentType="Blog" PageSize="8" Visible="false">
								<ContentHeaderTemplate>
									<div>
								</ContentHeaderTemplate>
								<ContentTemplate>
									<div class="entry">
										<h2>
											<carrot:NavLinkForTemplate ID="NavLinkForTemplate1" runat="server" UseDefaultText="true" />
										</h2>
										<p>
											<carrot:ListItemNavText runat="server" ID="ListItemNavText1" DataField="PageTextPlainSummary" />
										</p>
										<p>
											<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="PostMetaWordList1" runat="server" ContentType="Category"
												MetaDataTitle="Categories:" CssClass="meta" />
										</p>
									</div>
								</ContentTemplate>
								<ContentFooterTemplate>
									</div>
								</ContentFooterTemplate>
								<PagerHeaderTemplate>
									<div class="pagerfooterlinks">
								</PagerHeaderTemplate>
								<PagerTemplate>
									<carrot:ListItemWrapperForPager HtmlTagName="div" ID="wrap" runat="server" CSSSelected="selectedwrap" CssClassNormal="pagerlink">
										<carrot:NavLinkForPagerTemplate ID="lnkBtn" CSSSelected="selected" runat="server" RenderAsHyperlink="true" />
									</carrot:ListItemWrapperForPager>
								</PagerTemplate>
								<PagerFooterTemplate>
									</div>
								</PagerFooterTemplate>
							</carrot:PagedDataSummary>
							<div style="clear: both;">
								&nbsp;
							</div>
						</div>
						<!-- end #content -->
						<div style="clear: both;">
							&nbsp;
						</div>
					</div>
				</div>
			</div>
			<!-- end #page -->
		</div>
		<div style="clear: both;">
		</div>
		<asp:PlaceHolder ID="myFooter" runat="server">
			<div id="footer">
				<p>
					<%=String.Format("&copy;  {0}, {1}. ", DateTime.Now.Year, TheSite.SiteName.Trim()) %>
				All rights reserved.
				</p>
			</div>
		</asp:PlaceHolder>
		<!-- end #footer -->
		<asp:Panel ID="pnlHiddenControls" Visible="false" runat="server">
		</asp:Panel>
	</form>
</body>
</html>
