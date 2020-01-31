<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.OrderConfirmation" CodeBehind="OrderConfirmation.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="PackageDetails" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/PackageDetails.ascx" %>
<%@ Register TagPrefix="infsu" TagName="OtherDetails" Src="~/ComplianceAdministration/UserControl/CustomAttributeLoader.ascx" %>
<%@ Register TagPrefix="uc" TagName="PaymentInstructions" Src="~/ComplianceOperations/UserControl/PaymentInstructions.ascx" %>
<%@ Register TagPrefix="infsu" TagName="OLI" Src="~/ComplianceOperations/UserControl/OrderLineItems.ascx" %>
<meta charset="utf-8" />

<style type="text/css">
    .sucs {
        display: block;
        padding: 20px;
        padding: 15px 10px 20px 53px;
        border-width: 1px;
        margin: 0px;
        color: green !important;
        background-image: url('../../Resources/Themes/Default/images/success.png');
        background-color: #fffef0;
        background-position: 10px 8px;
        background-repeat: no-repeat;
        border-color: green;
    }

    /*UAT-3726*/
    .cmdButtonMinSize .sxcbar .RadButton .rbNext {
        left: 0px !important;
    }

        .cmdButtonMinSize .sxcbar .RadButton .rbNext + input {
            padding-left: 17px !important;
        }

    .rbMail, .rbPrevious, .rbNext {
        margin-left: 2px;
    }
</style>

