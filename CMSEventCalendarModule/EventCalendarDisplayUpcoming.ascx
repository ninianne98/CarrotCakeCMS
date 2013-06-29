<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventCalendarDisplayUpcoming.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.EventCalendarModule.EventCalendarDisplayUpcoming" %>
<div style="min-width: 75px;">
	<div style="text-align: left;">
		<div style="clear: both;">
		</div>
		<div style="width: 96%;">
			<asp:Repeater ID="rpDates" runat="server">
				<ItemTemplate>
					<div style="text-align: center;">
						<h2>
							<asp:Literal ID="litDate" runat="server" Text='<%# GetDateNameString( (DateTime)Eval("EventDate"), " {0:dddd MMMM d, yyyy} " ) %>' />
						</h2>
					</div>
					<div style="padding: 4px; text-align: left;">
						<div style="border: solid 1px <%# Eval("CategoryBGColor") %>; background-color: <%# Eval("CategoryFGColor") %>; padding: 1px;">
							<div style="border: solid 2px <%# Eval("CategoryFGColor") %>; color: <%# Eval("CategoryFGColor") %>; background-color: <%# Eval("CategoryBGColor") %>;
								padding: 5px; margin: 1px; text-align: left;">
								<b>
									<asp:Literal ID="litEvent" runat="server" Text='<%# String.Format( " {0} ", Eval("EventTitle") ) %>' />
									<%--<asp:Literal ID="litDate" runat="server" Text='<%# String.Format(" {0:d} ", Eval("EventDate") ) %>' />--%>
									<asp:Literal ID="litSTime" runat="server" Text='<%# String.Format(" at {0:h:mm tt} ", GetTimeFromTimeSpan( (TimeSpan?)Eval("EventStartTime")) ) %>' Visible='<%# !(bool)Eval("IsAllDayEvent") %>' />
									<asp:Literal ID="litETime" runat="server" Text='<%# String.Format(" - {0:h:mm tt} ", GetTimeFromTimeSpan( (TimeSpan?) Eval("EventEndTime")) ) %>' Visible='<%# !(bool)Eval("IsAllDayEvent") && (Eval("EventEndTime") != null) %>' />
								</b>
							</div>
						</div>
						<%--
						<div>
							<asp:Literal ID="litDetail1" runat="server" Text='<%# String.Format( "{0}", DataBinder.Eval(Container, "DataItem.EventSeriesDetail") ) %>' />
						</div>
						<div>
							<asp:Literal ID="litDetail2" runat="server" Text='<%# String.Format( "{0}", DataBinder.Eval(Container, "DataItem.EventDetail") ) %>' />
						</div>
						<br />
						--%>
						<div style="clear: both;">
						</div>
					</div>
				</ItemTemplate>
			</asp:Repeater>
		</div>
	</div>
	<asp:PlaceHolder ID="phLink" runat="server">
		<div>
			<p style="text-align: center; font-weight: bold;">
				<asp:HyperLink ID="lnkHyper" runat="server" NavigateUrl="#"> 
				View the Calendar</asp:HyperLink>
			</p>
		</div>
	</asp:PlaceHolder>
	<div style="clear: both;">
	</div>
</div>
