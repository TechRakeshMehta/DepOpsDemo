function openPopUp() {
    var composeScreenWindowName = "Agency Hierarchy";
    var tenantId = $jQuery("[id$=hdnTenantId]").val();
    if (tenantId != "0" && tenantId != "") {
        var popupHeight = $jQuery(window).height() * (100 / 100);
        var url = $page.url.create("~/AgencyHierarchy/Pages/AgencyHierarchyList.aspx?TenantId=" + tenantId);
        var win = $window.createPopup(url, { size: "500," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
    }
    else {
        alert("Please Select Institute.");
    }
}

function OnClientClose(oWnd, args) {
    //remove the handler on window close.
    oWnd.remove_close(OnClientClose);
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        $jQuery("[id$=hdnNodeId]").val(arg.NodeId);
        $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
        $jQuery("[id$=hdnAgencyNodeId]").val(arg.AgencyId);
        $jQuery("[id$=btnDoPostBack]").click();
    }
}

function Refresh() {
    $jQuery("[id$=btnDoPostBack]").click();
}

function ClearAgencyHierarchySelection() {
    $jQuery("[id$=hdnNodeId]")[0].value = "";
    $jQuery("[id$=hdnHierarchyLabel]")[0].value = "";
    $jQuery("[id$=hdnAgencyNodeId]")[0].value = "";
}