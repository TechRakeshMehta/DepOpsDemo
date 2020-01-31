
$jQuery(document).ready(function () {
    var chkRushOrder = $jQuery("input[type=checkbox][id$=chkRushOrder]");
    if (chkRushOrder.length > 0) {
        chkRushOrder.addClass("sxRushOrder");
    }
});
//comment to check file updation
//To submit Order
function submitOrder(sender, e) {
    var isCreditCardSelected = false;
    var data = [];
    var rpt = $jQuery("#divMain").find(".divPkg");
    //UAT-4057 - START
    var pnlPaymentTypeCommon = $jQuery("#divMain").find("div.pnlPaymentTypeCommon");
    var cmbPaymentModesCommon = $jQuery("#divMain").find("[id$=cmbPaymentModesCommon]");
    var cmbPaymentModesCommonVal = '';
    if (cmbPaymentModesCommon != undefined && cmbPaymentModesCommon.length > 0)
        cmbPaymentModesCommonVal = $find($jQuery("#divMain").find(cmbPaymentModesCommon)[0].id).get_value();
    var _allSelected = true;
    if (cmbPaymentModesCommonVal == "")
        _allSelected = false;
    var _allVisible = true;
    var cmbPaymentModesCommon = $jQuery("#divMain").find("[id$=cmbPaymentModesCommon]");
    if (cmbPaymentModesCommon != undefined && !cmbPaymentModesCommon.is(':visible'))
        _allVisible = false;
    var hdnIsCommonPaymentSelection = $jQuery("[id$=hdnIsCommonPaymentSelection]");
    //UAT-4057 - END

    if (pnlPaymentTypeCommon != undefined && pnlPaymentTypeCommon.is(':visible')) // All if condition is for UAT-4057
    {
        if (_allVisible && !_allSelected && ($jQuery("[id$=txtTotalPrice]")[0] == undefined || ($jQuery("[id$=txtTotalPrice]")[0] != undefined && $jQuery("[id$=txtTotalPrice]")[0].value != "$0.00"))) {
            if (Page_Validators != undefined && Page_Validators != null) {
                var i;
                for (i = 0; i < Page_Validators.length; i++) {
                    var val = Page_Validators[i];
                    if (!val.isvalid) {
                        return
                    }
                }
            }
        }
        else {
            rpt.each(function () {
                //debugger;                            
                var _poid = 0;
                var _addionalPoid = 0;
                var isZP = false;

                //UAT-3850
                var _cmbPaymentModeBalanceAmt = $jQuery(this).find("[id$=cmbPaymentModeBalanceAmt]");
                var _isPaymentByInstAloneCase = false;
                var IsPaymentByInst = false;
                var hdnIsPaymentByInst = $jQuery("[id$=hdnIsPaymentByInst]");
                if (hdnIsPaymentByInst != null && hdnIsPaymentByInst != undefined) {
                    IsPaymentByInst = hdnIsPaymentByInst[0].value;
                }

                if (IsPaymentByInst != null && IsPaymentByInst != undefined && IsPaymentByInst == "true") {

                    _poid = $jQuery("[id$=hdnPaymentByInstID]")[0].value;
                    data.push({
                        'pkgid': $jQuery(this).find("input[id*=hdfPkgId]").val(),
                        'poid': _poid,
                        'isbkg': $jQuery(this).find("input[id*=hdfIsBkgPkg]").val(),
                        'isZP': isZP,
                        'additionalPoid': _addionalPoid,
                        'isAnyOptionsApprovalReq': $jQuery(this).find("input[id*=hdnIsPaymentApprovalReq]").val()
                    });

                    _isPaymentByInstAloneCase = true;

                    if (_cmbPaymentModeBalanceAmt != undefined && _cmbPaymentModeBalanceAmt.is(":visible")) {
                        _poid = $find(_cmbPaymentModeBalanceAmt[0].id).get_value();
                        _isPaymentByInstAloneCase = false;
                        if (isCreditCardSelected == false && $jQuery("[id$=hdnCredtPymntOptnId]")[0] != undefined && _poid == $jQuery("[id$=hdnCredtPymntOptnId]")[0].value)
                            isCreditCardSelected = true;
                    }

                    //UAT-5029
                    if (cmbPaymentModesCommon != undefined && cmbPaymentModesCommon.is(':visible')) {
                        _poid = $find($jQuery("#divMain").find("[id$=cmbPaymentModesCommon]")[0].id).get_value();
                        _isPaymentByInstAloneCase = false;
                        if (isCreditCardSelected == false && $jQuery("[id$=hdnCredtPymntOptnId]")[0] != undefined && _poid == $jQuery("[id$=hdnCredtPymntOptnId]")[0].value)
                            isCreditCardSelected = true;
                    }

                }
                    //End
                else {                    
                    if (cmbPaymentModesCommon != undefined && cmbPaymentModesCommon.is(':visible')) {
                        _poid = $find($jQuery("#divMain").find("[id$=cmbPaymentModesCommon]")[0].id).get_value();
                        if (isCreditCardSelected == false && $jQuery("[id$=hdnCredtPymntOptnId]")[0] != undefined && _poid == $jQuery("[id$=hdnCredtPymntOptnId]")[0].value)
                            isCreditCardSelected = true;
                    }
                    else {
                        isZP = true;
                        _poid = $jQuery("[id$=hdnCredtPymntOptnId]")[0].value;
                    }
                }

                //UAT-3268
                //debugger;
                var _dvAdditionalPrice = $jQuery(this).find("[id$=dvAdditionalPrice]");
                var _dvPaymentType = $jQuery(this).find("[id$=dvPaymentType]");
                if (_dvAdditionalPrice != undefined && _dvPaymentType != undefined) {
                    _addionalPoid = $jQuery(this).find("input[id*=hdnAdditionalPaymentOptionID]").val();
                }

                if (_isPaymentByInstAloneCase != true) {
                    data.push({
                        'pkgid': $jQuery(this).find("input[id*=hdfPkgId]").val(),
                        'poid': _poid,
                        'isbkg': $jQuery(this).find("input[id*=hdfIsBkgPkg]").val(),
                        'isZP': isZP,
                        'additionalPoid': _addionalPoid,
                        'isAnyOptionsApprovalReq': $jQuery(this).find("input[id*=hdnIsPaymentApprovalReq]").val()

                    });
                }

            });

            var selectedPaymentModeData = JSON.stringify(data);

            var acceptanceValidationClear = false;
            if (isCreditCardSelected)
                acceptanceValidationClear = CheckIfUserAgreementAccepted(sender, acceptanceValidationClear);
            else {
                acceptanceValidationClear = true;
            }
            if (acceptanceValidationClear) {                
                //Start UAT-3958
                var isAnyOptionsApprovalReq = $jQuery("[id$=hdnIsAnyOptionsApprovalReq]")[0].value;
                var isLocationTenant = $jQuery("[id$=hdnIsLocationServiceTenant]")[0].value;
                if (isAnyOptionsApprovalReq != undefined && isAnyOptionsApprovalReq != null && isLocationTenant != undefined && isLocationTenant != null
                    && (isAnyOptionsApprovalReq == "true" || isAnyOptionsApprovalReq == "True") && (isLocationTenant != "true" && isLocationTenant != "True")) {
                    ShowPaymentMethodRequiredApprovalConfirmationMessage(selectedPaymentModeData, callBackForSubmitOrder);
                }
                else {
                    ContinueOrder(selectedPaymentModeData, callBackForSubmitOrder)
                }
                //ENd UAT-3958
            }
        }
    }
    else
    {
        // UAT 916 
        //var selectedPaymentModeId = $find($jQuery("[id$=cmbPaymentModes]")[0].id).get_value();
        //if ($jQuery("[id$=cmbPaymentModes]").is(':visible') == true && selectedPaymentModeId == "" && ($jQuery("[id$=txtTotalPrice]")[0] == undefined || ($jQuery("[id$=txtTotalPrice]")[0] != undefined && $jQuery("[id$=txtTotalPrice]")[0].value != "$0.00"))) {
        if (allVisible(rpt) && !allSelected(rpt) && ($jQuery("[id$=txtTotalPrice]")[0] == undefined || ($jQuery("[id$=txtTotalPrice]")[0] != undefined && $jQuery("[id$=txtTotalPrice]")[0].value != "$0.00"))) {
            if (Page_Validators != undefined && Page_Validators != null) {
                var i;
                for (i = 0; i < Page_Validators.length; i++) {
                    var val = Page_Validators[i];
                    if (!val.isvalid) {
                        return
                    }
                }
            }
        }
        else {
            rpt.each(function () {
                //debugger;            
                var _cmb = $jQuery(this).find("[id$=cmbPaymentModes]");
                var _poid = 0;
                var _addionalPoid = 0;
                var isZP = false;

                //UAT-3850
                var _cmbPaymentModeBalanceAmt = $jQuery(this).find("[id$=cmbPaymentModeBalanceAmt]");
                var _isPaymentByInstAloneCase = false;
                var IsPaymentByInst = false;
                var hdnIsPaymentByInst = $jQuery("[id$=hdnIsPaymentByInst]");
                if (hdnIsPaymentByInst != null && hdnIsPaymentByInst != undefined) {
                    IsPaymentByInst = hdnIsPaymentByInst[0].value;
                }

                if (IsPaymentByInst != null && IsPaymentByInst != undefined && IsPaymentByInst == "true") {

                    _poid = $jQuery("[id$=hdnPaymentByInstID]")[0].value;
                    data.push({
                        'pkgid': $jQuery(this).find("input[id*=hdfPkgId]").val(),
                        'poid': _poid,
                        'isbkg': $jQuery(this).find("input[id*=hdfIsBkgPkg]").val(),
                        'isZP': isZP,
                        'additionalPoid': _addionalPoid,
                        'isAnyOptionsApprovalReq': $jQuery(this).find("input[id*=hdnIsPaymentApprovalReq]").val()
                    });

                    _isPaymentByInstAloneCase = true;

                    if (_cmbPaymentModeBalanceAmt != undefined && _cmbPaymentModeBalanceAmt.is(":visible")) {
                        _poid = $find(_cmbPaymentModeBalanceAmt[0].id).get_value();
                        _isPaymentByInstAloneCase = false;
                        if (isCreditCardSelected == false && $jQuery("[id$=hdnCredtPymntOptnId]")[0] != undefined && _poid == $jQuery("[id$=hdnCredtPymntOptnId]")[0].value)
                            isCreditCardSelected = true;
                    }
                }
                    //End
                else {
                    if (_cmb != undefined && _cmb.is(":visible")) {
                        _poid = $find(_cmb[0].id).get_value();
                        if (isCreditCardSelected == false && $jQuery("[id$=hdnCredtPymntOptnId]")[0] != undefined && _poid == $jQuery("[id$=hdnCredtPymntOptnId]")[0].value)
                            isCreditCardSelected = true;
                    }
                    else {
                        isZP = true;
                        _poid = $jQuery("[id$=hdnCredtPymntOptnId]")[0].value;
                    }
                }

                //UAT-3268
                //debugger;
                var _dvAdditionalPrice = $jQuery(this).find("[id$=dvAdditionalPrice]");
                var _dvPaymentType = $jQuery(this).find("[id$=dvPaymentType]");
                if (_dvAdditionalPrice != undefined && _dvPaymentType != undefined) {
                    _addionalPoid = $jQuery(this).find("input[id*=hdnAdditionalPaymentOptionID]").val();
                }

                if (_isPaymentByInstAloneCase != true) {
                    data.push({
                        'pkgid': $jQuery(this).find("input[id*=hdfPkgId]").val(),
                        'poid': _poid,
                        'isbkg': $jQuery(this).find("input[id*=hdfIsBkgPkg]").val(),
                        'isZP': isZP,
                        'additionalPoid': _addionalPoid,
                        'isAnyOptionsApprovalReq': $jQuery(this).find("input[id*=hdnIsPaymentApprovalReq]").val()
                    });
                }

            });

            var selectedPaymentModeData = JSON.stringify(data);

            var acceptanceValidationClear = false;
            if (isCreditCardSelected)
                acceptanceValidationClear = CheckIfUserAgreementAccepted(sender, acceptanceValidationClear);
            else {
                acceptanceValidationClear = true;
            }
            if (acceptanceValidationClear) {
                //  Page.showProgress("Processing...");
                //  var isBillingInfoSameAsAccountInfo = false;  //GetBillingInfoStatus();
                //  var url = window.location.host;

                //PageMethods.SubmitOrder(selectedPaymentModeData, isBillingInfoSameAsAccountInfo, url, callBackForSubmitOrder);

                //Above commented code is shifted in ContinueOrder() method.
                //Start UAT-3958
                var isAnyOptionsApprovalReq = $jQuery("[id$=hdnIsAnyOptionsApprovalReq]")[0].value;
                var isLocationTenant = $jQuery("[id$=hdnIsLocationServiceTenant]")[0].value;
                if (isAnyOptionsApprovalReq != undefined && isAnyOptionsApprovalReq != null && isLocationTenant != undefined && isLocationTenant != null
                    && (isAnyOptionsApprovalReq == "true" || isAnyOptionsApprovalReq == "True") && (isLocationTenant != "true" && isLocationTenant != "True")) {
                    ShowPaymentMethodRequiredApprovalConfirmationMessage(selectedPaymentModeData, callBackForSubmitOrder);
                }
                else {
                    ContinueOrder(selectedPaymentModeData, callBackForSubmitOrder)
                }
                //ENd UAT-3958
            }
        }
    }
}

