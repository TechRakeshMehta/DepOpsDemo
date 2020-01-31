
var selectedUsers = [];
var selectedUserIds = []

function grdUsers_RowSelected(sender, args) {
    var orgID = args.getDataKeyValue("OrganizationUserID");
    var userName = args.getDataKeyValue("FirstName")+" "+ args.getDataKeyValue("LastName");
    selectedUsers.push(orgID + ":" + userName);
}
function grdUsers_RowDeselected(sender, args) {
    var orgID = args.getDataKeyValue("OrganizationUserID");
    var userName = args.getDataKeyValue("FirstName")+ " " +args.getDataKeyValue("LastName");

    var index = selectedUsers.indexOf(orgID + ":" + userName);
    selectedUsers.splice(index, 1);
}

function grdMessagingGroups_RowSelected(sender, args) {
    var messageGroupID = args.getDataKeyValue("ID");
    var groupName = args.getDataKeyValue("Name");
    var groupType = args.getDataKeyValue("Type");
    selectedUsers.push(messageGroupID + ":" + groupName + ":" + groupType);
}

function grdMessagingGroups_RowDeselected(sender, args) {
    var messageGroupID = args.getDataKeyValue("ID");
    var groupName = args.getDataKeyValue("Name");
    var groupType = args.getDataKeyValue("Type");
    var index = selectedUsers.indexOf(messageGroupID + ":" + groupName + ":" + groupType);
    selectedUsers.splice(index, 1);
}


function BindUsers(type) {
    var autoCompleteBox;
    if (type == 'toButton') {
        autoCompleteBox = $find($jQuery("[id*=acbToList]")[0].id);
    }
    else if (type == 'ccButton') {
        autoCompleteBox = $find($jQuery("[id*=acbCcList]")[0].id);
    }
    else {
        autoCompleteBox = $find($jQuery("[id*=acbBccList]")[0].id);
    }
    var entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
    for (var i = 0; i < selectedUsers.length; i++) {

        if (selectedUsers[i].split(":")[0]) {
            entry.set_value(selectedUsers[i].split(":")[0]);
        }
        if (selectedUsers[i].split(":")[2] != undefined) {
            entry.set_value(entry.get_value() + ":" + selectedUsers[i].split(":")[2]);
        }
        if (selectedUsers[i].split(":")[1])
            entry.set_text(selectedUsers[i].split(":")[1]);

        autoCompleteBox.get_entries().add(entry);
        entry = new Telerik.Web.UI.AutoCompleteBoxEntry();
    }
    autoCompleteBox = null;
    if ($jQuery("[id*=grdUsers]")[0] != null)
        $find($jQuery("[id*=grdUsers]")[0].id).clearSelectedItems();
    if ($jQuery("[id*=grdMessagingGroups]")[0] != null)
        $find($jQuery("[id*=grdMessagingGroups]")[0].id).clearSelectedItems();

}

function closeBindUsers() {

    var toUsers = $find($jQuery("[id*=acbToList]")[0].id).get_entries();
    var parentScreen = $jQuery("[id*=hdnParentScreen]")[0].value;
    var ccUsers = "";
    var bccUsers = "";
    if (parentScreen != "transferRules") {
        ccUsers = $find($jQuery("[id*=acbCcList]")[0].id).get_entries();
        if ($jQuery("[id*=acbBccList]")[0] != undefined) {
            bccUsers = $find($jQuery("[id*=acbBccList]")[0].id).get_entries();
        }
    }
    else {
        ccUsers = undefined;
    }

    if (parentScreen == "manageTemplate") {
        var screen = $page.get_window().get_windowManager().getWindowByName("ManageTemplateScreen");
        screen.get_contentFrame().contentWindow.setUsersForManageTemplate(toUsers, ccUsers, bccUsers);
    }
    else {
        var screen = $page.get_window().get_windowManager().getWindowByName("composeScreen");
        screen.get_contentFrame().contentWindow.setUsers(toUsers, ccUsers, bccUsers);
    }
    closeWindow();
}

function closeWindow() {
    $page.get_window().set_destroyOnClose(false);
    $page.get_window().close();
}

