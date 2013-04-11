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
		<asp:TextBox Style="display: none;" ID="lblDate" runat="server" ReadOnly="true" Columns="40"></asp:TextBox>
		<asp:TextBox CssClass="dateRegion" ID="txtDate" Columns="12" runat="server" AutoPostBack="true" OnTextChanged="txtDate_TextChanged"></asp:TextBox>
	</div>
	<div class="calendarCenter2">
		<div class="calendarCenter2">
			<carrot:Calendar runat="server" ID="Calendar1"></carrot:Calendar>
		</div>
		<asp:Button CssClass="calendarbutton" ID="btnLast" runat="server" Text="«««««" OnClick="btnLast_Click" />
		&nbsp;&nbsp;&nbsp;
		<asp:Button CssClass="calendarbutton" ID="btnNext" runat="server" Text="»»»»»" OnClick="btnNext_Click" />
	</div>
	<div class="calendarCenter2">
		<div class="calendarEventDetail">
			<asp:DataGrid ID="dgEvents" runat="server" CellPadding="2" ShowHeader="False" GridLines="None" AutoGenerateColumns="False">
				<ItemStyle></ItemStyle>
				<Columns>
					<asp:TemplateColumn>
						<ItemStyle Width="20px" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink ID="lnkedit" runat="server" NavigateUrl='<%#CreateLink("CalendarAdminAddEdit", String.Format("id={0}", DataBinder.Eval(Container, "DataItem.CalendarID")) ) %>'>
                            <img border="0" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" style="margin-right: 10px;" />
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
	</div>
</div>
