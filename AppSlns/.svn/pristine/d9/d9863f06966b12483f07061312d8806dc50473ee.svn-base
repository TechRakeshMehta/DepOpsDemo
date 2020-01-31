///<reference path="/Resources/Generic/ref.js"/>
//ScriptName: TransferRules.js


function openUsersPopUpWindow(senderId) {

    var parentPage = GetRadWindow().BrowserWindow;
    var parentRadWindowManager = parentPage.GetRadWindowManager();

    var oWnd = "";    
    var path = 'Messaging/UserControl/MessagingCompany.aspx?senderId=' + senderId;
    var url = $page.url.create(path);

    oWnd = parentRadWindowManager.open(url, "RadWindow77");

    if (oWnd != "") {
        window.setTimeout(function () {
            oWnd.setActive(true);
        }, 0);
    }
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function populateSelectedUsers(toList, isUserGroup, toListIds, senderId, combinedList) {

    var tempList = combinedList.split(',');
    var autoCompleteBox = $find($jQuery("[id*=autxSelectusers]")[0].id);

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

function returnToParent(sender, args) {
    try {
        $page.get_window().set_destroyOnClose(false);
        $page.get_window().close();
    } catch (e) {

    }
}

var e_showaddresslist = function (sender, args) {
    //var cmbProgram = $findByKey("cmbPrograms");//Commented by Sachin Singh for flexible hierarchy.
    var cmbTenant = $findByKey("cmbInstitutions");

    var tenantId = cmbTenant._value;
    //Commented by Sachin Singh for flexible hierarchy.
    //cmbProgram._value;
    var programId = "0"; 
    var url = $page.url.create("AddressLookup.aspx?parentScreen=transferRules&tenantId=" + tenantId + "&programId=" + programId);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (80 / 100);

    var win = $window.createPopup(url, { size: "600,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload });
}

function setUsers(toUsers, ccUsers) {
    var autoCompleteBox;

    var entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
    for (var i = 0; i < toUsers.get_count(); i++) {
        //autoCompleteBox = $find($jQuery("[id*=autxSelectusers]")[0].id);
        autoCompleteBox = $findByKey("autxSelectusers");
        entry.set_value(toUsers._array[i]._value);
        entry.set_text(toUsers._array[i]._text);
        autoCompleteBox.get_entries().add(entry);
        entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
    }

    //    for (var i = 0; i < ccUsers.get_count(); i++) {
    //        autoCompleteBox = $find($jQuery("[id*=autxSelectusers]")[0].id);
    //        entry.set_value(ccUsers._array[i]._value);
    //        entry.set_text(ccUsers._array[i]._text);
    //        autoCompleteBox.get_entries().add(entry);
    //        entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
    //    }
}