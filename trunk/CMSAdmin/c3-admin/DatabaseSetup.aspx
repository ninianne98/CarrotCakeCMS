<%@ Page Title="Setup Database" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Public.Master" AutoEventWireup="true" CodeBehind="DatabaseSetup.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.DatabaseSetup" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<style type="text/css">
		.errMsg {
			background-color: #FFE0E0;
		}
		.okMsg {
			background-color: #ffffff;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div style="height: 20px; width: 18px; float: right; border: 1px solid #ffffff; clear: both;">
		<a href="/">
			<img class="imgNoBorder" src="/c3-admin/images/house_go.png" alt="Homepage" title="Homepage" /></a>
	</div>
	<div style="border: solid 0px #000000; width: 375px; height: 140px; overflow: auto;" class="<%=CSSMsg() %>">
		<asp:Repeater ID="rpMessages" runat="server">
			<HeaderTemplate>
				<div>
			</HeaderTemplate>
			<ItemTemplate>
				<div>
					<b>
						<asp:Image ID="imgStat1" runat="server" Visible='<%# (bool)Eval("AlteredData") && !(bool)Eval("HasExceoption") %>' ImageUrl="/c3-admin/images/lightbulb.png"
							ToolTip="Executed" />
						<asp:Image ID="imgStat2" runat="server" Visible='<%# !(bool)Eval("AlteredData") && !(bool)Eval("HasExceoption") %>' ImageUrl="/c3-admin/images/lightbulb_off.png"
							ToolTip="Skipped" />
						<asp:Image ID="imgStat3" runat="server" Visible='<%# (bool)Eval("HasExceoption") %>' ImageUrl="/c3-admin/images/exclamation.png" ToolTip="Error" />
						<%#Eval("Message")%></b>
					<%#Eval("Response")%>
				</div>
				<div>
					<%#Eval("ExceptionText")%></div>
				<div>
					<i>
						<%#Eval("InnerExceptionText")%></i></div>
				<hr />
			</ItemTemplate>
			<FooterTemplate>
				</div>
			</FooterTemplate>
		</asp:Repeater>
	</div>
	<p>
		<asp:Button ID="btnLogin" runat="server" Text="Continue" OnClick="btnLogin_Click" />
		<asp:Button ID="btnCreate" runat="server" Text="Create User" OnClick="btnCreate_Click" />
	</p>
	<p>
		<a href="<%=SiteData.CurrentScriptName %>?">Re-run</a> &nbsp;|&nbsp; <a href="<%=SiteData.CurrentScriptName %>?signout=true">Re-run with signout</a>
	</p>
</asp:Content>
