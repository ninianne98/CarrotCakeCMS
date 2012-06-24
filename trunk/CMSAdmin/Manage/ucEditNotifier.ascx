<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEditNotifier.ascx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.ucEditNotifier" %>
<div style="clear: both;">
	&nbsp;</div>
<style type="text/css">
	#cmsAdminToolbox1 {
		left: 0 !important;
		top: 100px !important;
	}
	#cmsAdminToolbox2 {
		right: 0 !important;
		bottom: 100px !important;
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
	}
	.cmsNavigationBlock img {
		border: 1px solid #CDE3D6 !important;
	}
	.cmsFooterLinks {
		color: #676F6A !important;
		padding: 5px !important;
		margin: 2px !important;
		font-weight: bold !important;
		margin-left: 10px !important;
		margin-right: 10px !important;
	}
	.cmsFooterP {
		text-align: center !important;
		padding: 5px !important;
		margin: 5px !important;
		line-height: 110% !important;
	}
</style>
<div style="text-align: center; background: #CDE3D6; padding: 5px; margin: 5px; border: 2px dashed #676F6A;">
	<p class="cmsFooterP">
		<a class="cmsFooterLinks" target="_blank" href="/Manage/PageAddEdit.aspx?id=<%=CurrentPageID %>">EDIT</a> 
		<a class="cmsFooterLinks" target="_top" href="<%=CurrentScriptName %>?carrotedit=true">ADVANCED EDIT</a> 
		<a class="cmsFooterLinks" target="_top" href="/Manage/PageIndex.aspx">PAGE INDEX</a> 
		<a class="cmsFooterLinks" target="_top" href="Manage/ModuleIndex.aspx">MODULE INDEX</a>
	</p>
	<p class="cmsFooterP">
		<a class="cmsFooterLinks" href="/">HOME PAGE</a> 
		<a class="cmsFooterLinks" runat="server" id="lnkParent" href="#">PARENT PAGE</a>
		<a class="cmsFooterLinks" runat="server" id="lnkCurrent" href="#">CURRENT PAGE</a>		
		<asp:Label ID="lblChildDDL" runat="server" CssClass="cmsFooterLinks">CHILD PAGES</asp:Label>
		<asp:DropDownList ID="ddlCMSLinks" runat="server" onchange="cmsNavPage(this);" DataTextField="NavMenuText"
			DataValueField="FileName" ValidationGroup="cmsMenuLinks">
		</asp:DropDownList>
	</p>
</div>

<script language="javascript" type="text/javascript">

	function cmsNavPage(y) {

		var url = y.options[y.selectedIndex].value;

		if (url != '00000') {

			//window.setTimeout('location.href = \'' + url + '?carrotedit=true&carrottick=<%=DateTime.Now.Ticks.ToString() %>\'', 500);

			window.setTimeout('location.href = \'' + url + '\'', 500);
		}
	}
</script>

<div id="cmsAdminToolbox1" class="cmsNavigationBlock">
	<div style="padding-top: 2px; padding-bottom: 2px; text-align: center;">
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a style="color: #676F6A; padding: 5px; margin: 0px; font-weight: bold;" target="_blank"
				href="/Manage/PageAddEdit.aspx?id=<%=CurrentPageID %>">
				<img border="0" alt="EDIT" title="EDIT" src="/manage/images/application_edit.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a title="ADVANCED EDIT" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top"
				href="<%=CurrentScriptName %>?carrotedit=true">
				<img border="0" alt="ADVANCED EDIT" title="ADVANCED EDIT" src="/manage/images/overlays.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a title="PAGE INDEX" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top"
				href="/Manage/PageIndex.aspx">
				<img border="0" alt="PAGE INDEX" title="PAGE INDEX" src="/manage/images/table.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 2px; text-align: center;">
			<a title="MODULE INDEX" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top"
				href="/Manage/ModuleIndex.aspx">
				<img border="0" alt="MODULE INDEX" title="MODULE INDEX" src="/manage/images/brick.png" /></a>
		</div>
	</div>
</div>
<div id="cmsAdminToolbox2" class="cmsNavigationBlock">
	<div style="padding-top: 2px; padding-bottom: 2px; text-align: center;">
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a style="color: #676F6A; padding: 5px; margin: 0px; font-weight: bold;" target="_blank"
				href="/Manage/PageAddEdit.aspx?id=<%=CurrentPageID %>">
				<img border="0" alt="EDIT" title="EDIT" src="/manage/images/application_edit.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a title="ADVANCED EDIT" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top"
				href="<%=CurrentScriptName %>?carrotedit=true">
				<img border="0" alt="ADVANCED EDIT" title="ADVANCED EDIT" src="/manage/images/overlays.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a title="PAGE INDEX" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top"
				href="/Manage/PageIndex.aspx">
				<img border="0" alt="PAGE INDEX" title="PAGE INDEX" src="/manage/images/table.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 2px; text-align: center;">
			<a title="MODULE INDEX" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top"
				href="/Manage/ModuleIndex.aspx">
				<img border="0" alt="MODULE INDEX" title="MODULE INDEX" src="/manage/images/brick.png" /></a>
		</div>
	</div>
</div>
