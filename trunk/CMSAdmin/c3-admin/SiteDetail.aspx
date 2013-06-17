<%@ Page Title="SiteDetail" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteDetail.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.SiteDetail" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">

		var webSvc = cmsGetServiceAddress();

		$(document).ready(function () {
			var webMthd = webSvc + "/FindUsers";
			var resFld = "#spanResults";
			var hdnFld = "#<%=hdnUserID.ClientID %>";

			$("#<%=txtSearch.ClientID %>").autocomplete({
				source: function (request, response) {
					$.ajax({
						url: webMthd,
						type: 'POST',
						dataType: 'json',
						contentType: 'application/json; charset=utf-8',
						data: "{ 'searchTerm': '" + MakeStringSafe(request.term) + "' }",
						contentType: "application/json; charset=utf-8",
						dataFilter: function (data) { return data; },
						success: function (data) {
							$(hdnFld).val('');
							$(resFld).text('');
							//debugger;
							response($.map(data.d, function (item) {
								return {
									value: item.UserName + " (" + item.Email + ")",
									id: item.UserName
								}
							}))
							if (data.d.length < 1) {
								$(resFld).attr('style', 'color: #990000;');
								$(resFld).text('  No Results  ');
							} else {
								$(resFld).attr('style', 'color: #009900;');
								if (data.d.length == 1) {
									$(resFld).text('  ' + data.d.length + ' Result  ');
								} else {
									$(resFld).text('  ' + data.d.length + ' Results  ');
								}
							}
						},

						error: function (XMLHttpRequest, textStatus, errorThrown) {
							cmsAjaxFailed(XMLHttpRequest);
						}
					});
				},
				select: function (event, ui) {
					$(hdnFld).val('');
					if (ui.item) {
						$(hdnFld).val(ui.item.id);
					}
					$(resFld).text('');
				},
				minLength: 1,
				delay: 1000
			});
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
