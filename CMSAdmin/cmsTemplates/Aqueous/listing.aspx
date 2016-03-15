<%@ Page Title="" Language="C#" MasterPageFile="aqueous.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<%@ Register Src="navBlog.ascx" TagPrefix="uc" TagName="navBlog" %>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<!-- Sidebar -->
	<div id="sidebar" class="4u">
		<uc:navBlog runat="server" ID="navBlog1" />
	</div>
	<!-- Sidebar -->
	<!-- Content -->
	<div id="content" class="8u skel-cell-important">
		<section>
			<header>
				<h2>
					<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="PageHead" /></h2>
			</header>
			<carrot:WidgetContainer ID="phCenterTop" runat="server" />
			<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
			<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
			<div>
				<div style="clear: both;">
				</div>
				<carrot:PagedDataSummary ID="PagedDataSummary1" runat="server" ContentType="Blog" PageSize="10" CSSSelectedPage="selected" LinkNext="lnkNextWrap"
					LinkPrev="lnkPrevWrap" LinkFirst="lnkFirstWrap" LinkLast="lnkLastWrap">
					<TypeLabelPrefixes>
						<carrot:PagedDataSummaryTitleOption KeyValue="DateIndex" LabelText="Date:" />
						<carrot:PagedDataSummaryTitleOption KeyValue="DateDayIndex" LabelText="Day:" FormatText="{0:dddd, d MMMM yyyy}" />
						<carrot:PagedDataSummaryTitleOption KeyValue="DateMonthIndex" LabelText="Month:" FormatText="{0:MMM yyyy}" />
						<carrot:PagedDataSummaryTitleOption KeyValue="DateYearIndex" LabelText="Year:" FormatText="{0:yyyy}" />
						<carrot:PagedDataSummaryTitleOption KeyValue="CategoryIndex" LabelText="Category:" />
						<carrot:PagedDataSummaryTitleOption KeyValue="TagIndex" LabelText="Tag:" />
						<carrot:PagedDataSummaryTitleOption KeyValue="AuthorIndex" LabelText="Content by " />
						<carrot:PagedDataSummaryTitleOption KeyValue="SearchResults" LabelText="Search results for:" FormatText=" [ {0} ] " />
					</TypeLabelPrefixes>
					<EmptyDataTemplate>
						<p>
							<b>No results found.</b>
						</p>
					</EmptyDataTemplate>
					<ContentHeaderTemplate>
						<h1>
							Contents</h1>
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
									Full article</carrot:NavLinkForTemplate>
								<br />
							</div>
						</div>
					</ContentTemplate>
					<ContentFooterTemplate>
						</div>
					</ContentFooterTemplate>
					<PagerHeaderTemplate>
						<div class="pagerfooterlinks">
					</PagerHeaderTemplate>
					<PagerTemplate>
						<carrot:ListItemWrapperForPager HtmlTagName="div" ID="wrap" runat="server" CSSSelected="selectedwrap" CssClassNormal="pagerlink">
							<carrot:NavLinkForPagerTemplate ID="lnkBtn" CSSSelected="selected" runat="server" />
						</carrot:ListItemWrapperForPager>
					</PagerTemplate>
					<PagerFooterTemplate>
						</div>
					</PagerFooterTemplate>
				</carrot:PagedDataSummary>
				<div style="clear: both;">
				</div>
			</div>
		</section>
	</div>
	<!-- Content -->
</asp:Content>