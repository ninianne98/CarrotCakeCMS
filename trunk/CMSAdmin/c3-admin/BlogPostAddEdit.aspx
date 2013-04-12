<%@ Page ValidateRequest="false" Title="BlogPostAddEdit" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="BlogPostAddEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.BlogPostAddEdit" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = cmsGetServiceAddress();

		var thePageID = '<%=guidRootContentID.ToString() %>';

		var thePage = '';

		function exportPage() {
			window.open("<%=SiteFilename.DataExportURL %>?id=" + thePageID);
		}

		function GenerateBlogFilePrefix() {
			var sGoLiveDate = $('#<%= txtReleaseDate.ClientID %>').val();
			var pageTitle = $('#<%= txtPageSlug.ClientID %>').val();
			var myPage = MakeStringSafe(pageTitle);

			var webMthd = webSvc + "/GenerateBlogFilePrefix";

			$.ajax({
				type: "POST",
				url: webMthd,
				data: "{'ThePageSlug': '" + myPage + "', 'GoLiveDate': '" + sGoLiveDate + "'}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: editPrefixCallback,
				error: cmsAjaxFailed
			});

		}

		setTimeout("cmsSendTrackbackPageBatch('" + thePageID + "');", 1500);

		function editPrefixCallback(data, status) {
			if (data.d != "FAIL") {

				$('#<%= lblPrefix.ClientID %>').html(data.d);
			} else {
				$('#<%= lblPrefix.ClientID %>').html('/0000/00/00/file.aspx');
			}
		}

		var fldrValid = '#<%= txtFileValid.ClientID %>';

		function CheckFileName() {
			GenerateBlogFilePrefix();

			thePage = $('#<%= txtPageSlug.ClientID %>').val();

			$(fldrValid).val('');

			var sGoLiveDate = $('#<%= txtReleaseDate.ClientID %>').val();

			var webMthd = webSvc + "/ValidateUniqueBlogFilename";
			var myPage = MakeStringSafe(thePage);

			$.ajax({
				type: "POST",
				url: webMthd,
				data: "{'ThePageSlug': '" + myPage + "', 'GoLiveDate': '" + sGoLiveDate + "', 'PageID': '" + thePageID + "'}",
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
				$(fldrValid).val('VALID');
			} else {
				$(fldrValid).val('NOT VALID');
			}

			//var ret = cmsIsPageValid();
			setTimeout("cmsForceInputValidation('<%= txtFileValid.ClientID %>');", 500);
		}

		function AutoGeneratePageFilename() {
			var theTitle = $('#<%= txtTitle.ClientID %>').val();
			var theFile = $('#<%= txtPageSlug.ClientID %>').val();
			var theNav = $('#<%= txtNav.ClientID %>').val();

			if (theTitle.length > 0 && theFile.length < 1 && theNav.length < 1) {
				GeneratePageFilename();
			}
		}

		function GeneratePageFilename() {
			var theTitle = $('#<%= txtTitle.ClientID %>').val();
			var theFile = $('#<%= txtPageSlug.ClientID %>').val();
			var sGoLiveDate = $('#<%= txtReleaseDate.ClientID %>').val();

			if (theTitle.length > 0) {

				var webMthd = webSvc + "/GenerateNewFilename";
				var myPageTitle = MakeStringSafe(theTitle);

				$.ajax({
					type: "POST",
					url: webMthd,
					data: "{'ThePageTitle': '" + myPageTitle + "', 'GoLiveDate': '" + sGoLiveDate + "', 'PageID': '" + thePageID + "', 'Mode': 'blog'}",
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
			if (data.d == "FAIL") {
				cmsAlertModal(data.d);
			} else {
				var theTitle = $('#<%= txtTitle.ClientID %>').val();
				var theFile = $('#<%= txtPageSlug.ClientID %>').val();
				var theNav = $('#<%= txtNav.ClientID %>').val();
				var theHead = $('#<%= txtHead.ClientID %>').val();

				if (theFile.length < 3) {
					$('#<%= txtPageSlug.ClientID %>').val(data.d);
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

		/*  <asp:PlaceHolder  ID="pnlHBEmpty" runat="server"> begin empty cancel functions */

		function EditHB() { }

		function cancelEditing() { }

		function cmsRecordCancellation() { }


		function openPage() {
			var theURL = $('#<%= txtOldFile.ClientID %>').val();

			if (theURL.length > 3) {
				window.setTimeout("location.href = '" + theURL + "';", 250);
			} else {
				cmsAlertModalSmall("No saved post to show.");
			}
		}

		/*  </asp:PlaceHolder> */

		/* <asp:PlaceHolder ID="pnlHB" runat="server"> begin EditHB */

		function openPage() {
			var theURL = $('#<%= txtOldFile.ClientID %>').val();
			cmsOpenPage(theURL);
		}

		function EditHB() {
			setTimeout("EditHB();", 30 * 1000);

			var webMthd = webSvc + "/RecordHeartbeat";

			$.ajax({
				type: "POST",
				url: webMthd,
				data: "{'PageID': '" + thePageID + "'}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: updateHeartbeat,
				error: cmsAjaxFailed
			});
		}

		function updateHeartbeat(data, status) {
			var hb = $('#cmsHeartBeat');
			hb.empty().append('HB:  ');
			hb.append(data.d);
		}

		$(document).ready(function () {
			setTimeout("EditHB();", 500);
		});

		function cancelEditing() {

			$("#divCMSCancelWinMsg").text('Are you sure you want to leave the editor? All changes will be lost!');

			$("#divCMSCancelWin").dialog({
				open: function () {
					$(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
				},

				resizable: false,
				height: 350,
				width: 450,
				modal: true,
				buttons: {
					"No": function () {
						$(this).dialog("close");
					},
					"Yes": function () {
						cmsMakeOKToLeave();
						cmsRecordCancellation();
						window.setTimeout("location.href = '<%=SiteFilename.BlogPostIndexURL %>';", 800);
						$(this).dialog("close");
					}
				}
			});
		}

		function cmsRecordCancellation() {

			if (thePageID != '<%=Guid.Empty %>') {

				var webMthd = webSvc + "/CancelEditing";

				$.ajax({
					type: "POST",
					url: webMthd,
					data: "{'ThisPage': '" + thePageID + "'}",
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: cmsAjaxGeneralCallback,
					error: cmsAjaxFailed
				});
			}
		}


		/* </asp:placeholder> */

	</script>
	<script type="text/javascript">

		var cmsIsPageLocked = true;
		if ('<%=bLocked.ToString().ToLower() %>' != 'true') {
			cmsIsPageLocked = false;
		}

		$(window).bind('beforeunload', function () {
			//cmsConfirmLeavingPage = false;
			if (!cmsIsPageLocked) {
				if (cmsGetPageStatus()) {
					return '>>Are you sure you want to navigate away<<';
				}
			}
		});


		$(document).ready(function () {
			if (!cmsIsPageLocked) {
				// these click events because of stoopid IE9 navigate away behavior
				$('#menu a.lnkPopup').each(function (i) {
					$(this).click(function () {
						cmsMakeOKToLeave();
						setTimeout("cmsMakeNotOKToLeave();", 500);
					});
				});

				$('#PageContents a').each(function (i) {
					$(this).click(function () {
						cmsMakeOKToLeave();
						setTimeout("cmsMakeNotOKToLeave();", 500);
					});
				});
			}
		});

		var cmsTemplateDDL = '#<%=ddlTemplate.ClientID%>';
		var cmsTemplatePreview = '<%=SiteData.PreviewTemplateFilePage %>';

		function cmsPreviewTemplate() {
			var tmpl = $(cmsTemplateDDL).val();

			tmpl = MakeStringSafe(tmpl);

			ShowWindowNoRefresh(cmsTemplatePreview + "?carrot_templatepreview=" + tmpl);
		}

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Blog Post Add/Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<br />
	<div class="ui-widget" runat="server" id="divEditing">
		<div class="ui-state-highlight ui-corner-all" style="padding: 5px; margin-top: 5px; margin-bottom: 5px; width: 500px;">
			<p>
				<span class="ui-icon ui-icon-info" style="float: left; margin: 3px;"></span>
				<asp:Literal ID="litUser" runat="server">&nbsp;</asp:Literal></p>
		</div>
	</div>
	<div id="PageContents">
		<table style="width: 700px;">
			<tr>
				<td style="width: 125px;" class="tablecaption">
					last updated:
				</td>
				<td style="width: 575px;">
					<asp:Label ID="lblUpdated" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					create date:
				</td>
				<td>
					<asp:Label ID="lblCreateDate" runat="server" /><br />
					<br />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					release date:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtReleaseDate" runat="server" CssClass="dateRegion" Columns="16"
						onblur="CheckFileName()" onchange="CheckFileName();" />
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtReleaseTime" runat="server" CssClass="timeRegion" Columns="10"
						onblur="CheckFileName()" onchange="CheckFileName();" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					retire date:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtRetireDate" runat="server" CssClass="dateRegion" Columns="16" />
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtRetireTime" runat="server" CssClass="timeRegion" Columns="10" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
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
					<br />
				</td>
				<td>
					<div style="padding: 3px;" class=" ui-widget-content ui-corner-all ">
						<asp:Label ID="lblPrefix" runat="server" Text="/yy/mm/dd/" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					filename:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="CheckFileName()" ID="txtPageSlug" runat="server" Columns="45"
						MaxLength="200" />
					<a href="javascript:void(0)" onclick="openPage();">
						<img class="imgNoBorder" src="images/html2.png" title="Visit page" alt="Visit page" /></a>&nbsp;
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtPageSlug" ID="RequiredFieldValidator2"
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
					thumbnail:
					<br />
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtThumb" runat="server" Columns="45" MaxLength="200" />
					<input type="button" id="Button1" value="Browse" onclick="cmsFileBrowserOpenReturn('<%=txtThumb.ClientID %>');return false;" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					&nbsp;
				</td>
				<td>
					<asp:CheckBox ID="chkActive" runat="server" Text="Show publicly" />
					&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkHide" runat="server" Text="Hide from Search Engines" />
				</td>
			</tr>
			<tr>
				<td style="width: 125px;" class="tablecaption">
					meta keywords:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" ID="txtKey" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="4" TextMode="MultiLine" runat="server" />
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
		<table style="width: 700px;">
			<tr>
				<td style="width: 350px;">
					<div style="width: 340px; clear: both;">
						<div class="picker-area ui-widget-header ui-state-default ui-corner-top" style="width: 340px">
							categories</div>
						<div class="picker-area ui-widget-content ui-corner-bottom scroll-container" style="width: 340px">
							<div class="scroll-area">
								<asp:Repeater ID="rpCat" runat="server">
									<ItemTemplate>
										<asp:CheckBox runat="server" ID="chk" Text='<%#Eval("CategoryText") %>' value='<%#Eval("ContentCategoryID") %>' /><br />
									</ItemTemplate>
								</asp:Repeater>
							</div>
						</div>
					</div>
					<div style="clear: both;">
					</div>
				</td>
				<td>
					&nbsp;&nbsp;
				</td>
				<td style="width: 350px;">
					<div style="width: 340px; clear: both;">
						<div class="picker-area ui-widget-header ui-state-default ui-corner-top" style="width: 340px">
							tags</div>
						<div class="picker-area ui-widget-content ui-corner-bottom scroll-container" style="width: 340px">
							<div class="scroll-area">
								<asp:Repeater ID="rpTag" runat="server">
									<ItemTemplate>
										<asp:CheckBox runat="server" ID="chk" Text='<%#Eval("TagText") %>' value='<%#Eval("ContentTagID") %>' /><br />
									</ItemTemplate>
								</asp:Repeater>
							</div>
						</div>
					</div>
					<div style="clear: both;">
					</div>
				</td>
			</tr>
		</table>
		<table style="width: 700px;">
			<tr>
				<td class="tablecaption">
					template:
				</td>
				<td>
					<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplate" runat="server" />
					&nbsp;&nbsp;&nbsp;&nbsp;
					<input type="button" onclick="cmsPreviewTemplate()" value="Preview" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
				</td>
				<td align="right">
					<input type="button" id="btnBrowseSvr" value="Browse Server Files" onclick="cmsFileBrowserOpen('not-a-real-file');" />
				</td>
			</tr>
		</table>
		<br />
		<div id="jqtabs" style="height: 400px; width: 825px; margin-bottom: 10px;">
			<ul>
				<li><a href="#pagecontent-tabs-0">Left</a></li>
				<li><a href="#pagecontent-tabs-1">Center</a></li>
				<li><a href="#pagecontent-tabs-3">Right</a></li>
				<li><a href="#pagecontent-tabs-4">Trackback URLs</a></li>
			</ul>
			<div style="margin-bottom: 25px; height: 380px; width: 800px;">
				<div id="pagecontent-tabs-0">
					<div style="margin-bottom: 25px;">
						<div runat="server" id="divLeft">
							body (left)<br />
							<a href="javascript:cmsToggleTinyMCE('<%= reLeftBody.ClientID %>');">Show/Hide Editor</a></div>
						<asp:TextBox Style="height: 280px; width: 780px;" CssClass="mceEditor" ID="reLeftBody" runat="server" TextMode="MultiLine" Rows="15" Columns="80" />
						<br />
					</div>
				</div>
				<div id="pagecontent-tabs-1">
					<div style="margin-bottom: 25px;">
						<div runat="server" id="divCenter">
							body (main/center)<br />
							<a href="javascript:cmsToggleTinyMCE('<%= reBody.ClientID %>');">Show/Hide Editor</a></div>
						<asp:TextBox Style="height: 280px; width: 780px;" CssClass="mceEditor" ID="reBody" runat="server" TextMode="MultiLine" Rows="15" Columns="80" />
						<br />
					</div>
				</div>
				<div id="pagecontent-tabs-3">
					<div style="margin-bottom: 25px;">
						<div runat="server" id="divRight">
							body (right)<br />
							<a href="javascript:cmsToggleTinyMCE('<%= reRightBody.ClientID %>');">Show/Hide Editor</a></div>
						<asp:TextBox Style="height: 280px; width: 780px;" CssClass="mceEditor" ID="reRightBody" runat="server" TextMode="MultiLine" Rows="15" Columns="80" />
						<br />
					</div>
				</div>
				<div id="pagecontent-tabs-4">
					<div style="margin-bottom: 25px;">
						<div runat="server" id="divTrackback">
							new trackbacks, one per line<br />
						</div>
						<asp:TextBox Style="height: 125px; width: 780px;" CssClass="mceEditorNone" ID="txtTrackback" runat="server" TextMode="MultiLine" Rows="8" Columns="80" />
						<div class="scroll-container" style="height: 175px; width: 780px;">
							<div class="scroll-area" style="height: 170px; width: 775px;">
								<carrot:CarrotGridView CssClass="datatable" DefaultSort="ModifiedDate desc" ID="gvTracks" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
									AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
									<Columns>
										<asp:BoundField HeaderText="Trackback URL" DataField="TrackBackURL" />
										<carrot:CarrotHeaderSortTemplateField HeaderText="Last Modified" DataField="ModifiedDate" DataFieldFormat="{0:MM/dd/yy h:mm tt}" />
										<carrot:CarrotHeaderSortTemplateField HeaderText="Created On" DataField="CreateDate" DataFieldFormat="{0:MM/dd/yy h:mm tt}" />
										<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="TrackedBack" HeaderText="Status" AlternateTextFalse="Not Tracked" AlternateTextTrue="Tracked"
											ShowBooleanImage="true" />
									</Columns>
								</carrot:CarrotGridView>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<table style="width: 800px;">
			<tr>
				<td align="left">
					<div id="cmsHeartBeat" style="clear: both; padding: 2px; margin: 2px; min-height: 22px;">
						&nbsp;</div>
				</td>
				<td>
					&nbsp;&nbsp;
				</td>
				<td align="right">
					<%--<a target="_blank" id="lnkExport" runat="server" href="javascript:void(0);" onclick="exportPage();">Export latest version of this page</a>--%>
				</td>
			</tr>
		</table>
		<div style="display: none;">
			<asp:ValidationSummary ID="formValidationSummary" runat="server" ShowSummary="true" ValidationGroup="inputForm" />
		</div>
		<asp:Panel ID="pnlButtons" runat="server">
			<table style="width: 900px;">
				<tr>
					<td>
						<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="return SubmitPage()" Text="Save" />
						&nbsp;&nbsp;
						<asp:Button ValidationGroup="inputForm" ID="btnSaveButtonVisit" runat="server" OnClientClick="return SubmitPageVisit()" Text="Save and Visit" />
						&nbsp;&nbsp;
						<input type="button" id="btnCancel" value="Cancel" onclick="cancelEditing()" />
					</td>
					<td>
						<asp:CheckBox ID="chkDraft" runat="server" Text="  Save this as draft" />
					</td>
					<td>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					</td>
					<asp:Panel runat="server" ID="pnlReview">
						<td align="right">
							<asp:DropDownList ID="ddlVersions" runat="server" DataValueField="ContentID" DataTextField="EditDate" />
						</td>
						<td>
							&nbsp;&nbsp;
						</td>
						<td align="left">
							<input type="button" onclick="javascript:cmsPageVersionNav();" name="btnReview" value="Review / Revert" />
						</td>
					</asp:Panel>
				</tr>
			</table>
		</asp:Panel>
	</div>
	<script type="text/javascript">

		//		$(document).ready(function () {
		//			cmsLoadPrettyValidation('<%= formValidationSummary.ClientID %>');
		//		});

		function cmsPageVersionNav() {
			var qs = $('#<%= ddlVersions.ClientID %>').val();

			if (qs != '00000') {

				$("#confirmRevert").dialog({
					open: function () {
						$(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
					},

					resizable: false,
					height: 350,
					width: 450,
					modal: true,
					buttons: {
						"No": function () {
							$(this).dialog("close");
						},
						"Yes": function () {
							cmsMakeOKToLeave();
							window.setTimeout('location.href = \'<%=SiteData.CurrentScriptName %>?versionid=' + qs + '\'', 500);
							$(this).dialog("close");
						}
					}
				});

			}
		}
	</script>
	<br />
	<div style="display: none">
		<asp:TextBox ID="txtOldFile" runat="server" />
		<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save 1" />
		<asp:Button ValidationGroup="inputForm" ID="btnSaveVisit" runat="server" OnClick="btnSaveVisit_Click" Text="Save 2" />
		<div id="divCMSCancelWin" title="Quit Editor?">
			<p id="divCMSCancelWinMsg">
				Are you sure you want cancel?</p>
		</div>
		<div id="confirmRevert" title="Really Revert?">
			<div id="confirmRevertMsg">
				<p>
					Are you sure you want to open this older version of the content? All unsaved changes will be lost.</p>
				<p>
					This will not apply changes to the content until you save the prior version, generating a new copy.</p>
				<p>
					Note: this only applies to the page content shown here, all widgets currently assigned will remain.
				</p>
			</div>
		</div>
	</div>
	<script type="text/javascript">

		function SubmitPage() {
			var sc = SaveCommon();
			var ret = cmsIsPageValid();
			setTimeout("ClickSaveBtn();", 800);
			return ret;
		}

		function SubmitPageVisit() {
			var sc = SaveCommon();
			var ret = cmsIsPageValid();
			setTimeout("ClickSaveVisitBtn();", 800);
			return ret;
		}

		function ClickSaveBtn() {
			if (cmsIsPageValid()) {
				$('#<%=btnSave.ClientID %>').click();
			}
		}

		function ClickSaveVisitBtn() {
			if (cmsIsPageValid()) {
				$('#<%=btnSaveVisit.ClientID %>').click();
			}
		}

		function SaveCommon() {
			cmsSaveMakeOKAndCancelLeave();
			var ret = tinyMCE.triggerSave();
			cmsLoadPrettyValidationPopup('<%= formValidationSummary.ClientID %>');
			return true;
		}

	</script>
	<script type="text/javascript">

		$(document).ready(function () {
			setTimeout("$('#jqtabs').tabs('option', 'active', 1);", 500);
		});
	</script>
</asp:Content>
