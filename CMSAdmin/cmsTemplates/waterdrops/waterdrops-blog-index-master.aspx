<%@ Page Title="" Language="C#" MasterPageFile="waterdrops.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageMainContentPlaceHolder" runat="server">
	<h2 class="title">
		<carrot:ContentPageProperty runat="server" ID="ContentPageProperty10" DataField="PageHead" />
	</h2>
	<div class="entry">
		<carrot:WidgetContainer ID="phCenterTop" runat="server">
		</carrot:WidgetContainer>
		<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />

		<div style="clear: both;">
		</div>
		<carrot:PagedDataSummary ID="PagedDataSummary1" runat="server" ContentType="Blog" PageSize="5" CSSSelectedPage="selected">
			<ContentHeaderTemplate>
				<div>
			</ContentHeaderTemplate>
			<ContentTemplate>
				<div class="post">
					<h2 class="title">
						<carrot:NavLinkForTemplate CssClassNormal="readmore" ID="NavLinkForTemplate4" runat="server" UseDefaultText="true" />
					</h2>
					<div class="entry">
						<p class="postmeta">
							Posted by
							<carrot:ListItemNavText runat="server" ID="ListItemNavText11" DataField="Author_FullName_FirstLast" />
							|
							<carrot:ListItemNavText runat="server" ID="ListItemNavText1" DataField="GoLiveDate" FieldFormat="{0:MMM d, yyyy}" />
							|
							<carrot:NavLinkForTemplate CssClassNormal="readmore" ID="NavLinkForTemplate3" runat="server" UseDefaultText="false">
								<carrot:ListItemNavText runat="server" ID="ListItemNavText4" DataField="CommentCount" FieldFormat=" {0} " />
								comments
							</carrot:NavLinkForTemplate>
						</p>
						<p>
							<carrot:ListItemNavText runat="server" ID="ListItemNavText2" DataField="PageTextPlainSummary" />
						</p>
						<p>
							<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl1" runat="server" ContentType="Category" MetaDataTitle="Categories:" />
							<br />
							<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl2" runat="server" ContentType="Tag" MetaDataTitle="Tags:" />
						</p>
						<p class="readmore">
							<carrot:NavLinkForTemplate CssClassNormal="readmore" ID="NavLinkForTemplate2" runat="server" UseDefaultText="false">
								read more
							</carrot:NavLinkForTemplate>
						</p>
					</div>
				</div>
				<!-- post -->
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

		<carrot:WidgetContainer ID="phCenterBottom" runat="server">
		</carrot:WidgetContainer>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageSideContentPlaceHolder" runat="server">
	<div>
		<carrot:SiteMetaWordList ID="SiteMetaWordList1" runat="server" CssClass="list-style1" ContentType="DateMonth" MetaDataTitle="Archive" TakeTop="6" />
	</div>
	<div>
		<carrot:SiteMetaWordList ID="SiteMetaWordList2" runat="server" CssClass="list-style1" ContentType="Category" MetaDataTitle="Categories" ShowUseCount="true" />
	</div>
	<div>
		<carrot:SiteMetaWordList ID="SiteMetaWordList3" runat="server" CssClass="list-style1" ContentType="Tag" MetaDataTitle="Tags" ShowUseCount="true" />
	</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageFootContentPlaceHolder" runat="server">
	<p>&nbsp;</p>
</asp:Content>