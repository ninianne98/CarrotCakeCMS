<%@ Page Title="CalendarAdmin" ValidateRequest="false" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
	CodeBehind="CalendarAdmin.aspx.cs" Inherits="Carrotware.CMS.UI.Plugins.CalendarModule.WebForm1" %>

<%@ MasterType VirtualPath="~/Site1.Master" %>
<%@ Register Src="~/CalendarAdmin.ascx" TagPrefix="uc" TagName="CalendarAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<uc:CalendarAdmin runat="server" ID="ucCalendarAdmin" />
</asp:Content>