<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryAdminImport.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryAdminImport" %>
<h2>
	Photo Gallery Import
</h2>
<br />
<asp:Panel runat="server" ID="pnlUpload">
	<p>
		<asp:Label ID="lblWarning" runat="server" />
	</p>
	<p>
		Select a previously exported content to import:<br />
		<asp:FileUpload ID="upFile" runat="server" />
	</p>
	<p>
		You will be able to review the imported data before finalizing changes.
	</p>
	<p>
		<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /><br />
	</p>
</asp:Panel>
<asp:Panel runat="server" ID="pnlReview">
	<p>
		<asp:Button ID="btnCreate" runat="server" Text="Import Selected" OnClick="btnCreate_Click" /><br />
	</p>
	<p>
		<asp:Label ID="lblPages" runat="server" Text="Label" />
		records
	</p>
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
</asp:Panel>
