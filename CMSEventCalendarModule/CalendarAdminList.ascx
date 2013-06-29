<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarAdminList.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.EventCalendarModule.CalendarAdminList" %>
<div style="width: 600px; padding: 25px;">
	<div style="text-align: center;">
		<div style="clear: both;">
		</div>
		<div style="width: 250px; margin: 0 auto; text-align: center;">
			<asp:TextBox CssClass="dateRegion" ID="txtDate" Columns="12" runat="server" AutoPostBack="true" OnTextChanged="txtDate_TextChanged" />
			<br />
			<br />
		</div>
		<div style="width: 325px; margin: 0 auto; text-align: center;">
			<carrot:Calendar runat="server" ID="Calendar1" />
		</div>
		<div style="width: 350px; margin: 0 auto; text-align: center; padding-top: 20px;">
			<asp:Button CssClass="calendarbutton" ID="btnLast" runat="server" Text="«««««" OnClick="btnLast_Click" />
			&nbsp;&nbsp;&nbsp;
			<asp:Button CssClass="calendarbutton" ID="btnNext" runat="server" Text="»»»»»" OnClick="btnNext_Click" />
		</div>
		<div style="clear: both;">
		</div>
	</div>
</div>
<script type="text/javascript">

	function EditEventEntry(proID, evtID) {
		var opts = {
			"Cancel": function () { cmsAlertModalClose(); },
			"Event": function () { clickEventLink(evtID); },
			"Series": function () { clickProfileLink(proID); }
		};

		cmsAlertModalSmallBtns('Edit individual event or entire series/profile?', opts);

		return false;
	}

	function clickProfileLink(proID) {
		location.href = '<%= CreateLink("EventAdminDetail") %>&id=' + proID;
	}
	function clickEventLink(evtID) {
		location.href = '<%= CreateLink("EventAdminDetailSingle") %>&id=' + evtID;
	}

</script>
<div style="width: 650px; padding: 10px;">
	<asp:GridView ID="dgEvents" runat="server" CellPadding="2" ShowHeader="False" GridLines="None" AutoGenerateColumns="False">
		<EmptyDataTemplate>
			<p>
				<b>No events found.</b>
			</p>
		</EmptyDataTemplate>
		<Columns>
			<%--
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink ToolTip="Edit event profile" runat="server" ID="lnkEdit1" NavigateUrl='<%# CreateLink("EventAdminDetail", String.Format("id={0}", Eval("CalendarEventProfileID") )) %>'>
						<img class="imgNoBorder" src="/c3-admin/images/calendar.png" alt="Edit event profile" title="Edit event profile" /> Series</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink ToolTip="Edit event" runat="server" ID="lnkEdit2" NavigateUrl='<%# CreateLink("EventAdminDetailSingle", String.Format("id={0}", Eval("CalendarEventID") )) %>'>
						<img class="imgNoBorder" src="/c3-admin/images/clock_edit.png" alt="Edit event" title="Edit event" /> Event</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			--%>
			<asp:TemplateField>
				<ItemTemplate>
					<a href="javascript:void(0)" onclick="EditEventEntry('<%#Eval("CalendarEventProfileID") %>', '<%#Eval("CalendarEventID") %>');">
						<img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit event" title="Edit event" /></a>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField DataField="FrequencyName" HeaderText="Frequency" />
			<asp:TemplateField HeaderText="EventDate">
				<ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
				<ItemTemplate>
					<div>
						<asp:Literal ID="litDate" runat="server" Text='<%# String.Format("{0:d}", Eval("EventDate") ) %>' />
						<br />
					</div>
					<div>
						<asp:Literal ID="litSTime" runat="server" Text='<%# String.Format(" {0:h:mm tt} ", GetTimeFromTimeSpan( (TimeSpan?)Eval("EventStartTime")) ) %>' Visible='<%# !(bool)Eval("IsAllDayEvent") %>' />
						<asp:Literal ID="litETime" runat="server" Text='<%# String.Format(" - {0:h:mm tt} ", GetTimeFromTimeSpan( (TimeSpan?) Eval("EventEndTime")) ) %>' Visible='<%# !(bool)Eval("IsAllDayEvent") && (Eval("EventEndTime") != null) %>' />
					</div>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:Image ID="imgStatus" runat="server" ImageUrl="/c3-admin/images/clock_stop.png" ToolTip="Cancelled" Visible='<%# (bool)Eval("IsCancelledSeries") || (bool)Eval("IsCancelledEvent") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="EventDate">
				<ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
				<ItemTemplate>
					<div style="border: solid 1px <%# Eval("CategoryBGColor") %>; background-color: <%# Eval("CategoryFGColor") %>; padding: 1px;">
						<div style="border: solid 2px <%# Eval("CategoryFGColor") %>; color: <%# Eval("CategoryFGColor") %>; background-color: <%# Eval("CategoryBGColor") %>;
							padding: 5px; margin: 1px;">
							<b>
								<asp:Literal ID="litEvent" runat="server" Text='<%# String.Format( "{0}", Eval("EventTitle") ) %>' />
								<asp:Literal ID="litStatus1" runat="server" Text=" (SINGLE EVENT CANCELLED) " Visible='<%#  (bool)Eval("IsCancelledEvent") %>' />
								<asp:Literal ID="litStatus2" runat="server" Text=" [EVENT SERIES CANCELLED] " Visible='<%# (bool)Eval("IsCancelledSeries") %>' />
							</b>
						</div>
					</div>
					<div>
						<asp:Literal ID="litDetail1" runat="server" Text='<%# String.Format( "{0}", Eval("EventSeriesDetail") ) %>' />
					</div>
					<div>
						<asp:Literal ID="litDetail2" runat="server" Text='<%# String.Format( "{0}", Eval("EventDetail") ) %>' />
					</div>
					<br />
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</div>
<div style="clear: both;">
</div>
