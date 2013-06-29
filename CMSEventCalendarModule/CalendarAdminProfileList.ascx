<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarAdminProfileList.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.EventCalendarModule.CalendarAdminProfileList" %>
<h2>
	Event Profiles
</h2>
<p>
	<a href="<%= CreateLink("EventAdminDetail") %>">
		<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add" />
		Add Event</a>
</p>
<div class="SortableGrid">
	<carrot:CarrotGridView CssClass="datatable" DefaultSort="EventStartDate DESC" ID="dgEvents" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
		AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
		<EmptyDataTemplate>
			<p>
				<b>No event profiles found.</b>
			</p>
		</EmptyDataTemplate>
		<Columns>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink runat="server" ID="lnkEdit1" NavigateUrl='<%# CreateLink("EventAdminDetail", String.Format("id={0}", Eval("CalendarEventProfileID") )) %>'>
						<img class="imgNoBorder" src="/c3-admin/images/calendar.png" alt="Edit event profile" title="Edit event profile" /></asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<carrot:CarrotHeaderSortTemplateField SortExpression="EventTitle" HeaderText="Title" DataField="EventTitle" />
			<carrot:CarrotHeaderSortTemplateField SortExpression="FrequencyName" HeaderText="Recurrs" DataField="FrequencyName" />
			<carrot:CarrotHeaderSortTemplateField SortExpression="CategoryName" HeaderText="Category" DataField="CategoryName" />
			<carrot:CarrotHeaderSortTemplateField SortExpression="EventStartDate" HeaderText="Start Date" DataField="EventStartDate" DataFieldFormat="{0:d}" />
			<carrot:CarrotHeaderSortTemplateField SortExpression="EventEndDate" HeaderText="End By Date" DataField="EventEndDate" DataFieldFormat="{0:d}" />
			<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsCancelled" HeaderText="Cancelled" ShowBooleanImage="true" AlternateTextTrue="Yes"
				AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/lightbulb_off.png" ImagePathFalse="/c3-admin/images/lightbulb.png" />
			<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsPublic" HeaderText="Public" ShowBooleanImage="true" AlternateTextTrue="Yes"
				AlternateTextFalse="No" />
		</Columns>
	</carrot:CarrotGridView>
</div>
