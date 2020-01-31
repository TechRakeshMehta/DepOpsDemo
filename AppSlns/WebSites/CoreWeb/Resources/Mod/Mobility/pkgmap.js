//var mappable_nodes = "$CAT$ITM$ATR$";
//[BS_11272013]: Currently mapping can be defined between 2 attributes notes. 
var mappable_nodes = "$ATR$";

var trvSource_nodeClicked = function (sender, eventArgs) {

    var node = eventArgs.get_node();
    var nodeType = node.get_attributes().getAttribute("_nodeDataType")

    //Code to prevent selection
    if (mappable_nodes.indexOf(nodeType) < 0) {
        node.set_selected(false);
        return;
    }

    //Setting up post back data
    var trvSourceItemNode = "";
    var trvSourceAttributeNode = "";
    if (node != null) {
        trvSourceAttributeNode = node.get_value();
    }

    if (node._parent._parent != null) {
        trvSourceItemNode = node._parent._parent.get_value();
    }

    $jQuery("[id$=hdnTrvSourceItemNode]")[0].value = trvSourceItemNode;
    $jQuery("[id$=hdnTrvSourceAttributeNode]")[0].value = trvSourceAttributeNode;
}

function trvTarget_nodeClicked(sender, eventArgs) {

    var node = eventArgs.get_node();
    var nodeType = node.get_attributes().getAttribute("_nodeDataType")

    //Code to prevent selection
    if (mappable_nodes.indexOf(nodeType) < 0) {
        node.set_selected(false);
        return;
    }

    $findByKey("trvSource", function () {
        if (!this.get_selectedNode()) {
            node.set_selected(false);
            $alert("Please select a source node first!", "Package Mapping");
            return;
        }
    });

    var trvTargetAttributeNode = "";
    var trvTargetItemNode = "";
    if (node != null) {
        trvTargetAttributeNode = node.get_value();
    }

    if (node._parent._parent != null) {
        trvTargetItemNode = node._parent._parent.get_value();
    }
    //Setting up post back data    
    $jQuery("[id$=hdnTrvTargetAttributeNode]")[0].value = trvTargetAttributeNode;
    $jQuery("[id$=hdnTrvTargetItemNode]")[0].value = trvTargetItemNode;
}

var btnMappingClicking = function (s, a) {
    //Getting Current scroll position of source and target divs
    var getCurrentScollOfSource = $jQuery("[id$=dvSource]")[0].scrollTop;
    var getCurrentScollOfTarget = $jQuery("[id$=dvTarget]")[0].scrollTop;
    $jQuery("[id$=hdnDvSourceScrollPosition]")[0].value = getCurrentScollOfSource;
    $jQuery("[id$=hdnDvTargetScrollPosition]")[0].value = getCurrentScollOfTarget;

    var source = $findByKey("trvSource");
    var target = $findByKey("trvTarget");

    //no postback if tree not found, or nodes is not selected
    //in both of the tree
    if (!source || !target || !source.get_selectedNode() || !target.get_selectedNode()) {
        $alert("Please select nodes to map!", "Package Mapping");
        a.set_cancel(true); return;
    }

    var srcNodeType = source.get_selectedNode().get_attributes().getAttribute("_nodeDataType")
    var trgNodeType = target.get_selectedNode().get_attributes().getAttribute("_nodeDataType")

    //Code to prevent selection
    if (srcNodeType != trgNodeType) {
        //[BS_11272013]: Currently mapping can be defined between 2 attributes notes. 
        //$alert("Invalid mapping! <br/> Please note: Only Category-Category, Item-Item or Attribute-Attribute can be mapped together.", "Package Mapping");
        $alert("Invalid mapping! <br/> Please note: Only Attribute-Attribute can be mapped together.", "Package Mapping");
        a.set_cancel(true); return;
        return;
    }

    //[BS_11272013]: Currently mapping can be defined between 2 attributes notes. 
    if (srcNodeType != "ATR" || trgNodeType != "ATR") {
        $alert("Invalid mapping! <br/> Please note: Only Attribute-Attribute can be mapped together.", "Package Mapping");
        a.set_cancel(true); return;
        return;
    }

}

//Function to maintain scroll positions while adding mappings.
function MaintainScrollPositions(sourceScrollPosition, targetScrollPosition)
{
    //debugger;
    //var scrollSource =  $jQuery("[id$=hdnDvSourceScrollPosition]")[0].value;
    //var scrollTarget =  $jQuery("[id$=hdnDvTargetScrollPosition]")[0].value;
    $jQuery("[id$=dvSource]")[0].scrollTop = sourceScrollPosition;
    $jQuery("[id$=dvTarget]")[0].scrollTop = targetScrollPosition;
}