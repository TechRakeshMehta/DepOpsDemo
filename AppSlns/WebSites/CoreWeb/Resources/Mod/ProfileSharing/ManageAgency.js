function ValidateCheckBoxSelection(source, args) {
    var cntrlToValidate = $find($jQuery("[id$=ddlTenant]").attr("id"));
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

function ResetZipInvalidValidator() {
    if ($find($jQuery("[id$=txtZip]")[0].id).get_value() == "") {
        ResetCityComboBox();
    }
}


$page.add_pageLoad(function () {
    $jQuery("[id$=txtZip]").blur(function () {
        if ($find($jQuery("[id$=txtZip]")[0].id).get_value() == "") {
            DisableValidator($jQuery("[id$=rfvCity]")[0].id);
            ResetCityComboBox();
        }
        else {
            EnableValidator($jQuery("[id$=rfvCity]")[0].id);
        }
    });
});

function ResetCityComboBox() {
    var radcomboCity = $find($jQuery("[id$=cmbCity]")[0].id);
    var comboItems = radcomboCity.get_items();
    for (var i = 0; i < comboItems.get_count() ; i++) {
        var item = comboItems.getItem(i);
        if (item) {
            radcomboCity.get_items().remove(item);
        }
    }
    radcomboCity.clearItems();
    radcomboCity.clearCache();
    radcomboCity.commitChanges();
    radcomboCity.set_text('');
    radcomboCity.set_emptyMessage("--SELECT--");
    radcomboCity.commitChanges();
    //getCities($jQuery("[id$=txtZip]")[0].id, "", $jQuery("[id$=cmbCity]")[0].id, "", "", "", $jQuery("[id$=txtZip]")[0].id, "", "");
}

// Code:: Validator Enabled::
function EnableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], true);
        $jQuery('#' + id).hide();
    }
}

//Code:: Validator Disabled ::
function DisableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], false);
    }
}

function OnClientSelectedIndexChanged(sender, args) {
    if (areThereAnyChangesAtTheSelection(sender)) {
        __doPostBack('ddlTenant', '');
    }
    else {
        return false;
    }
}

var oldSelectedIdList = [];

function radComboBoxSelectedIdList(sender) {
    var selectedIdList = [];
    var combo = sender;
    var items = combo.get_items();
    var checkedIndices = items._parent._checkedIndices;
    var checkedIndicesCount = checkedIndices.length;
    for (var itemIndex = 0; itemIndex < checkedIndicesCount; itemIndex++) {
        var item = items.getItem(checkedIndices[itemIndex]);
        selectedIdList.push(item._properties._data.value);
    }
    return selectedIdList;
}


function areThereAnyChangesAtTheSelection(sender) {
    var hdnPreviousTenantValues = $jQuery("[id$=hdnPreviousTenantValues]");
    if (hdnPreviousTenantValues.val() != "" && hdnPreviousTenantValues.val() != null && hdnPreviousTenantValues.val() != undefined) {
        oldSelectedIdList = hdnPreviousTenantValues.val().split(',');
    }
    var selectedIdList = radComboBoxSelectedIdList(sender);
    hdnPreviousTenantValues.val(selectedIdList.join(","));
    var isTheCountOfEachSelectionEqual = (selectedIdList.length == oldSelectedIdList.length);
    if (isTheCountOfEachSelectionEqual == false)
        return true;

    var oldIdListMINUSNewIdList = $jQuery(oldSelectedIdList).not(selectedIdList).get();
    var newIdListMINUSOldIdList = $jQuery(selectedIdList).not(oldSelectedIdList).get();

    if (oldIdListMINUSNewIdList.length != 0 || newIdListMINUSOldIdList.length != 0)
        return true;

    return false;
}

var winopen = false;

function OpenInstitutionHierarchyPopup(sender) {
    var composeScreenWindowName = "Institution Hierarchy";
    var screenName = "CommonScreen";
    var tenantId = $jQuery(sender).attr('tenantid');
    var hdnDynamicTenantId = $jQuery("[id$=hdnDynamicTenantId_" + tenantId + "]");
    if (tenantId != "0" && tenantId != "") {
        var DepartmentProgramId = $jQuery(hdnDynamicTenantId).parent().children("[id$=hdnDepartmntPrgrmMppng]").val();
        var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url, { size: "600,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnHierarhyClientClose });
        winopen = true;
    }
    else {
        $alert("Please select Institution.");
    }
    return false;
}

function OnHierarhyClientClose(oWnd, args) {
    oWnd.remove_close(OnHierarhyClientClose);
    if (winopen) {
        var arg = args.get_argument();
        if (arg) {
            var tenantID = arg.TenantID;
            var hdnDynamicTenantId = $jQuery("[id$=hdnDynamicTenantId_" + tenantID + "]");
            $jQuery(hdnDynamicTenantId).parent().children("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
            $jQuery(hdnDynamicTenantId).parent().children("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
            $jQuery(hdnDynamicTenantId).parent().children("[id$=hdnInstitutionNodeId]").val(arg.InstitutionNodeId);
            $jQuery(hdnDynamicTenantId).parent().children("[id$=lblInstitutionHierarchy]")[0].innerHTML = arg.HierarchyLabel;
        }
        winopen = false;
    }
}
