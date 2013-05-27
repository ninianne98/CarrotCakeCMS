<%@ Control Language="C#" %>
<div id="cms_{ZONE_ID}" class="cmsContentContainer">
	<div class="cmsContentContainerTitle" id="cmsContentContainerHead">
		<div id="cmsContentEditTitle" class="cmsContentContainerTitleText">
			{ZONE_TYPE} ({ZONE_ID})
		</div>
		<div id="cmsEditMenuList" class="cmsContentContainerMenu">
			<ul class="cmsContentContainerMenu cmsMnuParent">
				<li class="cmsWidgetCogIcon"><a title="" alt="Modify" class="cmsWidgetBarLink cmsWidgetBarIconCog" id="cmsWidgetBarIcon" href="javascript:void(0);">M</a>
					<ul class="cmsContentContainerMenu cmsMnuChildren">
						<li runat="server" id="cmsContentAreaLink1_{ZONE_ID}"><a title="Edit HTML" alt="Edit HTML" class="cmsWidgetBarLink cmsWidgetBarIconPencil2" id="cmsContentEditLink1"
							href="javascript:cmsShowEditContentForm('{ZONE_CHAR}','html'); ">Edit HTML</a></li>
						<li runat="server" id="cmsContentAreaLink2_{ZONE_ID}"><a title="Edit Text" alt="Edit Text" class="cmsWidgetBarLink cmsWidgetBarIconPencil2" id="cmsContentEditLink2"
							href="javascript:cmsShowEditContentForm('{ZONE_CHAR}','plain'); ">Edit Text </a></li>
					</ul>
				</li>
			</ul>
		</div>
	</div>
	<div class="cmsWidgetControl" id="cmsAdmin_{ZONE_ID}">
		<div>
			<asp:Literal ID="litContent" runat="server" />
		</div>
		<div style="clear: both;">
		</div>
	</div>
</div>
