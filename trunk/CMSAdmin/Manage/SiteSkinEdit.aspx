<%@ Page Title="Site Skin Edit" ValidateRequest="false" Language="C#" MasterPageFile="~/Manage/MasterPages/Main.Master" AutoEventWireup="true"
	CodeBehind="SiteSkinEdit.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.SiteSkinEdit" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site Skin Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<h2>
		<a href="<%= String.Format("{0}?path={1}", Carrotware.CMS.Core.SiteData.CurrentScriptName, sTemplateFileQS) %>">
			<asp:Literal ID="litSkinFileName" runat="server"></asp:Literal>
		</a>
	</h2>
	<p>
		<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtPageContents" ID="RequiredFieldValidator1"
			runat="server" ErrorMessage="Skin File Contents Required" Display="Dynamic"></asp:RequiredFieldValidator>&nbsp;
	</p>
	<table width="100%">
		<tr>
			<td valign="top" width="180px">
				<p>
					<b>Supporting Styles/Scripts</b>
				</p>
				<ul>
					<asp:Repeater ID="rpFiles" runat="server">
						<ItemTemplate>
							<li><a href="<%# String.Format("{0}?path={1}&alt={2}", Carrotware.CMS.Core.SiteData.CurrentScriptName, sTemplateFileQS, EncodePath(String.Format("{0}{1}", Eval("FolderPath"), Eval("FileName"))) ) %>">
								<%# String.Format("{0}{1}", Eval("FolderPath"), Eval("FileName")) %>
							</a>
								<br />
								<br />
							</li>
						</ItemTemplate>
					</asp:Repeater>
				</ul>
			</td>
			<td valign="top" width="20px">
				<p>
					&nbsp;&nbsp;&nbsp;</p>
			</td>
			<td valign="top">
				<p>
					<b>Viewing file:
						<asp:Literal ID="litEditFileName" runat="server"></asp:Literal>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:Literal ID="litDateMod" runat="server"></asp:Literal></b>
				</p>
				<asp:TextBox Style="height: 450px; width: 790px;" ID="txtPageContents" runat="server" TextMode="MultiLine" Rows="15" Columns="100" />
				<p>
					<asp:Button ValidationGroup="inputForm" ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" />
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<input type="button" id="btnCancel" value="Cancel" onclick="location.href='./SiteSkinIndex.aspx';" />
				</p>
			</td>
		</tr>
	</table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
