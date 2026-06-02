<%@ Page Title="" Language="C#" MasterPageFile="oilpainting.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageMainContentPlaceHolder" runat="server">
	<carrot:WidgetContainer ID="phCenterTop" runat="server" />
	<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
	<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
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
			<h2>Pages</h2>
			<carrot:TopLevelNavigation runat="server" ID="TopLevelNavigation1" />
		</li>
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
