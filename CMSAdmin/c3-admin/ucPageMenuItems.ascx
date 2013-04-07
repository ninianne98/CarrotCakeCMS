<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPageMenuItems.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ucPageMenuItems" %>
<div>
	<table style="width: 625px;">
		<tr>
			<td style="width: 48%;">
				<a href="PageIndex.aspx">
					<img class="imgNoBorder" src="/c3-admin/images/table.png" alt="View Page Index" title="View Page Index" />
					View Page Index</a>
			</td>
			<td>
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			</td>
			<td style="width: 48%;">
				<a href="PageTemplateUpdate.aspx">
					<img class="imgNoBorder" src="/c3-admin/images/overlays.png" alt="Apply Skins" title="Bulk Apply Templates/Skins" />
					Bulk Apply Templates/Skins</a>
			</td>
		</tr>
		<%-- 
		<tr>
			<td style="width: 48%;">
				<a href="SiteMap.aspx">
					<img class="imgNoBorder"  src="/c3-admin/images/chart_organisation.png" alt="Map" title="Edit Site Map" />
					Edit Site Map</a>
			</td>
			<td>
				&nbsp;
			</td>
			<td style="width: 48%;">
				<a href="PageImport.aspx">
					<img class="imgNoBorder"  src="/c3-admin/images/book_previous.png" alt="Page" title="Import Page" />
					Import Content Page</a>
			</td>
		</tr>
		--%>
		<tr>
			<td style="width: 48%;">
				<a href="PageAddEdit.aspx?id=">
					<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add as WYSIWYG" />
					Add Page (with HTML editor)</a>
			</td>
			<td>
				&nbsp;
			</td>
			<td style="width: 48%;">
				<a href="PageAddEdit.aspx?mode=raw&id=">
					<img class="imgNoBorder" src="/c3-admin/images/script_add.png" alt="Add" title="Add as Plain Text" />
					Add Page (as plain text)</a>
			</td>
		</tr>
	</table>
</div>
<br />
