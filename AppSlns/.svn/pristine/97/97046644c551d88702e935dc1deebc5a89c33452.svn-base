///// <reference path="/Resources/Generic/Ref.js"/>

var previousToList = "";
var latestMessage = "";
var previousCCList = "";
var isFirstTemplate = true;
var html = ''

function returnToParent(sender, args) {
    try {
        $page.get_window().set_destroyOnClose(false);
        $page.get_window().close();
    } catch (e) {

    }
}

function SendClientClick(sender, args) {
    var button = $jQuery("[id$=hdnSubmitButtonName]")[0];
    button.value = "Send";
    //bug: 9348 - commenting out following line due to IE8 issue
    //button.innerHTML = "Send";

    var cmbToList = $find($jQuery("[id$=acbToList]")[0].id);
    var cmbCCList = $find($jQuery("[id$=acbCcList]")[0].id);
    // INITIAL PHASE
    //    var cmbTenantType = $find($jQuery("[id$=cmbTenantType]")[0].id)
    //    var cmbMessageType = $find($jQuery("[id$=cmbMessageType]")[0].id)
    if ((cmbToList.get_text().trim() == "") && (cmbCCList.get_text().trim() == "")) {
        alert("There must be at least one name or distribution in the To or Cc Box.");
        sender.set_autoPostBack(false);
        return false;
    }
    // INITIAL PHASE
    //    else if (cmbTenantType._text == "--SELECT--") {
    //        alert("Please select receiver type.");
    //        sender.set_autoPostBack(false);
    //    }
    // INITIAL PHASE
    //    else if (cmbTenantType._text != "--SELECT--") {
    //   sender.set_autoPostBack(false);
    //  PageMethods.ValidateMail(cmbToList._text, cmbCCList._text, queueType, cmbTenantType._value, cmbMessageType._value, ValidateSend);
    //  }

    sender.set_autoPostBack(false);
    //PageMethods.ValidateMail(cmbToList.get_text(), cmbCCList.get_text(), queueType, 0, 0, ValidateSend);
}
function trim(text) {
    return text.replace(/^\s+|\s+$/g, "");
}
function ValidateEmailList(sender, args) {

    var cmbTenantType = $find($jQuery("[id$=cmbTenantType]")[0].id)
    var cmbToList = $find($jQuery("[id$=cmbToList]")[0].id);
    var cmbCCList = $find($jQuery("[id$=cmbCcList]")[0].id);
    var cmbMessageType = $find($jQuery("[id$=cmbMessageType]")[0].id)

    if (cmbTenantType._text != "--SELECT--") {
        //PageMethods.ValidateMail(cmbToList._text, cmbCCList._text, queueType, cmbTenantType._value, cmbMessageType._value, SendMessage);
    }
}


function ValidateCCList(sender, args) {
    var cmbTenantType = $find($jQuery("[id$=cmbTenantType]")[0].id)
    var cmbToList = $find($jQuery("[id$=cmbToList]")[0].id);
    var cmbCCList = $find($jQuery("[id$=cmbCcList]")[0].id);
    var cmbMessageType = $find($jQuery("[id$=cmbMessageType]")[0].id)

    if (cmbTenantType._text != "--SELECT--" && previousCCList != cmbCCList._text) {
        previousCCList = cmbCCList._text;
        //PageMethods.ValidateMail(cmbCCList._text, cmbToList._text, queueType, cmbTenantType._value, cmbMessageType._value, SendMessage);
    }
}


function ValidateToList(sender, args) {

    var cmbTenantType = $find($jQuery("[id$=cmbTenantType]")[0].id)
    var cmbToList = $find($jQuery("[id$=cmbToList]")[0].id);
    var cmbCCList = $find($jQuery("[id$=cmbCcList]")[0].id);
    var cmbMessageType = $find($jQuery("[id$=cmbMessageType]")[0].id)

    if (cmbTenantType._text != "--SELECT--" && cmbToList._text != previousToList) {
        previousToList = cmbToList._text;
        //PageMethods.ValidateMail(cmbToList._text, cmbCCList._text, queueType, cmbTenantType._value, cmbMessageType._value, SendMessage);
    }
}


function SendMessage(result) {
    if (result != "") {
        latestMessage = result;
        alert(result);
    }
    else {
        latestMessage = "";
    }
}

