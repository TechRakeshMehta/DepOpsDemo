///// <reference path="/Resources/Generic/Ref.js"/>

if (typeof (IWEBLIB) != "undefined" && IWEBLIB) {

    $page.add_pageReady(function () {
        $jQuery(".filterbar").first().mouseenter(function () {
            $jQuery(this).addClass("showing").find(".bar_cmds").first().addClass("vis");
        }).mouseleave(function () {
            $jQuery(this).removeClass("showing").find(".bar_cmds").first().removeClass("vis");
        });

        //$jQuery(".sec_cmds .ihelp").each(function () {
        //    $jQuery(this).click(function (e) {
        //        $jQuery(this).toggleClass("help_on").parents(".section").first().find(".tab-block").slideToggle();

        //        e.stopPropagation();
        //    });
        //});


        //$jQuery(".tab-block .tabs span").each(function () {
        //    $jQuery(this).click(function () {
        //        var tab = $jQuery(this).attr("class").match(/tab\d+\s?/)[0].trim();
        //        if (tab) {
        //            tab = "." + tab;
        //            var tab_dom = $jQuery(this).parent().siblings(tab);
        //            if (!$jQuery(tab_dom).is(':visible')) {

        //                //Deselecting & hiding
        //                $jQuery(this).siblings().each(function () {
        //                    $jQuery(this).removeClass("focused");
        //                });
        //                $jQuery(this).parent().siblings().each(function () {
        //                    $jQuery(this).removeClass("focused");
        //                    $jQuery(this).hide();
        //                });

        //                //Displaying tab
        //                $jQuery(tab_dom).fadeIn();

        //                //Selecting tab
        //                $jQuery(this).addClass("focused");
        //            }
        //        }
        //    });
        //});
        //anchorClickEvents();
        //SetPreviousScrollPosition();
    });
}

$page.add_pageLoaded(function () {
    $jQuery(".sec_cmds .ihelp").each(function () {
        $jQuery(this).click(function (e) {
            $jQuery("[id$=hdnIsExplanatoryNoteClosed]")[0].value = $jQuery(this).parent(".sec_cmds").parent(".mhdr").parents(".section").first().find(".tab-block").is(':visible');
            $jQuery(this).toggleClass("help_on").parents(".section").first().find(".tab-block").slideToggle();
            e.stopPropagation();
            SaveUpdateExplanationState(this);
        });
    });


    $jQuery(".tab-block .tabs span").each(function () {
        $jQuery(this).click(function () {
            var tab = $jQuery(this).attr("class").match(/tab\d+\s?/)[0].trim();
            if (tab) {
                tab = "." + tab;
                var tab_dom = $jQuery(this).parent().siblings(tab);
                if (!$jQuery(tab_dom).is(':visible')) {

                    //Deselecting & hiding
                    $jQuery(this).siblings().each(function () {
                        $jQuery(this).removeClass("focused");
                    });
                    $jQuery(this).parent().siblings().each(function () {
                        $jQuery(this).removeClass("focused");
                        $jQuery(this).hide();
                    });

                    //Displaying tab
                    $jQuery(tab_dom).fadeIn();

                    //Selecting tab
                    $jQuery(this).addClass("focused");
                }
            }
        });
    });
    anchorClickEvents();
    //SetPreviousScrollPosition();
    //code to handle institute hierarchy tree
    (function () {

        var _prnt = $jQuery(".hier_data:first").parents(".hier:first");

        //create tree 
        if ($jQuery("#__hie_tree").length < 1) {
            var nodes = $jQuery(".hier_data:first").text().split(">");

            var tree = $jQuery("<div id='__hie_tree' style='display:none;margin-bottom:5px;'></div>");
            var left = 0;
            var colors = ["#0b1728", "#0055d4", "#670080", "#225500", "#aa0000"];
            var icolor = 0;
            $jQuery(nodes).each(function () {
                icolor = icolor < 4 ? icolor : 0;
                var span = $jQuery("<span class='hie_nodes'></span>");
                span.css("margin-left", left + "px");
                span.css("color", colors[icolor]);
                span.text(this.trim());
                tree.append(span).append("<br />");
                left = left + 5;
                icolor = icolor + 1;
            });
            $jQuery(_prnt).append(tree);
        }

        //code to attach events
        $jQuery(_prnt).click(function () {
            $jQuery(this).toggleClass("selected_row");
        })

        $jQuery(_prnt).hover(function () {
            if ($jQuery(_prnt).hasClass("selected_row")) return;
            $jQuery(".hier_data:first").hide();
            $jQuery("#__hie_tree").show();;
        }, function () {
            if ($jQuery(_prnt).hasClass("selected_row")) return;
            $jQuery(".hier_data:first").show();
            $jQuery("#__hie_tree").hide();;
        });

    })();



    //code to handle col/exp of appl details
    (function ($) {
        $("#cmd_profile_info").click(function () {
            $el = $("#profile_info");
            if ($el.length < 1) { return; }
            if ($el.is(":visible")) {
                $el.stop().slideUp(function () {
                    $("#cmd_profile_info").text("Show more details").removeClass("expdd").addClass("colsd");
                });
            }
            else {
                $("#cmd_profile_info").text("Show less details").removeClass("colsd").addClass("expdd");
                $el.stop().slideDown();
            }
        });
    })($jQuery);

});


function SetPreviousScrollPosition() {
    var a = $jQuery("[id$=hdnClassName]").val();
    var b = a.split(',');
    if (b.length > 1) {
        ShowHideApplicantDetail(b[0]);
        $jQuery("#applicant_data").scrollTop(b[1]);
    }
}

