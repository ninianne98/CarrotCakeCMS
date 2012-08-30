<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarDateInfo.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.CalendarModule.CalendarDateInfo" %>
<div class="calendarUpcomingList" style="padding: 25px;">
	<h2 class="calendarUpcoming eventTitle">
		<%=theEventDate.ToLongDateString() %></h2>
	<p>
	</p>
	<asp:Repeater ID="rEvents" runat="server">
		<ItemTemplate>
			<div class="calendarUpcoming eventListing">
				<div class="calendarEventTitle">
					<b>
						<asp:Label ID="lblEvent" runat="server" Text='<%# String.Format( "{0}", Eval("EventTitle") ) %>'>
						</asp:Label></b>
				</div>
				<div class="calendarEventBody">
					<asp:Literal ID="lblDetail" runat="server" Text='<%# String.Format( "{0}", Eval("EventDetail") ) %>' />
					<br />
				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>
