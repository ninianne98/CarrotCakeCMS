<%@ Page Title="CalendarAdminAddEdit" ValidateRequest="false" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
	CodeBehind="CalendarAdminAddEdit.aspx.cs" Inherits="Carrotware.CMS.UI.Plugins.CalendarModule.WebForm2" %>

<%@ MasterType VirtualPath="~/Site1.Master" %>
<%@ Register Src="~/CalendarAdminAddEdit.ascx" TagPrefix="uc" TagName="CalendarAdminAddEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<uc:CalendarAdminAddEdit runat="server" ID="ucCalendarAdminAddEdit" />
</asp:Content>