<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarAdmin.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.CalendarModule.CalendarAdmin" %>
<style type="text/css">
	.calendarCenter1 {
		width: 100%;
		margin: 0px auto;
		border: #ffffff 1px solid;
		text-align: center;
		padding-bottom: 10px;
	}
	
	.calendarCenter2 {
		width: 75%;
		margin: 0px auto;
		border: #ffffff 1px solid;
		text-align: center;
		padding-bottom: 10px;
	}
	
	.calendarEventDetail {
		width: 500px;
		padding: 25px;
	}
	
	table.calendarGrid {
		margin: 0px auto !important;
	}
</style>
<div class="calendarCenter1">
	<div id="divDatePicker" class="calendarCenter1" runat="server">
		<asp:TextBox Style="display: none;" ID="lblDate" runat="server" ReadOnly="true" Columns="40" />
		<asp:TextBox CssClass="dateRegion" ID="txtDate" Columns="12" runat="server" AutoPostBack="true" OnTextChanged="txtDate_TextChanged" />
	</div>
	<div class="calendarCenter2">
		<div class="calendarCenter2">
			<carrot:Calendar runat="server" ID="Calendar1" />
		</div>
		<asp:Button CssClass="calendarbutton" ID="btnLast" runat="server" Text="«««««" OnClick="btnLast_Click" />
		&nbsp;&nbsp;&nbsp;
		<asp:Button CssClass="calendarbutton" ID="btnNext" runat="server" Text="»»»»»" OnClick="btnNext_Click" />
	</div>
	<div class="calendarCenter2">
		<div class="calendarEventDetail">
			<asp:GridView ID="dgEvents" runat="server" ShowHeader="False" AutoGenerateColumns="False" GridLines="None">
				<EmptyDataTemplate>
					<p>
						<b>No entries found.</b>
					</p>
				</EmptyDataTemplate>
				<Columns>
					<asp:TemplateField>
						<ItemStyle Width="20px" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink ID="lnkEdit" runat="server" NavigateUrl='<%#CreateLink("CalendarAdminAddEdit", String.Format("id={0}", Eval("CalendarID")) ) %>'>
                            <img border="0" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" style="margin-right: 10px;" />
							</asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemStyle Width="80px" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
						<ItemTemplate>
							<asp:Label ID="lblDate" runat="server" Text='<%# String.Format( "{0:d}", Eval("EventDate") ) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemTemplate>
							&nbsp;&nbsp;
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
						<ItemTemplate>
							<b>
								<asp:Label ID="lblEvent" runat="server" Text='<%# String.Format( "{0}", Eval("EventTitle") ) %>'>
								</asp:Label></b><br />
							<asp:Label ID="lblDetail" runat="server" Text='<%# String.Format( "{0}<br /><br />", Eval("EventDetail") ) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
		</div>
	</div>
</div>
