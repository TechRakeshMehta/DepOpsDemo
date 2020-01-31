var loadDefaultDetailPage = true;
var compliancePackageID;
var selectedTenantId;
var winopen = false;
var _menuItemVal;

$jQuery(document).ready(function () {
    var ddlTenant = $find($jQuery("[id$=ddlTenant]")[0].id);

    //UAT-1116: Package selection combo box on package screens
    //if (ddlTenant != undefined && loadDefaultDetailPage == true && $jQuery("[id$=hdnIsSearchClicked]")[0].value != "") {
    //    $jQuery("[id$=ifrDetails]").attr('src', "packagelist.aspx?SelectedTenantId=" + ddlTenant._value);
      
    //}
});


function GetSelectedNode(sender, eventArgs) {
    $jQuery("[id$=hdnSelectedNode]")[0].value = eventArgs.get_node().get_value();
}

function SetTreeNode(sender, eventArgs) {
    if ($jQuery("[id$=hdnStoredData]")[0].value != "") {
        $jQuery("[id$=hdnSelectedNode]").val($jQuery("[id$=hdnStoredData]")[0].value);
        $jQuery("[id$=hdnStoredData]").val('');
    }
    if ($jQuery("[id$=hdnSelectedNode]")[0].value != "") {
        var node = sender.findNodeByValue($jQuery("[id$=hdnSelectedNode]")[0].value);
        if (node != undefined && node != null) {
            //            var parentNode = node.get_parent();
            //            while (parentNode.get_expanded != undefined) {
            //                parentNode.expand();
            //                parentNode = parentNode.get_parent();
            //            }
            node.select();
            var element = $jQuery("a[href$=" + node.get_value() + "]");
            if (element != null) {
                element.focus();
            }
        }
    }
    else if (sender.get_selectedNode() != null && sender.get_selectedNode() != undefined) {
        $jQuery("[id$=hdnSelectedNode]")[0].value = sender.get_selectedNode().get_value();
    }
}

function RefreshTree() {
    Page.showProgress("Processing...");
    $jQuery("[id$=hdnSelectedNode]")[0].value = "";
    var btn = $jQuery('[id$=btnUpdateTree]');
    btn.click();
    var ddlTenant = $find($jQuery("[id$=ddlTenant]")[0].id);
    if (ddlTenant != undefined) {//&& ValidatePackageDropdown() //Commented code to display the right panel after intitution selection package is not required.
        //UAT-1116: Package selection combo box on package screens

        window.setTimeout(function () { $jQuery("[id$=ifrDetails]").attr('src', "packagelist.aspx?SelectedTenantId=" + ddlTenant._value); }, 500);
    }
    else {
        window.setTimeout(function () { $jQuery("[id$=ifrDetails]").attr('src', ""); }, 500);
        //$jQuery("[id$=ifrDetails]").attr('src', "");
    }
}


function ResetTimer() {
    var hdntimeout = $jQuery('[id$=hdntimeout]');  //, $jQuery(parent.theForm));
    if (hdntimeout != null) {
        var timeout = hdntimeout.val();
        parent.StartCountDown(timeout);
    }
}

function NavigateToSelectedNode(url) {
    //        window.setTimeout(function () {
    //            $find($jQuery("[id$=pnUpper]")[0].id).SetScrollPos(0, scrollPosition);
    //        }, 200);

    $jQuery("[id$=ifrDetails]").attr('src', url);
    loadDefaultDetailPage = false;
}
