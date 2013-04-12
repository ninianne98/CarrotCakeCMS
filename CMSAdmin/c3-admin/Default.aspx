<%@ Page Title="Dashboard" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.Default" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucSitePageDrillDown.ascx" TagName="ucSitePageDrillDown" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = cmsGetServiceAddress();

		$(document).ready(function () {
			setTimeout("CheckFolderPrefixes();", 250);
			cmsIsPageValid();
		});

		function CheckFolderPrefixes() {
			var theFldr = $('#<%= txtFolderPath.ClientID %>').val();
			var theCat = $('#<%= txtCategoryPath.ClientID %>').val();
			var theTag = $('#<%= txtTagPath.ClientID %>').val();
			var theDateFldr = $('#<%= txtDatePath.ClientID %>').val();

			$('#<%= txtFoldersValid.ClientID %>').val('');

			var webMthd = webSvc + "/ValidateBlogFolders";

			var myFldr = MakeStringSafe(theFldr);
			var myCat = MakeStringSafe(theCat);
			var myTag = MakeStringSafe(theTag);
			var myDateF = MakeStringSafe(theDateFldr);

			$.ajax({
				type: "POST",
				url: webMthd,
				data: "{'FolderPath': '" + myFldr + "', 'DatePath': '" + myDateF + "', 'CategoryPath': '" + myCat + "', 'TagPath': '" + myTag + "'}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: editFilenameCallback,
				error: cmsAjaxFailed
			});
		}

		function editFilenameCallback(data, status) {
			if (data.d != "FAIL" && data.d != "OK") {
				cmsAlertModal(data.d);
			}

			var fldrValid = '#<%= txtFoldersValid.ClientID %>';

			if (data.d == "OK") {
				$(fldrValid).val('VALID');
			} else {
				$(fldrValid).val('NOT VALID');
			}

			//var ret = cmsIsPageValid();
			cmsForceInputValidation('<%= txtFoldersValid.ClientID %>');
		}

		function ExportContent() {
			window.open('<%=SiteFilename.DataExportURL %>');
		}

		setTimeout("cmsSendTrackbackBatch();", 1500);
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site Dashboard
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<asp:Button ValidationGroup="inputForm2" ID="btnResetVars" runat="server" Text="Refresh Configs" OnClick="btnResetVars_Click" />
		<%--&nbsp;&nbsp;&nbsp;
		<input type="button" runat="server" id="btnExport" value="Export Site" onclick="ExportContent();" /> --%>
		<br />
	</p>
	<fieldset style="width: 675px;">
		<legend>
			<label>
				Site Information
			</label>
		</legend>
		<table style="width: 99%;">
			<tr>
				<td class="tablecaption" style="width: 160px;">
					Site ID
				</td>
				<td>
					<div style="padding: 5px; padding-left: 10px; width: 275px;" class=" ui-widget-content ui-corner-all ">
						<asp:Literal ID="litID" runat="server" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Site Name
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtSiteName" MaxLength="100" Columns="80" Style="width: 425px;"
						runat="server" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtSiteName" ID="RequiredFieldValidator1"
						runat="server" ErrorMessage="Site Name is required" ToolTip="Site Name is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Site Tagline
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTagline" MaxLength="512" Columns="80" Style="width: 425px;"
						runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Site Titlebar Pattern
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTitleBar" MaxLength="512" Columns="80" Style="width: 425px;"
						runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Site Time Zone
				</td>
				<td>
					<asp:DropDownList ID="ddlTimeZone" runat="server" DataTextField="DisplayName" DataValueField="Id" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Site URL
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtURL" MaxLength="100" Columns="80" Style="width: 425px;" runat="server" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtURL" ID="RequiredFieldValidator2"
						runat="server" ErrorMessage="Site URL is required" ToolTip="Site URL is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr id="trSiteIndex" runat="server">
				<td class="tablecaption">
					Index Page
					<br />
				</td>
				<td>
					<!-- parent page plugin-->
					<uc1:ucSitePageDrillDown ID="ParentPagePicker" runat="server" />
					<div style="clear: both; height: 2px;">
					</div>
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
				</td>
				<td class="tablecaption">
					<p>
						Hide from Search Engines
						<asp:CheckBox ID="chkHide" runat="server" />
					</p>
					<p>
						Allow sending trackbacks
						<asp:CheckBox ID="chkSendTrackback" runat="server" />
					<p>
						Accept incoming trackbacks
						<asp:CheckBox ID="chkAcceptTrackbacks" runat="server" />
					</p>
				</td>
			</tr>
		</table>
	</fieldset>
	<fieldset style="width: 675px;">
		<legend>
			<label>
				Meta Data
			</label>
		</legend>
		<table style="width: 99%;">
			<tr>
				<td class="tablecaption" style="width: 175px;">
					Meta Keywords
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" ID="txtKey" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="4" TextMode="MultiLine" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Meta Description
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" ID="txtDescription" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="4" TextMode="MultiLine" runat="server" />
				</td>
			</tr>
		</table>
	</fieldset>
	<fieldset style="width: 675px;">
		<legend>
			<label>
				Blog Settings
			</label>
		</legend>
		<table style="width: 99%;">
			<tr>
				<td class="tablecaption" style="width: 180px;">
					&nbsp;
				</td>
				<td>
					<asp:CompareValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtFoldersValid" ID="CompareValidator1" runat="server"
						ErrorMessage="Blog folder parameters are not unique or not provided" ToolTip="Blog folder parameters are not unique or not provided" Text="Blog Configuration Errors"
						Display="Dynamic" ValueToCompare="VALID" Operator="Equal" />
					&nbsp;
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Blog Feature Base Folder
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtFolderPath" MaxLength="48" Columns="60" Style="width: 350px;"
						runat="server" onblur="CheckFolderPrefixes()" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtFolderPath" ID="RequiredFieldValidator4"
						runat="server" ErrorMessage="Blog Feature Base Folder is required" ToolTip="Blog Feature Base Folder is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Blog Category Path
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtCategoryPath" MaxLength="48" Columns="60" Style="width: 350px;"
						runat="server" onblur="CheckFolderPrefixes()" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtCategoryPath" ID="RequiredFieldValidator5"
						runat="server" ErrorMessage="Blog Category Path is required" ToolTip="Blog Category Path is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Blog Tag Path
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTagPath" MaxLength="48" Columns="60" Style="width: 350px;" runat="server"
						onblur="CheckFolderPrefixes()" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtTagPath" ID="RequiredFieldValidator6"
						runat="server" ErrorMessage="Blog Tag Path is required" ToolTip="Blog Tag Path is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Blog Date Path
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtDatePath" MaxLength="48" Columns="60" Style="width: 350px;"
						runat="server" onblur="CheckFolderPrefixes()" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtDatePath" ID="RequiredFieldValidator7"
						runat="server" ErrorMessage="Blog Date Path is required" ToolTip="Blog Date Path is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Date Pattern
				</td>
				<td>
					<asp:DropDownList ID="ddlDatePattern" runat="server" ValidationGroup="inputForm">
						<%--<asp:ListItem Value="/" Text="No Date" />--%>
						<asp:ListItem Value="yyyy/MM/dd" Text="YYYY/MM/DD" />
						<asp:ListItem Value="yyyy/M/d" Text="YYYY/M/D" />
						<asp:ListItem Value="yyyy/MM" Text="YYYY/MM" />
						<asp:ListItem Value="yyyy/MMMM" Text="YYYY/MonthName" />
						<asp:ListItem Value="yyyy" Text="YYYY" />
					</asp:DropDownList>
				</td>
			</tr>
		</table>
		<div style="display: none;">
			<asp:TextBox runat="server" ValidationGroup="inputForm" ID="txtFoldersValid" MaxLength="25" Columns="25" />
		</div>
	</fieldset>
	<div style="display: none;">
		<asp:ValidationSummary ID="formValidationSummary" runat="server" ShowSummary="true" ValidationGroup="inputForm" />
	</div>
	<br />
	<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" Text="Apply Changes" OnClick="btnSave_Click" OnClientClick="return ClickApplyBtn()" />
	<br />
	<script type="text/javascript">
		function ClickApplyBtn() {
			CheckFolderPrefixes();

			if (cmsIsPageValid()) {
				return true;
			} else {
				cmsLoadPrettyValidationPopup('<%= formValidationSummary.ClientID %>');
				return false;
			}
		}
	</script>
</asp:Content>
