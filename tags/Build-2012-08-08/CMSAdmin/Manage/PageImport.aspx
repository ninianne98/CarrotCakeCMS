<%@ Page Title="Page Import" Language="C#" MasterPageFile="~/Manage/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PageImport.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.PageImport" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucPageMenuItems.ascx" TagName="ucPageMenuItems" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Page Import
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
	<uc1:ucPageMenuItems ID="ucPageMenuItems1" runat="server" />
	<br />
	<p>
		<asp:Label ID="lblWarning" runat="server"></asp:Label>
	</p>
	<p>
		Select a previously exported page to import:<br />
		<asp:FileUpload ID="upFile" runat="server" />
	</p>
	<p>
		<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /><br />
	</p>
</asp:Content>
