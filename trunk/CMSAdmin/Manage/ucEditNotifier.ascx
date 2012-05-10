<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEditNotifier.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.ucEditNotifier" %>
<div style="clear: both;">
	&nbsp;</div>
<style type="text/css">
	#cmsAdminToolbox1 {
		position: fixed;
		left: 0;
		top: 25%;
		width: 8em;
		margin-top: -2.5em;
	}
	#cmsAdminToolbox2 {
		position: fixed;
		right: 0;
		top: 75%;
		width: 8em;
		margin-top: -2.5em;
	}
	#cmsAdminToolbox1 img, #cmsAdminToolbox2 img {
		border: 1px solid #CDE3D6 !important;
	}
	#cmsFooterLinks {
		color: #676F6A;
		padding: 5px;
		margin: 2px;
		font-weight: bold;
		margin-left: 10px;
		margin-right: 10px;
	}
</style>
<div style="text-align: center; background: #CDE3D6; padding: 5px; margin: 5px; border: 2px dashed #676F6A;">
	<a id="cmsFooterLinks" target="_blank" href="/Manage/PageAddEdit.aspx?id=<%=CurrentPageID %>">EDIT</a> 
	<a id="cmsFooterLinks" target="_top" href="<%=CurrentScriptName %>?carrotedit=true">ADVANCED EDIT</a> 
	<a id="cmsFooterLinks" target="_top" href="/Manage/PageIndex.aspx">PAGE INDEX</a> 
	<a id="cmsFooterLinks" target="_top" href="Manage/ModuleIndex.aspx">MODULE INDEX</a>
	<p style="text-align: center; padding: 5px; margin: 5px;">
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
<div id="cmsAdminToolbox1" style="padding: 0px; border: 2px dotted #676F6A; color: #676F6A; background: #CDE3D6; width: 30px;
	height: 125px; z-index: 1000; font-size: 14px;">
	<div style="padding-top: 2px; padding-bottom: 2px; text-align: center;">
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a style="color: #676F6A; padding: 5px; margin: 0px; font-weight: bold;" target="_blank" href="/Manage/PageAddEdit.aspx?id=<%=CurrentPageID %>">
				<img border="0" alt="EDIT" title="EDIT" src="/manage/images/application_edit.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a title="ADVANCED EDIT" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top" href="<%=CurrentScriptName %>?carrotedit=true">
				<img border="0" alt="ADVANCED EDIT" title="ADVANCED EDIT" src="/manage/images/overlays.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a title="PAGE INDEX" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top" href="/Manage/PageIndex.aspx">
				<img border="0" alt="PAGE INDEX" title="PAGE INDEX" src="/manage/images/table.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 2px; text-align: center;">
			<a title="MODULE INDEX" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top" href="/Manage/ModuleIndex.aspx">
				<img border="0" alt="MODULE INDEX" title="MODULE INDEX" src="/manage/images/brick.png" /></a>
		</div>
	</div>
</div>
<div id="cmsAdminToolbox2" style="padding: 0px; border: 2px dotted #676F6A; color: #676F6A; background: #CDE3D6; width: 30px;
	height: 125px; z-index: 1000; font-size: 14px;">
	<div style="padding-top: 2px; padding-bottom: 2px; text-align: center;">
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a style="color: #676F6A; padding: 5px; margin: 0px; font-weight: bold;" target="_blank" href="/Manage/PageAddEdit.aspx?id=<%=CurrentPageID %>">
				<img border="0" alt="EDIT" title="EDIT" src="/manage/images/application_edit.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a title="ADVANCED EDIT" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top" href="<%=CurrentScriptName %>?carrotedit=true">
				<img border="0" alt="ADVANCED EDIT" title="ADVANCED EDIT" src="/manage/images/overlays.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a title="PAGE INDEX" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top" href="/Manage/PageIndex.aspx">
				<img border="0" alt="PAGE INDEX" title="PAGE INDEX" src="/manage/images/table.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 2px; text-align: center;">
			<a title="MODULE INDEX" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top" href="/Manage/ModuleIndex.aspx">
				<img border="0" alt="MODULE INDEX" title="MODULE INDEX" src="/manage/images/brick.png" /></a>
		</div>
	</div>
</div>
