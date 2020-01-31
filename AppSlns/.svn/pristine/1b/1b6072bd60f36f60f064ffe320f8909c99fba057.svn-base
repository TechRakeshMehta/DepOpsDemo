function ConfirmCopy(sender, eventArgs) {

    var txtCatName = $find($jQuery("[id$=txtCatName]")[0].id);
    var cmbPackages = $find($jQuery("[id$=cmbPackages]")[0].id);
    var cmbselectpackageType = $find($jQuery("[id$=cmbselectpackageType]")[0].id);
    var cmbCopyCategory = $find($jQuery("[id$=cmbCopyCategory]")[0].id);


    if (cmbselectpackageType.get_value() == "-1") {
        $alert("Select package type to Copy the category from.");
        sender.set_autoPostBack(false);
        return;
    }

    if (cmbPackages.get_value() == "0") {
        $alert("Select a package to Copy category from.");
        sender.set_autoPostBack(false);
        return;
    }

    if (cmbCopyCategory.get_value() == "0") {
        $alert("Select a category to Copy.");
        sender.set_autoPostBack(false);
        return;
    }


    if (txtCatName.get_textBoxValue().trim() != "") {
        sender.set_autoPostBack(window.confirm("Are you sure you want to copy this compliance category, its items & attributes?"));
    }
    else {
        sender.set_autoPostBack(false);
    }
}

function grdMasterComplianceList_Command(sender, args) {

    if (args.get_commandName().toLowerCase() == "managerules") {
        args.set_cancel(true);
        var itemIndex = args.get_commandArgument();
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var selectedRecordDataKey = sender.get_masterTableView().get_dataItems()[itemIndex].getDataKeyValue("ClientComplianceCategoryID");
        $window.createPopup($page.url.create("~/ComplianceAdministration/Pages/ManageCategoryRuleset.aspx?cId=" + selectedRecordDataKey + "&tenantId=" + tenantId + "&type=client"), { size: "800,"+popupHeight });
    }
}

function openCreateExprPopup(clientComplianceItemAttributeID, clientComplianceCategoryID, tenantId) {
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    $window.createPopup($page.url.create("~/ComplianceAdministration/Pages/ManageExpression.aspx?clientComplianceItemAttributeID=" + clientComplianceItemAttributeID + "&clientComplianceCategoryID=" + clientComplianceCategoryID + "&tenantId=" + tenantId), { size: "800,"+popupHeight });
    return false;
}
