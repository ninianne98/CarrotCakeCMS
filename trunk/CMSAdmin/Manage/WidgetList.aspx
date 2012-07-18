<%@ Page Title="Widget List" Language="C#" MasterPageFile="~/Manage/MasterPages/MainPopup.Master" AutoEventWireup="true"
	CodeBehind="WidgetList.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.WidgetList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Widget List
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div id="SortableGrid">
		<asp:GridView CssClass="datatable" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<Columns>
				<asp:TemplateField>
					<HeaderTemplate>
						Control Path
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("ControlPath")%>
						<asp:HiddenField ID="hdnActive" runat="server" Value='<%# Eval("IsWidgetActive")%>' Visible="false" />
						<asp:HiddenField ID="hdnDelete" runat="server" Value='<%# Eval("IsWidgetPendingDelete")%>' Visible="false" />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						Active
					</HeaderTemplate>
					<ItemTemplate>
						<asp:Button CommandName='<%#String.Format("restore_{0}", Eval("Root_WidgetID")) %>' ID="btnRestore" runat="server" Text="Show"
							OnCommand="ClickAction" Visible="false" />
						<asp:Button CommandName='<%#String.Format("remove_{0}", Eval("Root_WidgetID")) %>' ID="btnRemove" runat="server" Text="Hide"
							OnCommand="ClickAction" Visible="false" />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						Pending Delete
					</HeaderTemplate>
					<ItemTemplate>
						<asp:Button CommandName='<%#String.Format("cancel_{0}", Eval("Root_WidgetID")) %>' ID="btnCancel" runat="server" Text="Cancel Deletion"
							OnCommand="ClickAction" Visible="false" />
						<asp:Button CommandName='<%#String.Format("delete_{0}", Eval("Root_WidgetID")) %>' ID="btnDelete" runat="server" Text="Flag For Deletion"
							OnCommand="ClickAction" Visible="false" />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						Edit Date
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("EditDate")%>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxBodyContentPlaceHolder" runat="server">
</asp:Content>
