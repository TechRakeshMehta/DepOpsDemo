//////////////////////////////////////////////////////////////
//----------------------------------------------------------
// Copyright (C) Copyright Intersoft Data Labs, Inc. All rights reserved.
//----------------------------------------------------------
// File: popup.js
// Version: 2012.11.2.1100
//

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

    //Current Window Related code
    $page.get_window = function () {
        /// <summary>Gets the reference to the current popup window</summary>
        /// <returns type="Telerik.Web.UI.RadWindow" />          
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }

    //Closes the pop up window
    $page.close = function () {
        //Following line of code is suggested by telerik team @ticket no. 558277
        setTimeout(function () {
            var oWindow = $page.get_window();
            if (oWindow) {
                oWindow.close();
            }
        }, 0);
    }

    $page.showMessage = function (msg, msgtype) {
        /// <summary>Shows message box on the page</summary>
        /// <param name="msg" type="String">Message to be displayed</param>
        /// <param name="msgtype" type="$page.msgTypes">Type of message box</param>

        if (typeof (msg) == "undefined") return;
        var c = typeof (msgtype) != "undefined" ? msgtype : "";
        $jQuery("#pageMsgBox").fadeIn().children("span").text(msg).attr("class", msgtype);
    }

    //ProgressBar code
    $page.showProgress = function (msg) {
        $jQuery("#popupProgress").fadeIn();
    }

    $page.hideProgress = function () {
        $jQuery("#popupProgress").fadeOut();
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


        //Code for displaying message boxes over the page
        (msgupd = function ($) {
            $(".error").each(function () {
                if ($(this).text().replace(/\s/g, "") != "") $(this).parents(".msgbox:last").fadeIn();
            });
            $(".info").each(function () {
                if ($(this).text().replace(/\s/g, "") != "") $(this).parents(".msgbox:last").fadeIn();
            });
            $(".sucs").each(function () {
                if ($(this).text().replace(/\s/g, "") != "") $(this).parents(".msgbox:last").fadeIn();
            });
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

        $page.hideProgress();
    });

    //EVENT -> PAGE READY
    $page.add_pageReady(function ($) {

        $(".mhdr").on("click", function () {
            if (!$(this).hasClass("nocolps")) {
                //$(this).next(".content:first").slideToggle();
                //$(this).toggleClass("colps");
                //if ($(this).hasClass('colps')) {
                //    $(this).parent(".section").addClass("collapsed");
                //} else {
                //    $(this).parent(".section").removeClass("collapsed");
                //}
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

        $(".sbhdr").on("click", function () {
            if (!$(this).hasClass("nocolps")) {
                $(this).next(".sbcontent:first").slideToggle();
                $(this).toggleClass("colps");
            }
        });

        //  $(document).bind('contextmenu', function (e) { return false; });

        $("input:checkbox").css({ 'padding': '5px', 'vertical-align': 'middle' });
        $("input:radio").css({ 'padding': '5px', 'vertical-align': 'middle' });

    });

    //EVENT -> BEGIN REQUEST
    $page.add_beginRequest(function (sender, args) {
        $page.showProgress();
    });


    //EVENT -> END REQUEST
    $page.add_endRequest(function (sender, args) {
        $page.hideProgress();
        if (args.get_error() != undefined) {
            var errorMessage = "An unknown exception occurred while connecting to the server. The possible reasons could be- 1) Expiration of Session or 2) Injection of Special character(s), where not allowed (for example using <> in filters or forms) or 3) Internet Connection failure. Please Re-login, remove special characters or check internet connection. If problem persists contact System Admin.";
            var element = '';
            const ErrorLogNumber = "-2147467259";
            if (args._response._xmlHttpRequest.responseText.indexOf(ErrorLogNumber) >= 0) {
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
    });

    $page.add_pageUnload(function () {
        $jQuery("#popupProgress").show();
    });

}

function FocusMenu(menu, eventArgs) {
    localStorage.setItem("filterinput", eventArgs._targetElement.id)
    menu.get_items().getItem(1).get_linkElement().focus();
}
function closeWindow() {
    $page.get_window().set_destroyOnClose(false);
    $page.get_window().close();
}