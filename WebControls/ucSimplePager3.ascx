<%@ Control Language="C#" %>
<asp:Repeater ID="rpPager" runat="server">
	<HeaderTemplate>
		<div class="pagerfooterlinks">
	</HeaderTemplate>
	<ItemTemplate>
		<carrot:ListItemWrapperForPager HtmlTagName="div" ID="wrap" runat="server" CSSSelected="selectedwrap" CssClassNormal="pagerlink">
			<carrot:NavLinkForPagerTemplate ID="lnkBtn" CSSSelected="selected" runat="server" RenderAsHyperlink="true" />
		</carrot:ListItemWrapperForPager>
	</ItemTemplate>
	<FooterTemplate>
		</div>
	</FooterTemplate>
</asp:Repeater>