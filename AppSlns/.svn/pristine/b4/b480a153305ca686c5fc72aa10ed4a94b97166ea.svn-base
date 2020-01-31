<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.OrderPaymentDetails" CodeBehind="OrderPaymentDetails.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%--<%@ Register TagName="OrderPackageDetails" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/OrderPackageDetails.ascx" %>--%>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<%@ Register Src="~/CommonControls/UserControl/LocationInfo.ascx" TagPrefix="uc"
    TagName="Location" %>
<%@ Register TagPrefix="uc" TagName="PersonAlias" Src="~/Shared/Controls/PersonAliasInfo.ascx" %>
<%@ Register TagPrefix="uc" TagName="AppointmentRescheduler" Src="~/FingerPrintSetUp/UserControl/AppointmentReschedulerOld.ascx" %>

<%@ Register TagPrefix="uc" TagName="ScheduleLocationUpdateControl" Src="~/FingerPrintSetUp/UserControl/AppointmentScheduleLocationUpdate.ascx" %>
<%--<%@ Register TagPrefix="infsu" TagName="OLI" Src="~/ComplianceOperations/UserControl/OrderLineItems.ascx" %>--%>

<style type="text/css">
    .AppOrderDetailsAppomentSchedule {
        width: 100% !important;
    }

    .buttonHidden {
        display: none;
    }

    .linkposition {
        position: relative;
        margin-right: 1%;
    }

    .a:focus, .a:hover {
        color: #23527c;
        text-decoration: underline;
    }

    .linkstyle {
        color: #337ab7;
        text-decoration: none;
    }

    .autoRenewalLink {
        display: inline-block;
        color: Black;
        background-color: #BDBDB4;
        border-style: None;
        text-decoration: none;
        padding: 5px 15px;
    }

    .autoRenewalLinkOffButton {
        display: inline-block;
        color: Black;
        background-color: #BDBDB4;
        border-style: None;
        text-decoration: none;
        padding: 5px 15px;
    }

    a.autoRenewalLink:hover {
        background-color: #D5E5FF;
    }

    .IsGraduatedToggle .rbText {
        display: inline-block;
        color: blue;
        text-decoration: underline;
        cursor: pointer;
        padding-left: 5px;
    }

    .padRight2 {
        padding-right: 3px;
    }

    .cancelBtnWidth .rbPrimary {
        width: 110px !important;
        padding-left: 20px !important;
    }

    .dvCancelOrder .input .submit {
        padding-left: 15px !important;
    }
</style>

<infs:WclResourceManagerProxy ID="rmpOrderReview" runat="server">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/OrderReview.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<div class="container-fluid">
    <div id="Div2" class="linkposition">
        <div class="row">
            <div class="col-md-12">
                <div id="modcmd_bar">
                    <div id="Div3">
                        <asp:LinkButton Text="Back to Queue" runat="server" ID="lnkbacksrch" OnClick="lnkbacksrch_Click" CssClass="linkstyle a" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="dvSection" style="padding-bottom: 15px">
    <div id="modcmd_bar1" style="float: right; padding-right: 20px">
        <div id="vermod_cmds">
            <asp:LinkButton ID="lnkGoBack" runat="server" OnClick="lnkGoBack_Click" Text="<%$ Resources:Language, BCKSPRTPRTLDTL %>" Visible="false"></asp:LinkButton>
        </div>
    </div>
</div>

<div class="section">

    <h1 class="mhdr" tabindex="0">
        <asp:Label ID="lblOrderQueue" runat="server" Text="<%$ Resources:Language, ORDRDTLS %>"></asp:Label></h1>

    <div class="content">
        <div class="sxform auto">
            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlOrderDetails">
                <h3>
                    <asp:Label Text="" runat="server" ID="lblExceptionHdr" CssClass="phdr" />
                </h3>
                <div class='sxro sx3co'>
                    <%--<div class='sxlb' title="<%$ Resources:Language, DSCRPTNPRGSCHL %>">--%>
                    <div class='sxlb' title="A description of your program within your school">
                        <%--<span class='cptn'>Institution Hierarchy</span>--%>
                        <label for="<%= txtHierarchy.ClientID %>" class="cptn"><%=Resources.Language.INSTHIERARCHY%></label>
                    </div>
                    <div class='sxlm m3spn'>
                        <infs:WclTextBox runat="server" ID="txtHierarchy" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <%--<span class='cptn'>Order Number</span>--%>
                        <label for="<%= txtOrderId.ClientID %>" class="cptn"><%=Resources.Language.ODRNUMBER%></label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtOrderId" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <%--  <div class='sxlb' title="The current status of this order">
                        <span class='cptn'>Order Status</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtOrderStatus" Enabled="false">
                        </infs:WclTextBox>
                    </div>--%>

                    <div class='sxlb' title="The date this order was placed">
                        <%--<span class='cptn'>Order Date</span>--%>
                        <label for="<%= txtOrderDate.ClientID %>" class="cptn"><%=Resources.Language.ODRDATE%></label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtOrderDate" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>

                    <div class='sxroend'>
                    </div>
                </div>

                <div class='sxro sx3co' id="dvPrice" runat="server">
                    <div id="divTotalPrice" runat="server">
                        <%--<div class='sxlb' title="<%$ Resources:Language, TTLPRICEFRORDR %>">--%>
                        <div class='sxlb' title="The total price for this order">
                            <%--<span class='cptn'>Total Price</span>--%>
                            <label for="<%= txtGrandTotal.ClientID %>" class="cptn"><%=Resources.Language.TOTALPRICE%></label>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox runat="server" ID="txtGrandTotal" ReadOnly="true" NumberFormat-DecimalDigits="2" EnableAriaSupport="true"
                                Type="Currency">
                            </infs:WclNumericTextBox>
                        </div>
                    </div>
                    <div id="dvDuePayment" runat="server" visible="false">
                        <div class='sxlb'>
                            <%--<span class='cptn'>Due Payment</span>--%>
                            <label for="<%= txtDuePayment.ClientID %>" class="cptn"><%=Resources.Language.DUEPAYMENT%></label>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox runat="server" ID="txtDuePayment" ReadOnly="true" NumberFormat-DecimalDigits="2" EnableAriaSupport="true"
                                Type="Currency">
                            </infs:WclNumericTextBox>
                        </div>
                    </div>
                    <%--    <div class='sxlb' title="The total price for this order">
                        <span class='cptn'>Total Price</span>
                    </div>
                    <div class='sxlm'>
                          <infs:WclNumericTextBox runat="server" ID="txtGrandTotal" Enabled="false" NumberFormat-DecimalDigits="2"
                            Type="Currency">
                        </infs:WclNumericTextBox>
                    </div>--%>
                    <div class='sxroend'>
                    </div>
                </div>


                <div id="dvCancellation" runat="server">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <%--<span class='cptn'>Rejection Reason</span>--%>
                            <label for="<%= txtRejectionReason.ClientID %>" class="cptn"><%=Resources.Language.RJCTNRSN%></label>

                        </div>
                        <div class='sxlm' style="width: 52%">
                            <infs:WclTextBox runat="server" ID="txtRejectionReason" MaxLength="1024" TextMode="MultiLine" EnableAriaSupport="true">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvRejectionReason" Display="Dynamic"
                                    ControlToValidate="txtRejectionReason" class="errmsg" ErrorMessage="<%$ Resources:Language, REJCTNRSNREQ %>"
                                    ValidationGroup="grpCancelReject" />
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div style="width: 100%; text-align: center">
                        <infs:WclButton runat="server" ID="btnRejectCancellation" Icon-PrimaryIconCssClass="rbCancel"
                            Text="<%$ Resources:Language, RJCTCNCLN %>" ValidationGroup="grpCancelReject" CausesValidation="true"
                            OnClick="btnRejectCancellation_Click">
                        </infs:WclButton>
                        <infs:WclButton runat="server" ID="btnApproveCancellation" Icon-PrimaryIconCssClass="rbOk"
                            OnClick="btnApproveCancellation_OnClick" Text="<%$ Resources:Language, APRVCNCLN %>">
                        </infs:WclButton>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>

