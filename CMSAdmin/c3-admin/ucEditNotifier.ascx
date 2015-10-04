<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEditNotifier.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ucEditNotifier" %>
<%@ Import Namespace="Carrotware.CMS.Core" %>
<link href="/c3-admin/includes/edit-notifier.css" rel="stylesheet" type="text/css" />

<div style="clear: both;">
	&nbsp;</div>
<div class="cmsNavFooterBox">
	<p class="cmsFooterP">
		<a class="cmsFooterLinks" target="_blank" href="<%=EditPageURL %>?id=<%=CurrentPageID %>">EDIT</a> 
		<a class="cmsFooterLinks" target="_top" href="<%=SiteData.AlternateCurrentScriptName %>?carrotedit=true">ADVANCED EDIT</a> 
		<a class="cmsFooterLinks" target="_top" href="<%=PageIndexURL %>">CONTENT INDEX</a> 
		<a class="cmsFooterLinks" target="_top" href="/c3-admin/ModuleIndex.aspx">MODULE INDEX</a>
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
		Release Date: <asp:Literal runat="server" ID="litRelease" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
		Retire Date: <asp:Literal runat="server" ID="litRetire" />
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
			<a class="cmsNavImageLink" target="_blank" href="<%=EditPageURL %>?id=<%=CurrentPageID %>">
				<img alt="EDIT" title="EDIT" src="/c3-admin/images/application_edit.png" /></a>
		</div>
		<div class="cmsNavBox1">
			<a title="ADVANCED EDIT" class="cmsNavImageLink" target="_top" href="<%=SiteData.AlternateCurrentScriptName %>?carrotedit=true">
				<img alt="ADVANCED EDIT" title="ADVANCED EDIT" src="/c3-admin/images/overlays.png" /></a>
		</div>
		<div class="cmsNavBox1">
			<a title="CONTENT INDEX" class="cmsNavImageLink" target="_top" href="<%=PageIndexURL %>">
				<img alt="CONTENT INDEX" title="CONTENT INDEX" src="/c3-admin/images/table.png" /></a>
		</div>
		<div class="cmsNavBox2">
			<a title="MODULE INDEX" class="cmsNavImageLink" target="_top" href="/c3-admin/ModuleIndex.aspx">
				<img alt="MODULE INDEX" title="MODULE INDEX" src="/c3-admin/images/brick.png" /></a>
		</div>
	</div>
</div>
<div id="cmsAdminToolbox2" class="cmsNavigationBlock">
	<div class="cmsNavBox2">
		<div class="cmsNavBox1">
			<a class="cmsNavImageLink" target="_blank" href="<%=EditPageURL %>?id=<%=CurrentPageID %>">
				<img alt="EDIT" title="EDIT" src="/c3-admin/images/application_edit.png" /></a>
		</div>
		<div class="cmsNavBox1">
			<a title="ADVANCED EDIT" class="cmsNavImageLink" target="_top" href="<%=SiteData.AlternateCurrentScriptName %>?carrotedit=true">
				<img alt="ADVANCED EDIT" title="ADVANCED EDIT" src="/c3-admin/images/overlays.png" /></a>
		</div>
		<div class="cmsNavBox1">
			<a title="CONTENT INDEX" class="cmsNavImageLink" target="_top" href="<%=PageIndexURL %>">
				<img alt="CONTENT INDEX" title="CONTENT INDEX" src="/c3-admin/images/table.png" /></a>
		</div>
		<div class="cmsNavBox2">
			<a title="MODULE INDEX" class="cmsNavImageLink" target="_top" href="/c3-admin/ModuleIndex.aspx">
				<img alt="MODULE INDEX" title="MODULE INDEX" src="/c3-admin/images/brick.png" /></a>
		</div>
	</div>
</div>
