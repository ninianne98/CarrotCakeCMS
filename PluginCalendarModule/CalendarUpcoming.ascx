<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarUpcoming.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.CalendarModule.CalendarUpcoming" %>
<center>
<div class="calendarUpcomingTable" style="width: 500px; padding: 25px;">
	<asp:DataGrid ID="dgEvents" runat="server" CellPadding="2" ShowHeader="False" GridLines="None" AutoGenerateColumns="False">
		<Columns>
			<asp:TemplateColumn ItemStyle-CssClass="calendarUpcoming dateColumn" HeaderText="EventDate">
				<ItemStyle Width="80px" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
				<ItemTemplate>
					<asp:Literal ID="litDate" runat="server" Text='<%# String.Format( "{0:d}", Eval("EventDate") ) %>' />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="EventDate">
				<ItemTemplate>
					&nbsp;&nbsp;
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn ItemStyle-CssClass="calendarUpcoming eventColumn" HeaderText="EventDate">
				<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
				<ItemTemplate>
					<div class="calendarEventTitle">
						<b>
							<asp:Literal ID="litEvent" runat="server" Text='<%# String.Format( "{0}", Eval("EventTitle") ) %>' />
						 </b>
					</div>
					<div class="calendarEventBody">
						<asp:Literal ID="litDetail" runat="server" Text='<%# String.Format( "{0}", Eval("EventDetail") ) %>' />
						<br />
					</div>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</div>
</center>