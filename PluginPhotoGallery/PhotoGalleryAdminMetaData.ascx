<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryAdminMetaData.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryAdminMetaData" %>
<h2>
	Photo Gallery : Image Meta Data</h2>
<p>
	<asp:Literal ID="litImgName" runat="server" /><br />
	<img src="/carrotwarethumb.axd?thumb=<%= HttpUtility.UrlEncode(sImageFile) %>&scale=true&square=100" alt="<%=sImageFile %>" title="<%=sImageFile %>" />
</p>
<p>
	<b>Title</b><br />
	<asp:TextBox ID="txtTitle" runat="server" MaxLength="200" Style="width: 680px;"></asp:TextBox>
</p>
<p>
	<b>Details</b><br />
	<a href="javascript:cmsToggleTinyMCE('<%= txtMetaInfo.ClientID %>');">Show/Hide Editor</a>
	<asp:TextBox Style="height: 200px; width: 680px;" CssClass="mceEditor" ID="txtMetaInfo" runat="server" TextMode="MultiLine" Rows="12" Columns="80" />
</p>
<br />
<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Save" />
<br />
<div style="display: none;">
	<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
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
