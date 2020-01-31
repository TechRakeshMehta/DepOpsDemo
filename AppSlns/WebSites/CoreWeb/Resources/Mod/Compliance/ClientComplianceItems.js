var tenantId;

function grdItems_Command(sender, args) {

    var parentDateKey;

    var ownerGridElement = sender.get_element();
    var parentRowId = $jQuery(ownerGridElement).parents("tr").first().prev("tr.rgRow, tr.rgAltRow")[0].id;
    if (parentRowId != "") {
        sender.get_parent().get_dataItems();
        // parentDateKey = $find(parentRowId).getDataKeyValue("ClientComplianceCategoryID");

        var itemIndex = args.get_commandArgument();
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        if (args.get_commandName().toLowerCase() == "managerules") {
            args.set_cancel(true);
            parentDateKey = sender.get_masterTableView().get_dataItems()[itemIndex].getDataKeyValue("ClientComplianceCategoryID");
            var selectedRecordDataKey = sender.get_masterTableView().get_dataItems()[itemIndex].getDataKeyValue("ClientComplianceItemID");
            var url = "~/ComplianceAdministration/Pages/ManageItemRule.aspx?ccCatId=" + parentDateKey + "&ccId=" + selectedRecordDataKey + "&tenantId=" + tenantId + "&type=client";
            $window.createPopup($page.url.create(url), { size: "800,"+popupHeight });
        }
        else if (args.get_commandName().toLowerCase() == "managevalidations") {            
            args.set_cancel(true);
            parentDateKey = sender.get_masterTableView().get_dataItems()[itemIndex].getDataKeyValue("ClientComplianceCategoryID");
            var selectedRecordDataKey = sender.get_masterTableView().get_dataItems()[itemIndex].getDataKeyValue("ClientComplianceItemID");
            $window.createPopup($page.url.create("~/ComplianceAdministration/Pages/ManageValidations.aspx?ccCatId=" + parentDateKey + "&ccId=" + selectedRecordDataKey + "&tenantId=" + tenantId + "&type=client"), { size: "800,"+popupHeight });
        }
    }
}