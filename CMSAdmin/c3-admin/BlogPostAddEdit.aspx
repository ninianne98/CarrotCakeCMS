<%@ Page ValidateRequest="false" Title="BlogPostAddEdit" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="BlogPostAddEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.BlogPostAddEdit" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = "/c3-admin/CMS.asmx";

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

		function CheckFileName() {
			GenerateBlogFilePrefix();

			thePage = $('#<%= txtPageSlug.ClientID %>').val();

			$('#<%= txtFileValid.ClientID %>').val('');
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
				cmsAlertModal("Cannot create a filename with there is no title value assigned.");
			}
		}

		function ajaxGeneratePageFilename(data, status) {
			debugger;
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
				cmsAlertModal("No saved page to show.");
			}
		}

		/*  </asp:PlaceHolder> */

		/* <asp:PlaceHolder ID="pnlHB" runat="server"> begin EditHB */

		function openPage() {
			var theURL = $('#<%= txtOldFile.ClientID %>').val();

			if (theURL.length > 3) {
				$("#divCMSCancelWinMsg").text('Are you sure you want to open the webpage leave this editor? All unsaved changes will be lost!');

//				var isOpen = $("#divCMSCancelWin").dialog("isOpen");
//				if (isOpen) {
//					$("#divCMSCancelWin").dialog("destroy");
//				}

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

//			var isOpen = $("#divCMSCancelWin").dialog("isOpen");
//			if (isOpen) {
//				$("#divCMSCancelWin").dialog("destroy");
//			}

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
		<table width="700">
			<tr>
				<td width="125" class="tablecaption" valign="top">
					last updated:
				</td>
				<td width="575" valign="top">
					<asp:Label ID="lblUpdated" runat="server" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					create date:
				</td>
				<td valign="top">
					<asp:Label ID="lblCreateDate" runat="server" /><br />
					<br />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					release date:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtReleaseDate" runat="server" CssClass="dateRegion" Columns="16"
						onblur="GenerateBlogFilePrefix()" onchange="GenerateBlogFilePrefix()" />
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtReleaseTime" runat="server" CssClass="timeRegion" Columns="10"
						onblur="GenerateBlogFilePrefix()" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					retire date:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtRetireDate" runat="server" CssClass="dateRegion" Columns="16" />
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtRetireTime" runat="server" CssClass="timeRegion" Columns="10" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					title bar:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="AutoGeneratePageFilename()" ID="txtTitle" runat="server" Columns="45"
						MaxLength="200" />
					<a href="javascript:void(0)" onclick="GeneratePageFilename()" class="lnkPopup">
						<img class="imgNoBorder" src="/c3-admin/images/page_white_wrench.png" title="Generate Filename and other Title fields" alt="Generate Filename and other Title fields" /></a>&nbsp;
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtTitle" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
						Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					<br />
				</td>
				<td valign="top">
					<div style="padding: 3px;" class=" ui-widget-content ui-corner-all ">
						<asp:Label ID="lblPrefix" runat="server" Text="/yy/mm/dd/" />
					</div>
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					filename:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="CheckFileName()" ID="txtPageSlug" runat="server" Columns="45"
						MaxLength="200" />
					&nbsp; <a href="javascript:void(0)" onclick="openPage();">
						<img class="imgNoBorder" src="/c3-admin/images/html2.png" title="Visit page" alt="Visit page" /></a>&nbsp;
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtPageSlug" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
						Display="Dynamic" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtFileValid" ID="RequiredFieldValidator6" runat="server" ErrorMessage="Not Valid/Unique"
						Display="Dynamic" />
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
						Display="Dynamic" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					page head:
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtHead" runat="server" Columns="45" MaxLength="200" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					thumbnail:
					<br />
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtThumb" runat="server" Columns="45" MaxLength="200" />
					<input type="button" id="Button1" value="Browse" onclick="cmsFileBrowserOpenReturn('<%=txtThumb.ClientID %>');return false;" />
				</td>
			</tr>
			<tr>
				<td valign="top" class="tablecaption">
					&nbsp;
				</td>
				<td valign="top">
					<asp:CheckBox ID="chkActive" runat="server" Text="Show publicly" />
					<%--&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkNavigation" runat="server" Text="Include in site navigation" Checked="true" />--%>
				</td>
			</tr>
			<tr>
				<td width="125" class="tablecaption" valign="top">
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
						Display="Dynamic" />
				</td>
			</tr>
		</table>
		<table width="700">
			<tr>
				<td width="350" valign="top">
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
				<td width="350" valign="top">
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
		<table width="700">
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
				<li><a href="#pagecontent-tabs-4">Trackback URLs</a></li>
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
			<div id="pagecontent-tabs-4">
				<div style="height: 310px; margin-bottom: 10px;">
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

//			var isOpen = $("#confirmRevert").dialog("isOpen");
//			if (isOpen) {
//				$("#confirmRevert").dialog("destroy");
//			}

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
			cmsSaveMakeOKAndCancelLeave();
			var ret = tinyMCE.triggerSave();
			CheckFileName();
			return true;
		}

		function ClickSaveBtn() {
			$('#<%=btnSave.ClientID %>').click();
		}
		function ClickSaveVisitBtn() {
			$('#<%=btnSaveVisit.ClientID %>').click();
		}
	</script>
	<script type="text/javascript">

		$(document).ready(function () {
			setTimeout("$('#jqtabs').tabs('option', 'active', 1);", 500);
		});
	</script>
</asp:Content>
