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
	<carrot:jquery runat="server" ID="jquery1" />
	<carrot:jqueryui runat="server" ID="jqueryui1" />
	<!--[if IE 6]><link rel="stylesheet" href="style.ie6.css" type="text/css" media="screen"  runat="server" id="cssLink2" /> <![endif]-->
	<!--[if IE 7]><link rel="stylesheet" href="style.ie7.css" type="text/css" media="screen"  runat="server" id="cssLink3" /> <![endif]-->
	<asp:PlaceHolder ID="myScripts" runat="server">
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
	<link href="style.css" rel="stylesheet" type="text/css" media="screen" runat="server" id="cssLink1" />
	<carrot:SiteCanonicalURL runat="server" ID="SiteCanonicalURL1" />
	<carrot:RSSFeed runat="server" ID="RSSFeed1" />
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
							<a href="/">
								<carrot:SiteDataProperty runat="server" ID="SiteDataProperty1" DataField="SiteName" /></a>
						</h1>
						<h2 class="art-logo-text">
							<carrot:SiteDataProperty runat="server" ID="SiteDataProperty2" DataField="SiteTagline" />
						</h2>
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
					<carrot:TwoLevelNavigationTemplate runat="server" CssClass="art-nav-inner" CSSSelected="active" ID="TwoLevelNavigationTemplate1" ShowSecondLevel="false">
						<TopNavHeaderTemplate>
							<ul class="art-hmenu">
						</TopNavHeaderTemplate>
						<TopNavTemplate>
							<carrot:ListItemWrapper ID="ListItemWrapper1" runat="server" HtmlTagName="li">
								<carrot:NavLinkForTemplate ID="NavLinkForTemplate1" runat="server" UseDefaultText="false">
									<span class="l"></span><span class="r"></span><span class="t">
										<carrot:ListItemNavText runat="server" ID="ListItemNavText1" DataField="NavMenuText" />
									</span>
								</carrot:NavLinkForTemplate>
							</carrot:ListItemWrapper>
						</TopNavTemplate>
						<TopNavFooterTemplate>
							</ul>
						</TopNavFooterTemplate>
					</carrot:TwoLevelNavigationTemplate>
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
											<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="PageHead" /></asp:Literal>
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
						<carrot:RSSFeed runat="server" ID="RSSFeed2" RSSFeedType="BlogAndPages" CssClass="art-rss-tag-icon" RenderRSSMode="TextLink" LinkText="  " />
						<%--<a href="#" class="art-rss-tag-icon" title="RSS"></a>--%>
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
	</asp:Panel>
	</form>
</body>
</html>
