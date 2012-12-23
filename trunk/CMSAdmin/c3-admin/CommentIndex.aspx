<%@ Page Title="CommentIndex" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="CommentIndex.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.CommentIndex" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucCommentIndex.ascx" TagName="ucCommentIndex" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Comment Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<uc1:ucCommentIndex ID="ucCommentIndex1" runat="server" LinkingPage="CommentAddEdit.aspx" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
