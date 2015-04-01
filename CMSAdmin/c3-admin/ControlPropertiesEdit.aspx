<%@ Page Title="Edit Properties" ValidateRequest="false" Language="C#" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="ControlPropertiesEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.ControlPropertiesEdit" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Edit Properties
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:Repeater ID="rpProps" runat="server" OnItemDataBound="rpProps_Bind">
		<ItemTemplate>
			<div style="padding-bottom: 5px;">
				<div style="float: left; padding-right: 10px;">
					<b>
						<%# String.Format("{0}", Eval("FieldDescription"))%></b>
				</div>
				<div style="clear: both;">
				</div>
			</div>
			<div style="padding-bottom: 20px;">
				<div style="float: left; padding-right: 50px;">
					<%# String.Format("{0}", Eval("Name"))%>
				</div>
				<div style="float: left;">
					<asp:DropDownList ID="ddlValue" runat="server" Visible="false" />
					<asp:CheckBoxList ID="chkValues" runat="server" Visible="false" />
					<asp:CheckBox ID="chkValue" runat="server" Visible="false" />
					<asp:TextBox ID="txtValue" Width="400px" runat="server" Text='<%# GetSavedValue( String.Format( "{0}", Eval("DefValue")), String.Format( "{0}", Eval("Name")) ) %>' />
					<asp:HiddenField runat="server" ID="hdnName" Value='<%# String.Format( "{0}", Eval("Name") ) %>' />
				</div>
				<div style="clear: both;">
				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
	<br />
	<div style="margin-top: 25px;">
		<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Apply" />
		<br />
	</div>
	<div style="display: none;">
		<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Apply" />
	</div>
	<script type="text/javascript">
		function SubmitPage() {
			var ret = tinyMCE.triggerSave();
			setTimeout("ClickBtn();", 800);
		}
		function ClickBtn() {
			$('#<%=btnSave.ClientID %>').click();
		}
	</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
