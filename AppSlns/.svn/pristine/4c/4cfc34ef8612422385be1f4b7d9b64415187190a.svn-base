function DeleteDocument() {
    if (confirm('Are you sure you want to delete this document?') == true) {
        $jQuery('[id$=hdnIsdocumentDeleted]').val("1");
        return true;
    }
    else return false;
}

function OnClientCloseOnSubmit(oWnd, args) {
    oWnd.remove_close(OnClientCloseOnSubmit);
}

function ClosePopup(TicketId) {
    var oArg = {};
    oArg.TicketID = TicketId;
    var oWnd = GetRadWindow();
    oWnd.Close(oArg);
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function showMessage(msg) {
    debugger;
    //var btn = $jQuery('[id$=btnDummy]', $jQuery(parent.theForm));
    //btn.click();
    alert(msg);
}

function ClearDropDownText(sender, args) {
    var checkedItems = sender.get_checkedItems();
    if (checkedItems.length == 0) {
        sender.clearSelection();
    }
}