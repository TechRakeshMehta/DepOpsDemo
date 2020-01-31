function ConfirmCopy(sender, eventArgs) {

    var txtCategoryName = $find($jQuery("[id$=txtCatName]")[0].id);
    var cmbComplianceCategoriesToCopy = $find($jQuery("[id$=cmbCopyCategories]")[0].id);

    if (cmbComplianceCategoriesToCopy.get_value() == "0") {
        $alert("No category is selected to Copy.");
        sender.set_autoPostBack(false);
        return;
    }

    if (txtCategoryName.get_textBoxValue().trim() != "") {
        sender.set_autoPostBack(window.confirm("Are you sure you want to copy this Compliance category and its compliance items?"));
    }
    else {
        sender.set_autoPostBack(false);
        $alert("Category Name is required for Copy process.");
    }
}

function grdItems_Command(sender, args) {
   
    var parentDateKey;

    var ownerGridElement = sender.get_element();
    var parentRowId = $jQuery(ownerGridElement).parents("tr").first().prev("tr.rgRow, tr.rgAltRow")[0].id;
    if (parentRowId != "") {
        sender.get_parent().get_dataItems();
        parentDateKey = $find(parentRowId).getDataKeyValue("ComplianceCategoryID");

        var itemIndex = args.get_commandArgument();
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        if (args.get_commandName().toLowerCase() == "managerules") {
            args.set_cancel(true);
            var selectedRecordDataKey = sender.get_masterTableView().get_dataItems()[itemIndex].getDataKeyValue("ComplianceCategoryComplianceItemID");
            $window.createPopup($page.url.create("Pages/ManageItemRule.aspx?cId=" + parentDateKey + "&cccId=" + selectedRecordDataKey), { size: "800,"+popupHeight });
        }
        else if (args.get_commandName().toLowerCase() == "managevalidations") {
            args.set_cancel(true);
            var selectedRecordDataKey = sender.get_masterTableView().get_dataItems()[itemIndex].getDataKeyValue("ComplianceCategoryComplianceItemID");
            $window.createPopup($page.url.create("Pages/ManageValidations.aspx?cId=" + parentDateKey + "&cccId=" + selectedRecordDataKey), { size: "800,"+popupHeight });
        }
    }
}

function grdMasterComplianceList_Command(sender, args) {
    
    if (args.get_commandName().toLowerCase() == "managerules") {
        args.set_cancel(true);
        var itemIndex = args.get_commandArgument();
        var selectedRecordDataKey = sender.get_masterTableView().get_dataItems()[itemIndex].getDataKeyValue("ComplianceCategoryID");
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        $window.createPopup($page.url.create("Pages/ManageCategoryRuleset.aspx?cId=" + selectedRecordDataKey), { size: "800,"+popupHeight });
    }
}
