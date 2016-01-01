<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminDetail.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.FAQ2Module.AdminDetail" %>
<h2>
	FAQs : Entry Add/Edit</h2>
<p>
	<asp:HyperLink ID="lnkBack" runat="server">
	<img class="imgNoBorder" src="/c3-admin/images/back.png" alt="Back" title="Back" />
	Return to list</asp:HyperLink>
</p>
<table width="750">
	<tr>
		<td width="100">
			caption
		</td>
		<td colspan="2">
			<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtCaption" runat="server" Columns="60" MaxLength="128" />
			<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtCaption" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
				Display="Dynamic" />
		</td>
	</tr>
	<tr>
		<td>
			&nbsp;
			<br />
			question
		</td>
		<td colspan="2">
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
			<div style="text-align: left; float: left; margin-right: 30px;">
				<asp:CheckBox ID="chkActive" runat="server" Checked="True" />
				&nbsp;Active
			</div>
			<div style="text-align: left; float: left; margin-right: 30px;">
				Sort:&nbsp;
				<asp:TextBox onkeypress="return ProcessKeyPress(event)" ID="txtSort" onblur="checkIntNumber(this)" runat="server" Columns="8" Text="0" MaxLength="3" />
			</div>
		</td>
		<td align="right" width="350">
			&nbsp;<input id="btnDelete" onclick="fnDelete()" type="button" value="Delete" name="btnDelete" runat="server" />
			&nbsp;<asp:Button ID="cmdClone" runat="server" OnClientClick="AddPage()" Text="Clone" />
			&nbsp;<asp:Button ID="cmdSave" runat="server" OnClientClick="SubmitPage()" Text="Save" />
			&nbsp;<asp:Button ID="btnCanel" runat="server" Text="Cancel" OnClick="btnCanel_Click" />
			<span style="display: none">
				<asp:Button ID="cmdDelete" runat="server" Text="cmdDelete" OnClick="cmdDelete_Click" />
				<asp:Button ID="btnAdd" runat="server" Text="Clone" OnClick="cmdAdd_Click" />
				<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="cmdSave_Click" />
				<asp:TextBox ID="txtID" runat="server" Text="0" />
				<asp:TextBox ID="txtCatID" runat="server" Text="0" />
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
