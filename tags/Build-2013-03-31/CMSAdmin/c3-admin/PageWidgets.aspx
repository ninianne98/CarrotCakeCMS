<%@ Page Title="PageWidgets" Language="C#" MasterPageFile="~/c3-admin/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="PageWidgets.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.PageWidgets" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Page Widgets
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" /><br />
	</p>
	<div id="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="WidgetOrder ASC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<EmptyDataTemplate>
				<p>
					<b>No widgets found.</b>
				</p>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:CheckBox runat="server" ID="chkContent" value='<%# Eval("Root_WidgetID") %>' Checked='<%# Eval("IsWidgetActive") %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Status" DataField="IsWidgetActive" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
					ShowBooleanImage="true" />
				<asp:BoundField HeaderText="Control Path" DataField="ControlPath" />
				<asp:BoundField HeaderText="Edit Date" DataField="EditDate" />
				<asp:BoundField HeaderText="PlaceholderName" DataField="PlaceholderName" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
