

$jQuery(".rtlCollapse").hide();

function CheckAll(id) {
    var treeView = $find($jQuery("[id$=treeListFeature]")[0].id);
    var row = treeView.get_dataItems();
    if (row != undefined && row != []) {
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            $jQuery("input[id$='chkFeature']")[i].checked = isChecked;
        }
    }
    $jQuery("[id$=hdnIfHeaderChecked]")[0].value = id.checked;
}

function ManageChild(obj) {
    $jQuery("." + $jQuery("#" + obj.id).parent("span").attr("fieldIndex") + " input:checkbox").each(function () {
        if (this.id.length > 0) {
            $jQuery("input[id$=" + this.id + "]")[0].checked = obj.checked;
            ManageChild(this);
        }
    });
}

function ManageParent(obj) {
    var checked = false;
    if (obj == undefined)
        return;
    if ($jQuery("#" + obj.id).parent("span").attr("fieldIndex").length == 0)
        return;
    var isSibiling = false;

    $jQuery('[fieldIndex="' + $jQuery("input[id$=" + obj.id + "]").parent("span").attr("parent") + '"]').children("input").each(function () {
        if (this.id.length > 0) {
            //here check all sibling is checked or unchecked
            var parent = $jQuery("#" + obj.id).parent("span").attr("parent");

            if (parent != undefined) {
                if (parent.length > 0) {
                    $jQuery('[parent="' + parent + '"]').children("input").each(function () {
                        if (this.checked) {
                            if (this.id != obj.id) {
                                isSibiling = true;
                                return;
                            }
                        }
                        if (isSibiling == 0 && !obj.checked) {
                            $jQuery("input[id$=" + this.id + "]")[0].checked = obj.checked;
                        }
                    }
            );
                }
            }
            // end sibling check
            if (!isSibiling && obj.checked) {
                $jQuery("input[id$=" + this.id + "]")[0].checked = obj.checked;
            }
            if (!isSibiling && !obj.checked) {
                $jQuery("input[id$=" + this.id + "]")[0].checked = obj.checked;

            }
            ManageParent(this);
        }
    }
    );

    //logic for checking/unchecking header
    var treeView = $find($jQuery("[id$=treeListFeature]")[0].id);
    var checkHeader = true;
    var row = treeView.get_dataItems();
    if (row != undefined && row != []) {
        for (var i = 0; i < row.length; i++) {
            checkHeader = $jQuery("input[id$='chkFeature']")[i].checked;
            if (checkHeader == false)
                break;
        }
    }
    $jQuery("input[id$='chkSelectAllFeature']")[0].checked = checkHeader;

}

function CheckAllUserType(id) {
    var masterTable = $find($jQuery("[id$=grdBlock]")[0].id).get_masterTableView();
    var row = masterTable.get_dataItems();
    var isChecked = false;
    if (id.checked == true) {
        var isChecked = true;
    }
    for (var i = 0; i < row.length; i++) {
        masterTable.get_dataItems()[i].findElement("chkUserType").checked = isChecked;
    }
}

function UnCheckAllUserTypeHeader(id) {
    var checkHeader = false;
    if (!id.checked) {
        $jQuery('[id$=chkAllUserType]')[0].checked = checkHeader;
    }

}

function CheckAllRoles(id) {
    var masterTable = $find($jQuery("[id$=grdRoleDetail]")[0].id).get_masterTableView();
    var row = masterTable.get_dataItems();
    var isChecked = false;
    if (id.checked == true) {
        var isChecked = true;
    }
    for (var i = 0; i < row.length; i++) {
        masterTable.get_dataItems()[i].findElement("chkRoles").checked = isChecked;
    }
}

function UnCheckRoleHeader(id) {
    var checkHeader = false;
    if (!id.checked) {
        $jQuery('[id$=chkAllRoles]')[0].checked = checkHeader;
    }
}




