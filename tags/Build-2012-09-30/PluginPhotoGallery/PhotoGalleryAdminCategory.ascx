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
			<asp:TextBox onkeypress="return ProcessKeyPress(event)" ID="txtGallery" runat="server" Columns="60" MaxLength="256"></asp:TextBox>
		</td>
	</tr>
</table>
<asp:Button ID="btnSave" class="myButton" runat="server" Text="Save" OnClick="btnSave_Click" />