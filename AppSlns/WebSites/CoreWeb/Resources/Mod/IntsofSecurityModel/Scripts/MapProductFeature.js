

$jQuery(".rtlCollapse").hide();

function SelP(sender, feaureParentClientID) {
    var setVal = false;
    $jQuery(".css" + feaureParentClientID + " INPUT[@name=" + name + "][type='checkbox']").each(function () { if (this.checked == true) { setVal = true; } });

    $jQuery(".cssP" + feaureParentClientID + " INPUT[@name=" + name + "][type='checkbox']").each(function () { this.checked = setVal; });
}

function HandleChild(sender, ParentID) {
    $jQuery(".css" + ParentID + " INPUT[@name=" + name + "][type='checkbox']").each(function () { this.checked = sender.checked; });
}

function ValidatePermission(cur0, cur1) {
    document.getElementById(cur0.id).checked = false;
    document.getElementById(cur1.id).checked = false;
}