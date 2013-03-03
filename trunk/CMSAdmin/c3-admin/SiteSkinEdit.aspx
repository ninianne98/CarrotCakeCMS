﻿<%@ Page Title="Site Skin Edit" ValidateRequest="false" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteSkinEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.SiteSkinEdit" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Import Namespace="Carrotware.CMS.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site Skin Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<h2>
		<asp:Literal ID="litSkinFileName" runat="server"></asp:Literal>
		[<a href="<%= String.Format("{0}?path={1}", SiteData.CurrentScriptName, sTemplateFileQS) %>">Edit</a>]
	</h2>
	<p>
		<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtPageContents" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Skin File Contents Required"
			Display="Dynamic" />&nbsp;
	</p>
	<table width="100%">
		<tr>
			<td valign="top" width="250px">
				<p>
					<b>Supporting Styles/Scripts</b>
				</p>
				<div style="border: solid 0px #000000; padding: 4px; width: 250px; height: 425px; overflow: auto;">
					<ul>
						<asp:Repeater ID="rpFiles" runat="server">
							<ItemTemplate>
								<li><a href="<%# String.Format("{0}?path={1}&alt={2}", SiteData.CurrentScriptName, sTemplateFileQS, EncodePath(String.Format("{0}{1}", Eval("FolderPath"), Eval("FileName"))) ) %>">
									<%# String.Format("{0}{1}", Eval("FolderPath"), Eval("FileName")) %>
								</a>
									<br />
									<br />
								</li>
							</ItemTemplate>
						</asp:Repeater>
					</ul>
				</div>
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
						<asp:Literal ID="litDateMod" runat="server"></asp:Literal></b> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<input type="button" id="btnBrowseSvr" value="Browse Server Files" onclick="cmsFileBrowserOpen('not-a-real-file');" />
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