function anchorClickEvents() {
    $jQuery("[id$=lnkCategoriesNavigation]").click(function (event) {
        var className = $jQuery("#cmd_profile_info")[0].className;
        var intY = $jQuery("#applicant_data").scrollTop();
        SetPostMethod(event.target.href, className, intY);
        event.stopPropagation();
        return false;
    });
}

function SetPostMethod(href, className, intY) {
    var form = $jQuery('<form></form>');

    form.attr("method", "post");
    form.attr("action", href);
    var field = $jQuery('<input></input>');
    field.attr("type", "hidden");
    field.attr("name", "hdnScrollClassValue");
    field.attr("value", className + "," + intY);
    form.append(field);

    /*send the dockleft and docktop position  */
    var dockLeft = $jQuery("[id$=hdnDockLeft]")[0];
    var dockTop = $jQuery("[id$=hdnDockTop]")[0];
    var isFloatingMode = $jQuery("[id$=hdnIsFloatingMode]")[0];

    var field = $jQuery('<input></input>');
    field.attr("type", "hidden");
    field.attr("name", "hdnDockLeft");
    field.attr("value", dockLeft.value);
    form.append(field);
    var field = $jQuery('<input></input>');
    field.attr("type", "hidden");
    field.attr("name", "hdnDockTop");
    field.attr("value", dockTop.value);
    form.append(field);

    var field = $jQuery('<input></input>');
    field.attr("type", "hidden");
    field.attr("name", "hdnFloatingMode");
    field.attr("value", isFloatingMode.value);
    form.append(field);

    // The form needs to be a part of the document in
    // order for us to be able to submit it.
    $jQuery(document.body).append(form);
    form.submit();
}

function clickCategory(event) {
    $jQuery(event).find("[id$=lnkCategoriesNavigation]")[0].click();
}

function openPopUp() {
    var composeScreenWindowName = "composeScreen";
    var communicationTypeId = 'CT01';
    var applicantId = $jQuery("[id$=hdnApplicantId]").val();
    var applicantName = $jQuery("[id$=hdnApplicantName]").val();
    var url = $page.url.create("~/Messaging/Pages/WriteMessage.aspx?applicantId=" + applicantId + "&applicantName=" + applicantName + "&cType=" + communicationTypeId);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName });
}

function OnClientClose(oWnd, args) {
    oWnd.set_title('');
}


function ShowHideApplicantDetail(addClass) {
    if (addClass == 'expdd') {
        $jQuery("#cmd_profile_info").text("Show less details").removeClass("colsd").addClass("expdd");
        $jQuery("#profile_info").show();
    }
}

function Reloadmiddlepanel() {
    $jQuery('a', '.rlbItem.rlbSelected')[0].click();

}

var e_mnSptrLoaded = function () {
    //Adjust Panels
    adjustPanels($jQuery);
}


var e_mnSptrResized = function () {
    //Adjust Panels
    adjustPanels($jQuery);
}


var adjustPanels = function ($) {
    if (!$) return;
    $(".scroll-box").each(function () {
        var fbxH = 0;
        $(this).siblings(".fixed_box").each(function () {
            fbxH = fbxH + $(this).height();
        });
        var pbxH = $(this).parents(".pn-container:first").height();
        $(this).height(pbxH - fbxH);
    });

}

//UAT-613 added this method to show-hide the explanatory notes.
function onExplanatoryTabClick(tabToVisible) {

	//tabToVisible = "spnAdminExplanation";
    var temp = $jQuery(".tab-block .tabs span");
    temp.each(function () {
        var tab = $jQuery(this).attr("class").match(/tab\d+\s?/)[0].trim();
        if (tab) {
            tab = "." + tab;
            var tab_dom = $jQuery(this).parent().siblings(tab);
            if (this.id == tabToVisible) {
                if (!$jQuery(tab_dom).is(':visible')) {

                    //Deselecting & hiding
                    $jQuery(this).siblings().each(function () {
                        $jQuery(this).removeClass("focused");
                    });
                    $jQuery(this).parent().siblings().each(function () {
                        $jQuery(this).removeClass("focused");
                        $jQuery(this).hide();
                    });

                    //Displaying tab
                    $jQuery(tab_dom).fadeIn();

                    //Selecting tab
                    $jQuery(this).addClass("focused");
                }
            }
        }
    });
    anchorClickEvents();
    SetPreviousScrollPosition();
}

//UAT-613 added this method to show the explanatory notes by default.
function ShowExplanatoryNote() {
    tabToVisible = $jQuery("[id$=hdnExplanatoryNoteState]")[0].value;
    if (tabToVisible != "closedState") {
        $jQuery(".sec_cmds .ihelp").each(function () {
            if (this.id == "spniHelp") {
                var tab_Block = $jQuery(this).parent(".sec_cmds").parent(".mhdr").parents(".section").first().find(".tab-block");
                if (!(tab_Block.is(':visible')) && tab_Block[0].id == "divTabBlockUC") {
                    $jQuery(this).toggleClass("help_on").parents(".section").first().find(".tab-block").slideToggle();
                }
            }
        });

        onExplanatoryTabClick(tabToVisible);
    }
}

//UAT-613 Method to save the state of explanatory notes.
function SaveUpdateExplanationState(e) {
    var userId = $jQuery("[id$=hdnUserId]")[0].value;
    var explanatoryTabId = "";
    if (e.id == "spniHelp") {
        if ($jQuery("[id$=hdnIsExplanatoryNoteClosed]")[0].value == "true") {
            explanatoryTabId = "closedState";
        }
    }
    else {
        explanatoryTabId = e.id;
    }
    var urltoPost = "/ComplianceOperations/Default.aspx/SaveUpdateExplanatoryState";
    var dataString = "userId : '" + userId + "',explanatoryTabId : '" + explanatoryTabId + "'";
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
          }
      });


}

