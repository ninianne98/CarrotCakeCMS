<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryAdminExport.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryAdminExport" %>
<h2>
	Photo Gallery Export
</h2>
<br />
<div id="SortableGrid">
	<carrot:CarrotGridView DefaultSort="GalleryTitle ASC" CssClass="datatable" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
		AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
		<Columns>
			<asp:TemplateField ItemStyle-HorizontalAlign="Center">
				<HeaderTemplate>
					Select
				</HeaderTemplate>
				<ItemTemplate>
					<asp:CheckBox ID="chkSelect" runat="server" />
					<asp:HiddenField ID="hdnGalleryID" runat="server" Value='<%# Eval("GalleryID") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField HeaderText="Gallery" DataField="GalleryTitle" />
		</Columns>
	</carrot:CarrotGridView>
</div>
<p>
	<asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" /><br />
</p>
