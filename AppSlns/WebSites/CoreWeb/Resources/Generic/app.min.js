///// <reference path="/Resources/Generic/Ref.js"/>
//----------------------------------------------------------
// Copyright (C) Copyright Intersoft Data Labs Inc. All rights reserved.
//----------------------------------------------------------
// File: app.js
// Version: 2012.11.2.1100
// [1/16/2013 1500] Bug#303 fix

//CODE FOR BACKWARD SUPPORT
Sys.Application.add_init(onPageInit);


//UI Pattern settings
FSObject.$(window).load(function () {
    FSObject.$("#applodrwr").fadeOut();
});

function onPageInit(sender) {
    Page.showProgress("Initializing...");
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(onBeginRequest);
    prm.add_endRequest(onEndRequest);
}

function onBeginRequest(sender, args) {
    Page.showProgress("Processing...");
}

function onEndRequest(sender, args) {
    Page.hideProgress();
}

var Page = {
    showProgress: function (msg) {
        FSObject.$('#modlodrwr .msg').text(msg);
        if (!FSObject.$("#progswr").is(":visible")) FSObject.$("#progswr").fadeTo(500, .5); // FSObject.$("#progswr").show();
    },
    hideProgress: function (onHidden) {
        if (onHidden != undefined) {
            FSObject.$("#progswr").fadeOut(function () { window.setTimeout(onHidden, 0); });
            window.onHidden = undefined;
        }
        else
            FSObject.$("#progswr").fadeOut();
    }
};


