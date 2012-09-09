<%@ Page ValidateRequest="false" Title="Module Index" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true"
	CodeBehind="ModuleIndex.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.ModuleIndex" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Module Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:PlaceHolder ID="phAjax" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
	<asp:PlaceHolder ID="phNoAjax" runat="server"></asp:PlaceHolder>
</asp:Content>
