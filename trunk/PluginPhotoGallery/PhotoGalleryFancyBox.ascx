<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryFancyBox.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryFancyBox" %>
<%@ Register TagPrefix="carrot" Namespace="Carrotware.CMS.UI.Plugins.PhotoGallery" Assembly="Carrotware.CMS.UI.Plugins.PhotoGallery" %>
<div style="clear: both;">
</div>
<div>
	<%--<carrot:jquery runat="server" ID="jquery1" />--%>
	<div runat="server" id="divGallery">
		<asp:Panel ID="pnlGalleryHead" runat="server">
			<h2>
				<asp:Literal ID="litGalleryName" runat="server"></asp:Literal></h2>
		</asp:Panel>
		<asp:Panel ID="pnlGallery" runat="server">
			<carrot:FancyBox runat="server" ID="FancyBox1" />
			<div class="fancyboxGallery">
				<asp:Repeater ID="rpGallery" runat="server">
					<ItemTemplate>
						<a target="_blank" href="<%# String.Format("{0}", Eval("GalleryImage"))  %>" rel="<%=this.ClientID %>" title="<%# String.Format("{0}", Eval("GalleryImage"))  %>">
							<img src="/carrotwarethumb.axd?scale=<%# GetScale()%>&thumb=<%# HttpUtility.UrlEncode(String.Format("{0}", Eval("GalleryImage")))  %>&square=<%# GetThumbSize() %>"
								alt="" /></a>
					</ItemTemplate>
				</asp:Repeater>
			</div>
			<asp:Panel ID="pnlScript" runat="server">
				<script type="text/javascript">
			function InitFancyBox<%=pnlGallery.ClientID %>() {
				$("#<%=pnlGallery.ClientID %> a[rel=<%=this.ClientID %>]").fancybox({
					'transitionIn': 'none',
					'transitionOut': 'none',
					'titlePosition': 'over',
					'titleFormat': function(title, currentArray, currentIndex, currentOpts) {
						return '<span id="fancybox-title-over">Image ' + (currentIndex + 1) + ' / ' + currentArray.length + (title.length ? ' &nbsp; ' + title : '') + '</span>';
					}
				});
			}

			$(document).ready(function() {
				InitFancyBox<%=pnlGallery.ClientID %>();
			});
				</script>
			</asp:Panel>
		</asp:Panel>
	</div>
</div>
<div style="clear: both;">
</div>
