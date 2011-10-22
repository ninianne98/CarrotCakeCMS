<%@ Page Title="" Language="C#" MasterPageFile="/Manage/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="ControlPropertiesEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.ControlPropertiesEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Edit Properties
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:Repeater ID="rpProps" runat="server">
		<ItemTemplate>
			<div>
				<%# String.Format("{0}", Eval("Name"))%>
				&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:TextBox ID="txtValue" Width="300px" runat="server" Text='<%# GetSavedValue( String.Format( "{0}", Eval("DefValue")), String.Format( "{0}", Eval("Name")) ) %>'></asp:TextBox>
				<asp:HiddenField runat="server" ID="hdnName" Value='<%# String.Format( "{0}", Eval("Name") ) %>' />
			</div>
		</ItemTemplate>
	</asp:Repeater>
	<br />
	<p>
		<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
	</p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxBodyContentPlaceHolder" runat="server">
</asp:Content>
