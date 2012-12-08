<%@ Page Title="Setup Database" Language="C#" MasterPageFile="~/Manage/MasterPages/Public.Master" AutoEventWireup="true" CodeBehind="DatabaseSetup.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.DatabaseSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div style="border: solid 0px #000000; width: 375px; height: 140px; overflow: auto;">
		<asp:Literal ID="litMsg" runat="server"></asp:Literal>
	</div>
	<p>
		<asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
		<asp:Button ID="btnCreate" runat="server" Text="Create User" OnClick="btnCreate_Click" />
	</p>
</asp:Content>
