﻿<%@ Page Title="Tag Add/Edit" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="TagAddEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.TagAddEdit" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = cmsGetServiceAddress();

		var thePageID = '<%= guidItemID %>';

		var tValid = '#<%= txtFileValid.ClientID %>';
		var tValidSlug = '#<%= txtSlug.ClientID %>';
		var tCaption = '#<%= txtLabel.ClientID %>';

		var thePage = '';

		function CheckSlug() {
			thePage = $(tValidSlug).val();

			$(tValid).val('');

			var webMthd = webSvc + "/ValidateUniqueTag";
			var myPage = MakeStringSafe(thePage);

			$.ajax({
				type: "POST",
				url: webMthd,
				data: JSON.stringify({ TheSlug: myPage, ItemID: thePageID }),
				contentType: "application/json; charset=utf-8",
				dataType: "json"
			}).done(editSlugCallback)
				.fail(cmsAjaxFailed);
		}

		function GenerateSlug() {
			var theSlug = $(tValidSlug).val();

			var webMthd = webSvc + "/GenerateCategoryTagSlug";

			if (theSlug.length < 1) {
				var theText = $(tCaption).val();
				var mySlug = MakeStringSafe(theText);

				$.ajax({
					type: "POST",
					url: webMthd,
					data: JSON.stringify({ TheSlug: mySlug, Mode: 'Tag' }),
					contentType: "application/json; charset=utf-8",
					dataType: "json"
				}).done(editSlug)
					.fail(cmsAjaxFailed);

			} else {
				CheckSlug();
			}
		}

		$(document).ready(function () {
			setTimeout("CheckSlug();", 250);
			cmsIsPageValid();
		});

		function editSlugCallback(data, status) {
			if (data.d != "FAIL" && data.d != "OK") {
				cmsAlertModal(data.d);
			}

			if (data.d == "OK") {
				$(tValid).val('VALID');
				$(tValidSlug).removeClass('validationExclaimBox');
			} else {
				$(tValid).val('NOT VALID');
				$(tValidSlug).addClass('validationExclaimBox');
			}

			var ret = cmsIsPageValid();
			cmsForceInputValidation('<%= txtFileValid.ClientID %>');
		}

		function editSlug(data, status) {
			if (data.d == "FAIL") {
				cmsAlertModal(data.d);
			} else {
				$(tValidSlug).val(data.d);
			}

			CheckSlug();
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Tag Add/Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<br />
	<table class="table-lg">
		<tr>
			<td style="width: 90px;" class="tablecaption">caption:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="GenerateSlug()" ID="txtLabel" runat="server" Columns="45"
					MaxLength="100" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtLabel" ID="RequiredFieldValidator3"
					runat="server" Text="**" ToolTip="Required" ErrorMessage="Required" Display="Dynamic" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">url slug:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="CheckSlug()" ID="txtSlug" runat="server" Columns="45"
					MaxLength="100" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtSlug" ID="RequiredFieldValidator1"
					runat="server" Text="**" ToolTip="Required" ErrorMessage="Required" Display="Dynamic" />
				<asp:CompareValidator ValidationGroup="inputForm" CssClass="validationExclaim" ForeColor="" ControlToValidate="txtFileValid" ID="CompareValidator1"
					runat="server" ErrorMessage="not valid/not unique" ToolTip="not valid/not unique" Text="##" Display="Dynamic" ValueToCompare="VALID" Operator="Equal" />
				<div style="display: none;">
					<asp:TextBox runat="server" ValidationGroup="inputForm" ID="txtFileValid" MaxLength="25" Columns="25" />
					<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtFileValid" ID="RequiredFieldValidator2"
						runat="server" ErrorMessage="Slug validation is required" ToolTip="Slug validation is required" Display="Dynamic" Text="**" />
				</div>
			</td>
		</tr>
		<tr>
			<td class="tablecaption">public:
			</td>
			<td>
				<asp:CheckBox ID="chkPublic" runat="server" />
			</td>
		</tr>
	</table>
	<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="return SubmitPage()" Text="Save" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<input type="button" id="btnCancel" value="Cancel" onclick="cancelEditing()" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:Button ValidationGroup="deleteForm" ID="btnDeleteButton" runat="server" OnClientClick="return DeleteItem()" Text="Delete" />
	<br />
	<div style="display: none;">
		<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
		<asp:Button ValidationGroup="deleteForm" ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" />
	</div>
	<script type="text/javascript">

		function DeleteItem() {
			var opts = {
				"No": function () { cmsAlertModalClose(); },
				"Yes": function () { ClickDeleteItem(); }
			};

			cmsAlertModalSmallBtns('Are you sure you want to delete this item?  This will untag any content using this keyword.', opts);

			return false;
		}

		function ClickDeleteItem() {
			$('#<%=btnDelete.ClientID %>').click();
		}

		function SubmitPage() {
			var ret = cmsIsPageValid();
			setTimeout("ClickSaveBtn();", 800);
			return ret;
		}

		function ClickSaveBtn() {
			if (cmsIsPageValid()) {
				$('#<%=btnSave.ClientID %>').click();
		}
	}

	function cancelEditing() {
		window.setTimeout("location.href = './TagIndex.aspx';", 250);
	}
	</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>