/////////////////////////////////////////////////////////////////
//NEW CODE
if (typeof (IWEBLIB) != "undefined" && IWEBLIB) {

    //Add application object in $page (code here serves for intellisense purpose only)
    IWeb.UI.Page.prototype.app = {};

    $page.app = {

        //Left Panel
        leftPanel: new IWeb.UI.AjaxRegion(),

        //Menu Panel
        menu: new IWeb.UI.AjaxRegion(),

        //iFrame
        modFrame: new IWeb.UI.Frame(),

        //Following method sets title for the window
        //AD: Due to bug 7391 the logic of setting the titles has been moved to app.js inorder to tackle more scenarios
        //like when module, page, portal titles are missing and when postback is made on app menu        
        setTitle: function (title) {
            /// <summary>Sets the title of the application window.</summary>
            /// <param name="title" type="String">A string that is used as a title. If not provided the title will be auto-generated based on the page context</param>

            var appname = "American Databank | ";
            var tag = "";

            if (title) {
                document.title = appname + title;
                return;
            }

            var mod;
            var page;
            title = "";

            if ($page.app.modFrame.get_contents()) {
                mod = $page.app.modFrame.get_contents().find("#lblModHdr").text();
                page = $page.app.modFrame.get_contents().find("#lblPageHdr").text();
            }

            //Getting the module title
            if (typeof (mod) == "string" && mod.trim()) {
                title = mod.trim() + title;
            }

            //Getting page title
            if (typeof (page) == "string" && page.trim()) {
                title = title ? title + " :: " + page.trim() : page.trim();
            }

            //Creating title from portal and tagline if module and page titles are missing
            if (!title) {
                title = $jQuery("#lblAppName").text().trim();
                if (!title) {
                    title = tag;
                }
            }

            //Setting up the window title
            document.title = appname + title;
        }
    }

    //Adding functionality in left panel object
    $page.app.leftPanel.collapse = function () {
        /// <summary>Collapses the left panel</summary>
        $findByKey("BottomSplitter", function () {
            //var This = $telerik.toSplitter();
            //var collapsed = this.getPaneByIndex(0).get_collapsed();
            this.getPaneByIndex(0).collapse();
        });
    }

    $page.app.leftPanel.expand = function () {
        /// <summary>Expands the left panel</summary>
        $findByKey("BottomSplitter", function () {
            this.getPaneByIndex(0).expand();
        });
    }

    //AD:Added this method to fix bug #7665
    $page.app.leftPanel.deselect = function () {
        /// <summary>Deselects the selected item of a panel bar within left panel</summary>
        $findByKey("LeftPanelBar", function () {
            //var This = $telerik.toPanelBar();
            if (this.get_selectedItem()) {
                this.get_selectedItem().unSelect();
            }
        });
    }

    //EVENT -> PAGE INIT
    $page.add_pageInit(function () {

        //Initializing menu region        
        $page.app.menu.set_panelID('updMenu');

        //Initializing panel region
        $page.app.leftPanel.set_panelID('updPanelLeft');

        //Initisalizing main iframe
        $page.app.modFrame = new IWeb.UI.Frame('ifrPage');


        //Handlign Iframe paged loaded event
        $page.app.modFrame.add_loaded(function () {

            //Hiding progress when page is loaded
            if (window.onHidden != undefined)
                Page.hideProgress(onHidden);
            else
                Page.hideProgress();

            //Disabling right click
            //  $jQuery(this.get_document()).bind('contextmenu', function (e) { return false; });

            //Setting titles
            $page.app.setTitle();

        });

        //Diabling right click on current page
        //   $jQuery(document).bind('contextmenu', function (e) { return false; });

    });


    //EVENT -> PAGE LOAD
    $page.add_pageLoad(function () {

        //Fixing menu every time page is loaded
        (menuFix = function ($, auto) {
            if (auto) {
                $("#applodrmsg").show();
                $("#applodrwr").fadeIn();
                $("#applodrmsg").html('<br />Please wait...');
            }
            var rzTOut = $("#appmenuwr ul:first").attr("sxResizeTimeout");
            if (!isNaN(rzTOut)) clearTimeout(rzTOut);
            rzTOut = setTimeout(function () {
                var mnwdth = 0;
                $("#appmenuwr ul:first > li").each(function (index) {
                    mnwdth = mnwdth + $(this).width() + 2;
                });
                $("#appmenuwr ul:first").width(mnwdth);
                $("#appmenuwr ul:first").height(38);
                $("#appmenuwr .RadMenu").height(38);
                if (auto) $("#applodrwr").fadeOut();
                $("#appmenuwr ul:first").removeAttr("sxResizeTimeout");
            }, (auto ? 1500 : 200));
            $("#appmenuwr ul:first").attr("sxResizeTimeout", rzTOut);
        })($jQuery);
        $jQuery(window).resize(function () { menuFix($jQuery, true); });


        //Handling anchors
        $jQuery('a').each(function () {
            $jQuery(this).click(function (evt) {
                if ($jQuery(this).attr('href') == "#") return;
                if (!$jQuery(this).attr('href')) return;
                if (evt.ctrlKey || evt.shiftKey) {
                    Page.showProgress('Please wait...');
                    //AD:bug #7665
                    $page.app.leftPanel.deselect();
                    $page.app.modFrame.navigate($jQuery(this).attr('href'));
                    return false;
                }
                else {
                    Page.showProgress('Please wait...');
                    //AD:bug #7665
                    $page.app.leftPanel.deselect();
                }

            });
        });

        //Bug: #6093
        //Handling collapsible button of non parent panel items
        $jQuery("#updPanelLeft .rpExpandHandle").click(function () {
            return false;
        });


    });

    //EVENT -> PAGE READY
    $page.add_pageReady(function () {
        var hdnCloseSplashScreen = $jQuery("[id$=hdnCloseSplashScreen]").val();
        var hdnClickingHere = $jQuery("[id$=hdnClickingHere]").val();
        var hdnWhilePageLoad = $jQuery("[id$=hdnWhilePageLoad]").val();

        $jQuery("#progswr").fadeTo(500, .5);
        $jQuery('#modoutwr').css("visibility", "visible");
        //$jQuery("#applodrmsg").append('<br />You can close this splash screen by <a href="javascript:void(0)" id="applodbtn">clicking here</a> while rest of the page loads.');
        $jQuery("#applodrmsg").append('<br />' + hdnCloseSplashScreen + ' <a href="javascript:void(0)" id="applodbtn">' + hdnClickingHere + '</a> ' + hdnWhilePageLoad);
        $jQuery("#applodbtn").click(function () { $jQuery("#applodrwr").fadeOut(); $jQuery("#pglodrmsg").hide(); Page.showProgress("Please wait..."); });

        //Setting up default dialog configurations
        $window.set_defaultDialogTitle("Complio");

        //Code to open chat popup
        if (typeof (__chatbuttonID) != "undefined" && typeof (__chatURL) != "undefined" && __chatbuttonID && __chatURL) {
            $jQuery("#" + __chatbuttonID).click(function () {
                var url = $page.url.create(__chatURL);
                //UAT-2364
                var popupHeight = $jQuery(window).height() * (50 / 100);

                var chat_win = $window.createPopup(url, { size: "400,"+popupHeight, behavior: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move }, function () { this.set_destroyOnClose(true); });
            });
        }
    });
}


// [1/16/2013 1500] Bug#303 fix
var mnuMain_clicked = function (a, e) {
    var i = e.get_item();
    setTimeout(function () {
        //Close menu automatically after redirection
        a.close();
        i.set_selected(false); $jQuery(i.get_linkElement()).removeClass("rmSelected").removeClass("rmFocused");
    }, 1000);
}
// [1/16/2013 1500] Bug#303 fix
var mnuMain_closed = function (a, e) {
    var i = e.get_item();
    $jQuery(i.get_linkElement()).removeClass("rmSelected").removeClass("rmFocused");
    i.set_selected(false);
}


//[9/30/2013 1520] JIRA Bug# UAT-51
//Menu bug - Not closing on mouse out when mouse moved directly from menu to page Iframe

var __mnuMainTimer = new IWeb.Timer(1000);
__mnuMainTimer.add_tick(function () {
    close_mnuMain();
});

var mnuMain_MouseOut = function (a, b) {
    __mnuMainTimer.startOnce();
}

var mnuMain_MouseOver = function (a, b) {
    __mnuMainTimer.stop();
}

var close_mnuMain = function () {
    //console.log("close");
    $findByKey("mnuMain_app", function () {
        this.close(true);
    });
}