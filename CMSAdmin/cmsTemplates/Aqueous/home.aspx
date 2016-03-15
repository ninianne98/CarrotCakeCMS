<%@ Page Title="" Language="C#" MasterPageFile="aqueous.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<%@ Register Src="navBlog.ascx" TagPrefix="uc" TagName="navBlog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {
			setBodyCSS('homepage');
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BannerBodyContentPlaceHolder" runat="server">
	<div id="banner">
		<div class="container">
			<div class="row">
				<section>
					<a href="#" class="image full">
						<img src="css/images/pics01.jpg" alt="" runat="server"></a>
				</section>
			</div>
		</div>
	</div>
</asp:Content>
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
		<carrot:PagedDataSummary ID="PagedDataSummary2" HideSpanWrapper="true" runat="server" ContentType="Blog" PageSize="5" ShowPager="false"
			IgnoreSitePath="true">
			<ContentHeaderTemplate>
				<ul class="style1 cal-lst">
			</ContentHeaderTemplate>
			<ContentTemplate>
				<li>
					<p class="date">
						<carrot:NavLinkForTemplate ID="NavLinkForTemplate1" runat="server" UseDefaultText="false">
							<carrot:ListItemNavText runat="server" ID="ListItemNavText2" DataField="GoLiveDate" FieldFormat=" {0:MMMM} " />
							<b>
								<carrot:ListItemNavText runat="server" ID="ListItemNavText3" DataField="GoLiveDate" FieldFormat=" {0:dd} " />
							</b>
						</carrot:NavLinkForTemplate>
					</p>
					<p>
						<carrot:ListItemNavText ID="ListItemNavText1" runat="server" DataField="NavMenuText" />
						<br style="clear: both" />
					</p>
				</li>
			</ContentTemplate>
			<ContentFooterTemplate>
				</ul>
			</ContentFooterTemplate>
		</carrot:PagedDataSummary>
		<uc:navBlog runat="server" ID="navBlog1" />
	</div>
</asp:Content>