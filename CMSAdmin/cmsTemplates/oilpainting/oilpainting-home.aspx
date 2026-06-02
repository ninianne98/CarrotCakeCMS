<%@ Page Language="C#" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPage" %>

<!DOCTYPE HTML>
<!--
Design by Free CSS Templates
https://templated.live/oilpainting/
Released for free under a Creative Commons Attribution 2.5 License

Name       : Oil Painting
Description: A two-column, fixed-width design with dark color scheme.
Version    : 1.0
Released   : 20120825

Conversion to CarrotCake CMS Template: Carrotware
-->
<html>
<head id="Head1" runat="server">
	<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="GlossyBlack" />
	<meta http-equiv="content-type" content="text/html; charset=utf-8" />
	<title>Oil Painting by TEMPLATED</title>
	<link href="https://fonts.googleapis.com/css?family=Arvo" rel="stylesheet" type="text/css" />
	<link href="https://fonts.googleapis.com/css?family=Open+Sans+Condensed:300|Coda:400,800" rel="stylesheet" type="text/css" />
	<asp:PlaceHolder ID="myPageHead" runat="server">
		<link href="<%=ThePage.TemplateFolderPath %>style.css" rel="stylesheet" type="text/css" media="screen" />
	</asp:PlaceHolder>
	<script type="text/javascript">
		$(document).ready(function () {
			$("button, input:button, input:submit, input:reset, button").button();
		});
	</script>
	<carrot:SocialMetaTag runat="server" ID="SocialMetaTag1" />
	<carrot:RSSFeed runat="server" ID="RSSFeed1" />
	<carrot:SiteCanonicalURL runat="server" ID="SiteCanonicalURL1" />
</head>
<body>
	<form id="form1" runat="server">
		<div id="menu-wrapper">
			<div id="menu-inner">
				<carrot:TwoLevelNavigation runat="server" WrapList="false" ID="TwoLevelNavigation1" FontSize="16px" ForeColor="#ffffff" BackColor="#453E37" SelFGColor="#ffffff"
					SelBGColor="#2E2925" SubBGColor="#DA802A" SubFGColor="#ffffff" CSSSelected="current_page_item" ExtraCSS="menu.css" />
			</div>
			<!-- end #menu -->
		</div>
		<div id="header-wrapper">
			<div id="header">
				<div id="logo">
					<h1>
						<a href="/">
							<carrot:SiteDataProperty runat="server" ID="SiteDataProperty1" DataField="SiteName" /></a>
					</h1>
					<p>
						<carrot:SiteDataProperty runat="server" ID="SiteDataProperty2" DataField="SiteTagline" />
					</p>
				</div>
			</div>
		</div>
		<!-- end #header -->
		<div id="banner">
			<img runat="server" id="imgHead" src="images/pics01.jpg" width="1000" height="200" alt="" />
		</div>
		<div id="wrapper">
			<div id="page">
				<div id="page-bgtop">
					<div id="page-bgbtm">
						<div id="content">
							<div class="post">
								<h2 class="title">
									<carrot:ContentPageProperty runat="server" ID="ContentPageProperty10" DataField="PageHead" /></h2>
								<div style="clear: both;">
									&nbsp;
								</div>
								<div class="entry">
									<carrot:WidgetContainer ID="phCenterTop" runat="server" />
									<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
									<carrot:WidgetContainer ID="phCenterBottom" runat="server" />

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
								</div>
							</div>
							<div style="clear: both;">
								&nbsp;
							</div>
						</div>
						<!-- end #content -->
						<div id="sidebar">
							<ul>
								<li>
									<h2>Search Here:</h2>
									<div id="search">
										<carrot:SearchBox ID="SearchBox1" runat="server">
											<SearchTemplate>
												<div class="searchzone">
													<div id="searchinner">
														<asp:TextBox ID="SearchText" runat="server" CssClass="search-text" MaxLength="40" />
														<asp:Button ID="btnSiteSearch" runat="server" CssClass="search-submit" Text="Search" />
													</div>
												</div>
											</SearchTemplate>
										</carrot:SearchBox>
									</div>
									<div style="clear: both;">
										&nbsp;
									</div>
								</li>
								<li>
									<h2>Pages</h2>
									<carrot:TopLevelNavigation runat="server" ID="TopLevelNavigation1" />
								</li>
								<li>
									<carrot:SiteMetaWordList ID="SiteMetaWordList1" runat="server" ContentType="DateMonth" MetaDataTitle="Dates" TakeTop="14" />
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
							&nbsp;
						</div>
					</div>
				</div>
			</div>
			<!-- end #page -->
		</div>
		<asp:PlaceHolder ID="myFooter" runat="server">
			<div id="footer">
				<p>
					<%=String.Format("&copy;  {0}, {1}. ", DateTime.Now.Year, TheSite.SiteName.Trim()) %> All rights reserved.
				 Design by <a href="https://templated.live" target="_blank" rel="nofollow">TEMPLATED</a>
					| Site built with <a target="_blank" href="http://www.carrotware.com/carrotcake-cms">carrotcake cms</a>
				</p>
			</div>
		</asp:PlaceHolder>
		<!-- end #footer -->
	</form>
</body>
</html>
