
var treeObj;
function ClientNodeClicked(sender, eventArgs) {
    treeObj = sender;
    var node = eventArgs.get_node();
    $jQuery("#hdnCurrentNode").val(node.get_text());
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function returnToParent() {
    var node = treeObj.get_selectedNode();
    $jQuery("#hdnCurrentNode").val(node.get_text());

    //create the argument that will be returned to the parent page
    var oArg = new Object();

    oArg.controlName = $jQuery("#hdnCurrentNode").val();
    oArg.NavigationUrl = node.get_parent().get_value();

    //get a reference to the current RadWindow
    var oWnd = GetRadWindow();

    //Close the RadWindow and send the argument to the parent page
    if (oArg.controlName) {
        oWnd.Close(oArg);
    }
    else {
        alert("Select at least one user control.");
    }
}

// To close the popup.
function ClosePopup() {
    //AD: Changing code to use latest lib function
    //parent.Page.closeWindow();
    top.$window.get_radManager().getActiveWindow().close();
}