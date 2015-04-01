<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FaqRandomOne.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.FAQ2Module.FaqRandomOne" %>
<asp:Panel runat="server" ID="pnlWrap">
	<div>
		<asp:Literal ID="litQuest" runat="server" /></div>
	<div>
		<asp:Literal ID="litAns" runat="server" /></div>
	<asp:Panel runat="server" ID="pnlButton">
		<div>
			<a class="button" href="<%=LinkUrl %>">
				<%=LinkText %></a></div>
	</asp:Panel>
</asp:Panel>
