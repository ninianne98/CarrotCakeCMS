<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryTest.aspx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryTest" %>

<%@ Register Src="PhotoGalleryFancyBox.ascx" TagName="FB" TagPrefix="uc1" %>
<%@ Register Src="PhotoGalleryFancyBox2.ascx" TagName="FB2" TagPrefix="uc1" %>
<%@ Register Src="PhotoGalleryAdminGalleryList.ascx" TagName="PhotoGalleryAdminGalleryList" TagPrefix="uc2" %>
<%@ Register Src="PhotoGalleryAdminCategory.ascx" TagName="PhotoGalleryAdminCategory" TagPrefix="uc2" %>
<%@ Register Src="PhotoGalleryAdmin.ascx" TagName="PhotoGalleryAdmin" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<carrot:jquerybasic runat="server" ID="jquerybasic" SelectedSkin="LightGreen" />
	<title>Test - Photo Gallery</title>
	<script type="text/javascript">
		$(document).ready(function () {
			$(function () {
				$("input:button, input:submit").button();

				$(".dateRegion").each(function (i) {
					$(this).datepicker({
						changeMonth: true,
						changeYear: true,
						showOn: 'button',
						buttonImage: '/c3-admin/images/calendar.gif',
						buttonImageOnly: true,
						constrainInput: true
					});
				});

			});
		});


		function ProcessKeyPress(e) {
			var obj = window.event ? event : e;
			var key = (window.event) ? event.keyCode : e.which;

			if ((key == 13) || (key == 10)) {
				obj.returnValue = false;
				obj.cancelBubble = true;
				return false;
			}
			return true;
		}


		function checkIntNumber(obj) {
			var n = obj.value;
			var intN = parseInt(n);
			if (isNaN(intN) || intN < 0 || n != intN) {
				alert("Value must be non-negative integers.");
				obj.focus();
			} else {
				obj.value = intN;
			}
		}

		function checkFloatNumber(obj) {
			var n = obj.value;
			var intN = parseFloat(n);
			if (isNaN(intN) || intN < 0 || n != intN) {
				alert("Value must be non-negative decimals.");
				obj.value = 0;
				obj.focus();
			} else {
				obj.value = intN;
			}
		}
	</script>
</head>
<body bgcolor="white">
	<form id="form1" runat="server">
	<uc1:FB ID="GalleryFancyBox1" runat="server" />
	<hr />
	<uc1:FB2 ID="GalleryFancyBox2" runat="server" />
	<hr />
	<a href="PhotoGalleryTest.aspx">PhotoGalleryTest.aspx</a>
	<hr />
	<uc2:PhotoGalleryAdminGalleryList ID="PhotoGalleryAdminGalleryList1" runat="server" SiteID="3BD253EA-AC65-4EB6-A4E7-BB097C2255A0" ModuleID="2D866DD3-54E1-434d-B77D-23CE7F07B934"
		QueryStringFragment="pf=GalleryList&pi=2D866DD3-54E1-434d-B77D-23CE7F07B934" QueryStringPattern="pf={0}&pi=2D866DD3-54E1-434d-B77D-23CE7F07B934" ModuleName="GalleryList" />
	<hr />
	<uc2:PhotoGalleryAdmin ID="PhotoGalleryAdmin1" runat="server" SiteID="3BD253EA-AC65-4EB6-A4E7-BB097C2255A0" ModuleID="2D866DD3-54E1-434d-B77D-23CE7F07B934"
		QueryStringFragment="pf=EditGalleryImageList&pi=2D866DD3-54E1-434d-B77D-23CE7F07B934" QueryStringPattern="pf={0}&pi=2D866DD3-54E1-434d-B77D-23CE7F07B934" ModuleName="EditGalleryImageList" />
	<hr />
	<uc2:PhotoGalleryAdminCategory ID="PhotoGalleryAdminCategory1" runat="server" SiteID="3BD253EA-AC65-4EB6-A4E7-BB097C2255A0" ModuleID="2D866DD3-54E1-434d-B77D-23CE7F07B934"
		QueryStringFragment="pf=CategoryEdit&pi=2D866DD3-54E1-434d-B77D-23CE7F07B934" QueryStringPattern="pf={0}&pi=2D866DD3-54E1-434d-B77D-23CE7F07B934" ModuleName="GalleryEdit" />
	<hr />
	<%--	<asp:ListBox DataTextField="FolderPath" DataValueField="FileName" Style="width: 600px" Rows="12" ID="lstFiles" runat="server">
	</asp:ListBox>--%>
	</form>
</body>
</html>