<div class="section" id="divRefund" runat="server">
    <h1 class="mhdr"><%=Resources.Language.ORDRRFND%>
    </h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlRefund">
                <asp:HiddenField ID="hdnIsPartialOrderCancellation" Value="0" runat="server" />
                <asp:HiddenField ID="hdnPartialOrderCancellationAmount" Value="0" runat="server" />
                <asp:HiddenField ID="hdfTotalRefundCount" Value="0" runat="server" />
                <asp:HiddenField ID="hdfVisibleRefundCount" Value="0" runat="server" />
                <asp:Repeater ID="rptRefundOrder" runat="server" OnItemDataBound="rptRefundOrder_ItemDataBound">
                    <ItemTemplate>
                        <div class="section" id="divEachOrderRefund" runat="server">
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span id="spnOriginalPrice" class='cptn'><%=Resources.Language.ORGNLPRICE%></span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclNumericTextBox runat="server" ID="txtOriginalPrice" EnableAriaSupport="true" aria-describedBy="spnOriginalPrice" ReadOnly="true" NumberFormat-DecimalDigits="2"
                                        Type="Currency">
                                    </infs:WclNumericTextBox>
                                </div>
                                <div class='sxlb'>
                                    <span id="spnRefund" class='cptn'><%=Resources.Language.REFUND%></span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclNumericTextBox runat="server" ID="txtTotalRefund" EnableAriaSupport="true" aria-describedBy="spnRefund" ReadOnly="true" NumberFormat-DecimalDigits="2"
                                        Type="Currency">
                                    </infs:WclNumericTextBox>
                                </div>
                                <div class='sxlb'>
                                    <span id="spnNetPrice" class='cptn'><%=Resources.Language.NETPRICE%></span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclNumericTextBox runat="server" ID="txtNetPrice" EnableAriaSupport="true" aria-describedBy="spnNetPrice" ReadOnly="true" NumberFormat-DecimalDigits="2"
                                        Type="Currency">
                                    </infs:WclNumericTextBox>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span id="spnRefundAmount" class='cptn'><%=Resources.Language.REFUNDAMT%></span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclNumericTextBox runat="server" EnableAriaSupport="true" aria-describedBy="spnRefundAmount" ID="txtRefundAmount" NumberFormat-DecimalDigits="2">
                                    </infs:WclNumericTextBox>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvRefundAmount" Display="Dynamic"
                                            ControlToValidate="txtRefundAmount" class="errmsg" ErrorMessage="<%$ Resources:Language, REFUNDAMTREQ %>"
                                            ValidationGroup="refundGrp" />

                                        <asp:CompareValidator ID="cmpRefund" runat="server" ControlToValidate="txtRefundAmount"
                                            ErrorMessage="<%$ Resources:Language, RFNDAMTGRTRZERO %>" Display="Dynamic"
                                            ValidationGroup="refundGrp" class="errmsg" Operator="GreaterThan" Type="Double" ValueToCompare="0"></asp:CompareValidator>
                                    </div>
                                </div>
                                <div>
                                    <infs:WclButton runat="server" ID="btnRefund" Icon-PrimaryIconCssClass="rbOk"
                                        Text="<%$ Resources:Language, REFUNDAMT %>" ValidationGroup="refundGrp" CausesValidation="true" OnClick="btnRefund_Click">
                                    </infs:WclButton>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <%--  <div class='sxro sx3co'>
                    <div>
                    </div>
                    <div class='sxlm m3spn'>
                        <div style="width: 103%; text-align: center">
                          
                        </div>
                    </div>
                </div>--%>
                            <asp:HiddenField ID="hdfOrderPaymentdetailID" runat="server" />
                            <asp:HiddenField ID="hdfOrderStatusTypeCode" runat="server" />
                            <asp:HiddenField ID="hdfTransactionId" runat="server" />
                            <asp:HiddenField ID="hdfCCNumber" runat="server" />
                            <asp:HiddenField ID="hdfInvoiceNumber" runat="server" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
        </div>
    </div>
</div>

