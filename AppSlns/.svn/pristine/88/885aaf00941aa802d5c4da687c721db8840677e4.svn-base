<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BkgPackagePriceSetUp.aspx.cs" Inherits="CoreWeb.BkgSetup.Views.BkgPackagePriceSetUp"
    Title="Manage Package Detail" MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagName="ucBkgpackage" TagPrefix="infsu" Src="~/BkgSetup/UserControl/ManageBkgPackageDetails.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/AdditionalDocumentsMapping.ascx" TagPrefix="infsu" TagName="AdditionalDocumentsMapping" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divAddPackageDetails" runat="server">
        <infsu:ucBkgpackage ID="ucBkgpackageDetails" runat="server" />
    </div>
    <hr style="border-bottom: solid 1px #c0c0c0;" />
    <div id="dvRuleset" runat="server">
    </div>
    <div style="display: none">
        <infs:WclButton runat="server" ID="btnEdit" Text="Page Is Under Progress.."
            Height="30px" AutoPostBack="false" ButtonType="SkinnedButton">
        </infs:WclButton>
    </div>
    <div class="section" id="dvAdditionalDocuments" runat="server">
        <h1 class="mhdr">
            <asp:Label ID="Label4" runat="server" Text="Map Additional Documents"></asp:Label>
        </h1>
        <div class="content">
            <infsu:AdditionalDocumentsMapping runat="server" ID="AdditionalDocumentsMapping" />
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }
        $jQuery(document).ready(function () {
            parent.ResetTimer();
            parent.Page.hideProgress();
        });
    </script>
</asp:Content>
