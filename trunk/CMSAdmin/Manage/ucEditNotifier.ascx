<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEditNotifier.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.ucEditNotifier" %>
<div style="clear: both;">
	&nbsp;</div>
<div style="text-align: center; background: #CDE3D6; padding: 5px; margin: 5px; border: 2px dashed #676F6A;">
	<a style="color: #676F6A; padding: 5px; margin: 2px; font-weight: bold;" target="_blank" href="/Manage/PageAddEdit.aspx?id=<%=CurrentPageID %>">
		EDIT</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a style="color: #676F6A; padding: 5px; margin: 2px; font-weight: bold;"
			target="_top" href="<%=CurrentScriptName %>?carrotedit=true">ADVANCED EDIT</a>
</div>
<style type="text/css">
	#cmsAdminToolbox1 {
		position: fixed;
		left: 0;
		top: 20%;
		width: 8em;
		margin-top: -2.5em;
	}
	#cmsAdminToolbox2 {
		position: fixed;
		right: 0;
		top: 80%;
		width: 8em;
		margin-top: -2.5em;
	}
	#cmsAdminToolbox1 img, #cmsAdminToolbox2 img {
		border: 1px solid #CDE3D6 !important;
	}
</style>
<div id="cmsAdminToolbox1" style="padding: 0px; border: 2px dotted #676F6A; color: #676F6A; background: #CDE3D6; width: 30px;
	height: 65px; z-index: 1000; font-size: 14px;">
	<div style="padding-top: 2px; padding-bottom: 2px; text-align: center;">
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a style="color: #676F6A; padding: 5px; margin: 0px; font-weight: bold;" target="_blank" href="/Manage/PageAddEdit.aspx?id=<%=CurrentPageID %>">
				<img border="0" alt="EDIT" title="EDIT" src="/manage/images/application_edit.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 2px; text-align: center;">
			<a title="ADVANCED EDIT" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top" href="<%=CurrentScriptName %>?carrotedit=true">
				<img border="0" alt="ADVANCED EDIT" title="ADVANCED EDIT" src="/manage/images/overlays.png" /></a>
		</div>
	</div>
</div>
<div id="cmsAdminToolbox2" style="padding: 0px; border: 2px dotted #676F6A; color: #676F6A; background: #CDE3D6; width: 30px;
	height: 65px; z-index: 1000; font-size: 14px;">
	<div style="padding-top: 2px; padding-bottom: 2px; text-align: center;">
		<div style="padding-top: 2px; padding-bottom: 10px; text-align: center;">
			<a style="color: #676F6A; padding: 5px; margin: 0px; font-weight: bold;" target="_blank" href="/Manage/PageAddEdit.aspx?id=<%=CurrentPageID %>">
				<img border="0" alt="EDIT" title="EDIT" src="/manage/images/application_edit.png" /></a>
		</div>
		<div style="padding-top: 2px; padding-bottom: 2px; text-align: center;">
			<a title="ADVANCED EDIT" style="color: #676F6A; padding: 2px; margin: 2px;" target="_top" href="<%=CurrentScriptName %>?carrotedit=true">
				<img border="0" alt="ADVANCED EDIT" title="ADVANCED EDIT" src="/manage/images/overlays.png" /></a>
		</div>
	</div>
</div>
<%--

<carrot:jquery runat="server" ID="jquery1" />

<!-- 
<script src="/Manage/Includes/jq-floating-1.6.js" type="text/javascript"></script>

<div id="cmsAdminToolbox" style="clear: both; padding: 5px; margin: 5px; border: 2px dashed #676F6A; color: #676F6A;
	background: #CDE3D6; position: absolute; top: 20px; left: 20px; width: 150px; height: 150px; z-index: 1000; font-size: 14px;">
	<p style="padding-top: 50px; padding-bottom: 20px; text-align: center;">
		<a style="color: #676F6A; padding: 5px; margin: 2px;" href="<%=CurrentScriptName %>?carrotedit=true">ADVANCED
			EDIT</a></p>
	<table style="width: 145px; margin: 8px;">
		<tr>
			<td>
				<div onclick="cmsShiftPosition('L')" id="cmsMnuLeft" title="L" style="margin: 2px; padding-bottom: 10px;">
				&lt;&lt;
			</td>
			<td>
				<div onclick="cmsFloatCenter()" id="cmsMnuCenter" title="C" style="margin: 2px; padding-bottom: 10px;">
					^^
				</div>
			</td>
			<td>
				<div onclick="cmsShiftPosition('R')" id="cmsMnuRight" title="R" style="margin: 2px; padding-bottom: 10px;">
					&gt;&gt;
				</div>
			</td>
		</tr>
	</table>
</div>
-->


<!-- script language="javascript" type="text/javascript">
	$(document).ready(function() {

		$('#cmsAdminToolbox').addFloating({
			targetLeft: 25,
			targetTop: 25,
			snap: true
		});

		cmsFloatCenter();

	});


	function cmsFloatCenter() {
		floatingArray[0].targetTop = 30;
		floatingArray[0].targetBottom = undefined;
		floatingArray[0].targetLeft = undefined;
		floatingArray[0].targetRight = undefined;

		floatingArray[0].centerX = true;
		floatingArray[0].centerY = undefined;
	}


	function cmsShiftPosition(p) {
		floatingArray[0].targetTop = 30;
		floatingArray[0].targetBottom = undefined;
		if (p == 'L') {
			floatingArray[0].targetLeft = 30;
			floatingArray[0].targetRight = undefined;
		} else {
			floatingArray[0].targetLeft = undefined;
			floatingArray[0].targetRight = 30;
		}

		floatingArray[0].centerX = undefined;
		floatingArray[0].centerY = undefined;

	}	

</script -->

--%>
