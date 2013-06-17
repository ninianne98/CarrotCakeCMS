<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryAdminGalleryList.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryAdminGalleryList" %>
<h2>
	Photo Gallery : Category List</h2>
<br />
<div id="SortableGrid">
	<carrot:CarrotGridView DefaultSort="GalleryTitle ASC" CssClass="datatable" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
		AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
		<EmptyDataTemplate>
			<p>
				<b>No galleries found.</b>
			</p>
		</EmptyDataTemplate>
		<Columns>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink ID="lnkedit1" runat="server" NavigateUrl='<%#CreateLink("GalleryEdit", String.Format("id={0}", Eval("GalleryID")) ) %>'>
						<img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" />
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink ID="lnkedit2" runat="server" NavigateUrl='<%#CreateLink("EditGalleryImageList", String.Format("id={0}", Eval("GalleryID")) ) %>'>
						<img class="imgNoBorder" src="/c3-admin/images/image.png" alt="Edit Contents" title="Edit Contents" />
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<carrot:CarrotHeaderSortTemplateField SortExpression="GalleryTitle" HeaderText="Gallery">
				<ItemTemplate>
					<%# Eval("GalleryTitle")%>
				</ItemTemplate>
			</carrot:CarrotHeaderSortTemplateField>
		</Columns>
	</carrot:CarrotGridView>
</div>