function ContinueOrder(selectedPaymentModeData, callBackForSubmitOrder) {
    //debugger;
    Page.showProgress("Processing...");
    var isBillingInfoSameAsAccountInfo = false;  //GetBillingInfoStatus();
    var url = window.location.host;
    PageMethods.SubmitOrder(selectedPaymentModeData, isBillingInfoSameAsAccountInfo, url, callBackForSubmitOrder);
}


//To submit Order when Payment Type changed
//Control goes to SubmitOrderPayTypeChanged() method in Default page
function SubmitOrderPayTypeChanged(paymentModeId, opdid) {
    //var orderId = $jQuery("[id$=txtOrderId]").val();
    var orderId = $jQuery("[id$=hdnOrderID]").val();
    var selectedPaymentModeId = paymentModeId; // TO DO INTERGATION
    var ordPaymentDetailId = opdid; // TO DO INTERGATION

    var url = window.location.host;
    PageMethods.SubmitOrderPayTypeChanged(orderId, selectedPaymentModeId, ordPaymentDetailId, url, callBack);
}

function allSelected(rpt) {
    var _allSelected = true;
    rpt.each(function () {
        var _val = $find($jQuery(this).find("[id$=cmbPaymentModes]")[0].id).get_value();
        if (_val == "")
            return _allSelected = false;
    });

    return _allSelected;
}

