<%@ Page Title="" Language="C#" MasterPageFile="oilpainting.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageMainContentPlaceHolder" runat="server">
	<carrot:WidgetContainer ID="phCenterTop" runat="server" />
	<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
	<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
	<div style="clear: both;">
	</div>
	<carrot:PagedDataSummary ID="PagedDataSummary1" runat="server" ContentType="Blog" PageSize="5" CSSSelectedPage="selected">
		<ContentHeaderTemplate>
			<div>
		</ContentHeaderTemplate>
		<ContentTemplate>
			<div class="post">
				<h2 class="title">
					<carrot:NavLinkForTemplate ID="NavLinkForTemplate1" runat="server" UseDefaultText="true" />
				</h2>
				<p class="meta">
					<span class="date">
						<carrot:ListItemNavText runat="server" ID="ListItemNavText1" DataField="GoLiveDate" FieldFormat="{0:MMMM d, yyyy}" />
					</span><span class="posted">Posted by
							<carrot:ListItemNavText runat="server" ID="ListItemNavText5" DataField="Author_FullName_FirstLast" />
					</span>
				</p>
				<div class="entry">
					<p>
						<carrot:ListItemNavText runat="server" ID="ListItemNavText2" DataField="PageTextPlainSummary" />
					</p>
					<p>
						<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="PostMetaWordList1" runat="server" ContentType="Category" MetaDataTitle="Categories:" />
						<br />
						<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="PostMetaWordList2" runat="server" ContentType="Tag" MetaDataTitle="Tags:" />
					</p>
					<p class="links">
						<carrot:NavLinkForTemplate CssClassNormal="readmore" ID="NavLinkForTemplate2" runat="server" UseDefaultText="false">
							Read more
						</carrot:NavLinkForTemplate>
						&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp; Comments
													<carrot:ListItemNavText runat="server" ID="ListItemNavText6" DataField="CommentCount" FieldFormat=" ({0}) " />
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
				<carrot:NavLinkForPagerTemplate ID="lnkBtn" CSSSelected="selected" runat="server" />
			</carrot:ListItemWrapperForPager>
		</PagerTemplate>
		<PagerFooterTemplate>
			</div>
		</PagerFooterTemplate>
	</carrot:PagedDataSummary>
	<div style="clear: both;">
	</div>
	<carrot:WidgetContainer ID="phRightTop" runat="server" />
	<carrot:ContentContainer EnableViewState="false" ID="BodyRight" runat="server" />
	<carrot:WidgetContainer ID="phRightBottom" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageSideContentPlaceHolder" runat="server">
	<carrot:WidgetContainer ID="phLeftTop" runat="server" />
	<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" runat="server" />
	<carrot:WidgetContainer ID="phLeftBottom" runat="server" />
	<ul>
		<li>
			<carrot:SiteMetaWordList ID="SiteMetaWordList1" runat="server" ContentType="DateMonth" MetaDataTitle="Dates" TakeTop="14" />
		</li>
		<li>
			<carrot:SiteMetaWordList ID="SiteMetaWordList2" runat="server" ContentType="Category" MetaDataTitle="Categories" ShowUseCount="true" />
		</li>
		<li>
			<carrot:SiteMetaWordList ID="SiteMetaWordList3" runat="server" ContentType="Tag" MetaDataTitle="Tags" ShowUseCount="true" />
		</li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageFootContentPlaceHolder" runat="server">
</asp:Content>
