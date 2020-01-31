<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderSubscriptionHistory.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.OrderSubscriptionHistory" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="Subscriptionhistory" Src="~/ComplianceOperations/PackageSubscription.ascx" %>
<%@ Register TagPrefix="uc" TagName="OrderHistory" Src="~/ComplianceOperations/OrderHistory.ascx" %>
 <infs:WclResourceManagerProxy runat="server" ID="rprxOrderSubscriptionHistory">
        <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/OrderSubscriptionHistory.js" ResourceType="JavaScript" />
     </infs:WclResourceManagerProxy>
<div onclick="setPostBackSourceOSH(event, this);">
    <uc:OrderHistory ID="ucOrderHistory" runat="server"></uc:OrderHistory>
    <uc:Subscriptionhistory ID="ucSubscriptionhistory" runat="server"></uc:Subscriptionhistory>
    <asp:TextBox ID="hdnPostbacksource" class="postbacksource" runat="server" Style="display: none;" />
</div>
