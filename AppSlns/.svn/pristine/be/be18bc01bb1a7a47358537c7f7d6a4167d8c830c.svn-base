var parentDiv;
function openInstituteHierarchyPackagePopUp(divID) {

    var composeScreenWindowName = "Institute Hierarchy Package";
    parentDiv = $jQuery("[id$=" + divID + "]");
    var tenantId = $jQuery("[id$=hdnTenantId]", parentDiv).val();

    if (tenantId != "0" && tenantId != "") {

        var compliancePackageTypeCode = $jQuery("[id$=hdnCompliancePackageTypeCode]", parentDiv).val();
        var isCompliancePackage = $jQuery("[id$=hdnIsCompliancePackage]", parentDiv).val();

        var packageNodeMappingID = $jQuery("[id$=hdnPackageNodeMappingID]", parentDiv).val();
        var packageId = $jQuery("[id$=hdnPackageId]", parentDiv).val();
        var institutionHierarchyNodeID = $jQuery("[id$=hdnInstitutionHierarchyNodeID]", parentDiv).val();
        var packageName = $jQuery("[id$=hdnPackageName]", parentDiv).val();

        var popupHeight = $jQuery(window).height() * (100 / 100);
        var url = $page.url.create("~/CommonOperations/Pages/InstituteHierarchyPackageList.aspx?TenantId=" + tenantId + "&CompliancePackageTypeCode=" + compliancePackageTypeCode + "&IsCompliancePackage=" + isCompliancePackage + "&PackageNodeMappingID=" + packageNodeMappingID + "&PackageId=" + packageId + "&InstitutionHierarchyNodeID=" + institutionHierarchyNodeID + "&PackageName=" + packageName);
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnInstituteHierarchyPackagePopUpClientClose });
    }
    else {
        $alert("Please Select Institution.");
    }
}

function OnInstituteHierarchyPackagePopUpClientClose(oWnd, args) {
    //remove the handler on window close.
    oWnd.remove_close(OnClientClose);

    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        $jQuery("[id$=hdnPackageNodeMappingID]", parentDiv).val(arg.PackageNodeMappingID);
        $jQuery("[id$=hdnPackageName]", parentDiv).val(arg.PackageName);
        $jQuery("[id$=hdnPackageId]", parentDiv).val(arg.PackageID);
        $jQuery("[id$=hdnInstitutionHierarchyNodeID]", parentDiv).val(arg.InstitutionHierarchyNodeID);

        $jQuery("[id$=btnDoPostBack]", parentDiv).click();

        if (typeof InstitutionHierarchyLabel != 'undefined' && $jQuery.isFunction(InstitutionHierarchyLabel)) {
            InstitutionHierarchyLabel();
        }

    }
}

$page.add_pageLoad(function () {
    if (typeof InstitutionHierarchyLabel != 'undefined' && $jQuery.isFunction(InstitutionHierarchyLabel)) {
        InstitutionHierarchyLabel();
    }
});
