///<reference path="/Resources/core/Ref.js" />
Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };
var e_editorpaneresize = function (sender, args) {
    var pheight = $jQuery(sender.get_element()).height();
    //    var nheight = $jQuery(sender.get_element()).height();
    //    console.log("resized");
    //    console.log($jQuery(sender.get_element()).find(".RadEditor").first().height());
    //console.log("=======");
    //console.log($jQuery(sender.get_element()).height());
    //    console.log($jQuery(sender.get_element()).find(".RadEditor").first().height());
    $jQuery(sender.get_element()).find(".RadEditor").first().height(pheight);
    $jQuery(sender.get_element()).find(".RadEditor table").first().height(pheight);
    //    console.log($jQuery(sender.get_element()).find(".RadEditor").first().height());
    //    $jQuery(sender.get_element()).find(".RadEditor").first().height(pheight);
    //    console.log(pheight);
    //    console.log($jQuery(sender.get_element()).find(".RadEditor").first().height());
}

var e_showaddresslist = function (sender, args) {
    /*Bug fix - Address popup not shown on top if this popup maximized*/
    var win = $page.get_window();
    if (win) {
        win.restore();
    }
    var url = $page.url.create("AddressLookup.aspx?parentScreen=composeMessage");
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url,
        { size: "700,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal },
        function () {
            //debugger;
            $page.get_window().set_destroyOnClose(true);
        });

    //UAT-3098
    setTimeout(function () {
        parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
    }, 750);

    parent.$jQuery('.rwTitlebarControls').on('click', function () {
        setTimeout(function () {
            parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
        }, 40);
    });

    //rwCloseButton
    //    window.focus();
    //    var zin = $page.get_window().get_zindex();
    //    $jQuery(win.get_popupElement()).css("z-index", zin + 10);

    //$(obj).attr('disabled', true);

}

var messageSentStatus = "";

var btntoolbar_clicked = function (a, args) {
    var button = args.get_item();
    top.button = button;
    if (button == null) return;

    var command = button.get_commandName().toLowerCase();

    switch (command) {
        case "highimportance":
            var currentValue = $jQuery("[id$=hdnMessageImportance]").val();
            if (currentValue == 'false') {
                button._setChecked(true);
                $jQuery("[id$=hdnMessageImportance]").val('true');
            }
            else {
                button._setChecked(false);
                $jQuery("[id$=hdnMessageImportance]").val('false');
            }

            args.set_cancel(true);
            break;

        case "savemessage":
            messageSentStatus = "saved";
            //closeWindow();
            break;
        case "send":

            var txtSubject = $find($jQuery("[id$=txtSubject]")[0].id);
            var txtBody = $find($jQuery("[id$=editorContent]")[0].id);
            var cmbToList = $find($jQuery("[id$=acbToList]")[0].id);
            var cmbCCList = $find($jQuery("[id$=acbCcList]")[0].id);

            //UAT-4179
            var cmbBCCList;
            if ($jQuery("[id$=acbBccList]") != null
                && $jQuery("[id$=acbBccList]") != undefined
                && $jQuery("[id$=acbBccList]").length > 0

                && $find($jQuery("[id$=acbBccList]")[0].id) != null
                && $find($jQuery("[id$=acbBccList]")[0].id) != undefined) {

                cmbBCCList = $find($jQuery("[id$=acbBccList]")[0].id);
            }
            var IsNeedToShowUsersInBCCInsteadOfTo;

            var hdnIsNeedToShowUsersInBCCInsteadOfTo = $jQuery("[id$=hdnIsNeedToShowUsersInBCCInsteadOfTo]");
            if (hdnIsNeedToShowUsersInBCCInsteadOfTo != undefined
                && hdnIsNeedToShowUsersInBCCInsteadOfTo != null
                && hdnIsNeedToShowUsersInBCCInsteadOfTo[0] != undefined
                && hdnIsNeedToShowUsersInBCCInsteadOfTo[0] != null
                && cmbBCCList != undefined) {
                IsNeedToShowUsersInBCCInsteadOfTo = hdnIsNeedToShowUsersInBCCInsteadOfTo[0].value;
            }

            if (IsNeedToShowUsersInBCCInsteadOfTo == "" || IsNeedToShowUsersInBCCInsteadOfTo == "false" || IsNeedToShowUsersInBCCInsteadOfTo == undefined) {
                if ((cmbToList.get_text().trim() == "") & (cmbCCList.get_text().trim() == "")) {

                    $alert("Please select the Recipient's email address.");
                    args.set_cancel(true);
                    return;
                }
            }
            else {
                if ((cmbToList.get_text().trim() == "") & (cmbCCList.get_text().trim() == "") & (cmbBCCList.get_text().trim() == "")) {

                    $alert("Please select the Recipient's email address.");
                    args.set_cancel(true);
                    return;
                }

            }
            if ((txtSubject.get_textBoxValue().trim() == "")) {
                $alert("Subject is required.");
                args.set_cancel(true);
                return;
            }
            if ((txtBody.get_text().trim() == "")) {
                $alert("Message Body is required.");
                args.set_cancel(true);
                return;
            }
            messageSentStatus = "sent";
          //  closeWindow();
            break;

        case "cancel":
            var hdnCount = $jQuery("[id$=hdnCount]").val();
            if ($jQuery("[id$=hdnIsSharedUser]").val().toLowerCase() != "shareduser") { //UAT-3098
                if (hdnCount != "") {                    
                    GetAddress1Info();
                }
                if (($jQuery("[id$=hdnIsSavedInDraft]").val() != undefined && $jQuery("[id$=hdnIsSavedInDraft]").val() != "") || (hdnCount != "")) {
                    alert("Your Message is saved in draft.");
                }
            }
            messageSentStatus = "canceled";
            closeWindow();
            args.set_cancel(true);
            return;
            break;
        default:
    }
}

