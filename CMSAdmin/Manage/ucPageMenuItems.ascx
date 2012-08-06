<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPageMenuItems.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.ucPageMenuItems" %>
<div>
	<table width="425">
		<tr>
			<td width="200">
				<a href="PageIndex.aspx">
					<img border="0" src="/manage/images/table.png" alt="View Page Index" title="View Page Index" />
					View Page Index</a>
			</td>
			<td>
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			</td>
			<td width="200">
				<a href="PageTemplateUpdate.aspx">
					<img border="0" src="/manage/images/overlays.png" alt="Apply Skins" title="Bulk Apply Templates/Skins" />
					Bulk Apply Templates/Skins</a>
			</td>
		</tr>
		<tr>
			<td width="200">
				<a href="SiteSkinIndex.aspx">
					<img border="0" src="/manage/images/page_white_wrench.png" alt="Skins" title="Edit Site Skins" />
					Edit Site Skins</a>
			</td>
			<td>
				&nbsp;
			</td>
			<td width="200">
				<a href="SiteMap.aspx">
					<img border="0" src="/manage/images/chart_organisation.png" alt="Map" title="Edit Site Map" />
					Edit Site Map</a>
			</td>
		</tr>
		<tr>
			<td width="200">
				<a href="PageImport.aspx">
					<img border="0" src="/manage/images/book_previous.png" alt="Page" title="Import Page" />
					Import Page</a>
			</td>
			<td>
				&nbsp;
			</td>
			<td width="200">
				&nbsp;
			</td>
		</tr>
		<tr>
			<td width="200">
				<a href="PageAddEdit.aspx?id=">
					<img border="0" src="/manage/images/add.png" alt="Add" title="Add as WYSIWYG" />
					Add Page (with HTML editor)</a>
			</td>
			<td>
				&nbsp;
			</td>
			<td width="200">
				<a href="PageAddEdit.aspx?mode=raw&id=">
					<img border="0" src="/manage/images/script_add.png" alt="Add" title="Add as Plain Text" />
					Add Page (as plain text)</a>
			</td>
		</tr>
	</table>
</div>
<br />
