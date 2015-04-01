<%@ Page Title="Content Import" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PageImport.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.PageImport" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Content Import
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
	<p>
		<asp:Label ID="lblWarning" runat="server" />
	</p>
	<p>
		Select a previously exported content to import:<br />
		<asp:FileUpload ID="upFile" runat="server" />
	</p>
	<p>
		You will be able to review the imported data before finalizing changes.
	</p>
	<p>
		<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /><br />
	</p>
</asp:Content>
