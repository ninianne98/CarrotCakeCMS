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
	<script type="text/javascript">

		function chkFileGrabClick(obj) {
			cmsIsPageValid();
		}

		function validateFolderSelection(oSrc, args) {

			var fldr = '#<%=ddlFolders.ClientID %>';
			var chk = '#<%=chkFileGrab.ClientID %>';

			var ddlFolders = $(fldr).get(0).selectedIndex;
			var chkFileGrab = $(chk).prop('checked');

			if (chkFileGrab) {
				if (ddlFolders < 1) {
					args.IsValid = false;
					return;
				} else {
					args.IsValid = true;
					return;
				}
			} else {
				args.IsValid = true;
				return;
			}
		}

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Wordpress Import
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		The below information (if checked) will be imported to the current site. You can always update the information after importing.
	</p>
	<table style="width: 700px;">
		<tr>
			<td>
				<asp:CheckBox ID="chkFileGrab" runat="server" onclick="chkFileGrabClick(this);" />
				Attempt to download page/post attachments and place them in the selected folder.
				<asp:Literal ID="litTrust" runat="server"><b>Downloading images requires a full trust website, and this installation has not been detected as such, leaving the download option disabled. </b> </asp:Literal>
			</td>
			<td>
				&nbsp;&nbsp;&nbsp;
			</td>
			<td>
				<asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Required" Display="Dynamic" CssClass="validationError" ForeColor="" ControlToValidate="ddlFolders"
					ClientValidationFunction="validateFolderSelection" ValidationGroup="inputForm" />
				<asp:DropDownList ID="ddlFolders" runat="server" DataTextField="FileName" DataValueField="FolderPath" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:CheckBox ID="chkFixBodies" runat="server" Checked="true" />
				Uncheck this box if you want the content bodies to remain in their original format, otherwise paragraph and line break tags will be added.
			</td>
			<td>
				&nbsp;
			</td>
			<td>
				&nbsp;
			</td>
		</tr>
		<tr>
			<td>
				<asp:CheckBox ID="chkAuthors" runat="server" Checked="true" />
				Uncheck this box if you do not want new user accounts to be created. Accounts will only be created if the username and email address have not already
				been used in this system. Password will be set according to normal reset values, and the user will have to request a new password by email to login. If
				this is unchecked and a matching account exists, the discovered user will be recorded as the content editor. If the user account does not exist, the current
				user will be recorded as the content editor.
			</td>
			<td>
				&nbsp;
			</td>
			<td>
				&nbsp;
			</td>
		</tr>
		<tr>
			<td>
				<asp:CheckBox ID="chkMapAuthor" runat="server" Checked="true" />
				Uncheck this box if you want all imported content to be recorded under the curent user. This option trumps the account creation checkbox.
			</td>
			<td>
				&nbsp;
			</td>
			<td>
				&nbsp;
			</td>
		</tr>
	</table>
	<div style="display: none" id="importMessage">
		<asp:Literal ID="litMessage" runat="server" />
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
				<td class="tablecaption" style="width: 175px;">
					Exported from WP Version
				</td>
				<td>
					<asp:Literal ID="litImportSource" runat="server" /><br />
					wxr:
					<asp:Literal ID="litWXR" runat="server" />
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
			<carrot:CarrotGridView CssClass="datatable" DefaultSort="PostOrder ASC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
				AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
				<EmptyDataTemplate>
					<p>
						<b>No records found.</b>
					</p>
				</EmptyDataTemplate>
				<Columns>
					<asp:BoundField DataField="PostOrder" HeaderText="Order" />
					<asp:BoundField DataField="ImportFileName" HeaderText="File Name" />
					<asp:BoundField DataField="PostTitle" HeaderText="Post Title" />
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
			<carrot:CarrotGridView CssClass="datatable" DefaultSort="PostDateUTC DESC" ID="gvPosts" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
				AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
				<EmptyDataTemplate>
					<p>
						<b>No records found.</b>
					</p>
				</EmptyDataTemplate>
				<Columns>
					<asp:BoundField DataField="ImportFileName" HeaderText="File Name" />
					<asp:BoundField DataField="PostTitle" HeaderText="Post Title" />
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
