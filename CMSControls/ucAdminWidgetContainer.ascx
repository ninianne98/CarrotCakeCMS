<%@ Control Language="C#" %>
<div style="clear: both;">
</div>
<div id="cms-outer_{WIDGETCONTAINER_ID}" class="cmsWidgetTargetOuterControl">
	<div class="cmsWidgetControlTitle">
		<div class="cmsWidgetControlIDZone">
			<div id="cmsWidgetContainerName">
				{WIDGETCONTAINER_ID}</div>
			<div id="cmsEditMenuList">
				<div id="cmsEditMenuList-inner">
					<ul class="cmsMnuParent">
						<li class="cmsWidgetCogIcon"><a class="cmsWidgetBarLink cmsWidgetBarIconCog" id="cmsWidgetBarIcon" href="javascript:void(0);">Modify</a>
							<ul class="cmsMnuChildren">
								<li><a class="cmsWidgetBarLink cmsWidgetBarIconWidget" id="cmsContentEditLink" href="javascript:cmsManageWidgetList('{WIDGETCONTAINER_ID}')">Widgets </a>
								</li>
							</ul>
						</li>
					</ul>
				</div>
			</div>
		</div>
	</div>
	<div class="cmsTargetArea cmsTargetMove cmsWidgetControl" id="cms_{WIDGETCONTAINER_ID}">
		<asp:PlaceHolder ID="phWidgetZone" runat="server" />
		<div style="clear: both;">
			&nbsp;
		</div>
	</div>
</div>
<div style="clear: both;">
</div>