function closeWindow() {
    var oArg = {};
    oArg.MessageSentStatus = messageSentStatus;
    $page.get_window().set_destroyOnClose(false);
    $page.get_window().close(oArg);
    var id = $jQuery("[id$=hdnIntervalId]").val();
    clearInterval(id);

}

function ValidateSave(result) {
    if (result != "") {
        alert(result);
    }
    else {
        //debugger;
        __doPostBack('ctl00$CommandContent$fsucCmdBar1$btnSave', '');
        $page.get_window().set_destroyOnClose(false);
        $page.get_window().close();
    }
}

function setUsers(toUsers, ccUsers, bccUsers) {
    var autoCompleteBox;
    var entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
    for (var i = 0; i < toUsers.get_count(); i++) {
        autoCompleteBox = $find($jQuery("[id*=acbToList]")[0].id);
        entry.set_value(toUsers._array[i]._value);
        entry.set_text(toUsers._array[i]._text);
        autoCompleteBox.get_entries().add(entry);
        entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
        textBoxKeyDown();
    }

    for (var i = 0; i < ccUsers.get_count(); i++) {
        autoCompleteBox = $find($jQuery("[id*=acbCcList]")[0].id);
        entry.set_value(ccUsers._array[i]._value);
        entry.set_text(ccUsers._array[i]._text);
        autoCompleteBox.get_entries().add(entry);
        entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
        textBoxKeyDown();
    }
    if (bccUsers != undefined && bccUsers != "") {
        for (var i = 0; i < bccUsers.get_count(); i++) {
            autoCompleteBox = $find($jQuery("[id*=acbBccList]")[0].id);
            entry.set_value(bccUsers._array[i]._value);
            entry.set_text(bccUsers._array[i]._text);
            autoCompleteBox.get_entries().add(entry);
            entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
            textBoxKeyDown();
        }
    }
}

// Code for the aspx page
$jQuery(document).ready(function () {
    disableKeyPress("acbToList");
    disableKeyPress("acbCcList");
    disableKeyPress("acbAttachedFiles");   
    if ($jQuery("[id$=hdnIsSharedUser]").val().toLowerCase() != "shareduser") {    //UAT-3098   
        var intervalId = setInterval(GetAddress1Info, 60000);
        $jQuery("[id$=hdnIntervalId]").val(intervalId);
    }
});

function disableKeyPress(control) {
    $jQuery("[id$=" + control + "]").on('keydown', function (e) {
        var key = e.charCode || e.keyCode;
        if (key != 9) {
            e.preventDefault();
        }
    });
}

function OnClientLoad(sender, args) {
    sender.attachEventHandler("onkeydown", function (e) {
        textBoxKeyDown();
    });
}

function textBoxKeyDown() {
    var hdnCount = $jQuery("[id$=hdnCount]").val();
    if (hdnCount == "") {
        $jQuery("[id$=hdnCount]").val("1");
    }
}

