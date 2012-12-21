<%@ Page Title="Dashboard" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.Default" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucSitePageDrillDown.ascx" TagName="ucSitePageDrillDown" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = "/Manage/CMS.asmx";


		$(document).ready(function () {
			CheckFolderPrefixes();
		});

		function CheckFolderPrefixes() {
			var theFldr = $('#<%= txtFolderPath.ClientID %>').val();
			var theCat = $('#<%= txtCategoryPath.ClientID %>').val();
			var theTag = $('#<%= txtTagPath.ClientID %>').val();

			$('#<%= txtFoldersValid.ClientID %>').val('');

			var webMthd = webSvc + "/ValidateBlogFolders";

			var myFldr = MakeStringSafe(theFldr);
			var myCat = MakeStringSafe(theCat);
			var myTag = MakeStringSafe(theTag);

			$.ajax({
				type: "POST",
				url: webMthd,
				data: "{'FolderPath': '" + myFldr + "', 'CategoryPath': '" + myCat + "', 'TagPath': '" + myTag + "'}",
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

			if (data.d == "OK") {
				$('#<%= txtFoldersValid.ClientID %>').val('VALID');
			} else {
				$('#<%= txtFoldersValid.ClientID %>').val('');
			}
			Page_ClientValidate();
		}

		function ExportContent() {
			window.open('<%=Carrotware.CMS.UI.Admin.SiteFilename.DataExportURL %>');
		}

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site Dashboard
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<asp:Button ValidationGroup="inputForm2" ID="btnResetVars" runat="server" Text="Refresh Configs" OnClick="btnResetVars_Click" />
		&nbsp;&nbsp;&nbsp;
		<input type="button" runat="server" id="btnExport" value="Export Site" onclick="ExportContent();" />
		<br />
	</p>
	<fieldset style="width: 650px;">
		<legend>
			<label>
				Site Information
			</label>
		</legend>
		<table style="width: 99%;">
			<tr>
				<td valign="top" class="tablecaption" style="width: 175px;">
					Site ID
				</td>
				<td valign="top">
					<div style="padding: 5px; padding-left: 10px; width: 275px;" class=" ui-widget-content ui-corner-all ">
						<asp:Literal ID="litID" runat="server" />
					</div>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Site Name
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtSiteName" MaxLength="100" Columns="80" Style="width: 425px;"
						runat="server" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtSiteName" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
						Display="Dynamic"></asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Site Tagline
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTagline" MaxLength="512" Columns="80" Style="width: 425px;"
						runat="server" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Site Titlebar Pattern
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTitleBar" MaxLength="512" Columns="80" Style="width: 425px;"
						runat="server" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Site Time Zone
				</td>
				<td valign="top">
					<asp:DropDownList ID="ddlTimeZone" runat="server" DataTextField="DisplayName" DataValueField="Id">
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Site URL
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtURL" MaxLength="100" Columns="80" Style="width: 425px;" runat="server" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtURL" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
						Display="Dynamic"></asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Hide from Search Engines
				</td>
				<td valign="top">
					<asp:CheckBox ID="chkHide" runat="server" />
				</td>
			</tr>
		</table>
	</fieldset>
	<fieldset style="width: 650px;">
		<legend>
			<label>
				Meta Data
			</label>
		</legend>
		<table style="width: 99%;">
			<tr>
				<td valign="top" class="tablecaption" style="width: 175px;">
					Meta Keywords
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" ID="txtKey" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="4" TextMode="MultiLine" runat="server" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Meta Description
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" ID="txtDescription" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="4" TextMode="MultiLine" runat="server" />
				</td>
			</tr>
		</table>
	</fieldset>
	<fieldset style="width: 650px;">
		<legend>
			<label>
				Blog Settings
			</label>
		</legend>
		<table style="width: 99%;">
			<tr>
				<td valign="top" class="tablecaption" style="width: 150px;">
					&nbsp;
				</td>
				<td valign="top">
					<asp:TextBox runat="server" ValidationGroup="inputForm" ID="txtFoldersValid" MaxLength="25" Columns="25" Style="display: none;" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtFoldersValid" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Blog folder parameters are not unique or not provided"
						Display="Dynamic"></asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Blog Feature Folder
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtFolderPath" MaxLength="48" Columns="80" Style="width: 425px;"
						runat="server" onblur="CheckFolderPrefixes()" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Blog Category Path
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtCategoryPath" MaxLength="48" Columns="80" Style="width: 425px;"
						runat="server" onblur="CheckFolderPrefixes()" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Blog Tag Path
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTagPath" MaxLength="48" Columns="80" Style="width: 425px;" runat="server"
						onblur="CheckFolderPrefixes()" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					Date Pattern
				</td>
				<td valign="top">
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
			<tr>
				<td valign="top" class="tablecaption">
					Blog Index Page
					<br />
				</td>
				<td valign="top">
					<!-- parent page plugin-->
					<uc1:ucSitePageDrillDown ID="ParentPagePicker" runat="server" />
					<div style="clear: both; height: 2px;">
					</div>
				</td>
			</tr>
		</table>
	</fieldset>
	<br />
	<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" Text="Apply Changes" OnClick="btnSave_Click" />
	<br />
</asp:Content>
