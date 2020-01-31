var e_viewMessage = function (sender, args) {

    var selectedRow = sender.get_masterTableView().get_dataItems()[args.get_itemIndexHierarchical()];
    var messageFrom = sender.get_masterTableView().getCellByColumnUniqueName(selectedRow, "From").innerText;
    var messageDate = sender.get_masterTableView().getCellByColumnUniqueName(selectedRow, "ReceivedDateFormat").innerText;
    var communicationTypeCode = args._dataKeyValues.CommunicationTypeCode;
    var messageImportance = args._dataKeyValues.IsHighImportant;

    $jQuery("[id$=hdnSelectedMessage]").val(args._dataKeyValues.MessageDetailID);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/Messaging/Pages/MessageViewer.aspx?messageID=" + args._dataKeyValues.MessageDetailID + "&isImportant=" + messageImportance +
    "&From=" + messageFrom + "&Date=" + messageDate + "&isDashboardMessage=" + true + "&cType=" + communicationTypeCode);
    var win = $window.createPopup(url, { size: "800,"+popupHeight, onclose: OnClientClose });
}

function OnClientClose() {
    top.location.href = top.location.href;
}

function RowContextMenu(sender, eventArgs) {

    var menu = $find($jQuery("[id$=rMenuRecentMessagesGrid]")[0].id);
    var evt = eventArgs.get_domEvent();

    if (evt.target.tagName == "INPUT" || evt.target.tagName == "A") {
        return;
    }

    var index = eventArgs.get_itemIndexHierarchical();
    $jQuery("[id$=hdnSelectedRow]").val(index);

    sender.get_masterTableView().selectItem(sender.get_masterTableView().get_dataItems()[index].get_element(), true);

    menu.show(evt);

    evt.cancelbubble = true;
    evt.returnvalue = false;

    if (evt.stoppropagation) {
        evt.stoppropagation();
        evt.preventdefault();
    }
}


function OpenWriteMessageWindow(messageId, queueType, currentUserID, userGroup, communicationTypeCode, actionType, windowName, parent_window) {
    var tableView = $find($jQuery("[id$=grdRecentMessages]")[0].id).get_masterTableView();
    var url = $page.url.create("~/Messaging/Pages/WriteMessage.aspx?messageID=" + messageId + "&action=" + actionType + "&queueType=" + queueType + "&currentUserId=" + currentUserID + "&userGroupId=" + userGroup + "&cType=" + communicationTypeCode);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var child = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: windowName });

    //get parent
    if (typeof (parent_window) != "undefined") {
        var zin = parent_window.get_zindex();
        $jQuery(child.get_popupElement()).css("z-index", zin + 10);
    }
}

function DeleteMessage(deleteFrom) {
    var messageId = $jQuery("[id$=hdnSelectedMessage]").val();
    var tableView = $find($jQuery("[id$=grdRecentMessages]")[0].id).get_masterTableView();
    PageMethods.DeleteMessage(messageId);
}

$jQuery(document).ready(function () {
    if ($jQuery("[id$=dkAdminOrderWidget]")[0] != undefined)
        $jQuery("[id$=dkAdminOrderWidget]").find(".rdTop")[0].title = "This widget displays a summary of month to date orders by order status";
    if ($jQuery("[id$=dkVerificationSummaryWidget]")[0] != undefined)
        $jQuery("[id$=dkVerificationSummaryWidget]").find(".rdTop")[0].title = "This widget displays the age of requirements that are pending verification";
    if ($jQuery("[id$=dkRecentMsgs]")[0] != undefined)
        $jQuery("[id$=dkRecentMsgs]").find(".rdTop")[0].title = "Read recently received messages";
    if ($jQuery("[id$=dkOrderHistory]")[0] != undefined)
        $jQuery("[id$=dkOrderHistory]").find(".rdTop")[0].title = "Get details on your past orders";
    if ($jQuery("[id$=dkSubscriptions]")[0] != undefined)
        $jQuery("[id$=dkSubscriptions]").find(".rdTop")[0].title = "Get details on your subscriptions";
    if ($jQuery("[id$=dkUserProfile]")[0] != undefined)
        $jQuery("[id$=dkUserProfile]").find(".rdTop")[0].title = "View your information";
    if ($jQuery("[id$=dkMyTasks]")[0] != undefined)
        $jQuery("[id$=dkMyTasks]").find(".rdTop")[0].title = "View tasks you need to complete";
});


