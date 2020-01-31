///<reference path="/Resources/core/Ref.js" />

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
    var url = $page.url.create("AddressLookup.aspx?parentScreen=manageTemplate");
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (80 / 100);

    var win = $window.createPopup(url, { size: "600,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload });
}


var btntoolbar_clicked = function (a, args) {
    var button = args.get_item();
    top.button = button;
    if (button == null) return;

    var command = button.get_commandName().toLowerCase();

    switch (command) {
        case "highimportance":
            var currentValue = $jQuery("[id$=hdnMessageImportance]").val();
            if (currentValue == 'false')
                $jQuery("[id$=hdnMessageImportance]").val('true');
            else
                $jQuery("[id$=hdnMessageImportance]").val('false');
            break;

        case "save":

            var cmbToList = $find($jQuery("[id$=acbToList]")[0].id);
            var cmbCCList = $find($jQuery("[id$=acbCcList]")[0].id);
            var cmbBCCList = $find($jQuery("[id$=acbBccList]")[0].id);
            //sender.set_autoPostBack(false);
            //PageMethods.ValidateMail(cmbToList.get_text(), cmbCCList.get_text(), queueType, 0, 0, ValidateSave);
            ValidateSave("");
            break;
        case "send":

            var txtSubject = $find($jQuery("[id$=txtSubject]")[0].id);
            var txtBody = $find($jQuery("[id$=editorContent]")[0].id);
            var cmbToList = $find($jQuery("[id$=acbToList]")[0].id);
            var cmbCCList = $find($jQuery("[id$=acbCcList]")[0].id);
            var cmbBCCList = $find($jQuery("[id$=acbBccList]")[0].id);
            if ((cmbToList.get_text().trim() == "") & (cmbCCList.get_text().trim() == "")) {

                $alert("Please select the Recipient's email address.");
            }
            if ((txtSubject.get_textBoxValue().trim() == "")) {
                $alert("Subject is required.");
                //sender.set_autoPostBack(false);
                //return false;
            }
            if ((txtBody.get_text().trim() == "")) {
                $alert("Message Body is required.");
                //sender.set_autoPostBack(false);
                //return false;
            }
            break;
        default:
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

function setUsersForManageTemplate(toUsers, ccUsers,bccUsers) {
    var autoCompleteBox;

    var entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
    for (var i = 0; i < toUsers.get_count(); i++) {
        autoCompleteBox = $find($jQuery("[id*=acbToList]")[0].id);
        entry.set_value(toUsers._array[i]._value);
        entry.set_text(toUsers._array[i]._text);
        autoCompleteBox.get_entries().add(entry);
        entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
    }

    for (var i = 0; i < ccUsers.get_count(); i++) {
        autoCompleteBox = $find($jQuery("[id*=acbCcList]")[0].id);
        entry.set_value(ccUsers._array[i]._value);
        entry.set_text(ccUsers._array[i]._text);
        autoCompleteBox.get_entries().add(entry);
        entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
    }

    for (var i = 0; i < bccUsers.get_count(); i++) {
        autoCompleteBox = $find($jQuery("[id*=acbBccList]")[0].id);
        entry.set_value(bccUsers._array[i]._value);
        entry.set_text(bccUsers._array[i]._text);
        autoCompleteBox.get_entries().add(entry);
        entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
    }
}