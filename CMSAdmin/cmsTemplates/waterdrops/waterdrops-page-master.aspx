<%@ Page Title="" Language="C#" MasterPageFile="waterdrops.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageMainContentPlaceHolder" runat="server">
	<h2 class="title">
		<carrot:ContentPageProperty runat="server" ID="ContentPageProperty10" DataField="PageHead" />
	</h2>
	<div class="entry">
		<carrot:WidgetContainer ID="phCenterTop" runat="server">
		</carrot:WidgetContainer>
		<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
		<carrot:WidgetContainer ID="phCenterBottom" runat="server">
		</carrot:WidgetContainer>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageSideContentPlaceHolder" runat="server">
	<div>
		<carrot:ChildNavigation MetaDataTitle="Child Pages" CssClass="list-style1" CSSSelected="active" runat="server" ID="ChildNavigation1" />
	</div>
	<div>
		<carrot:SiblingNavigation MetaDataTitle="In This Section" CssClass="list-style1" CSSSelected="active" runat="server" ID="ChildNavigation2" />
	</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageFootContentPlaceHolder" runat="server">
	<p>&nbsp;</p>
</asp:Content>