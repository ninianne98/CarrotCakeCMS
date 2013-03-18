<%@ Control Language="C#" %>
<div id="cms_{ZONE_ID}" class="cmsContentContainer">
	<div class="cmsContentContainerTitle">
		<a title="Edit {ZONE_ID}" id="cmsContentAreaLink_{ZONE_ID}" class="cmsContentContainerInnerTitle" href="javascript:cmsShowEditContentForm('{ZONE_CHAR}','html'); ">
			Edit {ZONE_TYPE} ({ZONE_ID})<span class="cmsWidgetBarIconPencil2"></span></a>
	</div>
	<div class="cmsWidgetControl" id="cmsAdmin_{ZONE_ID}">
		<div>
			<asp:Literal ID="litContent" runat="server" />
		</div>
		<div style="clear: both;">
		</div>
	</div>
</div>
