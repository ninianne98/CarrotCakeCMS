<%@ Page Language="C#" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPage" %>

<!--
Citrus Island 1.1 is is a free, W3C-compliant, CSS-based website template by styleshout.com.
This work is distributed under the Creative Commons Attribution 2.5 License, which means that you
are free to use and modify it for any purpose. All I ask is that you include a link back to my website in your credits.

http://www.styleshout.com/

Conversion to CarrotCake CMS Template: Carrotware
-->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="Silver" UseJqueryMigrate="true" />
	<meta name="description" />
	<meta name="keywords" />
	<link runat="server" id="theCSS" href="style.css" rel="stylesheet" type="text/css" media="screen" />
	<title>Citrus Island</title>
	<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
	<carrot:SiteCanonicalURL runat="server" ID="SiteCanonicalURL1" />
	<carrot:RSSFeed runat="server" ID="RSSFeed1" />
	<carrot:OpenGraph runat="server" ID="OpenGraph1" />
</head>
<body>
	<form id="form1" runat="server">
		<div id="wrap">
			<div id="header">
				<div class="search">
					<carrot:SearchBox ID="search1" runat="server">
						<SearchTemplate>
							<p>
								<asp:TextBox ID="SearchText" runat="server" CssClass="textbox" MaxLength="40" />
								<asp:Button ID="btnSiteSearch" runat="server" CssClass="button" Text="Search" />
							</p>
						</SearchTemplate>
					</carrot:SearchBox>
				</div>
				<h1 id="logo">
					<a href="/">
						<carrot:SiteDataProperty runat="server" ID="SiteDataProperty1" DataField="SiteName" /></a>
				</h1>
				<h2 id="slogan">
					<carrot:SiteDataProperty runat="server" ID="SiteDataProperty2" DataField="SiteTagline" />
				</h2>
			</div>
			<div id="menu-wrapper">
				<carrot:TwoLevelNavigation runat="server" ID="TwoLevelNavigation1" FontSize="10px" ForeColor="#FFFFFF" BackColor="#F4845A" />
			</div>
			<div style="clear: both;">
			</div>
			<div id="sidebar">
				<carrot:SiteMetaWordList ID="SiteMetaWordList1" HeadWrapTag="h1" runat="server" ContentType="Category" MetaDataTitle="Categories"
					CssClass="sidemenu" />
				<carrot:SiteMetaWordList ID="SiteMetaWordList2" HeadWrapTag="h1" runat="server" ContentType="Tag" MetaDataTitle="Tags" CssClass="sidemenu" />
				<carrot:WidgetContainer ID="phLeftTop" runat="server" />
				<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" TextZone="TextLeft" runat="server" />
				<carrot:WidgetContainer ID="phLeftBottom" runat="server" />
				<div style="clear: both;">
				</div>
			</div>
			<div id="main">
				<h1>
					<carrot:ContentPageProperty runat="server" ID="ContentPageProperty3" DataField="PageHead" /></h1>
				<p>
					By
				<carrot:AuthorLink runat="server" ID="AuthorLink1" />
					on
				<carrot:ContentPageProperty runat="server" ID="ContentPageProperty2" DataField="GoLiveDate" FieldFormat="{0:MMMM d, yyyy}" />
				</p>
				<carrot:WidgetContainer ID="phCenterTop" runat="server" />
				<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" TextZone="TextCenter" runat="server" />
				<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
				<div style="clear: both;">
				</div>
				<p>
					<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl1" runat="server" ContentType="Category"
						MetaDataTitle="Categories:" />
					<br />
					<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl2" runat="server" ContentType="Tag" MetaDataTitle="Tags:" />
				</p>
				<div>
					<carrot:ContentCommentForm runat="server" ID="commentFrm" NotifyEditors="true">
						<CommentEntryTemplate>
							<carrot:jsHelperLib runat="server" ID="jsHelperLib1" />
							<div>
								<asp:Label ID="ContentCommentFormMsg" runat="server" Text="" />
							</div>
							<div class="input-form">
								<p class="padding10">
									<label>
										name:
									<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="CommenterName"
										ErrorMessage="*" />
									</label>
									<asp:TextBox runat="server" ID="CommenterName" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" />
									<label>
										email:
									<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="CommenterEmail"
										ErrorMessage="*" />
									</label>
									<asp:TextBox runat="server" ID="CommenterEmail" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" />
									<label>
										website:
									</label>
									<asp:TextBox runat="server" ID="CommenterURL" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" />
									<label>
										comment:
									<asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="VisitorComments"
										ClientValidationFunction="__carrotware_ValidateLongText" EnableClientScript="true" ErrorMessage="**" />
									</label>
									<asp:TextBox runat="server" ID="VisitorComments" TextMode="MultiLine" Rows="8" Columns="40" MaxLength="1024" />
									<div class="padding10">
										<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="ContentCommentCaptcha"
											ErrorMessage="**" />
										<carrot:Captcha runat="server" ID="ContentCommentCaptcha" ValidationGroup="ContentCommentForm" CaptchaIsValidStyle-Style="clear: both; color: green;"
											CaptchaIsNotValidStyle-Style="clear: both; color: red;" CaptchaImageBoxStyle-Style="clear: both;" CaptchaInstructionStyle-Style="clear: both;"
											CaptchaTextStyle-Style="clear: both;" IsNotValidMessage="Code is not correct!" />
									</div>
									<div class="padding10">
										<asp:Button ID="SubmitCommentButton" CssClass="button padding10" runat="server" Text="Submit Comment" ValidationGroup="ContentCommentForm" />
									</div>
									<script type="text/javascript">
										__carrotware_PageValidate();
									</script>
								</p>
							</div>
						</CommentEntryTemplate>
					</carrot:ContentCommentForm>
					<div style="clear: both;">
					</div>
					<br />
					<hr />
					<div style="clear: both;">
					</div>
					<carrot:PagedComments ID="PagedComments1" runat="server" PageSize="5" CSSSelectedPage="selected">
						<ContentHeaderTemplate>
							<div>
						</ContentHeaderTemplate>
						<ContentTemplate>
							<div>
								<p>
									<b>
										<carrot:ListItemCommentText runat="server" ID="ListItemCommentText1" DataField="CommenterName" />
										on
									<carrot:ListItemCommentText runat="server" ID="ListItemCommentText2" DataField="CreateDate" FieldFormat="{0:d}" />
									</b>
								</p>
								<div class="comment-border top">
									<p>
										<carrot:ListItemCommentText runat="server" ID="ListItemCommentText3" DataField="PostCommentEscaped" />
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
							<carrot:ListItemWrapperForPager HtmlTagName="div" ID="ListItemWrapperForPager1" runat="server" CSSSelected="selectedwrap"
								CssClassNormal="pagerlink">
								<carrot:NavLinkForPagerTemplate ID="lnkBtn" CSSSelected="selected" runat="server" />
							</carrot:ListItemWrapperForPager>
						</PagerTemplate>
						<PagerFooterTemplate>
							</div>
						</PagerFooterTemplate>
					</carrot:PagedComments>
					<div style="clear: both;">
					</div>
				</div>
				<div style="clear: both;">
				</div>
			</div>
			<div id="rightbar">
				<div class="padding10">
					<div class="rsszone">
						<carrot:RSSFeed runat="server" ID="RSSFeed2" RSSFeedType="BlogOnly" CssClass="rssimage" RenderRSSMode="ImageLink" />
						&nbsp;&nbsp;&nbsp;&nbsp;
					<carrot:RSSFeed runat="server" ID="RSSFeed3" RSSFeedType="PageOnly" CssClass="rssimage" RenderRSSMode="ImageLink" />
					</div>
				</div>
				<div class="padding10 ">
					<div class="previous-next">
						<span class="nav-previous">
							<carrot:ContentPageNext runat="server" ID="prevLink" UseDefaultText="false" NavigationDirection="Prev"><span class="meta-nav">&larr;</span> Previous</carrot:ContentPageNext>
						</span><span class="nav-next">
							<carrot:ContentPageNext runat="server" ID="nextLink" UseDefaultText="false" NavigationDirection="Next"> Next <span class="meta-nav">&rarr;</span> </carrot:ContentPageNext>
						</span>
					</div>
				</div>
				<carrot:WidgetContainer ID="phRightTop" runat="server" />
				<carrot:ContentContainer EnableViewState="false" ID="BodyRight" TextZone="TextRight" runat="server" />
				<carrot:WidgetContainer ID="phRightBottom" runat="server" />
				<div style="clear: both;">
				</div>
			</div>
		</div>
		<div id="footer">
			<div id="footer-content">
				<div id="footer-right">
					<%--<a href="http://www.free-css.com/">Home</a> | <a href="http://www.free-css.com/">Site Map</a> | <a href="http://www.free-css.com/">
					RSS Feed</a>--%>
				</div>
				<asp:PlaceHolder ID="myFooter" runat="server">
					<div id="footer-left">
						<%=String.Format("&copy;  {0}, {1}. All rights reserved. ", DateTime.Now.Year, TheSite.SiteName.Trim()) %>
						| Site built with <a target="_blank" href="http://www.carrotware.com/">carrotcake cms</a>
						<br />
						Design by: <a target="_blank" href="http://www.styleshout.com/">styleshout</a>
						| Valid <a target="_blank" href="http://validator.w3.org/check/referer">XHTML</a>
						| <a target="_blank" href="http://jigsaw.w3.org/css-validator/check/referer">CSS</a>
					</div>
				</asp:PlaceHolder>
			</div>
		</div>
		<asp:Panel ID="pnlHiddenControls" Visible="false" runat="server">
		</asp:Panel>
	</form>
</body>
</html>