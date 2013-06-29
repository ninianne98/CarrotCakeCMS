<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventCalendarDisplay2.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.EventCalendarModule.EventCalendarDisplay2" %>
<div style="min-width: 380px;">
	<div style="text-align: center;">
		<div style="clear: both;">
		</div>
		<div style="width: 325px; margin: 0 auto; text-align: center; padding: 10px;">
			<div style="padding: 10px;">
				<asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" />
				<asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" />
				<br />
			</div>
			<div>
				<carrot:Calendar runat="server" ID="Calendar1" />
			</div>
		</div>
		<div style="width: 350px; margin: 0 auto; text-align: center; padding-top: 20px;">
			<asp:Button CssClass="calendarbutton" ID="btnLast" runat="server" Text="«««««" OnClick="btnLast_Click" />
			&nbsp;&nbsp;&nbsp;
			<asp:Button CssClass="calendarbutton" ID="btnNext" runat="server" Text="»»»»»" OnClick="btnNext_Click" />
		</div>
		<div style="width: 500px; padding: 25px;">
			<div>
				<asp:Repeater ID="rpEvent" runat="server">
					<ItemTemplate>
						<div style="text-align: center;">
							<h2>
								<asp:Literal ID="litDate" runat="server" Text='<%# GetDateNameString( (DateTime)Eval("EventDate"), " {0:dddd MMMM d, yyyy} " ) %>' />
							</h2>
						</div>
						<div style="text-align: left;">
							<div style="border: solid 1px <%# Eval("CategoryBGColor") %>; background-color: <%# Eval("CategoryFGColor") %>; padding: 1px;">
								<div style="border: solid 2px <%# Eval("CategoryFGColor") %>; color: <%# Eval("CategoryFGColor") %>; background-color: <%# Eval("CategoryBGColor") %>;
									padding: 5px; margin: 1px; min-width: 250px; text-align: left;">
									<b>
										<asp:Literal ID="litEvent" runat="server" Text='<%# String.Format( "{0} ", Eval("EventTitle") ) %>' />
										<asp:Literal ID="litSTime" runat="server" Text='<%# String.Format(" at {0:h:mm tt} ", GetTimeFromTimeSpan( (TimeSpan?)Eval("EventStartTime")) ) %>' Visible='<%# !(bool)Eval("IsAllDayEvent") %>' />
										<asp:Literal ID="litETime" runat="server" Text='<%# String.Format(" - {0:h:mm tt} ", GetTimeFromTimeSpan( (TimeSpan?) Eval("EventEndTime")) ) %>' Visible='<%# !(bool)Eval("IsAllDayEvent") && (Eval("EventEndTime") != null) %>' />
										<asp:Literal ID="litStatus" runat="server" Text=" (CANCELLED) " Visible='<%# (bool)Eval("IsCancelledSeries") || (bool)Eval("IsCancelledEvent") %>' />
									</b>
								</div>
							</div>
							<div>
								<asp:Literal ID="litDetail1" runat="server" Text='<%# String.Format( "{0}", DataBinder.Eval(Container, "DataItem.EventSeriesDetail") ) %>' />
							</div>
							<div>
								<asp:Literal ID="litDetail2" runat="server" Text='<%# String.Format( "{0}", DataBinder.Eval(Container, "DataItem.EventDetail") ) %>' />
							</div>
							<br />
						</div>
					</ItemTemplate>
				</asp:Repeater>
				<asp:PlaceHolder ID="phNone" runat="server">
					<p>
						<b>No events found.</b>
					</p>
				</asp:PlaceHolder>
				<div style="clear: both;">
				</div>
			</div>
			<div style="width: 300px; padding: 10px;">
				<asp:Repeater ID="rpCat" runat="server">
					<ItemTemplate>
						<div style="border: solid 1px <%# Eval("CategoryBGColor") %>; background-color: <%# Eval("CategoryFGColor") %>; padding: 1px;">
							<div style="border: solid 2px <%# Eval("CategoryFGColor") %>; color: <%# Eval("CategoryFGColor") %>; background-color: <%# Eval("CategoryBGColor") %>;
								padding: 5px; margin: 1px;">
								<%# Eval("CategoryName")%>
							</div>
						</div>
					</ItemTemplate>
				</asp:Repeater>
				<div style="clear: both;">
				</div>
			</div>
		</div>
		<div style="clear: both;">
		</div>
	</div>
</div>
<script type="text/javascript">
	function eventCalendarDateLaunch(val) {
		window.open("<%=LaunchURLWindow %>?calendardate=" + val);
	}
</script>