<div class="section" id="dvPackageDetails" runat="server">
    <h1 class="mhdr" tabindex="0" runat="server" id="hrPackageDetailsLocationTenant"><%=Resources.Language.ORDRDTLS%>
    </h1>
    <h1 class="mhdr" tabindex="0" runat="server" id="hrPackageDetails"><%=Resources.Language.PKGDETAILS%>
    </h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlBeforeSubmission">
                <div id="divCompliancePackage" visible="false" runat="server">
                    <div class='sxro sx3co' runat="server">
                        <div class='sxlb'>
                            <%--<span id="spnPackageHeading" runat="server" class='cptn'></span>--%>
                            <label id="spnPackageHeading" runat="server" for="txtPackage" class="cptn"></label>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ClientIDMode="Static" ID="txtPackage" ReadOnly="true" EnableAriaSupport="true">
                            </infs:WclTextBox>
                        </div>
                        <div id="divSubscriptionFee" runat="server">
                            <%--<div class='sxlb' title="<%$ Resources:Language, COSTFORSBCRPTN %>">--%>
                            <div class='sxlb' title="The cost for this subscription">
                                <%--<span class='cptn'>Subscription Fee</span>--%>
                                <label for="<%= txtTotalOrderValue.ClientID %>" class="cptn"><%=Resources.Language.SBSCRPTNFEE%></label>

                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox runat="server" ID="txtTotalOrderValue" ReadOnly="true" NumberFormat-DecimalDigits="2" EnableAriaSupport="true"
                                    Type="Currency">
                                </infs:WclNumericTextBox>
                            </div>
                        </div>
                        <div id="dvAutoRenewal" runat="server" class='sxlb'>
                            <asp:Label ID="lblAutoRenewal" runat="server" Text="Auto Renewal:"></asp:Label>
                        </div>
                        <div class='sxlm'>
                            <label id="lblAutoRenewalTmp" class="sr-only"><%= String.Concat("Auto Renewal : " ,btnAutoRenewal.Text) %> </label>
                            <asp:LinkButton ID="btnAutoRenewal" aria-describedby="lblAutoRenewalTmp" runat="server" Visible="false"
                                OnClientClick="return ResetAutoRenewalStatus(this);">
                            </asp:LinkButton>
                        </div>

                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb' title="The duration of the purchased subscription">
                            <%--<span class='cptn'>Subscription Period (Months)</span>--%>
                            <label for="<%= txtDurationMnths.ClientID %>" class="cptn"><%=Resources.Language.SBSCRPTNPRDMNTH%></label>

                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtDurationMnths" ReadOnly="true" EnableAriaSupport="true">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxlb' title="The start date of the subscription purchased with this order">
                            <%--<span class='cptn'>Subscription Start Date</span>--%>
                            <label for="<%= txtSubsStartDt.ClientID %>" class="cptn"><%=Resources.Language.SBSCRPTNSTRTDT%></label>

                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtSubsStartDt" ReadOnly="true" EnableAriaSupport="true">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxlb' title="The date the subscription purchased with this order will expire">
                            <%--<span class='cptn'>Subscription Expiration Date</span>--%>
                            <label for="<%= txtSubsExpirationDt.ClientID %>" class="cptn"><%=Resources.Language.SBSCRPTNEXPDT%></label>

                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtSubsExpirationDt" ReadOnly="true" EnableAriaSupport="true">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co' id="dvRushOrderFields" runat="server" visible="false">
                        <div id="dvRushOrder" runat="server" visible="false">
                            <div class='sxlb'>
                                <%--<span class='cptn'>Rush Order Price</span>--%>
                                <label for="<%= txtRushOrderPrice.ClientID %>" class="cptn"><%=Resources.Language.RSHORDRPRC%></label>

                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox runat="server" ID="txtRushOrderPrice" Enabled="false" NumberFormat-DecimalDigits="2" EnableAriaSupport="true"
                                    Type="Currency">
                                </infs:WclNumericTextBox>
                            </div>
                        </div>
                        <div id="dvRushOrderStatus" runat="server" visible="false">
                            <div class='sxlb'>
                                <%--<span class='cptn'>Rush Order Status</span>--%>
                                <label for="<%= txtRushOrderStatus.ClientID %>" class="cptn"><%=Resources.Language.RSHORDSTATUS%></label>

                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtRushOrderStatus" ReadOnly="true" EnableAriaSupport="true">
                                </infs:WclTextBox>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <%--<span class='cptn'>Payment Type</span>--%>
                            <label for="<%= txtCompPkgPaymentType.ClientID %>" class="cptn"><%=Resources.Language.PAYMENTTYPE%></label>

                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtCompPkgPaymentType" ReadOnly="true" EnableAriaSupport="true">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co' runat="server" id="dvCancelCompliancePkg">
                        <div class='sxlb'>
                            <%--<span class='cptn'>Cancel Compliance Package</span>--%>
                            <label tabindex="0" for="<%=chkPartialCancelCompliancePkg.ClientID %>" class="cptn"><%=Resources.Language.CNCLCMPLNPKG%></label>

                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclButton runat="server" ID="chkPartialCancelCompliancePkg" ButtonType="ToggleButton" AutoPostBack="false"
                                ToggleType="CheckBox">
                            </infs:WclButton>
                            <infs:WclTextBox runat="server" ID="txtPartialCancelCompliancePkgStatus" Visible="false" ReadOnly="true" EnableAriaSupport="true">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co' runat="server" id="dvSaveIsGraduatedCompliance" visible="false">
                        <div class='sxlb'>
                            <%--<span class='cptn'>Graduated Status</span>--%>
                            <label for="<%= rdbIsGraduatedCompliance.ClientID %>" class="cptn"><%=Resources.Language.GRDTDSTTS%></label>

                        </div>
                        <div class='sxlm m2spn'>
                            <%--     <infs:WclButton ID="tgIsGraduatedCompliance" CssClass="IsGraduatedToggle" OnClientClicked="ConfirmationMessageComp" ButtonType="ToggleButton" 
                                ToggleType="CustomToggle" runat="server" OnClick="tgIsGraduatedCompliance_Click" AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="I have Graduated" Value="True" />
                                    <telerik:RadButtonToggleState Text="Undo Graduated" Value="False"/>
                                </ToggleStates>
                            </infs:WclButton>--%>
                            <asp:RadioButtonList ID="rdbIsGraduatedCompliance" runat="server" CssClass="radio_list" RepeatDirection="Horizontal" AutoPostBack="false"
                                OnSelectedIndexChanged="tgIsGraduatedCompliance_Click" onchange="ConfirmationMessageComp(this)">
                                <asp:ListItem Text="<%$ Resources:Language, IHVGRAD %>" Value="True"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Language, IHVNOTYTGRAD %>" Value="False"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div id="divCancellationDetails" class='sxro sx3co' runat="server">
                        <div id="divCompPkgCancelledBy" runat="server">
                            <div class='sxlb'>
                                <span id="spnCompPackageCancelledBy" runat="server" class='cptn'>Package Cancelled By</span>
                                <%--<label id="spnPackageCancelledBy" runat="server" for="txtCompPackageCancelledBy" class="cptn"></label>--%>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ClientIDMode="Static" ID="txtCompPackageCancelledBy" ReadOnly="true" EnableAriaSupport="true">
                                </infs:WclTextBox>
                            </div>
                        </div>
                        <div id="divCompPkgCancelledOn" runat="server">
                            <div class='sxlb'>
                                <span id="spnCompPackageCancelledOn" runat="server" class='cptn'>Package Cancelled On</span>
                                <%--<label for="<%= txtPackageCancelledOn.ClientID %>" class="cptn"><%=Resources.Language.SBSCRPTNFEE%></label>--%>
                            </div>
                            <div class='sxlm'>
                                <%--<infs:WclNumericTextBox runat="server" ID="txtCompPackageCancelledOn" ReadOnly="true" EnableAriaSupport="true">
                                </infs:WclNumericTextBox>--%>
                                <infs:WclTextBox runat="server" ID="txtCompPackageCancelledOn" ReadOnly="true" EnableAriaSupport="true">
                                </infs:WclTextBox>
                            </div>
                        </div>
                        <%--<div class='sxlb'>
                            &nbsp;
                        </div>
                        <div class='sxlm'>
                            &nbsp;
                        </div>--%>

                        <div class='sxroend'>
                        </div>
                    </div>
                </div>
                <div id="divBackgroundPackage" runat="server" visible="false">
                    <asp:Repeater ID="rptBackgroundPackages" runat="server" OnItemDataBound="rptBackgroundPackages_ItemDataBound">
                        <ItemTemplate>
                            <hr id="hrline" runat="server" style="border-bottom: solid 1px #c0c0c0;" />
                            <div class='sxro sx3co'>
                                <div class='sxlb' id="dvBkgPackage" runat="server">
                                    <%--<span class='cptn'>Background Package</span>--%>
                                    <label class="cptn"><%=Resources.Language.BKGPKG%></label>
                                    <%-- for="<%= txtBkgPackage.ClientID %>"--%>
                                </div>
                                <div class='sxlb' id="dvBkgLocationTenant" runat="server">
                                    <%--<span class='cptn'>Background Package</span>--%>
                                    <label class="cptn"><%=Resources.Language.ORDERSELECTION%></label>
                                    <%-- for="<%= txtBkgPackage.ClientID %>"--%>
                                </div>
                                <div class='sxlm m2spn'>
                                    <infs:WclTextBox runat="server" ID="txtBkgPackage" Text='<%# String.IsNullOrEmpty( Convert.ToString( Eval("BPA_Label"))) 
                                            ? Convert.ToString( Eval("BPA_Name"))
                                            : Convert.ToString( Eval("BPA_Label"))%>'
                                        ReadOnly="true" EnableAriaSupport="true">
                                    </infs:WclTextBox>
                                </div>
                                <div runat="server" id="divBkgPackagePriceLabel">
                                    <div class='sxlb'>
                                        <%-- <span class='cptn'>Package Price</span>--%>
                                        <label for="<%= txtRushOrderPrice.ClientID %>" class="cptn"></label>

                                    </div>
                                </div>
                                <div class='sxlm' id="divBkgPackagePrice" runat="server" visible="true">
                                    <infs:WclNumericTextBox runat="server" ID="txtRushOrderPrice" ReadOnly="true" NumberFormat-DecimalDigits="2" Text='<%# Eval("TotalPrice")%>'
                                        Type="Currency" EnableAriaSupport="true">
                                    </infs:WclNumericTextBox>
                                </div>
                                <div id="dvBkgPkgPaymentType" runat="server">
                                    <div class='sxlb'>
                                        <%--<span class='cptn'>Payment Type</span>--%>
                                        <label id="lblBkgPkgPaymentType" class="cptn"><%=Resources.Language.PAYMENTTYPE%></label>

                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtBkgPkgPaymentType" ReadOnly="true" EnableAriaSupport="true">
                                        </infs:WclTextBox>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co' runat="server" id="dvCancelBackgroundPkg">
                                <div class='sxlb'>
                                    <%--<span class='cptn'>Cancel Background Package</span>--%>
                                    <label class="cptn"><%=Resources.Language.CNCLBKGPKG%></label>

                                </div>
                                <div class='sxlm m2spn'>
                                    <%--<asp:CheckBox runat="server" ID="chkPartialCancelBkgPkg" />--%>
                                    <asp:HiddenField runat="server" ID="hdnBkgOrderPackageID" Value='<%# Eval("BkgOrderPackageID") %>' />
                                    <infs:WclButton runat="server" ID="chkPartialCancelBkgPkg" ButtonType="ToggleButton" AutoPostBack="false"
                                        ToggleType="CheckBox">
                                    </infs:WclButton>
                                    <infs:WclTextBox runat="server" ID="txtPartialCancelBkgPkgStatus" Visible="false" ReadOnly="true" EnableAriaSupport="true">
                                    </infs:WclTextBox>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div id="divBkgCancellationDetails" class='sxro sx3co' runat="server">
                                <div id="divBkgPkgCancelledBy" runat="server">
                                    <div class='sxlb'>
                                        <span id="spnBkgPackageCancelledBy" runat="server" class='cptn'>Package Cancelled By</span>
                                        <%--<label id="spnPackageCancelledBy" runat="server" for="txtPackageCancelledBy" class="cptn"></label>--%>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtBkgPackageCancelledBy" ReadOnly="true" EnableAriaSupport="true"
                                            Text='<%# String.IsNullOrEmpty( Convert.ToString( Eval("CancelledBy"))) 
                                            ? string.Empty
                                            : Convert.ToString( Eval("CancelledBy")) %>'>
                                        </infs:WclTextBox>
                                    </div>
                                </div>
                                <div id="divBkgPkgCancelledOn" runat="server">
                                    <div class='sxlb'>
                                        <span id="spnBkgPackageCancelledOn" runat="server" class='cptn'>Package Cancelled On</span>
                                        <%--<label for="<%= txtPackageCancelledOn.ClientID %>" class="cptn"><%=Resources.Language.SBSCRPTNFEE%></label>--%>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtBkgPackageCancelledOn" ReadOnly="true" EnableAriaSupport="true"
                                            Text='<%# String.IsNullOrEmpty( Convert.ToString( Eval("CancelledOn"))) 
                                            ? string.Empty
                                            : Convert.ToDateTime( Eval("CancelledOn")).ToShortDateString() %>'>
                                        </infs:WclTextBox>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class='sxro sx3co' runat="server" id="dvSaveIsGraduatedBackground" visible="false">
                        <div class='sxlb'>
                            <%--<span class='cptn'>Graduated Status</span>--%>
                            <label for="<%= rdbIsGraduatedBackground.ClientID %>" class="cptn"><%=Resources.Language.GRDTDSTTS%></label>

                        </div>
                        <div class='sxlm m2spn'>
                            <%--    <infs:WclButton ID="tgIsGraduatedBackground" CssClass="IsGraduatedToggle" OnClientClicked="ConfirmationMessageBkg" AutoPostBack="false"
                                 ButtonType="ToggleButton" ToggleType="CustomToggle" runat="server" OnClick="tgIsGraduatedBackground_Click">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="I have Graduated" Value="True" />
                                    <telerik:RadButtonToggleState Text="Undo Graduated" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>--%>
                            <asp:RadioButtonList ID="rdbIsGraduatedBackground" runat="server" CssClass="radio_list" RepeatDirection="Horizontal" AutoPostBack="false"
                                OnSelectedIndexChanged="tgIsGraduatedBackground_Click" onchange="ConfirmationMessageBkg(this)">
                                <asp:ListItem Text="<%$ Resources:Language, IHVGRAD %>" Value="True"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Language, IHVNOTYTGRAD %>" Value="False" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </div>
                <infsu:CommandBar ID="btnPartialOrderCancellation" runat="server" ButtonPosition="Center" DisplayButtons="Save"
                    AutoPostbackButtons="Save" SaveButtonText="<%$ Resources:Language, CNCLSLTDPKG %>" OnSaveClick="btnPartialOrderCancellation_Click">
                </infsu:CommandBar>

                <div id="dvCBIUniqueID" runat="server" class="sxro sx3co" title="CBIUniqueID" visible="false">
                    <div class='sxlb'>
                        <label for="<%= txtCBIUniqueID.ClientID %>" class="cptn"><%=Resources.Language.CBIUNIQUEID%></label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ClientIDMode="Static" ID="txtCBIUniqueID" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                </div>

                <br />
                <div id="dvServiceTypeDetails" runat="server" width="746px" >
                    <infs:WclGrid runat="server" ID="grdServiceDetails" CellSpacing="0"
                        ShowAllExportButtons="false" ShowClearFiltersButton="false" AllowPaging="false"
                        GridLines="None" OnNeedDataSource="grdServiceDetails_NeedDataSource">
                        <MasterTableView DataKeyNames="OrderId,ServiceName,ServiceStatus,OrderNumber"
                            AutoGenerateColumns="False" AllowSorting="true" AllowPaging="false"
                            AllowFilteringByColumn="false">

                            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                ShowRefreshButton="false" />
                            <Columns>
                                <%--HeaderText="Order Number"--%>
                                <telerik:GridBoundColumn DataField="ServiceName" FilterControlAltText="Filter OrderId column"
                                    HeaderText="<% $Resources:Language, SERVICENAME%>" HeaderStyle-Width="27%"
                                    SortExpression="OrderNumber" UniqueName="OrderId">
                                </telerik:GridBoundColumn>

                                <%--HeaderText="Order Number"--%>
                                <telerik:GridBoundColumn DataField="ServiceStatus" FilterControlAltText="Filter OrderId column"
                                    HeaderText="<% $Resources:Language, SERVICESTATUS%>" HeaderStyle-Width="9%"
                                    SortExpression="OrderNumber" UniqueName="OrderId">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="TrackingNumber" FilterControlAltText="Filter OrderId column"
                                    HeaderText="<% $Resources:Language, TRACKINGNUMBER%>" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="Price" FilterControlAltText="Filter OrderId column" DataFormatString="{0:c}"
                                    HeaderText="<% $Resources:Language, BASEPRICENEW%>" HeaderStyle-Width="9%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                </telerik:GridBoundColumn>

                                 <telerik:GridBoundColumn DataField="Quantity" FilterControlAltText="Filter OrderId column"
                                    HeaderText="<% $Resources:Language, FINGPRNTCRDCOPIES%>"  HeaderStyle-Width="9%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="FCAdditionalPrice" FilterControlAltText="Filter FCAdditionalPrice column"
                                    HeaderText="<% $Resources:Language, PRICEPERADDCOPY%>"  HeaderStyle-Width="9%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="PPQuantity" FilterControlAltText="Filter PPQuantity column" Visible="false"
                                    HeaderText="<% $Resources:Language, PASSPORTPHOTOSETCOPIES%>"  HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="PPAdditionalPrice" FilterControlAltText="Filter PPAdditionalPrice column" Visible="false"
                                    HeaderText="<% $Resources:Language, PRICEPERADDSETNEW%>"  HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                </telerik:GridBoundColumn>
                                 
                                 <telerik:GridBoundColumn DataField="Amount" FilterControlAltText="Filter OrderId column"
                                    HeaderText="<% $Resources:Language, NETPRICENEW%>" HeaderStyle-Width="9%" Aggregate="Sum" FooterText="Total Net Price:$" DataFormatString="{0:c}" 
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                </telerik:GridBoundColumn>

                                <%--HeaderText="Order Number"--%>
                            </Columns>
                        </MasterTableView>
                        <PagerStyle Mode="NextPrevAndNumeric" Visible="false" AlwaysVisible="false" Position="Bottom" />
                    </infs:WclGrid>
                </div>
                <br />




                <%-- </div>--%>
                <%--<infsu:CommandBar ID="btnSaveIsGraduated" runat="server" Visible="true  " ButtonPosition="Center" AutoPostbackButtons="Save"
                    OnSaveClick="btnSaveIsGraduated_SaveClick" DisplayButtons="Save">--%>
                <%--</infsu:CommandBar>--%>
                <%--<infs:WclButton runat="server" ID="btnPartialOrderCancellation" Icon-PrimaryIconCssClass="rbOk"
                    Text="Cancel Selected Package" ValidationGroup="valGrpCancelPkg" CausesValidation="true"
                    OnClick="btnPartialOrderCancellation_Click">
                </infs:WclButton>--%>
            </asp:Panel>
        </div>
    </div>
