function ValidateCheckBoxSelection(source, args) {
    var cntrlToValidate = $find($jQuery("[id$=cmbTenant]").attr("id"));
    var check = 0;
    if (cntrlToValidate) {
        var cntrlItems = cntrlToValidate.get_items();
        for (var i = 0; i <= cntrlItems.get_count() - 1; i++) {
            var cntrlItem = cntrlItems.getItem(i);
            if (cntrlItem.get_checked()) {
                check = 1;
            }
        }
    }
    if (check)
        args.IsValid = true;
    else
        args.IsValid = false;
}


function CheckAll(id) { 
    var masterTable = $find($jQuery("[id$=grdAgencies]")[0].id).get_masterTableView()
    var row = masterTable.get_dataItems();
    var isChecked = false;
    if (id.checked == true) {
        var isChecked = true;
    }
    for (var i = 0; i < row.length; i++) {
        masterTable.get_dataItems()[i].findElement("chk").checked = isChecked;
    }
}

function UnCheckHeader(id) {
    var checkHeader = true;
    var masterTable = $find($jQuery("[id$=grdAgencies]")[0].id).get_masterTableView()
    var row = masterTable.get_dataItems();
    for (var i = 0; i < row.length; i++) {
        if (!(masterTable.get_dataItems()[i].findElement("chk").checked)) {
            checkHeader = false;
            break;
        }
    }
    $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
}
