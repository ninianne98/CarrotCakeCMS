<%@ Page Language="C#" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPage" %>

<!--
Citrus Island 1.1 is is a free, W3C-compliant, CSS-based website template by styleshout.com.
This work is distributed under the Creative Commons Attribution 2.5 License, which means that you
are free to use and modify it for any purpose. All I ask is that you include a link back to my website in your credits.

http://www.styleshout.com/

Conversion to CarrotCake CMS Template: Carrotware
-->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="Silver" UseJqueryMigrate="true" />
	<meta name="description" />
	<meta name="keywords" />
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
			<div>
				<carrot:BreadCrumbNavigation runat="server" ID="BreadCrumbNavigation1" CSSSelected="green" />
			</div>
			<div style="clear: both;">
			</div>
			<div id="sidebar">
				<carrot:ChildNavigation MetaDataTitle="Child Pages" HeadWrapTag="h1" CssClass="sidemenu" CSSSelected="active" runat="server"
					ID="ChildNavigation1" />
				<carrot:SiblingNavigation MetaDataTitle="In This Section" HeadWrapTag="h1" CssClass="sidemenu" CSSSelected="active" runat="server"
					ID="SiblingNavigation1" />
				<carrot:WidgetContainer ID="phLeftTop" runat="server" />
				<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" TextZone="TextLeft" runat="server" />
				<carrot:WidgetContainer ID="phLeftBottom" runat="server" />
				<div style="clear: both;">
				</div>
			</div>
			<div id="main">
				<h1>
					<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="PageHead" /></h1>
				<carrot:WidgetContainer ID="phCenterTop" runat="server" />
				<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" TextZone="TextCenter" runat="server" />
				<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
				<br />
				<div style="clear: both;">
				</div>
			</div>
			<div id="rightbar">
				<div class="padding10">
					<div class="rsszone">
						<carrot:RSSFeed runat="server" ID="RSSFeed2" RSSFeedType="BlogOnly" CssClass="rssimage" RenderRSSMode="ImageLink" />
						&nbsp;&nbsp;&nbsp;&nbsp;
					<carrot:RSSFeed runat="server" ID="RSSFeed3" RSSFeedType="PageOnly" CssClass="rssimage" RenderRSSMode="ImageLink" />
					</div>
				</div>
				<carrot:MostRecentUpdated MetaDataTitle="Recent Updates" HeadWrapTag="h1" CssClass="sidemenu" runat="server" ID="MostRecentUpdated1" />
				<carrot:WidgetContainer ID="phRightTop" runat="server" />
				<carrot:ContentContainer EnableViewState="false" ID="BodyRight" TextZone="TextRight" runat="server" />
				<carrot:WidgetContainer ID="phRightBottom" runat="server" />
				<div style="clear: both;">
				</div>
			</div>
		</div>
		<div id="footer">
			<div id="footer-content">
				<div id="footer-right">
					<%--<a href="http://www.free-css.com/">Home</a> | <a href="http://www.free-css.com/">Site Map</a> | <a href="http://www.free-css.com/">
					RSS Feed</a>--%>
				</div>
				<asp:PlaceHolder ID="myFooter" runat="server">
					<div id="footer-left">
						<%=String.Format("&copy;  {0}, {1}. All rights reserved. ", DateTime.Now.Year, TheSite.SiteName.Trim()) %>
						| Site built with <a target="_blank" href="http://www.carrotware.com/">carrotcake cms</a>
						<br />
						Design by: <a target="_blank" href="http://www.styleshout.com/">styleshout</a>
						| Valid <a target="_blank" href="http://validator.w3.org/check/referer">XHTML</a>
						| <a target="_blank" href="http://jigsaw.w3.org/css-validator/check/referer">CSS</a>
					</div>
				</asp:PlaceHolder>
			</div>
		</div>
		<asp:Panel ID="pnlHiddenControls" Visible="false" runat="server">
		</asp:Panel>
	</form>
</body>
</html>