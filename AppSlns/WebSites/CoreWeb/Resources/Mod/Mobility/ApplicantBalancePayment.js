function SubmitBalancePayment() {
    var selectedPaymentTypeId = $find($jQuery("[id$=cmbPaymentType]")[0].id).get_value();
    var isBillingInfoSameAsAccountInfo = false;  // GetBillingInfoStatus();
    Page.showProgress("Processing...");
    var url = window.location.host;
    PageMethods.SubmitBalancePayment(selectedPaymentTypeId, isBillingInfoSameAsAccountInfo, url, callBack);
}

//function callBack(result) {
//    window.parent.location = "http://" + window.location.host + "/" + result;
//}

//Order call back
function callBack(result) {
    if (result != null && result != undefined) {
        result = $jQuery.parseJSON(result)
        //if internal request i.e. payment through Invoice or Money Order, set current window location
        //else external request i.e. payment through Credit card or Paypal, set window parent location 
        if (result.redirectUrlType == "internal")
            //window.setTimeout(function () { window.location = ("http://" + window.location.host + "/" + result.redirectUrl); }, 1000);
            //Fixed:[13/09/2016] Security issue on change subscription balance payment regarding UAT-2308
            window.setTimeout(function () { window.location = (window.location.protocol + "//" + window.location.host + "/" + result.redirectUrl); }, 1000);

        else
            //window.setTimeout(function () { window.parent.location = ("http://" + window.location.host + "/" + result.redirectUrl); }, 1000);
            //Fixed:[13/09/2016] Security issue on change subscription balance payment regarding UAT-2308
            window.setTimeout(function () { window.parent.location = (window.location.protocol + "//" + window.location.host + "/" + result.redirectUrl); }, 1000);
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


$jQuery(document).ready(function () {
    //UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred browser.
    ShowNonPreferredBrowserPopUp();
});

//UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred browser.
function ShowNonPreferredBrowserPopUp() {
    var hdnIsPageRefereshed = $jQuery("[id$=hdnIsPageRefereshed]").val();
    var hdnShowApplicantNonPrefferedBrowserPopup = $jQuery("[id$=hdnShowApplicantNonPrefferedBrowserPopup]").val();
    //1 means browser is not complio preferred browser and user comes to dashboard after login and we have show the non-preferred browser to the user
    if (hdnIsPageRefereshed == '1' && hdnShowApplicantNonPrefferedBrowserPopup.toLowerCase() != 'true') {
        //In this method we are inserting the log into database if user click on dismiss button
        CheckNonPrefferedBrowserPopup();
    }
}

//In this method we are showing modal popup to user that he/she using the non-preferred browser.
//If he click on continue the simply pop-up will close if he clicks on dismiss then we are inserting log into database which indicates that user dont wannna see this pop-up again in the future
function CheckNonPrefferedBrowserPopup() {
    var hdnShowApplicantNonPrefferedBrowserPopup = $jQuery("[id$=hdnShowApplicantNonPrefferedBrowserPopup]").val();
    if (hdnShowApplicantNonPrefferedBrowserPopup.toLowerCase() != 'true') {
        $jQuery(".nonPrefferedBrowserPopup").css("display", "block");
        var dialog = $window.showDialog($jQuery(".nonPrefferedBrowserPopup").clone().show(), {
            approvebtn: {
                autoclose: true, text: "Dismiss", click: function () {
                    $.ajax({
                        type: "POST",
                        url: "Default.aspx/CheckUserNonPrefferedBrowserOption",
                        data: '',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            // alert(response.d);
                           // DashboardPopups();
                        },
                        failure: function (response) {
                            // alert(response.d);
                            
                        }
                    });
                }
            }, closeBtn: {
                autoclose: true, text: "Continue", click: function () {
                    
                }

            }
        }, 475, 'Complio');
        var dialogId = dialog.get_id();
        parent.$jQuery("[id *= '" + dialogId + "'] a.rwCloseButton").hide();
        $jQuery(".nonPrefferedBrowserPopup").css("display", "none");

    }
}
//END UAT-2302 