function NavigateTo(menuID, sender) {
    var hiddenField = $jQuery('[id$=hdnVisibleMenu]');
    var hdnSelectedMenu = $jQuery('[id$=hdnSelectedMenu]');
    $jQuery('[id$=DisplayChanged]').val('1');
    hiddenField.val(menuID);
    if (sender != undefined) {
        hdnSelectedMenu.val(sender.id);
        //debugger;
        if ($jQuery("#" + sender.id).closest(".nav_ComplDiv_Inner").length == 1) {
            $jQuery('[id$=hdnSelectedPkgText]').val(sender.innerHTML);
        }
        else {
            $jQuery('[id$=hdnSelectedPkgText]').val("");
        }

        if ($jQuery("#" + sender.id).closest(".nav_AdmnDiv_Inner").length == 1) {
            $jQuery('[id$=hdnSelectedAdmnPkgText]').val(sender.innerHTML);
        }
        else {
            $jQuery('[id$=hdnSelectedAdmnPkgText]').val("");
        }

        //Profile Sharing MenuId - 6
        if (menuID != undefined && menuID != null && (menuID == "6" || menuID == 6)) {
            $jQuery('[id$=hdnIsProfileSharingClicked]').val("1");
        }

        //Clinical Rotation MenuId - 7
        if (menuID != undefined && menuID != null && (menuID == "7" || menuID == 7)) {
            $jQuery('[id$=hdnIsClinicalRotationClicked]').val("1");
        }
        //Required Document MenuId - 8
        if (menuID != undefined && menuID != null && (menuID == "8" || menuID == 8)) {
            $jQuery('[id$=hdnRequiredDocument]').val("1");
        }
    }
}


function PlaceDashboardCommandButtons() {
    //debugger;
    var cmdbardata = $jQuery('[id$=dvDashboardCommands]').html();
    if (typeof window.parent != "undefined") {
        window.parent.document.getElementById("appmenuwr").innerHTML = cmdbardata;
    }
}

function Side_VideoTutorialNavigation(navUrl) {
    var win = window.open(navUrl, '_blank');
    if (win) {
        //Browser has allowed it to be opened
        win.focus();
    } else {
        //Broswer has blocked it
        //alert('Please allow popups for this site');
    }
}

$jQuery(document).ready(function () {
    $jQuery(".side_NavTabs").click(function (e) {
        //debugger;
        // $jQuery(this).find('a').trigger("click");
    });
});

function VisitRequiredDocument(url) {
    var win = window.open(url, '_blank');
    if (win) {
        //Browser has allowed it to be opened
        win.focus();
    } else {
        //Broswer has blocked it
        //alert('Please allow popups for this site');
    }
}

//UAT-3161
function VisitRotRequiredDocument(url) {
    var win = window.open(url, '_blank');
    if (win) {
        //Browser has allowed it to be opened
        win.focus();
    } else {
        //Broswer has blocked it
        //alert('Please allow popups for this site');
    }
}


function handleRadComboUI() {
    $jQuery('div[id$=pnLower]').on("scroll", function () {
        $jQuery.each($jQuery('.RadComboBox').toArray(), function (index, obj) {
            $find(obj.id).hideDropDown();
        });
    });
}

function handleRadPickerUI() {
    $jQuery('div[id$=pnLower]').on("scroll", function () {
        $jQuery.each($jQuery('.RadCalendarPopup').toArray(), function (index, obj) {
            obj.style.display = "none";
        });
    });
}

function OpenReportPopup() {
    var composeScreenWindowName = "Report Viewer";
    var fromScreenName = "PortfolioSearch";
    var url = $jQuery('[id$="hdnSubscriptionIds"]').val();
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName });
    return false;
}

function OpenCmplReportPopupOnSingleClick(subscriptionId, tenantId) {
    var url = $page.url.create("~/ComplianceOperations/Reports/ImmunizationSummaryReport.aspx?tid=" + tenantId + "&psid=" + subscriptionId);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var doc_win = $window.createPopup(url, { size: "760,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move }, function () { this.set_title("Compliance Tracking Summary Report"); this.set_destroyOnClose(true); });
    return false;
}

function openStartHerePopUp(PackageId, PackageName, tenantId) {
    var composeScreenWindowName = "StartHere";
    var url = "ComplianceOperations/Pages/DataEntryHelp.aspx?TenantId=" + tenantId + "&PackageId=" + PackageId + "&PackageName=" + PackageName;
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName });
    return false;
}

function setPostBackSourceMD(event, source) {
    if (window.DashboardChildClick == undefined || window.DashboardChildClick == 0) {
        $jQuery('.postbacksource').val('MD');
    }
    else {
        window.DashboardChildClick = 0;
    }
}


