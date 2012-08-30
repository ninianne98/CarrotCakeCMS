<%@ Page ValidateRequest="false" Title="Module Index" Language="C#" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true"
	CodeBehind="ModulePopup.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.ModulePopup" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Module Popup
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:PlaceHolder ID="phAjax" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
	<asp:PlaceHolder ID="phNoAjax" runat="server"></asp:PlaceHolder>
</asp:Content>
