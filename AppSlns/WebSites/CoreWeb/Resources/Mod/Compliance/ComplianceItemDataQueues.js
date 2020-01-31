function CheckAll(id) {
    var masterTable = $find("<%= grdVerificationItemData.ClientID %>").get_masterTableView();
    var row = masterTable.get_dataItems();
    var isChecked = false;
    if (id.checked == true) {
        var isChecked = true;
    }
    for (var i = 0; i < row.length; i++) {
        masterTable.get_dataItems()[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
    }
}
function unCheckHeader(id) {
    var checkHeader = true;
    var masterTable = $find("<%= grdVerificationItemData.ClientID %>").get_masterTableView();
    var row = masterTable.get_dataItems();
    for (var i = 0; i < row.length; i++) {
        if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").checked)) {
            checkHeader = false;
            break;
        }
    }
    $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
}