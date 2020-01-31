
if (typeof (IWEBLIB) != "undefined" && IWEBLIB) {
    $page.add_pageLoaded(function () {
        var oArg = {};
        $jQuery(':radio').click(function (e) {
            debugger;
            var completeValue = this.value;

            var itemvalue = this.getAttribute("textid");
            var compPath = this.getAttribute("ComponentPath");

            $findByKey("treecontrolfolder", function () {
                var node = null;
                if (itemvalue.length == 0) {
                    node = null;
                }
                else {
                    node = this.findNodeByValue(itemvalue);
                }
                if (typeof (node) != "undefined" && node != null) {
                    debugger;
                    node.set_selected(true);
                    $jQuery(node.get_element()).find("input").first().prop("checked", true).attr("value", completeValue);
                    $jQuery("#hdnCurrentNode").val(node.get_text());

                    oArg.controlName = $jQuery("#hdnCurrentNode").val();

                    oArg.NavigationUrl = compPath;
                    oArg.NavigationUrl = oArg.NavigationUrl.replace(/\s/g, '');

                    oArg.controlName = oArg.controlName.replace(/\s/g, '');
                    oArg.controlName = oArg.controlName;

                    var jsonString = oArg.controlName + ',' + oArg.NavigationUrl;

                    $jQuery("#hdnCurrentNode").attr("value", jsonString);
                }


            });

        });

    });
}


function ClientTreeNodeClicked(sender, eventArgs) {
    debugger;
    $jQuery("input").prop("checked", false);
    $jQuery("#hdnCurrentNode").val('');
    var node;
    var oArg = {};
    var jNode;

    try {
        node = eventArgs.get_node();
        jNode = $jQuery(node.get_element());
    }
    catch (e) {
        node = eventArgs;
        jNode = $jQuery(node.get_element());
    }
    try {
        if (eventArgs.get_node()._properties._data.expanded == 1) {
            return true;
        }
    }
    catch (e)
    { }

    $jQuery(jNode).find("input").first().prop("checked", true).attr("value", node.get_text());
    $jQuery("#hdnCurrentNode").val(node.get_text());

    oArg.controlName = $jQuery("#hdnCurrentNode").val();

    // var parent = node.get_parent();
    var compPath;
    if (eventArgs.get_node()._textElement != undefined && eventArgs.get_node()._textElement != null) {
        if (eventArgs.get_node()._textElement.firstElementChild != undefined && eventArgs.get_node()._textElement.firstElementChild != null
            && eventArgs.get_node()._textElement.firstElementChild.attributes != undefined && eventArgs.get_node()._textElement.firstElementChild.attributes != null) {

            compPath = eventArgs.get_node()._textElement.firstElementChild.attributes["ComponentPath"].value;
        }
    }

    //if (typeof (parent) != "undefined" && parent != null) {
    if (compPath != undefined && compPath != null) {

        //oArg.NavigationUrl = node.get_parent().get_text();
        oArg.NavigationUrl = compPath;
        oArg.NavigationUrl = oArg.NavigationUrl.replace(/\s/g, '');

        oArg.controlName = oArg.controlName.replace(/\s/g, '');
        oArg.controlName = oArg.controlName;

        var jsonString = oArg.controlName + ',' + oArg.NavigationUrl;
        $jQuery("#hdnCurrentNode").attr("value", jsonString);
    }
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function returnToParent() {
    debugger;
    var hdnCurrentNode = $jQuery("#hdnCurrentNode").attr("value");
    var oArg = {};

    if (hdnCurrentNode != "") {
        var strControlName = hdnCurrentNode.toString().split(',');
        oArg.controlName = strControlName[0];
        oArg.NavigationUrl = strControlName[1];
    }

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

//$page.add_pageReady(function () {
//    debugger;
//    $jQuery(".rtIn").each(function () {
//        var text = $jQuery(this).text();
//        if (text == "Admin Entry Portal") {
//            $jQuery(this).text("Admin Entry Portal");
//        }
//        //if (text == "American Databank Dash Board") {
//        //    $jQuery(this).text("ADB Dashboards");
//        //}
//    });
//});


//$page.add_pageReady(function () {
//    debugger;
//    $jQuery(".rtIn").each(function () {
//        debugger;
//        var text = $jQuery(this).text();
//        if (text == "American Databank") {
//            debugger;
//            $jQuery(this).text("American Databank");
//        }
//        if (text == "American Databank Dash Board") {
//            debugger;
//            $jQuery(this).text("ADB Dashboards");
//        }
//    });
//});