<%@ Control Language="C#" %>
<div id="ID-{WIDGET_KEY}" class="cmsWidgetContainerWrapper">
	<div id="cms_{WIDGET_ID}" class="cmsGlossySeaGreen">
		<div id="cmsWidgetHead" class="cmsWidgetTitleBar">
			<div id="cmsControlPath" title="{WIDGET_PATH}" tooltip="{WIDGET_PATH}">
				{WIDGET_TITLE}</div>
			<div id="cmsEditMenuList">
				<div id="cmsEditMenuList-inner">
					<ul class="cmsMnuParent">
						<li class="cmsWidgetCogIcon"><a title="" alt="Modify" class="cmsWidgetBarLink cmsWidgetBarIconCog" id="cmsWidgetBarIcon" href="javascript:void(0);">M</a>
							<ul class="cmsMnuChildren">
								<asp:PlaceHolder ID="phMenuItems" runat="server" />
								<li runat="server" id="liEdit"><a title="Edit" alt="Edit" class="cmsWidgetBarLink cmsWidgetBarIconPencil" id="cmsContentEditLink" href="javascript:{WIDGET_JS}">
									Edit </a></li>
								<li runat="server" id="liHistory"><a title="History" alt="History" class="cmsWidgetBarLink cmsWidgetBarIconWidget2" id="cmsContentHistoryLink" href="javascript:cmsManageWidgetHistory('{WIDGET_KEY}')">
									History </a></li>
								<li runat="server" id="liRemove"><a title="Remove" alt="Remove" class="cmsWidgetBarLink cmsWidgetBarIconCross" id="cmsContentRemoveLink" href="javascript:cmsRemoveWidgetLink('{WIDGET_KEY}');">
									Remove </a></li>
							</ul>
						</li>
					</ul>
				</div>
			</div>
		</div>
	</div>
	<div class="cmsWidgetControl cmsWidgetControlItem" id="cmsControl">
		<input type="hidden" id="cmsCtrlID" value="{WIDGET_KEY}" />
		<input type="hidden" id="cmsCtrlOrder" value="{WIDGET_ORDER}" />
		<asp:PlaceHolder ID="phWidgetZone" runat="server" />
		<div style="clear: both;">
		</div>
	</div>
</div>
