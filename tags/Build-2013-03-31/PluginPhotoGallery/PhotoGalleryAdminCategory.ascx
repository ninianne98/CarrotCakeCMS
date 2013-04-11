<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryAdminCategory.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryAdminCategory" %>
<h2>
	Photo Gallery : Category Add/Edit</h2>
<br />
<table>
	<tr>
		<td valign="top">
			gallery name
		</td>
		<td>
			<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtGallery" runat="server" Columns="60" MaxLength="256"></asp:TextBox>
			<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtGallery" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
				Display="Dynamic"></asp:RequiredFieldValidator>
		</td>
	</tr>
</table>
<asp:Button ValidationGroup="inputForm" ID="btnSave" class="myButton" runat="server" Text="Save" OnClick="btnSave_Click" />