</div>

<div class="section" id="dvServiceDetails" runat="server" visible="false">
    <%--<h1 class="mhdr" tabindex="0">Service Details--%>
    <h1 class="mhdr" tabindex="0"><%=Resources.Language.BKGORDRDTLS%>
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class='sxro sx4co' style="padding-top: 10px; padding-bottom: 10px; padding-left: 10px">
                <%--<a href="javascript:void(0);" id="lnkServiceDetails" onclick="ServiceDetailsPopUp();" style="color: blue; font-size: small;">Click here to view service details</a>--%>
                <a href="javascript:void(0);" id="lnkServiceDetails" onclick="ServiceDetailsPopUp();" style="color: blue; font-size: small;"><%=Resources.Language.VWORDRDTLS%></a>
            </div>
        </div>
    </div>
</div>

<div class="section">
    <h1 class="mhdr" tabindex="0" runat="server" id="hrAPILocationTenant"><%=Resources.Language.PROFILEDETAILS%>
    </h1>
    <h1 class="mhdr" tabindex="0" runat="server" id="hrAPI"><%=Resources.Language.APPLCNTPRSNLINFO%>
    </h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlApplicantDetails">
                <h3>
                    <asp:Label Text="" runat="server" ID="Label1" CssClass="phdr" />
                </h3>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <%--<span class='cptn'>First Name</span>--%>
                        <label for="<%= txtFirstName.ClientID %>" class="cptn"><%=Resources.Language.FIRSTNAME%></label>

                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtFirstName" ReadOnly="true" runat="server" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <%--<div class='sxlb' title="<%$ Resources:Language, DNTHVMNAMEBLNK %>">--%>
                    <div class='sxlb' title="<%=Resources.Language.DNTHVMNAMEBLNK%>">
                        <%--<span class='cptn'>Middle Name</span>--%>
                        <label for="<%= txtMiddleName.ClientID %>" class="cptn"><%=Resources.Language.MIDDLENAME%></label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtMiddleName" runat="server" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        <%--<span class='cptn'>Last Name</span>--%>
                        <label for="<%= txtLastName.ClientID %>" class="cptn"><%=Resources.Language.LASTNAME%></label>

                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtLastName" runat="server" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co' id="dvPersonalAlias" runat="server" visible="false">
                    <uc:PersonAlias ID="ucPersonAlias" runat="server" Visible="true" IsReadOnly="true" IsLabelMode="false"></uc:PersonAlias>
                    <%-- <div class='sxlb' title="Another name that you have used">
                        <span class='cptn'>Alias 1</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtAlias1" runat="server" Enabled="false">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb' title="Another name that you have used">
                        <span class='cptn'>Alias 2</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtAlias2" runat="server" Enabled="false">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb' title="Another name that you have used">
                        <span class='cptn'>Alias 3</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtAlias3" runat="server" Enabled="false">
                        </infs:WclTextBox>
                    </div>--%>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <%--<span class='cptn'>Gender</span>--%>
                        <label for="<%= txtGender.ClientID %>" class="cptn"><%=Resources.Language.GENDER%></label>

                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtGender" runat="server" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div id="divDOB" runat="server">
                        <div class='sxlb'>
                            <%--<span class='cptn'>Date of birth</span>--%>
                            <label for="<%= txtDateOfBirth.ClientID %>" class="cptn"><%=Resources.Language.DOB%></label>

                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtDateOfBirth" runat="server" ReadOnly="true" EnableAriaSupport="true">
                            </infs:WclTextBox>
                        </div>
                    </div>
                    <div id="divSSN" runat="server">
                        <%--<div class='sxlb' title="<%$ Resources:Language, DNTHVSSNDSPLY %>">--%>
                        <div class='sxlb' title="If you do not have a SSN, this should display 111-11-1111">
                            <%--<span class='cptn'>Social Security Number/ID number</span>--%>
                            <label for="<%= txtSSN.ClientID %>" class="cptn"><%=Resources.Language.SSNID%></label>

                        </div>
                        <div class='sxlm'>
                            <infs:WclMaskedTextBox ID="txtSSN" runat="server" MaxLength="10" Mask="###-##-####" EnableAriaSupport="true"
                                ReadOnly="true">
                            </infs:WclMaskedTextBox>
                        </div>
                    </div>
                    <div id="divSSNMasked" visible="false" runat="server">
                        <%--<div class='sxlb' title="<%$ Resources:Language, DNTHVSSNDSPLY %>">--%>
                        <div class='sxlb' title="If you do not have a SSN, this should display 111-11-1111">
                            <%--<span class='cptn'>Social Security Number/ID number</span>--%>
                            <label for="<%= txtSSNMAsked.ClientID %>" class="cptn"><%=Resources.Language.SSNID%></label>

                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtSSNMAsked" runat="server" ReadOnly="true" EnableAriaSupport="true">
                            </infs:WclTextBox>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <%--<span class='cptn'>Email</span>--%>
                        <label for="<%= txtEmail.ClientID %>" class="cptn"><%=Resources.Language.EMAIL%></label>

                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtEmail" runat="server" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb' title="This is your secondary email address">
                        <%--<span class='cptn'>Secondary Email</span>--%>
                        <label for="<%= txtSecondaryEmail.ClientID %>" class="cptn"><%=Resources.Language.SECEMAIL%></label>

                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtSecondaryEmail" runat="server" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <%--<span class='cptn'>Phone</span>--%>
                        <label for="<%= txtPhone.ClientID %>" class="cptn"><%=Resources.Language.PHONE%></label>

                    </div>
                    <div class='sxlm'>
                        <infs:WclMaskedTextBox ID="txtPhone" runat="server" ReadOnly="true" Mask="(###)-###-####" EnableAriaSupport="true">
                        </infs:WclMaskedTextBox>
                        <infs:WclTextBox ID="txtPhoneUnMasking" runat="server" EnableAriaSupport="true" ReadOnly="true"></infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        <%--<span class='cptn'>Secondary Phone</span>--%>
                        <label for="<%= txtSecondaryPhone.ClientID %>" class="cptn"><%=Resources.Language.SECPHONE%></label>

                    </div>
                    <div class='sxlm'>
                        <infs:WclMaskedTextBox ID="txtSecondaryPhone" runat="server" ReadOnly="true" Mask="(###)-###-####" EnableAriaSupport="true">
                        </infs:WclMaskedTextBox>
                        <infs:WclTextBox ID="txtSecondaryPhoneUnMasking" runat="server" ReadOnly="true" EnableAriaSupport="true"></infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <%--<span class='cptn'>Address 1</span>--%>
                        <asp:Label for="<%= txtAddress1.ClientID %>" runat="server" ID="lblAddress1" class="cptn"></asp:Label>

                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtAddress1" runat="server" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div runat="server" id="dvAddress2">
                        <div class='sxlb'>
                            <%--<span class='cptn'>Address 2</span>--%>
                            <label for="<%= txtAddress2.ClientID %>" class="cptn"><%=Resources.Language.ADDRESS2%></label>

                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtAddress2" runat="server" ReadOnly="true" EnableAriaSupport="true">
                            </infs:WclTextBox>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <%--<span class='cptn'>Zip</span>--%>
                        <label for="<%= txtZip.ClientID %>" class="cptn"><%=Resources.Language.ZIP%></label>

                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtZip" runat="server" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        <%--<span class='cptn'>City</span>--%>
                        <label for="<%= txtCity.ClientID %>" class="cptn"><%=Resources.Language.CITY%></label>

                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtCity" runat="server" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div id="dvState" runat="server">
                        <div class='sxlb'>
                            <%--<span class='cptn'>State</span>--%>
                            <label for="<%= txtState.ClientID %>" class="cptn"><%=Resources.Language.STATE%></label>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtState" runat="server" ReadOnly="true" EnableAriaSupport="true">
                            </infs:WclTextBox>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>


