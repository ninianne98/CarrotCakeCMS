<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenericPage.aspx.cs" Inherits="Carrotware.CMS.UI.Base.GenericPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<h2>
			<asp:Literal ID="litPageHeading" runat="server" /></h2>
		<carrot:WidgetContainer ID="phCenterTop" runat="server">
		</carrot:WidgetContainer>
		<carrot:ContentContainer ID="BodyCenter" runat="server" />
		<carrot:WidgetContainer ID="phCenterBottom" runat="server">
		</carrot:WidgetContainer>
	</div>
	<asp:Panel ID="pnlHiddenControls" Visible="false" runat="server">
		<!-- LEFT -->
		<carrot:WidgetContainer ID="phLeftTop" runat="server">
		</carrot:WidgetContainer>
		<carrot:ContentContainer ID="BodyLeft" runat="server" />
		<carrot:WidgetContainer ID="phLeftBottom" runat="server">
		</carrot:WidgetContainer>
		<!-- RIGHT -->
		<carrot:WidgetContainer ID="phRightTop" runat="server">
		</carrot:WidgetContainer>
		<carrot:ContentContainer ID="BodyRight" runat="server" />
		<carrot:WidgetContainer ID="phRightBottom" runat="server">
		</carrot:WidgetContainer>
	</asp:Panel>
	</form>
</body>
</html>
