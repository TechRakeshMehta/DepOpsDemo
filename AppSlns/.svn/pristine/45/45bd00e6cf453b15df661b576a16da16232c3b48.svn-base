///<reference path="/Resources/Generic/ref.js"/>
//ScriptName: Messaging.js
//UsedBy: default

var interval = 0;


if (typeof (IWEBLIB) != "undefined" && IWEBLIB) {
    //    //Page Events - Page init
    //    $page.add_pageInit(function () {
    //        $page.app.leftPanel.collapse();
    //    });

    //    //Page Events - Page unload
    //    $page.add_pageUnload(function () {
    //        $page.app.leftPanel.expand();
    //    });

}


////Isolated functions
//$jQuery().ready(function ($jQuery) {
//    alert('1');
//    //Code to reset counter on ajax stop
//    $jQuery(document).ajaxStop(function () {
//        alert('2');
//        if (typeof (parent.Page) != "undefined" && typeof (parent.Page.hideProgress) != "undefined") {
//            alert('3');
//            if (($jQuery("[id$=hdntimeout]")) != null) {
//                alert('4');
//                var timeout = $jQuery("[id$=hdntimeout]").val();
//                parent.StartCountDown(timeout);
//            }
//        }
//    });
//});


var targetControlID;
var targetNavigationURL;


var data = new Array();
var openedMsgs = new Array();
var flag;
var senderObj;
var messageId = 0;
var folderId = 2;
var folderCode = "MSGINB";
var column = '';
var moveToFolderId = 2;
var moveToFolderCode = "MSGINB";
var treeView = "";
var newFolderName = "";
var IsEventTriggeredOnDeleteOrMove = false;
var lastActivePageIndex = 0;
var lastSortingExpression = ""
var communicationTypeId = "";
var tableviewMessageMove = "";
var groupFolderTreeView = "";
var editFlag = false;
var selectedFolderName = "";
var messageIdArray = [];
var sendFromUserId = 0;
var groupID = 0;
var from = "";
var to = "";
var body = "";
var subject = "";
var folderCodeArray = new Array("MSGPNF", "MSGSNT", "MSGFUP", "MSGJNK");
var hdnIsApplicantAllowToSendMessages = "";
//bug: 9348 - changing event from ready to page init complete due to IE8 issue
$page.add_pageInitComplete(function () {
    var tableView = $find(FSObject.$("[id$=RadGrid1]")[0].id).get_masterTableView();
    tableviewMessageMove = $find(FSObject.$("[id$=RadGrid1]")[0].id).get_masterTableView();
    //    if (($jQuery("[id$=cmbUsergroup]")[0]) != null) {
    //        userGroup = ($jQuery("[id$=cmbUsergroup]")[0]).control.get_value();
    //    }
    //    if (userGroup == "") {
    userGroup = 0;
    //    }

    treeView = $find(FSObject.$("[id$=treePersonalFolders]")[0].id);
    var focusfolder = $jQuery("[id$=hdnFocusFolder]");

    if (focusfolder.val() == "") {
        var node = treeView.findNodeByValue("2#MSGINB");

        selectedFolderName = node.get_text().split('(')[0];
        node.select();
    }
    else {
        var node = treeView.findNodeByValue(focusfolder.val());
        folderId = focusfolder.val().split('#')[0];
        folderCode = focusfolder.val().split('#')[1];
        selectedFolderName = node.get_text().split('(')[0];
        node.select();
        node.get_parent().set_expanded(true);
    }
    PageMethods.GetData(0, tableView.get_pageSize(), tableView.get_sortExpressions().toString(), tableView.get_filterExpressions().toList(), folderId, folderCode, currentUserID, userGroup, queueType, from, to, subject, body, updateGrid);
    EnableDisableToolBarReplyForwardDeleteButton("tlbMessageMain", false);
    focusfolder.val("");
});


//The function is called every time the grid is updated or refreshed.
function updateGrid(result) {
    var tableView = $find($jQuery("[id$=RadGrid1]")[0].id).get_masterTableView();
    tableView.set_dataSource(result);
    tableView.dataBind();
    var sortExpressions = tableView.get_sortExpressions();
    var items = tableView.get_dataItems();
    var HasAttachmentControl = null;
    var imporantMailControl = null;
    var restoreButtonControl = null;
    var followUpControl = null;
    DisableReadFlagButton();
    tableView.clearSelectedItems();
    for (var i = 0; i < items.length; i++) {

        //if (folderId == 1 || folderId >= 7) {
        if (folderCode != 'MSGSNT') {
            if (items[i]._dataItem.IsUnread == true) {
                items[i].get_element().style.fontWeight = 'bold';
            }
            else {
                items[i].get_element().style.fontWeight = 'normal';
            }
            //            tableView.hideColumn(tableView.getColumnByUniqueName("CommunicationType").get_element().cellIndex);
        }
        else {
            items[i].get_element().style.fontWeight = 'normal';
        }
        followUpControl = items[i].findElement("imgMailFlagImageButton");

        if (followUpControl != null) {

            if (items[i]._dataItem.IsFollowUp == true) {

                followUpControl.setAttribute('class', 'on');
                followUpControl.attributes["src"].value = "/Resources/Mod/Messaging/Images/mailFlagRed.gif"
            }
            else {
                followUpControl.setAttribute('class', 'off');
                followUpControl.attributes["src"].value = "/Resources/Mod/Messaging/Images/mailFlag.gif"
            }
        }

        HasAttachmentControl = items[i].findElement("imgHasAttachment");

        if (HasAttachmentControl != null) {
            if (items[i]._dataItem.HasAttachment == true) {

                HasAttachmentControl.style.visibility = 'visible';
            }
            else {
                HasAttachmentControl.style.visibility = 'hidden';
            }
        }
        imporantMailControl = items[i].findElement("imgImportantMailButton");
        if (imporantMailControl != null) {
            if (items[i]._dataItem.IsHighImportant == false) {
                imporantMailControl.style.visibility = 'hidden';
            }
            else {
                imporantMailControl.style.visibility = 'visible';
            }
        }


        restoreButtonControl = items[i].findElement("imgRestoreButton");
        if (restoreButtonControl != null) {
            if (folderCode != 'MSGDEL') {
                restoreButtonControl.style.visibility = 'hidden';
            }
            else {
                restoreButtonControl.style.visibility = 'visible';
            }
        }
    }
    if (result.length > 0) {
        if (tableView.get_currentPageIndex() == 0 && folderCode == 'MSGINB' && (sortExpressions == "" || sortExpressions == "ReceivedDate DESC")) {
            var ctr = $jQuery("#hdnLastGetData");
            ctr.val(result[0].ReceivedDateString);
        }

        tableView.set_virtualItemCount(result[0].TotalRecords);
    }
    else {

        tableView.set_virtualItemCount(0);
    }
    if (items[0] != undefined && (tableView.get_selectedItems() == 0)) {
        items[0].set_selected(true);
    }
    else {
        $jQuery("iframe").contents().has('#msgBodyContent').contents().has("#msgBodyContent").html("");
    }
    Page.hideProgress();
    updateFolderCount();
    $jQuery(".messaging").show();
}

function RadGrid1_Command(sender, args) {
    args.set_cancel(true);
    var pageSize = sender.get_masterTableView().get_pageSize();
    PageMethods.SetGridPageSize(pageSize);
    var sortExpressions = sender.get_masterTableView().get_sortExpressions();
    var filterExpressions = sender.get_masterTableView().get_filterExpressions();
    var currentPageIndex = sender.get_masterTableView().get_currentPageIndex();

    if (args.get_commandName() == "Filter" || args.get_commandName() == "Sort") {
        currentPageIndex = 0;
        sender.get_masterTableView().set_currentPageIndex(0);
    }
    if (IsEventTriggeredOnDeleteOrMove == false) {
        var userGroup = "";
        if (($jQuery("[id$=cmbUsergroup]")[0]) != null) {
            userGroup = ($jQuery("[id$=cmbUsergroup]")[0]).control.get_value();
        }
        if (userGroup == "") {
            userGroup = 0;
        }
        if (groupID != 0) {
            if (sortExpressions.get_count() == 0) {
                sortExpressions = "ReceivedDate DESC";
            }
            PageMethods.GetUserGroupQueueData(currentPageIndex * pageSize, pageSize, sortExpressions.toString(), filterExpressions.toList(), groupID, currentUserID, from, to, subject, body, updateGrid);
        }
        else {
            PageMethods.GetData(currentPageIndex * pageSize, pageSize, sortExpressions.toString(), filterExpressions.toList(), folderId, folderCode, currentUserID, userGroup, queueType, from, to, subject, body, updateGrid);
        }
    }
    else {
        IsEventTriggeredOnDeleteOrMove = false;
    }


}
function RadGrid1_RowDataBound(sender, args) {
}

