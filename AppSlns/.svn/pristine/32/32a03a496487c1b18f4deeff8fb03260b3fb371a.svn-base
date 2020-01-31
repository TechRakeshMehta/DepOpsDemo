//Following 2 methods will be used for toolbar
function navigateSearch() {
    var query = FSObject.$("[id$=queryField]");
    if (query.val()) {
        //$get('ifrPage').src = query.val();

        //AD: Changing code to use latest lib function
        //Page.Frame.loadPage(query.val());
        $page.app.modFrame.navigate(query.val());
    }
    query.val('');
}

function OnKeyPress(sender, eventArgs) {
    if ((eventArgs._keyCode == 36 || eventArgs._keyCode == 35) && window.event.shiftKey == false && navigator.appName == "Netscape") {
        return;
    }
    var k = new RegExp("[</>~#;^()&,:!$*?]$");
    if (k.test(eventArgs._keyCharacter)) {
        eventArgs._cancel = true;
    }
}

function NumericOnly(sender, eventArgs) {
    var k = new RegExp("[0-9]$");
    if (!k.test(eventArgs._keyCharacter)) {
        eventArgs._cancel = true;
    }
}
//FSObject.onReady(function () {
//    debugger;
//    FSObject.$("[id$=txtSearchCriteria]")
//    .keypress(function () {
//        FSObject.$("input:regex([name,/\-/g])").css({ color: "red" });
//    });
// });


$jQuery.support.cors = true;
$page.dataStore.setItem("NTFY_NV", 0);
var ntfyAlerts_ClientUpdated = function (sender, args) {
    var NTFY_NV = $page.dataStore.getItem("NTFY_NV");
    var NTFY_ND = $page.dataStore.getItem("NTFY_ND");
    var NTFY_OV = $page.dataStore.getItem("NTFY_OV");
    if (NTFY_OV != NTFY_NV) {
        $jQuery(sender.get_popupElement()).children(".RadXmlHttpPanel").first().html("<div class='rnContentWrapper'><div class='rnContent'>" + NTFY_ND + ' ' + " <p><a href=\"../Alerts/Default.aspx\">See All Alerts</a></p>" + "</div></div>");
        sender.show();
    }
    //console.log("Old: " + NTFY_OV + " New: " + NTFY_NV);
}

var ntfyAlerts_ClientUpdating= function (sender, args) {
    $page.dataStore.setItem("NTFY_OV", $page.dataStore.getItem("NTFY_NV"));
    $page.dataStore.setItem("NTFY_NV", args.get_content().QueueNo);
    $page.dataStore.setItem("NTFY_ND", args.get_content().Description );
}

