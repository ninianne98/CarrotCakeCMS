<%@ Page Language="C#" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<!--
Design by Free CSS Templates
http://www.freecsstemplates.org
Released for free under a Creative Commons Attribution 2.5 License

Name       : Abundant
Description: A two-column, fixed-width design for 1024x768 screen resolutions.
Version    : 1.0
Released   : 20090703

-->
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="GlossyBlack" />
	<meta http-equiv="content-type" content="text/html; charset=utf-8" />
	<title>Abundant by Free CSS Templates</title>
	<carrot:SiteCanonicalURL runat="server" ID="SiteCanonicalURL1" />
	<carrot:RSSFeed runat="server" ID="RSSFeed1" />
	<link runat="server" id="theCSS" href="style.css" rel="stylesheet" type="text/css" media="screen" />
	<script type="text/javascript">
		$(document).ready(function () {
			$("input:button, input:submit").button();
		});

	</script>
</head>
<body>
	<form id="form1" runat="server">
	<div id="wrapper">
		<div id="header">
			<div id="menu-wrapper">
				<carrot:TwoLevelNavigation runat="server" ID="TwoLevelNavigation1" FontSize="14px" ForeColor="#FFFFFF" SelBGColor="#9D0708" HoverBGColor="#D4260B" SubBGColor="#8E1318" />
			</div>
			<!-- end #menu -->
		</div>
		<!-- end #header -->
		<div id="logo">
			<carrot:SearchBox ID="search1" runat="server">
				<SearchTemplate>
					<div id="searcharea">
						<p>
							<asp:TextBox ID="SearchText" runat="server" CssClass="textbox" MaxLength="40" />
							<asp:Button ID="btnSiteSearch" runat="server" CssClass="button" Text="Search" />
						</p>
					</div>
				</SearchTemplate>
			</carrot:SearchBox>
			<h1>
				<a href="/">
					<carrot:SiteDataProperty runat="server" ID="SiteDataProperty1" DataField="SiteName" /></a>
			</h1>
			<p>
				<em>
					<carrot:SiteDataProperty runat="server" ID="SiteDataProperty2" DataField="SiteTagline" /></em></p>
		</div>
		<hr />
		<!-- end #logo -->
		<!-- end #header-wrapper -->
		<div id="page">
			<div id="content">
				<div class="post">
					<h2 class="title">
						<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="PageHead" /></h2>
					<div class="entry">
						<carrot:WidgetContainer ID="phCenterTop" runat="server">
						</carrot:WidgetContainer>
						<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
						<div style="clear: both;">
						</div>
						<carrot:PagedDataSummary ID="PagedDataSummary1" runat="server" ContentType="Blog" PageSize="8" CSSSelectedPage="selected">
							<SummaryHeaderTemplate>
								<div>
							</SummaryHeaderTemplate>
							<SummaryTemplate>
								<div class="post">
									<h2 class="title">
										<carrot:ListItemNavText runat="server" ID="ListItemNavText5" DataField="NavMenuText" />
									</h2>
									<p class="date">
										<carrot:ListItemNavText runat="server" ID="ListItemNavText6" DataField="GoLiveDate" FieldFormat="{0:M.d.yyyy}" />
									</p>
									<div class="entry">
										<p>
											<carrot:ListItemNavText runat="server" ID="ListItemNavText2" DataField="PageTextPlainSummaryMedium" />
										</p>
										<p class="links">
											<carrot:NavLinkForTemplate CssClassNormal="comments" ID="NavLinkForTemplate3" runat="server" UseDefaultText="false">
												<carrot:ListItemNavText runat="server" ID="ListItemNavText7" DataField="CommentCount" FieldFormat="Comments ({0})" />
											</carrot:NavLinkForTemplate>
											&nbsp;&nbsp;&nbsp;
											<carrot:NavLinkForTemplate ID="NavLinkForTemplate2" runat="server" UseDefaultText="false">
												Full article</carrot:NavLinkForTemplate>
										</p>
									</div>
								</div>
							</SummaryTemplate>
							<SummaryFooterTemplate>
								</div>
							</SummaryFooterTemplate>
							<PagerHeaderTemplate>
								<div class="pagerfooterlinks">
							</PagerHeaderTemplate>
							<PagerTemplate>
								<carrot:ListItemWrapperForPager HtmlTagName="div" ID="wrap" runat="server" CSSSelected="selectedwrap" CssClassNormal="pagerlink">
									<carrot:NavLinkForPagerTemplate ID="lnkBtn" CSSSelected="selected" runat="server" />
								</carrot:ListItemWrapperForPager>
							</PagerTemplate>
							<PagerFooterTemplate>
								</div>
							</PagerFooterTemplate>
						</carrot:PagedDataSummary>
						<div style="clear: both;">
						</div>
						<carrot:WidgetContainer ID="phCenterBottom" runat="server">
						</carrot:WidgetContainer>
					</div>
				</div>
			</div>
			<!-- end #content -->
			<div id="sidebar">
				<ul>
					<li>
						<carrot:SiteMetaWordList ID="SiteMetaWordList1" runat="server" ContentType="DateMonth" MetaDataTitle="Dates" TakeTop="12" />
					</li>
					<li>
						<carrot:SiteMetaWordList ID="SiteMetaWordList2" runat="server" ContentType="Category" MetaDataTitle="Categories" ShowUseCount="true" />
					</li>
					<li>
						<carrot:SiteMetaWordList ID="SiteMetaWordList3" runat="server" ContentType="Tag" MetaDataTitle="Tags" ShowUseCount="true" />
					</li>
				</ul>
			</div>
			<!-- end #sidebar -->
			<div style="clear: both;">
				&nbsp;</div>
		</div>
		<!-- end #page -->
	</div>
	<div id="footer">
		<asp:PlaceHolder ID="myFooter" runat="server">
			<p>
				<%=String.Format("&copy;  {0}, {1}. ", DateTime.Now.Year, theSite.SiteName) %>
				All rights reserved. | Site built with <a target="_blank" href="http://www.carrotware.com/carrotcake-cms.aspx">carrotcake cms</a>
				<br />
				Design by <a target="_blank" href="http://www.freecsstemplates.org/">Free CSS Templates</a>.
			</p>
		</asp:PlaceHolder>
	</div>
	<!-- end #footer -->
	<asp:Panel ID="pnlHiddenControls" Visible="false" runat="server">
		<carrot:WidgetContainer ID="phLeftTop" runat="server">
		</carrot:WidgetContainer>
		<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" runat="server" />
		<carrot:WidgetContainer ID="phLeftBottom" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phRightTop" runat="server">
		</carrot:WidgetContainer>
		<carrot:ContentContainer EnableViewState="false" ID="BodyRight" runat="server" />
		<carrot:WidgetContainer ID="phRightBottom" runat="server">
		</carrot:WidgetContainer>
	</asp:Panel>
	</form>
</body>
</html>
