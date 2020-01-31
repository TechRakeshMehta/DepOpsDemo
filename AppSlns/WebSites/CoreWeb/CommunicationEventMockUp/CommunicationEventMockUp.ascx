<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.CommunicationEventMockUp.Views.CommunicationEventMockUp" Codebehind="CommunicationEventMockUp.ascx.cs" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<h1>
    Communication Event MockUp</h1>
<table style="width: 100%">
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <h2>
                            Notifications</h2>
                    </td>
                </tr>
                <tr>
                    <td>
                        <infs:WclButton ID="btnNotificationOrdeCreationr" runat="server" Text="Notification For Order Creation"
                            Width="200px" onclick="btnNotificationOrdeCreationr_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnNotificationOrderApproval" runat="server" Text="Notification For Order Approval"
                            Width="200px" onclick="btnNotificationOrderApproval_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnNotificationOrderCompletion" runat="server" Text="Notification For Order Completion "
                            Width="225px" onclick="btnNotificationOrderCompletion_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnNotificationCreditCard" runat="server" Text="Notification For Credit Card"
                            Width="200px" onclick="btnNotificationCreditCard_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnNotificationMoneyOrder" runat="server" Text="Notification For Money Order"
                            Width="200px" onclick="btnNotificationMoneyOrder_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnNotificationMBSpayment" runat="server" Text="Notification For MBS Payment"
                            Width="200px" onclick="btnNotificationMBSpayment_Click">
                        </infs:WclButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <infs:WclButton ID="btnNotificationNewSubscription" runat="server" Text="Notification For New Subscription"
                            Width="200px" onclick="btnNotificationNewSubscription_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnNotificationExpiredSubscription" runat="server" Text="Notification For Expired Subscription"
                            Width="200px" onclick="btnNotificationExpiredSubscription_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnNotificationRenewableSubscription" runat="server" Text="Notification For Renewable Subscription"
                            Width="225px" onclick="btnNotificationRenewableSubscription_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnNotificationProfileChanges" runat="server" Text="Notification For Profile Changes"
                            Width="200px" onclick="btnNotificationProfileChanges_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnNotificationAccountStatus" runat="server" Text="Notification For Account Status"
                            Width="200px" onclick="btnNotificationAccountStatus_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnNotificationInternalMessages" runat="server" Text="Notification For Internal Messages"
                            Width="200px">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <h2>
                            Alerts</h2>
                    </td>
                </tr>
                <tr>
                    <td>
                        <infs:WclButton ID="btnAlertOrderDenied" runat="server" Text="Alert For Order Denied"
                            Width="200px" onclick="btnAlertOrderDenied_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnAlertSubscriptionExpire" runat="server" Text="Alert For Subscription Expire"
                            Width="200px" onclick="btnAlertSubscriptionExpire_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnAlertProfileChangese" runat="server" Text="Alert For Profile Changes"
                            Width="200px" onclick="btnAlertProfileChangese_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnAlertAccountStatus" runat="server" Text="Alert For Account Status"
                            Width="200px" onclick="btnAlertAccountStatus_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnAlertApplicantStatus" runat="server" Text="Alert For Applicant's compliance status"
                            Width="230px" onclick="btnAlertApplicantStatus_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnAlertStartEvent" runat="server" Text="Alert For Start of Event"
                            Width="200px" onclick="btnAlertStartEvent_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <infs:WclButton ID="btnAlertEventModifications" runat="server" Text="Alert For Event Modifications"
                            Width="200px" onclick="btnAlertEventModifications_Click">
                        </infs:WclButton>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <h2>
                            Reminders</h2>
                    </td>
                </tr>
                <tr>
                    <td>
                        <infs:WclButton ID="btnReminderSubscriptionExpire" runat="server" Text="Reminder For Subscriptions Expire"
                            Width="200px" onclick="btnReminderSubscriptionExpire_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <infs:WclButton ID="btnReminderApplicantComplianceItemExpire" runat="server" Text="Reminder For Applicant's Compliance Item Expire"
                            Width="290px" onclick="btnReminderApplicantComplianceItemExpire_Click">
                        </infs:WclButton>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