<div class="section">
    <h2 style="width: 80%; margin: 0 auto;"><%= Resources.Language.THANKSORDERCONFIRMD%></h2>
    <asp:Panel ID="pnlMain" runat="server">
        <iframe id="iframe" runat="server" width="100%" height="0px"></iframe>
        <div style="direction: rtl; margin-right: 130px;">
            <%-- <asp:LinkButton ID="lnkDownloadPdf" OnClientClick="GetHtmlFromDiv()" runat="server">...Save as PDF</asp:LinkButton>--%>
        </div>
        <div runat="server" id="pnl" class="dvPdfViewer" style="width: 80%; margin: 0 auto; margin-top: 20px">
            <h1></h1>
            <asp:Label ID="lblMessage" runat="server" Text="" CssClass="sucs" Visible="false"></asp:Label>
            <%--<link href="../../App_Themes/Default/core.css" rel="stylesheet" type="text/css" />--%>
            <h1 style="border-bottom-color: #adadad; border-bottom-style: solid; border-bottom-width: 0px; border-left-color: #adadad; border-left-style: solid; border-left-width: 0px; border-right-color: #adadad; border-left-style: solid; border-bottom-width: 0px; border-top-color: #adadad; border-top-style: solid; border-top-width: 0px; color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 150%; font-weight: 700; margin-bottom: 10px; margin-left: 0px; margin-right: 0px; margin-top: 10px; padding-bottom: 0px; padding-left: 0px; padding-right: 0px; padding-top: 0px; position: relative; word-spacing: 2px"><%= Resources.Language.ORDERSUMMARY%></h1>
            <span style="float: right"><%--Order Summary--%>
                <infs:WclButton runat="server" ID="WclButton1" Text="<%$ Resources:Language, PRINT %>" OnClientClicked="GetHtmlFromDiv"
                    AutoPostBack="false">
                    <Icon PrimaryIconCssClass="rbPrint" />
                </infs:WclButton>
            </span><span style="clear: both"></span>
            <h1></h1>
            <div style="background-color: #FFF; border: 1px solid #C7C7C8; font-size: 13px; line-height: 150%; padding: 20px; margin-bottom: 20px">
                <h6 id="hdrOrderDetail" runat="server" style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Order Detail <%--Subscription Detail--%></h6>
                <%--Order Detail--%>
                <div>
                    <div style="display: inline-block; width: 100%; vertical-align: top; float: left">
                        <span><%= Resources.Language.INSTHIERARCHY%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblInstitutionHierarchy" runat="server"></asp:Label>
                            <%-- <span>Institution Hierarchy</span>--%>
                        </span>
                    </div>
                    <div style="clear: both">
                    </div>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.ODRNUMBER%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblOrderId" runat="server" Style="display: none"></asp:Label>
                            <%--<span>Order Number</span>--%>
                            <asp:Label ID="lblOrderNumber" runat="server"></asp:Label>
                        </span>
                    </div>
                    <%--UAT-1059:Remove I.P. address and mask social security number from order summary--%>
                    <%--<div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Applicant Machine IP</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblIPAddress" runat="server"></asp:Label>
                            </span>
                        </div>--%>
                    <%--<div style="clear: both">
                        </div>--%>
                    <div id="divRushOrder" runat="server" style="display: inline-block; width: 30%; vertical-align: top; float: left" visible="false">
                        <span>Rush Order Price</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblRushOrderPrice" runat="server"></asp:Label>
                        </span>
                    </div>
                    <div id="dvTotalPrice" runat="server" style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.TOTALPRICE%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblTotalPrice" runat="server"></asp:Label>
                        </span>
                    </div>
                    <div style="clear: both">
                    </div>
                </div>
                <hr style="border-bottom: solid 1px #c0c0c0;" />
                <div>
                    <h6 id="hdrPackageDetail" runat="server" style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Package Detail</h6>
                    <div id="dvCompliancePkgs">
                        <asp:Repeater ID="rptCompliancePkgs" runat="server">
                            <ItemTemplate>
                                <div id="divCompliancePackage" runat="server" visible="false">
                                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                                        <span>Compliance Tracking Package</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblPackage" runat="server"></asp:Label>
                                        </span>
                                    </div>
                                    <div id="divCPT" runat="server" style="display: inline-block; width: 23%; vertical-align: top; float: left">
                                        <span><%= Resources.Language.PAYMENTTYPE%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblCompliancePkgPaymentType" runat="server"></asp:Label>
                                            <%--<span>Payment Type</span>--%>
                                        </span>
                                    </div>
                                    <div id="dvPackagePrice" runat="server" style="display: inline-block; width: 20%; vertical-align: top; float: left">
                                        <span>Package Price</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblPrice" runat="server"></asp:Label>
                                        </span>
                                    </div>
                                    <div style="display: inline-block; width: 20%; vertical-align: top; float: left">
                                        <span>Subscription Period</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblSubscription" runat="server"></asp:Label>
                                        </span>Month(s)
                                    </div>
                                    <div style="clear: both">
                                    </div>
                                </div>

                                <asp:HiddenField ID="hdfPackageId" runat="server" Value='<%# Eval("CompliancePackageID") %>' />
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <%--  <div id="divCompliancePackage_2" runat="server" visible="false">
                            <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                                <span>Compliance Tracking Package</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblPackage_2" runat="server"></asp:Label>
                                </span>
                            </div>
                            <div id="divCPT_2" runat="server" style="display: inline-block; width: 23%; vertical-align: top; float: left">
                                <span>Payment Type</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblCompliancePkgPaymentType_2" runat="server"></asp:Label>
                                </span>
                            </div>
                            <div id="dvPackagePrice_2" runat="server" style="display: inline-block; width: 20%; vertical-align: top; float: left">
                                <span>Package Price</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblPrice_2" runat="server"></asp:Label>
                                </span>
                            </div>
                            <div style="display: inline-block; width: 20%; vertical-align: top; float: left">
                                <span>Subscription Period</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblSubscription_2" runat="server"></asp:Label>
                                </span>Month(s)
                            </div>
                            <div style="clear: both">
                            </div>
                        </div>--%>
                    <div id="divBackgroundPackage" runat="server" visible="false">
                        <asp:Repeater ID="rptBackgroundPackages" runat="server" OnItemDataBound="rptBackgroundPackages_ItemDataBound">
                            <ItemTemplate>
                                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                                    <span id="spnBkgPackage" runat="server">Background Package</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblBkgPackage" runat="server" Text='<%# String.IsNullOrEmpty( Convert.ToString( Eval("BPA_Label"))) 
                                            ? Convert.ToString( Eval("BPA_Name"))
                                            : Convert.ToString( Eval("BPA_Label"))%>'></asp:Label>
                                    </span>
                                    <asp:HiddenField ID="hdfBOPId" runat="server" Value='<%# Eval("BkgOrderPackageID") %>' />
                                    <asp:HiddenField ID="hdfBPAId" runat="server" Value='<%# Eval("BPA_ID") %>' />
                                </div>
                                <div id="divBPT" runat="server" style="display: inline-block; width: 23%; vertical-align: top; float: left">
                                    <span><%= Resources.Language.PAYMENTTYPE%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblPaymentType" runat="server"></asp:Label>
                                    </span>
                                </div>
                                <div id="dvBkgPackagePrice" runat="server" style="display: inline-block; width: 30%; vertical-align: top; float: left">
                                    <span>Package Price</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblBkgPackagePrice" runat="server" Text='<%# "$ " + Convert.ToString(decimal.Round(Convert.ToDecimal(Eval("TotalPrice")), 2)) %>'></asp:Label>
                                    </span>
                                    <asp:HiddenField ID="hdnfPrice" runat="server" Value='<%# Eval("TotalPrice") %>' />
                                </div>
                                <div style="clear: both">
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div id="divOrdLineItem" runat="server">
                        <infsu:OLI ID="OLI_OrderConfirmation" runat="server" />
                    </div>
                </div>
                <div id="divPaymentTypes" runat="server">
                    <hr style="border-bottom: solid 1px #c0c0c0;" />
                    <h6 style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px"><%= Resources.Language.PAYMENTTYPES%></h6>
                    <asp:Repeater ID="rptPaymentModes" runat="server" OnItemDataBound="rptPaymentModes_ItemDataBound">
                        <ItemTemplate>
                            <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                                <span><%= Resources.Language.PAYMENTTYPE%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("lkpPaymentOption.Name") %>'></asp:Label>
                                </span>
                                <asp:HiddenField ID="hdfPaymentType" runat="server" Value='<%# Eval("lkpPaymentOption.Code") %>' />
                            </div>
                            <div id="divPrice" runat="server" style="display: inline-block; width: 30%; vertical-align: top; float: left">
                                <span><span id="spnAmt" runat="server"></span><span style="font-weight: bold">
                                    <asp:Label ID="lblGroupPrice" runat="server" Text='<%# "$ " + Convert.ToString(decimal.Round(Convert.ToDecimal(Eval("OPD_Amount")), 2)) %>'></asp:Label>
                                </span></span>
                            </div>
                            <div style="clear: both">
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div id="dvAdditionalPaymentType" runat="server">
                    <hr style="border-bottom: solid 1px #c0c0c0;" />
                    <h6 style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Additional Payment Type(s)</h6>
                    <asp:Repeater ID="rptAdditionalPaymentModes" runat="server" OnItemDataBound="rptAdditionalPaymentModes_ItemDataBound">
                        <ItemTemplate>
                            <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                                <span><%= Resources.Language.PAYMENTTYPE%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("lkpPaymentOption.Name") %>'></asp:Label>
                                </span>
                                <asp:HiddenField ID="hdfAdditionalPaymentType" runat="server" Value='<%# Eval("lkpPaymentOption.Code") %>' />
                            </div>
                            <div id="divAdditionalPrice" runat="server" style="display: inline-block; width: 30%; vertical-align: top; float: left">
                                <span><%= Resources.Language.AMOUNT%>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblGroupPrice" runat="server" Text='<%# "$ " + Convert.ToString(decimal.Round(Convert.ToDecimal(Eval("OPD_Amount")), 2)) %>'></asp:Label>
                                </span></span>
                            </div>
                            <div style="clear: both">
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <div>
                    <hr style="border-bottom: solid 1px #c0c0c0;" />
                    <h6 id="hdrPersonalInfo" runat="server" style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Personal Information</h6>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.FIRSTNAME%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblFirstName" runat="server"></asp:Label>
                            <%--<span>First Name</span>--%>
                        </span>
                    </div>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.MIDDLENAME%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblMiddleName" runat="server"></asp:Label>
                            <%-- <span>Middle Name</span>--%>
                        </span>
                    </div>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.LASTNAME %></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblLastName" runat="server"></asp:Label>
                            <%--<span>Last Name</span>--%>
                        </span>
                    </div>
                    <div style="clear: both">
                    </div>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.GENDER%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblGender" runat="server"></asp:Label>
                            <%--<span>Gender</span>--%>
                        </span>
                    </div>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.DOB %></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblDateOfBirth" runat="server"></asp:Label>
                            <%--<span>Date of Birth</span>--%>
                        </span>
                    </div>
                    <%--UAT-1059:Remove I.P. address and mask social security number from order summary--%>
                    <div id="divSSN" runat="server" style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.SSN%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblSSN" runat="server"></asp:Label>
                            <%--<span>Social Security Number</span>--%>
                        </span>
                    </div>
                    <div style="clear: both">
                    </div>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.EMAILADD%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblEmail" runat="server"></asp:Label>
                            <%--  <span>Email Address</span>--%>
                        </span>
                    </div>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.PHONENUM%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblPhone" runat="server"></asp:Label>
                            <%--   <span>Phone Number</span>--%>
                        </span>
                    </div>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.ADDRESS%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblAddress1" runat="server"></asp:Label>
                            <%--<span>Address</span>--%>
                        </span>
                    </div>
                    <div style="clear: both">
                    </div>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.CITY%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblCity" runat="server"></asp:Label>
                            <%--   <span>City</span>--%>
                        </span>
                    </div>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left" id="dvState" runat="server">
                        <span><%= Resources.Language.STATE%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblState" runat="server"></asp:Label>
                            <%--<span>State</span>--%>
                        </span>
                    </div>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span><%= Resources.Language.COUNTRY%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblCountry" runat="server"></asp:Label>
                            <%--<span>Country</span>--%>
                        </span>
                    </div>
                    <div style="clear: both">
                    </div>
                    <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                        <span>
                            <asp:Label ID="LblNameZIPOPostalCode" runat="server" Text=""></asp:Label>
                        </span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblZip" runat="server"></asp:Label>
                            <%--<span>Zip</span>--%>
                        </span>
                    </div>
                    <div style="clear: both">
                    </div>

                    <div id="dvMailingAddress" runat="server" visible="false">
                        <hr style="border-bottom: solid 1px #c0c0c0;" />
                        <h6 style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px"><%=Resources.Language.MAILINGADDRESS%></h6>
                        <%--<span>Need to be change later</span>--%>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span><%= Resources.Language.MAILINGOPTION%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblMailingOption" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span><%= Resources.Language.ADDRESS%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblMailingAddress" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span><%= Resources.Language.CITY%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblMailingCity" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left" runat="server">
                            <span><%= Resources.Language.COUNTRY%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblMailingCountry" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div id="dvMailingState" runat="server" visible="false" style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span><%= Resources.Language.STATE%></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblMailingState" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span><asp:Label ID="MailingLblNameZIPOPostalCode" runat="server" Text=""></asp:Label></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblMailingZipCode" runat="server"></asp:Label>
                            </span>
                        </div>
                        <%--<span>Need to be change later</span>--%>
                        <div style="clear: both">
                        </div>
                    </div>

                    <div id="dvMoveinDate" runat="server" visible="false">
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Move in Date</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblResidingFrom" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Residing To</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblResidingTo" runat="server"></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div id="dvDriverLicenseNo" runat="server" visible="false">
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Driver License Number</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblDriverLiscence" runat="server"></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div id="dvDriverLicenseState" runat="server" visible="false">
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Driver License State</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblDriverLicenceState" runat="server"></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div style="clear: both">
                    </div>


                    <div runat="server" id="divMothersName" visible="false">
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Mother's Maiden Name</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblMotherName" runat="server"></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div id="divIdentificationNumber" runat="server" visible="false">
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Identification Number</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblIdentificationNumber" runat="server"></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div id="divCriminalLicenseNumber" runat="server" visible="false">
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Driver License Number</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblCriminalLicenseNumber" runat="server"></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div style="clear: both">
                    </div>


                </div>
                <hr style="border-bottom: solid 1px #c0c0c0;" />
                <div id="residentialHistory" runat="server" visible="false">
                </div>
                <div>
                    <infsu:OtherDetails ID="caOtherDetails" runat="server" />
                </div>
                <div id="divHR" runat="server" style="display: none;">
                    <div style="clear: both">
                    </div>
                    <hr style="border-bottom: solid 1px #c0c0c0;" />
                </div>
                <%--panel needed for custom form load--%>
                <asp:Panel ID="pnlLoader" runat="server">
                </asp:Panel>
                <div id="divPaymentInstruction" runat="server" visible="false">
                    <h6 style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Payment Instruction</h6>
                    <asp:Repeater ID="rptInstructions" runat="server">
                        <ItemTemplate>
                            <uc:PaymentInstructions ID="ucPaymentInst" runat="server" InstructionsText='<%# Eval("Item2") %>' PaymentModeText='<%# Eval("Item1") %>' />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="content" runat="server" id="dvUserAgreement" visible="false">
                    <hr style="border-bottom: solid 1px #c0c0c0;" />
                    <div class="sbsection">
                        <h6 style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px"><%= Resources.Language.USERAGRMNT%></h6>
                        <%--<h1 class="sbhdr">User Agreement</h1>--%>
                        <div class="sbcontent">
                            <div class="sxform auto">
                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlUserAgreement">
                                    <div class='sxro sx3co'>
                                        <div class='m4spn'>
                                            <asp:Literal ID="litText" runat="server"></asp:Literal>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divHrDisc" runat="server" style="display: none;">
                <div style="clear: both">
                </div>
                <hr style="border-bottom: solid 1px #c0c0c0;" />
            </div>
            <div id="dvDisclaimer" runat="server" style="display: none;">
                <h6 style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Disclaimer / Limitation of Liability</h6>
                <p>
                    The student or other user of this website (&quot;User&quot;) understands that the vaccination, immunization and other health information received by American DataBank is obtained &quot;AS IS&quot;; accordingly the reports and other data produced by American DataBank are delivered &quot;AS IS&quot;. American DataBank makes no representation or warranty whatsoever, express or implied, regarding the accuracy, validity, or completeness of any data or information products generated by American DataBank, or that such data and information products will be provided on an uninterrupted basis. American DataBank expressly disclaims any and all such representations and warranties.
                </p>
                <p style="font-weight: bold; margin: 20px 0;">
                    User hereby releases American DataBank from any liability for damages arising under any theory of liability to the fullest extent permitted by law and public policy, provided however, that User does not release American DataBank from any liability arising solely from willful misconduct or gross negligence. In the event American DataBank is liable to User for any matter, whether arising in contract, equity or tort, or related to a failure of American DataBank&#39;s online system, the amount of damages recoverable against American DataBank will not exceed the amount paid to American DataBank by User and will not include any amount for indirect or consequential damages, including lost profits or lost income.
                </p>
                <p>
                    User hereby declares that no health information or documents submitted to this website or received from doctors, clinics and other third parties, has been altered, modified, or falsified in any way. User understands that submitting to this website altered, modified, or falsified information or documents may result in the User&#39;s termination or disqualification from all school programs, forfeiture of all tuition and fees and that disclosure of User&#39;s conduct is part of User&#39;s permanent school record.
                </p>
                <p style="margin: 20px 0;">
                    American DataBank does not guarantee User&#39;s compliance with applicable laws regarding User&#39;s use of reported information and does not provide legal or other compliance-related services upon which User may rely. User understands that any conversation or communication with American DataBank&#39;s representatives regarding searches, verifications or other services offered by American DataBank are not to be considered legal advice on which User may rely.
                </p>
                <p>
                    I hereby declare that I have not altered, modified or falsified any part of the health information or documents I submit to this website or that I have received from doctors, clinics and other third parties. I understand that submitting to this website altered, modified or falsified information or documents may result in my termination or disqualification from all school programs, forfeiture of all tuition and fees and disclosure of my conduct as part of my permanent school record.
                </p>
            </div>
            <h1></h1>
            <h1></h1>
            <h1></h1>
            <h1></h1>
            <%-- </h1>--%>
        </div>
        <%--</div>--%>
        <div id="PageTextComplete"></div>
    </asp:Panel>
    <div class="cmdButtonMinSize">
        <infsu:CommandBar ID="cbbuttons" runat="server" ButtonPosition="Center" DisplayButtons="Submit, Extra"
            AutoPostbackButtons="Submit" SubmitButtonText="<%$ Resources:Language, GOTODASHBOARD %>" OnSubmitClick="CmdBarSubmit_Click" DefaultPanel="pnlMain" DefaultPanelButton="Submit"
            ExtraButtonIconClass="rbPrint" ExtraButtonText="<%$ Resources:Language, PRINT %>" OnExtraClientClick="GetHtmlFromDiv">
        </infsu:CommandBar>
    </div>
    <asp:HiddenField runat="server" ID="hdnTenantID" />
    <asp:HiddenField runat="server" ID="hdnFileIdentifier" />
    <asp:HiddenField runat="server" ID="hdnIsCreateHTMLFileOnRender" />

    <br />
    <br />
