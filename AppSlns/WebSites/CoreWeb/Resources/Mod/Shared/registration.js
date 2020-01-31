///<reference path="/Resources/Generic/ref.js"/>

$page.add_pageReady(function () {
    //$jQuery("span.reqd").html("").parent("label").addClass("req");    
    //[BS UAT-199]. Captcha Validate issue on Firefox after screen design changes by [AD]. 
    //Resolved by reloading the Captcha Code when page loads.
    //$jQuery('.LBD_ReloadLink').click();

    //Date:20-Mar-2014, Code for refreshing the captcha on every postback.
    $jQuery(window).on("load", function () {
        //UAT - 905: Focus of the cursor gets displays on incorrect field (verification code) when navigated on Create Account screen.
        var focused = document.activeElement;
        $jQuery(".LBD_ReloadLink").trigger("click");
        //UAT - 905: Focus of the cursor gets displays on incorrect field (verification code) when navigated on Create Account screen.
        if (focused != undefined) {
            //setTimeout(function () { $jQuery("[id$=" + focused.id + "]").focus(); }, 0);
            $jQuery("[id$=" + focused.id + "]").focus();
        }
    });
    $jQuery(".LBD_ReloadLink").on("click", function () { });
});



function clearlblUserMessage(sender, eventArgs) {

    var userName = $jQuery("[id$=txtUsername]").val();
    if (userName == "") {
        //Clear the lblUserNameMessage text
        $jQuery("[id$=lblUserNameMessage]").text('');
    }
}

var txtpwd_load = function (a, b) {
    $jQuery(a.get_element()).keyup(function (event) {
        var text = $jQuery(this).val();
        //show_pwdtool();
        check_password(text);
    });
    $jQuery(a.get_element()).focusout(function () {
        show_pwdtool(false);
    });
    $jQuery(a.get_element()).focusout(function () {
        show_pwdtool(false);
    });
}

var show_pwdtool = function (show) {

    $findByKey("pwdTip", function () {
        if (show === false) this.hide();
        else {
            this.show();
        }
    });
}

var specialChars = "&*.;=\"<>|`"; //Sumit 20/08/2014 UAT-713  Check for special characters not allowed.

var check = function (string) {
    for (i = 0; i < specialChars.length; i++) {
        if (string.indexOf(specialChars[i]) > -1) {
            return true
        }
    }
    return false;
}


var check_password = function (text) {
    var _white = /\s/;
    var _digit = /\d/;
    var _char = /[A-Z]/;
    var _symb = /[@#$%\^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-]/; //Sumit 20/08/2014 UAT-713 Check for special characters allowed
    var _len = text.length;
    var _ul = $jQuery(".pwd_hint ul");

    if (!_white.test(text)) {
        $jQuery(_ul).children(".white:first").addClass("yes").removeClass("no");
    }
    else {
        $jQuery(_ul).children(".white:first").addClass("no").removeClass("yes");
    }

    if (_digit.test(text)) {

        $jQuery(_ul).children(".digit:first").addClass("yes").removeClass("no");
    }
    else {
        $jQuery(_ul).children(".digit:first").addClass("no").removeClass("yes");
    }
    if (_char.test(text)) {
        $jQuery(_ul).children(".char:first").addClass("yes").removeClass("no");
    }
    else {
        $jQuery(_ul).children(".char:first").addClass("no").removeClass("yes");
    }
    if (_symb.test(text) && check(text) == false) {
        $jQuery(_ul).children(".sym:first").addClass("yes").removeClass("no");
    }
    else {
        $jQuery(_ul).children(".sym:first").addClass("no").removeClass("yes");
    }

    if (_len > 7 && _len < 16) {
        $jQuery(_ul).children(".len:first").addClass("yes").removeClass("no");
    }
    else {
        $jQuery(_ul).children(".len:first").addClass("no").removeClass("yes");
    }

}