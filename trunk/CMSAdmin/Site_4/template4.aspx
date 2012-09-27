<%@ Page Language="C#" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"[]>
<html xmlns="http://www.w3.org/1999/xhtml" dir="ltr" lang="en-US" xml:lang="en">
<head id="Head1" runat="server">
	<!--
    Created by Artisteer v3.0.0.39952
    Base template (without user's data) checked by http://validator.w3.org : "This page is valid XHTML 1.0 Transitional"
    -->
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
	<title>New Page</title>
	<carrot:jquery runat="server" ID="jquery1" JQVersion="1.6" />
	<carrot:jqueryui runat="server" ID="jqueryui1" />
	<asp:PlaceHolder ID="myScripts" runat="server">
		<link href="<%=pageContents.TemplateFolderPath %>style.css" rel="stylesheet" type="text/css" media="screen" />
		<!--[if IE 6]><link rel="stylesheet" href="<%=pageContents.TemplateFolderPath %>style.ie6.css" type="text/css" media="screen" /><![endif]-->
		<!--[if IE 7]><link rel="stylesheet" href="<%=pageContents.TemplateFolderPath %>style.ie7.css" type="text/css" media="screen" /><![endif]-->
		<script type="text/javascript" src="<%=pageContents.TemplateFolderPath %>script.js"></script>
	</asp:PlaceHolder>
	<script type="text/javascript">

		$(document).ready(function () {
			//		$(".art-nav-inner .art-hmenu").each(function (i) {
			//			mnuEnhanced(this);
			//		});

			$(".art-vmenublockcontent-body .art-vmenu").each(function (i) {
				mnuEnhanced(this);
			});

		});

		function mnuEnhanced(elm) {
			$("li a").each(function (i) {
				$(this).html("<span class='l'></span><span class='r'></span><span class='t'>" + $(this).text() + "</span>");
			});

			$("li.active a").each(function (i) {
				$(this).attr('class', 'active');
			});
		}

	</script>
</head>
<body>
	<form id="form1" runat="server">
	<div id="art-main">
		<div class="art-header">
			<div class="art-header-clip">
				<div class="art-header-center">
					<div class="art-header-jpeg">
					</div>
				</div>
			</div>
			<div class="art-header-wrapper">
				<div class="art-header-inner">
					<div class="art-logo">
						<h1 class="art-logo-name">
							<asp:PlaceHolder ID="myHeading" runat="server"><a href="/">
								<%=theSite.SiteName%></a></asp:PlaceHolder>
						</h1>
						<!--h2 class="art-logo-text">
							Enter Site Slogan</h2-->
					</div>
				</div>
			</div>
		</div>
		<div class="cleared reset-box">
		</div>
		<div class="art-nav">
			<div class="art-nav-l">
			</div>
			<div class="art-nav-r">
			</div>
			<div class="art-nav-outer">
				<div class="art-nav-wrapper">
					<%--<div class="art-nav-inner">--%>
					<%--<ul class="art-hmenu">
							<li><a href="./new-page.html" class="active"><span class="l"></span><span class="r"></span><span class="t">New Page</span></a>
							</li>
							<li><a href="./new-page-2.html"><span class="l"></span><span class="r"></span><span class="t">New Page 2</span></a> </li>
						</ul>--%>
					<%--<carrot:TopLevelNavigation CSSSelected="active" CssClass="art-hmenu" MenuWidth="600px" MenuHeight="44px" runat="server" ID="TopLevelNavigation1" />--%>
					<carrot:TwoLevelNavigationTemplate runat="server" CssClass="art-nav-inner" CSSSelected="active" ID="TwoLevelNavigationTemplate1" ShowSecondLevel="false">
						<TopNavHeaderTemplate>
							<ul class="art-hmenu">
						</TopNavHeaderTemplate>
						<TopNavTemplate>
							<carrot:ListItemWrapper runat="server" HtmlTagName="li" ID="ListItemWrapper1">
								<carrot:NavLinkForTemplate ID="NavLinkForTemplate1" runat="server">
									<span class="l"></span><span class="r"></span><span class="t">
										<%# Eval("NavMenuText").ToString()%></span>
								</carrot:NavLinkForTemplate>
							</carrot:ListItemWrapper>
						</TopNavTemplate>
						<TopNavFooterTemplate>
							</ul>
						</TopNavFooterTemplate>
					</carrot:TwoLevelNavigationTemplate>
					<%--</div>--%>
				</div>
			</div>
		</div>
		<div class="cleared reset-box">
		</div>
		<div class="art-sheet">
			<div class="art-sheet-tl">
			</div>
			<div class="art-sheet-tr">
			</div>
			<div class="art-sheet-bl">
			</div>
			<div class="art-sheet-br">
			</div>
			<div class="art-sheet-tc">
			</div>
			<div class="art-sheet-bc">
			</div>
			<div class="art-sheet-cl">
			</div>
			<div class="art-sheet-cr">
			</div>
			<div class="art-sheet-cc">
			</div>
			<div class="art-sheet-body">
				<div class="art-content-layout">
					<div class="art-content-layout-row">
						<div class="art-layout-cell art-sidebar1">
							<div class="art-vmenublock">
								<div class="art-vmenublock-tl">
								</div>
								<div class="art-vmenublock-tr">
								</div>
								<div class="art-vmenublock-bl">
								</div>
								<div class="art-vmenublock-br">
								</div>
								<div class="art-vmenublock-tc">
								</div>
								<div class="art-vmenublock-bc">
								</div>
								<div class="art-vmenublock-cl">
								</div>
								<div class="art-vmenublock-cr">
								</div>
								<div class="art-vmenublock-cc">
								</div>
								<div class="art-vmenublock-body">
									<div class="art-vmenublockcontent">
										<div class="art-vmenublockcontent-body">
											<!--<ul class="art-vmenu">
											<li><a href="./new-page.html" class="active"><span class="l"></span><span class="r"></span><span class="t">New Page</span></a>
											</li>
											<li><a href="./new-page-2.html"><span class="l"></span><span class="r"></span><span class="t">New Page 2</span></a> </li>
											</ul>-->
											<carrot:ChildNavigation CssClass="art-vmenu" CSSSelected="active" runat="server" ID="ChildNavigation1" />
											<carrot:SiblingNavigation CssClass="art-vmenu" CSSSelected="active" runat="server" ID="ChildNavigation2" />
											<div class="cleared">
											</div>
										</div>
									</div>
									<div class="cleared">
									</div>
								</div>
							</div>
							<div class="art-block">
								<div class="art-block-tl">
								</div>
								<div class="art-block-tr">
								</div>
								<div class="art-block-bl">
								</div>
								<div class="art-block-br">
								</div>
								<div class="art-block-tc">
								</div>
								<div class="art-block-bc">
								</div>
								<div class="art-block-cl">
								</div>
								<div class="art-block-cr">
								</div>
								<div class="art-block-cc">
								</div>
								<div class="art-block-body">
									<div class="art-blockcontent">
										<div class="art-blockcontent-body">
											<carrot:WidgetContainer ID="phLeftTop" runat="server">
											</carrot:WidgetContainer>
											<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" runat="server"></carrot:ContentContainer>
											<carrot:WidgetContainer ID="phLeftBottom" runat="server">
											</carrot:WidgetContainer>
											<div class="cleared">
											</div>
										</div>
									</div>
									<div class="cleared">
									</div>
								</div>
							</div>
							<div class="cleared">
							</div>
						</div>
						<div class="art-layout-cell art-content">
							<div class="art-post">
								<div class="art-post-body">
									<div class="art-post-inner art-article">
										<h2 class="art-postheader">
											<asp:Literal ID="litPageHeading" runat="server"></asp:Literal>
										</h2>
										<div class="cleared">
										</div>
										<div class="art-postcontent">
											<carrot:WidgetContainer ID="phCenterTop" runat="server">
											</carrot:WidgetContainer>
											<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server"></carrot:ContentContainer>
											<carrot:WidgetContainer ID="phCenterBottom" runat="server">
											</carrot:WidgetContainer>
										</div>
										<div class="cleared">
										</div>
									</div>
									<div class="cleared">
									</div>
								</div>
							</div>
							<div class="cleared">
							</div>
						</div>
					</div>
				</div>
				<div class="cleared">
				</div>
				<div class="art-footer">
					<div class="art-footer-t">
					</div>
					<div class="art-footer-body">
						<a href="#" class="art-rss-tag-icon" title="RSS"></a>
						<div class="art-footer-text">
							<asp:PlaceHolder ID="myFooter" runat="server">
								<p>
									<%=String.Format("&copy;  {0}, {1}. ", DateTime.Now.Year, theSite.SiteName) %>
									All rights reserved. | Site built with <a target="_blank" href="http://www.carrotware.com/carrotcake-cms.aspx">carrotcake cms</a>
								</p>
							</asp:PlaceHolder>
						</div>
						<div class="cleared">
						</div>
					</div>
				</div>
				<div class="cleared">
				</div>
			</div>
		</div>
		<div class="cleared">
		</div>
		<p class="art-page-footer">
			<a href="http://www.artisteer.com/?p=website_templates">Website Template</a> created with Artisteer.</p>
	</div>
	<asp:Panel ID="pnlHiddenControls" Visible="false" runat="server">
		<carrot:WidgetContainer ID="phRightTop" runat="server">
		</carrot:WidgetContainer>
		<carrot:ContentContainer EnableViewState="false" ID="BodyRight" runat="server"></carrot:ContentContainer>
		<carrot:WidgetContainer ID="phRightBottom" runat="server">
		</carrot:WidgetContainer>
		<br />
		<carrot:WidgetContainer ID="phExtraZone1" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone2" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone3" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone4" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone5" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone6" runat="server">
		</carrot:WidgetContainer>
	</asp:Panel>
	</form>
</body>
</html>