<div class="section" id="dvMailingAddressDetail" runat="server" visible="false">
    <h1 class="mhdr" tabindex="0" runat="server" id="h1"><%=Resources.Language.MAILINGADDRESS%>
    </h1>

    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="Panel1">
                <div runat="server" id="Div5">

                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn'><%=Resources.Language.MAILINGOPTION%></span>
                        </div>
                        <div class='sxlm'>
                            <asp:Label ID="lblMailingOption" CssClass="ronly" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn'><%=Resources.Language.ADDRESS%></span>
                        </div>
                        <div class='sxlm'>
                            <asp:Label ID="lblMailingAddress" CssClass="ronly" runat="server"></asp:Label>
                        </div>

                        <div class='sxlb'>
                            <span class='cptn'><%=Resources.Language.CITY%></span>
                        </div>
                        <div class='sxlm'>
                            <asp:Label ID="lblMailingCity" CssClass="ronly" runat="server"></asp:Label>
                        </div>
                        <div class='sxlb'>
                            <span class='cptn'><%=Resources.Language.COUNTRY%></span>
                        </div>
                        <div class='sxlm'>
                            <asp:Label ID="lblMailingCountry" CssClass="ronly" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class='sxro sx3co'>
                        <div id="dvMailingState" runat="server" visible="false">
                            <div class='sxlb'>
                                <span class='cptn'><%=Resources.Language.STATE%></span>
                            </div>
                            <div class='sxlm'>
                                <asp:Label ID="lblMailingState" CssClass="ronly" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class='cptn'><asp:Label ID="lblZipOrPostalCode" Text="" runat="server" class="cptn"></asp:Label></span>
                        </div>

                        <div class='sxlm'>
                            <asp:Label ID="lblMailingZipCode" CssClass="ronly" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>


