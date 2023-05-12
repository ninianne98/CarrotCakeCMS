﻿<%@ Page Title="Edit Properties" ValidateRequest="false" Language="C#" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="ControlPropertiesEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.ControlPropertiesEdit" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script src="MiniColors/jquery.minicolors.min.js"></script>
	<link href="MiniColors/jquery.minicolors.css" rel="stylesheet" />

	<script type="text/javascript">
		function setColorSwatch() {
			$('.color-field').minicolors({
				format: 'hex',
				letterCase: 'lowercase',
				theme: 'default'
			});
		}
		function colorAjax() {
			if (typeof (Sys) != 'undefined') {
				var prm = Sys.WebForms.PageRequestManager.getInstance();
				prm.add_endRequest(function () {
					setColorSwatch();
				});
			}
		}

		$(document).ready(function () {
			colorAjax();
			setColorSwatch();
		});
	</script>

	<style type="text/css">
		.minicolors-swatch-color, .minicolors-theme-default .minicolors-swatch {
			height: 28px;
			width: 28px;
			margin-left: 12px;
			border-radius: 50%;
			display: inline-block;
			left: auto;
		}

		.minicolors-swatch::after {
			box-shadow: none;
		}

		.minicolors-swatch {
			border: 1px solid #555555;
			cursor: pointer;
		}

		.minicolors-theme-default .minicolors-input {
			padding-left: 8px;
			padding-right: 8px;
		}
	</style>
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
					<asp:CheckBoxList ID="chkValues" runat="server" Visible="false" />
					<asp:CheckBox ID="chkValue" runat="server" Visible="false" />
					<asp:TextBox ID="txtValue" Width="400px" runat="server" Text='<%# GetSavedValue( String.Format( "{0}", Eval("DefValue")), String.Format( "{0}", Eval("Name")) ) %>' />
					<asp:HiddenField runat="server" ID="hdnName" Value='<%# String.Format( "{0}", Eval("Name") ) %>' />
				</div>
				<div style="clear: both;">
				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
	<br />
	<div style="margin-top: 25px;">
		<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Apply" />
		<br />
	</div>
	<div style="display: none;">
		<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Apply" />
	</div>
	<script type="text/javascript">
		function SubmitPage() {
			var ret = cmsPreSaveTrigger();
			setTimeout("ClickBtn();", 800);
		}
		function ClickBtn() {
			$('#<%=btnSave.ClientID %>').click();
		}
	</script>
	<p>
		<br />
		&nbsp;<br />
	</p>
	<div style="width: 350px; height: 250px; clear: both;"></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>