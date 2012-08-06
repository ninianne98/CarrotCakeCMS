<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FAQDisplay.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.FAQModule.FAQDisplay" %>
<div style="clear: both;">
</div>
<div>
	<asp:DataGrid ID="dgFAQ" runat="server" AutoGenerateColumns="False" GridLines="None" CellPadding="2" CellSpacing="0" ShowHeader="False">
		<Columns>
			<asp:TemplateColumn>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
				<ItemTemplate>
					<br />
					<h2>
						<asp:Literal ID="Literal1" runat="server" Text='<%# FaqCounter() %>'>
						</asp:Literal></h2>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<ItemStyle VerticalAlign="Top"></ItemStyle>
				<ItemTemplate>
					<div style="clear: both; float: left; width: 30px; font-weight: bold;">
						<p>
							Q:</p>
					</div>
					<div style="float: left; width: 500px;">
						<asp:Literal ID="Literal2" runat="server" Text='<%# String.Format( "{0}", DataBinder.Eval(Container, "DataItem.question") ) %>'>
						</asp:Literal>
					</div>
					<div style="clear: both; float: left; width: 30px; font-weight: bold;">
						<p>
							A:</p>
					</div>
					<div style="float: left; width: 500px;">
						<asp:Literal ID="Literal3" runat="server" Text='<%# String.Format( "{0}", DataBinder.Eval(Container, "DataItem.answer") ) %>'>
						</asp:Literal>
					</div>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</div>
<div style="clear: both;">
</div>
