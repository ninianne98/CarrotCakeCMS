<%@ Page Title="" Language="C#" MasterPageFile="imagination.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {

		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div class="row">
		<div class="3u">
			<section class="sidebar">
				<carrot:PostCalendar CalendarHead="Calendar" runat="server" ID="calendar" />
			</section>
			<section class="sidebar">
				<carrot:SiteMetaWordList ID="SiteMetaWordList1" runat="server" HeadWrapTag="H2" ContentType="DateMonth" MetaDataTitle="Dates" ShowUseCount="true" TakeTop="12"
					CssClass="default" />
			</section>
			<section class="sidebar">
				<carrot:SiteMetaWordList ID="SiteMetaWordList2" runat="server" HeadWrapTag="H2" ContentType="Category" MetaDataTitle="Categories" ShowUseCount="true" CssClass="default" />
			</section>
			<section class="sidebar">
				<carrot:SiteMetaWordList ID="SiteMetaWordList3" runat="server" HeadWrapTag="H2" ContentType="Tag" MetaDataTitle="Tags" ShowUseCount="true" CssClass="default" />
			</section>
		</div>
		<div class="9u skel-cell-important">
			<section>
				<header>
					<h2>
						<carrot:ContentPageProperty runat="server" ID="ContentPageProperty3" DataField="PageHead" /></h2>
				</header>
				<article>
					<carrot:WidgetContainer ID="phCenterTop" runat="server" />
					<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
					<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
				</article>
			</section>
			<section>
				<carrot:PagedDataSummary ID="PagedDataSummary1" runat="server" ContentType="Blog" PageSize="8" CSSSelectedPage="selected">
					<ContentHeaderTemplate>
						<div>
					</ContentHeaderTemplate>
					<ContentTemplate>
						<div>
							<h3>
								<carrot:ListItemNavText runat="server" ID="ListItemNavText5" DataField="NavMenuText" />
								|
								<carrot:ListItemNavText runat="server" ID="ListItemNavText6" DataField="GoLiveDate" FieldFormat="{0:M.d.yyyy}" />
							</h3>
							<div>
								<p>
									<carrot:ListItemNavText runat="server" ID="ListItemNavText2" DataField="PageTextPlainSummary" />
								</p>
								<carrot:NavLinkForTemplate ID="NavLinkForTemplate2" runat="server" UseDefaultText="false" CssClassNormal="button small">
									Full article
								</carrot:NavLinkForTemplate>
								<br />
							</div>
						</div>
					</ContentTemplate>
					<ContentFooterTemplate>
						</div>
					</ContentFooterTemplate>
					<PagerHeaderTemplate>
						<br />
						<div class="pagerfooterlinks">
					</PagerHeaderTemplate>
					<PagerTemplate>
						<carrot:ListItemWrapperForPager HtmlTagName="div" ID="wrap" runat="server" CSSSelected="selectedwrap" CssClassNormal="pagerlink">
							<carrot:NavLinkForPagerTemplate ID="lnkBtn" CSSSelected="selected" runat="server" RenderAsHyperlink="true" />
						</carrot:ListItemWrapperForPager>
					</PagerTemplate>
					<PagerFooterTemplate>
						</div>
					</PagerFooterTemplate>
				</carrot:PagedDataSummary>
			</section>
		</div>
	</div>
</asp:Content>
