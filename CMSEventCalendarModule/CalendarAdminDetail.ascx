<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarAdminDetail.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.EventCalendarModule.CalendarAdminDetail" %>
<h2>
	Add/Edit Event
</h2>
<fieldset style="width: 650px;">
	<legend>
		<label>
			Event
		</label>
	</legend>
	<div>
		<b class="tablecaption">title: </b>
		<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtEventTitle" runat="server" Columns="80" MaxLength="100" />
		<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtEventTitle" ID="RequiredFieldValidator1"
			runat="server" Text="**" ToolTip="Title is required" ErrorMessage="Title is required" Display="Dynamic" />
	</div>
	<table style="width: 98%">
		<tr>
			<td style="width: 25%">
			</td>
			<td style="width: 25%">
			</td>
			<td style="width: 25%">
			</td>
			<td style="width: 25%">
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				recurrence frequency:
			</td>
			<td>
				<asp:DropDownList DataTextField="FrequencyName" DataValueField="CalendarFrequencyID" ID="ddlRecurr" runat="server" />
			</td>
			<td class="tablecaption">
				<div style="float: left; display: block; clear: left;">
					category:
				</div>
				<div style="float: right; clear: right; border: 1px dotted #c0c0c0; background-color: #ffffff; margin: 2px; padding: 2px; width: 36px;">
					<div id="ColorTextSample" style="margin: 2px; padding: 2px; font-weight: bolder; text-align: center;">
						X</div>
				</div>
				<div style="clear: both">
				</div>
			</td>
			<td>
				<asp:DropDownList DataTextField="CategoryName" DataValueField="CalendarEventCategoryID" ID="ddlCategory" runat="server" />
			</td>
		</tr>
		<tr id="trDaysOfWeek" style="display: none;">
			<td colspan="4">
				<asp:Repeater ID="rpDays" runat="server">
					<HeaderTemplate>
						<table style="width: 100%">
							<tr>
					</HeaderTemplate>
					<ItemTemplate>
						<td>
							<asp:CheckBox ID="chkDay" runat="server" Checked='<%# GetSelectedDays( (int)Eval("Key") ) %>' value='<%# Eval("Key") %>' />
							<asp:Literal ID="litDay" runat="server" Text='<%# Eval("Value")%>' />
						</td>
					</ItemTemplate>
					<FooterTemplate>
						</tr> </table>
					</FooterTemplate>
				</asp:Repeater>
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				start date:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtEventStartDate" runat="server" CssClass="dateRegion" Columns="16" />
			</td>
			<td class="tablecaption">
				end by date:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtEventEndDate" runat="server" CssClass="dateRegion" Columns="16" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				time from:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtEventStartTime" runat="server" CssClass="timeRegion" Columns="10" />
			</td>
			<td class="tablecaption">
				time to:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtEventEndTime" runat="server" CssClass="timeRegion" Columns="10" />
			</td>
		</tr>
		<tr>
			<td colspan="4">
				<table style="width: 96%">
					<tr>
						<td>
							<asp:CheckBox ID="chkIsAllDayEvent" runat="server" Text="All Day Event" Checked="true" />
						</td>
						<td>
							<asp:CheckBox ID="chkIsPublic" runat="server" Text="Show publicly" />
						</td>
						<td>
							<asp:CheckBox ID="chkIsCancelled" runat="server" Text="Cancelled" />
						</td>
						<td>
							<asp:CheckBox ID="chkIsCancelledPublic" runat="server" Text="Show even when cancelled" Checked="true" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</fieldset>
<fieldset style="width: 650px;">
	<legend>
		<label>
			Details
		</label>
	</legend>
	<div>
		<a href="javascript:cmsToggleTinyMCE('<%= reContent.ClientID %>');">Add/Remove editor</a><br />
		<asp:TextBox CssClass="mceEditor" ID="reContent" runat="server" TextMode="MultiLine" Rows="10" Columns="80" />
	</div>
</fieldset>
<div>
	<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="return SubmitPage('s')" Text="Save" />
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:Button ValidationGroup="inputForm" ID="btnCopyButton" runat="server" OnClientClick="return SubmitPage('c')" Text="Save as Copy" />
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:Button ValidationGroup="deleteForm" ID="btnDeleteButton" runat="server" OnClientClick="return DeleteItem()" Text="Delete" />
	<br />