function allVisible(rpt) {
    var _allVisible = true;
    rpt.each(function () {
        var _cmb = $jQuery(this).find("[id$=cmbPaymentModes]");
        if (_cmb != undefined && !_cmb.is(':visible'))
            return _allVisible = false;
    });

    return _allVisible;
}



//Order call back
function callBack(result) {
    if (result != null && result != undefined) {
        result = $jQuery.parseJSON(result)
        //if internal request i.e. payment through Invoice or Money Order, set current window location
        //else external request i.e. payment through Credit card or Paypal, set window parent location 
        if (result.redirectUrlType == "internal")
            window.setTimeout(function () { window.location = result.redirectUrl; }, 1000);
        else
            window.setTimeout(function () { window.parent.location = result.redirectUrl; }, 1000);
    }
}

//Order call back
function callBackForSubmitOrder(result) {
    if (result != null && result != undefined && result != '') {
        result = $jQuery.parseJSON(result)
        //if internal request i.e. payment through Invoice or Money Order, set current window location
        //else external request i.e. payment through Credit card or Paypal, set window parent location 
        if (result.redirectUrlType == "internal")
            window.setTimeout(function () { window.location = result.redirectUrl; }, 1000);
        else
            window.setTimeout(function () { window.parent.location = result.redirectUrl; }, 1000);
    }
    else
        Page.hideProgress();
}

