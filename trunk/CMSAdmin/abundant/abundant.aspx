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
	<carrot:jquery runat="server" ID="jquery1" JQVersion="1.6" />
	<carrot:jqueryui runat="server" ID="jqueryui1" />
	<meta http-equiv="content-type" content="text/html; charset=utf-8" />
	<title>Abundant by Free CSS Templates</title>
	<asp:PlaceHolder ID="myCSS" runat="server">
		<link href="<%=pageContents.TemplateFolderPath %>style.css" rel="stylesheet" type="text/css" media="screen" />
	</asp:PlaceHolder>
</head>
<body>
	<form id="form1" runat="server">
	<div id="wrapper">
		<div id="header">
			<div id="menu">
				<carrot:TopLevelNavigation CSSSelected="current_page_item" runat="server" ID="TopLevelNavigation1" />
			</div>
			<!-- end #menu -->
		</div>
		<!-- end #header -->
		<div id="logo">
			<h1>
				<asp:PlaceHolder ID="myHeading" runat="server"><a href="/">
					<%=theSite.SiteName%></a></asp:PlaceHolder>
			</h1>
			<p>
				<em>template design by <a href="http://www.freecsstemplates.org/">Free CSS Templates</a></em></p>
		</div>
		<hr />
		<!-- end #logo -->
		<!-- end #header-wrapper -->
		<div id="page">
			<div id="content">
				<div class="post">
					<h2 class="title">
						<asp:Literal ID="litPageHeading" runat="server" /></h2>
					<div class="entry">
						<carrot:WidgetContainer ID="phCenterTop" runat="server">
						</carrot:WidgetContainer>
						<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server"></carrot:ContentContainer>
						<carrot:WidgetContainer ID="phCenterBottom" runat="server">
						</carrot:WidgetContainer>
					</div>
				</div>
			</div>
			<!-- end #content -->
			<div id="sidebar">
				<ul>
					<li>
						<carrot:ChildNavigation SectionTitle="Child Pages" runat="server" ID="SecondLevelNavigation2"></carrot:ChildNavigation>
					</li>
					<li>
						<carrot:SecondLevelNavigation SectionTitle="Section Pages" runat="server" ID="SecondLevelNavigation1"></carrot:SecondLevelNavigation>
					</li>
					<li>
						<carrot:MostRecentUpdated UpdateTitle="Recent Updates" runat="server" ID="MostRecentUpdated1"></carrot:MostRecentUpdated>
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
		<p>
			<asp:PlaceHolder ID="myFooter" runat="server">&copy;
				<%= DateTime.Now.Year %>,
				<%=theSite.SiteName%>. All rights reserved. </asp:PlaceHolder>
			Design by <a target="_blank" href="http://www.freecsstemplates.org/">Free CSS Templates</a>.</p>
	</div>
	<!-- end #footer -->
	<asp:Panel ID="pnlHiddenControls" Visible="false" runat="server">
		<carrot:WidgetContainer ID="phLeftTop" runat="server">
		</carrot:WidgetContainer>
		<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" runat="server"></carrot:ContentContainer>
		<carrot:WidgetContainer ID="phLeftBottom" runat="server">
		</carrot:WidgetContainer>
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
