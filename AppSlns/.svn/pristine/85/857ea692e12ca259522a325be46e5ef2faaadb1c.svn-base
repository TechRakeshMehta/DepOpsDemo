<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEntry.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.DataEntry" %>
<%@ Register TagPrefix="uc" TagName="DataEntryItem" Src="DataEntryItem.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<style>
    .borderB {
        font-size: 12px;
    }

    .sxcbar .RadButton .rbPrimary {
        padding-left: 16px;
    }

    .rbSkinnedButton {
        margin-right: 4px !important;
    }

    #pageoutwr {
        overflow-y: hidden !important;
        overflow-x: hidden !important;
    }

    #UpdatePanel1 {
        height: 83% !important;
    }

    .rspPane.rspFirstItem > div {
        height: 100% !important;
    }

    #ctl00_DefaultContent_ucDynamicControl_sptrMain {
        height: 95% !important;
    }

    .sptrMain > div {
        height: 100% !important;
    }

    .width17 {
        width: 17%;
    }

    .width83 {
        width: 83%;
    }

    .left {
        float: left;
    }

    .docfont {
        font-style: normal;
        font-size: 13px;
    }

    .wordwrap {
        word-wrap: break-word;
    }

    .nowrap {
        white-space: nowrap;
    }
</style>
<infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/AdminDataEntry.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/AdminDataEntry.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/verification.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<asp:UpdatePanel ID="pnlErrorDataEntry" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="msgbox" id="pageMsgBoxDataEntry" style="overflow-y: auto; max-height: 400px">
            <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblError"></asp:Label>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="section">
    <h1 class="mhdr">
        <span class="width17 left">
            <asp:Label ID="lblAdminDataEntry" runat="server" Text=""></asp:Label></span>
        <span class="width83 left"><span class="docfont nowrap">(Document Name:</span>
            &nbsp<asp:Label ID="lblDocName" CssClass="docfont wordwrap" runat="server" Text=""></asp:Label>)</span>
    </h1>

    <infs:WclSplitter ID="sptrMain" runat="server" CssClass="sptrMain" LiveResize="true" Width="99.5%" Height="100%" Orientation="Vertical" ResizeWithParentPane="true">
        <infs:WclPane ID="pnLeft" runat="server" Width="49%" Scrolling="None">
            <infs:WclSplitter ID="sptrLeftPanel" runat="server" LiveResize="true" CssClass="heighFixed" Width="100%" OnClientResized="RadPaneResized" Orientation="Horizontal" ResizeWithParentPane="true">
                <infs:WclPane ID="paneHeader" runat="server" Width="100%" Scrolling="Y" Height="10%">
                    <div class="">
                        <div class="">
                            <asp:Panel ID="Panel1" runat="server">
                                <table>
                                    <thead>
                                    </thead>
                                </table>
                            </asp:Panel>
                        </div>
                        <div class="gclr">
                        </div>
                    </div>
                </infs:WclPane>
                <infs:WclPane ID="paneTop" runat="server" Width="100%" Scrolling="Both" Height="83%">
                    <div class="">
                        <div class="">
                            <asp:Panel ID="pnl" runat="server" Visible="true">
                            </asp:Panel>
                        </div>
                        <div class="gclr">
                        </div>
                    </div>
                </infs:WclPane>
                <infs:WclPane ID="paneBottom" runat="server" Height="7%" Width="100%">
                    <infsu:CommandBar ID="fsucCmdBar" runat="server" DisplayButtons="None" AutoPostbackButtons="Submit,Save,Clear"
                        ButtonPosition="Left">
                        <ExtraCommandButtons>
                            <infs:WclButton ID="btnSaveTemp" runat="server" Text="Save and Return to Queue" Enabled="false"
                                OnClick="btnSaveTemp_Click" CssClass="btnLeftMarg" UseSubmitBehavior="false">
                                <Icon PrimaryIconCssClass="rbSave" />
                            </infs:WclButton>
                            <infs:WclButton ID="btnBackToQueue" runat="server" Text="Back to Queue" ButtonType="StandardButton"
                                OnClick="btnCancel_Click" CssClass="btnLeftMarg" UseSubmitBehavior="false">
                                <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                                    PrimaryIconWidth="14" />
                            </infs:WclButton>
                            <infs:WclButton ID="btnSaveDone" runat="server" Text="Save and Done" Enabled="false"
                                OnClick="btnSave_Click" CssClass="btnLeftMarg" UseSubmitBehavior="false">
                                <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                                    PrimaryIconWidth="14" />
                            </infs:WclButton>
                            <infs:WclButton ID="btnDiscard" CssClass="btnLeftMarg" runat="server" Text="Discard Document" AutoPostBack="true"
                                OnClick="btnDiscard_Click1">
                                <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                                    PrimaryIconWidth="14" />
                            </infs:WclButton>
                            <infs:WclButton ID="btnSwap" runat="server" Text="Swap Items" ButtonType="StandardButton"
                                AutoPostBack="false" OnClientClicked="onSwapClick" CssClass="btnRightMarg" UseSubmitBehavior="false">
                                <Icon PrimaryIconCssClass="rbNext" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                                    PrimaryIconWidth="14" />
                            </infs:WclButton>
                        </ExtraCommandButtons>
                    </infsu:CommandBar>
                </infs:WclPane>
            </infs:WclSplitter>
        </infs:WclPane>
        <infs:WclSplitBar runat="server" CollapseMode="Both" ID="spltBar"></infs:WclSplitBar>
        <infs:WclPane ID="pnRight" runat="server" Width="49%" Scrolling="None">
            <asp:HiddenField runat="server" ID="hdnADEDocVwr" />
            <infs:WclButton runat="server" AutoPostBack="false" ID="btnUndockPdfVwr" OnClientClicked="btnUndockClick" Text="UnDock" UseSubmitBehavior="false"></infs:WclButton>
            <iframe id="iframePdfDocViewer" runat="server" width="100%" height="95%"></iframe>
        </infs:WclPane>
    </infs:WclSplitter>
    <asp:HiddenField ID="hdnFDEQ_IDs" runat="server" />
    <asp:HiddenField ID="hdnSelectedTenantID" runat="server" />
    <asp:HiddenField ID="hdnApplicantUserID" runat="server" />
    <asp:HiddenField ID="hdnApplicantDocumentID" runat="server" />
    <asp:HiddenField ID="hdnPackageSubscriptionID" runat="server" />
    <asp:HiddenField ID="hdnFDEQ_ID" runat="server" />
    <asp:HiddenField ID="hdnDiscardDocumentCount" runat="server" />
    <asp:HiddenField ID="hdnCrntFdeqId" runat="server" />

    <asp:HiddenField ID="hdnIsSameDocument" runat="server" />
    <asp:HiddenField ID="hdnNextTenantId" runat="server" />
    <asp:HiddenField ID="hdnNextApplicantId" runat="server" />
    <asp:HiddenField ID="hdnDocumentId" runat="server" />
    <asp:HiddenField ID="hdnCurrentUserId" runat="server" />
    <asp:HiddenField ID="hdnCrntApplicantId" runat="server" />

    <asp:HiddenField ID="hdnDocChanged" runat="server" />
    <asp:HiddenField ID="hdnDocIDDataEntry" runat="server" />
    <asp:HiddenField ID="hdnDocumentDiscardReasonId" runat="server" />
    <asp:HiddenField ID="hdnOverRideUiRule" runat="server" />
    <asp:HiddenField ID="hdnButtonClicked" runat="server" />
    <asp:HiddenField ID="hdnIsDiscardDocument" runat="server" />
    <div style="display: none">
        <infs:WclButton ID="btnRedirect" UseSubmitBehavior="false" ClientIDMode="Static" runat="server" AutoPostBack="true" OnClick="btnRedirect_Click" />
        <infs:WclButton ID="btnDiscardReasonRedirect" ClientIDMode="Static" runat="server" AutoPostBack="true" OnClick="btnDiscard_Click" />
    </div>
