//START-UAT-3157//
$jQuery(document).ready(function () {
    //  debugger;
    var hdnIsDefaultPreferredTenant = $jQuery("[id$=hdnIsDefaultPreferredTenant]").val();
    if (hdnIsDefaultPreferredTenant) {
        btn = $jQuery('[id$=btnUpdateTree]');
        btn.click();
    }
});

//END UAT-3157//

var winopen = false;
function OpenDefaultScreen() {
    var hdnDefaultScreen = $jQuery("[id$=hdnDefaultScreen]")[0];

    if (hdnDefaultScreen != undefined && hdnDefaultScreen.value != "") {
        $jQuery("[id$=ifrDetails]").attr('src', hdnDefaultScreen.value);
        hdnDefaultScreen.value = "";
    }
    else {
        $jQuery("[id$=ifrDetails]").attr('src', "");
    }
    $jQuery("[id$=hdnTenantIsChanged]")[0].value = false;
}

function GetSelectedNode(sender, eventArgs) {
    $jQuery("[id$=hdnSelectedNode]")[0].value = eventArgs.get_node().get_value();
}

function SetTreeNode(sender, eventArgs) {
    if ($jQuery("[id$=hdnSelectedNode]")[0].value != "") {
        var node = sender.findNodeByValue($jQuery("[id$=hdnSelectedNode]")[0].value);
        if (node != undefined && node != null) {
            node.select();
            var element = $jQuery("a[href$=" + node.get_value() + "]");
            if (element != null) {
                element.focus();
            }
        }
    }
    else {
        if (sender.get_selectedNode() != null && sender.get_selectedNode().get_value() != "") {
            $jQuery("[id$=hdnSelectedNode]")[0].value = sender.get_selectedNode().get_value();
        }
    }
}

function ResetTimer() {
    var hdntimeout = $jQuery('[id$=hdntimeout]');  //, $jQuery(parent.theForm));
    if (hdntimeout != null) {
        var timeout = hdntimeout.val();
        parent.StartCountDown(timeout);
    }
}

function OnClientSelectedIndexChanged() {
    $jQuery("[id$=hdnTenantIsChanged]")[0].value = true;
    RefreshTree();
}

function RefreshTree() {
    Page.showProgress("Processing...");

    $jQuery("[id$=hdnSelectedNode]")[0].value = "";
    var btn = $jQuery('[id$=btnUpdateTree]');
    btn.click();
}

function ClientContextMenuShowing(sender, eventArgs) {
    var treeNode = eventArgs.get_node();
    var parametersValue = treeNode.get_navigateUrl();

    if (parametersValue.indexOf("BackGroundPackageId") > 0 && parametersValue.indexOf("SelectedTenantId") > 0) {
        var parameters = treeNode.get_navigateUrl().split("&");

        //Get compliancePackageID value for Package Node
        BkgPackageHierarchyMappingId = parameters[0].replace("BkgPackagePriceSetUp.aspx?BackGroundPackageId=", '');

        //Get  value CompliancePackageID for Package Node
        HierarchyNodeId = parameters[1].replace("ParentID=", '');
        //Get selectedTenantId value
        selectedTenantId = parameters[5].replace("SelectedTenantId=", '');
        treeNode.set_selected(true);
        $jQuery("[id$=hdnSelectedNode]")[0].value = treeNode.get_value();
    }
    else {
        //Disable the right click on the node 
        eventArgs.set_cancel(true);
    }
}

function ClientContextMenuItemClicked(sender, eventArgs) {
    var treeNode = eventArgs.get_node();
    var parametersValue = treeNode.get_navigateUrl();
    openPopUp(BkgPackageHierarchyMappingId, HierarchyNodeId, selectedTenantId);
}

function openPopUp(BkgPackageHierarchyMappingId, HierarchyNodeId, selectedTenantId) {
    var packageCopyScreenWindowName = "packageCopyScreen";
    ResetTimer();
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (80 / 100);

    var url = $page.url.create("~/BkgSetup/Pages/BkgPackageCopy.aspx?TenantId=" + selectedTenantId + "&BPHM_ID=" + BkgPackageHierarchyMappingId + "&HierarchyNodeId=" + HierarchyNodeId);
    var win = $window.createPopup(url, { size: "1000," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: packageCopyScreenWindowName, onclose: OnClientClose });
    winopen = true;
}

function OnClientClose(oWnd, args) {
    oWnd.remove_close(OnClientClose);
    if (winopen) {
        winopen = false;
        RefreshTreeData();
    }
}

function RefreshTreeData() {
    Page.showProgress("Processing...");
    var btn = $jQuery('[id$=btnUpdateTree]');
    btn.click();
}