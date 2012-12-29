<%@ Page Title="Wordpress Import" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="wp-SiteImport.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.wp_SiteImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">

		function AjaxShowAlertImport() {

			errorMessage = $('#importMessage').html();
			if (errorMessage.length > 1) {
				cmsAlertModal(errorMessage);
			}

		}

		function UpdateAjaxShowAlertImport() {

			if (typeof (Sys) != 'undefined') {
				var prm = Sys.WebForms.PageRequestManager.getInstance();
				prm.add_endRequest(function () {
					AjaxShowAlertImport();
				});
			}
		}

		$(document).ready(function () {
			UpdateAjaxShowAlertImport();
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Wordpress Import
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		The below information (if checked) will be imported to the current site. You can always update the information after importing.
	</p>
	<table>
		<tr>
			<td valign="top">
				<asp:CheckBox ID="chkFileGrab" runat="server" />
				Attempt to download page/post attachments and place them in the selected folder.
			</td>
			<td valign="top">
				&nbsp;&nbsp;&nbsp;
			</td>
			<td valign="top">
				<asp:DropDownList ID="ddlFolders" runat="server" DataTextField="FileName" DataValueField="FolderPath">
				</asp:DropDownList>
			</td>
		</tr>
	</table>
	<div style="display: none" id="importMessage">
		<asp:Literal ID="litMessage" runat="server"></asp:Literal>
	</div>
	<fieldset style="width: 90%;">
		<legend>
			<label>
				Site Information
				<asp:CheckBox ID="chkSite" runat="server" />
			</label>
		</legend>
		<table style="width: 99%;">
			<tr>
				<td valign="top" class="tablecaption" style="width: 175px;">
					Exported from WP Version
				</td>
				<td valign="top">
					<asp:Literal ID="litImportSource" runat="server" /><br />
					wxr:
					<asp:Literal ID="litWXR" runat="server" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Export Date
				</td>
				<td valign="top">
					<asp:Literal ID="litDate" runat="server" />
				</td>
			</tr>
			<tr>
				<td colspan="2">
					&nbsp;
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Site Name
				</td>
				<td valign="top">
					<asp:Literal ID="litName" runat="server" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Site Description
				</td>
				<td valign="top">
					<asp:Literal ID="litDescription" runat="server" />
				</td>
			</tr>
		</table>
	</fieldset>
	<fieldset style="width: 90%;">
		<legend>
			<label>
				Pages
				<asp:CheckBox ID="chkPages" runat="server" />
			</label>
		</legend>
		<table>
			<tr>
				<td valign="top" class="tablecaption">
					template:
				</td>
				<td valign="top">
					<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplatePage" runat="server">
					</asp:DropDownList>
				</td>
				<td valign="top">
				</td>
			</tr>
		</table>
		<p>
			<asp:Label ID="lblPages" runat="server" Text="Label"></asp:Label>
			records
		</p>
		<div id="SortableGrid">
			<carrot:CarrotGridView CssClass="datatable" DefaultSort="PostOrder ASC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
				AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
				<Columns>
					<asp:BoundField DataField="PostOrder" HeaderText="Order" />
					<asp:BoundField DataField="ImportFileName" HeaderText="File Name" />
					<asp:BoundField DataField="PostTitle" HeaderText="Post Title" />
					<%--<asp:BoundField DataField="PostType" HeaderText="Post Type" />--%>
					<asp:BoundField DataField="PostDateUTC" HeaderText="Created On" DataFormatString="{0:d}" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsPublished" HeaderText="Published" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
						ShowBooleanImage="true" />
				</Columns>
			</carrot:CarrotGridView>
		</div>
	</fieldset>
	<fieldset style="width: 90%;">
		<legend>
			<label>
				Blog Posts
				<asp:CheckBox ID="chkPosts" runat="server" />
			</label>
		</legend>
		<table>
			<tr>
				<td valign="top" class="tablecaption">
					template:
				</td>
				<td valign="top">
					<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplatePost" runat="server">
					</asp:DropDownList>
				</td>
				<td valign="top">
				</td>
			</tr>
		</table>
		<p>
			<asp:Label ID="lblPosts" runat="server" Text="Label"></asp:Label>
			records
		</p>
		<div id="SortableGrid">
			<carrot:CarrotGridView CssClass="datatable" DefaultSort="PostDateUTC ASC" ID="gvPosts" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
				AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
				<Columns>
					<asp:BoundField DataField="ImportFileName" HeaderText="File Name" />
					<asp:BoundField DataField="PostTitle" HeaderText="Post Title" />
					<%--<asp:BoundField DataField="PostType" HeaderText="Post Type" />--%>
					<asp:BoundField DataField="PostDateUTC" HeaderText="Created On" DataFormatString="{0:d}" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsPublished" HeaderText="Published" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
						ShowBooleanImage="true" />
				</Columns>
			</carrot:CarrotGridView>
		</div>
	</fieldset>
	<p>
		<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" Text="Apply Changes" OnClick="btnSave_Click" />
	</p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
