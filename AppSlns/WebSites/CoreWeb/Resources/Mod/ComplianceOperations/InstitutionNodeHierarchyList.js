function OpenDefaultScreen() {
    var hdnDefaultScreen = $jQuery("[id$=hdnDefaultScreen]")[0];

    if (hdnDefaultScreen != undefined && hdnDefaultScreen.value != "") {
        window.setTimeout(function () { $jQuery("[id$=ifrDetails]").attr('src', hdnDefaultScreen.value); }, 500);
    }
}

function ClientContextMenuShowing(sender, eventArgs) {
    eventArgs.set_cancel(true);
}

function GetSelectedNode(sender, eventArgs) {
    //$jQuery("[id$=hdnSelectedNode]")[0].value = eventArgs.get_node().get_value();
}

function GetCheckedNode(sender, eventArgs) {
    if (eventArgs.get_node().get_checkState() == 1) {
        //Checked
        var checkedNodeValue = eventArgs.get_node().get_value();
        var checkedNodeValueArr = checkedNodeValue.split('_');
        $jQuery("[id$=hdnSelectedNode]")[0].value += checkedNodeValueArr[2] + ",";
    }
    else if (eventArgs.get_node().get_checkState() == 0) {
        //Unchecked
        var unCheckedNodeValue = eventArgs.get_node().get_value();
        var checkedNodeValueArr = unCheckedNodeValue.split('_');
        var valueToRemove = checkedNodeValueArr[2];
        var tmpArr = $jQuery("[id$=hdnSelectedNode]")[0].value.split(',');
        $jQuery("[id$=hdnSelectedNode]")[0].value = "";
        for (var i = 0; i < tmpArr.length; i++) {
            if (tmpArr[i].trim() != valueToRemove && tmpArr[i] != "") {
                $jQuery("[id$=hdnSelectedNode]")[0].value += tmpArr[i] + ",";
            }
        }
    }
}

function SetTreeNode(sender, eventArgs) {
    if ($jQuery("[id$=hdnSelectedNode]")[0].value != "") {
        var node = sender.findNodeByValue($jQuery("[id$=hdnSelectedNode]")[0].value);
        if (node != undefined && node != null) {
            var parentNode = node.get_parent();
            while (parentNode.get_expanded != undefined) {
                parentNode.expand();
                parentNode = parentNode.get_parent();
            }
            node.select();
            var element = $jQuery("a[href$=" + node.get_value() + "]");
            if (element != null) {
                element.focus();
            }
            $jQuery("[id$=hdnDefaultScreen]")[0].value = sender.get_selectedNode().get_navigateUrl();
            //OpenDefaultScreen();
        }
    }
    else {
        if (sender.get_selectedNode() != null && sender.get_selectedNode().get_value() != "") {
            $jQuery("[id$=hdnSelectedNode]")[0].value = sender.get_selectedNode().get_value();
        }
        //                $jQuery("[id$=hdnDefaultScreen]")[0].value = sender.get_selectedNode().get_navigateUrl();
        //                OpenDefaultScreen();
    }
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function returnToParent() {
    var hdnSelectedNode = $jQuery("[id$=hdnSelectedNode]")[0].value;
    var hdnLabel = $jQuery("[id$=hdnLabel]")[0].value;
    var hdnTenantId = $jQuery("[id$=hdnTenantId]")[0].value;
    var hdnInstitutionNodeId = $jQuery("[id$=hdnInstitutionNodeId]")[0].value;
    var oArg = {};
    var count = 0;
    if (hdnSelectedNode != "") {
        for (var i = 0; i < hdnSelectedNode.length; i++) {
            if (hdnSelectedNode[i] == "_") {
                count++;
                break;
            }
        }
        if (count > 0) {
            var strControlName = hdnSelectedNode.split('_');
            oArg.DepPrgMappingId = strControlName[2];
        }
        else {
            oArg.DepPrgMappingId = hdnSelectedNode;
        }

    }
    else {
        oArg.DepPrgMappingId = "";
    }
    oArg.HierarchyLabel = hdnLabel;
    oArg.InstitutionNodeId = hdnInstitutionNodeId;
    oArg.TenantID = hdnTenantId;
    oArg.IsHierarchyChanged = "true";
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

function clientNodeCollapsing(sender, args) {
    var treeViewDepartment = $find(FSObject.$("[id$=treeDepartment]")[0].id);

    //if (treeViewDepartment != null && typeof (treeViewDepartment) != "undefined" && treeViewDepartment.get_allNodes()[0].get_level() == '0') {

    //    var childNodes = treeViewDepartment.get_allNodes()[0]._getChildren();

    //    for (var i = 0; i < childNodes._array.length; i++) {

    //        childNodes._array[i].set_expanded(false)
    //    }
    //}
    var nodes = treeViewDepartment.get_allNodes();

    if ($jQuery("[id$=fsucInstitutionHierarchyList_btnSubmit_input]").val() == "Collapse") {
        for (var i = 1; i < nodes.length; i++) {
            if (nodes[i].get_nodes() != null) {
                nodes[i].collapse();
            }
        }
        $jQuery("[id$=hdnIsHierarchyCollapsed]")[0].value = "True";
        $jQuery("[id$=fsucInstitutionHierarchyList_btnSubmit_input]").val('Expand');
    }
    else {
        for (var i = 1; i < nodes.length; i++) {
            if (nodes[i].get_nodes() != null) {
                nodes[i].expand();
            }
        }
        $jQuery("[id$=hdnIsHierarchyCollapsed]")[0].value = "False";
        $jQuery("[id$=fsucInstitutionHierarchyList_btnSubmit_input]").val('Collapse');
    }

}


function clientNodeCollapsed(sender, eventArgs) {
    var node = eventArgs.get_node();
    var childnodes = node._children._array;
    if (childnodes != null && typeof (childnodes) != "undefined" && childnodes.length > 0) {
        for (var i = 0; i < childnodes.length; i++) {
            if (childnodes[i].get_nodes() != null) {
                childnodes[i].collapse();
            }
        }
    }
}

