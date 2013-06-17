<%@ Control Language="C#" %>
<div id="cms-outer_{WIDGETCONTAINER_ID}" class="cmsWidgetTargetOuterControl">
	<div class="cmsWidgetControlTitle">
		<div class="cmsWidgetControlIDZone">
			<div id="cmsWidgetContainerName">
				{WIDGETCONTAINER_ID}</div>
			<div id="cmsEditMenuList" class="cmsWidgetContainerMenu">
				<ul class="cmsWidgetContainerMenu cmsMnuParent">
					<li class="cmsWidgetCogIcon"><a title="" alt="Modify" class="cmsWidgetBarLink cmsWidgetBarIconCog" id="cmsWidgetBarIcon" href="javascript:void(0);">M</a>
						<ul class="cmsWidgetContainerMenu cmsMnuChildren">
							<li><a title="Widgets" alt="Widgets" class="cmsWidgetBarLink cmsWidgetBarIconWidget" id="cmsContentEditLink" href="javascript:cmsManageWidgetList('{WIDGETCONTAINER_ID}')">
								Widgets </a></li>
							<li runat="server" id="liCopy"><a title="Copy From" alt="Copy From" class="cmsWidgetBarLink cmsWidgetBarIconCopy" id="A1" href="javascript:cmsCopyWidgetFrom('{WIDGETCONTAINER_ID}')">
								Copy From </a></li>
						</ul>
					</li>
				</ul>
			</div>
		</div>
	</div>
	<div class="cmsTargetArea cmsTargetMove cmsWidgetControl" id="cms_{WIDGETCONTAINER_ID}">
