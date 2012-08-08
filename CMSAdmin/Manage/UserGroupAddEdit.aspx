<%@ Page Title="User Group Add/Edit" Language="C#" MasterPageFile="~/Manage/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="UserGroupAddEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.UserGroupAddEdit" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">

		var webSvc = "/Manage/CMS.asmx";

		function MakeStringSafe(val) {
			val = Base64.encode(val);
			return val;
		}

		//===============================================

		function cmsAjaxFailed(request) {
			var s = "";
			s = s + "<b>status: </b>" + request.status + '<br />\r\n';
			s = s + "<b>statusText: </b>" + request.statusText + '<br />\r\n';
			s = s + "<b>responseText: </b>" + request.responseText + '<br />\r\n';
			cmsAlertModal(s);
		}

		function cmsAlertModal(request) {

			$("#CMSmodalalertmessage").html(request);

			$("#CMSmodalalert").dialog("destroy");

			$("#CMSmodalalert").dialog({
				//autoOpen: false,
				height: 400,
				width: 550,
				modal: true,
				buttons: {
					"OK": function () {
						$(this).dialog("close");
					}
				}
			});
		}

		$(document).ready(function () {
			var webMthd = webSvc + "/FindUsers";

			$("#<%=txtSearch.ClientID %>").autocomplete({
				source: function (request, response) {
					$.ajax({
						url: webMthd,
						type: 'POST',
						dataType: 'json',
						contentType: 'application/json; charset=utf-8',
						//data: JSON.stringify(paramters),
						data: "{ 'searchTerm': '" + MakeStringSafe(request.term) + "' }",
						contentType: "application/json; charset=utf-8",
						dataFilter: function (data) { return data; },
						success: function (data) {
							$("#<%=hdnUserID.ClientID %>").val('');
							response($.map(data.d, function (item) {
								return {
									value: item.UserName + " (" + item.Email + ")",
									id: item.UserName
								}
							}))
						},

						error: function (XMLHttpRequest, textStatus, errorThrown) {
							cmsAjaxFailed(XMLHttpRequest);
						}
					});
				},
				select: function (event, ui) {
					$("#<%=hdnUserID.ClientID %>").val('');
					if (ui.item) {
						$("#<%=hdnUserID.ClientID %>").val(ui.item.id);
					}
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
		<asp:TextBox ID="txtRoleName" Width="300px" MaxLength="100" runat="server"></asp:TextBox>
		<asp:RequiredFieldValidator ID="RoleNameRequired" runat="server" ControlToValidate="txtRoleName" ErrorMessage="!" ToolTip="Role is required."
			ValidationGroup="createWizard" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
	</p>
	<p>
		<br />
		<asp:Button ValidationGroup="createWizard" ID="btnApply" runat="server" Text="Save" OnClick="btnApply_Click" />
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
					<p>
						Search for users to add to this group.  Search by either username or email address.
					</p>
					<p>
						<b>Search: </b>
						<asp:TextBox ValidationGroup="addUsers" ID="txtSearch" runat="server" Columns="60" />
						<asp:HiddenField ID="hdnUserID" runat="server" />
					</p>
					<p style="text-align: right;">
						<asp:Button ValidationGroup="addUsers" ID="btnAddUsers" runat="server" Text="Add User" OnClick="btnAddUsers_Click" />
					</p>
				</td>
			</tr>
		</table>
		<div style="display: none">
			<div id="CMSmodalalert" title="CMS Alert">
				<p id="CMSmodalalertmessage">
					&nbsp;</p>
			</div>
		</div>
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
