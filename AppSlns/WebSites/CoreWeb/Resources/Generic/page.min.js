//////////////////////////////////////////////////////////////
//----------------------------------------------------------
// Copyright (C) Copyright Intersoft Data Labs, Inc. All rights reserved.
//----------------------------------------------------------
// File: page.js
// Version: 2012.11.2.1100
// Ad hoc changes-
// [1/9/2013 0930] Added code to fix ie focus issue with iframe
//[1/17/2013 1400] Addedd Code to fix tab focus on rad input
//[4/29/2013 1015] IE 9/jQuery1.9 focus issue resolved

//CODE FOR BACKWARD SUPPORT
Sys.Application.add_init(onPageInit);
Sys.Application.add_unload(onPageUnload);
var activeelem = null;
var Page = {
    showProgress: function (msg) {
        try {
            //debugger;
            if (document.activeElement.tagName == 'A' || document.activeElement.tagName == 'INPUT' || document.activeElement.tagName == 'SPAN') {
                activeelem = document.activeElement;
                activeelem.blur();
            }
        }
        catch (e) {
            // alert('error')
        }
        if (parent.Page != undefined) {
            parent.Page.showProgress(msg);
        }
    },
    hideProgress: function (onHideComplete) {
        if (parent.Page != undefined) {
            parent.Page.hideProgress(onHideComplete);
        }
    }
};

function onPageInit(sender) {
    Page.showProgress("Initializing...");
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(onBeginRequest);
    prm.add_endRequest(onEndRequest);
}


function onBeginRequest(sender, args) {
    Page.showProgress("Processing...");
    $jQuery("#pageMsgBox").fadeIn().children("span").text("").attr("class", "");
}

function onHideComplete() {
    if (activeelem != null) {
        activeelem.focus();
        activeelem = null;
    }
}

function onEndRequest(sender, args) {

    if ($jQuery('#hdnCompliancePackageDetails').val() == 'true') {
        setTimeout(FocusSetFunction, 1);
    }

    Page.hideProgress(onHideComplete);
    if (args.get_error() != undefined) {
        var errorMessage = "An unknown exception occurred while connecting to the server. The possible reasons could be- 1) Expiration of Session or 2) Injection of Special character(s), where not allowed (for example using <> in filters or forms) or 3) Internet Connection failure. Please Re-login, remove special characters or check internet connection. If problem persists contact System Admin.";
        var element = '';
        const ErrorLogNumber = "-2147467259";
        if (args._response._xmlHttpRequest.responseText.indexOf(ErrorLogNumber) >= 0) {
            $jQuery('#MsgBox').hide();
            $jQuery("#pageMsgBox").show();
            $jQuery("#pnlError").show();
            element = parent.document.getElementById("hdnGenericExceptionMsgXSSANDHTML");
            if (element == null) {
                element = parent.parent.document.getElementById("hdnGenericExceptionMsgXSSANDHTML");
            }
        }
        else {
            element = parent.document.getElementById("hdnGenericExceptionMsg");
            if (element == null) {
                element = parent.parent.document.getElementById("hdnGenericExceptionMsg");
            }
        }
        if (element != '' && typeof (element) != 'undefined' && element != null) {
            errorMessage = element.value;
        }
        $page.showMessage(errorMessage, $page.msgTypes.ERROR);
    }
}

function onPageUnload(sender) {
    $jQuery(window).focus();

    // Page.showProgress("Please wait...");
}

function toggle_visibility(id) {
    $telerik.$("#" + id).slideToggle();
}

function ShowHide(div1, div2) {
    var div1 = $telerik.$(this).attr(div1);
    var div2 = $telerik.$(this).attr(div2);

    $telerik.$(div1).css('display', 'inline');
    $telerik.$(div2).css('display', 'none');
}

function ShowHideSinRad(div1, obj) {
    $telerik.$('.' + obj).addClass('HideObj');
    $telerik.$('#' + div1).removeClass('HideObj');
}

function FocusOnPopUpButton(sender, eventArgs) {
    sender._popupButton.focus();
}


