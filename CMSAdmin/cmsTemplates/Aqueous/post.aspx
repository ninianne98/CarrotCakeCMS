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
				<span class="byline">By
					<carrot:ContentPageProperty runat="server" ID="ContentPageProperty2" DataField="Credit_FullName_FirstLast" />
					on
					<carrot:ContentPageProperty runat="server" ID="ContentPageProperty3" DataField="GoLiveDate" FieldFormat="{0:d}" />
				</span>
			</header>
			<carrot:WidgetContainer ID="phCenterTop" runat="server" />
			<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
			<div id="meta-zone">
				<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl1" runat="server" ContentType="Category" MetaDataTitle=" " />
				<br />
				<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl2" runat="server" ContentType="Tag" MetaDataTitle=" " />
			</div>
			<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
		</section>
	</div>
	<!-- Content -->
</asp:Content>