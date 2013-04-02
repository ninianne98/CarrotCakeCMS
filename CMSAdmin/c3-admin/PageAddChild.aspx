<%@ Page Title="Add Page Info" Language="C#" MasterPageFile="~/c3-admin/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="PageAddChild.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.PageAddChild" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = "/c3-admin/CMS.asmx";

		var thePageID = '<%=Guid.Empty %>';

		var thePage = '';


		function AutoGeneratePageFilename() {
			var theTitle = $('#<%= txtTitle.ClientID %>').val();
			var theFile = $('#<%= txtFileName.ClientID %>').val();
			var theNav = $('#<%= txtNav.ClientID %>').val();

			if (theTitle.length > 0 && theFile.length < 1 && theNav.length < 1) {
				GeneratePageFilename();
			}
		}

		function GeneratePageFilename() {
			var theTitle = $('#<%= txtTitle.ClientID %>').val();
			var theFile = $('#<%= txtFileName.ClientID %>').val();
			var sGoLiveDate = '<%=DateTime.Now.ToShortDateString() %>';

			if (theTitle.length > 0) {

				var webMthd = webSvc + "/GenerateNewFilename";
				var myPageTitle = MakeStringSafe(theTitle);

				$.ajax({
					type: "POST",
					url: webMthd,
					data: "{'ThePageTitle': '" + myPageTitle + "', 'GoLiveDate': '" + sGoLiveDate + "', 'PageID': '" + thePageID + "', 'Mode': 'page'}",
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: ajaxGeneratePageFilename,
					error: cmsAjaxFailed
				});
			} else {
				cmsAlertModal("Cannot create a filename with there is no title value assigned.");
			}
		}

		function ajaxGeneratePageFilename(data, status) {
			debugger;
			if (data.d == "FAIL") {
				cmsAlertModal(data.d);
			} else {
				var theTitle = $('#<%= txtTitle.ClientID %>').val();
				var theFile = $('#<%= txtFileName.ClientID %>').val();
				var theNav = $('#<%= txtNav.ClientID %>').val();
				var theHead = $('#<%= txtHead.ClientID %>').val();

				if (theFile.length < 3) {
					$('#<%= txtFileName.ClientID %>').val(data.d);
				}
				if (theNav.length < 1) {
					$('#<%= txtNav.ClientID %>').val(theTitle);
				}
				if (theHead.length < 1) {
					$('#<%= txtHead.ClientID %>').val(theTitle);
				}
			}
			CheckFileName();
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
			This creates a blank and unpublished page (no content) with the specified filename and title etc. This new page will use the plain/default template. You
			will have the opportunity to update the appearance when you visit the new page.
		</p>
		<table style="width: 700px;">
			<tr>
				<td class="tablecaption">
					title bar:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="AutoGeneratePageFilename()" ID="txtTitle" runat="server" Columns="45"
						MaxLength="200" />
					<a href="javascript:void(0)" onclick="GeneratePageFilename()" class="lnkPopup">
						<img class="imgNoBorder" src="images/page_white_wrench.png" title="Generate Filename and other Title fields" alt="Generate Filename and other Title fields" /></a>&nbsp;
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtTitle" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
						Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					filename:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="CheckFileName()" ID="txtFileName" runat="server" Columns="45"
						MaxLength="200" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtFileName" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
						Display="Dynamic" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtFileValid" ID="RequiredFieldValidator6" runat="server" ErrorMessage="Not Valid/Unique"
						Display="Dynamic" />
					<asp:TextBox runat="server" ValidationGroup="inputForm" ID="txtFileValid" MaxLength="25" Columns="25" Style="display: none;" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					navigation:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtNav" runat="server" Columns="45" MaxLength="200" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtNav" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required"
						Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					page head:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtHead" runat="server" Columns="45" MaxLength="200" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					meta keywords:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtKey" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="4"
						TextMode="MultiLine" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					meta description:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" ID="txtDescription" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="4" TextMode="MultiLine" runat="server" />
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

			function SubmitPage() {
				CheckFileName();
				setTimeout("ClickBtn();", 1200);
			}
			function ClickBtn() {
				$('#<%=btnSave.ClientID %>').click();
			}
		</script>
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
