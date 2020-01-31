
$jQuery(".rtlCollapse").hide();

function ManageChild(obj) {

    $jQuery("." + $jQuery("#" + obj.id).parent("span").attr("fieldIndex") + " input:checkbox").each(function () {
        if (this.id.length > 0) {
            $jQuery("input[id$=" + this.id + "]").attr("checked", obj.checked);
            ManageChild(this);
        }
    });

}

function ManageParentCounty(obj) {
    var checked = false;
    if (obj == undefined)
        return;
    if ($jQuery("#" + obj.id).parent("span").attr("fieldIndex").length == 0)
        return;
    var isSibiling = false;
    var checkbox = $jQuery("input[id$=" + obj.id + "]");
    var isChecked = checkbox.is(":checked");
    if (isChecked) {
        var subParentCheckbox = $jQuery('[fieldIndex="' + checkbox.parent("span").attr("Parent") + '"].chkCouties').children("input[:checkbox]");
        subParentCheckbox.attr("checked", isChecked);

        var stateParentCheckbox = $jQuery('[fieldIndex="' + subParentCheckbox.parent("span").attr("Parent") + '"].chkStates').children("input[:checkbox]");
        stateParentCheckbox.attr("checked", isChecked);

    }
}

function ManageParentStates(obj) {
    var checked = false;
    if (obj == undefined)
        return;
    if ($jQuery("#" + obj.id).parent("span").attr("fieldIndex").length == 0)
        return;
    var isSibiling = false;
    var checkbox = $jQuery("input[id$=" + obj.id + "]");
    var isChecked = checkbox.is(":checked");
    if (isChecked) {
        
        var stateParentCheckbox = $jQuery('[fieldIndex="' + checkbox.parent("span").attr("Parent") + '"].chkStates').children("input[:checkbox]");
        stateParentCheckbox.attr("checked", isChecked);
    }
    else {
        var judgesCheckBox = $jQuery('[parent="' + checkbox.parent("span").attr("fieldindex") + '"].chkJudges').children("input[:checkbox]");
        judgesCheckBox.attr("checked",isChecked);
    }
}



function ManageChildCounties(obj) {
    var checked = false;
    if (obj == undefined)
        return;
    if ($jQuery("#" + obj.id).parent("span").attr("fieldIndex").length == 0)
        return;
    var isSibiling = false;
    var checkbox = $jQuery("input[id$=" + obj.id + "]");
    var isChecked = checkbox.is(":checked");
    if (isChecked) {
    }
    else {
        var countiesCheckBox = $jQuery('[parent="' + checkbox.parent("span").attr("fieldindex") + '"].chkCouties').children("input[:checkbox]");
        countiesCheckBox.attr("checked", isChecked);
        countiesCheckBox.each(function () {
            var judgesCheckBox = $jQuery('[parent="' + $jQuery(this).parent("span").attr("fieldindex") + '"].chkJudges').children("input[:checkbox]");
            judgesCheckBox.attr("checked", isChecked);
        });
        
        
        
    }
}
