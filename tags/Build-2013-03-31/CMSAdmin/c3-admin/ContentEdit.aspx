<%@ Page ValidateRequest="false" Title="Content Edit" Language="C#" MasterPageFile="~/c3-admin/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="ContentEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.ContentEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Edit Content
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div style="margin-bottom: 25px;">
		<div runat="server" id="divCenter">
			<a href="javascript:cmsToggleTinyMCE('<%= reBody.ClientID %>');">Show/Hide Editor</a></div>
		<asp:TextBox ValidationGroup="inputForm" Style="height: 300px; width: 800px;" CssClass="mceEditor" ID="reBody" runat="server" TextMode="MultiLine" Rows="20"
			Columns="100" />
		<br />
	</div>
	<div style="margin-top: 25px;">
		<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Apply" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		<br />
	</div>
	<div style="display: none;">
		<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Apply" />
	</div>
	<asp:Panel ID="pnlRichEdit" runat="server" Visible="false">
		<script type="text/javascript">
			function SubmitPage() {
				var ret = tinyMCE.triggerSave();
				setTimeout("ClickBtn();", 500);
			}
			function ClickBtn() {
				$('#<%=btnSave.ClientID %>').click();
			}
		</script>
	</asp:Panel>
	<asp:Panel ID="pnlPlain" runat="server" Visible="false">
		<script type="text/javascript">
			function SubmitPage() {
				setTimeout("ClickBtn();", 250);
			}
			function ClickBtn() {
				$('#<%=btnSave.ClientID %>').click();
			}
		</script>
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
