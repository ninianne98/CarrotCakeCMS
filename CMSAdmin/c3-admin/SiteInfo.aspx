﻿<%@ Page Title="Site Info" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteInfo.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.SiteInfo" %>

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
			var theEd = $('#<%= txtEditorPath.ClientID %>').val();
			var theDateFldr = $('#<%= txtDatePath.ClientID %>').val();

			$('#<%= txtFoldersValid.ClientID %>').val('');

			var webMthd = webSvc + "/ValidateBlogFolders";

			var myFldr = MakeStringSafe(theFldr);
			var myCat = MakeStringSafe(theCat);
			var myTag = MakeStringSafe(theTag);
			var myEd = MakeStringSafe(theEd);
			var myDateF = MakeStringSafe(theDateFldr);

			$.ajax({
				type: "POST",
				url: webMthd,

				data: JSON.stringify({
					FolderPath: myFldr, DatePath: myDateF,
					CategoryPath: myCat, TagPath: myTag,
					EditorPath: myEd
				}),

				contentType: "application/json; charset=utf-8",
				dataType: "json"
			}).done(editFilenameCallback)
				.fail(cmsAjaxFailed);
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
	Site Info
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<asp:Button ValidationGroup="inputForm2" ID="btnResetVars" runat="server" Text="Refresh Configs" OnClick="btnResetVars_Click" />
		<br />
	</p>
	<asp:PlaceHolder ID="phFeeds" runat="server">
		<fieldset class="fieldset-med">
			<legend>
				<label>
					Feeds
				</label>
			</legend>
			<div>
				<table style="width: 80%;">
					<tr>
						<td class="tablecaption" style="text-align: right; margin-right: 1em;">Rss Blog
						</td>
						<td style="width: 40px; text-align: left;">
							<carrot:RSSFeed runat="server" ID="RSSFeedBlog" RSSFeedType="BlogOnly" LinkTarget="_blank" CssClass="rssimage" RenderRSSMode="ImageLink" />
						</td>
						<td class="tablecaption" style="text-align: right; margin-right: 1em;">Rss Content
						</td>
						<td style="width: 40px; text-align: left;">
							<carrot:RSSFeed runat="server" ID="RSSFeedPage" RSSFeedType="PageOnly" LinkTarget="_blank" CssClass="rssimage" RenderRSSMode="ImageLink" />
						</td>
						<td class="tablecaption" style="text-align: right; margin-right: 1em;">Sitemap
						</td>
						<td style="width: 40px; text-align: left;">
							<a href="<%= SiteFilename.SiteMapUri %>" target="_blank">
								<img src="/c3-admin/images/chart_organisation.png" alt="Sitemap" title="Sitemap" />
							</a>
						</td>
					</tr>
				</table>
			</div>
			<br />
		</fieldset>
		<br />
	</asp:PlaceHolder>

	<fieldset class="fieldset-med">
		<legend>
			<label>
				Site Information
			</label>
		</legend>
		<table style="width: 99%;">
			<tr>
				<td class="tablecaption" style="min-width: 135px; width: 15em;">Site ID
				</td>
				<td>
					<div style="padding: 5px; padding-left: 10px; min-width: 275px; width: 24em;" class="ui-widget-content ui-corner-all">
						<asp:Literal ID="litID" runat="server" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="tablecaption">Site Name
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtSiteName" MaxLength="100" Columns="80" CssClass="form-control-xlg"
						runat="server" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtSiteName" ID="RequiredFieldValidator1"
						runat="server" ErrorMessage="Site Name is required" ToolTip="Site Name is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">Site Tagline
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTagline" MaxLength="512" Columns="80" CssClass="form-control-xlg"
						runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">Site Titlebar Pattern
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTitleBar" MaxLength="512" Columns="80" CssClass="form-control-xlg"
						runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">Site Time Zone
				</td>
				<td>
					<asp:DropDownList ID="ddlTimeZone" runat="server" DataTextField="DisplayName" DataValueField="Id" CssClass="form-control-xlg" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">Site URL
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtURL" MaxLength="100" Columns="80" CssClass="form-control-xlg"
						runat="server" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtURL" ID="RequiredFieldValidator2"
						runat="server" ErrorMessage="Site URL is required" ToolTip="Site URL is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr id="trSiteIndex" runat="server">
				<td class="tablecaption">Index Page
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
				<td class="tablecaption"></td>
				<td class="tablecaption">
					<p>
						Hide from Search Engines
						<asp:CheckBox ID="chkHide" runat="server" />
					</p>
					<asp:PlaceHolder runat="server" ID="phTrackback">
						<p>
							Allow sending trackbacks
						<asp:CheckBox ID="chkSendTrackback" runat="server" />
						</p>
						<p>
							Accept incoming trackbacks
						<asp:CheckBox ID="chkAcceptTrackbacks" runat="server" />
						</p>
					</asp:PlaceHolder>
				</td>
			</tr>
		</table>
	</fieldset>
	<br />
	<fieldset class="fieldset-med">
		<legend>
			<label>
				Meta Data
			</label>
		</legend>
		<table style="width: 99%;">
			<tr>
				<td class="tablecaption" style="min-width: 135px; width: 15em;">Meta Keywords
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" ID="txtKey" MaxLength="1000" Columns="60" CssClass="form-control-xlg" Rows="4" TextMode="MultiLine" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">Meta Description
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" ID="txtDescription" MaxLength="1000" Columns="60" CssClass="form-control-xlg" Rows="4" TextMode="MultiLine"
						runat="server" />
				</td>
			</tr>
		</table>
	</fieldset>
	<br />
	<fieldset class="fieldset-med">
		<legend>
			<label>
				Blog Settings
			</label>
		</legend>
		<table style="width: 99%;">
			<tr>
				<td class="tablecaption" style="min-width: 135px; width: 15em;">&nbsp;
				</td>
				<td>
					<asp:CompareValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtFoldersValid" ID="CompareValidator1"
						runat="server" ErrorMessage="Blog folder parameters are not unique or not provided" ToolTip="Blog folder parameters are not unique or not provided"
						Text="Blog Configuration Errors" Display="Dynamic" ValueToCompare="VALID" Operator="Equal" />
					&nbsp;
				</td>
			</tr>
			<tr>
				<td class="tablecaption">Blog Feature Base Folder
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtFolderPath" MaxLength="48" Columns="60" CssClass="form-control-md"
						runat="server" onblur="CheckFolderPrefixes()" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtFolderPath" ID="RequiredFieldValidator4"
						runat="server" ErrorMessage="Blog Feature Base Folder is required" ToolTip="Blog Feature Base Folder is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">Blog Category Path
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtCategoryPath" MaxLength="48" Columns="60" CssClass="form-control-md"
						runat="server" onblur="CheckFolderPrefixes()" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtCategoryPath" ID="RequiredFieldValidator5"
						runat="server" ErrorMessage="Blog Category Path is required" ToolTip="Blog Category Path is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">Blog Tag Path
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTagPath" MaxLength="48" Columns="60" CssClass="form-control-md"
						runat="server" onblur="CheckFolderPrefixes()" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtTagPath" ID="RequiredFieldValidator6"
						runat="server" ErrorMessage="Blog Tag Path is required" ToolTip="Blog Tag Path is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">Blog Date Path
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtDatePath" MaxLength="48" Columns="60" CssClass="form-control-md"
						runat="server" onblur="CheckFolderPrefixes()" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtDatePath" ID="RequiredFieldValidator7"
						runat="server" ErrorMessage="Blog Date Path is required" ToolTip="Blog Date Path is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">Blog Author Path
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtEditorPath" MaxLength="48" Columns="60" CssClass="form-control-md"
						runat="server" onblur="CheckFolderPrefixes()" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtEditorPath" ID="RequiredFieldValidator3"
						runat="server" ErrorMessage="Blog Author Path is required" ToolTip="Blog Author Path is required" Text="**" Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">Date Pattern
				</td>
				<td>
					<asp:DropDownList ID="ddlDatePattern" runat="server" ValidationGroup="inputForm">
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
	<asp:PlaceHolder ID="phCreatePage" runat="server">
		<br />
		<h2>Create initial homepage on site creation?
			<asp:CheckBox ID="chkHomepage" runat="server" />
		</h2>
		<br />
		<h2>Create initial index (blog results) on site creation?
			<asp:CheckBox ID="chkIndex" runat="server" />
		</h2>
		<br />
	</asp:PlaceHolder>
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
