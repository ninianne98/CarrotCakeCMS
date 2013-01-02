<%@ Control Language="C#" %>
<asp:Repeater ID="rpPager" runat="server">
	<HeaderTemplate>
		<br />
		<div class="datafooter ui-helper-clearfix ui-widget-content ui-corner-all ui-corner-all">
	</HeaderTemplate>
	<ItemTemplate>
		<carrot:ListItemWrapperForPager HtmlTagName="div" ID="wrap" runat="server" CSSSelected="sel-pagerlink ui-state-active" CssClassNormal="pagerlink ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">
			<carrot:NavLinkForPagerTemplate ID="lnkBtn" runat="server" CSSSelected="selected" />
		</carrot:ListItemWrapperForPager>
	</ItemTemplate>
	<FooterTemplate>
		</div>
		<br />
	</FooterTemplate>
</asp:Repeater>
