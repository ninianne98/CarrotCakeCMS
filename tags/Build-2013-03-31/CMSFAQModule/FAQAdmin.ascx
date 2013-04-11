<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FAQAdmin.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.FAQModule.FAQAdmin" %>
<asp:DataGrid ID="dgMenu" runat="server" ShowHeader="False" AutoGenerateColumns="False" GridLines="None">
	<Columns>
		<asp:TemplateColumn>
			<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
			<ItemTemplate>
				<asp:HyperLink ID="lnkedit" runat="server" NavigateUrl='<%#CreateLink("FAQAdminAddEdit", String.Format("id={0}", DataBinder.Eval(Container, "DataItem.FaqID") )) %>'>
                      <img border="0" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" style="margin-right: 20px;" />
				</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="tag">
			<HeaderStyle HorizontalAlign="Center" VerticalAlign="Bottom"></HeaderStyle>
			<ItemStyle VerticalAlign="Top"></ItemStyle>
			<ItemTemplate>
				<asp:Label ID="lblQuestion" runat="server" Text='<%# String.Format( " {0}", DataBinder.Eval(Container, "DataItem.Question") ) %>'>
				</asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>
