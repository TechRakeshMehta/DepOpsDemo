
var checkbox;
function ClickRole(obj, usergroupid, roleid) {
    $jQuery("#pageMsgBox").fadeOut();
    checkbox = obj;
    if (!obj.checked) {
        PageMethods.IsMappedRole(roleid,usergroupid, onSuccess)
    }
}

function onSuccess(result) {
    if (result) {
        checkbox.checked=true;
        $page.showMessage("Role can not be unassigned, It is used by user of this group.");
    }
}