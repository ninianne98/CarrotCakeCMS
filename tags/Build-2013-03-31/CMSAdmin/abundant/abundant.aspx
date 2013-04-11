<%@ Page Title="" Language="C#" MasterPageFile="abundant.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div id="content">
		<div class="post">
			<h2 class="title">
				<carrot:ContentPageProperty runat="server" ID="ContentPageProperty3" DataField="PageHead" /></h2>
			<div class="entry">
				<p>
					By
					<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="Author_FullName_FirstLast" />
					on
					<carrot:ContentPageProperty runat="server" ID="ContentPageProperty2" DataField="GoLiveDate" FieldFormat="{0:MMMM d, yyyy}" />
				</p>
				<carrot:WidgetContainer ID="phCenterTop" runat="server">
				</carrot:WidgetContainer>
				<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
				<carrot:WidgetContainer ID="phCenterBottom" runat="server">
				</carrot:WidgetContainer>
			</div>
		</div>
	</div>
	<!-- end #content -->
	<div id="sidebar">
		<ul>
			<li>
				<carrot:ChildNavigation MetaDataTitle="Child Pages" runat="server" ID="SecondLevelNavigation2" />
			</li>
			<li>
				<carrot:SecondLevelNavigation MetaDataTitle="Section Pages" runat="server" ID="SecondLevelNavigation1" />
			</li>
			<li>
				<carrot:MostRecentUpdated MetaDataTitle="Recent Updates" runat="server" ID="MostRecentUpdated1" />
			</li>
		</ul>
	</div>
	<!-- end #sidebar -->
</asp:Content>
