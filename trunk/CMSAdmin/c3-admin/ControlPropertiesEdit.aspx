<%@ Page Title="Edit Properties" Language="C#" MasterPageFile="/c3-admin/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="ControlPropertiesEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.ControlPropertiesEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Edit Properties
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:Repeater ID="rpProps" runat="server" OnItemDataBound="rpProps_Bind">
		<ItemTemplate>
			<div style="padding-bottom: 5px;">
				<div style="float: left; padding-right: 10px;">
					<b>
						<%# String.Format("{0}", Eval("FieldDescription"))%></b>
				</div>
				<div style="clear: both;">
				</div>
			</div>
			<div style="padding-bottom: 20px;">
				<div style="float: left; padding-right: 50px;">
					<%# String.Format("{0}", Eval("Name"))%>
				</div>
				<div style="float: left;">
					<asp:DropDownList ID="ddlValue" runat="server" Visible="false" />
					<asp:CheckBoxList ID="chkValues" runat="server" Visible="false">
					</asp:CheckBoxList>
					<asp:CheckBox ID="chkValue" runat="server" Visible="false" />
					<asp:TextBox ID="txtValue" Width="300px" runat="server" Text='<%# GetSavedValue( String.Format( "{0}", Eval("DefValue")), String.Format( "{0}", Eval("Name")) ) %>' />
					<asp:HiddenField runat="server" ID="hdnName" Value='<%# String.Format( "{0}", Eval("Name") ) %>' />
				</div>
				<div style="clear: both;">
				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
	<br />
	<p>
		<asp:Button ID="btnSave" runat="server" Text="Apply" OnClick="btnSave_Click" />
	</p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