function formatJSONDate(result) {
    $jQuery.each(result, function () {
        this.ReceivedDate = new Date(parseInt(this.ReceivedDate.substr(6)));
    });
}



function updateGrid123(result) {
    if (result != null && result.length > 0) {

        var ctr = $jQuery("#hdnLastGetData");
        ctr.val(result[0].ReceivedDateString);
        senderObj.show();
        var tableView = $find($jQuery("[id$=RadGrid1]")[0].id).get_masterTableView();
        var sortExpressions = tableView.get_sortExpressions().toString();
        if (folderCode == 'MSGINB' && tableView.get_currentPageIndex() == 0 && (sortExpressions == "" || sortExpressions.t == "ReceivedDate DESC")) {
            var userGroup = "";
            if (($jQuery("[id$=cmbUsergroup]")[0]) != null) {
                userGroup = ($jQuery("[id$=cmbUsergroup]")[0]).control.get_value();
            }
            if (userGroup == "") {
                userGroup = 0;
            }
            PageMethods.GetData(0, tableView.get_pageSize(), tableView.get_sortExpressions().toString(), tableView.get_filterExpressions().toList(), folderId, folderCode, currentUserID, userGroup, queueType, updateGrid);
        }
    }

}

function ShowMessageDetails(sender, args) {

    if (folderCode == 'MSGDRF') {
        var userGroup = "";
        if (($jQuery("[id$=cmbUsergroup]")[0]) != null) {
            userGroup = ($jQuery("[id$=cmbUsergroup]")[0]).control.get_value();
        }
        if (userGroup == "") {
            userGroup = 0;
        }
        //        window.radopen("UserControl/Reply.aspx?messageID=" + messageId + "&action=5&queueType=" + queueType + "&currentUserId=" + currentUserID, "replyID");
        var url = $page.url.create("UserControl/Reply.aspx?messageID=" + messageId + "&action=5&queueType=" + queueType + "&currentUserId=" + currentUserID + "&userGroupId=" + userGroup + "&cType=" + sender.get_masterTableView().get_selectedItems()[0]._dataItem.CommunicationTypeCode);
        var oWnd = radopen(url, "RadWindow1");
    }
    else {
        var url = $page.url.create("UserControl/MessageDetails.aspx?messageID=" + sender.get_masterTableView().get_selectedItems()[0]._dataItem.MessageDetailID + "&queueType=" + queueType + "&currentUserId=" + currentUserID + "&cType=" + sender.get_masterTableView().get_selectedItems()[0]._dataItem.CommunicationTypeCode);
        //  fnOpenPopup(url);
        var oWnd = radopen(url, "RadWindow1");
        PageMethods.GetMessageContent(sender.get_masterTableView().get_selectedItems()[0]._dataItem.MessageDetailID, currentUserID, queueType, UpdateContent);
    }
}



function RemoveBold(sender, args) {
    var item = args.get_item();
    messageId = item._dataItem.MessageDetailID;
    if (messageId != 0) {
        PageMethods.GetMessageContent(item._dataItem.MessageDetailID, currentUserID, queueType, UpdateContent);
    }

    flag = true;
}

function RowSelected(sender, args) {
    var item;
    var countIfMessageFromSystem;
    var topIndex = 0, prevTopIndex = 0;
    if (sender.get_masterTableView().get_selectedItems().length > 0) {
        EnableDisableToolBarReplyForwardDeleteButton("tlbMessageMain", true);

        for (var i = 0; i < sender.get_masterTableView().get_selectedItems().length; i++) {
            topIndex = parseInt(sender.get_masterTableView().get_selectedItems()[i]._itemIndexHierarchical, 10);
            if (i == 0) {
                item = sender.get_masterTableView().get_selectedItems()[i];
                messageId = item._dataItem.MessageDetailID;
                prevTopIndex = topIndex;
            } else if (topIndex <= prevTopIndex) {
                item = sender.get_masterTableView().get_selectedItems()[i];
                messageId = item._dataItem.MessageDetailID;
                prevTopIndex = topIndex;
            }

        }
        UpdateReadFlagButton(sender, args);
    }
    if (sender.get_masterTableView().get_selectedItems().length > 1) {
        EnableDisableToolBarReplyForwardButton("tlbMessageMain", false);
    }

    if (folderCode == 'MSGDRF') {
        EnableDisableToolBarReplyForwardButton("tlbMessageMain", false);
    }
    if (folderCode == 'MSGDRF' || folderCode == 'MSGDEL') {
        DisableReadFlagButton();
    }
    if (folderCode.length < 6) {
        DisableReadFlagButton();
    }
    if (folderCode == 'MSGSNT') {
        var toolbar = findRadControlByServerId("tlbMessageMain");
        var splitButton = toolbar.findItemByText("More");
        var buttonRead = splitButton.get_buttons().getButton(0);
        buttonRead.set_text("Mark As Read");
        buttonRead.set_enabled(false);
        EnableDisableToolBarReplyReplyAllButton("tlbMessageMain", false)
    }
    // Disable the MoveToFolder for few Default Folders
    var toolbar = findRadControlByServerId("tlbMessageMain");

    if (folderCode == 'MSGINM' || folderCode == 'MSGEML' || folderCode == 'MSGINM' || folderCode == 'MSGEML' || folderCode == 'MSGSMS' ||
     folderCode == 'MSGNTI' || folderCode == 'MSGOTB' || folderCode == 'MSGSNT' || folderCode == 'MSGDEL' || folderCode == 'MSGNTE'
      || folderCode == 'MSGSRF' || folderCode == 'MSGDRF' || folderCode == 'MSGFUP' || folderCode.length < 6) {
        toolbar.findItemByValue("MoveToFolders").set_enabled(false);
    }
    else {
        toolbar.findItemByValue("MoveToFolders").set_enabled(true);
    }

    if (sender.get_masterTableView().get_selectedItems().length = 1) {
        if (sender.get_masterTableView().get_selectedItems()[0]._dataItem.FromUserId == systemCommunicationUserId
        || sender.get_masterTableView().get_selectedItems()[0]._dataItem.FromUserId == backgroundProcessUserId) {
            EnableDisableToolBarReplyReplyAllButton("tlbMessageMain", false)
        }
    }
    $find($jQuery("[id$=WclPaneContent]")[0].id).set_contentUrl("Message.aspx?messageID=" + messageId);
    //PageMethods.GetMessageContent(item._dataItem.MessageDetailID, currentUserID, queueType, UpdateContent);
    flag = true;
}

/* 
This function is fire when the raw is deseleced.
this methods mark the message as read on basis of different scenario.
1. Should not be the first message of the queue when loaded for the fist time.
2. If queue is changed then the selected index message of previous queeue is not marked read.
3. If user marks the message read or unread then for first time deselecting the message is not marked read.
4. On deleting unread message in numbers the message are not marked as read.
5. If message is restored then the restored message should not be marked as read if it is unread.
*/
//05E67970-A4D2-40EA-A193-619D62987FE9

