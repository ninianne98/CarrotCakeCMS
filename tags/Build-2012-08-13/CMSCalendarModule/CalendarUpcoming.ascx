<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarUpcoming.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.CalendarModule.CalendarUpcoming" %>

<center>
	<div style="width: 500px; padding: 25px;">
		<asp:DataGrid ID="dgEvents" runat="server" CellPadding="2" ShowHeader="False" GridLines="None" AutoGenerateColumns="False">
			<ItemStyle></ItemStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="EventDate">
					<ItemStyle Width="80px" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
					<ItemTemplate>
						<asp:Label ID="lblDate" runat="server" Text='<%# String.Format( "{0:d}", DataBinder.Eval(Container, "DataItem.EventDate") ) %>'>
						</asp:Label>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="EventDate">
					<ItemTemplate>
						&nbsp;&nbsp;
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="EventDate">
					<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
					<ItemTemplate>
						<b>
							<asp:Label ID="lblEvent" runat="server" Text='<%# String.Format( "{0}", DataBinder.Eval(Container, "DataItem.EventTitle") ) %>'>
							</asp:Label></b><br />
						<asp:Label ID="lblDetail" runat="server" Text='<%# String.Format( "{0}<br /><br />", DataBinder.Eval(Container, "DataItem.EventDetail") ) %>'>
						</asp:Label>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:DataGrid>
	</div>
</center>