<div class="section">
    <h1 class="mhdr" tabindex="0"><%=Resources.Language.PAYMENTDETAILS%>
    </h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlPaymentDetails">
                <h3>
                    <asp:Label Text="" runat="server" ID="Label2" CssClass="phdr" />
                </h3>
                <asp:Repeater ID="rptOrderPAymentDetail" runat="server" OnItemDataBound="rptOrderPAymentDetail_ItemDataBound" OnItemCommand="rptOrderPAymentDetail_ItemCommand">
                    <ItemTemplate>
                        <div id="ContainerDiv" runat="server">
                            <hr id="hrline" runat="server" style="border-bottom: solid 1px #c0c0c0;" />

                            <div id="dvItemPaymentDetail" runat="server" style="display: none">
                                <div class='sxro sx3co'>
                                    <div class='sxlb' title="The method used to pay for this order">
                                        <span id="spnItemName" class='cptn'>Item Name</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-describedby="spnItemName" ID="txtItemName" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxlb' title="The method used to pay for this order">

                                        <span id="spnCIDOrOrderLabel" class='cptn'>
                                            <asp:Label runat="server" ID="lblCIDOrOrderLabel"></asp:Label></span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-describedby="spnCIDOROrderLabel" ID="txtCIDOROrderNumber" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>

                                </div>
                            </div>

                            <div class='sxro sx3co'>
                                <%--<div class='sxlb' title="<%$ Resources:Language, MTHDUSDTOPAYFRORDR %>">--%>
                                <div class='sxlb' title="The method used to pay for this order">
                                    <span id="spnPaymentType" class='cptn'><%=Resources.Language.PAYMENTTYPE%></span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-describedby="spnPaymentType" ID="txtPaymentType" ReadOnly="true">
                                    </infs:WclTextBox>
                                </div>
                                <div id="dvChangePaymentType" runat="server">
                                    <div class='sxlb'>
                                        <div id="vermod_cmds">
                                            <asp:LinkButton Text="<%$ Resources:Language, CHNGPAYMENTTYPE %>" runat="server" ID="lnkChangePaymentType" OnClick="lnkChangePaymentType_click" />
                                        </div>
                                    </div>
                                </div>
                                <div id="divReferenceNumber" runat="server">
                                    <%--<div class='sxlb' title="<%$ Resources:Language, IDNTFYNOFRMNYORDR %>">--%>
                                    <div class='sxlb' title="Identification Number for Money Orders and Invoices">
                                        <span id="spnRefNumber" class='cptn'><%=Resources.Language.REFNO%></span><span id="spRefNo" class="reqd" runat="server"
                                            visible="false">*</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtReferenceNumber" aria-describedby="spnRefNumber" MaxLength="200" EnableAriaSupport="true">
                                        </infs:WclTextBox>
                                        <div class='vldx'>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvReferenceNumber" Display="Dynamic"
                                                ControlToValidate="txtReferenceNumber" class="errmsg" ErrorMessage="<%$ Resources:Language, REFNOREQ %>"
                                                ValidationGroup="grpFormSubmit" />
                                        </div>
                                    </div>
                                </div>

                                <div id="divPaymentApprovedBy" runat="server">
                                    <div class='sxlb' title="This order has been approved by">
                                        <span id="spnPymntAprvdBy" runat="server" class='cptn'>Payment Approved By</span>
                                    </div>
                                </div>
                                <div class='sxlm' id="divlblPaymentApprovedBy" runat="server" style="padding-top: 5px;">
                                    <asp:Label ID="lblPaymentApprovedBy" runat="server" aria-describedby="spnPymntAprvdBy" MaxLength="200" EnableAriaSupport="true">
                                    </asp:Label>
                                </div>

                                <div class='sxroend'>
                                </div>
                            </div>
                            <div id="divOrderStatusAndAmunt" runat="server">
                                <div class='sxro sx3co'>
                                    <%--<div class='sxlb' title="<%$ Resources:Language, MTHDUSDTOPAYFRORDR %>">--%>
                                    <div class='sxlb' title="The method used to pay for this order">
                                        <span id="spnOrderStatus" class='cptn'><%=Resources.Language.PAYMENTSTATUS%></span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-describedby="spnOrderStatus" ID="txtPaymentOrderStatus" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>
                                    <div id="dvOrderPaymentAmount" runat="server">
                                        <%--<div class='sxlb' title="<%$ Resources:Language, MTHDUSDTOPAYFRORDR %>">--%>
                                        <div class='sxlb' title="The method used to pay for this order">
                                            <span id="spnAmount" class='cptn' runat="server"></span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclNumericTextBox runat="server" EnableAriaSupport="true" aria-describedby="spnAmount" ID="txtOrderPaymentAmount" ReadOnly="true" NumberFormat-DecimalDigits="2"
                                                Type="Currency">
                                            </infs:WclNumericTextBox>
                                            <%-- <infs:WclTextBox runat="server" ID="txtOrderPaymentAmount" Enabled="false">
                                    </infs:WclTextBox>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="dvTrackingCnclBy" runat="server">
                                <div class='sxro sx3co'>
                                    <%--<div class='sxlb' title="<%$ Resources:Language, MTHDUSDTOPAYFRORDR %>">--%>
                                    <div class='sxlb' title="The method used to display the name of user who cancelled the order">
                                        <span id="spnTrackCNCLBY" class='cptn'><%=Resources.Language.TRACKCANCLBY%></span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-describedby="spnTrackCNCLBY" ID="txtTrackCNCLBY" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>
                                </div>
                            </div>
                            <div id="dvPreviousOrderStatus" visible="false" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb' title="">
                                        <span id="spnPrevOrderStatus" class='cptn'><%=Resources.Language.PREVORDSTATUS%></span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-describedby="spnPrevOrderStatus" ID="txtPrevOrderStatus" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>
                                </div>
                            </div>
                            <div id="divNewPaymentType" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb' title="<%=Resources.Language.SELPAYMTHD %>">
                                        <%-- <div class='sxlb' title="Select a payment method">--%>
                                        <span id="spnNewPaymentType" class='cptn'><%=Resources.Language.NEWPAYTYPE%></span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclComboBox ID="cmbPaymentModes" EnableAriaSupport="true" aria-describedby="spnNewPaymentType" runat="server" DataTextField="Name" DataValueField="PaymentOptionID" OnSelectedIndexChanged="cmbPaymentModes_SelectedIndexChanged" AutoPostBack="true">
                                        </infs:WclComboBox>
                                    </div>
                                    <%--<div class='sxlb' title="Identification Number for Money Orders and Invoices">
                            <span class='cptn'>Reference Number</span><span id="spNewRefNo" class="reqd" runat="server"
                                visible="false">*</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclTextBox runat="server" ID="txtNewReferenceNumber" Enabled="false">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvNewReferenceNumber" Display="Dynamic"
                                    ControlToValidate="txtNewReferenceNumber" class="errmsg" ErrorMessage="Reference Number is required."
                                    ValidationGroup="grpFormSubmit" />
                            </div>
                        </div>--%>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </div>
                            <div id="dvPaymentRejection" runat="server">
                                <div class='sxro sx3co' runat="server" id="divRejectionReason">
                                    <div class='sxlb'>
                                        <span id="spnRejectionReason" class='cptn'><%=Resources.Language.RJCTNRSN%></span><span class="reqd">*</span>
                                    </div>
                                    <div class='sxlm' style="width: 52%">
                                        <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-describedby="spnRejectionReason" ID="txtPaymentRejection" MaxLength="1024" TextMode="MultiLine">
                                        </infs:WclTextBox>
                                        <div class='vldx'>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvPaymentRejection" Display="Dynamic"
                                                ControlToValidate="txtPaymentRejection" class="errmsg" ErrorMessage="<%$ Resources:Language, REJCTNRSNREQ %>"
                                                ValidationGroup="grpPaymntReject" />
                                        </div>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div style="width: 100%; text-align: center" id="divApprovePayment" runat="server">
                                    <infs:WclButton runat="server" ID="btnRejectPayment" Icon-PrimaryIconCssClass="rbCancel" CommandName="RejectPayment"
                                        Text="<%$ Resources:Language, RJCTPAY %>" ValidationGroup="grpPaymntReject" CausesValidation="true">
                                        <%--OnClick="btnRejectPayment_Click"--%>
                                    </infs:WclButton>
                                    <infs:WclButton runat="server" ID="btnApprovePayment" Icon-PrimaryIconCssClass="rbOk" CommandName="ApprovePayment"
                                        Text="<%$ Resources:Language, APRVPAY %>" ValidationGroup="grpFormSubmit">
                                        <%-- OnClick="btnApprovePayment_OnClick" --%>
                                    </infs:WclButton>

                                </div>
                            </div>
                            <div style="width: 100%; text-align: center" id="divSaveNewPaymentType" runat="server" visible="false">
                                <infs:WclButton runat="server" ID="btnSaveNewPaymentType" Text="<%$ Resources:Language, SUBMITORD %>" OnClientClicked="CallSubmitOrderPayTypeChanged">
                                </infs:WclButton>
                            </div>
                            <asp:HiddenField ID="hdnOrderPaymentDetailID" runat="server" />
                            <asp:HiddenField ID="hdfpaymentTypeCode" runat="server" />
                            <asp:HiddenField ID="hdfOrderPackageType" runat="server" />
                            <asp:HiddenField ID="hdfOrderStatusCode" runat="server" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <div style="width: 100%; text-align: center; padding-top: 5px;" id="divCancelOrder" runat="server">
                    <infs:WclButton runat="server" ID="btnCancelOrder" Icon-PrimaryIconCssClass="rbCancel" CommandName="CancelOrder"
                        Text="<%$ Resources:Language, CNCLORDR %>" ValidationGroup="grpOfflineSettlement" CausesValidation="false" OnClick="btnCancelOrder_Click">
                    </infs:WclButton>
                    <infs:WclButton runat="server" ID="btnCancelOrderTmp" AutoPostBack="false" Icon-PrimaryIconCssClass="rbCancel" Text="<%$ Resources:Language, CNCLORDR %>" CausesValidation="false">
                    </infs:WclButton>
                </div>
            </asp:Panel>
        </div>
    </div>
    <asp:HiddenField ID="hdnLocId" runat="server" />

    <%--<div id="divOrdLineItem" runat="server">
        <infsu:OLI ID="uc_OLI_OrderPaymentDetail" runat="server" />
    </div>--%>
</div>

