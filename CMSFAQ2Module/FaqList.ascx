<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FaqList.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.FAQ2Module.FaqList" %>
<asp:Repeater ID="rpFAQ" runat="server">
	<ItemTemplate>
		<div>
			<%#Eval("Question") %></div>
		<div>
			<%#Eval("Answer") %></div>
	</ItemTemplate>
</asp:Repeater>
