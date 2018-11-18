<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminCategoryList.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.FAQ2Module.AdminCategoryList" %>
<h2>FAQs : Category List</h2>
<br />
<br />
<p>
	<asp:HyperLink ID="lnkAdd" runat="server">
	<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add" />
	Add Entry</asp:HyperLink>
</p>
<div id="SortableGrid">
	<carrot:CarrotGridView DefaultSort="FAQTitle ASC" CssClass="datatable" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
		AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
		<EmptyDataTemplate>
			<p>
				<b>No FAQs found.</b>
			</p>
		</EmptyDataTemplate>
		<Columns>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink ID="lnkedit1" runat="server" NavigateUrl='<%#CreateLink("CategoryDetail", String.Format("id={0}", Eval("FaqCategoryID")) ) %>'>
						<img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" />
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink ID="lnkedit2" runat="server" NavigateUrl='<%#CreateLink("FAQList", String.Format("id={0}", Eval("FaqCategoryID")) ) %>'>
						<img class="imgNoBorder" src="/c3-admin/images/table.png" alt="Edit Contents" title="Edit Contents" />
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink ID="lnkedit2" runat="server" NavigateUrl='<%#CreateLink("FAQDetail", String.Format("cat={0}", Eval("FaqCategoryID")) ) %>'>
						<img class="imgNoBorder" src="/c3-admin/images/table_go.png" alt="Add Entry" title="Add Entry" />
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<carrot:CarrotHeaderSortTemplateField SortExpression="FAQTitle" HeaderText="FAQ">
				<ItemTemplate>
					<%# Eval("FAQTitle")%>
				</ItemTemplate>
			</carrot:CarrotHeaderSortTemplateField>
		</Columns>
	</carrot:CarrotGridView>
</div>