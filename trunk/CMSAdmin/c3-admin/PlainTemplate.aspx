﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPage" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<script runat="server">

	protected override void OnLoad(EventArgs e) {

		base.OnLoad(e);

		if (TheSite.Blog_Root_ContentID.HasValue && ThePage.Root_ContentID == TheSite.Blog_Root_ContentID.Value) {
			PagedDataSummary2.Visible = true;
		} else {
			PagedDataSummary2.Visible = false;
			PagedDataSummary2.EnableViewState = false;
			PagedDataSummary2.PageSize = 3;
			PagedDataSummary2.ContentType = PagedDataSummary.SummaryContentType.Unknown;
		}
	}

</script>
<!DOCTYPE html>
<!--[if lt IE 7 ]><html class="ie ie6" lang="en"> <![endif]-->
<!--[if IE 7 ]><html class="ie ie7" lang="en"> <![endif]-->
<!--[if IE 8 ]><html class="ie ie8" lang="en"> <![endif]-->
<!--[if (gte IE 9)|!(IE)]><!-->
<html lang="en">
<!--<![endif]-->
<head id="Head1" runat="server">
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1">
	<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="GlossyBlack" />
	<script type="text/javascript">
		$(document).ready(function () {
			$("input:button, input:submit, input:reset").button();
		});

	</script>
	<title>plain</title>
	<style type="text/css">
		body {
			margin: 0;
			padding: 0;
			background: #FFFFFF;
			font-family: Arial, Helvetica, sans-serif;
			font-size: 12px;
			color: #787878;
		}
		h1, h2, h3 {
			margin: 0;
			padding: 0;
			font-weight: normal;
			color: #000000;
			margin-bottom: 5px;
		}
		h1 {
			font-size: 2.5em;
		}
		h2 {
			font-size: 2em;
		}
		h3 {
			font-size: 1.5em;
		}
		p, ul, ol {
			margin-top: 0;
			line-height: 180%;
		}
		a {
			text-decoration: none;
			color: #424242;
		}
		a:hover {
			text-decoration: underline;
			color: #333333;
		}
		#wrapper {
			width: 980px;
			margin: 0 auto;
			padding: 0;
		}
		
		/* Header */
		
		#header {
			clear: both;
			width: 980px;
			height: 300px;
			margin: 0 auto;
			padding: 0px;
		}
		
		/* Menu */
		
		#menu {
			width: 940px;
			margin: 0 auto;
			padding: 0;
			padding-bottom: 10px;
		}
		
		/* Page */
		
		#page {
			width: 940px;
			margin: 0 auto;
			padding: 0px 0px 0px 0px;
		}
		#page-bgtop {
			padding: 10px 0px;
		}
		
		/* Content */
		
		#content {
			float: right;
			width: 580px;
			padding: 20px 0px 0px 0px;
		}
		.post {
			margin-bottom: 15px;
		}
		.post .title {
			height: 38px;
			margin-bottom: 10px;
			padding: 12px 0 0 0px;
			letter-spacing: -.5px;
			color: #82050B;
		}
		.post .title a {
			color: #82050B;
			border: none;
		}
		.post .entry {
			padding: 0px 0px 20px 0px;
			padding-bottom: 20px;
			text-align: justify;
		}
		.links {
			padding-top: 20px;
			font-size: 12px;
			font-weight: bold;
		}
		
		/* Sidebar */
		
		#sidebar {
			float: left;
			width: 280px;
			margin: 0px;
			padding: 0px 0px 0px 0px;
			color: #787878;
		}
		#sidebar ul {
			margin: 0;
			padding: 0;
			list-style: none;
		}
		#sidebar li {
			margin: 0;
			padding: 0;
		}
		#sidebar li ul {
			margin: 0px 0px;
			padding-bottom: 30px;
		}
		#sidebar li li {
			line-height: 25px;
			margin: 0px 30px 15px;
			border-left: none;
			border-bottom: dotted 1px #737373;
		}
		#sidebar h2 {
			height: 30px;
			padding-left: 30px;
			letter-spacing: -.5px;
			font-size: 1.8em;
		}
		#sidebar p {
			margin: 0 0px;
			padding: 0px 30px 20px 30px;
			text-align: justify;
		}
		#sidebar a {
			border: none;
		}
		#sidebar a:hover {
			text-decoration: underline;
			color: #8A8A8A;
		}
		
		/* Footer */
		
		#footer {
			height: 50px;
			margin: 0 auto;
			padding: 0px 0 15px 0;
			background: #ECECEC;
			border-top: 1px solid #DEDEDE;
			font-family: Arial, Helvetica, sans-serif;
		}
		#footer p {
			margin: 0;
			padding-top: 20px;
			line-height: normal;
			font-size: 9px;
			text-transform: uppercase;
			text-align: center;
			color: #A0A0A0;
		}
		#footer a {
			color: #8A8A8A;
		}
		
		/* OTHER */
		
		.aligncenter, div.aligncenter {
			display: block;
			margin-left: auto;
			margin-right: auto;
		}
		
		.pagerfooterlinks {
			line-height: normal;
			color: #ffffff;
		}
		
		.pagerfooterlinks .pagerlink {
			margin: 2px 2px 2px 2px;
			padding: 5px 5px 5px 5px;
			border: 2px solid #333333;
			background-color: #cccccc;
			float: left;
		}
		
		.pagerfooterlinks .pagerlink a {
			margin: 2px 2px 2px 2px;
			padding: 5px 5px 5px 5px;
			font-weight: bold;
			color: #666666;
		}
		
		.pagerfooterlinks .pagerlink a.selected {
			color: #ffffff;
			font-weight: bold;
		}
		
		.pagerfooterlinks .selectedwrap {
			color: #ffffff;
			background-color: #333333;
			font-weight: bold;
		}
	</style>
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
			<carrot:TwoLevelNavigation runat="server" ID="TwoLevelNavigation1" WrapList="false" ForeColor="#424242" SelFGColor="#ffffff" SubBGColor="#787878" SubFGColor="#ffffff"
				UnSelBGColor="#808080" UnSelFGColor="#eeeeee" FontSize="14px" />
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
								<carrot:WidgetContainer ID="phLeftTop" runat="server">
								</carrot:WidgetContainer>
								<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" runat="server" />
								<carrot:WidgetContainer ID="phLeftBottom" runat="server">
								</carrot:WidgetContainer>
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
								<carrot:WidgetContainer ID="phCenterTop" runat="server">
								</carrot:WidgetContainer>
								<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
								<carrot:WidgetContainer ID="phCenterBottom" runat="server">
								</carrot:WidgetContainer>
							</div>
							<div style="clear: both;">
							</div>
							<div class="entry">
								<carrot:WidgetContainer ID="phRightTop" runat="server">
								</carrot:WidgetContainer>
								<carrot:ContentContainer EnableViewState="false" ID="BodyRight" runat="server" />
								<carrot:WidgetContainer ID="phRightBottom" runat="server">
								</carrot:WidgetContainer>
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
										<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="PostMetaWordList1" runat="server" ContentType="Category" MetaDataTitle="Categories:" />
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
							&nbsp;</div>
					</div>
					<!-- end #content -->
					<div style="clear: both;">
						&nbsp;</div>
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
