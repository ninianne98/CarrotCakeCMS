<%@ Page ValidateRequest="false" Title="Page Add/Edit" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PageAddEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.PageAddEdit" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucSitePageDrillDown.ascx" TagName="ucSitePageDrillDown" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = cmsGetServiceAddress();

		var thePageID = '<%= guidRootContentID.ToString() %>';

		var tTitle = '#<%= txtTitle.ClientID %>';
		var tNav = '#<%= txtNav.ClientID %>';
		var tHead = '#<%= txtHead.ClientID %>';

		var thePage = '';

		function exportPage() {
			window.open("<%=SiteFilename.DataExportURL %>?id=" + thePageID);
		}

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
			var sGoLiveDate = $('#<%= txtReleaseDate.ClientID %>').val();

			if (theTitle.length > 0) {

				var webMthd = webSvc + "/GenerateNewFilename";
				var myPageTitle = MakeStringSafe(theTitle);

				$.ajax({
					type: "POST",
					url: webMthd,
					data: JSON.stringify({ ThePageTitle: myPageTitle, GoLiveDate: sGoLiveDate, PageID: thePageID, Mode: 'page' }),
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
				data: JSON.stringify({ TheFileName: myPage, PageID: thePageID }),
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

		/*  <asp:PlaceHolder  ID="pnlHBEmpty" runat="server"> begin empty cancel functions */

		function EditHB() { }

		function cancelEditing() { }

		function cmsRecordCancellation() { }


		function openPage() {
			var theURL = $('#<%= txtOldFile.ClientID %>').val();

			if (theURL.length > 3) {
				window.setTimeout("location.href = '" + theURL + "';", 250);
			} else {
				cmsAlertModalSmall("No saved page to show.");
			}
		}

		/*  </asp:PlaceHolder> */

		/* <asp:PlaceHolder ID="pnlHB" runat="server"> begin EditHB */

		function openPage() {
			var theURL = $('#<%= txtOldFile.ClientID %>').val();
			cmsOpenPage(theURL);
		}

		function EditHB() {
			setTimeout("EditHB();", 25 * 1000);

			var webMthd = webSvc + "/RecordHeartbeat";

			$.ajax({
				type: "POST",
				url: webMthd,
				data: JSON.stringify({ PageID: thePageID }),
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
						window.setTimeout("location.href = '<%=SiteFilename.PageIndexURL %>';", 800);
						$(this).dialog("close");
					}
				}
			});
		}

		setTimeout("cmsSendTrackbackPageBatch('" + thePageID + "');", 1500);

		function cmsRecordCancellation() {

			if (thePageID != '<%=Guid.Empty %>') {

				var webMthd = webSvc + "/CancelEditing";

				$.ajax({
					type: "POST",
					url: webMthd,
					data: JSON.stringify({ ThisPage: thePageID }),
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: cmsAjaxGeneralCallback,
					error: cmsAjaxFailed
				});
			}
		}


		/* </asp:PlaceHolder> */

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
				$('#nav-menu a.lnkPopup').each(function (i) {
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
	<link href="/c3-admin/Includes/tooltiphelper.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		var webSvc = cmsGetServiceAddress();
		var thisPageID = '<%=guidContentID.ToString() %>';

		function cmsGetWidgetText(val) {

			var webMthd = webSvc + "/GetWidgetLatestText";

			$.ajax({
				type: "POST",
				url: webMthd,
				data: JSON.stringify({ DBKey: val, ThisPage: thisPageID }),
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: cmsReqContentCallback,
				error: cmsAjaxFailed
			});
		}

		function cmsDoToolTipDataRequest(val) {
			cmsGetWidgetText(val);
		}

		function cmsReqContentCallback(data, status) {
			if (data.d == "FAIL") {
				cmsSetHTMLMessage('<i>An error occurred. Please try again.</i>');
			} else {
				cmsSetTextMessage(data.d);
			}
		}

	</script>
	<script src="Includes/FindUsers.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			initFindUsersMethod("<%=hdnCreditUserID.ClientID %>", "<%=txtSearchUser.ClientID %>", "FindCreditUsers");
		});
	</script>
	<script src="/c3-admin/Includes/tooltiphelper.js" type="text/javascript"></script>
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
					<a href="javascript:void(0)" onclick="openPage();">
						<img class="imgNoBorder" src="images/html2.png" title="Visit page" alt="Visit page" /></a>&nbsp;
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
					thumbnail:
					<br />
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtThumb" runat="server" Columns="45" MaxLength="200" />
					<input type="button" id="btnThumb" value="Browse" onclick="cmsFileBrowserOpenReturn('<%=txtThumb.ClientID %>');return false;" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					&nbsp;
				</td>
				<td>
					<asp:CheckBox ID="chkActive" runat="server" Text="Show publicly" />
					&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkNavigation" runat="server" Text="Include in site navigation" Checked="true" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					&nbsp;
				</td>
				<td>
					<asp:CheckBox ID="chkSiteMap" runat="server" Text="Include In Sitemap" Checked="true" />
					&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkHide" runat="server" Text="Hide from Search Engines" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					credit author:
				</td>
				<td>
					<b>find:</b> <span id="spanResults"></span>
					<br />
					<asp:TextBox ValidationGroup="inputForm" ID="txtSearchUser" onkeypress="return ProcessKeyPress(event)" Width="350px" MaxLength="100" runat="server" />
					<asp:HiddenField ID="hdnCreditUserID" runat="server" />
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
			<tr style="display: none">
				<td class="tablecaption">
					sort:
				</td>
				<td>
					<asp:TextBox ValidationGroup="inputForm" onblur="checkIntNumber(this);" Text="1" ID="txtSort" runat="server" Columns="15" MaxLength="5" onkeypress="return ProcessKeyPress(event)" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtSort" ID="RequiredFieldValidator5"
						runat="server" ErrorMessage="Sort Required" Display="Dynamic" Text="**" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					parent page:
					<br />
				</td>
				<td>
					<!-- parent page plugin-->
					<uc1:ucSitePageDrillDown ID="ParentPagePicker" runat="server" />
					<div style="clear: both; height: 2px;">
					</div>
					<%--<asp:DropDownList DataTextField="NavMenuText" DataValueField="Root_ContentID" ID="ddlParent" runat="server">
				</asp:DropDownList>--%>
				</td>
			</tr>
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
				<li><a href="#pagecontent-tabs-5">Text Controls</a></li>
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
				<div id="pagecontent-tabs-5">
					<div class="scroll-container" style="height: 325px; width: 780px;">
						<div class="scroll-area" style="height: 310px; width: 775px;">
							<div class="SortableGrid">
								<div>
									<h3>
										HTML Rich Text Widgets</h3>
									<carrot:CarrotGridView CssClass="datatable" DefaultSort="WidgetOrder ASC" ID="gvHtmControls" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
										AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
										<EmptyDataTemplate>
											<p>
												<b>No html text widgets found.</b>
											</p>
										</EmptyDataTemplate>
										<Columns>
											<asp:TemplateField>
												<ItemTemplate>
													<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('<%# String.Format("{0}?mode=html&pageid={1}&widgetid={2}", "ContentEdit.aspx", guidContentID, Eval("Root_WidgetID")) %>');">
														<img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit with WYSIWYG" title="Edit with WYSIWYG" /></a>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField>
												<ItemTemplate>
													<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('<%# String.Format("{0}?mode=plain&pageid={1}&widgetid={2}", "ContentEdit.aspx", guidContentID, Eval("Root_WidgetID")) %>');">
														<img class="imgNoBorder" src="/c3-admin/images/script.png" alt="Edit with Plain Text" title="Edit with Plain Text" /></a>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField HeaderText="Last Edited" DataField="EditDate" DataFormatString="{0}" />
											<asp:BoundField HeaderText="Placeholder Name" DataField="PlaceholderName" DataFormatString="{0}" />
											<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsWidgetActive" HeaderText="Active" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
												ShowBooleanImage="true" />
											<asp:TemplateField>
												<HeaderTemplate>
												</HeaderTemplate>
												<ItemTemplate>
													<a class="dataPopupTrigger" rel="<%# Eval("Root_WidgetID") %>" href="javascript:void(0)">
														<img src="/c3-admin/images/doc.png" alt="text" title="text" /></a>
												</ItemTemplate>
											</asp:TemplateField>
										</Columns>
									</carrot:CarrotGridView>
								</div>
								<br />
								<div>
									<h3>
										Plain Text Widgets</h3>
									<carrot:CarrotGridView CssClass="datatable" DefaultSort="WidgetOrder ASC" ID="gvTxtControls" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
										AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
										<EmptyDataTemplate>
											<p>
												<b>No plain text widgets found.</b>
											</p>
										</EmptyDataTemplate>
										<Columns>
											<asp:TemplateField>
												<ItemTemplate>
													<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('<%# String.Format("{0}?mode=plain&pageid={1}&widgetid={2}", "ContentEdit.aspx", guidContentID, Eval("Root_WidgetID")) %>');">
														<img class="imgNoBorder" src="/c3-admin/images/script.png" alt="Edit with Plain Text" title="Edit with Plain Text" /></a>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField HeaderText="Last Edited" DataField="EditDate" DataFormatString="{0}" />
											<asp:BoundField HeaderText="Placeholder Name" DataField="PlaceholderName" DataFormatString="{0}" />
											<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsWidgetActive" HeaderText="Active" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
												ShowBooleanImage="true" />
											<asp:TemplateField>
												<HeaderTemplate>
												</HeaderTemplate>
												<ItemTemplate>
													<a class="dataPopupTrigger" rel="<%# Eval("Root_WidgetID") %>" href="javascript:void(0)">
														<img src="/c3-admin/images/doc.png" alt="text" title="text" /></a>
												</ItemTemplate>
											</asp:TemplateField>
										</Columns>
									</carrot:CarrotGridView>
								</div>
							</div>
						</div>
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
			</tr>
		</table>
		<div style="display: none;">
			<asp:ValidationSummary ID="formValidationSummary" runat="server" ShowSummary="true" ValidationGroup="inputForm" />
		</div>
		<asp:PlaceHolder ID="pnlButtons" runat="server">
			<table style="width: 1000px;">
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
						&nbsp;&nbsp;
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
		</asp:PlaceHolder>
	</div>
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
