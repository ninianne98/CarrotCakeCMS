<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryPrettyPhotoContent.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryPrettyPhotoContent" %>
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
			<carrot:PrettyPhoto runat="server" ID="PrettyPhoto1" />
			<div class="prettyphotoGallery  gallery">
				<asp:Repeater ID="rpGallery" runat="server">
					<ItemTemplate>
						<a href="#<%# CtrlId %>" rel="prettyPhoto[<%=this.ClientID %>]" title="<%# String.Format("{0}", Eval("GalleryImage"))  %>">
							<img src="/carrotwarethumb.axd?scale=<%# GetScale()%>&thumb=<%# HttpUtility.UrlEncode(String.Format("{0}", Eval("GalleryImage")))  %>&square=<%# GetThumbSize() %>"
								alt="" /></a>
					</ItemTemplate>
				</asp:Repeater>
				<asp:Repeater ID="rpGalleryDetail" runat="server">
					<ItemTemplate>
						<div id="<%# CtrlId %>" style="display: none;">
							<div style="clear: both;">
								<b>
									<%# GetImageTitle(Eval("GalleryImage").ToString())%></b>
							</div>
							<div style="border: solid 0px #000000; padding: 4px; width: <%# GetWindowWidth() %>px; height: 500px; overflow: auto;">
								<div class="gallery" style="margin: 0 auto 20px; padding: 0; text-align: center; clear: both;">
									<a target="_blank" href="<%# String.Format("{0}", Eval("GalleryImage"))  %>" rel="prettyPhoto[<%=this.ClientID %>_2]" title="<%# String.Format("{0}", Eval("GalleryImage"))  %>">
										<img src="/carrotwarethumb.axd?scale=<%# GetScale()%>&thumb=<%# HttpUtility.UrlEncode(String.Format("{0}", Eval("GalleryImage")))  %>&square=<%# GetThumbSize2() %>"
											alt="" style="margin: 0 auto 20px; padding: 0; text-align: center;" /></a>
								</div>
								<div style="clear: both;">
									<%# GetImageBody(Eval("GalleryImage").ToString())%>
								</div>
							</div>
						</div>
					</ItemTemplate>
				</asp:Repeater>
			</div>
			<asp:Panel ID="pnlScript" runat="server">
				<script type="text/javascript">
			function InitPrettyPhoto<%=pnlGallery.ClientID %>() {
				$("#<%=pnlGallery.ClientID %> a[rel^='prettyPhoto']").prettyPhoto( { theme:'<%=PrettyPhotoSkin %>', default_width: <%=GetWindowWidth() %>, social_tools : '' } );
			}

			$(document).ready(function() {
				InitPrettyPhoto<%=pnlGallery.ClientID %>();
			});
				</script>
			</asp:Panel>
		</asp:Panel>
	</div>
</div>
<div style="clear: both;">
</div>
