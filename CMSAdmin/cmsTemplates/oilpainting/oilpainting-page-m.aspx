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
			<carrot:ChildNavigation MetaDataTitle="Section Pages" CssClass="linkedList" CSSSelected="active" runat="server" ID="ChildNavigation1" />
		</li>
		<li>
			<carrot:SiblingNavigation MetaDataTitle="In This Section" CssClass="linkedList" CSSSelected="active" runat="server" ID="ChildNavigation2" />
		</li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageFootContentPlaceHolder" runat="server">
</asp:Content>
