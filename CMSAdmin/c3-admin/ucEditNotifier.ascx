<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEditNotifier.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ucEditNotifier" %>
<%@ Import Namespace="Carrotware.CMS.Core" %>
<asp:PlaceHolder ID="plcIncludes" runat="server">
	<carrot:CmsSkin runat="server" ID="siteSkin" WindowMode="Notify" SelectedColor="Classic" />
</asp:PlaceHolder>

<div style="clear: both;">
	&nbsp;
</div>

<asp:PlaceHolder runat="server" ID="phNotify">
	<div class="cmsNavFooterBox">
		<p class="cmsFooterP">
			<a class="cmsFooterLinks" target="_top" href="<%=SiteFilename.DashboardURL %>">DASHBOARD</a>
			<a class="cmsFooterLinks" target="_top" href="<%=AdvEditPageURL %>">ADVANCED EDIT</a>
			<a class="cmsFooterLinks" target="_blank" href="<%=EditPageURL %>">EDIT</a>
			<a class="cmsFooterLinks" target="_top" href="<%=PageIndexURL %>"><%=PageIndexText %></a>
			<a class="cmsFooterLinks" target="_top" href="<%=SiteFilename.ModuleIndexURL %>">MODULE INDEX</a>
		</p>
		<p class="cmsFooterP">
			<a class="cmsFooterLinks" href="/">HOME PAGE</a>
			<a class="cmsFooterLinks" runat="server" id="lnkParent" href="#">PARENT PAGE</a>
			<a class="cmsFooterLinks" runat="server" id="lnkCurrent" href="#">CURRENT PAGE</a>
			<asp:Label ID="lblChildDDL" runat="server" CssClass="cmsFooterLinks">CHILD PAGES</asp:Label>
			<asp:DropDownList ID="ddlCMSLinks" runat="server" onchange="cmsNavPage(this);" DataTextField="NavMenuText" DataValueField="FileName"
				ValidationGroup="cmsMenuLinkGroup">
			</asp:DropDownList>
		</p>
		<p class="cmsFooterP cmsFooterLinks">
			<asp:Literal runat="server" ID="litVersion" />
		</p>
		<p class="cmsFooterP cmsFooterLinks">
			<asp:Literal runat="server" ID="litTemplate" />
		</p>
		<p class="cmsFooterP cmsFooterLinks">
			Release Date:
		<asp:Literal runat="server" ID="litRelease" />
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		Retire Date:
		<asp:Literal runat="server" ID="litRetire" />
		</p>
	</div>
	<script type="text/javascript">
		function cmsNavPage(y) {
			var url = y.options[y.selectedIndex].value;

			if (url != '00000') {
				window.setTimeout('location.href = \'' + url + '\'', 500);
			}
		}
	</script>
	<div id="cmsAdminToolbox1" class="cmsNavigationBlock">
		<div class="cmsNavBox2">
			<div class="cmsNavBox1">
				<a class="cmsNavImageLink" target="_top" href="<%=SiteFilename.DashboardURL %>">
					<img alt="DASHBOARD" title="DASHBOARD" src="/c3-admin/images/layout_content.png" /></a>
			</div>
			<div class="cmsNavBox1">
				<a title="ADVANCED EDIT" class="cmsNavImageLink" target="_top" href="<%=AdvEditPageURL %>">
					<img alt="ADVANCED EDIT" title="ADVANCED EDIT" src="/c3-admin/images/overlays.png" /></a>
			</div>
			<div class="cmsNavBox1">
				<a class="cmsNavImageLink" target="_blank" href="<%=EditPageURL %>">
					<img alt="EDIT" title="EDIT" src="/c3-admin/images/application_edit.png" /></a>
			</div>
			<div class="cmsNavBox1">
				<a title="<%=PageIndexText %>" class="cmsNavImageLink" target="_top" href="<%=PageIndexURL %>">
					<img alt="<%=PageIndexText %>" title="<%=PageIndexText %>" src="/c3-admin/images/table.png" /></a>
			</div>
			<div class="cmsNavBox2">
				<a title="MODULE INDEX" class="cmsNavImageLink" target="_top" href="<%=SiteFilename.ModuleIndexURL %>">
					<img alt="MODULE INDEX" title="MODULE INDEX" src="/c3-admin/images/brick.png" /></a>
			</div>
		</div>
	</div>
	<div id="cmsAdminToolbox2" class="cmsNavigationBlock">
		<div class="cmsNavBox2">
			<div class="cmsNavBox1">
				<a class="cmsNavImageLink" target="_top" href="<%=SiteFilename.DashboardURL %>">
					<img alt="DASHBOARD" title="DASHBOARD" src="/c3-admin/images/layout_content.png" /></a>
			</div>
			<div class="cmsNavBox1">
				<a title="ADVANCED EDIT" class="cmsNavImageLink" target="_top" href="<%=AdvEditPageURL %>">
					<img alt="ADVANCED EDIT" title="ADVANCED EDIT" src="/c3-admin/images/overlays.png" /></a>
			</div>
			<div class="cmsNavBox1">
				<a class="cmsNavImageLink" target="_blank" href="<%=EditPageURL %>">
					<img alt="EDIT" title="EDIT" src="/c3-admin/images/application_edit.png" /></a>
			</div>
			<div class="cmsNavBox1">
				<a title="<%=PageIndexText %>" class="cmsNavImageLink" target="_top" href="<%=PageIndexURL %>">
					<img alt="<%=PageIndexText %>" title="<%=PageIndexText %>" src="/c3-admin/images/table.png" /></a>
			</div>
			<div class="cmsNavBox2">
				<a title="MODULE INDEX" class="cmsNavImageLink" target="_top" href="<%=SiteFilename.ModuleIndexURL %>">
					<img alt="MODULE INDEX" title="MODULE INDEX" src="/c3-admin/images/brick.png" /></a>
			</div>
		</div>
	</div>
</asp:PlaceHolder>