function RowDeselecting(sender, args) {
    var item = sender.get_selectedItems();
    // Check for condition 1,2,3,5.
    var IsDeleted = $jQuery("[id$=hdnIsRead]").val();
    // Check for condition 4.
    var contains = $jQuery.inArray(item[0]._dataItem.MessageDetailID, messageIdArray);
    if ((folderCode != 'MSGSNT') && (item[0]._dataItem.IsUnread == true) && (item.length < 2) && (IsDeleted == "0") && (contains == -1)) {
        PageMethods.UpdateReadStatus(item[0]._dataItem.MessageDetailID, currentUserID, queueType, 0, UpdateReadStatus);
        item[0]._dataItem.IsUnread = false;
        item[0].get_element().style.fontWeight = 'normal';
    }
    else if (IsDeleted != "0") {
        $jQuery("[id$=hdnIsRead]").val(IsDeleted - 1);
    }
    else if (contains == -1) {
        messageIdArray = [];
    }
}

function RowDeselected(sender, args) {

    var item;
    var topIndex = 0, prevTopIndex = 0;


    if (sender.get_masterTableView().get_selectedItems().length > 0) {

        for (var i = 0; i < sender.get_masterTableView().get_selectedItems().length; i++) {
            topIndex = parseInt(sender.get_masterTableView().get_selectedItems()[i]._itemIndexHierarchical, 10);
            if (i == 0) {
                item = sender.get_masterTableView().get_selectedItems()[i];
                messageId = item._dataItem.MessageDetailID;
                prevTopIndex = topIndex;
            } else if (topIndex <= prevTopIndex) {
                item = sender.get_masterTableView().get_selectedItems()[i];
                messageId = item._dataItem.MessageDetailID;
                prevTopIndex = topIndex;
            }

        }
        PageMethods.GetMessageContent(item._dataItem.MessageDetailID, currentUserID, queueType, UpdateContent);
        UpdateReadFlagButton(sender, args);
    }

    else {
        DisableReadFlagButton();
        UpdateContent("");
    }

    if (sender.get_masterTableView().get_selectedItems().length < 2) {
        EnableDisableToolBarReplyForwardDeleteButton("tlbMessageMain", true);
    }
    if (sender.get_masterTableView().get_selectedItems().length == 0) {
        EnableDisableToolBarReplyForwardDeleteButton("tlbMessageMain", false);
    }
    if (folderCode == 'MSGDRF') {
        EnableDisableToolBarReplyForwardButton("tlbMessageMain", false);
    }
}

/*
Code optimized to reduce the line of code. 
The code is used to set the flag buttons as read,unread, flag etc.
*/
function UpdateReadFlagButton(sender, args) {
    var countRead = 0;
    var countUnread = 0;
    var countFlag = 0;
    var countUnflag = 0;
    for (var i = 0; i < sender.get_masterTableView().get_selectedItems().length; i++) {
        if (sender.get_masterTableView().get_selectedItems()[i]._dataItem.IsUnread == true) {
            countUnread++;
        }
        else {
            countRead++;
        }

        if (sender.get_masterTableView().get_selectedItems()[i]._dataItem.IsFollowUp == true) {
            countFlag++;
        }
        else {
            countUnflag++;
        }
    }
    var toolbar = findRadControlByServerId("tlbMessageMain");
    var splitButton = toolbar.findItemByText("More");
    var buttonRead = splitButton.get_buttons().getButton(0);
    var buttonFlag = splitButton.get_buttons().getButton(1);
    if (countRead > 0 && countUnread > 0) {
        buttonRead.set_text("Mark As Read");
        buttonRead.set_enabled(false);
    }
    else if (countRead > 0 && countUnread == 0) {
        buttonRead.set_text("Mark As Unread");
        buttonRead.set_enabled(true);
        buttonRead.set_commandName("Unread");
    }
    else if (countRead == 0 && countUnread > 0) {
        buttonRead.set_text("Mark As Read");
        buttonRead.set_enabled(true);
        buttonRead.set_commandName("Read")
    }
    if (countFlag > 0 && countUnflag > 0) {
        buttonFlag.set_text("Mark As Flagged");
        buttonFlag.set_enabled(false);
    }
    else if (countFlag > 0 && countUnflag == 0) {
        buttonFlag.set_text("Mark As Unflagged");
        buttonFlag.set_enabled(true);
        buttonFlag.set_commandName("Unflag");
    }
    else if (countFlag == 0 && countUnflag > 0) {
        buttonFlag.set_text("Mark As Flagged");
        buttonFlag.set_enabled(true);
        buttonFlag.set_commandName("Flag")
    }
}

function DisableReadFlagButton() {
    var toolbar = findRadControlByServerId("tlbMessageMain");
    var splitButton = toolbar.findItemByText("More");
    var buttonRead = splitButton.get_buttons().getButton(0);
    var buttonFlag = splitButton.get_buttons().getButton(1);
    buttonRead.set_text("Mark As Read");
    buttonRead.set_enabled(false);
    buttonFlag.set_text("Mark As Flagged");
    buttonFlag.set_enabled(false);
}
function UpdateReadStatus() {
    updateFolderCount();
}

function UpdateContent(result) {
    $jQuery("#msgBody").html(result);
}


function PrintMessage(messageContent) {
    var tempMessageWindow = window.open('', 'message', 'height=1,width=1');
    tempMessageWindow.document.write('<html><head><title>Message Details</title>');
    //tempMessageWindow.document.write('<link rel="stylesheet" href="main.css" type="text/css" />');
    tempMessageWindow.document.write('</head><body >');
    tempMessageWindow.document.write(messageContent);
    tempMessageWindow.document.write('</body></html>');
    var printDocument = tempMessageWindow.document;
    printDocument.execCommand("Print");
    tempMessageWindow.close();

    return true;
}
function SetUserGroup() {
    var userGroup = "";
    if (($jQuery("[id$=cmbUsergroup]")[0]) != null) {
        userGroup = ($jQuery("[id$=cmbUsergroup]")[0]).control.get_value();
    }
    if (userGroup == "") {
        userGroup = 0;
    }
    return userGroup;
}


function ForwardMessage(messageId, queueType, currentUserID, userGroup, communicationTypeCode) {

    var tableView = $find($jQuery("[id$=RadGrid1]")[0].id).get_masterTableView();
    if (tableView.get_selectedItems().length == 1) {
        var url = $page.url.create("UserControl/Reply.aspx?messageID=" + messageId + "&action=4&queueType=" + queueType + "&currentUserId=" + currentUserID + "&userGroupId=" + userGroup + "&cType=" + communicationTypeCode);
        var oWnd = radopen(url, "RadWindow1");
    }
}

// 0 : Delete from the main UI, 1 : Delete from the Pop up window
function DeleteMessage(deleteFrom) {
    var tableView = $find($jQuery("[id$=RadGrid1]")[0].id).get_masterTableView();
    var pageSize = tableView.get_pageSize();
    var sortExpressions = tableView.get_sortExpressions();
    var filterExpressions = tableView.get_filterExpressions();
    for (var i = 0; i < tableView.get_selectedItems().length; i++) {
        var cellID = tableView.get_selectedItems()[i]._dataItem.MessageDetailID
        // To keep track of deleted messages. To prevent then from Marked as read if unread
        messageIdArray[i] = cellID;
        if (groupID != 0) {
            if (sortExpressions.get_count() == 0) {
                sortExpressions = "ReceivedDate DESC";
            }
            PageMethods.DeleteGroupMessageAndUpdateResult(0, pageSize, sortExpressions.toString(), filterExpressions.toList(), cellID, groupID, currentUserID, from, to, subject, body, updateGrid);
        }
        else {
            PageMethods.DeleteMessageAndUpdateResult(0, pageSize, sortExpressions.toString(), filterExpressions.toList(), folderId, folderCode, cellID, currentUserID, SetUserGroup(), queueType, from, to, subject, body, updateGrid);
        }
        $jQuery("#msgBody").html("");
    }
    //For deleting only one item from the queue.
    if (messageIdArray[1] == undefined) {
        $jQuery("[id$=hdnIsRead]").val(tableView.get_selectedItems().length);
    }
}

function onSingleClick(sender, e) {
    if (ResetEditing(sender, e))
        handleSingleClick(sender, e.get_node());
}

