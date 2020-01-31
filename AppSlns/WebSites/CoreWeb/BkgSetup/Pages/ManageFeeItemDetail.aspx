<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageFeeItemDetail.aspx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageFeeItemDetail"  MaintainScrollPositionOnPostback="true"
    Title="Manage Fee Item Detail" MasterPageFile="~/Shared/ChildPage.master" %>



<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/BkgSetup/UserControl/EditFeeItemDetail.ascx" TagPrefix="infsu" TagName="EditFeeItemDetail" %>
<%@ Register Src="~/BkgSetup/UserControl/ManageFeeItemFeeRecords.ascx" TagPrefix="infsu" TagName="ManageFeeItemFeeRecords" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="divFeeEditFields" runat="server">
        <infsu:EditFeeItemDetail ID="ucEditFeeItemDetail" runat="server" />
    </div>
    <infs:WclButton ID="btnHidden" runat="server" Visible="false"></infs:WclButton>
    <hr style="border-bottom: solid 1px #c0c0c0;" />
    <div id="dvFeeRecords" runat="server">
        <infsu:ManageFeeItemFeeRecords ID="ucManageFeeItemFeeRecords" runat="server" />
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