//To submit Rush Order
//Control goes to SubmitRushOrder() method in Default page
function SubmitRushOrder() {
    var orderId = $jQuery("[id$=lblOrderNumber]").text();
    var deptPrgPackageSubscriptionId = $jQuery("[id$=hdnDeptPrgPackageSubscriptionId]").val();
    var selectedPaymentModeId = $find($jQuery("[id$=cmbPaymentOptions]")[0].id).get_value();
    var isBillingInfoSameAsAccountInfo = false;  //GetBillingInfoStatus();
    var url = window.location.host;
    PageMethods.SubmitRushOrder(orderId, deptPrgPackageSubscriptionId, selectedPaymentModeId, isBillingInfoSameAsAccountInfo, url, callBack);
}

//Rush Order Call Back function
function RushOrderCallBack(result) {
    window.parent.location = result;
    //window.location = result;
}

function ShowHidePaymentType(isVisible) {
    if (isVisible) {
        $jQuery("#dvPaymentTypelb").show();
        $jQuery("#dvPaymentTypelm").show();
    }
    else {
        $jQuery("#dvPaymentTypelb").hide();
        $jQuery("#dvPaymentTypelm").hide();
    }
}

function DisableValidators() {
    DisableValidator($jQuery("[id$=rfvPaymentModes]")[0].id);
}
//Code:: Validator Disabled ::
function DisableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], false);
    }
}


