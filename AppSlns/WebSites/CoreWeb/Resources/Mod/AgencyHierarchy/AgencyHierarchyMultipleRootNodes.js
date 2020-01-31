
function ClientContextMenuShowing(sender, eventArgs) {
    eventArgs.set_cancel(true);
}


function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function returnToParent(jsonObj) {
    //debugger;
    var oArg = {};
    if (typeof jsonObj != 'undefined') {
        var agencyHierarchyJsonObj = JSON.stringify(jsonObj);
        oArg.AgencyHierarchyJsonObj = agencyHierarchyJsonObj;
    }

    var hdnSelectedAgecnyIds = $jQuery("[id$=hdnSelectedAgecnyIds]")[0].value;
    var hdnSelectedNodeIds = $jQuery("[id$=hdnSelectedNodeIds]")[0].value;
    var hdnSelectedRootNodeId = $jQuery("[id$=hdnSelectedRootNodeId]")[0].value;

    oArg.SelectedAgecnyIds = hdnSelectedAgecnyIds;
    oArg.SelectedNodeIds = hdnSelectedNodeIds;
    oArg.SelectedRootNodeId = hdnSelectedRootNodeId;
    //get a reference to the current RadWindow
    var oWnd = GetRadWindow();
    oWnd.Close(oArg);
}

// To close the popup.
function ClosePopup() {
    top.$window.get_radManager().getActiveWindow().close();
}

function OnClearClick() {
    $jQuery("[id$=hdnSelectedNode]")[0].value = "";
    $jQuery("[id$=hdnLabel]")[0].value = "";
    $jQuery("[id$=hdnInstitutionNodeId]")[0].value = "";
    returnToParent();
}

function pageLoad() {
    $jQuery(".rbPrimaryIcon.rbSave").removeClass().addClass("fa fa-floppy-o");
    $jQuery(".rbPrimaryIcon.rbCancel").removeClass().addClass("fa fa-ban");
}
function clientNodeChecking(sender, args) {
    //debugger;
    if (args.get_node().get_nodes().get_count() == 0 && args.get_node().get_level() != 0) {
        args.set_cancel(false);
    }
    else {
        args.set_cancel(true);
    }
}
function ClientNodeClicked(sender, args) {
    //debugger;
    var currentNode = args.get_node();
    var allNodes = currentNode.get_allNodes();
    var allNodesCount = allNodes.length;
    if (currentNode.get_checked()) {

        for (var i = 0; i < allNodesCount; i++) {
            allNodes[i].set_enabled(true);
            //allNodes[i].set_checked(false);
            //here you may check what is the current node level or child nodes count
        } 
       
    }
    else {
        for (var i = 0; i < allNodesCount; i++) {
            allNodes[i].set_checked(false);
            allNodes[i].set_enabled(false);
           
         
            //here you may check what is the current node level or child nodes count
        }
    }

}