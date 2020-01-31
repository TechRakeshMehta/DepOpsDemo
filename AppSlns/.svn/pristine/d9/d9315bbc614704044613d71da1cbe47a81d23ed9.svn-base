
function pageLoad(sender) {
    (msgupd = function ($) {
        $(".error").each(function () {
            if ($(this).text().replace(/\s/g, "") != "") $(this).parent(".msgbox:last").fadeIn();
        });
        $(".info").each(function () {
            if ($(this).text().replace(/\s/g, "") != "") $(this).parent(".msgbox:last").fadeIn();
        });
        $(".sucs").each(function () {
            if ($(this).text().replace(/\s/g, "") != "") $(this).parent(".msgbox:last").fadeIn();
        });
    })($jQuery);
}

//Removes the messages from Login page.
function btnSubmit_ClientClicked(sender) {
    $jQuery('[id$="lblErrorMessage"]').text("");
    if ($jQuery('[id$="btnResendActivationLink"]') != undefined && $jQuery('[id$="btnResendActivationLink"]').length > 0) {
        $jQuery('[id$="btnResendActivationLink"]').hide();
    }
    if ($jQuery('[id$="lblErrorMessageExtended"]') != undefined && $jQuery('[id$="lblErrorMessageExtended"]').length > 0) {
        $jQuery('[id$="lblErrorMessageExtended"]').hide();
    }
}

//Removes the messages from Change Password page.
function btnUpdate_ClientClicked(sender) {
    $jQuery('[id$="lblMessage"]').text("");
}

//Removes the messages from Forgot Password page.
function btnSave_ClientClicked(sender) {
    $jQuery('[id$="lblMessage"]').text("");
}

//Removes the messages from Forgot Password page.
function btnGenerate_ClientClicked(sender) {
    $jQuery('[id$="lblMessage"]').text("");
}


//AD: displays message
$jQuery(document).ready(function () {

    $jQuery(".msgbox .msg_cmd").first().click(function () {

        $jQuery(this).parents(".msgbox:last").fadeOut();
    });
    $jQuery(".reg-complete").each(function () {
        if ($jQuery(this).text().replace(/\s/g, "") != "") {
            $jQuery(this).parents(".msgbox:last").fadeIn();
        }
    });

    if (document.msCapsLockWarningOff == false) {
        document.msCapsLockWarningOff = true;

    } else {
        document.msCapsLockWarningOff = false;

    }
});


// Checks the caps lock is on or not.
function capLock(e) {

    var kc = e.keyCode ? e.keyCode : e.which;
    var sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
    if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk)) {
        document.getElementById('divCapsLock').style.visibility = 'visible';
    }
    else {
        document.getElementById('divCapsLock').style.visibility = 'hidden';
    }
}

var capsLockEnabled = null;

function getChar(e) {

    if (e.which == null) {
        return String.fromCharCode(e.keyCode); // IE
    }
    if (e.which != 0 && e.charCode != 0) {
        return String.fromCharCode(e.which); // rest
    }

    return null;
}

document.onkeydown = function (e) {
    e = e || event;

    if (e.keyCode == 20 && capsLockEnabled !== null) {
        capsLockEnabled = !capsLockEnabled;
    }
}

document.onkeypress = function (e) {
    e = e || event;

    var chr = getChar(e);
    if (!chr) return; // special key

    if (chr.toLowerCase() == chr.toUpperCase()) {
        // caseless symbol, like whitespace 
        // can't use it to detect Caps Lock
        return;
    }

    capsLockEnabled = (chr.toLowerCase() == chr && e.shiftKey) || (chr.toUpperCase() == chr && !e.shiftKey);
}

/**
 * Check caps lock 
 */
function checkCapsWarning() {
    document.getElementById('caps').style.display = capsLockEnabled ? 'block' : 'none';
}

function removeCapsWarning() {
    window.setTimeout(function ()
    { document.getElementById('caps').style.display = 'none'; }, 500);
}


