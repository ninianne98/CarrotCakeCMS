<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryAdminCategoryList.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryAdminCategoryList" %>
<h2>
	Photo Gallery : Category List</h2>
<br />
<div id="SortableGrid">
	<carrot:CarrotGridView DefaultSort="GalleryTitle  ASC" CssClass="datatable" ID="gvPages" runat="server" AutoGenerateColumns="false"
		HeaderStyle-CssClass="tablehead" AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
		<Columns>
			<asp:TemplateField>
				<ItemStyle Width="20px" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
				<ItemTemplate>
					<asp:HyperLink ID="lnkedit1" runat="server" NavigateUrl='<%#CreateLink("CategoryEdit", String.Format("id={0}", DataBinder.Eval(Container, "DataItem.GalleryID")) ) %>'>
                            <img border="0" src="/Manage/images/pencil.png" alt="Edit" title="Edit" style="margin-right: 10px;" />
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Width="20px" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
				<ItemTemplate>
					<asp:HyperLink ID="lnkedit2" runat="server" NavigateUrl='<%#CreateLink("EditGallery", String.Format("id={0}", DataBinder.Eval(Container, "DataItem.GalleryID")) ) %>'>
                            <img border="0" src="/Manage/images/image.png" alt="Edit Contents" title="Edit Contents" style="margin-right: 10px;" />
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderTemplate>
					<asp:LinkButton ID="lnkHead1" runat="server" CommandName="GalleryTitle">Gallery</asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
					<%# Eval("GalleryTitle")%>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</carrot:CarrotGridView>
</div>