function ConfirmSubmit(sender, eventArgs) {
    // 3804 Some change in this funcation as per requirement.
    //
    // debugger;
    var hdnLanguageCode = $jQuery("[id$=hdnLanguageCode]");
    var acceptancePrivacyValidationClear = false;
    var lstCheckBox = $jQuery("[id$=chkAcceptPrivacy]");
    if (lstCheckBox != undefined && lstCheckBox.length > 0) {
        if (lstCheckBox.prop('checked') != true) {
            var lblValidationMsg = $jQuery(".lblValidationMsg");
            if (hdnLanguageCode != null && hdnLanguageCode != undefined) {
                var LanguageCode = hdnLanguageCode.val();
                if (LanguageCode != null && LanguageCode != undefined) {
                    var valdnMsg = $jQuery("[id$=hdnPlsReadVldanMsg]").val();
                    $jQuery(".lblValidationMsg").text(valdnMsg);
                }
            }
            else {
                $jQuery(".lblValidationMsg").text("Please read and accept the Privacy Act Statement");
            }
            acceptancePrivacyValidationClear = false;
        }
        else
            acceptancePrivacyValidationClear = true;
    }
    else {
        acceptancePrivacyValidationClear = true;
    }
    if (acceptancePrivacyValidationClear) {
        var lblValidationMsg = $jQuery(".lblValidationMsg");
        lblValidationMsg.text("");
    }
    if (acceptancePrivacyValidationClear == true) {
        var cnrfmMsg = 'You will not be able to edit any information for this order after proceeding. Have you reviewed all your information?' +"\n\n"+
            "Refund Policy:" + "\n" +
            "You have agreed to a non-tangible service, and as a customer you agree to the terms and conditions of service. In addition, you have acknowledged that there are no refunds that can be issued.";
        if ($jQuery("[id$=hdnIsLocTen]").val() == "True") {
            var cnfrmationMsg = $jQuery("[id$=hdnConfirmMsg]").val();
            //cnrfmMsg = 'You will not be able to edit any information for this order after proceeding – MAKE SURE YOUR INFORMATION IS CORRECT.Have you reviewed all your information?';
            cnrfmMsg = cnfrmationMsg;  //spanish translation task
        }
        if (confirm(cnrfmMsg)) {
            sender.set_autoPostBack(true);
        } else {
            sender.set_autoPostBack(false);
        }
    }
    else {
        sender.set_autoPostBack(false);
    }


}

