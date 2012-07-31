<%@ Page Title="Add Page Info" Language="C#" MasterPageFile="~/Manage/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="PageAddChild.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.PageAddChild" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = "/Manage/CMS.asmx";

		var thePageID = '<%=Guid.Empty %>';

		var thePage = '';

		function MakeStringSafe(val) {
			val = Base64.encode(val);
			return val;
		}

		function cmsAjaxFailed(request) {
			var s = "";
			s = s + "<b>status: </b>" + request.status + '<br />\r\n';
			s = s + "<b>statusText: </b>" + request.statusText + '<br />\r\n';
			s = s + "<b>responseText: </b>" + request.responseText + '<br />\r\n';
			cmsAlertModal(s);
		}

		function CheckFileName() {
			thePage = $('#<%= txtFileName.ClientID %>').val();

			$('#<%= txtFileValid.ClientID %>').val('');

			var webMthd = webSvc + "/ValidateUniqueFilename";
			var myPage = MakeStringSafe(thePage);

			$.ajax({
				type: "POST",
				url: webMthd,
				data: "{'TheFileName': '" + myPage + "', 'PageID': '" + thePageID + "'}",
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
			if (data.d == "PASS") {
				$('#<%= txtFileValid.ClientID %>').val('VALID');
			} else {
				$('#<%= txtFileValid.ClientID %>').val('');
			}
			Page_ClientValidate();
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Add Page Info
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:Panel runat="server" ID="pnlSaved">
		<h2>
			The page
			<asp:Literal ID="litPageName" runat="server"></asp:Literal>
			has been created.</h2>
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlAdd">
		<p>
			This creates a blank and unpublished page (no content) under the currently being edited page. Will have the same template as the currently being
			edited page.
		</p>
		<table width="700">
			<tr>
				<td valign="top" class="tablecaption">
					title bar:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTitle" runat="server" Columns="45" MaxLength="200" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtTitle" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
						Display="Dynamic"></asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					filename:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="CheckFileName()" ID="txtFileName" runat="server"
						Columns="45" MaxLength="200" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtFileName" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
						Display="Dynamic"></asp:RequiredFieldValidator>
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtFileValid" ID="RequiredFieldValidator6" runat="server" ErrorMessage="Not Valid/Unique"
						Display="Dynamic"></asp:RequiredFieldValidator>
					<asp:TextBox runat="server" ValidationGroup="inputForm" ID="txtFileValid" MaxLength="25" Columns="25" Style="display: none;" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					navigation:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtNav" runat="server" Columns="45" MaxLength="200" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtNav" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required"
						Display="Dynamic"></asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					page head:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtHead" runat="server" Columns="45" MaxLength="200" />
					<%--<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtHead" ID="RequiredFieldValidator3" runat="server"
					ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>--%>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					meta keywords:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtKey" MaxLength="1000" Columns="60" Style="width: 425px;"
						Rows="4" TextMode="MultiLine" runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					meta description:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" ID="txtDescription" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="4" TextMode="MultiLine"
						runat="server"></asp:TextBox>
				</td>
			</tr>
		</table>
		<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Create" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		<%-- <input type="button" id="btnCancel" value="Cancel" onclick="location.href='./PageIndex.aspx';" />--%>
		<br />
		<div style="display: none;">
			<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
		</div>
		<script type="text/javascript">
			var saving = 0;

			function SubmitPage() {
				saving = 1;
				CheckFileName();
				setTimeout("ClickBtn();", 1000);
			}
			function ClickBtn() {
				$('#<%=btnSave.ClientID %>').click();
			}
		</script>
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxBodyContentPlaceHolder" runat="server">
</asp:Content>
