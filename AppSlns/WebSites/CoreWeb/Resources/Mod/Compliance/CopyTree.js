//Check or uncheck nodes of Copy Tree List.
function ManageNodes(obj) {
    var currentNode = $jQuery("#" + obj.id)[0];

    //If a node is checked true then, checked all its parent nodes and children.
    if (currentNode != undefined && currentNode.checked == true) {
        ManageParentAndChildren(obj);
    }
    //If a node is checked false then, unchecked all children.
    else if (currentNode != undefined && currentNode.checked == false) {
        ManageChildrenIfParentUnChecked(obj);
    }
}

//If child node is checked true then, checked its parent.
function ManageParents(obj) {
    var parentObj = $jQuery('[fieldIndex="' + $jQuery(obj).parent("span").attr("parent") + '"] input');

    if (parentObj != undefined && parentObj != null && parentObj.length > 0) {
        parentObj.prop('checked', true);
        ManageParents(parentObj);
    }
}

//If a node is checked true then, checked its children.
function ManageParentAndChildren(obj) {
    var parentObj = $jQuery('[fieldIndex="' + $jQuery(obj).parent("span").attr("parent") + '"] input');

    //If package node is checked true then, checked its children.
    if (parentObj.length == 0) {
        $jQuery("[id$=hdnPackageNode]")[0].value = obj.alt;
        $jQuery("[id$=" + this.theForm.id + "]").submit();
    }
    else {
        //If child node is checked true then, checked its parent.
        ManageParents(obj);
    }
}

//If a node is checked false then, unchecked all children.
function ManageChildrenIfParentUnChecked(obj) {
    $jQuery('[parent="' + $jQuery(obj).attr("alt") + '"] input').each(function () {
    
        if (this.id.length > 0) {
            $jQuery("input[id$=" + this.id + "]").prop('checked', false);
            ManageChildrenIfParentUnChecked(this);
        }
    });
}


