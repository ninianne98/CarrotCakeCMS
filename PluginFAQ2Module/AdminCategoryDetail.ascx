<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminCategoryDetail.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.FAQ2Module.AdminCategoryDetail" %>
<h2>FAQs : Category Add/Edit</h2>
<br />
<table>
	<tr>
		<td valign="top">faq name
		</td>
		<td>
			<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtFAQ" runat="server" Columns="60" MaxLength="200"></asp:TextBox>
			<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtFAQ" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
				Display="Dynamic"></asp:RequiredFieldValidator>
		</td>
	</tr>
</table>
<asp:Button ValidationGroup="inputForm" ID="btnSave" class="myButton" runat="server" Text="Save" OnClick="btnSave_Click" />