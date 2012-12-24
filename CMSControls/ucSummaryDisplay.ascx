<%@ Control Language="C#" %>
<div id="divPost" runat="server" class="post-detail">
	<p>
		<b>
			<carrot:NavLinkForTemplate CssClassNormal="green" ID="NavLinkForTemplate1" runat="server" UseDefaultText="true" />
			&nbsp;|&nbsp;
			<carrot:ListItemNavText runat="server" ID="ListItemNavText1" DataField="GoLiveDate" FieldFormat="{0:d}" />
		</b>
		<br />
		<carrot:ListItemNavText runat="server" ID="ListItemNavText2" DataField="PageTextPlainSummary" />
	</p>
	<p>
		<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wplCat" runat="server" ContentType="Category" MetaDataTitle="Categories:" />
		<br />
		<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpltag" runat="server" ContentType="Tag" MetaDataTitle="Tags:" />
	</p>
	<p class="post-footer">
		<carrot:NavLinkForTemplate CssClassNormal="readmore" ID="NavLinkForTemplate2" runat="server" UseDefaultText="false">
			Read more</carrot:NavLinkForTemplate>
		&nbsp;&nbsp;&nbsp; <span class="comments">Comments
			<carrot:ListItemNavText runat="server" ID="ListItemNavText4" DataField="CommentCount" FieldFormat=" ({0}) " />
		</span>&nbsp;&nbsp;&nbsp; <span class="date">
			<carrot:ListItemNavText runat="server" ID="ListItemNavText3" DataField="GoLiveDate" FieldFormat="{0:MMM d, yyyy}" />
		</span>
	</p>
</div>
