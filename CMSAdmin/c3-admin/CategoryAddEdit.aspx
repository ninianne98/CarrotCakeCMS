<%@ Page Title="CategoryAddEdit" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="CategoryAddEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.CategoryAddEdit" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = "/c3-admin/CMS.asmx";

		var thePageID = '<%=guidItemID %>';

		var thePage = '';


		function CheckFileName() {
			thePage = $('#<%= txtSlug.ClientID %>').val();

			$('#<%= txtFileValid.ClientID %>').val('');

			var webMthd = webSvc + "/ValidateUniqueCategory";
			var myPage = MakeStringSafe(thePage);

			$.ajax({
				type: "POST",
				url: webMthd,
				data: "{'TheSlug': '" + myPage + "', 'ItemID': '" + thePageID + "'}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: editFilenameCallback,
				error: cmsAjaxFailed
			});
		}

		$(document).ready(function () {
			CheckFileName();
		});

		function editFilenameCallback(data, status) {
			if (data.d == "OK") {
				$('#<%= txtFileValid.ClientID %>').val('VALID');
			} else {
				$('#<%= txtFileValid.ClientID %>').val('');
			}
			Page_ClientValidate();
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Category Add/Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<table width="700">
		<tr>
			<td width="125" valign="top" class="tablecaption">
				url slug:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="CheckFileName()" ID="txtSlug" runat="server" Columns="45" MaxLength="100" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtSlug" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
					Display="Dynamic" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtFileValid" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Not Valid/Unique"
					Display="Dynamic" />
				<asp:TextBox runat="server" ValidationGroup="inputForm" ID="txtFileValid" MaxLength="25" Columns="25" Style="display: none;" />
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				caption:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtLabel" runat="server" Columns="45" MaxLength="100" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtLabel" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required"
					Display="Dynamic" />
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				public:
			</td>
			<td valign="top">
				<asp:CheckBox ID="chkPublic" runat="server" />
			</td>
		</tr>
	</table>
	<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Save" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<input type="button" id="btnCancel" value="Cancel" onclick="cancelEditing()" />
	<br />
	<div style="display: none;">
		<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
	</div>
	<script type="text/javascript">

		function SubmitPage() {
			CheckFileName();
			setTimeout("ClickBtn();", 250);
		}
		function ClickBtn() {
			$('#<%=btnSave.ClientID %>').click();
		}

		function cancelEditing() {
			window.setTimeout("location.href = './CategoryIndex.aspx';", 250);
		}

	</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