function handleSingleClick(tree, node) {
    clearSearchboxValues();
    fn_hideSearchBox();
    $jQuery("#suscriptionSetting").hide();
    $jQuery("#messagingGrid").show();
    EnableDisableToolBarButtons("tlbMessageMain", true);
    EnableDisableToolBarReplyForwardDeleteButton("tlbMessageMain", false);
    groupFolderTreeView = $find(FSObject.$("[id$=treeGroupFolders]")[0].id);
    groupFolderTreeView.unselectAllNodes();
    groupID = 0;
    folderId = node.get_value();
    selectedFolderName = node.get_text().split('(')[0];
    var folderValue = node.get_value().split("#");
    folderId = folderValue[0];
    if (folderValue.length > 0) {
        folderCode = folderValue[1];
    }
    else {
        folderCode = "";
    }
    messageId = 0;
    UpdateContent("");
    var tableView = $find($jQuery("[id$=RadGrid1]")[0].id).get_masterTableView();
    tableView._filterExpressions.clear();

    IsEventTriggeredOnDeleteOrMove = false;
    // tableView.sort("ReceivedDate DESC");
    tableView.set_currentPageIndex(0);
    var column = tableView.getColumnByUniqueName("Date");
    Page.showProgress("Processing...");
    var userGroup = "";
    if (($jQuery("[id$=cmbUsergroup]")[0]) != null) {
        userGroup = ($jQuery("[id$=cmbUsergroup]")[0]).control.get_value();
    }
    if (userGroup == "") {
        userGroup = 0;
    }
    PageMethods.GetData(0, tableView.get_pageSize(), "ReceivedDate DESC", tableView.get_filterExpressions().toList(), folderId, folderCode, currentUserID, userGroup, queueType, from, to, subject, body, updateGrid);
    // tableView.rebind();
    SetColumnsforFolder();
    ResetTimer();
    // If queue is changed then the selected index message of previous queeue is not marked read.
    $jQuery("[id$=hdnIsRead]").val("1");
}



function SetColumnsforFolder() {

    var tableView = $find($jQuery("[id$=RadGrid1]")[0].id).get_masterTableView();
    //If Folder is  Sent
    if (folderCode == 'MSGSNT') {
        tableView.showColumn(tableView.getColumnByUniqueName("To").get_element().cellIndex);
        tableView.hideColumn(tableView.getColumnByUniqueName("From").get_element().cellIndex);
        tableView.showColumn(tableView.getColumnByUniqueName("flag").get_element().cellIndex);
    }
        // If Folder is  Draft    
    else if (folderCode == 'MSGDRF') {
        tableView.showColumn(tableView.getColumnByUniqueName("To").get_element().cellIndex);
        tableView.hideColumn(tableView.getColumnByUniqueName("From").get_element().cellIndex);
        tableView.hideColumn(tableView.getColumnByUniqueName("flag").get_element().cellIndex);

    }
        // If Folder is  Delete    
    else if (folderCode == 'MSGDEL') {
        tableView.showColumn(tableView.getColumnByUniqueName("To").get_element().cellIndex);
        tableView.showColumn(tableView.getColumnByUniqueName("From").get_element().cellIndex);
        tableView.hideColumn(tableView.getColumnByUniqueName("flag").get_element().cellIndex);
    }
        //If Folder is  Delete    
    else if (folderCode == 'MSGINB') {
        tableView.hideColumn(tableView.getColumnByUniqueName("To").get_element().cellIndex);
        tableView.showColumn(tableView.getColumnByUniqueName("From").get_element().cellIndex);
        tableView.showColumn(tableView.getColumnByUniqueName("flag").get_element().cellIndex);
    }
    else {
        tableView.hideColumn(tableView.getColumnByUniqueName("To").get_element().cellIndex);
        tableView.showColumn(tableView.getColumnByUniqueName("From").get_element().cellIndex);
        tableView.showColumn(tableView.getColumnByUniqueName("flag").get_element().cellIndex);
    }
}

function UserGroupChanged(sender, args) {
    //sender represents the combobox that has fired the event
    //the code below obtains the item that has been changed
    var item = args.get_item();
    //moveToFolderId = item != null ? item.get_value() : sender.get_value();    
    var userGroup = item != null ? item.get_value() : sender.get_value();
    var tableView = $find(FSObject.$("[id$=RadGrid1]")[0].id).get_masterTableView();
    PageMethods.GetData(0, tableView.get_pageSize(), tableView.get_sortExpressions().toString(), tableView.get_filterExpressions().toList(), folderId, folderCode, currentUserID, userGroup, queueType, updateGrid);

    // PageMethods.GetUpdatedFolder(currentUserID, userGroup, newAddedFolder, failAddedFolder);
    // __doPostBack();

}


function newAddedFolder(result) {
    treeView = $find(FSObject.$("[id$=RadTreeView1]")[0].id);
    treeView._nodeData = result;
    treeView.set_nodeData(result);
    treeView.commitChanges();
    //    FSObject.$("[id$=RadTreeView1]")[0].dataSrc = result;
    //    treeView.set_dataSource(result);
    //        tableView.dataBind();


    //    result.foreach(  
    //    var node = new Telerik.Web.UI.RadTreeNode();
    //    node.set_text(obj.MessageFolderName);
    //    if (typeof (__newFolderImg) != "undefined" && __newFolderImg) {
    //        node.set_imageUrl(__newFolderImg);
    //    }
    //    treeView.trackChanges();
    //    treeView.get_nodes().add(node);
    //    treeView.commitChanges();   
    //    )
    treeView.repaint();
    var node = treeView.findNodeByValue("2#MSGINB");
    node.select();
    alert("Success");
}

function SetCommunicationType(sender, args) {

    //sender represents the combobox that has fired the event
    //the code below obtains the item that has been changed
    var item = args.get_item();
    var param = item != null ? item.get_value() : sender.get_value();
    var communicationTypeSelectedValue = param.split("#");
    communicationTypeId = communicationTypeSelectedValue[0];

    if (communicationTypeId == 'Compose') {
        communicationTypeId = 'CT01';
    }
    else {
        if (communicationTypeSelectedValue.length > 0) {
            communicationTypeId = communicationTypeSelectedValue[1];
        }
        else {
            communicationTypeId = "";
        }
        if (communicationTypeSelectedValue[0] == "0") {
            communicationTypeId = "";
        }
    }
}

function SetMoveToFolder(sender, args) {
    //sender represents the combobox that has fired the event
    //the code below obtains the item that has been changed
    var item = args.get_item();
    //moveToFolderId = item != null ? item.get_value() : sender.get_value();    
    var param = item != null ? item.get_value() : sender.get_value();
    var moveTofolderValue = param.split("#");
    moveToFolderId = moveTofolderValue[0];
    if (moveTofolderValue.length > 0) {
        moveToFolderCode = moveTofolderValue[1];
    }
    else {
        moveToFolderCode = "";
    }
}

function ChgFlag(index, control) {

    if (folderCode != 'MSGDRF' && folderCode != 'MSGDEL') {
        var grid = $find($jQuery("[id$=RadGrid1]")[0].id);
        var MasterTable = grid.get_masterTableView();
        var items = MasterTable.get_dataItems();
        var cell = MasterTable.getCellByColumnUniqueName(items[index], "MessageDetailID");

        var isFollowUp = 0;
        if (control.getAttribute('class') == 'on') {

            control.setAttribute('class', 'off');
            control.attributes["src"].value = "/Resources/Mod/Messaging/Images/mailFlag.gif"
        }
        else if (control.getAttribute('class') == 'off') {
            isFollowUp = 1;
            control.setAttribute('class', 'on');
            control.attributes["src"].value = "/Resources/Mod/Messaging/Images/mailFlagRed.gif"
        }
        PageMethods.UpdateFollowUpStatus(cell.innerHTML, currentUserID, queueType, isFollowUp, folderCode, UpdateFlag);
        if (folderCode == 'MSGFUP') {
            // MasterTable.hideItem(index);
            MasterTable.rebind();
        }

    }
    return false;
}

