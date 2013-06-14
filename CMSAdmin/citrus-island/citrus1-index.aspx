<%@ Page Language="C#" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="Silver" />
	<meta name="description">
	<meta name="keywords">
	<link runat="server" id="theCSS" href="style.css" rel="stylesheet" type="text/css" media="screen" />
	<title>Citrus Island</title>
	<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
	<carrot:SiteCanonicalURL runat="server" ID="SiteCanonicalURL1" />
	<carrot:RSSFeed runat="server" ID="RSSFeed1" />
	<carrot:OpenGraph runat="server" ID="OpenGraph1" />
</head>
<body>
	<form id="form1" runat="server">
	<div id="wrap">
		<div id="header">
			<div class="search">
				<carrot:SearchBox ID="search1" runat="server">
					<SearchTemplate>
						<p>
							<asp:TextBox ID="SearchText" runat="server" CssClass="textbox" MaxLength="40" />
							<asp:Button ID="btnSiteSearch" runat="server" CssClass="button" Text="Search" />
						</p>
					</SearchTemplate>
				</carrot:SearchBox>
			</div>
			<h1 id="logo">
				<a href="/">
					<carrot:SiteDataProperty runat="server" ID="SiteDataProperty1" DataField="SiteName" /></a>
			</h1>
			<h2 id="slogan">
				<carrot:SiteDataProperty runat="server" ID="SiteDataProperty2" DataField="SiteTagline" />
			</h2>
		</div>
		<div id="menu-wrapper">
			<carrot:TwoLevelNavigation runat="server" ID="TwoLevelNavigation1" FontSize="10px" ForeColor="#FFFFFF" BackColor="#F4845A" />
		</div>
		<div style="clear: both;">
		</div>
		<div id="sidebar">
			<carrot:SiteMetaWordList ID="SiteMetaWordList1" HeadWrapTag="h1" CssClass="sidemenu" runat="server" ContentType="DateMonth" MetaDataTitle="Dates" TakeTop="14" />
			<carrot:SiteMetaWordList ID="SiteMetaWordList2" HeadWrapTag="h1" CssClass="sidemenu" runat="server" ContentType="Category" MetaDataTitle="Categories" ShowUseCount="true" />
			<carrot:SiteMetaWordList ID="SiteMetaWordList3" HeadWrapTag="h1" CssClass="sidemenu" runat="server" ContentType="Tag" MetaDataTitle="Tags" ShowUseCount="true" />
			<carrot:WidgetContainer ID="phLeftTop" runat="server">
			</carrot:WidgetContainer>
			<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" runat="server">
			</carrot:ContentContainer>
			<carrot:WidgetContainer ID="phLeftBottom" runat="server">
			</carrot:WidgetContainer>
			<div style="clear: both;">
			</div>
		</div>
		<div id="main">
			<h1>
				<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="PageHead" /></h1>
			<carrot:WidgetContainer ID="phCenterTop" runat="server">
			</carrot:WidgetContainer>
			<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server">
			</carrot:ContentContainer>
			<div style="clear: both;">
			</div>
			<carrot:PagedDataSummary ID="PagedDataSummary1" runat="server" ContentType="Blog" PageSize="5" CSSSelectedPage="selected" LinkNext="lnkNextWrap" LinkPrev="lnkPrevWrap"
				LinkFirst="lnkFirstWrap" LinkLast="lnkLastWrap">
				<TypeLabelPrefixes>
					<carrot:PagedDataSummaryTitleOption KeyValue="DateIndex" LabelText="Date:" />
					<carrot:PagedDataSummaryTitleOption KeyValue="DateDayIndex" LabelText="Day:" FormatText="{0:dddd, d MMMM yyyy}" />
					<carrot:PagedDataSummaryTitleOption KeyValue="DateMonthIndex" LabelText="Month:" FormatText="{0:MMM yyyy}" />
					<carrot:PagedDataSummaryTitleOption KeyValue="DateYearIndex" LabelText="Year:" FormatText="{0:yyyy}" />
					<carrot:PagedDataSummaryTitleOption KeyValue="CategoryIndex" LabelText="Category:" />
					<carrot:PagedDataSummaryTitleOption KeyValue="TagIndex" LabelText="Tag:" />
					<carrot:PagedDataSummaryTitleOption KeyValue="AuthorIndex" LabelText="Content by " />
					<carrot:PagedDataSummaryTitleOption KeyValue="SearchResults" LabelText="Search results for:" FormatText=" [ {0} ] " />
				</TypeLabelPrefixes>
				<EmptyDataTemplate>
					<p>
						<b>No results found.</b>
					</p>
				</EmptyDataTemplate>
				<ContentHeaderTemplate>
					<h1>
						Contents</h1>
					<div>
				</ContentHeaderTemplate>
				<ContentTemplate>
					<div>
						<p>
							<b class="green" style="font-size: 110%;">
								<carrot:NavLinkForTemplate CssClassNormal="green" ID="NavLinkForTemplate1" runat="server" UseDefaultText="true" />
								&nbsp;|&nbsp;
								<carrot:ListItemNavText runat="server" ID="ListItemNavText1" DataField="CreateDate" FieldFormat="{0:d}" />
							</b>
							<br />
							by
							<carrot:AuthorLink runat="server" ID="AuthorLink1" />
							<br />
							<carrot:ListItemNavText runat="server" ID="ListItemNavText2" DataField="PageTextPlainSummary" />
						</p>
						<p>
							<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl1" runat="server" ContentType="Category" MetaDataTitle="Categories:" />
							<br />
							<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl2" runat="server" ContentType="Tag" MetaDataTitle="Tags:" />
						</p>
						<p class="post-footer align-right">
							<carrot:NavLinkForTemplate CssClassNormal="readmore" ID="NavLinkForTemplate2" runat="server" UseDefaultText="false">
								Read more</carrot:NavLinkForTemplate>
							<span class="comments">Comments
								<carrot:ListItemNavText runat="server" ID="ListItemNavText4" DataField="CommentCount" FieldFormat=" ({0}) " />
							</span><span class="date">
								<carrot:ListItemNavText runat="server" ID="ListItemNavText3" DataField="CreateDate" FieldFormat="{0:MMM d, yyyy}" />
							</span>
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
						<carrot:NavLinkForPagerTemplate ID="lnkBtn" CSSSelected="selected" runat="server" />
					</carrot:ListItemWrapperForPager>
				</PagerTemplate>
				<PagerFooterTemplate>
					</div>
				</PagerFooterTemplate>
			</carrot:PagedDataSummary>
			<div style="clear: both;">
			</div>
			<div style="float: left;">
				<carrot:PagedDataNextPrevLinkWrapper ID="lnkFirstWrap" runat="server">
					<carrot:PagedDataNextPrevLink ID="PagedDataNextPrevLink1" runat="server" UseDefaultText="false" Text="&lt; First" />&nbsp;&nbsp;
				</carrot:PagedDataNextPrevLinkWrapper>
				<carrot:PagedDataNextPrevLinkWrapper ID="lnkPrevWrap" runat="server">
					<carrot:PagedDataNextPrevLink ID="PagedDataNextPrevLink2" runat="server" />
				</carrot:PagedDataNextPrevLinkWrapper>
				&nbsp;
			</div>
			<div style="float: right;">
				&nbsp;
				<carrot:PagedDataNextPrevLinkWrapper ID="lnkNextWrap" runat="server">
					<carrot:PagedDataNextPrevLink ID="PagedDataNextPrevLink3" runat="server" />&nbsp;&nbsp;
				</carrot:PagedDataNextPrevLinkWrapper>
				<carrot:PagedDataNextPrevLinkWrapper ID="lnkLastWrap" runat="server">
					<carrot:PagedDataNextPrevLink ID="PagedDataNextPrevLink4" runat="server" UseDefaultText="false" Text="Last &gt;" />
				</carrot:PagedDataNextPrevLinkWrapper>
			</div>
			<div style="clear: both;">
			</div>
			<carrot:WidgetContainer ID="phCenterBottom" runat="server">
			</carrot:WidgetContainer>
			<br />
			<div style="clear: both;">
			</div>
		</div>
		<div id="rightbar">
			<carrot:WidgetContainer ID="phRightTop" runat="server">
			</carrot:WidgetContainer>
			<carrot:ContentContainer EnableViewState="false" ID="BodyRight" runat="server">
			</carrot:ContentContainer>
			<carrot:WidgetContainer ID="phRightBottom" runat="server">
			</carrot:WidgetContainer>
			<div style="clear: both;">
			</div>
		</div>
		<div style="clear: both;">
		</div>
	</div>
	<div id="footer">
		<div id="footer-content">
			<div id="footer-right">
				<%--<a href="http://www.free-css.com/">Home</a> | <a href="http://www.free-css.com/">Site
	Map</a> | <a href="http://www.free-css.com/"> RSS Feed</a>--%>
			</div>
			<div id="footer-left">
				<asp:PlaceHolder ID="myFooter" runat="server">
					<%=String.Format("&copy; {0}, {1}. ", DateTime.Now.Year, TheSite.SiteName.Trim()) %>
					All rights reserved. | Site built with <a target="_blank" href="http://www.carrotware.com/carrotcake-cms.aspx">carrotcake cms</a>
					<br />
					Design by: <a target="_blank" href="http://www.styleshout.com/">styleshout</a> | Valid <a target="_blank" href="http://validator.w3.org/check/referer">XHTML</a>
					| <a target="_blank" href="http://jigsaw.w3.org/css-validator/check/referer">CSS</a> </asp:PlaceHolder>
			</div>
		</div>
	</div>
	<asp:Panel ID="pnlHiddenControls" Visible="false" runat="server">
	</asp:Panel>
	</form>
</body>
</html>
