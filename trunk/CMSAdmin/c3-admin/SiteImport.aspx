<%@ Page Title="Site Import" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteImport.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.SiteImport" %>

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
	Site Import
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		The below information (if checked) will be imported to the current site. You can always update the information after importing.
	</p>
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
				<td class="tablecaption" style="width: 200px;">
					Exported from CMS Version
				</td>
				<td>
					<asp:Literal ID="litImportSource" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Export Date
				</td>
				<td>
					<asp:Literal ID="litDate" runat="server" />
				</td>
			</tr>
			<tr>
				<td colspan="2">
					&nbsp;
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Site Name
				</td>
				<td>
					<asp:Literal ID="litName" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Site Description
				</td>
				<td>
					<asp:Literal ID="litDescription" runat="server" />
				</td>
			</tr>
		</table>
		<table style="width: 99%;">
			<tr>
				<td>
					<asp:CheckBox ID="chkAuthors" runat="server" Checked="true" />
					Uncheck this box if you do not want user accounts to be created. Accounts will only be created if the username and email address have not already been
					used in this system. Password will be set according to normal reset values, and the user will have to request a new password by email to login. If this
					is unchecked and a matching account exists, the discovered user will be recorded as the content editor. If the user account does not exist, the current
					user will be recorded as the content editor.
				</td>
			</tr>
			<tr>
				<td>
					<asp:CheckBox ID="chkMapAuthor" runat="server" Checked="true" />
					Uncheck this box if you want all imported content to be recorded under the curent user. This option trumps the account creation checkbox.
				</td>
			</tr>
		</table>
	</fieldset>
	<fieldset style="width: 90%;">
		<legend>
			<label>
				Comments
				<asp:CheckBox ID="chkComments" runat="server" />
			</label>
		</legend>
		<p>
			<asp:Literal ID="lblComments" runat="server" Text="Label" />
			records
		</p>
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
				<td class="tablecaption">
					template:
				</td>
				<td>
					<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplatePage" runat="server" />
				</td>
			</tr>
		</table>
		<p>
			<asp:Literal ID="lblPages" runat="server" Text="Label" />
			records
		</p>
		<div class="SortableGrid">
			<carrot:CarrotGridView CssClass="datatable" DefaultSort="NavOrder ASC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
				AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
				<EmptyDataTemplate>
					<p>
						<b>No records found.</b>
					</p>
				</EmptyDataTemplate>
				<Columns>
					<asp:BoundField DataField="NavOrder" HeaderText="Order" />
					<asp:BoundField DataField="FileName" HeaderText="File Name" />
					<asp:BoundField DataField="NavMenuText" HeaderText="Post Title" />
					<asp:BoundField DataField="CreateDate" HeaderText="Created On" DataFormatString="{0:d}" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="PageActive" HeaderText="Published" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
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
				<td class="tablecaption">
					template:
				</td>
				<td>
					<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplatePost" runat="server" />
				</td>
			</tr>
		</table>
		<p>
			<asp:Literal ID="lblPosts" runat="server" Text="Label" />
			records
		</p>
		<div class="SortableGrid">
			<carrot:CarrotGridView CssClass="datatable" DefaultSort="CreateDate DESC" ID="gvPosts" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
				AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
				<EmptyDataTemplate>
					<p>
						<b>No records found.</b>
					</p>
				</EmptyDataTemplate>
				<Columns>
					<asp:BoundField DataField="FileName" HeaderText="File Name" />
					<asp:BoundField DataField="NavMenuText" HeaderText="Post Title" />
					<asp:BoundField DataField="CreateDate" HeaderText="Created On" DataFormatString="{0:d}" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="PageActive" HeaderText="Published" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
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
