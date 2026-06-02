<%@ Page Title="" Language="C#" MasterPageFile="oilpainting.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageMainContentPlaceHolder" runat="server">
	<asp:PlaceHolder ID="PlaceHolder2" runat="server">
		<p>
			Posted By
			<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="Author_FullName_FirstLast" />
			on
			<carrot:ContentPageProperty runat="server" ID="ContentPageProperty2" DataField="GoLiveDate" FieldFormat="{0:MMMM d, yyyy}" />
		</p>
	</asp:PlaceHolder>
	<carrot:WidgetContainer ID="phCenterTop" runat="server" />
	<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
	<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
	<div style="clear: both;">
		<carrot:PostMetaWordList CssClass="meta" HtmlTagNameInner="li" HtmlTagNameOuter="ul" ID="wpl1" runat="server" ContentType="Category" MetaDataTitle="Categories:" />
		<carrot:PostMetaWordList CssClass="meta" HtmlTagNameInner="li" HtmlTagNameOuter="ul" ID="wpl2" runat="server" ContentType="Tag" MetaDataTitle="Tags:" />
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
