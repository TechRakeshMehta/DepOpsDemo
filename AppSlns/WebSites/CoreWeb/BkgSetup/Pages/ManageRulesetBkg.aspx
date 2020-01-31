<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageRulesetBkg.aspx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageRulesetBkg"  MaintainScrollPositionOnPostback="true"
    Title="Manage RuleSet" MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="RuleSetDetail" Src="~/BkgSetup/UserControl/RuleSetInfoBkg.ascx" %>
<%@ Register TagPrefix="infsu" TagName="RuleList" Src="~/BkgSetup/UserControl/RuleListBkg.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclButton ID="btnHidden" runat="server" Visible="false"></infs:WclButton>

    <div id="divBasicFields" runat="server">
        <infsu:RuleSetDetail ID="ucRuleSetDetail" runat="server" />
    </div>

    <hr style="border-bottom: solid 1px #c0c0c0;" />
    <div id="dvRuleList" runat="server">
        <infsu:RuleList runat="server" ID="ucRuleList" />
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
        function OnCancelClientClick(sender, eventArgs) {
            $jQuery("[id$=hdnIsCancelRequest]")[0].value = true;
        }
    </script>
</asp:Content>
