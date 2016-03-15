<%@ Page Title="" Language="C#" MasterPageFile="aqueous.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<%@ Register Src="navPage.ascx" TagPrefix="uc" TagName="navPage" %>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<!-- Content -->
	<div id="content" class="8u skel-cell-important">
		<section>
			<header>
				<h2>
					<carrot:ContentPageProperty runat="server" ID="ContentPageProperty3" DataField="PageHead" /></h2>
			</header>
			<carrot:WidgetContainer ID="phCenterTop" runat="server" />
			<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
			<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
		</section>
	</div>
	<!-- Sidebar -->
	<div id="sidebar" class="4u">
		<uc:navPage runat="server" ID="navPage1" />
	</div>
</asp:Content>