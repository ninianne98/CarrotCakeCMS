<%@ Page Title="" Language="C#" MasterPageFile="abundant.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div id="content">
		<div class="post">
			<h2 class="title">
				<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="PageHead" /></h2>
			<div class="entry">
				<carrot:WidgetContainer ID="phCenterTop" runat="server">
				</carrot:WidgetContainer>
				<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
				<div style="clear: both;">
				</div>
				<carrot:PagedDataSummary ID="PagedDataSummary1" runat="server" ContentType="Blog" PageSize="8" CSSSelectedPage="selected">
					<ContentHeaderTemplate>
						<div>
					</ContentHeaderTemplate>
					<ContentTemplate>
						<div class="post">
							<h2 class="title">
								<carrot:ListItemNavText runat="server" ID="ListItemNavText5" DataField="NavMenuText" />
							</h2>
							<p class="date">
								<carrot:ListItemNavText runat="server" ID="ListItemNavText6" DataField="GoLiveDate" FieldFormat="{0:M.d.yyyy}" />
							</p>
							<div class="entry">
								<p>
									<carrot:ListItemNavText runat="server" ID="ListItemNavText2" DataField="PageTextPlainSummaryMedium" />
								</p>
								<p class="links">
									<carrot:NavLinkForTemplate CssClassNormal="comments" ID="NavLinkForTemplate3" runat="server" UseDefaultText="false">
										<carrot:ListItemNavText runat="server" ID="ListItemNavText7" DataField="CommentCount" FieldFormat="Comments ({0})" />
									</carrot:NavLinkForTemplate>
									&nbsp;&nbsp;&nbsp;
									<carrot:NavLinkForTemplate ID="NavLinkForTemplate2" runat="server" UseDefaultText="false">
										Full article</carrot:NavLinkForTemplate>
								</p>
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
							<carrot:NavLinkForPagerTemplate ID="lnkBtn" CSSSelected="selected" runat="server" RenderAsHyperlink="true" />
						</carrot:ListItemWrapperForPager>
					</PagerTemplate>
					<PagerFooterTemplate>
						</div>
					</PagerFooterTemplate>
				</carrot:PagedDataSummary>
				<div style="clear: both;">
				</div>
				<carrot:WidgetContainer ID="phCenterBottom" runat="server">
				</carrot:WidgetContainer>
			</div>
		</div>
	</div>
	<!-- end #content -->
	<div id="sidebar">
		<ul>
			<li>
				<carrot:SiteMetaWordList ID="SiteMetaWordList1" runat="server" ContentType="DateMonth" MetaDataTitle="Dates" TakeTop="12" />
			</li>
			<li>
				<carrot:SiteMetaWordList ID="SiteMetaWordList2" runat="server" ContentType="Category" MetaDataTitle="Categories" ShowUseCount="true" />
			</li>
			<li>
				<carrot:SiteMetaWordList ID="SiteMetaWordList3" runat="server" ContentType="Tag" MetaDataTitle="Tags" ShowUseCount="true" />
			</li>
		</ul>
	</div>
	<!-- end #sidebar -->
</asp:Content>