</div>
<%---//TODO for dialog box functionality---%>
<div id="divDiscardStatus" class="discardStatusPopup" runat="server" style="display: none">
    <div class="section">
        <div class="content">
            <div class="sxform auto">
                <div class="sxform auto" id="divSearchPanel">
                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlShowFilters">
                        <div class='sxro sx2co'>
                            <div class='sxlb'>
                                <span class="cptn">Discard Reason</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="ddlDiscardReason" runat="server" AutoPostBack="false"
                                    DataTextField="DDR_Name" DataValueField="DDR_ID" EmptyMessage="--Select--"
                                    Filter="Contains">
                                </infs:WclComboBox>

                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvDiscardReason" ControlToValidate="ddlDiscardReason"
                                        InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="vgDiscardReason"
                                        Text="Discard Reason is required." />
                                </div>

                            </div>

                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx2co'>
                            <div class='sxlb'>
                                <span class="cptn">Additional Notes </span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclTextBox runat="server" ID="txtAdditionalNotes" TextMode="MultiLine" Height="50px">
                                </infs:WclTextBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <%-- <infsu:CommandBar ID="cmdBarMultipleSubscriptions" runat="server" GridMode="false" DefaultPanel="pnlMultipleSubscriptions"
                                DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit,Cancel"
                                SubmitButtonText="Continue" SubmitButtonIconClass="" OnSubmitClick="Continue_Click" OnSubmitClientClick="show_progress_OnSubmit"
                                CancelButtonText="Cancel" OnCancelClientClick="ClosePopup">
                            </infsu:CommandBar>--%>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

</div>

