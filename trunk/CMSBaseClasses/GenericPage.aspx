<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenericPage.aspx.cs" Inherits="Carrotware.CMS.UI.Base.GenericPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<%--
	<carrot:TopLevelNavigation runat="server" ID="TopLevelNavigation1"></carrot:TopLevelNavigation>
	<carrot:SecondLevelNavigation IncludeParent="true" runat="server" ID="SecondLevelNavigation1"></carrot:SecondLevelNavigation>
	<carrot:TwoLevelNavigation runat="server" ID="TwoLevelNavigation1"></carrot:TwoLevelNavigation>
	--%>
	<h2>
		<asp:Literal ID="litPageHeading" runat="server"></asp:Literal></h2>
	<carrot:WidgetContainer ID="phCenterTop" runat="server">
	</carrot:WidgetContainer>
	<carrot:ContentContainer ID="BodyCenter" runat="server"></carrot:ContentContainer>
	<carrot:WidgetContainer ID="phCenterBottom" runat="server">
	</carrot:WidgetContainer>
	<br />
	<asp:Panel ID="pnlHiddenControls" Visible="false" runat="server">
		<carrot:WidgetContainer ID="phLeftTop" runat="server">
		</carrot:WidgetContainer>
		<carrot:ContentContainer ID="BodyLeft" runat="server"></carrot:ContentContainer>
		<carrot:WidgetContainer ID="phLeftBottom" runat="server">
		</carrot:WidgetContainer>
		<br />
		<carrot:WidgetContainer ID="phRightTop" runat="server">
		</carrot:WidgetContainer>
		<carrot:ContentContainer ID="BodyRight" runat="server"></carrot:ContentContainer>
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
		<carrot:WidgetContainer ID="phExtraZone7" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone8" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone9" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone10" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone11" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone12" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone13" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone14" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone15" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone16" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone17" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone18" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone19" runat="server">
		</carrot:WidgetContainer>
		<carrot:WidgetContainer ID="phExtraZone20" runat="server">
		</carrot:WidgetContainer>
	</asp:Panel>
	</form>
</body>
</html>
