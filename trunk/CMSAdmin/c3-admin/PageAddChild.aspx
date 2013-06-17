<%@ Page Title="Add Page Info" Language="C#" MasterPageFile="~/c3-admin/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="PageAddChild.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.PageAddChild" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = cmsGetServiceAddress();

		var thePageID = '<%= Guid.Empty %>';

		var tTitle = '#<%= txtTitle.ClientID %>';
		var tNav = '#<%= txtNav.ClientID %>';
		var tHead = '#<%= txtHead.ClientID %>';

		var thePage = '';

		function AutoGeneratePageFilename() {
			var theTitle = $(tTitle).val();
			var theFile = $(tValidFile).val();
			var theNav = $(tNav).val();

			if (theTitle.length > 0 && theFile.length < 1 && theNav.length < 1) {
				GeneratePageFilename();
			}
		}

		function GeneratePageFilename() {
			var theTitle = $(tTitle).val();
			var theFile = $(tValidFile).val();
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
				cmsAlertModalSmall("Cannot create a filename with there is no title value assigned.");
			}
		}

		function ajaxGeneratePageFilename(data, status) {
			//debugger;
			if (data.d == "FAIL") {
				cmsAlertModal(data.d);
			} else {
				var theTitle = $(tTitle).val();
				var theFile = $(tValidFile).val();
				var theNav = $(tNav).val();
				var theHead = $(tHead).val();

				if (theFile.length < 3) {
					$(tValidFile).val(data.d);
				}
				if (theNav.length < 1) {
					$(tNav).val(theTitle);
				}
				if (theHead.length < 1) {
					$(tHead).val(theTitle);
				}
			}
			CheckFileName();
		}

		var tValid = '#<%= txtFileValid.ClientID %>';
		var tValidFile = '#<%= txtFileName.ClientID %>';

		function CheckFileName() {
			thePage = $(tValidFile).val();

			$(tValid).val('');

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
			setTimeout("CheckFileName();", 250);
			cmsIsPageValid();
		});


		function editFilenameCallback(data, status) {
			if (data.d != "FAIL" && data.d != "OK") {
				cmsAlertModal(data.d);
			}

			if (data.d == "OK") {
				$(tValid).val('VALID');
				$(tValidFile).removeClass('validationExclaimBox');
			} else {
				$(tValid).val('NOT VALID');
				$(tValidFile).addClass('validationExclaimBox');
			}

			var ret = cmsIsPageValid();
			setTimeout("cmsForceInputValidation('<%= txtFileValid.ClientID %>');", 800);
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
		<h3>
			<asp:HyperLink ID="lnkCreatePage" runat="server">
				<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add" />
			Create Another Page</asp:HyperLink>
		</h3>
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlAdd">
		<p>
			This creates a blank and unpublished page (no content) with the specified filename and title etc. This new page will use the plain/default template. You
			will have the opportunity to update the appearance when you visit the new page.
		</p>
		<table style="width: 700px;">
			<tr>
				<td style="width: 125px;" class="tablecaption">
					titlebar:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="AutoGeneratePageFilename()" ID="txtTitle" runat="server" Columns="45"
						MaxLength="200" />
					<a href="javascript:void(0)" onclick="GeneratePageFilename()" class="lnkPopup">
						<img class="imgNoBorder" src="images/page_white_wrench.png" title="Generate Filename and other Title fields" alt="Generate Filename and other Title fields" /></a>&nbsp;
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtTitle" ID="RequiredFieldValidator1"
						runat="server" ErrorMessage="Titlebar is required" ToolTip="Titlebar is required" Display="Dynamic" Text="**" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					filename:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="CheckFileName()" ID="txtFileName" runat="server" Columns="45"
						MaxLength="200" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtFileName" ID="RequiredFieldValidator2"
						runat="server" ErrorMessage="Filename is required" ToolTip="Filename is required" Display="Dynamic" Text="**" />
					<asp:CompareValidator ValidationGroup="inputForm" CssClass="validationExclaim" ForeColor="" ControlToValidate="txtFileValid" ID="CompareValidator1" runat="server"
						ErrorMessage="Filename is not valid/not unique" ToolTip="Filename is not valid/not unique" Text="##" Display="Dynamic" ValueToCompare="VALID" Operator="Equal" />
					<div style="display: none;">
						<asp:TextBox runat="server" ValidationGroup="inputForm" ID="txtFileValid" MaxLength="25" Columns="25" />
						<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtFileValid" ID="RequiredFieldValidator3"
							runat="server" ErrorMessage="Filename validation is required" ToolTip="Filename validation is required" Display="Dynamic" Text="**" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					navigation:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtNav" runat="server" Columns="45" MaxLength="200" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtNav" ID="RequiredFieldValidator4"
						runat="server" ErrorMessage="Navigation text is required" ToolTip="Navigation text is required" Display="Dynamic" Text="**" />
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
		<div style="display: none;">
			<asp:ValidationSummary ID="formValidationSummary" runat="server" ShowSummary="true" ValidationGroup="inputForm" />
		</div>
		<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="return SubmitPage()" Text="Apply" />
		<br />
		<div style="display: none;">
			<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Apply" />
		</div>
		<script type="text/javascript">
			function SubmitPage() {
				var ret = cmsIsPageValid();
				setTimeout("ClickSaveBtn();", 800);
				return ret;
			}

			function ClickSaveBtn() {
				if (cmsIsPageValid()) {
					$('#<%=btnSave.ClientID %>').click();
				}
				cmsLoadPrettyValidationPopup('<%= formValidationSummary.ClientID %>');
				return true;
			}
		</script>
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
