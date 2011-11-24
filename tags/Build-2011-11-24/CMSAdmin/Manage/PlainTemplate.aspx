<%@ Page Language="C#" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<carrot:jquerybasic runat="server" ID="jquerybasic" />
	<meta http-equiv="content-type" content="text/html; charset=utf-8" />
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
			font-size: 2em;
		}
		h2 {
			font-size: 2.4em;
		}
		h3 {
			font-size: 1.6em;
		}
		p, ul, ol {
			margin-top: 0;
			line-height: 180%;
		}
		ul, ol {
		}
		a {
			text-decoration: none;
			color: #82050B;
		}
		a:hover {
		}
		#wrapper {
			width: 980px;
			margin: 0 auto;
			padding: 0;
		}
		/* Header */#header {
			clear: both;
			width: 980px;
			height: 300px;
			margin: 0 auto;
			padding: 0px;
		}
		/* Menu */#menu {
			width: 940px;
			height: 80px;
			margin: 0 auto;
			padding: 0;
		}
		/* Page */#page {
			width: 940px;
			margin: 0 auto;
			padding: 0px 0px 0px 0px;
		}
		#page-bgtop {
			padding: 20px 0px;
		}
		#page-bgbtm {
		}
		/* Content */#content {
			float: right;
			width: 580px;
			padding: 30px 0px 0px 0px;
		}
		.post {
			margin-bottom: 15px;
		}
		.post-bgtop {
		}
		.post-bgbtm {
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
		/* Sidebar */#sidebar {
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
		/* Footer */#footer {
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
	</style>
</head>
<body>
	<form id="form1" runat="server">
	<div id="wrapper">
		<div id="menu">
			<%--<carrot:TopLevelNavigation MenuWidth="960px" MenuHeight="44px" ForeColor="#000000"
                BackColor="#FFFFFF" runat="server" OverrideCSS="button-style.css" ID="TopLevelNavigation1" />--%>
			<carrot:TwoLevelNavigation MenuWidth="960px" MenuHeight="44px" ForeColor="#000000" BackColor="#FFFFFF" runat="server" ID="TwoLevelNavigation1" />
		</div>
		<!-- end #menu -->
		<div id="page">
			<div id="page-bgtop">
				<div id="page-bgbtm">
					<div id="content">
						<div class="post">
							<h2>
								<asp:Literal ID="litPageHeading" runat="server"></asp:Literal></h2>
							<div class="entry">
								<carrot:WidgetContainer ID="phCenterTop" runat="server">
								</carrot:WidgetContainer>
								<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server"></carrot:ContentContainer>
								<carrot:WidgetContainer ID="phCenterBottom" runat="server">
								</carrot:WidgetContainer>
							</div>
							<div style="clear: both;">
							</div>
							<div class="entry">
								<carrot:WidgetContainer ID="phRightTop" runat="server">
								</carrot:WidgetContainer>
								<carrot:ContentContainer EnableViewState="false" ID="BodyRight" runat="server"></carrot:ContentContainer>
								<carrot:WidgetContainer ID="phRightBottom" runat="server">
								</carrot:WidgetContainer>
							</div>
						</div>
						<div style="clear: both;">
							&nbsp;</div>
					</div>
					<!-- end #content -->
					<div id="sidebar">
						<ul>
							<li>
								<carrot:WidgetContainer ID="phLeftTop" runat="server">
								</carrot:WidgetContainer>
								<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" runat="server"></carrot:ContentContainer>
								<carrot:WidgetContainer ID="phLeftBottom" runat="server">
								</carrot:WidgetContainer>
							</li>
							<li>
								<carrot:ChildNavigation SectionTitle="Child Pages" runat="server" ID="SecondLevelNavigation2"></carrot:ChildNavigation>
							</li>
							<li>
								<carrot:SecondLevelNavigation SectionTitle="Section Pages" runat="server" ID="SecondLevelNavigation1"></carrot:SecondLevelNavigation>
							</li>
							<li>
								<carrot:MostRecentUpdated runat="server" ID="MostRecentUpdated1"></carrot:MostRecentUpdated>
							</li>
						</ul>
					</div>
					<!-- end #sidebar -->
					<div style="clear: both;">
						&nbsp;</div>
				</div>
			</div>
		</div>
		<!-- end #page -->
	</div>
	<div id="footer">
		<p>
			<asp:PlaceHolder ID="myFooter" runat="server">&copy;
				<%= DateTime.Now.Year %>,
				<%=theSite.SiteName%>. All rights reserved. </asp:PlaceHolder>
		</p>
	</div>
	<!-- end #footer -->
	<asp:Panel ID="pnlHiddenControls" Visible="false" runat="server">
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
