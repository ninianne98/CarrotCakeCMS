<%@ Page Title="Widget Time" Language="C#" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="WidgetTime.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.WidgetTime" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<%@ Import Namespace="Carrotware.CMS.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Widget Time
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<asp:Literal ID="litControlPath" runat="server" /><br />
		<asp:Literal ID="litControlPathName" runat="server" /><br />
	</p>
	<table style="width: 700px;">
		<tr>
			<td class="tablecaption">
				release date:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtReleaseDate" runat="server" CssClass="dateRegion" Columns="16" />
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtReleaseTime" runat="server" CssClass="timeRegion" Columns="10" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				retire date:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtRetireDate" runat="server" CssClass="dateRegion" Columns="16" />
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtRetireTime" runat="server" CssClass="timeRegion" Columns="10" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				enabled:
			</td>
			<td>
				<asp:CheckBox ID="chkActive" runat="server" />
			</td>
		</tr>
	</table>
	<p>
		<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Apply" />
	</p>
	<asp:PlaceHolder ID="phNavIndex" runat="server">
		<p>
			<asp:HyperLink ID="lnkIndex" runat="server" NavigateUrl="./PageWidgets.aspx">
		<img src="/c3-admin/images/back.png" alt="Return" title="Return" />
		Return to widget list</asp:HyperLink>
		</p>
	</asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
