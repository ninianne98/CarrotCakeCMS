<%@ Page ValidateRequest="false" Title="Content Edit" Language="C#" MasterPageFile="~/Manage/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="ContentEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.ContentEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Edit Content
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div style="height: 350px; margin-bottom: 10px;">
		<div runat="server" id="divCenter">
			<a href="javascript:cmsToggleTinyMCE('<%= reBody.ClientID %>');">Show/Hide Editor</a></div>
		<asp:TextBox ValidationGroup="inputForm" Style="height: 300px; width: 700px;" CssClass="mceEditor" ID="reBody" runat="server" TextMode="MultiLine" Rows="20"
			Columns="90" />
		<br />
	</div>
	<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Save" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<%-- <input type="button" id="btnCancel" value="Cancel" onclick="location.href='./PageIndex.aspx';" />--%>
	<br />
	<div style="display: none;">
		<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
	</div>
	<asp:Panel ID="pnlRichEdit" runat="server" Visible="false">
		<script type="text/javascript">

			function AutoSynchMCE() {
				if (saving != 1) {
					tinyMCE.triggerSave();
					setTimeout("AutoSynchMCE();", 3000);
				}
			}

			AutoSynchMCE();

			var saving = 0;

			function SubmitPage() {
				saving = 1;
				tinyMCE.triggerSave();
				setTimeout("ClickBtn();", 1500);
			}
			function ClickBtn() {
				$('#<%=btnSave.ClientID %>').click();
			}
		</script>
	</asp:Panel>
	<asp:Panel ID="pnlPlain" runat="server" Visible="false">
		<script type="text/javascript">
			function SubmitPage() {
				setTimeout("ClickBtn();", 500);
			}
			function ClickBtn() {
				$('#<%=btnSave.ClientID %>').click();
			}
		</script>
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxBodyContentPlaceHolder" runat="server">
</asp:Content>
