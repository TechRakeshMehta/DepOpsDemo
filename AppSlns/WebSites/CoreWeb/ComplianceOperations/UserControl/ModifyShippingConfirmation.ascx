<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModifyShippingConfirmation.ascx.cs" Inherits="CoreWeb.ComplianceOperations.UserControl.ModifyShippingConfirmation" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="OLI" Src="~/ComplianceOperations/UserControl/OrderLineItems.ascx" %>
<%@ Register TagPrefix="infsu" TagName="OtherDetails" Src="~/ComplianceAdministration/UserControl/CustomAttributeLoader.ascx" %>
<%@ Register TagPrefix="uc" TagName="PaymentInstructions" Src="~/ComplianceOperations/UserControl/PaymentInstructions.ascx" %>
<meta charset="utf-8" />
<style type="text/css">
    .sucs {
        display: block;
        padding: 20px;
        padding: 15px 10px 20px 53px;
        border-width: 1px;
        margin: 0px;
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

    <h2 style="width: 80%; margin: 0 auto;"><%= Resources.Language.THANKSMODIFYSHIPPING%></h2>
    <asp:Panel ID="pnlMain" runat="server">
        <iframe id="iframe" runat="server" width="100%" height="0px"></iframe>
        <div style="direction: rtl; margin-right: 130px;">
            <%-- <asp:LinkButton ID="lnkDownloadPdf" OnClientClick="GetHtmlFromDiv()" runat="server">...Save as PDF</asp:LinkButton>--%>
        </div>
        <div runat="server" id="pnl" class="dvPdfViewer" style="width: 80%; margin: 0 auto; margin-top: 20px">
            <h1></h1>
            <asp:Label ID="lblMessage" runat="server" Text="" CssClass="sucs" Visible="false"></asp:Label>

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
                        <infsu:oli id="OLI_OrderConfirmation" runat="server" />
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
                            <span><asp:Label ID="lblNameMailingZipOrPostalCode" runat="server" Text=""></asp:Label></span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblMailingZipCode" runat="server"></asp:Label>
                            </span>
                        </div>
                        <%--<span>Need to be change later</span>--%>
                        <div style="clear: both">
                        </div>
                    </div>

                    <div style="clear: both">
                    </div>


                </div>
                <hr style="border-bottom: solid 1px #c0c0c0;" />

                <%--panel needed for custom form load--%>
                <asp:Panel ID="pnlLoader" runat="server">
                </asp:Panel>
                <div id="divPaymentInstruction" runat="server" visible="false">
                    <h6 style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Payment Instruction</h6>
                    <asp:Repeater ID="rptInstructions" runat="server">
                        <ItemTemplate>
                            <uc:paymentinstructions id="ucPaymentInst" runat="server" instructionstext='<%# Eval("Item2") %>' paymentmodetext='<%# Eval("Item1") %>' />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="content" runat="server" id="dvUserAgreement" visible="false" style="page-break-inside:avoid">
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
    $jQuery(document).ready(function () {
        //debugger;
        $jQuery("div[id$='_pnlRendercustomForm']").attr('style','page-break-inside:avoid !important');
        var isCreateHTMLFileOnRender = $jQuery("[id$=hdnIsCreateHTMLFileOnRender]")[0].value;
        if (isCreateHTMLFileOnRender == null || isCreateHTMLFileOnRender == "False") {
            GetHtmlFromDiv(true);
        }
        else {

            CreatePfd();
        }

    });

    function CreatePfd() {
        //debugger;
        var tenantID = $jQuery("[id$=hdnTenantID]")[0].value;
        var fileIdentifier = $jQuery("[id$=hdnFileIdentifier]")[0].value;
        var orderNo = $jQuery("[id$=lblOrderId]").text();
        $jQuery("[id$=iframe]").attr('src', "/ComplianceOperations/Pages/DownloadPdf.aspx?fileIdentifier=" + fileIdentifier + "&OrderID=" + orderNo + "&IsSavePdfFile=true" + "&TenantID=" + tenantID + "&IsFromModifyShipping=" + true);

    }

    function GetHtmlFromDiv(flag) {
        //debugger;

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
                        $jQuery("[id$=iframe]").attr('src', "/ComplianceOperations/Pages/DownloadPdf.aspx?fileIdentifier=" + fileIdentifier + "&OrderID=" + orderNo + "&IsSavePdfFile=true" + "&TenantID=" + tenantID + "&IsFromModifyShipping=" + true);
                    }
                    else {
                        $jQuery("[id$=iframe]").attr('src', "/ComplianceOperations/Pages/DownloadPdf.aspx?fileIdentifier=" + fileIdentifier + "&OrderID=" + orderNo + "&IsFromModifyShipping=" + true);
                    }
                }
            });
    }

</script>
