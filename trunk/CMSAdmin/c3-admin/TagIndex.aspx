<%@ Page Title="TagIndex" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="TagIndex.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.TagIndex" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Tag Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<a href="./TagAddEdit.aspx">
			<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add" />
			Add Tag</a>
	</p>
	<p>
		<br />
	</p>
	<div id="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="TagText asc" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%# String.Format("./TagAddEdit.aspx?id={0}", Eval("ContentTagID")) %>'><img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="TagText" HeaderText="Text" DataField="TagText" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="TagSlug" HeaderText="Slug" DataField="TagSlug" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="UseCount" HeaderText="Count" DataField="UseCount" ItemStyle-HorizontalAlign="Center" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
