<%@ Page Title="ContentSnippetAddEdit" Language="C#" ValidateRequest="false" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true"
	CodeBehind="ContentSnippetAddEdit.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ContentSnippetAddEdit" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = cmsGetServiceAddress();

		var thePageID = '<%= guidItemID %>';

		var tValid = '#<%= txtFileValid.ClientID %>';
		var tValidSlug = '#<%= txtSlug.ClientID %>';

		var thePage = '';

		function CheckFileName() {
			thePage = $(tValidSlug).val();

			$(tValid).val('');

			var webMthd = webSvc + "/ValidateUniqueSnippet";
			var myPage = MakeStringSafe(thePage);

			$.ajax({
				type: "POST",
				url: webMthd,
				data: JSON.stringify({ TheSlug: myPage, ItemID: thePageID }),
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
				$(tValidSlug).removeClass('validationExclaimBox');
			} else {
				$(tValid).val('NOT VALID');
				$(tValidSlug).addClass('validationExclaimBox');
			}

			var ret = cmsIsPageValid();
			cmsForceInputValidation('<%= txtFileValid.ClientID %>');
		}


		$(document).ready(function () {
			// these click events because of stoopid IE9 navigate away behavior
			$('#nav-menu a.lnkPopup').each(function (i) {
				$(this).click(function () {
					cmsMakeOKToLeave();
					setTimeout("cmsMakeNotOKToLeave();", 500);
				});
			});

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
						cmsRecordCancellation();
						cmsMakeOKToLeave();
						window.setTimeout("location.href = '<%=SiteFilename.ContentSnippetIndexURL %>';", 800);
						$(this).dialog("close");
					}
				}
			});
		}

		/*  <asp:PlaceHolder  ID="pnlHBEmpty" runat="server"> begin empty cancel functions */

		function EditHB() { }

		function cmsRecordCancellation() { }

		/*  </asp:PlaceHolder> */

		/* <asp:PlaceHolder ID="pnlHB" runat="server"> begin EditHB */

		function EditHB() {
			setTimeout("EditHB();", 25 * 1000);

			var webMthd = webSvc + "/RecordSnippetHeartbeat";

			$.ajax({
				type: "POST",
				url: webMthd,
				data: JSON.stringify({ ItemID: thePageID }),
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
			setTimeout("EditHB();", 2000);
		});

		function cmsRecordCancellation() {

			if (thePageID != '<%=Guid.Empty %>') {

				var webMthd = webSvc + "/CancelSnippetEditing";

				$.ajax({
					type: "POST",
					url: webMthd,
					data: JSON.stringify({ ItemID: thePageID }),
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: cmsAjaxGeneralCallback,
					error: cmsAjaxFailed
				});
			}
		}

		/* </asp:PlaceHolder> */

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
				$('#nav-menu a.lnkPopup').each(function (i) {
					$(this).click(function () {
						cmsMakeOKToLeave();
						setTimeout("cmsMakeNotOKToLeave();", 500);
					});
				});
			}
		});

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Content Snippet Add/Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div class="ui-widget" runat="server" id="divEditing">
		<div class="ui-state-highlight ui-corner-all" style="padding: 5px; margin-top: 5px; margin-bottom: 5px; width: 500px;">
			<p>
				<span class="ui-icon ui-icon-info" style="float: left; margin: 3px;"></span>
				<asp:Literal ID="litUser" runat="server">&nbsp;</asp:Literal></p>
		</div>
	</div>
	<table style="width: 820px;">
		<tr>
			<td style="width: 110px;" class="tablecaption">
				slug:
			</td>
			<td style="width: 700px;">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="CheckFileName()" ID="txtSlug" runat="server" Columns="60" MaxLength="100" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtSlug" ID="RequiredFieldValidator1"
					runat="server" Text="**" ToolTip="Slug is required" ErrorMessage="Slug is required" Display="Dynamic" />
				<asp:CompareValidator ValidationGroup="inputForm" CssClass="validationExclaim" ForeColor="" ControlToValidate="txtFileValid" ID="CompareValidator1" runat="server"
					ErrorMessage="Slug is not valid/not unique" ToolTip="Slug is not valid/not unique" Text="##" Display="Dynamic" ValueToCompare="VALID" Operator="Equal" />
				<div style="display: none;">
					<asp:TextBox runat="server" ValidationGroup="inputForm" ID="txtFileValid" MaxLength="25" Columns="25" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtFileValid" ID="RequiredFieldValidator2"
						runat="server" ErrorMessage="Slug validation is required" ToolTip="Slug validation is required" Display="Dynamic" Text="**" />
				</div>
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				name:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="CheckFileName()" ID="txtLabel" runat="server" Columns="60"
					MaxLength="100" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtLabel" ID="RequiredFieldValidator3"
					runat="server" Text="**" ToolTip="Name is required" ErrorMessage="Name is required" Display="Dynamic" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				release date:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtReleaseDate" runat="server" CssClass="dateRegion" Columns="16" />
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtReleaseTime" runat="server" CssClass="timeRegion" Columns="10" />
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
				active:
			</td>
			<td>
				<asp:CheckBox ID="chkPublic" runat="server" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				snippet text:
			</td>
			<td>
				<div runat="server" id="divCenter">
					<a href="javascript:cmsToggleTinyMCE('<%= reBody.ClientID %>');">Show/Hide Editor</a></div>
				<asp:TextBox ValidationGroup="inputForm" Style="height: 300px; width: 650px;" CssClass="mceEditor" ID="reBody" runat="server" TextMode="MultiLine" Rows="20"
					Columns="60" />
				<br />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				&nbsp;
			</td>
			<td>
				<div id="cmsHeartBeat" style="clear: both; padding: 2px; margin: 2px; min-height: 22px;">
					&nbsp;</div>
			</td>
		</tr>
	</table>
	<asp:Panel ID="pnlButtons" runat="server">
		<table style="width: 900px;">
			<tr>
				<td>
					<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="return SubmitPage()" Text="Save" />
					&nbsp;&nbsp;
					<input type="button" id="btnCancel" value="Cancel" onclick="cancelEditing()" />
					&nbsp;&nbsp;
					<asp:Button ValidationGroup="deleteForm" ID="btnDeleteButton" runat="server" OnClientClick="return DeleteItem()" Text="Delete" />
				</td>
				<td>
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				</td>
				<asp:PlaceHolder runat="server" ID="pnlReview">
					<td align="right">
						<asp:DropDownList ID="ddlVersions" runat="server" DataValueField="Key" DataTextField="Value" />
					</td>
					<td>
						&nbsp;&nbsp;
					</td>
					<td align="left">
						<input type="button" onclick="javascript:cmsPageVersionNav();" name="btnReview" value="Review / Revert" />
					</td>
				</asp:PlaceHolder>
			</tr>
		</table>
	</asp:Panel>
	<script type="text/javascript">

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
	<div style="display: none;">
		<asp:ValidationSummary ID="formValidationSummary" runat="server" ShowSummary="true" ValidationGroup="inputForm" />
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
			</div>
		</div>
	</div>
	<div style="display: none;">
		<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
		<asp:Button ValidationGroup="deleteForm" ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" />
	</div>
	<script type="text/javascript">

		function DeleteItem() {
			var opts = {
				"No": function () { cmsAlertModalClose(); },
				"Yes": function () { ClickDeleteItem(); }
			};

			cmsAlertModalSmallBtns('Are you sure you want to delete this item?', opts);

			return false;
		}

		function ClickDeleteItem() {
			cmsMakeOKToLeave();
			$('#<%=btnDelete.ClientID %>').click();
		}

		function SubmitPage() {
			var sc = SaveCommon();
			var ret = cmsIsPageValid();
			setTimeout("ClickSaveBtn();", 800);
			return ret;
		}

		function ClickSaveBtn() {
			if (cmsIsPageValid()) {
				$('#<%=btnSave.ClientID %>').click();
			}
		}

		function SaveCommon() {
			cmsSaveMakeOKAndCancelLeave();
			var ret = tinyMCE.triggerSave();
			cmsLoadPrettyValidationPopup('<%= formValidationSummary.ClientID %>');
			return true;
		}


	</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
