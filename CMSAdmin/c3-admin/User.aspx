<%@ Page Title="User" Language="C#" MasterPageFile="MasterPages/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.User" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	User Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<br />
	<fieldset style="width: 400px;">
		<legend>
			<label>
				User Profile
			</label>
		</legend>
		<table style="width: 500px;">
			<tr>
				<td style="width: 180px;">
					<b class="caption">
						<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name</asp:Label>
					</b>
				</td>
				<td style="width: 300px;">
					<div style="height: 30px;">
						<asp:Label ID="lblUserName" runat="server" Columns="60" MaxLength="100" />
						<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="140px" ID="UserName" runat="server" MaxLength="50" ValidationGroup="createWizard" />
						<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="UserName" ErrorMessage="Username is required."
							ToolTip="Username is required." ValidationGroup="createWizard" Display="Dynamic" Text="**" />
					</div>
				</td>
			</tr>
			<tr>
				<td>
					<b class="caption">
						<asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email" Text="E-mail " />
					</b>
				</td>
				<td>
					<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="200px" ID="Email" runat="server" MaxLength="100" ValidationGroup="createWizard" />
					<asp:RequiredFieldValidator ID="EmailRequired" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="Email" ErrorMessage="E-mail is required."
						ToolTip="E-mail is required." ValidationGroup="createWizard" Display="Dynamic" Text="**" />
				</td>
			</tr>
			<tr>
				<td class="tablecontents">
					<b class="caption">Nickname</b>
				</td>
				<td class="tablecontents">
					<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="180px" MaxLength="100" ID="txtNickName" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecontents">
					<b class="caption">First Name</b>
				</td>
				<td class="tablecontents">
					<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="180px" MaxLength="100" ID="txtFirstName" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="tablecontents">
					<b class="caption">Last Name</b>
				</td>
				<td class="tablecontents">
					<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="180px" MaxLength="100" ID="txtLastName" runat="server" />
				</td>
			</tr>
			<tr>
				<td>
					<b class="caption">Locked </b>
				</td>
				<td>
					<asp:CheckBox ID="chkLocked" runat="server" />
				</td>
			</tr>
		</table>
	</fieldset>
	<div>
		<fieldset style="width: 650px;">
			<legend>
				<label>
					Bio Data</label>
			</legend>
			<div runat="server" id="divCenter">
				<a href="javascript:cmsToggleTinyMCE('<%= reBody.ClientID %>');">Show/Hide Editor</a></div>
			<asp:TextBox ValidationGroup="inputForm" Style="height: 250px; width: 550px;" CssClass="mceEditor" ID="reBody" runat="server" TextMode="MultiLine" Rows="20"
				Columns="60" />
			<br />
		</fieldset>
	</div>
	<table>
		<tr>
			<td>
				<fieldset style="width: 300px;">
					<legend>
						<label>
							Roles
						</label>
					</legend>
					<div class="SortableGrid" style="width: 300px;">
						<carrot:CarrotGridView CssClass="datatable" DefaultSort="RoleName ASC" ID="gvRoles" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
							AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
							<EmptyDataTemplate>
								<p>
									<b>No users found.</b>
								</p>
							</EmptyDataTemplate>
							<Columns>
								<asp:TemplateField>
									<ItemTemplate>
										<asp:CheckBox ID="chkSelected" runat="server" />
										<asp:HiddenField Value='<%#Eval("RoleName") %>' ID="hdnRoleId" runat="server" Visible="false" />
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="RoleName" HeaderText="Role Name" />
							</Columns>
						</carrot:CarrotGridView>
					</div>
				</fieldset>
			</td>
			<td>
				&nbsp;
			</td>
			<td>
				<fieldset style="width: 400px;">
					<legend>
						<label>
							Sites
						</label>
					</legend>
					<div class="SortableGrid" style="width: 400px;">
						<carrot:CarrotGridView CssClass="datatable" DefaultSort="SiteName ASC" ID="gvSites" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
							AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
							<EmptyDataTemplate>
								<p>
									<b>No sites found.</b>
								</p>
							</EmptyDataTemplate>
							<Columns>
								<asp:TemplateField>
									<ItemTemplate>
										<asp:CheckBox ID="chkSelected" runat="server" />
										<asp:HiddenField Value='<%#Eval("SiteID") %>' ID="hdnSiteID" runat="server" Visible="false" />
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="SiteName" HeaderText="Site Name" />
								<asp:BoundField DataField="MainURL" HeaderText="Main URL" />
							</Columns>
						</carrot:CarrotGridView>
					</div>
				</fieldset>
			</td>
		</tr>
	</table>
	<div style="display: none;">
		<asp:ValidationSummary ID="formValidationSummary" runat="server" ShowSummary="true" ValidationGroup="inputForm" />
	</div>
	<br />
	<asp:Button ID="btnApply" runat="server" Text="Save" OnClick="btnApply_Click" ValidationGroup="createWizard" OnClientClick="return ClickApplyBtn()" />
	<input type="button" id="btnCancel" runat="server" value="Cancel" onclick="javascript:window.location='./UserMembership.aspx';" />
	<br />
	<script type="text/javascript">
		function ClickApplyBtn() {
			var ret = tinyMCE.triggerSave();

			if (cmsIsPageValid()) {
				return true;
			} else {
				cmsLoadPrettyValidationPopup('<%= formValidationSummary.ClientID %>');
				return false;
			}
		}
	</script>
</asp:Content>
