<%@ Page Title="Setup Database" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Public.Master" AutoEventWireup="true" CodeBehind="DatabaseSetup.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.DatabaseSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div style="height: 20px; width: 18px; float: right; border: 1px solid #ffffff; clear: both;">
		<a href="/">
			<img class="imgNoBorder" src="/c3-admin/images/house_go.png" alt="Homepage" title="Homepage" /></a>
	</div>
	<div style="border: solid 0px #000000; width: 350px; height: 140px; overflow: auto;">
		<asp:Literal ID="litMsg" runat="server"></asp:Literal>
	</div>
	<p>
		<asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
		<asp:Button ID="btnCreate" runat="server" Text="Create User" OnClick="btnCreate_Click" />
	</p>
	<p>
		<asp:Literal ID="litCMSBuildInfo" runat="server"></asp:Literal>
	</p>
</asp:Content>
