<%@ Page Title="" Language="C#" MasterPageFile="~/c3-admin/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="SiteMapPop.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.SiteMapPop" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<%@ Register Src="ucSiteMap.ascx" TagName="ucSiteMap" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site Map
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<uc1:ucSiteMap ID="ucSiteMap1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