function UpdateFlag(result) {

}

function RestoreMessage(index, control) {
    if (folderCode == 'MSGDEL') {
        var userGroup = "";
        if (($jQuery("[id$=cmbUsergroup]")[0]) != null) {
            userGroup = ($jQuery("[id$=cmbUsergroup]")[0]).control.get_value();
        }
        if (userGroup == "") {
            userGroup = 0;
        }
        var tableView = $find($jQuery("[id$=RadGrid1]")[0].id).get_masterTableView();
        var items = tableView.get_dataItems();
        PageMethods.CheckIfFolderNeedToBeRestored(items[index]._dataItem.MessageDetailID, currentUserID, queueType, RestoreMessageFolder);
        $jQuery("[id$=hdnIsRead]").val(tableView.get_selectedItems().length);
        //PageMethods.RestoreMessageAndUpdateResult(0, tableView.get_pageSize(), tableView.get_sortExpressions().toString(), tableView.get_filterExpressions().toList(), folderId, folderCode, items[index]._dataItem.MessageDetailID, currentUserID, userGroup, queueType, updateGrid);
    }
    return false;
}

function RestoreMessageFolder(result) {
    var response = result.split('#')[0];
    var messageDetailID = result.split('#')[1];
    var restoreFolderId = String.format("{0}#{1}", result.split('#')[2], result.split('#')[3]);

    var tableView = $find($jQuery("[id$=RadGrid1]")[0].id).get_masterTableView();
    var items = tableView.get_dataItems();
    if (response == "False") {

        PageMethods.RestoreMessageAndUpdateResult(0, tableView.get_pageSize(), tableView.get_sortExpressions().toString(), tableView.get_filterExpressions().toList(), folderId, folderCode, messageDetailID, currentUserID, userGroup, queueType, from, to, subject, body, updateGrid);
    }
    else {

        $confirm("Restoring this message will lead to restoring of container folder and its parent folders. Do you want to continue?", function (cr) {
            if (cr) {
                $jQuery("[id$=hdnFocusFolder]").val(restoreFolderId);
                PageMethods.RestoreMessageAndUpdateResult(0, tableView.get_pageSize(), tableView.get_sortExpressions().toString(), tableView.get_filterExpressions().toList(), folderId, folderCode, messageDetailID, currentUserID, userGroup, queueType, from, to, subject, body, refreshPage);
            }
        }, 'Confirm Delete', true);
    }
}
function refreshPage(result) {
    if (result) {
        __doPostBack();
    }
}
function GetUpdates(result) {
    var tableView = $find($jQuery("[id$=RadGrid1]")[0].id);
    UpdateReadFlagButton(tableView, result);
    updateFolderCount();
}
function AddFolder() {
    var textBox = $find($jQuery("[id$=RadTextBox1]")[0].id);
    newFolderName = textBox.get_value();
    if (!newFolderName) {
        alert("Please specify the text for new folder.");
        return false;
    }

    var checkDuplicatenode = false;

    for (var i = 0; i < treeView.get_nodes().get_count() ; i++) {
        var node = treeView.get_nodes().getNode(i);
        if (trim(node.get_text().toUpperCase()) == trim(newFolderName.toUpperCase())) {
            checkDuplicatenode = true;
        }
    }

    if (checkDuplicatenode) {

        alert("Folder can not be duplicate.");
        return false;
    }

    var node = new Telerik.Web.UI.RadTreeNode();
    node.set_text(newFolderName);
    if (typeof (__newFolderImg) != "undefined" && __newFolderImg) {
        node.set_imageUrl(__newFolderImg);
    }
    treeView.trackChanges();
    treeView.get_nodes().add(node);
    treeView.commitChanges();
    Page.showProgress("Processing...");
    var userGroup = "";
    if (($jQuery("[id$=cmbUsergroup]")[0]) != null) {
        userGroup = ($jQuery("[id$=cmbUsergroup]")[0]).control.get_value();
    }
    if (userGroup == "") {
        userGroup = 0;
    }
    PageMethods.AddNewFolder(trim(newFolderName), currentUserID, userGroup, UpdateFolder);
    textBox.clear();
    __doPostBack();
    ResetTimer();
    return false;
}

function DeleteFolder(treenode) {
    $confirm("Are you sure you want to delete the selected folder and all its content" + " <br/><span style='font-weight:bolder'>Do you want to continue?</span><br/>&nbsp;", function (cr) {
        if (cr) {
            var tableView = $find($jQuery("[id$=RadGrid1]")[0].id).get_masterTableView();
            IsEventTriggeredOnDeleteOrMove = true;

            PageMethods.DeleteFolderAndMessage(folderId, currentUserID, queueType);

            //                var combo = $find(dropDownClientId);
            //                var comboItem = combo.findItemByValue(folderId + "#" + folderCode);
            //                if (comboItem != null) {
            //                    combo.trackChanges();
            //                    combo.get_items().remove(comboItem);
            //                    combo.commitChanges();
            //                }

            treeView = $find(FSObject.$("[id$=treePersonalFolders]")[0].id);
            var node = treenode.get_value();
            if (node != "undefined") {
                treeView.trackChanges();
                var selectedNode = treeView.get_selectedNodes()
                treenode.get_parent().get_nodes().remove(treenode);
                treeView.commitChanges();
                //var selectNode = treeView.findNodeByValue("2#MSGINB");
                //selectNode.select();
                //handleSingleClick(treeView, selectNode);
                __doPostBack();
            }
        }

    }, 'Confirm Delete', true);
}

function openWin(messageID, action) {
    var oWnd = window.radopen("UserControl/ReplyMail.aspx?MessageID=" + messageID + "&Action=" + action, "RadWindow1");
    return false;
}
function trim(text) {
    return text.replace(/^\s+|\s+$/g, "");
}

function UpdateFolder(data) {
    data = $jQuery.parseJSON(data);
    var node = treeView.findNodeByText(newFolderName);
    if (data.result == "success") {
        node.set_value(data.folderName);
        treeView.commitChanges();
    }
    else if (data.result == "duplicate") {
        $alert("A folder with this name previously created, please use another name.");
        remvoeDuplicateNode(treeView, node, true);
    }
    else {
        remvoeDuplicateNode(treeView, node, false);
    }

    editFlag = false;
    //        var combo = $find(dropDownClientId);
    //        var comboItem = new Telerik.Web.UI.RadComboBoxItem();
    //        comboItem.set_text(newFolderName);
    //        comboItem.set_value(result);

    //        combo.trackChanges();
    //        combo.get_items().add(comboItem);
    //        combo.commitChanges();
    //    }
    Page.hideProgress();
}

function OnClientClose(oWnd, args) {
    oWnd.set_title('');
}

function OnClientCloseWindow(save) {
    updateFolderCount();
}

var menu = null;
function MenuLoaded(s, e) {
    menu = s;
}

function RowContextMenu(sender, eventArgs) {
    var evt = eventArgs.get_domEvent();
    var index = eventArgs.get_itemIndexHierarchical();
    document.getElementById("radGridClickedRowIndex").value = index;
    sender.get_masterTableView().selectItem(sender.get_masterTableView().get_dataItems()[index].get_element(), true);
    menu.show(evt);
    evt.cancelBubble = true;
    evt.returnValue = false;

    if (evt.stopPropagation) {
        evt.stopPropagation();
        evt.preventDefault();
    }
}

var btnID = null;

function SysxButtonID(s) {

    btnID = s;
}

function RadMenu1_OnClicked(sender, eventArgs) {
    if (eventArgs._item._text == 'Edit') {

        $jQuery("input[id$='btnCreate_input']")[0].value = 'Update'
    }
}
var gridID;
function GridCreatedID(sender, event) {
    gridID = sender;
}



var fnOpenPopup = function (url) {
    if (url) {
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (80 / 100);

        $window.createPopup(url, function () {
            this.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
            this.setSize(650, popupHeight);
            this.set_destroyOnClose(true);
            this.center();
            this.set_openerElementID = "RadWindow1";
        });
    }
}

