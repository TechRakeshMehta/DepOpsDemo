
function ClientContextMenuShowing(sender, eventArgs) {
    eventArgs.set_cancel(true);
}

$page.add_pageLoad(function () {
    $jQuery(".rbPrimaryIcon.rbSave").removeClass().addClass("fa fa-floppy-o");
    $jQuery(".rbPrimaryIcon.rbCancel").removeClass().addClass("fa fa-ban");
    $jQuery(".rbPrimaryIcon icnreset").removeClass();
});
function clientNodeClicking(sender, args) {

    if (args.get_node().get_attributes().getAttribute("UICode") != "undefinded" && args.get_node().get_attributes().getAttribute("UICode") == "PKG" && args.get_node().get_attributes().getAttribute("IsCompliancePackage") == "True") {
        args.set_cancel(false);
    }
    else if (args.get_node().get_attributes().getAttribute("IsCompliancePackage") != "undefinded" && args.get_node().get_attributes().getAttribute("IsCompliancePackage") == "False") {
        args.set_cancel(true);
    }
    else {
        args.set_cancel(true);
    }
}
function ClientNodeClicked(sender, args) {

    var currentNode = args.get_node();
    var allNodes = currentNode.get_allNodes();
    var allNodesCount = allNodes.length;

    if (currentNode.get_checked() && currentNode.get_attributes().getAttribute("UICode") != "undefinded" && currentNode.get_attributes().getAttribute("UICode") == "PKG") {

        var hdnPackageNodeMappingID = $jQuery("[id$=hdnPackageNodeMappingID]")[0].value;
        var hdnPackageName = $jQuery("[id$=hdnPackageName]")[0].value;
        var hdnPackageID = $jQuery("[id$=hdnPackageID]")[0].value;
        var hdnInstitutionHierarchyNodeID = $jQuery("[id$=hdnInstitutionHierarchyNodeID]")[0].value;
        if (hdnPackageNodeMappingID != '') {
            var PackageNodeMappingIDs = jQuery("[id$=hdnPackageNodeMappingID]")[0].value;
            $jQuery("[id$=hdnPackageNodeMappingID]")[0].value = PackageNodeMappingIDs + "," + currentNode.get_attributes().getAttribute("PackageNodeMappingID")
        }
        else {
            $jQuery("[id$=hdnPackageNodeMappingID]")[0].value = currentNode.get_attributes().getAttribute("PackageNodeMappingID")
        }

        if (hdnPackageName != '') {
            var PackageName = jQuery("[id$=hdnPackageName]")[0].value
            $jQuery("[id$=hdnPackageName]")[0].value = PackageName + "," + currentNode.get_attributes().getAttribute("PackageName")
        }
        else {
            $jQuery("[id$=hdnPackageName]")[0].value = currentNode.get_attributes().getAttribute("PackageName")
        }

        if (hdnPackageID != '') {
            var PackageID = jQuery("[id$=hdnPackageID]")[0].value
            $jQuery("[id$=hdnPackageID]")[0].value = PackageID + "," + currentNode.get_attributes().getAttribute("PackageID")
        }
        else {
            $jQuery("[id$=hdnPackageID]")[0].value = currentNode.get_attributes().getAttribute("PackageID")
        }
        if (hdnInstitutionHierarchyNodeID != '') {
            var DeptProgramMappingID = jQuery("[id$=hdnInstitutionHierarchyNodeID]")[0].value
            $jQuery("[id$=hdnInstitutionHierarchyNodeID]")[0].value = DeptProgramMappingID + "," + currentNode.get_attributes().getAttribute("InstitutionHierarchyNodeID")
        }
        else {
            $jQuery("[id$=hdnInstitutionHierarchyNodeID]")[0].value = currentNode.get_attributes().getAttribute("InstitutionHierarchyNodeID")
        }
    }
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function returnToParent() {

    var hdnPackageNodeMappingID = $jQuery("[id$=hdnPackageNodeMappingID]")[0].value;
    var hdnPackageName = $jQuery("[id$=hdnPackageName]")[0].value;
    var hdnPackageID = $jQuery("[id$=hdnPackageID]")[0].value;
    var hdnInstitutionHierarchyNodeID = $jQuery("[id$=hdnInstitutionHierarchyNodeID]")[0].value;
    var oArg = {};

    oArg.PackageNodeMappingID = hdnPackageNodeMappingID;
    oArg.PackageName = hdnPackageName;
    oArg.PackageID = hdnPackageID
    oArg.InstitutionHierarchyNodeID = hdnInstitutionHierarchyNodeID
    //get a reference to the current RadWindow
    var oWnd = GetRadWindow();
    oWnd.Close(oArg);
}
// To close the popup.
function ClosePopup() {
    top.$window.get_radManager().getActiveWindow().close();
}
function OnClearClick() {
    $jQuery("[id$=hdnPackageNodeMappingID]")[0].value = "";
    $jQuery("[id$=hdnPackageName]")[0].value = "";
    $jQuery("[id$=hdnPackageID]")[0].value = "";
    $jQuery("[id$=hdnInstitutionHierarchyNodeID]")[0].value = "";
    returnToParent();
}
