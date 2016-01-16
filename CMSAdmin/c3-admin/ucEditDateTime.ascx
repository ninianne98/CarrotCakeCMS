<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEditDateTime.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ucEditDateTime" %>
<asp:TextBox onkeypress="return ProcessKeyPress(event)" ID="txtDate" runat="server" CssClass="dateRegion" Columns="16" />
<asp:TextBox onkeypress="return ProcessKeyPress(event)" ID="txtTime" runat="server" CssClass="timeRegion" Columns="10" />