//Function to Check whether Billing Info same as account info or not
function GetBillingInfoStatus() {
    //var isBillingInfoSameAsAccountInfo = true;
    if ($jQuery("[id$=chkBillingAddress]").length > 0) {
        return $jQuery("[id$=chkBillingAddress] > span:first").hasClass('rbToggleCheckboxChecked');
    }
    return false;
}

function CheckIfUserAgreementAccepted(sender, acceptanceValidationClear) {
    //sender.set_autoPostBack(false);
    //debugger;
    var lstCheckBox = $jQuery("[id$=chkAccept]");

    if (lstCheckBox != undefined && lstCheckBox.length > 0) {
        if (lstCheckBox.prop('checked') != true) {
            var lblValidationMsg = $jQuery(".lblValidationMsg");
            // $jQuery(".lblValidationMsg").text("Please read and accept the User Agreement");
            var hdnValidationMsg = $jQuery("[id$=hdnValidationMsg]").val();
            $jQuery(".lblValidationMsg").text(hdnValidationMsg);
            acceptanceValidationClear = false;
            return acceptanceValidationClear;
        }
        acceptanceValidationClear = true;
    }
    else {
        acceptanceValidationClear = true;
    }

    if (acceptanceValidationClear) {
        var lblValidationMsg = $jQuery(".lblValidationMsg");
        lblValidationMsg.text("");
        //sender.set_autoPostBack(true);
    }
    return acceptanceValidationClear;
}

var message = "Are you sure you wish to mark this order graduated? Clicking " + '"Yes"' + " below will remove your institution's ability to view all information pertaining to this order. Orders should only be marked graduated once you are no longer a member of the program for which the order was placed.";

function ConfirmationMessageComp(sender, args) {
    //debugger;

    var rdbTextMessageID = sender.id;
    var selectedValue = $jQuery("[id$=" + rdbTextMessageID + "]").find('input:radio:checked')[0].value;
    //sender.set_autoPostBack(false);
    if (selectedValue == "True") {

        //sender.set_selectedToggleStateIndex(0);
        $confirm(message, function (cr) {
            if (cr) {
                //sender.set_selectedToggleStateIndex(1);
                __doPostBack(rdbTextMessageID);
            }
            else {
                //debugger;
                $jQuery("[id$=" + rdbTextMessageID + "]").find('input:radio')[0].checked = false;
                $jQuery("[id$=" + rdbTextMessageID + "]").find('input:radio')[1].checked = true;
            }
        }, 'Complio', true);
    }
    else {
        //sender.set_autoPostBack(true);
        __doPostBack(rdbTextMessageID);
    }
}

function ConfirmationMessageBkg(sender, args) {
    //debugger;
    var rdbTextMessageID = sender.id;
    var selectedValue = $jQuery("[id$=" + rdbTextMessageID + "]").find('input:radio:checked')[0].value;
    //sender.set_autoPostBack(false);
    if (selectedValue == "True") {
        //  sender.set_selectedToggleStateIndex(0);
        $confirm(message, function (cr) {
            if (cr) {
                // sender.set_selectedToggleStateIndex(1);
                __doPostBack(rdbTextMessageID);
            }
            else {
                $jQuery("[id$=" + rdbTextMessageID + "]").find('input:radio')[0].checked = false;
                $jQuery("[id$=" + rdbTextMessageID + "]").find('input:radio')[1].checked = true;
            }
        }, 'Complio', true);
    }
    else {
        //sender.set_autoPostBack(true);
        __doPostBack(rdbTextMessageID);
    }
}


//UAT-3958//
function ShowPaymentMethodRequiredApprovalConfirmationMessage(selectedPaymentModeData, callBackForSubmitOrder) {
    //debugger;
    var dialog = $window.showDialog($jQuery("[id$=ApprovalMsgPopup]").clone().show(), {
        approvebtn: {
            autoclose: true, text: "Ok", click: function () {
                //debugger;
                ContinueOrder(selectedPaymentModeData, callBackForSubmitOrder);
            }
        }
    }, 550, 'Information');
}
//ENd UAT-3958
