var parentDiv;
function openAgencyHierarchyPopUp(divID) {
    //debugger;
    parentDiv = $jQuery("[id$=" + divID + "]");
    var composeScreenWindowName = "Agency Hierarchy";

    var IsDisabledMode = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsDisabledMode]").val();
    var IsRequestFromAddRotationScrn = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsRequestFromAddRotationScrn]").val(); // UAT-4443
    var IsRequestFromManageRotationByAgencyScrn = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsRequestFromManageRotationByAgencyScrn]").val(); // UAT-4520
    if (IsDisabledMode == "True") {
        return;
    }

    var tenantId = $jQuery("[id$=" + divID + "]").find("input[name*=hdnTenantId]").val();
    var currentOrgUserID = $jQuery("[id$=" + divID + "]").find("input[name*=hdnCurrentOrgUserID]").val();
    var agencyHierarchyNodeSelection = $jQuery("[id$=" + divID + "]").find("input[name*=hdnAgencyHierarchyNodeSelection]").val();
    var nodeHierarchySelection = $jQuery("[id$=" + divID + "]").find("input[name*=hdnNodeHierarchySelection]").val();
    var selectedAgecnyIds = $jQuery("[id$=" + divID + "]").find("input[name*=hdnSelectedAgecnyIds]").val();
    var selectedNodeIds = $jQuery("[id$=" + divID + "]").find("input[name*=hdnSelectedNodeIds]").val();
    var selectedRootNodeId = $jQuery("[id$=" + divID + "]").find("input[name*=hdnSelectedRootNodeId]").val();
    var IsAllNodeDisabledMode = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsAllNodeDisabledMode]").val();
    var IsParentDisable = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsParentDisable]").val();
    var IsAgencyNodeCheckable = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsAgencyNodeCheckable]").val();
    var IsRotationPkgCopyFromReq = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsRotationPkgCopyFromAgencyHierarchy]").val(); //UAT-3494

    //debugger;
    if (tenantId != undefined && tenantId != "" && tenantId >= 0) {
        var institutionNodeIds = $jQuery("[id$=" + divID + "]").find("input[name*=hdnInstitutionNodeIds]").val();
        var isInstitutionRequired = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsInstitutionHierarchyRequired]").val();
        if (isInstitutionRequired == "True" && institutionNodeIds == "") {
            alert("Please Select Institution Hierarchy.");
            return;
        }

        var popupHeight = $jQuery(window).height() * (100 / 100);
        var url = $page.url.create("~/AgencyHierarchy/Pages/AgencyHierarchyMultipleRootNodes.aspx?TenantId=" + tenantId + "&CurrentOrgUserID=" + currentOrgUserID + "&AgencyHierarchyNodeSelection=" + agencyHierarchyNodeSelection + "&NodeHierarchySelection=" + nodeHierarchySelection + "&SelectedAgecnyIds=" + selectedAgecnyIds + "&SelectedNodeIds=" + selectedNodeIds + "&SelectedRootNodeId=" + selectedRootNodeId + "&IsAllNodeDisabledMode=" + IsAllNodeDisabledMode + "&IsParentDisable=" + IsParentDisable + "&IsAgencyNodeCheckable=" + IsAgencyNodeCheckable + "&InstitutionNodeIds=" + institutionNodeIds + "&IsRotationPkgCopyFromAgencyHierarchy=" + IsRotationPkgCopyFromReq + "&IsRequestFromAddRotationScrn=" + IsRequestFromAddRotationScrn);
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientAgencyPopClose });
    }
    else {
        if (IsRequestFromManageRotationByAgencyScrn == "True") {
            var popupHeight = $jQuery(window).height() * (100 / 100);
            var url = $page.url.create("~/AgencyHierarchy/Pages/AgencyHierarchyMultipleRootNodes.aspx?TenantId=" + tenantId + "&CurrentOrgUserID=" + currentOrgUserID + "&AgencyHierarchyNodeSelection=" + agencyHierarchyNodeSelection + "&NodeHierarchySelection=" + nodeHierarchySelection + "&SelectedAgecnyIds=" + selectedAgecnyIds + "&SelectedNodeIds=" + selectedNodeIds + "&SelectedRootNodeId=" + selectedRootNodeId + "&IsAllNodeDisabledMode=" + IsAllNodeDisabledMode + "&IsParentDisable=" + IsParentDisable + "&IsAgencyNodeCheckable=" + IsAgencyNodeCheckable + "&InstitutionNodeIds=" + institutionNodeIds + "&IsRotationPkgCopyFromAgencyHierarchy=" + IsRotationPkgCopyFromReq + "&IsRequestFromAddRotationScrn=" + IsRequestFromAddRotationScrn);
            var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientAgencyPopClose });
        }
        else
            $alert("Please Select Institution.");
    }
}
function OnClientAgencyPopClose(oWnd, args) {
  //  debugger;
    //remove the handler on window close.
    oWnd.remove_close(OnClientAgencyPopClose);
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {

        $jQuery(parentDiv).find("input[name*=hdnAgencyHierarchyJsonObj]").val(arg.AgencyHierarchyJsonObj);
        $jQuery(parentDiv).find("input[name*=hdnSelectedAgecnyIds]").val(arg.SelectedAgecnyIds);
        $jQuery(parentDiv).find("input[name*=hdnSelectedNodeIds]").val(arg.SelectedNodeIds);
        $jQuery(parentDiv).find("input[name*=hdnSelectedRootNodeId]").val(arg.SelectedRootNodeId);
        $jQuery(parentDiv).find("input[name*=hdnIsChildTreeNodeChecked]").val(arg.IsChildTreeNodeChecked);
        $jQuery(parentDiv).find("input[name*=btnDoPostBack]").click();

        if (typeof GetRotationRequiredPackage != 'undefined' && $jQuery.isFunction(GetRotationRequiredPackage)) {
            GetRotationRequiredPackage(arg.SelectedNodeIds);
        }

        if (typeof GetRotationRequiredFieldOptions != 'undefined' && $jQuery.isFunction(GetRotationRequiredFieldOptions)) {
            var jsonObj = $.parseJSON(arg.AgencyHierarchyJsonObj);
            var agencyIdsStr = "";
            if (jsonObj.agencyhierarchy.AgencyID == undefined) {
                var agencyIDs = []
                var agencyNames = []; //UAT-3241
                $.each(jsonObj.agencyhierarchy, function (i, item) {
                    if (item.AgencyID != '' && item.AgencyID !== undefined) {
                        agencyIDs.push(item.AgencyID);
                        agencyNames.push(item.AgencyName); //UAT-3241
                    }
                });
                agencyIdsStr = agencyIDs.join();
                agencyNamesStr = agencyNames.join(); //UAT-3241
            }
            else {
                agencyIdsStr = jsonObj.agencyhierarchy.AgencyID;
                agencyNamesStr = jsonObj.agencyhierarchy.AgencyName; //UAT-3241
            }
            GetRotationRequiredFieldOptions($jQuery("[id$=hdnTenantId]", parentDiv).val(), agencyIdsStr, agencyNamesStr); //UAT-3241
        }
    }
}

