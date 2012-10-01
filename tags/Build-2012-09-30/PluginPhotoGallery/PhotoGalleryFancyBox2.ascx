<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryFancyBox2.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryFancyBox2" %>
<%@ Register Src="PhotoGalleryFancyBox.ascx" TagName="PhotoGallery" TagPrefix="uc1" %>
<div style="clear: both;">
</div>
<div>
	<asp:Panel ID="pnlGallery" runat="server">
		<asp:Repeater ID="rpGalleries" runat="server">
			<ItemTemplate>
				<uc1:PhotoGallery ID="PhotoGalleryFancyBox1" runat="server" GalleryID='<%# Eval("GalleryID") %>' ThumbSize='<%# ThumbSize %>' ScaleImage='<%# ScaleImage %>'
					ShowHeading='<%# ShowHeading %>' IsBeingEdited='<%# IsBeingEdited %>' SiteID='<%# SiteID %>' RootContentID='<%# RootContentID %>' PageWidgetID='<%# PageWidgetID %>' />
			</ItemTemplate>
		</asp:Repeater>
	</asp:Panel>
</div>
<div style="clear: both;">
</div>
