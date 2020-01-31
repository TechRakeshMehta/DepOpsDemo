<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicantOrderNotificationHistoryGridControl.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ApplicantOrderNotificationHistoryGridControl" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style>
    .classImageMail {
        background: url(../../App_Themes/Default/images/mail.png);
        background-position: 0 0;
        background-repeat: no-repeat;
        width: 20px;
        height: 20px;
    }

    .classImagePdf {
        background: url(../../images/medium/pdf.gif);
        background-position: 0 0;
        background-repeat: no-repeat;
        width: 20px;
        height: 20px;
    }
</style>
<script type="text/javascript">


    var winopen = false;
    function openMailPopUp(sender) {
        var btnID = sender.get_id();
        var containerID = btnID.substr(0, btnID.indexOf("btnNotificationMail"));
        var hdnfSystemCommunicationID = $jQuery("[id$=" + containerID + "hdnfSystemCommunicationID]").val();
        var composeScreenWindowName = "Notification Mail Details";

        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/BkgOperations/Pages/NotificationMailDetails.aspx?SystemCommunicationID=" + hdnfSystemCommunicationID);
        var win = $window.createPopup(url, { size: "520," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
        winopen = true;
        return false;
    }

    function openPdfPopUp(sender) { 
        var btnID = sender.get_id();
        var containerID = btnID.substr(0, btnID.indexOf("btnNotificationPdf"));
        var hdnfSystemDocumentId = $jQuery("[id$=" + containerID + "hdnfSystemDocumentId]").val();
        var documentType = "ServiceFormDocument";
        var composeScreenWindowName = "Service Form Details";
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?systemDocumentId=" + hdnfSystemDocumentId + "&DocumentType=" + documentType);
        var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientPdfClose });
        winopen = true;
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            winopen = false;
        }
    }

    function OnClientPdfClose(oWnd, args) {
        oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            winopen = false;
        }
    }

</script>

<div class="row">
    <div class="col-md-12">
        <h2 class="header-color">Service Forms Notification History</h2>
    </div>
</div>
<div class="row">
    <infs:WclGrid runat="server" ID="grdApplicantNotificationHistory" AutoGenerateColumns="False"  AllowSorting="false" 
        EnableAriaSupport="true" OnNeedDataSource="grdApplicantNotificationHistory_NeedDataSource" AllowPaging="true"
        AutoSkinMode="True" CellSpacing="0" GridLines="Both" ShowClearFiltersButton="false" OnItemCommand="grdApplicantNotificationHistory_ItemCommand">
        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="true"></Selecting>
        </ClientSettings>
        <ValidationSettings ValidationGroup="grpFormEdit" EnableValidation="true" />
        <MasterTableView CommandItemDisplay="Top" DataKeyNames="NotificationId,SystemCommunicationId,OrderId,OrderNumber,NotificationType"
          AllowSorting="false" AllowFilteringByColumn="false">
            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
            <Columns>
                <telerik:GridTemplateColumn HeaderText="Mail" UniqueName="imgMail">
                    <ItemTemplate>
                        <telerik:RadButton ID="btnNotificationMail" OnClientClicked="openMailPopUp" AutoPostBack="false" CssClass="classImageMail"
                            runat="server" Font-Underline="true">
                            <Image EnableImageButton="true" />
                        </telerik:RadButton>
                        <asp:HiddenField ID="hdnfSystemCommunicationID" runat="server" Value='<%#Eval("SystemCommunicationId") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Form" UniqueName="imgServiceForm">
                    <ItemTemplate>
                        <telerik:RadButton ID="btnNotificationPdf" OnClientClicked="openPdfPopUp" AutoPostBack="false" CssClass="classImagePdf"
                            runat="server" Font-Underline="true">
                            <Image EnableImageButton="true" />
                        </telerik:RadButton>
                        <asp:HiddenField ID="hdnfSystemDocumentId" runat="server" Value='<%#Eval("SystemDocumentId")%>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="OrderNumber" FilterControlAltText="Filter Order Number column" AllowFiltering="false"
                    HeaderText="Order Number" SortExpression="OrderNumber" UniqueName="OrderNumber">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="NotificationType" FilterControlAltText="Filter NotificationType column" AllowFiltering="false"
                    HeaderText="Notification Type" SortExpression="NotificationType" UniqueName="NotificationType">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="NotificationDetail" FilterControlAltText="Filter NotificationDetail column" AllowFiltering="false"
                    HeaderText="Notification Detail" SortExpression="NotificationDetail" UniqueName="NotificationDetail">
                </telerik:GridBoundColumn>
                <telerik:GridDateTimeColumn DataField="CreatedDate" FilterControlAltText="Filter CreatedDate column" AllowFiltering="false"
                    HeaderText="Create Date" SortExpression="CreatedDate" UniqueName="CreatedDate">
                </telerik:GridDateTimeColumn>
                <telerik:GridBoundColumn DataField="SentBy" FilterControlAltText="Filter SentBy column" AllowFiltering="false"
                    HeaderText="Sent By" SortExpression="SentBy" UniqueName="SentBy">
                </telerik:GridBoundColumn>
                <telerik:GridButtonColumn ButtonType="LinkButton" Text="Send" HeaderText="Resend" UniqueName="ShowResendBtn"
                    CommandName="ResendMail">
                </telerik:GridButtonColumn>
            </Columns>
        </MasterTableView>
    </infs:WclGrid>
</div> 