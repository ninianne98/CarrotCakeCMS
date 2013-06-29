<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarAdminDetailSingle.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.EventCalendarModule.CalendarAdminDetailSingle" %>
<h2>
	Edit Event
</h2>
<fieldset style="width: 650px;">
	<legend>
		<label>
			Individual Event
		</label>
	</legend>
	<div>
		<b class="tablecaption">title: </b>
		<asp:Literal ID="litTitle" runat="server" />
	</div>
	<table style="width: 98%">
		<tr>
			<td style="width: 20%">
			</td>
			<td style="width: 30%">
			</td>
			<td style="width: 20%">
			</td>
			<td style="width: 30%">
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				date:
			</td>
			<td>
				<asp:Literal ID="litDate" runat="server" />
				<asp:Literal ID="litTime" runat="server" />
			</td>
			<td colspan="2">
				<asp:CheckBox ID="chkIsCancelled" runat="server" Text="Cancelled" />
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
	<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Save" />
	<br />
</div>
<div style="display: none;">
	<asp:DropDownList DataTextField="Value" DataValueField="Key" ID="ddlColors" runat="server" />
	<asp:ValidationSummary ID="formValidationSummary" runat="server" ShowSummary="true" ValidationGroup="inputForm" />
	<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Apply" />
</div>
<script type="text/javascript">
	function SubmitPage() {
		var ret = tinyMCE.triggerSave();
		cmsIsPageValid();
		setTimeout("ClickSaveBtn();", 500);
	}
	function ClickSaveBtn() {
		if (cmsIsPageValid()) {
			$('#<%=btnSave.ClientID %>').click();
		}
		cmsLoadPrettyValidationPopup('<%= formValidationSummary.ClientID %>');
		return true;
	}
	
</script>
