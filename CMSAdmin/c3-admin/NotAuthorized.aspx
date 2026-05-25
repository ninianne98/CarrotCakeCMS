<%@ Page Title="Not Authorized" Language="C#" MasterPageFile="MasterPages/Public.Master" AutoEventWireup="true" CodeBehind="NotAuthorized.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.NotAuthorized" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:PlaceHolder ID="phNotAuth" runat="server">
		<h2>Not Authorized</h2>
		<p>
			You do not have access to the admin tools for this website.
			<br />
		</p>
	</asp:PlaceHolder>
	<asp:PlaceHolder ID="phIsSiteEditor" runat="server">
		<p>
			View <a href="<%=SiteFilename.SiteInfoURL %>">Site Info</a>.
			<br />
		</p>
	</asp:PlaceHolder>
	<p>
		Click <a href="<%=SiteFilename.LogonURL %>">here</a> to logon.
	</p>
	<p>
		Click to <a href="<%=SiteData.CurrentScriptName %>?signout=true">sign out</a>.
	</p>
</asp:Content>
