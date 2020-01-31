var loadDefaultDetailPage = true;
var compliancePackageID;
var selectedTenantId;
var winopen = false;
var _menuItemVal;

$jQuery(document).ready(function () {

});

function OpenDefaultScreen() {
    var hdnDefaultScreen = $jQuery("[id$=hdnDefaultScreen]")[0];
    if (hdnDefaultScreen != undefined && hdnDefaultScreen.value != "") {
        $jQuery("[id$=ifrDetails]").attr('src', hdnDefaultScreen.value);
        hdnDefaultScreen.value = "";
    }
    else {
        $jQuery("[id$=ifrDetails]").attr('src', "");
    }
}

function GetSelectedNode(sender, eventArgs) {
    $jQuery("[id$=hdnSelectedNode]")[0].value = eventArgs.get_node().get_value();
}

function SetTreeNode(sender, eventArgs) {
    if (sender.get_selectedNode() != null && sender.get_selectedNode() != undefined) {
        $jQuery("[id$=hdnSelectedNode]")[0].value = sender.get_selectedNode().get_value();
    }
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
    if ($jQuery("[id$=hdnDefaultScreen]")[0].value != "") {
        OpenDefaultScreen();
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

    $jQuery("[id$=ifrDetails]").attr('src', url);
    loadDefaultDetailPage = false;
}

//UAT-3078 :
function ClientNodeDropping(sender, eventArgs) {
    if (eventArgs.get_sourceNodes().length > 1 || sender.get_selectedNodes().length > 1) {
        alert("error");
    }
    if (eventArgs.get_sourceNode() == null || eventArgs.get_sourceNode() == undefined || eventArgs.get_destNode() == null || eventArgs.get_destNode() == undefined) {
        eventArgs.get_sourceNode().unselect()
        eventArgs.set_cancel(true);
        return;
    }
    if (eventArgs.get_sourceNode().get_level() != eventArgs.get_destNode().get_level() || eventArgs.get_sourceNode().get_parent() != eventArgs.get_destNode().get_parent()) {
        // $alert("You cannot drag nodes between levels");
        eventArgs.get_sourceNode().unselect()
        eventArgs.set_cancel(true);
    }
    else {
        $jQuery("[id$=hdnSelectedNode]")[0].value = eventArgs.get_sourceNode().get_value();
        $jQuery("[id$=hdnIfDragEventIsFired]")[0].value = true;
    }
}


