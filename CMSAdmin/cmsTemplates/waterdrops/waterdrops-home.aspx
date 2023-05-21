<%@ Page Language="C#" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<!--
Design by Free CSS Templates
https://templated.live
Released for free under a Creative Commons Attribution 2.5 License

Name       : WaterDrops

Description: A two-column, fixed-width design with dark color scheme.
Version    : 1.0
Released   : 20120902

-->
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="content-type" content="text/html; charset=utf-8" />
	<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="Blue" />
	<title>WaterDrops by FCT</title>
	<link href="http://fonts.googleapis.com/css?family=Open+Sans+Condensed:300" rel="stylesheet" type="text/css" />
	<asp:PlaceHolder ID="myPageHead" runat="server">
		<link href="<%=ThePage.TemplateFolderPath %>style.css" rel="stylesheet" type="text/css" media="screen" />
	</asp:PlaceHolder>
	<script type="text/javascript">
		$(document).ready(function () {
			$("input:button, input:submit").button();
		});
	</script>
	<carrot:RSSFeed runat="server" ID="RSSFeed1" />
	<carrot:SiteCanonicalURL runat="server" ID="SiteCanonicalURL1" />
</head>
<body>
	<form id="form1" runat="server">
		<div id="header-wrapper">
			<div id="header-med" class="container">
				<div id="logo-full">
					<h1>
						<a href="/">
							<carrot:SiteDataProperty runat="server" ID="SiteDataProperty1" DataField="SiteName" /></a>
					</h1>
					<p>
						<carrot:SiteDataProperty runat="server" ID="SiteDataProperty2" DataField="SiteTagline" />
					</p>
				</div>
				<div id="navmenu-wrapper">
					<carrot:TwoLevelNavigation runat="server" ID="TwoLevelNavigation1" FontSize="16px" CSSSelected="current_page_item" CSSHasChildren="sub" WrapList="false"
						ForeColor="#ffffff" BackColor="#513256" UnSelFGColor="#CE92A6" HoverFGColor="#ffffff" HoverBGColor="#CE92A6" />
				</div>
			</div>
			<div id="banner" class="container">
				<div class="image-style">
					<img runat="server" id="imgHead" src="images/img03.jpg" width="970" height="320" alt="" />
				</div>
			</div>
		</div>
		<div id="wrapper">
			<div id="page" class="container">
				<div id="content">
					<div class="post">
						<h2 class="title">
							<carrot:ContentPageProperty runat="server" ID="ContentPageProperty10" DataField="PageHead" />
						</h2>
						<div class="entry">
							<carrot:WidgetContainer ID="phCenterTop" runat="server">
							</carrot:WidgetContainer>
							<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
							<carrot:WidgetContainer ID="phCenterBottom" runat="server">
							</carrot:WidgetContainer>
						</div>
					</div>
					<div style="clear: both;">
						&nbsp;
					</div>
				</div>
				<!-- end #content -->
				<div id="sidebar">
					<div id="box1">
						<carrot:WidgetContainer ID="phRightTop" runat="server">
						</carrot:WidgetContainer>
						<carrot:ContentContainer EnableViewState="false" ID="BodyRight" runat="server">
						</carrot:ContentContainer>
						<carrot:WidgetContainer ID="phRightBottom" runat="server">
						</carrot:WidgetContainer>
					</div>
					<div>
						<carrot:ChildNavigation MetaDataTitle="Child Pages" CssClass="list-style1" CSSSelected="active" runat="server" ID="ChildNavigation1" />
					</div>
					<div>
						<carrot:SiblingNavigation MetaDataTitle="In This Section" CssClass="list-style1" CSSSelected="active" runat="server" ID="ChildNavigation2" />
					</div>
					<div>
						<carrot:SiteMetaWordList ID="SiteMetaWordList1" runat="server" CssClass="list-style1" ContentType="DateMonth" MetaDataTitle="Archive" TakeTop="6" />
					</div>
				</div>
				<!-- end #sidebar -->
				<div style="clear: both;">
					&nbsp;
				</div>
			</div>
			<!-- end #page -->
		</div>
		<div id="footer-content" class="container">
			<div id="footer-bg">
				<carrot:PagedDataSummary ID="PagedDataSummary1" runat="server" ContentType="Blog" PageSize="3" ShowPager="false">
					<ContentHeaderTemplate>
					</ContentHeaderTemplate>
					<ContentTemplate>
						<div id="column">
							<h2>
								<carrot:NavLinkForTemplate ID="NavLinkForTemplate1" runat="server" UseDefaultText="true" />
							</h2>
							<p>
								<carrot:ListItemImageThumb runat="server" ID="ListItemImageThumb1" PerformURLResize="true" ScaleImage="true" ThumbSize="150" />
								<br />
								<carrot:ListItemNavText runat="server" ID="ListItemNavText2" DataField="PageTextPlainSummary" />
							</p>
						</div>
					</ContentTemplate>
					<ContentFooterTemplate>
					</ContentFooterTemplate>
				</carrot:PagedDataSummary>
			</div>
		</div>
		<asp:PlaceHolder ID="myFooter" runat="server">
			<div id="footer">
				<p>
					<%=String.Format("&copy;  {0}, {1}. ", DateTime.Now.Year, TheSite.SiteName.Trim()) %>
					| Design by <a href="https://templated.live">FCT</a>
					| Photos by <a href="http://fotogrph.com/">Fotogrph</a>
					| Site built with <a target="_blank" href="http://www.carrotware.com/">carrotcake cms</a>
				</p>
			</div>
		</asp:PlaceHolder>
		<!-- end #footer -->
		<asp:Panel ID="pnlHiddenControls" Visible="false" runat="server">
			<carrot:WidgetContainer ID="phLeftTop" runat="server">
			</carrot:WidgetContainer>
			<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" runat="server" />
			<carrot:WidgetContainer ID="phLeftBottom" runat="server">
			</carrot:WidgetContainer>
		</asp:Panel>
	</form>
</body>
</html>