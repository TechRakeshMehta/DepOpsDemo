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
    var hdntimeout = $jQuery('[id$=hdntimeout]');
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