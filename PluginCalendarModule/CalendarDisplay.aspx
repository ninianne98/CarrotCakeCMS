<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CalendarDisplay.aspx.cs"
	Inherits="Carrotware.CMS.UI.Plugins.CalendarModule.WebForm3" %>

<%@ MasterType VirtualPath="~/Site1.Master" %>
<%@ Register Src="~/CalendarDisplay.ascx" TagPrefix="uc" TagName="CalendarDisplay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<uc:CalendarDisplay runat="server" ID="ucCalendarDisplay" />
</asp:Content>