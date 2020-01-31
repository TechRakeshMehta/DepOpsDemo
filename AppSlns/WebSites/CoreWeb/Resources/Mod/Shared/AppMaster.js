//----------------------------------------------------------
// Copyright (C) Copyright Intersoft Data Labs Inc. All rights reserved.
//----------------------------------------------------------
// File: AppMaster.js

/// <reference path="/Resources/Generic/ref.js"/>
var lblTimeLeft;
var timeoutId;
var lock = true;

var pdfDocViewerChildWnd = null;
var wnd_load_in = 0;

function StartCountDown(seconds) {

    if (lock) {
        lock = false;
        clearTimeout(timeoutId);
        $jQuery('#totalSeconds').val(seconds);
        UpdateSessionTimeLeft(seconds);
        timeoutId = window.setTimeout("Tick()", 1000);
        lock = true;
    }
}

function Tick() {
    var totalSeconds = $jQuery('#totalSeconds').val();

    if (totalSeconds == 0) {
        $jQuery('#txtIsSessionExpired').val("true");
        // document.getElementById('<%= btnLogoff.ClientID%>').click();
        __doPostBack('ctl00$btnLogoff', '');
        return false;
    }

    totalSeconds--;
    UpdateSessionTimeLeft(totalSeconds);
    $jQuery('#totalSeconds').val(totalSeconds);
    timeoutId = window.setTimeout("Tick()", 1000);
    return false;
}

function UpdateSessionTimeLeft(totalSeconds) {
    var seconds = totalSeconds;
    var minutes = Math.floor(seconds / 60);
    seconds -= minutes * (60);
    var timeStr = AppendZero(minutes) + ":" + AppendZero(seconds);
    lblTimeLeft = document.getElementById('lblTimeLeft');
    lblTimeLeft.innerHTML = timeStr;

}

function AppendZero(seconds) {
    return (seconds < 10) ? "0" + seconds : +seconds;
}

function SetUserLink(userName) {
    var lnkButton = $jQuery("[id$=lnkUserName]")
    lnkButton[0].innerHTML = userName;
}

function ShowHideInstitute(isVisible) {
    if (isVisible) {
        $jQuery("#divInstitute").show();
        $jQuery("#divTenantName").show();
    }
    else {
        $jQuery("#divInstitute").hide();
        $jQuery("#divTenantName").hide();
    }
}

$jQuery(document).ready(function () {
    $jQuery("[id$=ddlTenantName_DropDown]").css("z-index", 900000);
    $jQuery("[id$=ddlUserTypeSwitchingView_DropDown]").css("z-index", 900000);
});

window.onbeforeunload = function (e) {
    CloseChildWindow();
}

function CloseChildWindow() {
    if (pdfDocViewerChildWnd != null) {
        pdfDocViewerChildWnd.close();
    }
}


///***UAT-1926 Start***///
//UAT-1926: 508 22 (o): A method shall be provided that permits users to skip repetitive navigation links. Start
$jQuery(document).ready(function () {
    $jQuery(".ifrmain").on("load", function () {
        //if ($jQuery(".ifrmain").contents().find(".page-heading").length > 0) {
        //    $jQuery(".ifrmain").contents().find(".page-heading").focus();
        //}
       // else {
            $jQuery("#mainBodyDiv").attr("tabindex", "0").focus();
       // }
    });
    $jQuery(".skip-main").keyup(function (e) {
        e.preventDefault();
        $jQuery(this).focusout();
        var mykeycode = e.keyCode;
        if (mykeycode == 13) {
            $jQuery("#modoutwr").focus();
        }
    });
});
///***UAT-1926 End***///