<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarAdminAddEdit.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.CalendarModule.CalendarAdminAddEdit" %>
<table width="600" align="center">
	<tr>
		<td valign="top" width="150" class="tablecaption">
			date (m/d/y)
		</td>
		<td valign="top" width="450">
			<div id="divDatePicker" runat="server">
				<asp:TextBox Style="display: none;" ID="lblDate" runat="server" ReadOnly="true" Columns="40" />
				<asp:TextBox ValidationGroup="inputForm" CssClass="dateRegion" ID="txtDate" Columns="12" runat="server" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtDate"
					ID="RequiredFieldValidator1" runat="server" ErrorMessage="Date is required" ToolTip="Date is required" Display="Dynamic"
					Text="**" />
			</div>
		</td>
	</tr>
	<tr>
		<td valign="top" class="tablecaption">
			event
		</td>
		<td>
			<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtEvent" runat="server" Columns="60" />
			<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtEvent"
				ID="RequiredFieldValidator2" runat="server" ErrorMessage="Event is required" ToolTip="Event is required" Display="Dynamic"
				Text="**" />
		</td>
	</tr>
	<tr>
		<td valign="top" class="tablecaption">
			details
		</td>
		<td>
			<%--<a href="javascript:cmsToggleTinyMCE('<%= reContent.ClientID %>');">Add/Remove editor</a>--%>
			<asp:TextBox CssClass="mceEditor" ID="reContent" runat="server" TextMode="MultiLine" Rows="8" Columns="80" />
		</td>
	</tr>
	<tr>
		<td valign="top" align="right">
			&nbsp;
		</td>
		<td valign="top" align="right">
			<div style="text-align: left; float: left">
				Active
				<asp:CheckBox ID="chkActive" runat="server" Checked="True" />
			</div>
			<br />
			&nbsp;<input id="btnDelete" type="button" onclick="fnDelete()" value="Delete" name="btnDelete" runat="server" />
			&nbsp;<asp:Button ID="cmdClone" UseSubmitBehavior="false" runat="server" OnClientClick="return AddPage();" Text="Clone" />
			&nbsp;<asp:Button ID="cmdSave" UseSubmitBehavior="false" runat="server" OnClientClick="return SubmitPage();" Text="Save" />
			<span style="display: none">
				<asp:Button ID="cmdDelete" runat="server" Text="cmdDelete" OnClick="cmdDelete_Click" />
				<asp:Button ID="btnAdd" runat="server" Text="Clone" OnClick="cmdAdd_Click" ValidationGroup="inputForm" />
				<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="cmdSave_Click" ValidationGroup="inputForm" />
				<asp:TextBox onkeypress="return ProcessKeyPress(event)" ID="txtID" runat="server" Columns="5" Text="0" />
			</span>
		</td>
	</tr>
</table>
<script type="text/javascript">
    function calAutoSynchMCE() {

        if (saving != 1) {
           var ret = cmsPreSaveTrigger();
            setTimeout("calAutoSynchMCE();", 2500);
            //alert("AutoSynchMCE");
        }
    }

    calAutoSynchMCE();

    var saving = 0;

    function SubmitPage() {
        calAutoSynchMCE();
        saving = 1;
        setTimeout("ClickSub();", 800);
		return false;
    }
    function ClickSub() {
        $('#<%=btnSave.ClientID %>').click();
    }

    function AddPage() {
        calAutoSynchMCE();
        saving = 1;
        setTimeout("ClickAdd();", 800);
		return false;
    }
    function ClickAdd() {
        $('#<%=btnAdd.ClientID %>').click();
    }

    function fnDelete() {
	    var oFrm = document.forms[0];
	    if (confirm("You sure you want to delete?")) {
		    oFrm.<%=cmdDelete.ClientID%>.click();
	    }
    }
</script>
</asp:Content>