<div class="section" id="dvAppointmentInfo" runat="server" visible="false">
    <h1 class="mhdr" tabindex="0" runat="server" id="hrAILocationTenant"><%=Resources.Language.APPMNTDETAILS%>
    </h1>
    <h1 class="mhdr" tabindex="0" runat="server" id="hrAI"><%=Resources.Language.APPNMNTINFO%>
    </h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAppointmentInfo">
                <div runat="server" id="dvAppointmentInfoData">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <label id="lblLocationName" runat="server" for="txtLocationName" class="cptn"><%=Resources.Language.NAME%></label>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ClientIDMode="Static" ID="txtLocationName" ReadOnly="true" EnableAriaSupport="true">
                            </infs:WclTextBox>
                        </div>

                        <div class='sxlb'>
                            <span class='cptn'><%=Resources.Language.DESCRIPTION%></span>
                        </div>
                        <div class='sxlm'>
                            <asp:Label ID="lblSiteDescription" CssClass="ronly" runat="server"></asp:Label>
                        </div>

                        <div class='sxlm'>
                            <asp:LinkButton ID="btnViewLocImage" runat="server" Text="<%$ Resources:Language, VWLOCIMAGES %>" ToolTip="<%$ Resources:Language, CLKTOVWLOCIMGS %>" OnClick="btnViewLocImage_Click"></asp:LinkButton>
                        </div>
                    </div>
                    <div class='sxro sx3co' id="dvLocAdd" runat="server">
                        <div class='sxlb'>
                            <label id="lblLocationAddress" runat="server" for="txtLocationAddress" class="cptn"><%=Resources.Language.ADDRESS%></label>
                        </div>
                        <div class='sxlm' style="width: 50%">
                            <infs:WclTextBox runat="server" ClientIDMode="Static" ID="txtLocationAddress" ReadOnly="true" EnableAriaSupport="true" Width="100%">
                            </infs:WclTextBox>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <label id="lblAppointmentFor" runat="server" class="cptn"><%=Resources.Language.APPMNTTIME%></label>
                        </div>
                        <div class='sxlm' style="padding-top: 5px; padding-left: 5px; width: 18.5%">
                            <asp:Label runat="server" ClientIDMode="Static" ID="lblAppointmentDateTime" EnableAriaSupport="true" Width="100%">
                            </asp:Label>
                        </div>
                        <%--   <div class='sxlb'>
                        <label id="lblAppointmentStatus" runat="server" for="txtAppointmentStatus" class="cptn">Appointment Status</label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ClientIDMode="Static" ID="txtAppointmentStatus" ReadOnly="true" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>--%>
                        <div class='sxlm' id="dvRescheduleAppoinment" style="padding-top: 5px; padding-left: 0px; float: left" runat="server">
                            <asp:LinkButton runat="server" ID="lnkChangeAppointment" Text="<%$ Resources:Language, RSCHDLAPPNMNT %>" oid='<%#Eval("OrderId")%>' OnClick="lnkChangeAppointment_Click"></asp:LinkButton>
                            <%-- OnClientClick ="return openOrderPayment(this)"--%>
                        </div>
                    </div>
                </div>
                <div id="dvOutOfStateAppointmentDetails" runat="server" visible="false">
                    <%--<div class="mhdr" style="position: relative; bottom: 2px;">
                        <h1 style="font-size: 14px; padding-bottom: 2px; margin: 0px;">Appointment Details</h1>
                    </div>--%>
                    <%--<div class="content">--%>
                    <%-- <div class="sxform auto">--%>
                    <%--   <asp:Panel runat="server" CssClass="sxpnl" ID="pnlOutOfStateAppointmentDetails">--%>
                    <div class='sxro sx3co' id="Div1" runat="server">
                        <div class='sxlb'>
                            <label id="Label4" runat="server" for="txtLocationAddress" class="cptn"><%=Resources.Language.APPMNTSTATUS%></label>
                        </div>
                        <div class='sxlm' style="width: 50%">
                            <infs:WclTextBox runat="server" ClientIDMode="Static" ID="txtApptStatus" ReadOnly="true" Text="<%$ Resources:Language, MAILINPROCESS %>" EnableAriaSupport="true" Width="100%">
                            </infs:WclTextBox>
                        </div>
                    </div>

                    <%-- </asp:Panel>--%>
                    <%--  </div>--%>
                    <%-- </div>--%>
                </div>

                <%--  <div class='sxro sx3co'>--%>

                <div id="dvCancelOrder" style="width: 100%; text-align: center;" runat="server" class="cancelBtnWidth sxro sx3co">
                    <infs:WclButton runat="server" ID="btnCancelBkgOrder" Icon-PrimaryIconCssClass="rbCancel"
                        Text="<%$ Resources:Language, CNCLORDR %>" OnClick="btnCancelBkgOrder_Click">
                    </infs:WclButton>
                </div>
                <%--</div>--%>
            </asp:Panel>

        </div>
        <div class='sxro sx3co' id="dvUCAppointmentRescheduler" visible="false" runat="server">
            <uc:AppointmentRescheduler ID="ucAppointmentRescheduler" runat="server" />
        </div>
        <div class='sxro sx3co' id="divUCScheduleLocationUpdateControl" runat="server" style="display: none">
            <uc:ScheduleLocationUpdateControl ID="ucScheduleLocationUpdateControl" runat="server" />
        </div>
    </div>
    <div id="dvAppointmentButtons" style="width: 100%; text-align: center" runat="server" visible="false">
        <infs:WclButton runat="server" ID="btnSelectAppointment" CssClass="padRight2" Icon-PrimaryIconCssClass="rbNext" Visible="false"
            Text="<%$ Resources:Language, NEXT %>" OnClick="btnSelectAppointment_Click">
        </infs:WclButton>
        <infs:WclButton runat="server" ID="btnSaveAppointment" CssClass="padRight2" Icon-PrimaryIconCssClass="rbSave" Visible="false"
            Text="<%$ Resources:Language, SAVE %>" OnClick="btnSaveAppointment_Click">
        </infs:WclButton>
        <infs:WclButton runat="server" ID="btnCancel" Icon-PrimaryIconCssClass="rbCancel"
            OnClick="btnCancel_Click" Text="<%$ Resources:Language, CNCL %>">
        </infs:WclButton>
    </div>


</div>

























<div class="section" id="divSvcFrm" runat="server" visible="false">
    <h1 class="mhdr" tabindex="0"><%=Resources.Language.SRVCFORMS%>
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="sxpnl">
                <div class="swrap">
                    <infs:WclGrid runat="server" ID="grdServiceForms" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false"
                        AutoSkinMode="True" CellSpacing="0" GridLines="Both" ShowClearFiltersButton="false">
                        <MasterTableView CommandItemDisplay="Top" AllowPaging="false" PagerStyle-Visible="false" AllowSorting="false" AllowFilteringByColumn="false">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="ServiceFormName" FilterControlAltText="Filter ServiceFormName column" AllowFiltering="false"
                                    HeaderText="<%$ Resources:Language, SRVCFORMNAME %>" SortExpression="ServiceFormName" UniqueName="ServiceFormName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ServiceName" FilterControlAltText="Filter ServiceName column" AllowFiltering="false"
                                    HeaderText="<%$ Resources:Language, BKGSRVCNAME %>" SortExpression="ServiceName" UniqueName="ServiceName">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="imgServiceForm">
                                    <ItemTemplate>
                                        <telerik:RadButton ID="btnNotificationPdf" OnClientClicked="openPdfPopUp" AutoPostBack="false" CssClass="classImagePdf"
                                            runat="server" Font-Underline="true">
                                            <Image EnableImageButton="true" />
                                        </telerik:RadButton>
                                        <asp:HiddenField ID="hdnfSystemDocumentId" runat="server" Value='<%#Eval("SystemDocumentID")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </infs:WclGrid>
                </div>
                <div class="gclr">
                </div>
            </div>
        </div>
    </div>
</div>
<div id="divServiceLevelDetails" runat="server" visible="false"></div>
<div id="errorMessageBox" class="msgbox" runat="server">
    <asp:Label ID="lblError" runat="server" CssClass="error" Text="">
    </asp:Label>
</div>
<infsu:CommandBar ID="cbbuttons" runat="server" ButtonPosition="Center" DisplayButtons="Clear,Cancel"
    AutoPostbackButtons="Clear,Cancel" ClearButtonIconClass="rbNext" ClearButtonText="<%$ Resources:Language, NEXT %>"
    Visible="false" CancelButtonText="<%$ Resources:Language, CNCL %>" OnClearClick="CmdBarNext_Click" OnCancelClick="CmdBarCancel_Click">
</infsu:CommandBar>
<div style="width: 100%; text-align: center" id="dvShowBackLink" runat="server" visible="false">
    <%-- <infs:WclButton runat="server" ID="btnSaveNewPaymentType" Text="Submit Order" OnClientClicked="SubmitOrderPayTypeChanged">
    </infs:WclButton>--%>
    <infs:WclButton runat="server" ID="btnGoBack" Text="<%$ Resources:Language, GOBCKORDHSTRY %>" OnClick="CmdBarCancel_Click">
    </infs:WclButton>
</div>
<div id="confirmSave" class="confirmProfileSave" runat="server" style="display: none">
    <p style="text-align: left">Do you want to continue?</p>

</div>

<div id="ConfirmationMsgPopup" title="Complio" runat="server" style="display: none">
    <p style="text-align: left;">This order is set up as School Approval Only and need approval from applicant's school. Do you still want to continue?</p>
</div>

<asp:HiddenField ID="hdnTenantID" runat="server" />
<asp:HiddenField ID="hdnOrderID" runat="server" />
<asp:HiddenField ID="hfCurrentUserID" runat="server" />
<asp:HiddenField ID="hdfShowOffLineSettlement" runat="server" />
<asp:HiddenField ID="hdfShowApprovePaymentSetting" runat="server" />
<asp:HiddenField ID="hdnIsLocationServiceTenant" runat="server" />
<asp:HiddenField ID="hdnConfirmSave" runat="server" Value="0" />
<asp:HiddenField ID="hdnPopUpText" runat="server" Value="" />
<asp:HiddenField ID="hdfFingerPrint" runat="server" Value="" />
<asp:HiddenField ID="hdfPassport" runat="server" Value="" />
<asp:HiddenField ID="hdnCurrentOrderPaymentdetailID" runat="server" Value="" />
<asp:HiddenField ID="hdnConfirmApproveOrderPayment" runat="server" Value="0" />
<asp:Button ID="btnApprovePaymentHide" runat="server" Style="display: none;" OnClick="btnApprovePaymentHide_Click" />
<infs:WclButton ID="btnUpdateOrderDetails" CssClass="diplayHidden" AutoPostBack="true" runat="server" OnClick="btnUpdateOrderDetails_Click" Visible="false" />
<style>
    .classImagePdf {
        background: url(../../images/medium/pdf.gif);
        background-position: 0 0;
        background-repeat: no-repeat;
        width: 20px;
        height: 20px;
    }