function findRadControlByServerId(Id) {
    var buttonId = $find($jQuery("[id$=" + Id + "]").attr('id'));
    return buttonId;
}

function EnableDisableToolBarReplyForwardButton(RadBtn, bool) {

    var toolbar = findRadControlByServerId(RadBtn);
    toolbar.findItemByText("Forward").set_enabled(bool);
    toolbar.findItemByText("Reply").set_enabled(bool);
    toolbar.findItemByText("Reply All").set_enabled(bool);
    toolbar.findItemByText("Print").set_enabled(bool);

}
function EnableDisableToolBarReplyForwardDeleteButton(RadBtn, bool) {

    var toolbar = findRadControlByServerId(RadBtn);
    toolbar.findItemByText("Print").set_enabled(bool);
    toolbar.findItemByText("Delete").set_enabled(bool);
    toolbar.findItemByText("Reply All").set_enabled(bool);
    toolbar.findItemByText("Reply").set_enabled(bool);
    toolbar.findItemByText("Forward").set_enabled(bool);
    toolbar.findItemByText("Move To").set_enabled(bool);
  
    if (($jQuery("[id$=hdnIsApplicantAllowToSendMessages]")) != null) {
        hdnIsApplicantAllowToSendMessages = $jQuery("[id$=hdnIsApplicantAllowToSendMessages]").val();
        if (hdnIsApplicantAllowToSendMessages == "false")
        {
            toolbar.findItemByText("Reply All").set_enabled(false);
            toolbar.findItemByText("Reply").set_enabled(false);
            toolbar.findItemByText("Forward").set_enabled(false);
        }
    }
}

function EnableDisableToolBarReplyReplyAllButton(RadBtn, bool) {
    var toolbar = findRadControlByServerId(RadBtn);
    toolbar.findItemByText("Reply").set_enabled(bool);
    toolbar.findItemByText("Reply All").set_enabled(bool);
}

var messaging = {
    defaultPopup: {
        size: "800, 600",
        behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize | Telerik.Web.UI.WindowBehaviors.Reload
    }
}

var btntoolbar_clicked = function (sender, args) {
    var button = args.get_item();
    top.button = button;
    var composeScreenWindowName = "composeScreen";
    //UAT-4179
    var fromScreenName = "CommunicationCenter";
    if (button == null) return;

    var command = button.get_commandName().toLowerCase();
    var tableView = $find($jQuery("[id$=RadGrid1]")[0].id).get_masterTableView();

    switch (command) {
        case "newmessage":
            SetCommunicationType(sender, args);
            var url = $page.url.create("Pages/WriteMessage.aspx?cType=" + communicationTypeId + "&SName=" + fromScreenName);
            //console.log(url);
            //            var win = $window.createPopup(url, messaging.defaultPopup);
            //Telerik.Web.UI.WindowBehaviors.Close | 
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Resize | Telerik.Web.UI.WindowBehaviors.Maximize, name: composeScreenWindowName, onclose: OnClientCloseWindow });
            win.add_command(function (sender, args) {
                if (typeof (sender.get_contentFrame().contentWindow.e_onWindowCommand) == "function") {
                    sender.get_contentFrame().contentWindow.e_onWindowCommand(sender, args);
                }
            });
            break;
        case "morefolders":
            var url = $page.url.create("Pages/MoveToFolders.aspx?selectedFolder=" + folderCode);
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (80 / 100);

            var win = $window.createPopup(url, { size: "300," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload });
            break;
        case "searchmail":
            fn_showSearchBox();
            break;
        case "managetemplate":
            var url = $page.url.create("Pages/ManageTemplate.aspx");
            //console.log(url);
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: "ManageTemplateScreen" });
            //var win = $window.createPopup(url, messaging.defaultPopup);
            win.add_command(function (sender, args) {
                if (typeof (sender.get_contentFrame().contentWindow.e_onWindowCommand) == "function") {
                    sender.get_contentFrame().contentWindow.e_onWindowCommand(sender, args);
                }
            });
            break;
        case "suscibeexternalemail":
            var ctr = $jQuery("#hdnSubscriptionStatus");
            if (ctr.val() == "true") {
                PageMethods.UpdateSubscriptionStatus(currentUserID, false, updateButtonText);
                ctr.val(false);
            }
            else {
                PageMethods.UpdateSubscriptionStatus(currentUserID, true, updateButtonText);
                ctr.val(true);
            }
            break;
        case "reply":
            if (messageId <= 0) break;
            OpenWriteMessageWindow(messageId, queueType, currentUserID, SetUserGroup(), tableView.get_selectedItems()[0]._dataItem.CommunicationTypeCode, 2, composeScreenWindowName);
            break;
        case "replyall":
            if (messageId <= 0) break;
            OpenWriteMessageWindow(messageId, queueType, currentUserID, SetUserGroup(), tableView.get_selectedItems()[0]._dataItem.CommunicationTypeCode, 3, composeScreenWindowName);
            break;
        case "forward":
            if (messageId <= 0) break;
            OpenWriteMessageWindow(messageId, queueType, currentUserID, SetUserGroup(), tableView.get_selectedItems()[0]._dataItem.CommunicationTypeCode, 4, composeScreenWindowName);
            break;
        case "read":
            for (var i = 0; i < tableView.get_selectedItems().length; i++) {
                tableView.get_selectedItems()[i]._dataItem.IsUnread = false;
                tableView.get_selectedItems()[i].get_element().style.fontWeight = 'normal';
                PageMethods.UpdateReadStatus(tableView.get_selectedItems()[i]._dataItem.MessageDetailID, currentUserID, queueType, 0, GetUpdates);
            }
            break;
        case "unread":
            for (var i = 0; i < tableView.get_selectedItems().length; i++) {
                tableView.get_selectedItems()[i]._dataItem.IsUnread = true;
                tableView.get_selectedItems()[i].get_element().style.fontWeight = 'bold';
                PageMethods.UpdateReadStatus(tableView.get_selectedItems()[i]._dataItem.MessageDetailID, currentUserID, queueType, 1, GetUpdates);
            }
            $jQuery("[id$=hdnIsRead]").val((tableView.get_selectedItems().length));
            break;
        case "flag":
            for (var i = 0; i < tableView.get_selectedItems().length; i++) {
                tableView.get_selectedItems()[i]._dataItem.IsFollowUp = true;
                var followUpControl = tableView.get_selectedItems()[i].findElement("imgMailFlagImageButton");
                followUpControl.setAttribute('class', 'on');
                followUpControl.attributes["src"].value = "/Resources/Mod/Messaging/Images/mailFlagRed.gif"
                PageMethods.UpdateFollowUpStatus(tableView.get_selectedItems()[i]._dataItem.MessageDetailID, currentUserID, queueType, 1, folderCode, GetUpdates);
            }
            break;
        case "unflag":
            for (var i = 0; i < tableView.get_selectedItems().length; i++) {
                tableView.get_selectedItems()[i]._dataItem.IsFollowUp = false;
                var followUpControl = tableView.get_selectedItems()[i].findElement("imgMailFlagImageButton");
                followUpControl.setAttribute('class', 'off');
                followUpControl.attributes["src"].value = "/Resources/Mod/Messaging/Images/mailFlag.gif"
                PageMethods.UpdateFollowUpStatus(tableView.get_selectedItems()[i]._dataItem.MessageDetailID, currentUserID, queueType, 0, folderCode, GetUpdates);
            }
            break;
        case 'delete':
            var message = "";
            if (tableView.get_selectedItems().length > 1) {
                message = tableView.get_selectedItems().length + " messages.";
            }
            else {
                message = " message.";
            }


            if (tableView.get_selectedItems().length > 0) {

                $confirm("Are you sure you want to delete the selected messages(s). " + " <br/><span style='font-weight:bolder'>Do you want to continue?</span><br/>&nbsp;", function (cr) {
                    if (cr) {
                        IsEventTriggeredOnDeleteOrMove = true;
                        DeleteMessage(0);
                    }
                }, 'Confirm Delete', true);

            }

            break;
        case 'rules':
            var url = $page.url.create("Pages/transferrules.aspx?messageID=0&action=1&queueType=" + queueType + "&currentUserId=" + currentUserID + "&userGroupId=" + userGroup);
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName, onclose: OnClientClose });
            break;
        case 'print':
            PrintMessage($jQuery("iframe").contents().has('#msgBodyContent').contents().has("#msgBodyContent").html());
            break;
        case 'moveto':
            pageSize = tableView.get_pageSize();
            var selectedItem = args.get_item();
            var param = selectedItem != null ? selectedItem.get_value() : sender.get_value();
            moveToFolderId = param.split("#")[0];
            moveToFolderCode = param.split("#")[1];

            for (var i = 0; i < tableView.get_selectedItems().length; i++) {
                var cellID = tableView.get_selectedItems()[i]._dataItem.MessageDetailID
                PageMethods.SetMoveToFolderAndUpdateResult(0, pageSize, tableView.get_sortExpressions().toString(), tableView.get_filterExpressions().toList(), folderId, folderCode, moveToFolderId, moveToFolderCode, cellID, currentUserID, userGroup, queueType, from, to, subject, body, updateGrid);
                messageIdArray[i] = cellID;
            }
            $jQuery("[id$=hdnIsRead]").val((tableView.get_selectedItems().length));
            break;
        default:
            break;

    }
    ResetTimer();
}


