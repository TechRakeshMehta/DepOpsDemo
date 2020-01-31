
/////////////////////////////////////////////////////////////
//Changed code based on new lib

if (typeof (IWEBLIB) != "undefined" && IWEBLIB) {

    $page.add_pageLoad(function () {

        //Updating menu on page load if change came from the server        
        if ($jQuery("#" + $page.IDBag.getID("menu_trigger")).val()) {
            //$page.app.menu.update();

        }
    });
}

/////////////////////////////////////////////////////////////

function ClientNodeClicked(sender, eventArgs) {
    var node = eventArgs.get_node();
    $jQuery("#hdnCurrentNode").val(node.get_text());
}

var targetControlID;
var targetNavigationURL;

function openWin(trgControlId, txtNavigationUrl) {

    var chckbox = $jQuery('[id$="chkIsParent"]')[0].checked;

    if (chckbox) {
        $alert("<h1>Request Aborted!</h1><br/>Please uncheck Is this parent Feature?", "Web Page Name");
        return false;
    } else {
        var selectedBusinessChannel = $jQuery("[id$=cmbBusinessChannel]").val();
        targetControlID = trgControlId;
        targetNavigationURL = txtNavigationUrl;
        var controlName = $jQuery("#" + trgControlId).val();
        var controlFullPath = $jQuery("#" + txtNavigationUrl).val();
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        $window.createPopup('IntsofSecurityModel/UserControl/UserControlFolderList.aspx?selectedWePageName=' + controlName + '&controlFullPath=' + controlFullPath + '&businessChannel=' + selectedBusinessChannel, { size: "500," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Move, onclose: OnClientClose });
    }

    return false;
}
function OnClientClose(oWnd, args) {

    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        var controlName = arg.controlName;
        $jQuery("#" + targetControlID).val(controlName);
        $jQuery("#hdnCurrentNode").val(controlName);
        $jQuery("#" + targetNavigationURL).val('~/' + arg.NavigationUrl + '/default.aspx?ucid=' + arg.controlName);
        __doPostBack(targetControlID, '');
    }
}

function openWinReport(trgControlId, txtNavigationUrl, viewName, sheetName) {
    var chckbox = $jQuery('[id$="chkIsParent"]')[0].checked;

    if (chckbox) {
        $alert("<h1>Request Aborted!</h1><br/>Please uncheck Is this parent Feature?", "Web Page Name");
        return false;
    } else {

        targetControlID = trgControlId;
        targetNavigationURL = txtNavigationUrl;
        var controlName = $jQuery("#" + trgControlId).val();

        var arysplitSheet = controlName.split(";");
        var controlFullPath = $jQuery("#" + txtNavigationUrl).val();
        var selectedBusinessChannel = $jQuery("[id$=cmbBusinessChannel]").val();
        var popupHeight = 400; //$jQuery(window).height() * (70 / 70);
        if (arysplitSheet.length > 1)
            $window.createPopup('IntsofSecurityModel/UserControl/MapTableauReport.aspx?ViewName=' + arysplitSheet[0] + '&SheetName=' + arysplitSheet[1] + '&businessChannel=' + selectedBusinessChannel, { size: "650," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Move, onclose: OnClientClose });
        else
            $window.createPopup('IntsofSecurityModel/UserControl/MapTableauReport.aspx?ViewName=' + "" + '&SheetName=' + "" + '&businessChannel=' + selectedBusinessChannel, { size: "650," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Move, onclose: OnClientClose });
    }
    return false;
}


function showReqdSpan() {
    $jQuery('#reqdSpanWebPageName').show();
}
function hideReqdSpan() {
    $jQuery('#reqdSpanWebPageName').css('display', 'none');
}

//Below Code is Added in Admin Entry Portal//

function openComponentWin(trgControlId, txtNavigationUrl) {
   
    var chckbox = $jQuery('[id$="chkIsParent"]')[0].checked;

    if (chckbox) {
        $alert("<h1>Request Aborted!</h1><br/>Please uncheck Is this parent Feature?", "Web Page Name");
        return false;
    } else {
        var selectedBusinessChannel = $jQuery("[id$=cmbBusinessChannel]").val();
        targetControlID = trgControlId;
        targetNavigationURL = txtNavigationUrl;
        var controlName = $jQuery("#" + trgControlId).val();
        var controlFullPath = $jQuery("#" + txtNavigationUrl).val();
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        $window.createPopup('IntsofSecurityModel/Pages/AdminPortalComponentList.aspx?selectedWePageName=' + controlName + '&controlFullPath=' + controlFullPath + '&businessChannel=' + selectedBusinessChannel, { size: "500," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Move, onclose: OnClientClosePopup });
}

    return false;
}


function OnClientClosePopup(oWnd, args) {
   
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        var controlName = arg.controlName;
        $jQuery("#" + targetControlID).val(controlName);
        $jQuery("#hdnCurrentNode").val(controlName);
        $jQuery("#" + targetNavigationURL).val(arg.NavigationUrl);
        __doPostBack(targetControlID, '');
    }
}
//End Admin Entry Portal//