<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemPaymentConfirmation.ascx.cs"
    Inherits="CoreWeb.ComplianceOperations.Views.ItemPaymentConfirmation" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="PackageDetails" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/PackageDetails.ascx" %>
<%--<%@ Register TagPrefix="infsu" TagName="OtherDetails" Src="~/ComplianceAdministration/UserControl/CustomAttributeLoader.ascx" %>--%>
<%@ Register TagPrefix="uc" TagName="PaymentInstructions" Src="~/ComplianceOperations/UserControl/PaymentInstructions.ascx" %>


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
</style>

<div class="section">
    <h2 style="width: 80%; margin: 0 auto;">To complete your order, please click "Finish" below.</h2>
    <asp:Panel ID="pnlMain" runat="server">
        <iframe id="iframe" runat="server" width="100%" height="0px"></iframe>
        <div style="direction: rtl; margin-right: 130px;">
        </div>
        <div runat="server" id="pnlDetail" class="dvPdfViewer" style="width: 80%; margin: 0 auto; margin-top: 20px">
            <h1></h1>
            <asp:Label ID="lblMessage" runat="server" Text="" CssClass="sucs" Visible="false"></asp:Label>
            <h1 style="border-bottom-color: #adadad; border-bottom-style: solid; border-bottom-width: 0px; border-left-color: #adadad; border-left-style: solid; border-left-width: 0px; border-right-color: #adadad; border-left-style: solid; border-bottom-width: 0px; border-top-color: #adadad; border-top-style: solid; border-top-width: 0px; color: #1c4d87; font-family: Helvetia, Arial, sans-serif; font-size: 150%; font-weight: 700; margin-bottom: 10px; margin-left: 0px; margin-right: 0px; margin-top: 10px; padding-bottom: 0px; padding-left: 0px; padding-right: 0px; padding-top: 0px; position: relative; word-spacing: 2px">Order Summary<span style="float: right">
                <infs:WclButton runat="server" ID="WclButton1" Text="Print" OnClientClicked="GetHtmlFromDiv"
                    AutoPostBack="false">
                    <Icon PrimaryIconCssClass="rbPrint" />
                </infs:WclButton>
            </span><span style="clear: both"></span>
                <div style="background-color: #FFF; border: 1px solid #C7C7C8; font-size: 13px; line-height: 150%; padding: 20px; margin-bottom: 20px">
                    <h6 style="color: #1c4d87; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Order Detail</h6>
                    <div>
                        <div style="clear: both">
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Order Number</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblOrderId" runat="server" Style="display: none"></asp:Label>
                                <asp:Label ID="lblOrderNumber" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div id="dvTotalPrice" runat="server" style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Total Price</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblTotalPrice" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="clear: both">
                        </div>
                    </div>
                    <hr style="border-bottom: solid 1px #c0c0c0;" />
                    <div>
                        <h6 style="color: #1c4d87; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Item Detail</h6>
                        <div id="divItemPaymentDetail" runat="server">
                            <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                                <span>Item Name</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblItemName" runat="server"></asp:Label>
                                </span>
                            </div>
                            <div id="divCPT" runat="server" style="display: inline-block; width: 23%; vertical-align: top; float: left">
                                <span>Package Type</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblCompliancePkgPaymentType" runat="server"></asp:Label>
                                </span>
                            </div>
                            <div id="dvPackagePrice" runat="server" style="display: inline-block; width: 20%; vertical-align: top; float: left">
                                <span>Item Price</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblPrice" runat="server"></asp:Label>
                                </span>
                            </div>
                            <div style="clear: both">
                            </div>
                        </div>
                    </div>
                    <div id="divPaymentTypes" runat="server">
                        <hr style="border-bottom: solid 1px #c0c0c0;" />
                        <h6 style="color: #1c4d87; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Payment Type(s)</h6>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Payment Type</span>:&nbsp;<span style="font-weight: bold">
                                <asp:Label ID="lblPaymentMode" runat="server" Text=''></asp:Label>
                            </span>
                            <asp:HiddenField ID="hdfPaymentType" runat="server" Value='' />
                        </div>
                        <div id="divPrice" runat="server" style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Amount:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblGroupPrice" runat="server" Text=''></asp:Label>
                            </span></span>
                        </div>
                        <div style="clear: both">
                        </div>

                    </div>
                    <hr style="border-bottom: solid 1px #c0c0c0;" />
                    <div>
                        <h6 style="color: #1c4d87; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Personal Information</h6>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>First Name</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblFirstName" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Middle Name</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblMiddleName" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Last Name</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblLastName" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="clear: both">
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Gender</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblGender" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Date of Birth</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblDateOfBirth" runat="server"></asp:Label>
                            </span>
                        </div>
                        <%--UAT-1059:Remove I.P. address and mask social security number from order summary--%>
                        <div id="divSSN" runat="server" style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Social Security Number</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblSSN" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="clear: both">
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Email Address</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblEmail" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Phone Number</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblPhone" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Address</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblAddress1" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="clear: both">
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>City</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblCity" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>State</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblState" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Country</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblCountry" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="clear: both">
                        </div>
                        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                            <span>Zip</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblZip" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div style="clear: both">
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
                    <div id="divHR" runat="server" style="display: none;">
                        <div style="clear: both">
                        </div>
                        <hr style="border-bottom: solid 1px #c0c0c0;" />
                    </div>
                    <div id="divPaymentInstruction" runat="server" visible="false">
                        <h6 style="color: #1c4d87; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Payment Instruction</h6>

                        

                        <asp:Repeater ID="rptInstructions" runat="server">
                            <ItemTemplate>
                                <uc:PaymentInstructions ID="ucPaymentInst" runat="server" InstructionsText='<%# Eval("Item2") %>' PaymentModeText='<%# Eval("Item1") %>' />
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="content" runat="server" id="dvUserAgreement">
                        <hr style="border-bottom: solid 1px #c0c0c0;" />
                        <div class="sbsection">
                            <h6 style="color: #1c4d87; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">User Agreement</h6>
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
                    <h6 style="color: #1c4d87; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Disclaimer / Limitation of Liability</h6>
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
            </h1>
        </div>
        <%--</div>--%>
        <div id="PageTextComplete"></div>
    </asp:Panel>
    <infsu:CommandBar ID="cbbuttons" runat="server" ButtonPosition="Center" DisplayButtons="Submit, Extra"
        AutoPostbackButtons="Submit" SubmitButtonText="Finish" OnSubmitClick="CmdBarSubmit_Click" DefaultPanel="pnlMain" DefaultPanelButton="Submit"
        ExtraButtonIconClass="rbPrint" ExtraButtonText="Print" OnExtraClientClick="GetHtmlFromDiv">
    </infsu:CommandBar>
    <asp:HiddenField runat="server" ID="hdnTenantID" />
    <asp:HiddenField runat="server" ID="hdnIsNotificationSent" Value="0" />
    <br />
    <br />
</div>
<script type="text/javascript">

    $jQuery(document).ready(function () {
       // GetHtmlFromDiv(true);
    });

    function GetHtmlFromDiv(flag) {
        var ordHtml = ($jQuery('#box_content').html());
        //hide print button
        ordHtml = ordHtml.replace('_WclButton1" class', '_WclButton1" style="display:none;" class');
        var last = ordHtml.indexOf('<div id="PageTextComplete"');
        var header = ordHtml.indexOf("<h1");
        ordHtml = ordHtml.substring(header, last);
        ordHtml = ordHtml.replace("<h6>", "<h6 style='color:#bd6a38 ; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px'>");
        ordHtml = ordHtml.trim(ordHtml);
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
