function IsPrefixNameExists(obj) {
    $jQuery("#pageMsgBox").fadeOut();
    PageMethods.IsPrefixNameExist(obj.value, onSuccess)
}
function onSuccess(result) {
    if (result) {
        if (result == true) {
            $page.showMessage("User Name prefix already exist.");
            $jQuery("#ctl00_DefaultContent_ucDynamicControl_grdTenant_ctl00_ctl02_ctl04_grdOrganizationUserNamePrefix_ctl00_ctl02_ctl04_txtPrefixName_text").focus()
        }
    }
}

function OnTenantTypeSelectedIndexChanged(sender, eventArgs) {
    var item = eventArgs.get_item();
    var connectionStringDiv = $jQuery(".currentconnDiv");

    if (item.get_text() == "Institution") {
        connectionStringDiv.show("slow");
        connectionStringDiv.find('input:text').val('');
        connectionStringDiv.find('input:password').val('');
    }
    else {
        connectionStringDiv.hide("slow");
        connectionStringDiv.find('input:text').val('tt');
        connectionStringDiv.find('input:password').val('tt');
    }

}