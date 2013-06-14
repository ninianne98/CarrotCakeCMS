<%@ Page Title="Site-Wide Template Update" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteTemplateUpdate.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.SiteTemplateUpdate" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucSitePageDrillDown.ascx" TagName="ucSitePageDrillDown" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site-Wide Template Update
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<fieldset style="width: 500px;">
		<legend>
			<label>
				Content
			</label>
		</legend>
		<p>
			<b>Home Page</b>&nbsp;&nbsp;&nbsp;&nbsp;
			<asp:Literal ID="litHomepage" runat="server" />
			<br />
			<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlHome" runat="server" />
			<br />
		</p>
		<p>
			<b>All Content Pages (only)</b>
			<br />
			<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlPages" runat="server" />
			<br />
		</p>
		<p>
			<b>All Top Level Pages (only)</b>
			<br />
			<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTop" runat="server" />
			<br />
		</p>
		<p>
			<b>All Sub Level Pages (only)</b>
			<br />
			<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlSub" runat="server" />
			<br />
		</p>
	</fieldset>
	<fieldset style="width: 500px;">
		<legend>
			<label>
				Blog
			</label>
		</legend>
		<p>
			<b>Index Page</b>
			<br />
			<span style="clear: both; display: block; min-height: 4px;">
				<!-- parent page plugin-->
				<uc1:ucSitePageDrillDown ID="ParentPagePicker" runat="server" />
			</span>
			<br />
			<span style="clear: both; display: block">
				<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlBlog" runat="server" />
			</span>
			<br />
		</p>
		<p>
			<b>All Blog Posts (only)</b>
			<br />
			<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlPosts" runat="server" />
			<br />
		</p>
	</fieldset>
	<fieldset style="width: 500px;">
		<legend>
			<label>
				Everything (including home and blog index)
			</label>
		</legend>
		<p>
			<b>All Content (Content Pages and Blog Posts)</b>
			<br />
			<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlAll" runat="server" />
			<br />
		</p>
	</fieldset>
	<p>
		<br />
	</p>
	<p>
		<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
	</p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
