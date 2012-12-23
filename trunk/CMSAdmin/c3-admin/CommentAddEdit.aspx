<%@ Page Title="CommentAddEdit" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="CommentAddEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.CommentAddEdit" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucCommentAddEdit.ascx" TagName="ucCommentAddEdit" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Comment Add/Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<uc1:ucCommentAddEdit ID="ucCommentAddEdit1" runat="server" ReturnPage="CommentIndex.aspx" IsFullSite="true" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
