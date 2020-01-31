<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageServiceItemDetail.aspx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageServiceItemDetail"  MaintainScrollPositionOnPostback="true"
    Title="Manage Service Item Detail" MasterPageFile="~/Shared/ChildPage.master" %>



<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/BkgSetup/UserControl/ManageSrvcItemEntityRecord.ascx" TagPrefix="infsu" TagName="ManageSrvcItemEntityRecord" %>
<%@ Register TagName="EditServiceItem" TagPrefix="infsu" Src="~/BkgSetup/UserControl/EditServiceItemDetail.ascx" %>
<%@ Register TagName="RuleSetList" TagPrefix="infsu" Src="~/BkgSetup/UserControl/RulesetListBkg.ascx" %>
<%@ Register TagName="ManageSvcItemFeeItems" TagPrefix="infsu" Src="~/BkgSetup/UserControl/ManageSvcItemFeeItems.ascx" %>
<%@ Register TagName="ManageSvcItemCustomForms" TagPrefix="infsu" Src="~/BkgSetup/UserControl/ManageSvcItemCustomForms.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="divBasicFields" runat="server">
        <infsu:EditServiceItem ID="ucEditServiceItem" runat="server" />
    </div>
    <infs:WclButton ID="btnHidden" runat="server" Visible="false"></infs:WclButton>
    <hr style="border-bottom: solid 1px #c0c0c0;" />
    <div id="dvServiceItemEntityRecord" runat="server">
        <infsu:ManageSrvcItemEntityRecord runat="server" ID="ucSrvcItemEntityRecords" />
    </div>
    <hr style="border-bottom: solid 1px #c0c0c0;" />
    <div id="dvFeeItems" runat="server">
        <infsu:ManageSvcItemFeeItems runat="server" ID="ucSvcItemFeeItem" />
    </div>
    <hr style="border-bottom: solid 1px #c0c0c0;" />
    <div id="dvRuleset" runat="server">
        <infsu:RuleSetList runat="server" ID="ucRuleSetList" />
    </div>
     <hr style="border-bottom: solid 1px #c0c0c0;" />
    <div id="dvCustomForms" runat="server">
        <infsu:ManageSvcItemCustomForms runat="server" ID="ucSvcItemCustomForms" />
    </div>
    <div style="display:none">
        <asp:Button ID ="btnDoPostBack" runat="server" OnClick="btnDoPostBack_Click" />
    </div>
    <script language="javascript" type="text/javascript">
        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

        function RefreshPage() {
            //debugger;
            var btn = $jQuery('[id$=btnbtnDoPostBack]', $jQuery(parent.theForm));
            btn.click();
        }
        $jQuery(document).ready(function () {
            parent.ResetTimer();
            parent.Page.hideProgress();
        });
    </script>
</asp:Content>

