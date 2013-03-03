﻿<%@ Page Title="User Group Add/Edit" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="UserGroupAddEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.UserGroupAddEdit" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">

		var webSvc = "/c3-admin/CMS.asmx";

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
	User Group Add/Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<asp:TextBox ValidationGroup="editGroupName" ID="txtRoleName" onkeypress="return ProcessKeyPress(event)" Width="300px" MaxLength="100" runat="server" />
		<asp:RequiredFieldValidator ValidationGroup="editGroupName" ControlToValidate="txtRoleName" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
			Display="Dynamic" />
	</p>
	<p>
		<br />
		<asp:Button ValidationGroup="editGroupName" ID="btnApply" runat="server" Text="Save" OnClick="btnApply_Click" />
		<input type="button" id="btnCancel" runat="server" value="Cancel" onclick="javascript:window.location='./UserGroups.aspx';" />
		<br />
	</p>
	<asp:Panel ID="pnlUsers" runat="server" Visible="false">
		<table cellspacing="2">
			<tr>
				<td valign="top">
					<br />
					<div id="SortableGrid" style="width: 350px;">
						<carrot:CarrotGridView CssClass="datatable" DefaultSort="Email ASC" ID="gvUsers" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
							AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
							<Columns>
								<asp:TemplateField>
									<ItemTemplate>
										<asp:CheckBox ValidationGroup="removeUsers" ID="chkSelected" runat="server" />
										<asp:HiddenField Value='<%#Eval("UserName") %>' ID="hdnUserName" runat="server" Visible="false" />
										<asp:HiddenField Value='<%#Eval("ProviderUserKey") %>' ID="hdnUserId" runat="server" Visible="false" />
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="UserName" HeaderText="User Name" />
								<asp:BoundField DataField="Email" HeaderText="Email" />
							</Columns>
						</carrot:CarrotGridView>
					</div>
					<br />
					<p>
						<asp:Button ValidationGroup="removeUsers" ID="btnRemove" runat="server" Text="Remove Selected" OnClick="btnRemove_Click" />
					</p>
				</td>
				<td valign="top">
					&nbsp;&nbsp;&nbsp;
				</td>
				<td valign="top">
					<div style="width: 375px;">
						<p>
							Search for users to add to this group. Search by either username or email address.
						</p>
						<p>
							<b>Search: </b><span id="spanResults"></span>
							<asp:TextBox ValidationGroup="addUsers" ID="txtSearch" onkeypress="return ProcessKeyPress(event)" Width="350px" MaxLength="100" runat="server" />
							<asp:RequiredFieldValidator ValidationGroup="addUsers" ControlToValidate="txtSearch" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required"
								Display="Dynamic" />
							<asp:HiddenField ID="hdnUserID" runat="server" />
						</p>
						<p style="text-align: right;">
							<asp:Button ValidationGroup="addUsers" ID="btnAddUsers" runat="server" Text="Add User" OnClick="btnAddUsers_Click" />
						</p>
					</div>
				</td>
			</tr>
		</table>
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>