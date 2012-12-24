<%@ Control Language="C#" %>
<carrot:jsHelperLib runat="server" ID="jsHelperLib1" />
<div class="search-form-outer" id="frmSearch" runat="server">
	<div class="search-form-text">
		<asp:TextBox ID="SearchText" runat="server" Columns="16" MaxLength="40" />
	</div>
	<div class="search-form-button">
		<asp:Button ID="btnSiteSearch" runat="server" Text="Search" />
	</div>
</div>