function GetAddress1Info() {
    var acbCcList = getIds("acbCcList");
    var acbToList = getIds("acbToList");
    var acbBccList = getIds("acbBccList");
    var acbCcNameList = getNames("acbCcList");
    var acbToNameList = getNames("acbToList");
    var acbBccNameList = getNames("acbBccList");
    //UAT-4179
    //var IsSenderNeededCopyOfMailInBCC = "false";
    //var hdnIsNededToShowCopyMeInMailCheckBox = $jQuery("[id$=hdnIsNededToShowCopyMeInMailCheckBox]").val();
    //if (hdnIsNededToShowCopyMeInMailCheckBox == "true") {
    //    if ($jQuery("[id$=chkIsCopyOfMailToSender]") != undefined
    //        && $jQuery("[id$=chkIsCopyOfMailToSender]")[0] != undefined
    //        && $jQuery("[id$=chkIsCopyOfMailToSender]")[0].checked == "true")
    //    {
    //        IsSenderNeededCopyOfMailInBCC = "true";
    //    }
    //}

    //var data = $find($jQuery("[id$=acbAttachedFiles]")[0].id)._children._array;
    var hdnOriginalDocmntNme = "";
    var documentsName = getIds("acbAttachedFiles");
    var subject = $jQuery("[id$=txtSubject]").val();
    var isReplyMode = $jQuery("[id$=hdnIsReplyMode]").val();
    var editorContent = $find($jQuery("[id$=editorContent]")[0].id)._document.body.innerHTML;
    var hdnCount = $jQuery("[id$=hdnCount]").val();
    var mainText = "save";
    if (hdnCount == "") {
        var mainText = $find($jQuery("[id$=editorContent]")[0].id)._document.body.innerText;
    }
    var hdnMessageValue = $jQuery("[id$=hdnMessageId]").val();
    var dataurl = "messageIdUser: '" + hdnMessageValue + "', toList : '" + acbToList + "', bccList:'" + acbBccList + "',ccList:'" + acbCcList + "',toListName : '" + acbToNameList + "', ccListName:'" + acbCcNameList + "',bccListName:'" + acbBccNameList + "', subject:'" + subject + "', content: '" + editorContent + "',documentName: '" + documentsName + "',originalDocumentName: '" + hdnOriginalDocmntNme + "', communicationType:'CT01' , messageType1: '0',mainText: '" + mainText + "',isReplyMode: '" + isReplyMode + "'";//+ "',IsSenderNeededCopyOfMailInBCC: '" + IsSenderNeededCopyOfMailInBCC + "'";
    $jQuery.ajax({
        type: "POST",
        url: "WriteMessage.aspx/AutoSaveData",
        data: "{ " + dataurl + " }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $jQuery("[id$=hdnMessageId]").val(msg.d);
            $jQuery("[id$=hdnIsSavedInDraft]").val(msg.d);
        }
    });
}

function getIds(element) {
    var stringOfIds = "";
    var count = 0;
    if ($jQuery("[id$=" + element + "]")[0] != undefined) {
        var data = $find($jQuery("[id$=" + element + "]")[0].id)._children._array;
        while (data[count] != undefined) {
            stringOfIds = stringOfIds + data[count]._value + ";";
            count = count + 1;
        }
    }
    return stringOfIds;
}

function getNames(element) {
    var stringOfNames = "";
    var count = 0;
    if ($jQuery("[id$=" + element + "]")[0] != undefined) {
        var data = $find($jQuery("[id$=" + element + "]")[0].id)._children._array;
        while (data[count] != undefined) {
            stringOfNames = stringOfNames + data[count]._text + ";";
            count = count + 1;
        }
    }
    return stringOfNames;
}

function onFileRemoved(obj) {
    var totalFileSize = 0;
    totalFileSize = GetTotalSize(obj, totalFileSize);
    $jQuery("[id$=hdnAllowedFileSize]").val(totalFileSize);
}

function onFileUploaded(obj, args) {
    var totalFileSize = 0;
    var allowedSizeInMB = 20;
    totalFileSize = GetTotalSize(obj, totalFileSize);

    if (totalFileSize == (allowedSizeInMB * 1024 * 1024)) {
        obj._cancelUpload(args.get_row());
        obj._updateCancelButton(args.get_row());
        $telerik.$(".ruRemove", args.get_row()).click();
        alert("Cannot attach file more than 20MB");
    }
    else {
        $jQuery("[id$=hdnAllowedFileSize]").val(totalFileSize);
    }
}

function Validationfailed(obj, args) {
    obj._cancelUpload(args.get_row());
    obj._updateCancelButton(args.get_row());
    $telerik.$(".ruRemove", args.get_row()).click();
    alert("Only .txt,.pdf,.doc,.docx,.xls,.xlsx,.zip,.jpg,.png,.bmp,.gif file formats are allowed.");
}

function GetTotalSize(obj, totalFileSize) {
    var count = 0;
    while ($jQuery(obj)[0]._uploadedFiles[count] != undefined) {
        totalFileSize = parseInt(totalFileSize) + parseInt($jQuery(obj)[0]._uploadedFiles[count].fileInfo.ContentLength);
        count++;
    }
    return totalFileSize;
}