/////////////////////////////////////////////////////////////////
//NEW CODE
if (typeof (IWEBLIB) != "undefined" && IWEBLIB) {

    //Setting up application object
    IWeb.UI.Page.prototype.app = null;
    $page.app = top.$page.app;

    //Mapping window manager, window bahaviours & global dialog functions with top window 
    $window = top.$window;
    if (typeof (Telerik.Web.UI.WindowBehaviors) == "undefined") {
        Telerik.Web.UI.WindowBehaviors = top.Telerik.Web.UI.WindowBehaviors;
    }
    $alert = top.$alert;
    $confirm = top.$confirm;

    //Setting up message box object
    IWeb.UI.Page.prototype.msgTypes = {};
    $page.msgTypes = {
        'ERROR': 'error',
        'INFO': 'info',
        'SUCCESS': 'sucs'
    };

    $page.showMessage = function (msg, msgtype) {
        /// <summary>Shows message box on the page</summary>
        /// <param name="msg" type="String">Message to be displayed</param>
        /// <param name="msgtype" type="$page.msgTypes">Type of message box</param>

        if (typeof (msg) == "undefined") return;
        var c = typeof (msgtype) != "undefined" ? msgtype : "";
        if ($jQuery(".no_error_panel").length > 0) {
            $jQuery("#pageMsgBox").children("span").text(msg).attr("class", msgtype);
            if (c == 'sucs') { c = "Success"; }
            else (c = c.toUpperCase());
            $window.showDialog($jQuery("#pageMsgBox").clone().show(), { closeBtn: { autoclose: true, text: "Close" } }, 400, c);

        }
        else {
            $jQuery("#pageMsgBox").fadeIn().children("span").text(msg).attr("class", msgtype);
        }
    }

    $page.showAlertMessage = function (msg, msgtype, overriderErrorPanel) {
        /// <summary>Shows message box on the page</summary>
        /// <param name="msg" type="String">Message to be displayed</param>
        /// <param name="msgtype" type="$page.msgTypes">Type of message box</param>

        if (typeof (msg) == "undefined") return;
        var c = typeof (msgtype) != "undefined" ? msgtype : "";
        if ($jQuery(".no_error_panel").length > 0 || overriderErrorPanel) {
            $jQuery("#pageMsgBox").children("span").text(msg).attr("class", msgtype);
            if (c == 'sucs') {
                c = "Success";
            }
            else (c = c.toUpperCase());

            $jQuery("#pnlError").hide();

            $window.showDialog($jQuery("#pageMsgBox").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 400, c);
        }
        else {
            $jQuery("#pageMsgBox").fadeIn().children("span").text(msg).attr("class", msgtype);
        }
    }



    //EVENT -> PAGE LOAD
    $page.add_pageLoad(function () {

        //Code to handle anchors
        $jQuery('a').each(function () {
            $jQuery(this).click(function (evt) {
                if (evt.ctrlKey || evt.shiftKey) {
                    return false;
                }
            });
        });

        //Code to mark label for elements with validation failed state (currently not functional)
        $jQuery("span.errmsg").each(function () {
            $jQuery(this).parent().prev(".sxlb").css({ "background-color": "Red", "opacity": .5 });
        });

        //Code for displaying message boxes over the page
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

        //Code to show page message when HideTitleBars method is used on the screen
        (function ($) {
            if ($("form.no_error_panel").length > 0) {

                if ($("#pnlError .msgbox span").text().replace(/\s/g, "") != "") {
                    //alert($("#pnlError .msgbox span").text());
                    var c = $("#pnlError .msgbox span").attr("class");
                    var titleText_LangCulture = "Success";
                    if ($("#hdnSuccessText_LangCulture") != undefined) {
                        titleText_LangCulture = $("#hdnSuccessText_LangCulture").val();
                    }
                    var close_btnText = "Close";
                    if ($("#hdnCloseButtonText_LangCulture") != undefined) {
                        close_btnText = $("#hdnCloseButtonText_LangCulture").val();
                    }

                    //if (c == 'sucs') { c = "Success"; }
                    if (c == 'sucs') { c = titleText_LangCulture; }
                    else (c = c.toUpperCase());

                    var box = $jQuery("#pageMsgBox").clone();
                    $jQuery(box).css("opacity", 1);
                    $jQuery(box).css("overflow-y", 'auto');
                    $jQuery(box).css("max-height", 350);
                    //$window.showDialog($jQuery(box), { closeBtn: { autoclose: true, text: "Close" } }, 400, c);
                    $window.showDialog($jQuery(box), { closeBtn: { autoclose: true, text: close_btnText } }, 400, c);
                }
            }
        })($jQuery);


        //Code to create a wrapper around grid's detail table
        var rgDetail = $jQuery(".rgDetailTable");
        if (rgDetail.html()) {
            rgDetail.each(function () {
                if (!$jQuery(this).children(".rgDetWrapper").html()) {
                    $jQuery(this).wrap('<div class="rgDetWrapper" />').css("border-width", "1px").children(".rdDetWrapper").css("width", rgDetail.outerWidth());
                }
            });
        }
    });

    //EVENT -> PAGE READY
    $page.add_pageReady(function ($) {
        $('#modoutwr').css("visibility", "visible");

        $(document).on("click", ".mhdr", function () {
            if (!$(this).hasClass("nocolps")) {
                if ($(this).hasClass("colps")) {
                    //Expand
                    $(this).removeClass("colps").parent(".section").removeClass("collapsed");
                    $(this).next(".content:first").slideDown();

                }
                else {
                    //collapse
                    var that = this;
                    $(this).next(".content:first").slideUp(function () {
                        $(that).addClass("colps").parent(".section").addClass("collapsed");
                    });
                }
            }
        });



        $(document).on("click", ".sbhdr", function () {
            if (!$(this).hasClass("nocolps")) {
                $(this).next(".sbcontent:first").slideToggle();

                $(this).toggleClass("colps");
                if ($(this).parent(".sbsection").hasClass("collapsed")) {
                    $(this).parent(".sbsection").removeClass("collapsed");
                }
                else {
                    $(this).parent(".sbsection").addClass("collapsed");
                }
            }
        });

        //    $(document).bind('contextmenu', function (e) { return false; });

        $("input:checkbox").css({ 'padding': '5px', 'vertical-align': 'middle' });
        $("input:radio").css({ 'padding': '5px', 'vertical-align': 'middle' });

        //Hiding progress
        Page.hideProgress();
    });


}