function OpenWriteMessageWindow(messageId, queueType, currentUserID, userGroup, communicationTypeCode, actionType, windowName, parent_window) {
    //UAT-4179
    var fromScreenName = "CommunicationCenter";
    var tableView = $find($jQuery("[id$=RadGrid1]")[0].id).get_masterTableView();
    var url = $page.url.create("Pages/WriteMessage.aspx?messageID=" + messageId + "&action=" + actionType + "&queueType=" + queueType + "&currentUserId=" + currentUserID + "&userGroupId=" + userGroup + "&cType=" + communicationTypeCode + "&SName=" + fromScreenName);
    // Telerik.Web.UI.WindowBehaviors.Close | 
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var child = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: windowName });
    child.parentView = this;
    //get parent
    if (typeof (parent_window) != "undefined") {
        var zin = parent_window.get_zindex();
        $jQuery(child.get_popupElement()).css("z-index", zin + 10);
    }
}

var fn_showSearchBox = function () {
    clearSearchboxValues();
    $jQuery("#m_msgsearch").slideToggle(function () {
        var nheight = "100%";
        if ($jQuery(this).is(':visible')) {
            var sheight = $jQuery(this).height();
            //console.log(sheight);
            var pheight = $jQuery(this).parents(".rspPane:first").height();
            nheight = (pheight - sheight) + "px";
        }

        $findByKey("grdmessage", function () {
            //var This = new Telerik.Web.UI.RadGrid();
            this.get_element().style.height = nheight;
            this.repaint();

        });
    });
}

var fn_hideSearchBox = function () {
    //    $jQuery("#m_msgsearch").slideToggle(function () {
    var nheight = "100%";
    if ($jQuery("#m_msgsearch").is(':visible')) {
        $jQuery("#m_msgsearch").hide();
        $findByKey("grdmessage", function () {
            //var This = new Telerik.Web.UI.RadGrid();
            this.get_element().style.height = nheight;
            this.repaint();
        });
    }
    //    });
}
var e_closeserachbox = function () {
    fn_showSearchBox();
    clearSearchboxValues();
    e_PerformSearch();
}

var e_PerformSearch = function () {
    from = findRadControlByServerId("txtFrom").get_textBoxValue();
    to = findRadControlByServerId("txtTo").get_textBoxValue();
    body = findRadControlByServerId("txtBody").get_textBoxValue();;
    subject = findRadControlByServerId("txtSubject").get_textBoxValue();
    var tableView = $find(FSObject.$("[id$=RadGrid1]")[0].id).get_masterTableView();
    var sortExpressions = "ReceivedDate DESC";
    if (groupID != 0) {
        PageMethods.GetUserGroupQueueData(0, tableView.get_pageSize(), sortExpressions.toString(), tableView.get_filterExpressions().toList(), groupID, currentUserID, from, to, subject, body, updateGrid);
    }
    else {
        PageMethods.GetData(0, tableView.get_pageSize().toString(), sortExpressions.toString(), tableView.get_filterExpressions().toList(), folderId, folderCode, currentUserID, userGroup, queueType, from, to, subject, body, updateGrid);
    }
}

function clearSearchboxValues() {
    findRadControlByServerId("txtFrom").clear();
    findRadControlByServerId("txtTo").clear();
    findRadControlByServerId("txtBody").clear();
    findRadControlByServerId("txtSubject").clear();
    from = "";
    to = "";
    body = "";
    subject = "";

}
var e_viewMessage = function (sender, args) {
    if (folderCode == 'MSGDRF') {
        var composeScreenWindowName = "composeScreen";
        var item = sender.get_masterTableView().get_selectedItems();
        if (item[0]._dataItem.IsUnread == true) {
            PageMethods.UpdateReadStatus(item[0]._dataItem.MessageDetailID, currentUserID, queueType, 0, UpdateReadStatus);
            item[0]._dataItem.IsUnread = false;
            item[0].get_element().style.fontWeight = 'normal';
        }
        OpenWriteMessageWindow(sender.get_masterTableView().get_selectedItems()[0]._dataItem.MessageDetailID, queueType, currentUserID, userGroup, sender.get_masterTableView().get_selectedItems()[0]._dataItem.CommunicationTypeCode, 5, composeScreenWindowName)
    }
    else {
        var item = sender.get_masterTableView().get_selectedItems();
        sendFromUserId = item[0]._dataItem.FromUserId;
        if ((folderCode != 'MSGSNT') && (item[0]._dataItem.IsUnread == true)) {
            PageMethods.UpdateReadStatus(item[0]._dataItem.MessageDetailID, currentUserID, queueType, 0, UpdateReadStatus);
            item[0]._dataItem.IsUnread = false;
            item[0].get_element().style.fontWeight = 'normal';
        }
        var url = $page.url.create("Pages/MessageViewer.aspx?messageID=" + sender.get_masterTableView().get_selectedItems()[0]._dataItem.MessageDetailID + "&cType=" + sender.get_masterTableView().get_selectedItems()[0]._dataItem.CommunicationTypeCode + "&isImportant=" + sender.get_masterTableView().get_selectedItems()[0]._dataItem.IsHighImportant
         + "&From=" + sender.get_masterTableView().get_selectedItems()[0]._dataItem.From + "&Date=" + sender.get_masterTableView().get_selectedItems()[0]._dataItem.ReceivedDateFormat);

        var win = $window.createPopup(url, messaging.defaultPopup, function () { this.set_destroyOnClose(true); });
    }
}

function onClientContextMenuShowing(sender, args) {
    var treeNode = args.get_node();
    treeNode.set_selected(true);
    //enable/disable menu items
    setMenuItemsState(args.get_menu().get_items(), treeNode);
}

