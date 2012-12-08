<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEditNotifier.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.ucEditNotifier" %>
<div style="clear: both;">
	&nbsp;</div>
<style type="text/css">
	#cmsAdminToolbox1 {
		left: 0 !important;
		top: 100px !important;
		line-height: 15px !important;
	}
	#cmsAdminToolbox2 {
		right: 0 !important;
		bottom: 125px !important;
		line-height: 15px !important;
	}
	.cmsNavigationBlock {
		position: fixed !important;
		width: 8em !important;
		margin-top: -2.5em !important;
		padding: 0px !important;
		border: 2px dotted #676F6A !important;
		color: #676F6A !important;
		background: #CDE3D6 !important;
		width: 30px !important;
		height: 128px !important;
		z-index: 1000 !important;
		font-size: 14px !important;
		font-family: Lucida Grande, Lucida Sans, Arial, sans-serif !important;
	}
	.cmsNavBox1 {
		padding-top: 2px !important;
		padding-bottom: 10px !important;
		text-align: center !important;
		line-height: 15px !important;
	}
	.cmsNavBox2 {
		padding-top: 2px !important;
		padding-bottom: 2px !important;
		text-align: center !important;
		line-height: 15px !important;
	}
	.cmsNavImageLink {
		margin: 0px !important;
		padding: 0px !important;
		color: #676F6A !important;
		padding: 2px !important;
		margin: 2px !important;
		line-height: 10px !important;
	}
	.cmsNavigationBlock img, .cmsNavImageLink img {
		margin: 0px !important;
		padding: 0px !important;
		border: 1px solid #CDE3D6 !important;
		line-height: 10px !important;
		background: transparent !important;
	}
	.cmsFooterLinks {
		color: #676F6A !important;
		padding: 5px !important;
		margin: 2px !important;
		font-weight: bold !important;
		margin-left: 10px !important;
		margin-right: 10px !important;
		font-size: 14px !important;
		font-family: Lucida Grande, Lucida Sans, Arial, sans-serif !important;
		text-decoration: underline !important;
	}
	span.cmsFooterLinks, p.cmsFooterLinks {
		text-decoration: none !important;
	}
	.cmsFooterP {
		color: #676F6A !important;
		line-height: 20px !important;
		text-align: center !important;
		padding: 5px !important;
		margin: 5px !important;
		line-height: 110% !important;
		font-size: 14px !important;
		font-family: Lucida Grande, Lucida Sans, Arial, sans-serif !important;
	}
	.cmsNavFooterBox {
		line-height: 20px !important;
		text-align: center !important;
		background: #CDE3D6 !important;
		padding: 5px !important;
		margin: 5px !important;
		border: 2px dashed #676F6A !important;
	}
	
	.cmsNavFooterBox input:focus, .cmsNavFooterBox select:focus, 
	.cmsNavFooterBox textarea:focus, .cmsNavFooterBox input, 
	.cmsNavFooterBox select, .cmsNavFooterBox textarea {
		line-height: normal;
		border: 1px solid #676F6A !important;
		background: #FFFFFF !important;
		color: #000000 !important;
		font-family: Lucida Grande, Lucida Sans, Arial, sans-serif !important;
		float: none !important;
		margin: 2px !important;
		padding: 1px !important;
		width: auto !important;
	}
</style>
<div class="cmsNavFooterBox">
	<p class="cmsFooterP">
		<a class="cmsFooterLinks" target="_blank" href="<%=EditPageURL %>?id=<%=CurrentPageID %>">EDIT</a> 
		<a class="cmsFooterLinks" target="_top" href="<%=Carrotware.CMS.Core.SiteData.AlternateCurrentScriptName %>?carrotedit=true">ADVANCED EDIT</a> 
		<a class="cmsFooterLinks" target="_top" href="<%=PageIndexURL %>">CONTENT INDEX</a> 
		<a class="cmsFooterLinks" target="_top" href="/Manage/ModuleIndex.aspx">MODULE INDEX</a>
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
		<asp:Literal runat="server" ID="litVersion"></asp:Literal>
	</p>
</div>
<script language="javascript" type="text/javascript">
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
				<img border="0" alt="EDIT" title="EDIT" src="/Manage/images/application_edit.png" /></a>
		</div>
		<div class="cmsNavBox1">
			<a title="ADVANCED EDIT" class="cmsNavImageLink" target="_top" href="<%=Carrotware.CMS.Core.SiteData.AlternateCurrentScriptName %>?carrotedit=true">
				<img border="0" alt="ADVANCED EDIT" title="ADVANCED EDIT" src="/Manage/images/overlays.png" /></a>
		</div>
		<div class="cmsNavBox1">
			<a title="CONTENT INDEX" class="cmsNavImageLink" target="_top" href="<%=PageIndexURL %>">
				<img border="0" alt="CONTENT INDEX" title="CONTENT INDEX" src="/Manage/images/table.png" /></a>
		</div>
		<div class="cmsNavBox2">
			<a title="MODULE INDEX" class="cmsNavImageLink" target="_top" href="/Manage/ModuleIndex.aspx">
				<img border="0" alt="MODULE INDEX" title="MODULE INDEX" src="/Manage/images/brick.png" /></a>
		</div>
	</div>
</div>
<div id="cmsAdminToolbox2" class="cmsNavigationBlock">
	<div class="cmsNavBox2">
		<div class="cmsNavBox1">
			<a class="cmsNavImageLink" target="_blank" href="<%=EditPageURL %>?id=<%=CurrentPageID %>">
				<img border="0" alt="EDIT" title="EDIT" src="/Manage/images/application_edit.png" /></a>
		</div>
		<div class="cmsNavBox1">
			<a title="ADVANCED EDIT" class="cmsNavImageLink" target="_top" href="<%=Carrotware.CMS.Core.SiteData.AlternateCurrentScriptName %>?carrotedit=true">
				<img border="0" alt="ADVANCED EDIT" title="ADVANCED EDIT" src="/Manage/images/overlays.png" /></a>
		</div>
		<div class="cmsNavBox1">
			<a title="CONTENT INDEX" class="cmsNavImageLink" target="_top" href="<%=PageIndexURL %>">
				<img border="0" alt="CONTENT INDEX" title="CONTENT INDEX" src="/Manage/images/table.png" /></a>
		</div>
		<div class="cmsNavBox2">
			<a title="MODULE INDEX" class="cmsNavImageLink" target="_top" href="/Manage/ModuleIndex.aspx">
				<img border="0" alt="MODULE INDEX" title="MODULE INDEX" src="/Manage/images/brick.png" /></a>
		</div>
	</div>
</div>
