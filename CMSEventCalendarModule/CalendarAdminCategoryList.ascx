<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarAdminCategoryList.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.EventCalendarModule.CalendarAdminCategoryList" %>
<h2>
	Category List
</h2>
<p>
	<a href="<%= CreateLink("EventAdminCategoryDetail") %>">
		<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add" />
		Add Category</a>
</p>
<div class="SortableGrid">
	<carrot:CarrotGridView CssClass="datatable" DefaultSort="CategoryName asc" ID="dgMenu" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
		AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
		<EmptyDataTemplate>
			<p>
				<b>No categories found.</b>
			</p>
		</EmptyDataTemplate>
		<Columns>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%# CreateLink("EventAdminCategoryDetail", String.Format("id={0}", Eval("CalendarEventCategoryID") )) %>'>
						<img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" /></asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<carrot:CarrotHeaderSortTemplateField SortExpression="CategoryName" HeaderText="Name" DataField="CategoryName" />
			<asp:TemplateField>
				<ItemTemplate>
					<div style="border: solid 1px <%# Eval("CategoryBGColor") %>; background-color: <%# Eval("CategoryFGColor") %>; padding: 1px;">
						<div style="border: solid 2px <%# Eval("CategoryFGColor") %>; color: <%# Eval("CategoryFGColor") %>; background-color: <%# Eval("CategoryBGColor") %>;
							padding: 5px; margin: 1px;">
							<%# Eval("CategoryFGColor") %>,
							<%# Eval("CategoryBGColor") %>
						</div>
					</div>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</carrot:CarrotGridView>
</div>