function setMenuItemsState(menuItems, treeNode) {
    var nodeCode = treeNode.get_value().split('#')[1];
    for (var i = 0; i < menuItems.get_count() ; i++) {
        var menuItem = menuItems.getItem(i);
        switch (menuItem.get_value()) {
            case "Add":
                if (nodeCode == "MSGINB" || nodeCode == "MSGSNT" || nodeCode == "MSGDEL" || nodeCode == "MSGDRF" || nodeCode == "MSGFUP" || nodeCode == "MSGJNK") {
                    menuItem.set_enabled(false);
                }
                else {
                    menuItem.set_enabled(true);
                }
                break;
            case "Delete":
                if (nodeCode == "MSGPNF" || nodeCode == "MSGINB" || nodeCode == "MSGSNT" ||
                nodeCode == "MSGDEL" || nodeCode == "MSGDRF" || nodeCode == "MSGFUP" || nodeCode == "MSGJNK") {
                    menuItem.set_enabled(false);
                }
                else {
                    menuItem.set_enabled(true);
                }
                break;
            default:
                break;
        }
    }
}
function ClientNodeEdited(sender, eventArgs) {
    var node = eventArgs.get_node();

    var count = 0;
    var checkDuplicatenode = false;
    if (node.get_text().toUpperCase().trim() == "") {
        $alert("Please Specify the name for folder.");
        treeView.trackChanges();
        node.get_parent().get_nodes().remove(node);
        treeView.commitChanges();
        editFlag = false;
        return false;
    }

    for (var i = 0; i < treeView.get_allNodes().length; i++) {
        var treeNode = treeView.get_allNodes()[i].get_textElement().innerHTML;
        if (treeNode.toUpperCase().trim() == node.get_text().toUpperCase().trim()) {
            count++;
        }
    }
    if (count > 1) {
        checkDuplicatenode = true;
    }

    if (checkDuplicatenode) {
        $alert("A folder with this name already exists. Use another name.");
        remvoeDuplicateNode(treeView, node, true);
        editFlag = false;
        return false;
    }
    newFolderName = node.get_text();
    var parentFolderID = treeView.get_selectedNodes()[0]._getData().value.split('#')[0];
    PageMethods.AddNewFolder(node.get_text().trim(), currentUserID, 0, parentFolderID, UpdateFolder);

}
function remvoeDuplicateNode(treeView, node, showMessage) {
    //    if (showMessage) {
    //        $alert("A folder with this name already exists. Use another name.");
    //    }
    treeView.trackChanges();
    node.get_parent().get_nodes().remove(node);
    treeView.commitChanges();
}
function OnClientNodeEditing(sender, eventArgs) {
    var node = eventArgs.get_node();
    var textInput = node.get_inputElement();
    textInput.id = "newNodeTextInput";
    $jQuery('#' + textInput.id).keydown(function (event) {
        if (event.keyCode == '27') {
            alert('u pressed esc');
            treeView.trackChanges();
            nodeInEditMode.get_parent().get_nodes().remove(nodeInEditMode);
            treeView.commitChanges();
        }
    });

}

function OnClientKeyPressing(sender, eventArgs) {
}
function ResetEditing(sender, eventArgs) {
    var node = eventArgs.get_node();
    if (editFlag == false) {
        node.endEdit();
        return true;
    }
    return false;
}
function onClientContextMenuItemClicking(sender, args) {
    // treeView = $find(FSObject.$("[id$=treePersonalFolders]")[0].id);
    var menuItem = args.get_menuItem();
    var treeNode = args._node;
    menuItem.get_menu().hide();

    switch (menuItem.get_value()) {
        case "Add":
            var node = new Telerik.Web.UI.RadTreeNode();
            //node.set_text("");
            if (typeof (__newFolderImg) != "undefined" && __newFolderImg) {
                node.set_imageUrl(__newFolderImg);
            }
            //            treeView.trackChanges();

            treeView.get_selectedNode().get_nodes().add(node);
            // Sys.Application.add_load(node);
            treeView.get_selectedNode().set_expanded(true);
            editFlag = true;
            node.startEdit();
            //    Sys.Application.remove_load(node);

            break;
        case "Delete":
            folderId = args._node.get_value().split('#')[0];
            DeleteFolder(treeNode);
            break;
        default:
            break;
    }
}

function onSingleClickGroupFolder(sender, e) {
    clearSearchboxValues();
    fn_hideSearchBox();
    $jQuery("#suscriptionSetting").hide();
    $jQuery("#messagingGrid").show();
    EnableDisableToolBarButtons("tlbMessageMain", true);
    var userGroupID = e.get_node().get_value();
    // var selectedNode1 = treeView.get_selectedNodes();
    var tableView = $find(FSObject.$("[id$=RadGrid1]")[0].id).get_masterTableView();
    tableView.hideColumn(tableView.getColumnByUniqueName("flag").get_element().cellIndex);
    Page.showProgress("Processing...");
    DisableReadFlagButton();
    groupID = userGroupID.split('#')[0];
    folderCode = userGroupID.split('#')[1];
    treeView.unselectAllNodes();
    SetColumnsforFolder();
    PageMethods.GetUserGroupQueueData(0, tableView.get_pageSize(), "ReceivedDate DESC", tableView.get_filterExpressions().toList(), groupID, currentUserID, from, to, subject, body, updateGrid);
    ResetTimer();

};

function OnNodeEditStart(sender, args) {
    var treeView = $find(FSObject.$("[id$=treePersonalFolders]")[0].id);
    var nodeInEditMode = args.get_node();
    // get a reference to the INPUT area of the edited node
    var textInput = nodeInEditMode.get_inputElement();

    // set the width and id property of the INPUT
    // textInput.size = 15;
    textInput.id = "newNodeTextInput";

    // Add onclick event to text input box to handle ESC key (uses some JQuery)
    $jQuery('#' + textInput.id).keydown(function (event) {
        if (event.keyCode == '27') {
            treeView.trackChanges();
            nodeInEditMode.get_parent().get_nodes().remove(nodeInEditMode);
            treeView.commitChanges();
        }
    });
}

function updateMessagingGrid(tableView, folderId, folderCode, currentUserID, userGroup, queueType) {
    PageMethods.GetData(0, tableView.get_pageSize(), tableView.get_sortExpressions().toString(), tableView.get_filterExpressions().toList(), folderId, folderCode, currentUserID, userGroup, queueType, updateGrid);
}

//FolderName, folderID, foldercode, queueOwnerId
/*
This function will update 3 folder count.
*/
function updateFolderCount() {
    var i = 0;
    //while (treeView.get_allNodes()[0]._itemData[i] != undefined) {
    while (treeView.get_allNodes()[i] != undefined) {
        var node = treeView.get_allNodes()[i];
        var valOfNode = node._properties._data.value;
        var idAndCode = valOfNode.split("#");
        if ($jQuery.inArray(idAndCode[1], folderCodeArray) == -1) {
            if (node._itemData != undefined) {
                var count = 0;
                while (node._itemData[count] != undefined) {
                    getCountOfFolder(node._itemData[count].value);
                    count++;
                }
            }
            getCountOfFolder(valOfNode);
        }

        i++;
    }
}

function getCountOfFolder(valOfNode) {
    var idAndCode = valOfNode.split("#");
    if ($jQuery.inArray(idAndCode[1], folderCodeArray) == -1) {
        PageMethods.UpdateFolderCount("folderName", idAndCode[0], idAndCode[1], currentUserID, updateFolderName);
    }

}

//This function sets the name to the node.
function updateFolderName(result) {
    var param = result.split("-");
    var value = param[1] + "#" + param[2];
    treeView.findNodeByValue(value).set_text(param[0]);
}

function OnClientItemClicking(sender, args) {
    $jQuery("#suscriptionSetting").show();
    $jQuery("#messagingGrid").hide();
    EnableDisableToolBarButtons("tlbMessageMain", false);
    treeView.unselectAllNodes();
    groupFolderTreeView = $find(FSObject.$("[id$=treeGroupFolders]")[0].id);
    groupFolderTreeView.unselectAllNodes();
    ResetTimer();
}

function EnableDisableToolBarButtons(RadBtn, bool) {

    var toolbar = findRadControlByServerId(RadBtn);
    toolbar.findItemByText("Print").set_enabled(bool);
    toolbar.findItemByText("Delete").set_enabled(bool);
    toolbar.findItemByText("Reply All").set_enabled(bool);
    toolbar.findItemByText("Reply").set_enabled(bool);
    toolbar.findItemByText("Forward").set_enabled(bool);
    toolbar.findItemByText("Move To").set_enabled(bool);
    toolbar.findItemByText("More").set_enabled(bool);
}

function ResetTimer() {
    if (($jQuery("[id$=hdntimeout]")) != null) {
        var timeout = $jQuery("[id$=hdntimeout]").val();
        parent.StartCountDown(timeout);
    }
}