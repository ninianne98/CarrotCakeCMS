<%@ Page Title="Dashboard" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.Default" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site Dashboard
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div class="ui-widget-content ui-corner-all" style="width: 762px; clear: both;">
		<div class="ui-widget-header ui-corner-all dashboardCell">
			<a href="/c3-admin/SiteInfo.aspx">
				<img alt="" src="Images/SiteData.png" class="imgNoBorder" />
				<br />
				<br />
				Site Info</a>
		</div>
		<div class="ui-widget-header ui-corner-all dashboardCell">
			<a href="/c3-admin/PageImport.aspx">
				<img alt="" src="Images/ImportData.png" class="imgNoBorder" />
				<br />
				<br />
				Import Content</a>
		</div>
		<div class="ui-widget-header ui-corner-all dashboardCell">
			<a href="/c3-admin/SiteExport.aspx">
				<img alt="" src="Images/ExportData.png" class="imgNoBorder" />
				<br />
				<br />
				Export Content</a>
		</div>
		<div class="ui-widget-header ui-corner-all dashboardCell">
			<a href="/c3-admin/SiteTemplateUpdate.aspx">
				<img alt="" src="Images/TemplateUpdate.png" class="imgNoBorder" />
				<br />
				<br />
				Bulk Apply Templates</a>
		</div>
		<div class="ui-widget-header ui-corner-all dashboardCell">
			<a href="/c3-admin/SiteContentStatusChange.aspx">
				<img alt="" src="Images/StatusChange.png" class="imgNoBorder" />
				<br />
				<br />
				Bulk Change Status</a>
		</div>
		<div class="ui-widget-header ui-corner-all dashboardCell">
			<a href="/c3-admin/PageIndex.aspx">
				<img alt="" src="Images/PageIndex.png" class="imgNoBorder" />
				<br />
				<br />
				Pages
				<asp:Literal ID="litPage" runat="server" /></a>
			<p>
				<a href="PageAddEdit.aspx?id=">
					<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add as WYSIWYG" />
					Add</a>
			</p>
		</div>
		<div class="ui-widget-header ui-corner-all dashboardCell">
			<a href="/c3-admin/BlogPostIndex.aspx">
				<img alt="" src="Images/BlogIndex.png" class="imgNoBorder" />
				<br />
				<br />
				Posts
				<asp:Literal ID="litPost" runat="server" /></a>
			<p>
				<a href="BlogPostAddEdit.aspx?id=">
					<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add as WYSIWYG" />
					Add </a>
			</p>
		</div>
		<div class="ui-widget-header ui-corner-all dashboardCell">
			<a href="/c3-admin/CategoryIndex.aspx">
				<img alt="" src="Images/PostCategories.png" class="imgNoBorder" />
				<br />
				<br />
				Categories
				<asp:Literal ID="litCat" runat="server" /></a>
			<p>
				<a href="CategoryAddEdit.aspx?id=">
					<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add as WYSIWYG" />
					Add </a>
			</p>
		</div>
		<div class="ui-widget-header ui-corner-all dashboardCell">
			<a href="/c3-admin/TagIndex.aspx">
				<img alt="" src="Images/PostTags.png" class="imgNoBorder" />
				<br />
				<br />
				Tags
				<asp:Literal ID="litTag" runat="server" /></a>
			<p>
				<a href="TagAddEdit.aspx?id=">
					<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add as WYSIWYG" />
					Add </a>
			</p>
		</div>
		<div class="ui-widget-header ui-corner-all dashboardCell">
			<a href="/c3-admin/ModuleIndex.aspx">
				<img alt="" src="Images/ModuleAdmin.png" class="imgNoBorder" />
				<br />
				<br />
				Modules</a>
		</div>
		<div class="ui-widget-header ui-corner-all dashboardCell">
			<a href="/c3-admin/SiteMap.aspx">
				<img alt="" src="Images/SiteMap.png" class="imgNoBorder" />
				<br />
				<br />
				Edit Site Map</a>
		</div>
		<div class="ui-widget-header ui-corner-all dashboardCell">
			<a href="/c3-admin/SiteSkinIndex.aspx">
				<img alt="" src="Images/SiteSkins.png" class="imgNoBorder" />
				<br />
				<br />
				Site Skin Index</a>
		</div>
		<asp:PlaceHolder ID="phUserSecurity" runat="server">
			<div class="ui-widget-header ui-corner-all dashboardCell">
				<a href="/c3-admin/UserMembership.aspx">
					<img alt="" src="Images/UserList.png" class="imgNoBorder" />
					<br />
					<br />
					Users</a>
			</div>
			<div class="ui-widget-header ui-corner-all dashboardCell">
				<a href="/c3-admin/UserGroups.aspx">
					<img alt="" src="Images/GroupList.png" class="imgNoBorder" />
					<br />
					<br />
					Groups</a>
			</div>
		</asp:PlaceHolder>
		<div style="clear: both;">
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
