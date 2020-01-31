<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ReconciliationItemDataLoader" CodeBehind="ReconciliationItemDataLoader.ascx.cs" %>
<%@ Register TagPrefix="infsu" TagName="ItemDataEditMode" Src="ReconciliationItemDataEditMode.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ItemDataReadOnly" Src="ReconciliationItemDataReadOnlyMode.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ItemDataException" Src="ReconciliationItemDataReviewer.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register Src="VerificationDetailsDocumentConrol.ascx" TagName="VerificationDetailsDocumentConrol"
    TagPrefix="uc3" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxCompliancePackageDetails">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/ComplianceVerification.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .radioButtonSpace {
        padding-right: 5px;
    }

    .auto .sxro .radio-list label {
        font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        font-size: 12px !important;
    }

    a.cat_lslnk {
        height: auto !important;
    }

    .auto .sxro .radio-list {
        margin-left: 0px;
    }

    #uploadControlDiv td a, #tdPrint a, #mappedDocument td span {
        font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        font-size: 11px !important;
    }

    hr {
        display: block;
        margin: 0.5em 10px;
        border-style: inset;
        border-width: 1px;
        color: #dedede;
        border-color: #dedede;
    }
</style>

<div class="msgbox">
    <asp:Label ID="lblMessageCatException" runat="server">
    </asp:Label>
</div>
<asp:Panel ID="pnlCategoryException" Visible="false" runat="server">

    <div class="section">
        <div class="content">
            <div class="sxform auto">
                <div class="sxpnl">
                    <div class='sxro sx1co'>
                        <div class='sxlb' title="The current verification status for this Category">
                            <span class="cptn">Current Status</span>
                        </div>
                        <div class='sxlm'>
                            <span class="ronly">
                                <asp:Literal ID="litCurrentCategoryStatus" runat="server"></asp:Literal></span>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx1co'>
                        <div class='sxlb' title="The Category Exception status for this Category">
                            <span class="cptn">Category Exception Status</span>
                        </div>
                        <div class='sxlm'>
                            <span class="ronly">
                                <asp:Literal ID="litCategoryExceptionStatus" runat="server"></asp:Literal></span>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div id="divExpiDateReadOnly" runat="server" style="display: none;">
                        <div class='sxro sx1co'>
                            <div class='sxlb'>
                                <span class="cptn">Expiration Date</span>
                            </div>
                            <div class='sxlm monly'>
                                <infs:WclDatePicker ID="dpExpiDateReadOnly" runat="server" Enabled="false" DateInput-EmptyMessage="Select a date">
                                </infs:WclDatePicker>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div class='sxro monly'>
                        <div class='sxlb'>
                            <span class="cptn">Exception Reason</span>
                        </div>
                        <div class='sxroend'>
                        </div>
                        <span class="ronly">
                            <asp:Literal ID="litExceptionReason" runat="server"></asp:Literal>
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdfCatRejectionCodeException" runat="server" />
</asp:Panel>

<div style="margin-bottom: 40px;">
</div>

<asp:Panel ID="pnlItems" runat="server">
</asp:Panel>

<div class="page_cmd_main">
</div>

<asp:HiddenField ID="hdfSaveResultMessage" Value="Test Message" runat="server" />
<asp:HiddenField ID="hdnExpirationDate" runat="server" />

