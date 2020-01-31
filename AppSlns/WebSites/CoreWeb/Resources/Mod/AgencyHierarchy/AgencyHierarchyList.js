
function ClientContextMenuShowing(sender, eventArgs) {
    eventArgs.set_cancel(true);
}

$page.add_pageLoad(function () {
    $jQuery(".rbPrimaryIcon.rbSave").removeClass().addClass("fa fa-floppy-o");
    $jQuery(".rbPrimaryIcon.rbCancel").removeClass().addClass("fa fa-ban");
    $jQuery(".rbPrimaryIcon.icnreset").removeClass().addClass("fa fa-mail-reply");
    $jQuery(".fa.fa-arrow-right.right-arrow-color").removeClass();
    $jQuery(".rbPrimaryIcon.rbNext").removeClass();
    // $jQuery(".rbPrimaryIcon").removeClass().addClass("fa fa-compress");

    DisabledCollaspeButton();
    //var treeChildAgencyHierarchy = $find("<%=treeChildAgencyHierarchy.ClientID %>");
    //if (treeChildAgencyHierarchy != null) {

    //    for (var i = 0; i < treeChildAgencyHierarchy.get_allNodes().length; i++) {
    //        if (treeChildAgencyHierarchy.get_allNodes()[i]._hasChildren() == true) {

    //            treeChildAgencyHierarchy.get_allNodes()[i].set_enabled(false)
    //        }
    //    }
    //}
});
function clientNodeClicking(sender, args) {
    if (args.get_node().get_nodes().get_count() == 0 && args.get_node().get_level() != 0) {
        args.set_cancel(false);
    }
    else {
        args.set_cancel(true);
    }
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function returnToParent() {

    var hdnNodeId = $jQuery("[id$=hdnNodeId]")[0].value;
    var hdnLabel = $jQuery("[id$=hdnLabel]")[0].value;
    var hdnAgencyId = $jQuery("[id$=hdnAgencyId]")[0].value;
    var hdnAgencyName = $jQuery("[id$=hdnAgencyName]")[0].value;
    var hdnRootNodeId = $jQuery("[id$=hdnRootNodeId]")[0].value;
    var oArg = {};

    oArg.HierarchyLabel = hdnLabel;
    oArg.NodeId = hdnNodeId;
    oArg.AgencyId = hdnAgencyId;
    oArg.AgencyName = hdnAgencyName;
    oArg.AgencyName = hdnAgencyName;
    oArg.RootNodeId = hdnRootNodeId;

    //get a reference to the current RadWindow
    var oWnd = GetRadWindow();
    oWnd.Close(oArg);
}
function clear() {
    $jQuery("[id$=hdnAgencyNodeId]", parentDiv).val('');
}
// To close the popup.
function ClosePopup() {
    top.$window.get_radManager().getActiveWindow().close();
}
function OnClearClick() {
    $jQuery("[id$=hdnNodeId]")[0].value = "";
    $jQuery("[id$=hdnLabel]")[0].value = "";
    $jQuery("[id$=hdnAgencyId]")[0].value = "";
    $jQuery("[id$=hdnAgencyName]")[0].value = "";
    $jQuery("[id$=hdnRootNodeId]")[0].value = "";
    returnToParent();
}
function clientNodeCollapsing(sender, args) {
    var isDisabled = $jQuery("input[type=submit]").prop('disabled');
    if (!isDisabled) {
        var treeViewDepartment = $find(FSObject.$("[id$=treeChildAgencyHierarchy]")[0].id);
        var nodes = treeViewDepartment.get_allNodes();
        //UAT-3952
        if ($jQuery("[id$=fsucAgencyHierarchyList_btnSubmit_input]").val() == "Collapse") {
            for (var i = 1; i < nodes.length; i++) {
                if (nodes[i].get_nodes() != null) {
                    nodes[i].collapse();
                }
            }
            $jQuery("[id$=hdnIsHierarchyCollapsed]")[0].value = "True";
            $jQuery("[id$=fsucAgencyHierarchyList_btnSubmit_input]").val('Expand');
        }
        else {
            for (var i = 1; i < nodes.length; i++) {
                if (nodes[i].get_nodes() != null) {
                    nodes[i].expand();
                }
            }
            $jQuery("[id$=hdnIsHierarchyCollapsed]")[0].value = "False";
            $jQuery("[id$=fsucAgencyHierarchyList_btnSubmit_input]").val('Collapse');
        }
    }
    //var treeViewDepartment = $find(FSObject.$("[id$=treeChildAgencyHierarchy]")[0].id);
    //if (treeViewDepartment != null && typeof (treeViewDepartment) != "undefined" && treeViewDepartment.get_allNodes()[0].get_level() == '0') {
    //    var childNodes = treeViewDepartment.get_allNodes()[0]._getChildren();
    //    for (var i = 0; i < childNodes._array.length; i++) {
    //        childNodes._array[i].set_expanded(false)
    //    }
    //}
}

function DisabledCollaspeButton() {

    if ($jQuery("[id$=hdnIsHierarchyCollapsed]")[0].value == "True") {
        $jQuery("[id$=fsucAgencyHierarchyList_btnSubmit_input]").val('Expand');
    } else {
        $jQuery("[id$=fsucAgencyHierarchyList_btnSubmit_input]").val('Collapse');
    }

    if ($("[id$=treeChildAgencyHierarchy]") != null && $("[id$=treeChildAgencyHierarchy]").length > 0) {

        var treeViewDepartment = $find(FSObject.$("[id$=treeChildAgencyHierarchy]")[0].id);

        if (treeViewDepartment != null && typeof (treeViewDepartment) != "undefined" && treeViewDepartment.get_allNodes()[0].get_level() == '0') {

            var childNodes = treeViewDepartment.get_allNodes()[0]._getChildren();

            if (childNodes._array[0].get_level() == '1' && childNodes._array[0]._getChildren()._array.length == 0) {
                $jQuery("input[type=submit]").attr('disabled', 'disabled');
                $jQuery("[id$=fsucAgencyHierarchyList_btnSubmit]").addClass('rbDisabled')
            }
        }
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
