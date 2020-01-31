<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageServiceDetail.aspx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageServiceDetail" MaintainScrollPositionOnPostback="true"
    Title="Manage Service Detail" MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagName="ucServiceItem" TagPrefix="infsu" Src="~/BkgSetup/UserControl/ManageServiceItem.ascx" %>
<%@ Register TagName="RuleSetList" TagPrefix="infsu" Src="~/BkgSetup/UserControl/RulesetListBkg.ascx" %>
<%@ Register TagName="ServiceDetail" TagPrefix="infsu" Src="~/BkgSetup/UserControl/EditServiceDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="dvServiceDetail" runat="server">
        <infsu:ServiceDetail ID="ucServiceDetail" runat="server" />
    </div>
    <hr style="border-bottom: solid 1px #c0c0c0;" />
    <div id="divAddServiceItem" runat="server">
        <infsu:ucServiceItem ID="ucServiceItemDetail" runat="server" />
    </div>
    <hr style="border-bottom: solid 1px #c0c0c0;" />
    <div id="dvRuleset" runat="server">
        <infsu:RuleSetList runat="server" ID="ucRuleSetList" />
    </div>
    <div style="display: none">
        <infs:WclButton runat="server" ID="btnEdit" Text="Page Is Under Progress.."
            Height="30px" AutoPostBack="false" ButtonType="SkinnedButton">
        </infs:WclButton>
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
