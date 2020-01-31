<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.RushOrderConfirmation" CodeBehind="RushOrderConfirmation.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<iframe id="iframe" runat="server" width="100%" height="0px"></iframe>
<div runat="server" id="pnl" style="width: 80%; margin: 0 auto; margin-top: 20px">
    <asp:Panel ID="pnlMain" runat="server">
        <h1 style="border-bottom-color: #adadad; border-bottom-style: solid; border-bottom-width: 0px; border-left-color: #adadad; border-left-style: solid; border-left-width: 0px; border-right-color: #adadad; border-left-style: solid; border-bottom-width: 0px; border-top-color: #adadad; border-top-style: solid; border-top-width: 0px; color: #1c4d87; font-family: Helvetia, Arial, sans-serif; font-size: 150%; font-weight: 700; margin-bottom: 10px; margin-left: 0px; margin-right: 0px; margin-top: 10px; padding-bottom: 0px; padding-left: 0px; padding-right: 0px; padding-top: 0px; position: relative; word-spacing: 2px">Order Summary<span style="float: right">
            <infs:WclButton runat="server" ID="WclButton1" Text="Print" OnClientClicked="GetHtmlFromDiv"
                AutoPostBack="false">
                <Icon PrimaryIconCssClass="rbPrint" />
            </infs:WclButton>
        </span><span style="clear: both"></span>
        </h1>
        <div style="background-color: #fff; border: 1px solid #C7C7C8; font-size: 13px; line-height: 150%; margin-left: 10px; padding: 20px; margin-bottom: 20px">
            <h6 style="color: #1c4d87; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Subscription Detail
            </h6>
            <div>
                <div style="display: inline-block; width: 100%; vertical-align: top; float: left">
                    <span>Institution Hierarchy</span>:&nbsp;<span style="font-weight: bold"><asp:Label
                        ID="lblInstitutionHierarchy" runat="server"></asp:Label></span>
                </div>
                <div style="clear: both">
                </div>
                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>Order Number</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblOrderId"
                        runat="server"></asp:Label></span>
                </div>
                <%--UAT-1059:Remove I.P. address and mask social security number from order summary--%>
                <%--  <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>Applicant Machine IP</span>:&nbsp;<span style="font-weight: bold"><asp:Label
                        ID="lblIPAddress" runat="server"></asp:Label></span>
                </div>--%>
                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>Package</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblPackage"
                        runat="server"></asp:Label></span>
                </div>
                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>Subscription Period (Months)</span>:&nbsp;<span style="font-weight: bold"><asp:Label
                        ID="lblSubscription" runat="server"></asp:Label></span>
                </div>
                <div style="clear: both">
                </div>

                <div style="display: inline-block; width: 30%; vertical-align: top; float: left"
                    runat="server">
                    <span>Rush Order Price</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblRushOrderPrice"
                        runat="server"></asp:Label></span>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <hr style="border-bottom: solid 1px #c0c0c0;" />
            <div>
                <h6 style="color: #1c4d87; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Personal Information</h6>
                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>First Name</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblFirstName"
                        runat="server"></asp:Label></span>
                </div>
                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>Last Name</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblLastName"
                        runat="server"></asp:Label></span>
                </div>
                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>Date of Birth</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblDateOfBirth"
                        runat="server"></asp:Label></span>
                </div>
                <div style="clear: both">
                </div>
                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>Email Address</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblEmail"
                        runat="server"></asp:Label></span>
                </div>
                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>Phone Number</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblPhone"
                        runat="server"></asp:Label></span>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div id="divPaymentInstruction" runat="server" visible="false">
                <hr style="border-bottom: solid 1px #c0c0c0;" />
                <h6 style="color: #1c4d87; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px"><%--Subscription--%>Payment Instruction</h6>
                <div id="divPayment">
                    <p style="font-weight: bold;">
                        <asp:Literal ID="litPaymentInstruction"
                            runat="server"></asp:Literal>
                    </p>
                </div>
            </div>
        </div>
        <infsu:CommandBar ID="cbbuttons" runat="server" ButtonPosition="Center" DisplayButtons="Submit, Extra" DefaultPanel="pnlMain" DefaultPanelButton="Submit"
            AutoPostbackButtons="Submit" SubmitButtonText="Finish" OnSubmitClick="CmdBarSubmit_Click"
            ExtraButtonIconClass="rbPrint" ExtraButtonText="Print" OnExtraClientClick="GetHtmlFromDiv">
        </infsu:CommandBar>
        <br />
        <br />
    </asp:Panel>
</div>
<script type="text/javascript">
    function GetHtmlFromDiv() {

        var ordHtml = ($jQuery('#box_content').html());
        var last = ordHtml.indexOf('<div class="sxcmds">');
        var header = ordHtml.indexOf("<h1");
        //hide print button
        ordHtml = ordHtml.replace('_WclButton1" class', '_WclButton1" style="display:none;" class');
        ordHtml = ordHtml.substring(header, last);
        ordHtml = ordHtml.trim(ordHtml);
        var htmlCode = escape(ordHtml);
        var orderNo = $jQuery("[id$=lblOrderId]").text();
        var urltoPost = "/ComplianceOperations/Default.aspx/CreateHtmlFile";
        var dataString = "strHtmlCode : '" + htmlCode + "',orderNo : '" + orderNo + "'";

        $jQuery.ajax
        (
        {
            type: "POST",
            url: urltoPost,
            data: "{ " + dataString + " }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var fileIdentifier = data.d;
                $jQuery("[id$=iframe]").attr('src', "/ComplianceOperations/Pages/DownloadPdf.aspx?fileIdentifier=" + fileIdentifier);
            }
        }
       );
    }

</script>
