<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ApplicantRotationRequirement.Views.ClinicalRotationDefault"
    Title="Default" MasterPageFile="~/Shared/DefaultMaster.master" CodeBehind="Default.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxDash">
        <infs:LinkedResource Path="~/Resources/Mod/Signature/jquery.signaturepad.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <asp:PlaceHolder runat="server" ID="phDynamic"></asp:PlaceHolder>
    <script type="text/javascript">

        function ItemPaymentClick(PkgName, CategoryName, ItemID, CategoryID, ItemName, PkgId, PkgSubscriptionId, OrderID, OrderNumber, TotalPrice, InvoiceNumber, IsRequirement, OrganizationUserProfileID, RefreshItemPanel, IsOrderCreated, clinicalRotationID, tenantId, createdByID) {
            $jQuery("[id$=hdnItemPaymentComplianceCategoryId]").val(CategoryID);
            $jQuery("[id$=hdnItemPaymentComplianceItemId]").val(ItemID);
            $jQuery("[id$=hdnItemPaymentItemName]").val(ItemName);
            $jQuery("[id$=hdnItemPaymentPackageId]").val(PkgId);
            $jQuery("[id$=hdnItemPaymentPackageSubscriptionID]").val(PkgSubscriptionId);
            $jQuery("[id$=hdnItemPaymentPackageName]").val(PkgName);
            $jQuery("[id$=hdnItemPaymentCategoryName]").val(CategoryName);
            $jQuery("[id$=hdnItemPaymentOrderID]").val(OrderID);
            $jQuery("[id$=hdnItemPaymentOrderNumber]").val(OrderNumber);
            $jQuery("[id$=hdnInvoiceNumber]").val(InvoiceNumber);
            $jQuery("[id$=hdnItemPaymentAmount]").val(TotalPrice);
            $jQuery("[id$=hdnItemPaymentIsRequirement]").val(IsRequirement);
            $jQuery("[id$=hdnItemPaymentOrganizationUserProfileID]").val(OrganizationUserProfileID);
            $jQuery("[id$=hdnItemPaymentRefreshItemPanel]").val(RefreshItemPanel);
            $jQuery("[id$=hdnItemPaymentClinicalRotationID]").val(clinicalRotationID);
            $jQuery("[id$=hdnTenantID]").val(tenantId);
            $jQuery("[id$=hdnCreatedByID]").val(createdByID);

            $jQuery("#<%=btnItemPaymentHidden.ClientID %>").trigger('click');
        }

        function ItemPaymentRefreshClick() {
            if ($jQuery("[id$=hdnItemPaymentRefreshItemPanel]").val() == 0) {
                //$jQuery("#<%=btnRefeshPage.ClientID %>").trigger('click');
                var url = window.location.href;
                window.location.reload();
            }
            else
                CompleteItemPaymentClick(true);
        }
        function ItemPaymentClosepopUpWindow() {
            top.$window.get_radManager().getActiveWindow().close();
        }

        function CompleteItemPaymentClick(isReload) {           
            if (isReload) {
                //Update html of item payment form
                //  var getItemPaymentHtml = jQuery("[id$=hdnItemPaymentComplianceCategoryId]").val(CategoryID);
                //  var itemPaymentOrderNumber = "";
                var PkgName = "";
                var CategoryName = "";
                var ItemID = "";
                var CategoryID = "";
                var ItemName = "";
                var PkgId = "";
                var PkgSubscriptionId = "";
                var OrderID = "";
                var OrderNumber = "";
                var TotalPrice = "";
                var InvoiceNumber = "";
                var IsRequirementPackage = 'false';
                var OrganizationUserProfileID = "";
                var IsOrderCreated = "false";
                var CompleteItemPaymentClickHtml = "";
                var IsPaid = "0";
                var OrderStatusCode = "";
                var RefreshItemSectionHtml = "false";
                var ShowSubmitButton = "true";
                //Step 1. Ajax Call
                $.ajax({
                    type: "POST",
                    url: "/ApplicantRotationRequirement/Default.aspx/GetSelectedItemPaymentDetail",
                    data: '',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    cache: false,
                    success: function (response) {                    
                        if (response != null && response.d != null) {
                            var data = response.d;
                            //    alert(typeof (data)); //it comes out to be string 
                            //we need to parse it to JSON 
                            data = $.parseJSON(data);
                            OrderNumber = data.OrderNumber;
                            PkgName = data.PkgName;
                            CategoryName = data.CategoryName;
                            ItemID = data.ItemID;
                            CategoryID = data.CategoryID;
                            ItemName = data.ItemName;
                            PkgId = data.PkgId;
                            PkgSubscriptionId = data.PkgSubscriptionId;
                            OrderID = data.orderID;
                            if (OrderID != "undefined" && OrderID != "" && OrderID != null && OrderID > 0) {
                                jQuery("[id$=hdnIsOrderCreated]").val("1");
                            }
                            if (data.IsPaid == "true" || data.IsPaid == "1") {
                                IsPaid = "1";
                            }
                            if (data.ShowSubmitButton == "true" || data.ShowSubmitButton == "1") {
                                ShowSubmitButton = "true";
                            }
                            else {
                                ShowSubmitButton = "false";
                            }
                            OrderStatusCode = data.OrderStatusCode;
                            OrderNumber = data.OrderNumber;
                            TotalPrice = data.TotalPrice;
                            InvoiceNumber = data.InvoiceNumber;
                            IsRequirementPackage = data.IsRequirementPackage;
                            OrganizationUserProfileID = data.OrganizationUserProfileID;
                            CompleteItemPaymentClickHtml = data.CompleteItemPaymentClickHtml;
                            IsOrderCreated = "true";
                            RefreshItemSectionHtml = data.RefreshItemSectionHtml;
                        }
                        else {
                            //  alert('No Data Found');
                        }
                    },
                    failure: function (response) {
                    }
                });

                //Step 2. Update UI
                if (IsOrderCreated != "false" && OrderNumber != "" && OrderNumber != null && $jQuery("[id$=hdnItemPaymentRefreshItemPanel]").val() == "1") {
                    var dvItemPaymentOrderNumber = jQuery("[id$=dvItemPaymentOrderNumber]");
                    var dvComplteItemPayment = jQuery("[id$=dvComplteItemPayment]");
                    var dvCreateItemPayment = jQuery("[id$=dvCreateItemPayment]");
                    var dvOrderStatus = jQuery("[id$=dvOrderStatus]");
                    var lblItemPaymentOrderNumber = jQuery("[id$=lblItemPaymentDisplayOrderNumber]");
                    var lblOrderStatus = jQuery("[id$=lblOrderStatus]");

                    initializeSignaturePad();

                    if (dvItemPaymentOrderNumber.length > 0) {
                        if (OrderNumber != "") {
                            dvItemPaymentOrderNumber.css("display", "block");
                            dvComplteItemPayment.css("display", "block");
                            dvCreateItemPayment.css("display", "none");
                            if (lblItemPaymentOrderNumber.length > 0) {
                                lblItemPaymentOrderNumber[0].innerHTML = OrderNumber;
                            }
                            if (dvComplteItemPayment.length > 0 && IsPaid == "0") {
                                dvComplteItemPayment[0].innerHTML = CompleteItemPaymentClickHtml;
                                dvOrderStatus.css("display", "block");
                                if (OrderStatusCode == "OSCNL") //i.e. Order Is cancelled
                                {
                                    lblOrderStatus[0].innerHTML = "Cancelled";
                                }
                                else {
                                    lblOrderStatus[0].innerHTML = "Sent For Online Payment";
                                }

                            }
                            else if (IsPaid == "1") {
                                dvOrderStatus.css("display", "block");
                                dvComplteItemPayment.css("display", "none");
                                dvCreateItemPayment.css("display", "none");
                                lblOrderStatus[0].innerHTML = "Paid";
                            }
                        }
                        else {
                            dvItemPaymentOrderNumber.css("display", "none");
                            dvComplteItemPayment.css("display", "none");
                            dvCreateItemPayment.css("display", "block");
                            dvOrderStatus.css("display", "none");
                        }
                    }
                }
            }
        }

        function ItemPaymentRequiredFieldValidation() {
            // Requirement
            var submissionButton = $jQuery('[id$="cmdBar_btnTreeGrd"][type=submit]');
            var SubmissionSaveButtonIcon = $jQuery('[class$="rbSave"]');
            if (submissionButton[0] != undefined && submissionButton.length > 0) {
                submissionButton.add(SubmissionSaveButtonIcon).on("click", function () {
                    if (($jQuery("[id$=dvCreateItemPayment]")[0] != "" && $jQuery("[id$=dvCreateItemPayment]")[0] != undefined) || ($jQuery("[id$=dvComplteItemPayment]")[0] != "" && $jQuery("[id$=dvComplteItemPayment]")[0] != undefined)) {
                        var completePaymentLink = $jQuery("[id$=dvComplteItemPayment]");
                        var itemPaymentLink = $jQuery("[id$=lnkItemPayment]");
                        var itemPaymentError = $jQuery("[id$=spnItemPaymentValidation]");
                        if (itemPaymentError[0] != undefined && itemPaymentError.length > 0) {
                            itemPaymentError[0].innerHTML = "";
                        }
                        if (itemPaymentLink[0] != undefined && itemPaymentLink.length > 0 && itemPaymentLink.is(':visible'))//&& itemPaymentLink[0].css('display') != 'none'
                        {
                            if (itemPaymentError[0] != undefined && itemPaymentError.length > 0) {
                                itemPaymentError[0].innerHTML = "Payment is required for this item.";
                                return false;
                            }
                        }
                        else if (completePaymentLink[0] != undefined && completePaymentLink.length > 0 && completePaymentLink.is(':visible'))//&& completePaymentLink[0].css('display') != 'none'
                        {
                            if (itemPaymentError[0] != undefined && itemPaymentError.length > 0) {
                                itemPaymentError[0].innerHTML = "Payment is required for this item.";
                                return false;
                            }
                        }
                    }
                });
            }

            // Tracking , cmdBar_btnTreeGrd_input
            var TrackingSubmissionButton = $jQuery('[id$="cmdBar_btnTreeGrd_input"][type=submit]');
            var TrackingSubmissionSaveButtonIcon = $jQuery('[class$="rbSave"]');
            if (TrackingSubmissionButton[0] != undefined && TrackingSubmissionButton.length > 0) {
                TrackingSubmissionButton.add(TrackingSubmissionSaveButtonIcon).on("click", function () {
                    if (($jQuery("[id$=dvCreateItemPayment]")[0] != "" && $jQuery("[id$=dvCreateItemPayment]")[0] != undefined) || ($jQuery("[id$=dvComplteItemPayment]")[0] != "" && $jQuery("[id$=dvComplteItemPayment]")[0] != undefined)) {
                        var completePaymentLink = $jQuery("[id$=dvComplteItemPayment]");
                        var itemPaymentLink = $jQuery("[id$=lnkItemPayment]");
                        var itemPaymentError = $jQuery("[id$=spnItemPaymentValidation]");
                        if (itemPaymentError[0] != undefined && itemPaymentError.length > 0) {
                            itemPaymentError[0].innerHTML = "";
                        }
                        if (itemPaymentLink[0] != undefined && itemPaymentLink.length > 0 && itemPaymentLink.is(':visible'))//&& itemPaymentLink[0].css('display') != 'none'
                        {
                            if (itemPaymentError[0] != undefined && itemPaymentError.length > 0) {
                                itemPaymentError[0].innerHTML = "Payment is required for this item.";
                                return false;
                            }
                        }
                        else if (completePaymentLink[0] != undefined && completePaymentLink.length > 0 && completePaymentLink.is(':visible'))//&& completePaymentLink[0].css('display') != 'none'
                        {
                            if (itemPaymentError[0] != undefined && itemPaymentError.length > 0) {
                                itemPaymentError[0].innerHTML = "Payment is required for this item.";
                                return false;
                            }
                        }
                    }
                    //Find Signature Canvas First
                    var validSignature = true;
                    $jQuery('div.sigPad').each(function (index, item) {
                        var signatureLength = $jQuery('[id$=hdnSignatureMinLengh]');
                        //Find the hiddedSignature Value
                        var hdfSignature = $jQuery('input[class="output"][type=hidden]');
                        if (hdfSignature.length > 0) {
                            $jQuery('input[class="output"][type=hidden]').each(function (index, item) {
                                var currenthdfSignature = $(item);
                                if (currenthdfSignature.length > 0 && currenthdfSignature[index].value.length > 0) {
                                    var currentSignatureErrorMessage = currenthdfSignature.parent().parent().find('.checkMinLengthSignature');
                                    if (currentSignatureErrorMessage.length > 0) {
                                        currentSignatureErrorMessage[0].innerHTML = "";
                                        if (signatureLength.length > 0) {
                                            var MinSignLengthValue = parseInt(signatureLength[0].value);
                                            var ActualSignLengthValue = parseInt(currenthdfSignature[index].value.length);
                                            if (MinSignLengthValue > ActualSignLengthValue) {
                                                currentSignatureErrorMessage[0].innerHTML = "Provided text does not qualify as valid Signature. Please provide valid Signature.";
                                                validSignature = false;
                                            }
                                        }

                                    }
                                }
                            });
                        }
                    });
                    if (!validSignature)
                        return false;
                });
            }
        };

        //UAT-3248
        function nextStepPopUp() {

            //$jQuery("[id$=nextStepPopUpDiv]").css("display", "block");
            //$jQuery("[id$=lblMsg]").val(msg);
            //$jQuery("#rwDialogPopup").attr('style', 'color : LightYellow')
            $window.showDialog($jQuery("[id$=nextStepPopUpDiv]").clone().show(), {
                closeBtn: {
                    autoclose: true, text: "Close", click: function () {
                        $jQuery("[id$=lblMsg]")[0].innerText = "";
                    }
                }
            }, 475, 'NEXT STEP! PLEASE READ!');
        }

        //function ManageButton(sender, args) {
        //    debugger;
        //    var languageCode;
        //    var language = sender._text;

        //    if (language != null && language.toLowerCase() == "english") {
        //        languageCode = 'AAAA';
        //        sender.set_text("Spanish");
        //        sender.set_toolTip("Click for Spanish");
        //    }

        //    if (language != null && language.toLowerCase() == "spanish") {
        //        languageCode = 'AAAB';
        //        sender.set_text("English");
        //        sender.set_toolTip("Click for English");
        //    }
        //    var hdnLanguageCode = $jQuery('[id$=hdnLanguageCode]');

        //    if (hdnLanguageCode != null && languageCode != null) {
        //        hdnLanguageCode.val(languageCode);
        //    }
        //}
        function initializeSignaturePad() {
            setTimeout(function () {         
                if ($jQuery("input[type='hidden'][id*='hiddenOutput_']").length > 0) {
                    for (var i = 0; i < $jQuery("input[type='hidden'][id*='hiddenOutput_']").length; i++) {
                        var hdnSign = $jQuery("input[type='hidden'][id*='hiddenOutput_']")[i];
                        var parentDivSigPad = $jQuery(hdnSign).parents('.sigPad');
                        var sig = $jQuery(hdnSign).val();

                        if (sig != undefined && sig != "") {
                            $jQuery(document).ready(function () {
                                $jQuery(parentDivSigPad).signaturePad({ penWidth: 2, drawOnly: true, lineWidth: 0, validateFields: false }).regenerate(sig);
                            });
                        }
                        else {
                            var options = { penWidth: 2, drawOnly: true, lineWidth: 0, validateFields: false }
                            $jQuery(parentDivSigPad).signaturePad(options);
                        }
                    }
                }
            }, 700);
        };
    </script>
    <div style="display: none">
        <asp:Button ID="btnItemPaymentHidden" runat="server" OnClick="btnItemPaymentHidden_Click" />
        <asp:Button ID="btnRefeshPage" runat="server" Text="" OnClick="btnRefeshPage_Click"></asp:Button>
    </div>
    <asp:HiddenField ID="hdnItemPaymentComplianceCategoryId" Value="0" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentComplianceItemId" Value="0" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentPackageId" Value="0" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentItemName" Value="0" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentPackageSubscriptionID" Value="0" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentPackageName" Value="" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentCategoryName" Value="" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentOrderID" Value="0" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentOrderNumber" Value="" runat="server" />
    <asp:HiddenField ID="hdnInvoiceNumber" Value="" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentAmount" Value="" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentIsRequirement" Value="false" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentOrganizationUserProfileID" Value="0" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentRefreshItemPanel" Value="1" runat="server" />
    <asp:HiddenField ID="hdnItemPaymentClinicalRotationID" Value="0" runat="server" />
    <asp:HiddenField ID="hdnShowAlumniProfilePopup" Value="false" runat="server" />
    <asp:HiddenField ID="hdnAlumniPopupRefreshed" Value="false" runat="server" />
    <asp:HiddenField ID="hdnApplicantZipCode" runat="server" />
    <asp:HiddenField ID="hdnTenantID" runat="server" />
    <asp:HiddenField ID="hdnCreatedByID" runat="server" />
</asp:Content>
