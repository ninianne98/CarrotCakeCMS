<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarAdminAddEdit.ascx.cs"
    Inherits="Carrotware.CMS.UI.Plugins.CalendarModule.CalendarAdminAddEdit" %>
<table width="600" align="center">
    <tr>
        <td valign="top" width="150">
            date (m/d/y)
        </td>
        <td valign="top" width="450">
            <div id="divDatePicker" runat="server">
                <asp:TextBox Style="display: none;" ID="lblDate" runat="server" ReadOnly="true" Columns="40"></asp:TextBox>
                <asp:TextBox CssClass="dateRegion" ID="txtDate" Columns="12" runat="server"></asp:TextBox>
            </div>
        </td>
    </tr>
    <tr>
        <td valign="top">
            event
        </td>
        <td>
            <asp:TextBox onkeypress="return ProcessKeyPress(event)" ID="txtEvent" runat="server"
                Columns="60"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td valign="top">
            details
        </td>
        <td>
            <a href="javascript:toggleEditor('<%= reContent.ClientID %>');">Add/Remove editor</a>
            <asp:TextBox CssClass="mceEditor" ID="reContent" runat="server" TextMode="MultiLine"
                Rows="8" Columns="80"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td valign="top" align="right">
            &nbsp;
        </td>
        <td valign="top" align="right">
            <div style="text-align: left; float: left">
                Active
                <asp:CheckBox ID="chkActive" runat="server" Checked="True"></asp:CheckBox>
            </div>
            <br />

            &nbsp;<input id="btnDelete" onclick="fnDelete()" type="button" value="Delete" name="btnDelete"
                runat="server" />
            &nbsp;<asp:Button ID="cmdClone" runat="server" OnClientClick="AddPage()" Text="Clone" />
            &nbsp;<asp:Button ID="cmdSave" runat="server" OnClientClick="SubmitPage()" Text="Save" />
            <span style="display: none">
                <asp:Button ID="cmdDelete" runat="server" Text="cmdDelete" OnClick="cmdDelete_Click" />
                &nbsp;<asp:Button ID="btnAdd" runat="server" Text="Clone" OnClick="cmdAdd_Click" />
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="cmdSave_Click" />
                <asp:TextBox onkeypress="return ProcessKeyPress(event)" ID="txtID" runat="server"
                    Columns="5">0</asp:TextBox>
            </span>
        </td>
    </tr>
</table>

<script type="text/javascript">

    $(document).ready(function() {
        $(".dateRegion").each(function(i) {
            $(this).datepicker({
                changeMonth: true,
                changeYear: true,
                showOn: 'button',
                buttonImage: '/manage/images/calendar.gif',
                buttonImageOnly: true,

                popupContainer: '#<%=divDatePicker.ClientID%>',
                altField: '#<%=lblDate.ClientID%>',
                altFormat: 'DD,  MM d, yy',
                constrainInput: true
            });
        });
    });
	
</script>

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

</asp:Content>