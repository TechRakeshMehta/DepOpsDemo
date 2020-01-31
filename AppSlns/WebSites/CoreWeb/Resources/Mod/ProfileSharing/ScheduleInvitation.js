function SaveAndReturnToParent(isOnlyRotPkgShare) {
    var oArg = {};
    oArg.Action = "Submit";

    if ($jQuery("[id$=divEffectiveDate]").length > 0) {
        if (Page_ClientValidate()) {
            oArg.EffectiveDate = $jQuery("[id$=dpkrEffectiveDate]").val();
        }
        else {
            return false;
        }
    }
    oArg.IsOnlyRotPkgShare = isOnlyRotPkgShare;
    ClosePopup(oArg);
}

//function to get current popup window
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function ClosePopup(oArg) {
    var oWnd = GetRadWindow();
    if (oArg)
        oWnd.Close(oArg);
    else
        oWnd.Close();
}

$page.showAlertMessageWithTitle = function (msg, msgtype, overriderErrorPanel) {
    msg = $jQuery("[id$=hdnErrorMessage]")[0].value;
    if (typeof (msg) == "undefined") return;
    var c = typeof (msgtype) != "undefined" ? msgtype : "";
    if (overriderErrorPanel) {
        $jQuery("#pageMsgBoxSchuduleInv").children("span")[0].innerHTML = msg;
        $jQuery("#pageMsgBoxSchuduleInv").children("span").attr("class", msgtype);
        if (c == 'sucs') {
            c = "Success";
        }
        else (c = "Validation Message for Tracking Package:");

        $jQuery("[id$=pnlErrorSchuduleInv]").hide();

        $window.showDialog($jQuery("#pageMsgBoxSchuduleInv").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 500, c);
    }
    else {
        $jQuery("#pageMsgBoxSchuduleInv").fadeIn().children("span")[0].innerHTML = msg;
        $jQuery("#pageMsgBoxSchuduleInv").fadeIn().children("span").attr("class", msgtype);

    }
}
