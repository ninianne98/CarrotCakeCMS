<%@ Page ValidateRequest="false" Title="PageAddEdit" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PageAddEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.PageAddEdit" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucSitePageDrillDown.ascx" TagName="ucSitePageDrillDown" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = "/Manage/CMS.asmx";

		var thePageID = '<%=guidRootContentID.ToString() %>';

		var thePage = '';


		function exportPage() {
			window.open("<%=Carrotware.CMS.UI.Admin.SiteFilename.DataExportURL %>?id=" + thePageID);
		}

		function openPage() {
			var theURL = $('#<%= txtOldFile.ClientID %>').val();

			if (theURL.length > 3) {
				$("#divCMSCancelWinMsg").text('Are you sure you want to open the webpage leave this editor? All unsaved changes will be lost!');

				$("#divCMSCancelWin").dialog("destroy");

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
							window.setTimeout("location.href = '" + theURL + "';", 800);
							$(this).dialog("close");
						}
					}
				});
			} else {
				cmsAlertModal("No saved page to show.");
			}
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
			if (data.d != "FAIL" && data.d != "OK") {
				cmsAlertModal(data.d);
			}

			if (data.d == "OK") {
				$('#<%= txtFileValid.ClientID %>').val('VALID');
			} else {
				$('#<%= txtFileValid.ClientID %>').val('');
			}
			Page_ClientValidate();
		}

		/* <asp:placeholder ID="pnlHB" runat="server"> begin EditHB */

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

			$("#divCMSCancelWin").dialog("destroy");

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
						window.setTimeout("location.href = '<%=Carrotware.CMS.UI.Admin.SiteFilename.PageIndexURL %>';", 800);
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
		var cmsTemplatePreview = '<%=Carrotware.CMS.Core.SiteData.PreviewTemplateFilePage %>';

		function cmsPreviewTemplate() {
			var tmpl = $(cmsTemplateDDL).val();

			tmpl = MakeStringSafe(tmpl);

			ShowWindowNoRefresh(cmsTemplatePreview + "?carrot_templatepreview=" + tmpl);
		}

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Page Add/Edit
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
		<table width="700">
			<tr>
				<td width="125" class="tablecaption" valign="top">
					last updated:
				</td>
				<td width="575" valign="top">
					<table cellpadding="0" cellspacing="0">
						<tr>
							<td width="175" valign="top">
								<asp:Label ID="lblUpdated" runat="server"></asp:Label>
							</td>
							<td width="175" valign="top">
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</td>
							<td width="100" valign="top">
								<asp:CheckBox ID="chkActive" runat="server" Text="published" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					create date:
				</td>
				<td valign="top">
					<asp:Label ID="lblCreatDate" runat="server"></asp:Label><br />
					<br />
				</td>
			</tr>
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
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="CheckFileName()" ID="txtFileName" runat="server" Columns="45"
						MaxLength="200" />&nbsp; <a href="javascript:void(0)" onclick="openPage();">
							<img class="imgNoBorder" src="/Manage/images/html2.png" title="Visit page" alt="Visit page" /></a>&nbsp;
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
					<%--<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtHead" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required"
					Display="Dynamic"></asp:RequiredFieldValidator>--%>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					meta keywords:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" ID="txtKey" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="4" TextMode="MultiLine" runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					meta description:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" ID="txtDescription" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="4" TextMode="MultiLine" runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr style="display: none">
				<td class="tablecaption" valign="top">
					sort:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onblur="checkIntNumber(this);" Text="1" ID="txtSort" runat="server" Columns="15" MaxLength="5" onkeypress="return ProcessKeyPress(event)" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtSort" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Required"
						Display="Dynamic"></asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					parent page:
					<br />
				</td>
				<td valign="top">
					<!-- parent page plugin-->
					<uc1:ucSitePageDrillDown ID="ParentPagePicker" runat="server" />
					<div style="clear: both; height: 2px;">
					</div>
					<%--<asp:DropDownList DataTextField="NavMenuText" DataValueField="Root_ContentID" ID="ddlParent" runat="server">
				</asp:DropDownList>--%>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					template:
				</td>
				<td valign="top">
					<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplate" runat="server">
					</asp:DropDownList>
					&nbsp;&nbsp;&nbsp;&nbsp;
					<input type="button" onclick="cmsPreviewTemplate()" value="Preview" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
				</td>
				<td valign="top" align="right">
					<input type="button" id="btnBrowseSvr" value="Browse Server Files" onclick="cmsFileBrowserOpen('not-a-real-file');" />
				</td>
			</tr>
		</table>
		<br />
		<div id="jqtabs" style="height: 400px; width: 825px;">
			<ul>
				<li><a href="#pagecontent-tabs-0">Left</a></li>
				<li><a href="#pagecontent-tabs-1">Center</a></li>
				<li><a href="#pagecontent-tabs-3">Right</a></li>
			</ul>
			<div id="pagecontent-tabs-0">
				<div style="height: 325px; margin-bottom: 10px;">
					<div runat="server" id="divLeft">
						body (left)<br />
						<a href="javascript:cmsToggleTinyMCE('<%= reLeftBody.ClientID %>');">Show/Hide Editor</a></div>
					<asp:TextBox Style="height: 280px; width: 780px;" CssClass="mceEditor" ID="reLeftBody" runat="server" TextMode="MultiLine" Rows="15" Columns="80" />
					<br />
				</div>
			</div>
			<div id="pagecontent-tabs-1">
				<div style="height: 310px; margin-bottom: 10px;">
					<div runat="server" id="divCenter">
						body (main/center)<br />
						<a href="javascript:cmsToggleTinyMCE('<%= reBody.ClientID %>');">Show/Hide Editor</a></div>
					<asp:TextBox Style="height: 280px; width: 780px;" CssClass="mceEditor" ID="reBody" runat="server" TextMode="MultiLine" Rows="15" Columns="80" />
					<br />
				</div>
			</div>
			<div id="pagecontent-tabs-3">
				<div style="height: 310px; margin-bottom: 10px;">
					<div runat="server" id="divRight">
						body (right)<br />
						<a href="javascript:cmsToggleTinyMCE('<%= reRightBody.ClientID %>');">Show/Hide Editor</a></div>
					<asp:TextBox Style="height: 280px; width: 780px;" CssClass="mceEditor" ID="reRightBody" runat="server" TextMode="MultiLine" Rows="15" Columns="80" />
					<br />
				</div>
			</div>
		</div>
		<table width="800">
			<tr>
				<td valign="top" align="left">
					<div id="cmsHeartBeat" style="clear: both; padding: 2px; margin: 2px; min-height: 22px;">
						&nbsp;</div>
				</td>
				<td>
					&nbsp;&nbsp;
				</td>
				<td valign="top" align="right">
					<%--<a target="_blank" id="lnkExport" runat="server" href="javascript:void(0);" onclick="exportPage();">Export latest version of this page</a>--%>
				</td>
			</tr>
		</table>
		<asp:Panel ID="pnlButtons" runat="server">
			<table width="900">
				<tr>
					<td valign="top">
						<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Save" />
						&nbsp;&nbsp;
						<asp:Button ValidationGroup="inputForm" ID="btnSaveButtonVisit" runat="server" OnClientClick="SubmitPageVisit()" Text="Save and Visit" />
						&nbsp;&nbsp;
						<input type="button" id="btnCancel" value="Cancel" onclick="cancelEditing()" />
					</td>
					<td valign="top">
						<asp:CheckBox ID="chkDraft" runat="server" Text="  Save this as draft" />
					</td>
					<td valign="top">
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					</td>
					<asp:Panel runat="server" ID="pnlReview">
						<td valign="top" align="right">
							<asp:DropDownList ID="ddlVersions" runat="server" DataValueField="ContentID" DataTextField="EditDate">
							</asp:DropDownList>
						</td>
						<td valign="top">
							&nbsp;&nbsp;
						</td>
						<td valign="top" align="left">
							<input type="button" onclick="javascript:cmsPageVersionNav();" name="btnReview" value="Review / Revert" />
						</td>
					</asp:Panel>
				</tr>
			</table>
		</asp:Panel>
	</div>
	<script type="text/javascript">
		function cmsPageVersionNav() {
			var qs = $('#<%= ddlVersions.ClientID %>').val();

			$("#confirmRevert").dialog("destroy");

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
							window.setTimeout('location.href = \'<%=Carrotware.CMS.Core.SiteData.CurrentScriptName %>?versionid=' + qs + '\'', 500);
							$(this).dialog("close");
						}
					}
				});

			}
		}
	</script>
	<br />
	<div style="display: none">
		<asp:TextBox ID="txtOldFile" runat="server"></asp:TextBox>
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
			var ret = SaveCommon();
			setTimeout("ClickSaveBtn();", 500);
		}

		function SubmitPageVisit() {
			var ret = SaveCommon();
			setTimeout("ClickSaveVisitBtn();", 500);
		}

		function SaveCommon() {
			cmsMakeOKToLeave();
			var ret = tinyMCE.triggerSave();
			CheckFileName();
			return true;
		}

		function ClickSaveBtn() {
			$('#<%=btnSave.ClientID %>').click();
			setTimeout("cmsMakeNotOKToLeave();", 2500);
		}
		function ClickSaveVisitBtn() {
			$('#<%=btnSaveVisit.ClientID %>').click();
			setTimeout("cmsMakeNotOKToLeave();", 2500);
		}
	</script>
	<script type="text/javascript">

		$(document).ready(function () {
			setTimeout("$('#jqtabs').tabs('select', 'pagecontent-tabs-1');", 500);
		});
	</script>
</asp:Content>
