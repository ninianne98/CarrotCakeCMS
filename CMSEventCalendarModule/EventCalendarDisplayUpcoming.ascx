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
									<asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# !(bool)Eval("IsAllDayEvent")%>'>
										<asp:Literal ID="litSTime1" runat="server" Text='<%# String.Format(" at {0:h:mm tt} ", GetTimeFromTimeSpan( (TimeSpan?)Eval("EventStartTime")) ) %>' Visible='<%# (Eval("EventStartTimeOverride") == null) %>' />
										<asp:Literal ID="litSTime2" runat="server" Text='<%# String.Format(" at {0:h:mm tt} ", GetTimeFromTimeSpan( (TimeSpan?)Eval("EventStartTimeOverride")) ) %>'
											Visible='<%# (Eval("EventStartTimeOverride") != null) %>' />
										<asp:Literal ID="litETime1" runat="server" Text='<%# String.Format(" - {0:h:mm tt} ", GetTimeFromTimeSpan( (TimeSpan?) Eval("EventEndTime")) ) %>' Visible='<%# (Eval("EventEndTime") != null) && (Eval("EventEndTimeOverride") == null) %>' />
										<asp:Literal ID="litETime2" runat="server" Text='<%# String.Format(" - {0:h:mm tt} ", GetTimeFromTimeSpan( (TimeSpan?) Eval("EventEndTimeOverride")) ) %>'
											Visible='<%# (Eval("EventEndTimeOverride") != null) %>' />
									</asp:PlaceHolder>
									<asp:Literal ID="litStatus" runat="server" Text=" (CANCELLED) " Visible='<%# (bool)Eval("IsCancelledSeries") || (bool)Eval("IsCancelledEvent") %>' />
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