</div>
<script type="text/javascript">
    $jQuery("div[id$='_pnlRendercustomForm']").attr('style','page-break-inside:avoid');
    $jQuery(document).ready(function () {
        var isCreateHTMLFileOnRender = $jQuery("[id$=hdnIsCreateHTMLFileOnRender]")[0].value;
        if (isCreateHTMLFileOnRender == null || isCreateHTMLFileOnRender == "False") {
            GetHtmlFromDiv(true);
        }
        else {

            CreatePfd();
        }

    });

    function CreatePfd() {
        var tenantID = $jQuery("[id$=hdnTenantID]")[0].value;
        var fileIdentifier = $jQuery("[id$=hdnFileIdentifier]")[0].value;
        var orderNo = $jQuery("[id$=lblOrderId]").text();
        $jQuery("[id$=iframe]").attr('src', "/ComplianceOperations/Pages/DownloadPdf.aspx?fileIdentifier=" + fileIdentifier + "&OrderID=" + orderNo + "&IsSavePdfFile=true" + "&TenantID=" + tenantID);

    }

    function GetHtmlFromDiv(flag) {

        var ordHtml = ($jQuery('#box_content').html());
        //hide print button
        ordHtml = ordHtml.replace('_WclButton1" class', '_WclButton1" style="display:none;" class');
        ordHtml = ordHtml.replace("submit", "hidden");
        var last = ordHtml.indexOf('<div id="PageTextComplete"');
        var header = ordHtml.indexOf("<h1");
        ordHtml = ordHtml.substring(header, last);
        ordHtml = ordHtml.replace("<h6>", "<h6 style='color:#bd6a38 ; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px'>");
        ordHtml = "<meta charset='utf-8' />" + ordHtml.trim(ordHtml);
        var htmlCode = escape(ordHtml);
        var orderNo = $jQuery("[id$=lblOrderId]").text();
        var orderNumber = $jQuery("[id$=lblOrderNumber]").text();
        $jQuery("#hdnPdfHtmlString").val(htmlCode);
        var urltoPost = "/ComplianceOperations/Default.aspx/CreateHtmlFile";
        htmlCode = htmlCode.replace(/\+/g, '%2B');
        var dataString = "strHtmlCode : '" + htmlCode + "',orderNo : '" + orderNo + "',orderNumber : '" + orderNumber + "'";
        $jQuery.ajax
            ({
                type: "POST",
                //async:false,
                url: urltoPost,
                data: "{ " + dataString + " }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var fileIdentifier = data.d;
                    if (flag == true) {
                        var tenantID = $jQuery("[id$=hdnTenantID]")[0].value;
                        $jQuery("[id$=iframe]").attr('src', "/ComplianceOperations/Pages/DownloadPdf.aspx?fileIdentifier=" + fileIdentifier + "&OrderID=" + orderNo + "&IsSavePdfFile=true" + "&TenantID=" + tenantID);
                    }
                    else {
                        $jQuery("[id$=iframe]").attr('src', "/ComplianceOperations/Pages/DownloadPdf.aspx?fileIdentifier=" + fileIdentifier + "&OrderID=" + orderNo);
                    }
                }
            });
    }

</script>
<%--<div style="clear: both" id="divClearRushOrder" runat="server" visible="false">
            </div>--%>