<%@ Page Title="SiteDetail" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteDetail.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.SiteDetail" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script src="Includes/FindUsers.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			initFindUsers("<%=hdnUserID.ClientID %>", "<%=txtSearch.ClientID %>");
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site Detail
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<fieldset style="width: 550px;">
		<legend>
			<label>
				Site Information
			</label>
		</legend>
		<table style="width: 99%;">
			<tr>
				<td class="tablecaption" style="width: 160px;">
					Site ID
				</td>
				<td>
					<div style="padding: 5px; padding-left: 10px; width: 275px;" class=" ui-widget-content ui-corner-all ">
						<asp:Literal ID="litID" runat="server" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Site URL
				</td>
				<td>
					<asp:Literal ID="litURL" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Site Name
				</td>
				<td>
					<asp:Literal ID="litSiteName" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecaption">
					Site Tagline
				</td>
				<td>
					<asp:Literal ID="litTagline" runat="server" />
				</td>
			</tr>
		</table>
	</fieldset>
	<table>
		<tr>
			<td>
				<fieldset style="width: 450px;">
					<legend>
						<label>
							Users
						</label>
					</legend>
					<br />
					<div class="SortableGrid" style="width: 450px;">
						<carrot:CarrotGridView CssClass="datatable" DefaultSort="Email ASC" ID="gvUsers" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
							AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
							<EmptyDataTemplate>
								<p>
									<b>No users found.</b>
								</p>
							</EmptyDataTemplate>
							<Columns>
								<asp:TemplateField>
									<ItemTemplate>
										<asp:CheckBox ValidationGroup="removeUsers" ID="chkSelected" runat="server" />
										<asp:HiddenField Value='<%#Eval("UserName") %>' ID="hdnUserName" runat="server" Visible="false" />
										<asp:HiddenField Value='<%#Eval("UserId") %>' ID="hdnUserId" runat="server" Visible="false" />
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="UserName" HeaderText="User Name" />
								<asp:BoundField DataField="EmailAddress" HeaderText="Email" />
								<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsAdmin" HeaderText="Is Admin" ShowBooleanImage="true" AlternateTextTrue="Yes"
									AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
								<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsEditor" HeaderText="Is Editor" ShowBooleanImage="true" AlternateTextTrue="Yes"
									AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
							</Columns>
						</carrot:CarrotGridView>
					</div>
					<br />
					<p>
						<asp:Button ValidationGroup="removeUsers" ID="btnRemove" runat="server" Text="Remove Selected" OnClick="btnRemove_Click" />
					</p>
				</fieldset>
			</td>
			<td>
				&nbsp;&nbsp;&nbsp;
			</td>
			<td>
				<fieldset style="width: 400px;">
					<legend>
						<label>
							Add Users
						</label>
					</legend>
					<div style="width: 400px;">
						<p>
							Search for users to add to this group. Search by either username or email address.
						</p>
						<p>
							<b>Search: </b><span id="spanResults"></span>
							<asp:TextBox ValidationGroup="addUsers" ID="txtSearch" onkeypress="return ProcessKeyPress(event)" Width="350px" MaxLength="100" runat="server" />
							<asp:RequiredFieldValidator ValidationGroup="addUsers" CssClass="validationError" ForeColor="" ControlToValidate="txtSearch" ID="RequiredFieldValidator3"
								runat="server" ErrorMessage="Required" Display="Dynamic" />
							<asp:HiddenField ID="hdnUserID" runat="server" />
						</p>
						<p>
							<asp:CheckBox ID="chkAddToEditor" runat="server" Text=" Add to Editor Role " />
						</p>
						<p style="text-align: right;">
							<asp:Button ValidationGroup="addUsers" ID="btnAddUsers" runat="server" Text="Add User" OnClick="btnAddUsers_Click" />
						</p>
					</div>
				</fieldset>
			</td>
		</tr>
	</table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
