<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryPrettyPhoto.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryPrettyPhoto" %>
<%@ Register TagPrefix="carrot" Namespace="Carrotware.CMS.UI.Plugins.PhotoGallery" Assembly="Carrotware.CMS.UI.Plugins.PhotoGallery" %>
<div style="clear: both;">
</div>
<div>
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
						<a target="_blank" href="<%# String.Format("{0}", Eval("GalleryImage"))  %>" rel="prettyPhoto[<%=this.ClientID %>]" title="<%# String.Format("{0}", Eval("GalleryImage"))  %>">
							<carrot:ImageSizer runat="server" ID="ImageSizer1" ImageUrl='<%# Eval("GalleryImage")  %>' ThumbSize='<%# ThumbSize %>' ScaleImage='<%# ScaleImage %>'
								ToolTip="" />
						</a>
					</ItemTemplate>
				</asp:Repeater>
			</div>
			<asp:Panel ID="pnlScript" runat="server">
				<script type="text/javascript">
			function InitPrettyPhoto<%=pnlGallery.ClientID %>() {
				$("#<%=pnlGallery.ClientID %> a[rel^='prettyPhoto']").prettyPhoto( { theme:'<%=PrettyPhotoSkin %>', social_tools : '' } );
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
