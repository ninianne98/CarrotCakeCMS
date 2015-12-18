<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminList.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.FAQ2Module.AdminList" %>
<h2>
	FAQs : List
	"<asp:Literal ID="litFaqName" runat="server" />"
</h2>
<br />
<p>
	<asp:HyperLink ID="lnkAdd" runat="server"> 
	<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add" />
	Add Entry</asp:HyperLink>
</p>
<div id="SortableGrid">
	<carrot:CarrotGridView DefaultSort="ItemOrder ASC" CssClass="datatable" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
		AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
		<EmptyDataTemplate>
			<p>
				<b>No FAQs found.</b>
			</p>
		</EmptyDataTemplate>
		<Columns>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink ID="lnkedit1" runat="server" NavigateUrl='<%#CreateLink("FAQDetail", String.Format("id={0}", Eval("FaqItemID")) ) %>'>
						<img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" />
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<carrot:CarrotHeaderSortTemplateField SortExpression="ItemOrder" HeaderText="Item Order">
				<ItemTemplate>
					<%# Eval("ItemOrder")%>
				</ItemTemplate>
			</carrot:CarrotHeaderSortTemplateField>
			<carrot:CarrotHeaderSortTemplateField SortExpression="Caption" HeaderText="Caption">
				<ItemTemplate>
					<%# Eval("Caption")%>
				</ItemTemplate>
			</carrot:CarrotHeaderSortTemplateField>
			<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" HeaderText="Is Active" AlternateTextFalse="Inactive"
				AlternateTextTrue="Active" ShowBooleanImage="true" />
		</Columns>
	</carrot:CarrotGridView>
</div>