</style>
<script type="text/javascript">
    function pageLoad() {

        if ($jQuery('#pageMsgBox').css('display') != 'none') {
            //$jQuery("#pnlError").attr("tabindex", -1).focus();
            $jQuery("#lblError").attr("tabindex", -1).focus();
        }

        $jQuery('[id$=btnCancelOrder]').attr('style', 'display:none;');

        $jQuery('[id$=btnCancelOrderTmp]').keydown(function (e) {
            if (e.keyCode == 13) {
                if (confirm('Are you sure you wish to cancel this order?')) {
                    $jQuery('[id$=btnCancelOrder]').click();
                }
                else {
                    return false;
                }
            }
        });

        $jQuery('[id$=btnCancelOrderTmp_input]').click(function (e) {
            if (confirm('Are you sure you wish to cancel this order?')) {
                $jQuery('[id$=btnCancelOrder_input]').click();
            }
            else {
                return false;
            }
        });

        <%--function pageLoad()  ///Code to remove
        {
            alert('test');
            var selectedvalue = $('#<%=rblChoosLocation.ClientID%> input:checked').val();
        OnClieckFingerprinting(selectedvalue, 'CallPageLoad');--%>
        //setTimeout(function () { LocationMap(); }, 6000);
        //$jQuery('[id$=hdn]')

        try {
            var hdnIsLocationServiceTenant = $jQuery('[id$=hdnIsLocationServiceTenant]').val();
            if (hdnIsLocationServiceTenant == 'true') {
                if (typeof gmarker !== "undefined" && gmarker && gmarker.length > 0) {
                    gmarker = [];
                }

                if (typeof Id !== "undefined" && Id && Id > 0) {
                    Id = 0;
                }
                LocationMap();
            }
        }
        catch (err) {
            //google map issue for non location based tenants
        }

        //ShowDiv();
        //}
    }

    function OnCloseItemPymtPopup(oWnd, args) {
        oWnd.remove_close(OnClose);
        win = false;
        $jQuery("[id$=btnUpdateOrderDetails]").click();
    }

    function openOrderPayment(ctrl) {

        var TenantId = $jQuery("#<%= hdnTenantID.ClientID %>").val();
        var OrderId = $jQuery("[id$=hdnOrderID]").val();
        var hdfFingerPrint = $jQuery("[id$=hdfFingerPrint]").val();
        var hdfPassport = $jQuery("[id$=hdfPassport]").val();


        //alert($jQuery("[id$=" + containerID + "hdnfOrderID]").val());
        var popupWindowName = "Order Payment Details";
        var widht = (window.screen.width) * (90 / 100);
        var height = (window.screen.height) * (80 / 100);
        var popupsize = widht + ',' + height;
        var a = "~/ComplianceOperations/Pages/OrderPaymentDetails.aspx?TenantId=" + TenantId + "&OrderId=" + OrderId + "&hdfFingerPrint=" + hdfFingerPrint + "&hdfPassport=" + hdfPassport;

        var url = $page.url.create(a);
        var win = $window.createPopup(url, { size: popupsize, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnCloseItemPymtPopup }
        );
        return false;

    }

    function EnableSubmitOrder() {
        var btnSubmitOrder = $find("<%= btnSelectAppointment.ClientID %>");
        if (btnSubmitOrder != null)
            btnSubmitOrder.set_enabled(true);
    }

    function openImageSliderPopup() {
        var LocationId = $jQuery("[id$=hdnLocId]").val();
        var composeScreenWindowName = "Location Images";
        var popupHeight = $jQuery(window).height() * (100 / 100);
        var url = $page.url.create("~/FingerPrintSetUp/Pages/ImageViewer.aspx?LocationId=" + LocationId);
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
    }
    var winopen = false;

    function openPdfPopUp(sender) {
        var btnID = sender.get_id();
        var containerID = btnID.substr(0, btnID.indexOf("btnNotificationPdf"));
        var hdnfSystemDocumentId = $jQuery("[id$=" + containerID + "hdnfSystemDocumentId]").val();
        var documentType = "ServiceFormDocument";
        var composeScreenWindowName = "Service Form Details";
        //UAT-2364
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

    //UAT-796 -Function to turn on/off Automatic renewal of an applicant.
    function ResetAutoRenewalStatus(sender) {
        //debugger;
        if (sender.attributes.Enabled != undefined && sender.attributes.Enabled.value == "false") {
            return false;
        }
        var btnID = sender.id;
        var containerID = btnID.substr(0, btnID.indexOf("btnAutoRenewal"));
        var tenantId = $jQuery("#<%= hdnTenantID.ClientID %>").val();
        var orderID = $jQuery("[id$=" + containerID + "hdnOrderID]").val();
        var currentUserID = $jQuery("#<%= hfCurrentUserID.ClientID %>").val();
        var urltoPost = "/ComplianceOperations/Default.aspx/ResetAutoRenewalStatus";

        var dataString = "tenantID : '" + tenantId + "', orderID : '" + orderID + "', currentUserID : '" + currentUserID + "', buttonid : '" + btnID + "'";//, autoRenewalCurrentValue : '" + autoRenewalCurrentValue +"'";
        $jQuery.ajax
            (
            {
                type: "POST",
                url: urltoPost,
                data: "{ " + dataString + " }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var data = JSON.parse(result.d);

                    if (data.orderRenwalStatus == "True") {
                        $jQuery('#' + data.buttonid).html("ON");
                        $jQuery('#' + data.buttonid)[0].title = "Click to Turn Off Auto Renewal"
                    }
                    else if (data.orderRenwalStatus == "False") {
                        $jQuery('#' + data.buttonid).html("OFF");
                        $jQuery('#' + data.buttonid)[0].title = "Click to Turn On Auto Renewal"
                    }
                }
            });
        return false;
    }

    //UAT-872
    function ConfirmCancel(sender, eventArgs) {
        if (confirm('Are you sure you wish to cancel this order?')) {
            sender.set_autoPostBack(true);
        }
        else {
            sender.set_autoPostBack(false);
        }
    }

    function CallSubmitOrderPayTypeChanged(sender, args) {
        var paymentOptionId;
        var opdid = sender._element.getAttribute("OPDID");
        var containerdivID = "ContainerDiv_" + opdid;
        paymentOptionId = $find($jQuery('.' + containerdivID).find("[id$=cmbPaymentModes]")[0].id).get_value();
        SubmitOrderPayTypeChanged(paymentOptionId, opdid);
    }

    //function pageLoad() {
    //    $jQuery('[id$=btnCancelOrder]').attr('style', 'display:none;');

    //    $jQuery('[id$=btnCancelOrderTmp]').keydown(function (e) {
    //        if (e.keyCode == 13) {
    //            if (confirm('Are you sure you wish to cancel this order?')) {
    //                $jQuery('[id$=btnCancelOrder]').click();
    //            }
    //            else {
    //                return false;
    //            }
    //        }
    //    });

    //    $jQuery('[id$=btnCancelOrderTmp_input]').click(function (e) {
    //        if (confirm('Are you sure you wish to cancel this order?')) {
    //            $jQuery('[id$=btnCancelOrder_input]').click();
    //        }
    //        else {
    //            return false;
    //        }
    //    });
    //}

    //UAT-2970
    function SetOrderConfirmationMailWithDoc(tenantId, currentLoggedInUserId, orderId, orderPaymentDetailID) {
        PageMethods.SetOrderConfirmationDocForCreditCard(tenantId, currentLoggedInUserId, orderId, orderPaymentDetailID);
    }

    //UAT-3166
    function ServiceDetailsPopUp() {
        //debugger;
        var popupWindowName = "Service Details";
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var tenantId = $jQuery("#<%= hdnTenantID.ClientID %>").val();
        var orderId = $jQuery("#<%= hdnOrderID.ClientID %>").val();

        var url = $page.url.create("~/BkgSetup/Pages/BkgPackageServiceDetails.aspx?TenantID=" + tenantId + "&OrderID=" + orderId);
        var win = $window.createPopup(url, { size: "980," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close, onclose: OnClientClose },
            function () {
                this.set_title(popupWindowName);
            });
        return false;
    }

    function OnClientClose(oWnd, args) {
        //debugger;
        oWnd.remove_close(OnClientClose);
        winopen = false;
    }

    //function CheckRowRdBtn(sender, args) {
    //    //debugger;
    //    //MarkTheLocationSelected(sender, args.get_gridDataItem());
    //}
    function confirmClick() {
        debugger;
        var popUpText = $jQuery("[id$=hdnPopUpText]").val();
        $jQuery(".confirmProfileSave p").text(popUpText);
        var dialog = $window.showDialog($jQuery(".confirmProfileSave").clone().show(), {
            approvebtn: {
                autoclose: true, text: "Proceed and Approve Payment", click: function () {
                    debugger;
                    $jQuery("[id$=hdnConfirmSave]").val(1);
                    //$jQuery("[id$=btnApprovePaymentHide]").trigger('click');
                    //$jQuery("[id*=btnApprovePaymentHide]").trigger("click");
                    $jQuery("#<%=btnApprovePaymentHide.ClientID %>").trigger('click');
                }
            }, closeBtn: {
                autoclose: true, text: "Cancel", click: function () {
                    $jQuery("[id$=hdnConfirmSave]").val(0);
                    return false;
                }
            }
        }, 475, 'Alert');
    }

    function ShowCCOrdersRequirApprovalConfirmationPopup() {
        //debugger;
        var dialog = $window.showDialog($jQuery("[id$=ConfirmationMsgPopup]").clone().show(), {
            approvebtn: {
                autoclose: true, text: "Proceed and Approve Payment", click: function () {

                    $jQuery("[id$=hdnConfirmApproveOrderPayment]").val(1);
                    $jQuery("#<%=btnApprovePaymentHide.ClientID %>").trigger('click');
                }
            }, closeBtn: {
                autoclose: true, text: "Cancel", click: function () {
                    $jQuery("[id$=hdnConfirmApproveOrderPayment]").val(0);
                    return false;
                }
            }
        }, 550, 'Alert');
    }
</script>