//[1/9/2013 0930] Code to force ie keep focus on iframe
$page.add_pageReady(function () {
    if (navigator.userAgent.toLowerCase().indexOf("msie") >= 0) {
        $jQuery("#modcmds").append("<input type='button' id='_elementZero' style='height:1px;width:1px;padding:0;margin:0;border:none;' />");
        //[4/29/2013 1015] IE 9/jQuery1.9 focus issue resolved by using setTimeout
        setTimeout(function () {
            $jQuery("#_elementZero").focus();
        }, 500);
    }
});


//[1/17/2013 1400] Code to fix tab focus on rad input
$page.add_pageReady(function ($) {
    $(".ruFileWrap .ruFileInput").each(function () {
        var That = this;
        $(That).focus(function (e) {
            if ($(this).siblings(".ruButton:first").attr("_ruFix") != "true") {
                $(this).siblings(".ruButton:first").attr("_ruFix", true)
                $(this).siblings(".ruButton:first").focus();
            }
            else {
                $(this).siblings(".ruButton:first").attr("_ruFix", false);
            }
        });

        $(this).siblings(".ruButton:first").click(function () {
            $(That).click();
        });
    });

    $("body").click(function () {
        $(".ruButton").each(function () {
            $(this).attr("_ruFix", false);
        });
    });
});


//[1/29/2014] Code to add titles on grid command item buttons
$page.add_pageLoad(function () {
    var $ = $jQuery;
    $(".grdCmdBar .RadButton").each(function () {
        //console.log($(this).find(".rbText").text());
        if ($(this).text().toLowerCase() == "refresh") {
            $(this).attr("title", "Click to reload the data in the grid");
        }
        if ($(this).text().toLowerCase() == "clear filters") {
            $(this).attr("title", "Click to clear any information entered in the filters");
        }
        if ($(this).text().toLowerCase() == "download") {
            $(this).attr("title", "Click to export the data displayed in the grid");
        }
    });
});

//UAT 726
function openCmbBoxOnTab(sender, e) {
    //console.log(e.get_domEvent().keyCode);
    var code = e.get_domEvent().keyCode
    if (code != 9) {
        if (!sender.get_dropDownVisible())
            sender.showDropDown();
    }
}


$jQuery(document).on("keydown", ".rmItem , .rmLink, .rmText", function (e) {

    if ($jQuery(this).text() == "IsEmpty") {
        $jQuery(".rmFirst").attr("tabindex", "0");
        $jQuery(".rmFirst").focus();
    }

    if (e.keyCode == 27) {
        $jQuery(":focus").blur();
        $jQuery(this).parents(".rmActive").removeAttr("style");
        $jQuery(this).parents(".rmActive").css("display", "none");
        $jQuery("#" + localStorage.getItem("filterinput")).focus();
        //localStorage.clear();
    }

});

function FocusMenu(menu, eventArgs) {
    localStorage.setItem("filterinput", eventArgs._targetElement.id)
    menu.get_items().getItem(1).get_linkElement().focus();
}