</div>
<div style="display: none;">
	<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save btn" />
	<asp:Button ValidationGroup="inputForm" ID="btnCopy" runat="server" OnClick="btnCopy_Click" Text="Copy btn" />
	<asp:Button ValidationGroup="deleteForm" ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete btn" />
	<asp:ValidationSummary ID="formValidationSummary" runat="server" ShowSummary="true" ValidationGroup="inputForm" />
	<asp:DropDownList DataTextField="Value" DataValueField="Key" ID="ddlColors" runat="server" />
</div>
<script type="text/javascript">

	function SubmitPage(parm) {
		var ret = tinyMCE.triggerSave();
		cmsIsPageValid();
		setTimeout("ClickSaveBtn('" + parm + "');", 800);

		return false;
	}
	function ClickSaveBtn(p) {

		if (cmsIsPageValid()) {
			if (p == 's') {
				$('#<%=btnSave.ClientID %>').click();
			}
			if (p == 'c') {
				$('#<%=btnCopy.ClientID %>').click();
			}
		}

		cmsLoadPrettyValidationPopup('<%= formValidationSummary.ClientID %>');
		return true;
	}

	function DeleteItem() {
		var opts = {
			"No": function () { cmsAlertModalClose(); },
			"Yes": function () { ClickDeleteItem(); }
		};

		cmsAlertModalSmallBtns('Are you sure you want to delete this item? This will remove all events for this series.', opts);

		return false;
	}
	function ClickDeleteItem() {
		$('#<%=btnDelete.ClientID %>').click();
	}


	function setDayPickerVis(pat) {
		if (pat == 2) {
			$('#trDaysOfWeek').css('display', '');
		} else {
			$('#trDaysOfWeek').css('display', 'none');
		}
	}

	function initDays() {
		var val = $('#<%=ddlRecurr.ClientID %> option:selected').index();
		setDayPickerVis(val);
	}

	$(document).ready(function () {

		$("#<%=ddlRecurr.ClientID %>").bind("change keyup input", function () {
			var val = $('option:selected', $(this)).index();
			setDayPickerVis(val);
		});

		initDays();
	});


	function setCatColor(pat) {
		$('#<%=ddlColors.ClientID %>').val(pat);
		var txt = $("#<%=ddlColors.ClientID %> option:selected").text();

		var catColor = txt.split("|");

		$('#ColorTextSample').css('color', catColor[1]);
		$('#ColorTextSample').css('background-color', catColor[0]);
	}

	function initCatColor() {
		var val = $('#<%=ddlCategory.ClientID %>').val();
		setCatColor(val);
	}

	$(document).ready(function () {

		$("#<%=ddlCategory.ClientID %>").bind("change keyup input", function () {
			var val = $(this).val();
			setCatColor(val);
		});

		initCatColor();
	});




</script>
<%--
<tr>
	<td colspan="4">
		<div id="tabs">
			<asp:Repeater ID="rpTabs" runat="server">
				<HeaderTemplate>
					<ul>
				</HeaderTemplate>
				<ItemTemplate>
					<li><a href="#<%# String.Format("tabs-{0}", Eval("FrequencySortOrder")) %>">
						<%# Eval("FrequencyName")%></a></li>
				</ItemTemplate>
				<FooterTemplate>
					</ul></FooterTemplate>
			</asp:Repeater>
			<div id="tabs-1">
				<h2>
					Content heading 1</h2>
			</div>
			<div id="tabs-2">
				<h2>
					Content heading 2</h2>
			</div>
			<div id="tabs-3">
				<h2>
					Content heading 3</h2>
			</div>
			<div id="tabs-4">
				<h2>
					Content heading 4</h2>
			</div>
			<div id="tabs-5">
				<h2>
					Content heading 5</h2>
			</div>
		</div>
	</td>
</tr>
		
	$(document).ready(function () {
		$("#tabs").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
		$("#tabs li").removeClass("ui-corner-top").addClass("ui-corner-left");
	});	

--%>