<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarAdmin.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.CalendarModule.CalendarAdmin" %>
<center>
	<carrot:calendar runat="server" id="Calendar1"></carrot:calendar>
	<br />
	<asp:Button CssClass="calendarbutton" ID="btnLast" runat="server" Text="«««««" OnClick="btnLast_Click" />
	&nbsp;&nbsp;&nbsp;
	<asp:Button CssClass="calendarbutton" ID="btnNext" runat="server" Text="»»»»»" OnClick="btnNext_Click" />
	<div class="calendarEventDetail" style="width: 500px; padding: 25px;">
		<asp:DataGrid ID="dgEvents" Width="520px" runat="server" CellPadding="2" ShowHeader="False" GridLines="None" AutoGenerateColumns="False">
			<ItemStyle></ItemStyle>
			<Columns>
				<asp:TemplateColumn>
					<ItemStyle Width="20px" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
					<ItemTemplate>
						<asp:HyperLink ID="lnkedit" runat="server" NavigateUrl='<%#CreateLink("CalendarAdminAddEdit", String.Format("id={0}", DataBinder.Eval(Container, "DataItem.CalendarID")) ) %>'>
                            <img border="0" src="/Manage/images/pencil.png" alt="Edit" title="Edit" style="margin-right: 10px;" />
						</asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
					<ItemStyle Width="80px" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
					<ItemTemplate>
						<asp:Label ID="lblDate" runat="server" Text='<%# String.Format( "{0:d}", DataBinder.Eval(Container, "DataItem.EventDate") ) %>'>
						</asp:Label>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
					<ItemTemplate>
						&nbsp;&nbsp;
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
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
