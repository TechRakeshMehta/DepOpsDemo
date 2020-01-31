

function ClientNodeClicked(sender, eventArgs) {
    var node = eventArgs.get_node();
    $jQuery("#hdnCurrentNode").val(node.get_text());
}

var targetControlID;
var targetNavigationURL;

function openWin(trgControlId, txtNavigationUrl, selectedWePageName) {
    targetControlID = trgControlId;
    targetNavigationURL = txtNavigationUrl;
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    $window.createPopup('IntsofSecurityModel/UserControl/UserControlFolderListForPolicy.aspx?selectedWePageName=' + 'selectedWePageName', {
        size: "500,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Move, onclose: OnClientClose
    });
}

function OnClientClose(oWnd, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        var controlName = arg.controlName;
        $jQuery("#" + targetControlID).val(controlName);
        $jQuery("#hdnCurrentNode").val(controlName);
        $jQuery("#" + targetNavigationURL).val(arg.NavigationUrl + '\\\\' + arg.controlName);
        __doPostBack(targetControlID, '');
    }
}

function OnClientClicked(button, args) {
    if ($jQuery("input[id$=txtControlPath]")[0].value == '') {
        $jQuery("[id$=lblUiCtrError]").html('Control Path is required.');
        button.set_autoPostBack(false);
    } else {
        button.set_autoPostBack(true);
    }
}
