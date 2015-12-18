<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FaqListTop.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.FAQ2Module.FaqListTop" %>
<asp:Repeater ID="rpFAQ" runat="server">
	<ItemTemplate>
		<div>
			<%#Eval("Question") %></div>
		<div>
			<%#Eval("Answer") %></div>
	</ItemTemplate>
</asp:Repeater>
