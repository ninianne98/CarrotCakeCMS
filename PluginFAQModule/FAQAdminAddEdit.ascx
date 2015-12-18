<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FAQAdminAddEdit.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.FAQModule.FAQAdminAddEdit" %>
<table width="750">
	<tr>
		<td width="100">
			&nbsp;
			<br />
			question
		</td>
		<td colspan="2" width="600">
			<a href="javascript:cmsToggleTinyMCE('<%= reQuestion.ClientID %>');">Add/Remove editor</a><br />
			<asp:TextBox Style="height: 150px; width: 625px;" CssClass="mceEditor" ID="reQuestion" runat="server" TextMode="MultiLine" Rows="6" Columns="60" />
		</td>
	</tr>
	<tr>
		<td>
			&nbsp;
			<br />
			answer
		</td>
		<td colspan="2">
			<a href="javascript:cmsToggleTinyMCE('<%= reAnswer.ClientID %>');">Add/Remove editor</a><br />
			<asp:TextBox Style="height: 150px; width: 625px;" CssClass="mceEditor" ID="reAnswer" runat="server" TextMode="MultiLine" Rows="6" Columns="60" />
		</td>
	</tr>
	<tr>
		<td>
			&nbsp;
		</td>
		<td align="center">
			<div style="text-align: left; float: left; margin-right: 20px;">
				<asp:CheckBox ID="chkActive" runat="server" Checked="True" />
				Active
			</div>
			<div style="text-align: left; float: left; margin-right: 20px;">
				Sort:&nbsp;
				<asp:TextBox onkeypress="return ProcessKeyPress(event)" ID="txtSort" onblur="checkIntNumber(this)" runat="server" Columns="8" Text="0" MaxLength="3" />
			</div>
		</td>
		<td align="right">
			&nbsp;<input id="btnDelete" onclick="fnDelete()" type="button" value="Delete" name="btnDelete" runat="server" />
			&nbsp;<asp:Button ID="cmdClone" runat="server" OnClientClick="AddPage()" Text="Clone" />
			&nbsp;<asp:Button ID="cmdSave" runat="server" OnClientClick="SubmitPage()" Text="Save" />
			<span style="display: none">
				<asp:Button ID="cmdDelete" runat="server" Text="cmdDelete" OnClick="cmdDelete_Click" />
				&nbsp;<asp:Button ID="btnAdd" runat="server" Text="Clone" OnClick="cmdAdd_Click" />
				<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="cmdSave_Click" />
				<asp:TextBox onkeypress="return ProcessKeyPress(event)" ID="txtID" runat="server" Columns="5" Text="0" />
			</span>
		</td>
	</tr>
</table>
<script language="javascript">
    function calAutoSynchMCE() {

        if (saving != 1) {
            tinyMCE.triggerSave();
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
    }
    function ClickSub() {
        $('#<%=btnSave.ClientID %>').click();
    }

    function AddPage() {
        calAutoSynchMCE();
        saving = 1;
        setTimeout("ClickAdd();", 800);
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