function Refresh() {
    $jQuery("[id$=btnDoPostBack]").click();
}

//UAT-4257
function openAgencyHierarchyPopUpOld(divID) {
    //debugger;
    parentDiv = $jQuery("[id$=" + divID + "]");
    var composeScreenWindowName = "Agency Hierarchy";

    var IsDisabledMode = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsDisabledMode]").val();
    if (IsDisabledMode == "True") {
        return;
    }

    var tenantId = $jQuery("[id$=" + divID + "]").find("input[name*=hdnTenantId]").val();
    var currentOrgUserID = $jQuery("[id$=" + divID + "]").find("input[name*=hdnCurrentOrgUserID]").val();
    var agencyHierarchyNodeSelection = $jQuery("[id$=" + divID + "]").find("input[name*=hdnAgencyHierarchyNodeSelection]").val();
    var nodeHierarchySelection = $jQuery("[id$=" + divID + "]").find("input[name*=hdnNodeHierarchySelection]").val();
    var selectedAgecnyIds = $jQuery("[id$=" + divID + "]").find("input[name*=hdnSelectedAgecnyIds]").val();
    var selectedNodeIds = $jQuery("[id$=" + divID + "]").find("input[name*=hdnSelectedNodeIds]").val();
    var selectedRootNodeId = $jQuery("[id$=" + divID + "]").find("input[name*=hdnSelectedRootNodeId]").val();
    var IsAllNodeDisabledMode = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsAllNodeDisabledMode]").val();
    var IsParentDisable = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsParentDisable]").val();
    var IsAgencyNodeCheckable = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsAgencyNodeCheckable]").val();
    var IsRotationPkgCopyFromReq = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsRotationPkgCopyFromAgencyHierarchy]").val(); //UAT-3494
    var isChildBackButtonDisabled = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsChildBackButtonDisabled]").val(); //UAT-4257

    if (tenantId != undefined && tenantId != "" && tenantId >= 0) {
        var institutionNodeIds = $jQuery("[id$=" + divID + "]").find("input[name*=hdnInstitutionNodeIds]").val();
        var isInstitutionRequired = $jQuery("[id$=" + divID + "]").find("input[name*=hdnIsInstitutionHierarchyRequired]").val();
        if (isInstitutionRequired == "True" && institutionNodeIds == "") {
            alert("Please Select Institution Hierarchy.");
            return;
        }

        var popupHeight = $jQuery(window).height() * (100 / 100);
        var url = $page.url.create("~/AgencyHierarchy/Pages/OldAgencyHierarchyMultipleRootNodes.aspx?TenantId=" + tenantId + "&CurrentOrgUserID=" + currentOrgUserID + "&AgencyHierarchyNodeSelection=" + agencyHierarchyNodeSelection + "&NodeHierarchySelection=" + nodeHierarchySelection + "&SelectedAgecnyIds=" + selectedAgecnyIds + "&SelectedNodeIds=" + selectedNodeIds + "&SelectedRootNodeId=" + selectedRootNodeId + "&IsAllNodeDisabledMode=" + IsAllNodeDisabledMode + "&IsParentDisable=" + IsParentDisable + "&IsAgencyNodeCheckable=" + IsAgencyNodeCheckable + "&InstitutionNodeIds=" + institutionNodeIds + "&IsRotationPkgCopyFromAgencyHierarchy=" + IsRotationPkgCopyFromReq + "&IsChildBackButtonDisabled=" + isChildBackButtonDisabled);
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientAgencyPopClose });
    }
    else {
        $alert("Please Select Institution.");
    }
}

