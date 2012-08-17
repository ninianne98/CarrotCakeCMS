<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryAdminMetaData.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryAdminMetaData" %>
<h2>
	Photo Gallery : Image Meta Data</h2>
<p>
	<asp:Literal ID="litImgName" runat="server"></asp:Literal>
</p>
<a href="javascript:cmsToggleTinyMCE('<%= txtMetaInfo.ClientID %>');">Show/Hide Editor</a></div>
<asp:TextBox Style="height: 280px; width: 680px;" CssClass="mceEditor" ID="txtMetaInfo" runat="server" TextMode="MultiLine" Rows="15" Columns="80" />
<br />
<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Apply" />
<br />
<div style="display: none;">
	<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Apply" />
</div>
<script type="text/javascript">
	function SubmitPage() {
		var ret = tinyMCE.triggerSave();
		setTimeout("ClickBtn();", 500);
	}
	function ClickBtn() {
		$('#<%=btnSave.ClientID %>').click();
	}
</script>