function ValidateSave(result) {

    if (result != "") {
        alert(result);
    }
    else {
        __doPostBack('ctl00$CommandContent$fsucCmdBar1$btnSave', '');
        $page.get_window().set_destroyOnClose(false);
        $page.get_window().close();
    }
}
function ValidateSend(result) {

    if (result != "") {
        alert(result);
    }
    else {
        __doPostBack('ctl00$CommandContent$fsucCmdBar1$btnSubmit', '');
        $page.get_window().set_destroyOnClose(false);
        $page.get_window().close();
    }
}
function SaveClientClick(sender, args) {

    var cmbToList = $find($jQuery("[id$=acbToList]")[0].id);
    var cmbCCList = $find($jQuery("[id$=acbCcList]")[0].id);
    sender.set_autoPostBack(false);
    //PageMethods.ValidateMail(cmbToList.get_text(), cmbCCList.get_text(), queueType, 0, 0, ValidateSave);
}

function SaveTemplateClientClick(sender, args) {
    sender.set_autoPostBack(false);
    var flag = false;

    if (flag == false) {
        $findByKey("TemplateName", function () {
            if (trim(this.get_value()) == "") {
                $alert("Please specify the name of the Template.");
                sender.set_autoPostBack(false);
                flag = true;
            }
        });
    }

    if (flag == false) {
        $findByKey("TemplateSubject", function () {
            if (trim(this.get_value()) == "") {
                $alert("Please specify subject for the Template.");
                sender.set_autoPostBack(false);
                flag = true;
            }
        });

    }
    if (flag == false) {
        $findByKey("templateBody", function () {
            if (trim(this.get_text()) == "") {
                $alert("Please specify the body of the Template.");
                sender.set_autoPostBack(false);
                flag = true;
            }
        });
    }
    if (flag == false) {
        var txtTemplateName = $find($jQuery("[id$=txtTemplateName]")[0].id);
        var cmbTemplates = $find($jQuery("[id$=cmbTemplates]")[0].id);
        PageMethods.GetUniqueTemplateName(txtTemplateName.get_textBoxValue().trim(), cmbTemplates._value, UniqueTemplateName);
    }
}

function UniqueTemplateName(templateName) {
    var txtTemplateName = $find($jQuery("[id$=txtTemplateName]")[0].id);

    if (txtTemplateName.get_textBoxValue().trim() != templateName) {
        var message = "This template name is already exists. Do you want to continue with template name \'" + templateName + "\'?";

        $confirm(message, function (cr) {
            if (cr) {
                __doPostBack('ctl00$CommandContent$fsucCmdBar1$btnSave', '');
                //$page.get_window().set_destroyOnClose(false);
                //$page.get_window().close();
            }
        });
    }
    else {
        __doPostBack('ctl00$CommandContent$fsucCmdBar1$btnSave', '');
        //        $page.get_window().set_destroyOnClose(false);
        //        $page.get_window().close();
    }
}

function CloseManageTemplatePopup(isInsert, isSaved) {
    if (isInsert && isSaved) {
        $alert("Template saved successfully.");
        $page.get_window().Reload();
    }
    else if (!isInsert && isSaved) {
        $alert("Template updated successfully.");
        $page.get_window().Reload();
    }
    else if (isInsert && !isSaved) {
        $alert("An error occurred while saving Template. Please contact support team.");
    }
    else if (!isInsert && !isSaved) {
        $alert("An error occurred while updating Template. Please contact support team.");
    }

    
    //set_destroyOnClose(false);
    //    $page.get_window().close();
}


function UpdateMessageContent(sender, args) {
    PageMethods.GetMessageContent(sender._value, currentUserId, queueType, UpdateEditor);
}

function UpdateEditor(result) {

    if (isFirstTemplate) {
        html = $find($jQuery("[id$=editorContent]")[0].id).get_html();
        isFirstTemplate = false;
    }
    var content = result + '' + html;
    $find($jQuery("[id$=editorContent]")[0].id).set_html(content);
}


function ShowHideDivisions(control, division1, division2) {
    FSObject.$("[id$=" + division1 + "]")[0].style.display = "block";
    FSObject.$("[id$=" + division2 + "]")[0].style.display = "none";

}


function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function openWin2(senderId) {
    var parentPage = GetRadWindow().BrowserWindow;
    var parentRadWindowManager = parentPage.GetRadWindowManager();
    var path = "";
    var url = "";
    var oWnd2 = "";

    // INITIAL PHASE
    //var cmbTenantType = $find($jQuery("[id$=cmbTenantType]")[0].id)
    //  var cmbMessageType = $find($jQuery("[id$=cmbMessageType]")[0].id)

    path = 'Messaging/UserControl/MessagingCompany.aspx?senderId=' + senderId;
    url = $page.url.create(path);
    oWnd2 = parentRadWindowManager.open(url, "RadWindow4");

    // INITIAL PHASE
    //    if (cmbMessageType._value != '0') {
    //        if (cmbTenantType._value == '1') {
    //            path = 'Messaging/UserControl/MessagingClientList.aspx?senderId=' + senderId + '&messageType=' + cmbMessageType._value;
    //            url = $page.url.create(path);
    //            oWnd2 = parentRadWindowManager.open(url, "RadWindow3");
    //        }
    //        else if (cmbTenantType._value == '2') {
    //            path = 'Messaging/UserControl/MessagingSupplierList.aspx?senderId=' + senderId + '&messageType=' + cmbMessageType._value;
    //            url = $page.url.create(path);
    //            oWnd2 = parentRadWindowManager.open(url, "RadWindow2");
    //        }
    //        else if (cmbTenantType._value == '3' && cmbTenantType.get_items().get_count() > 1) {
    //            path = 'Messaging/UserControl/MessagingCompany.aspx?senderId=' + senderId + '&messageType=' + cmbMessageType._value;
    //            url = $page.url.create(path);
    //            oWnd2 = parentRadWindowManager.open(url, "RadWindow4");
    //        }
    //        else if (cmbTenantType._value == '3' && cmbTenantType.get_items().get_count() == '1') {
    //            path = 'Messaging/UserControl/MessagingCompany.aspx?senderId=' + senderId + '&messageType=' + cmbMessageType._value + '&showgrouponly=true';
    //            url = $page.url.create(path);
    //            oWnd2 = parentRadWindowManager.open(url, "RadWindow4");
    //        }
    //    }

    if (oWnd2 != "") {
        window.setTimeout(function () {
            oWnd2.setActive(true);
        }, 0);
    }
    else {
        if (cmbTenantType._value == '0' && cmbMessageType._value == '0')
            alert("Please Select Receiver Type and Message Type");
        else if (cmbTenantType._value == '0' && cmbMessageType._value != '0')
            alert("Please Select Receiver Type");
        else if (cmbTenantType._value != '0' && cmbMessageType._value == '0')
            alert("Please Select Message Type");
    }
}

function populateSupplier(arg, toListIds, senderId) {
    var cmbToList = $find(senderId);
    cmbToList.set_text(arg);
    if ($jQuery("[id$=cmbToList]")[0].id == senderId) {
        FSObject.$("[id*=hdnToListIds]").val(toListIds);
    }
    else {
        FSObject.$("[id*=hdnCcListIds]").val(toListIds);
    }
}

function populateSelectedUsers(toList, isUserGroup, toListIds, senderId, combinedList) {
    //    var cmbToList = $find(senderId);
    //    cmbToList.set_text(toList.toString().replace(/,/g, ";"));

    // var tempList = toListIds.toString().replace(/,/g, ";")
    var tempList = combinedList.split(',');


    FSObject.$("[id*=hdnIsUserGroupforCompany]").val(isUserGroup);

    var autoCompleteBox;
    if ($jQuery("[id$=acbToList]")[0].id == senderId) {

        //        var initialValues = FSObject.$("[id*=hdnToListIds]").val();
        //        if (initialValues)
        //            initialValues += ";";

        //        FSObject.$("[id*=hdnToListIds]").val(initialValues + tempList);
        autoCompleteBox = $find($jQuery("[id*=acbToList]")[0].id);
    }
    else {
        //FSObject.$("[id*=hdnCcListIds]").val(toListIds);

        //        var initialValues = FSObject.$("[id*=hdnCcListIds]").val();
        //        if (initialValues)
        //            initialValues += ";";

        //        FSObject.$("[id*=hdnCcListIds]").val(initialValues + tempList);
        autoCompleteBox = $find($jQuery("[id*=acbCcList]")[0].id);
    }


    var entry = new Telerik.Web.UI.AutoCompleteBoxEntry();

    for (var i = 0; i < tempList.length; i++) {


        if (tempList[i].split(":")[0])
            entry.set_value(tempList[i].split(":")[0]);

        if (tempList[i].split(":")[1])
            entry.set_text(tempList[i].split(":")[1]);

        autoCompleteBox.get_entries().add(entry);
        entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
    }
}


function editorOnClientLoad(editor, args) {
    editor.attachEventHandler("onclick", function (e) {
        var sel = editor.getSelection().getParentElement(); //get the currently selected element
        var href = null;
        if (sel.tagName == "A") {
            href = sel.href; //get the href value of the selected link
            window.open(href);
            return false;
        }
    });
}
