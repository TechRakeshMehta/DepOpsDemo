var loadDefaultDetailPage = true;
var compliancePackageID;
var selectedTenantId;
var winopen = false;
var _menuItemVal;

$jQuery(document).ready(function () {
    var ddlTenant = $find($jQuery("[id$=ddlTenant]")[0].id);

    //UAT-1116: Package selection combo box on package screens
    if (ddlTenant != undefined && loadDefaultDetailPage == true && $jQuery("[id$=hdnIsSearchClicked]")[0].value != "") {
        $jQuery("[id$=ifrDetails]").attr('src', "packagelist.aspx?SelectedTenantId=" + ddlTenant._value);
        //        window.setTimeout(function () { $jQuery("[id$=ifrDetails]").attr('src', "packagelist.aspx?SelectedTenantId=" + ddlTenant.control._selectedValue); }, 500);
    }
    //    var scrollPosition;
    //    window.setTimeout(function () {
    //        debugger;
    //        scrollPosition = parseInt($jQuery("[id$=hdnScrollPosition]")[0].value, 10);
    //        if (scrollPosition != undefined && scrollPosition != null && scrollPosition != "" && scrollPosition != 'NaN') {
    //            var sender = $find($jQuery("[id$=treePackages]")[0].id);
    //            var treestart = sender._getTotalOffsetTop(sender.get_element());
    //            var selectednodepos = sender._getTotalOffsetTop(sender.get_selectedNode().get_contentElement());
    //            var treenodedepth = selectednodepos - treestart;
    //            scrollPosition = scrollPosition + treenodedepth;
    //            $find($jQuery("[id$=pnUpper]")[0].id).SetScrollPos(0, scrollPosition);
    //            $jQuery("[id$=hdnScrollPosition]")[0].value = "";
    //        }
    //    }, 500);
});


function ClientContextMenuItemClicked(sender, eventArgs) {
    openPopUp(compliancePackageID, selectedTenantId, eventArgs._menuItem.get_value());
}

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

//UAT-1116: Package selection combo box on package screens
//Function to validate Package checkbox selection
function ValidatePackageDropdown() {
    var checkedItems = $jQuery("[id$=ddlPackage]")[0].control.get_checkedItems();

    if (checkedItems.length > 0)
        return true;
    else
        return false;
}

function ResetTimer() {
    var hdntimeout = $jQuery('[id$=hdntimeout]');  //, $jQuery(parent.theForm));
    if (hdntimeout != null) {
        var timeout = hdntimeout.val();
        parent.StartCountDown(timeout);
    }
}

function openPopUp(compliancePackageID, selectedTenantId, menuItemVal) {
    var packageCopyScreenWindowName = "packageCopyScreen";
    _menuItemVal = menuItemVal;
    ResetTimer();
    if (_menuItemVal == "MenuItemCopyOtherClient") {
        var url = $page.url.create("~/ComplianceAdministration/Pages/PackageCopyToOtherClient.aspx?TenantId=" + selectedTenantId + "&CompliancePackageID=" + compliancePackageID + "&menuItemValue=" + menuItemVal);
    }
    else { var url = $page.url.create("~/ComplianceAdministration/Pages/PackageCopy.aspx?TenantId=" + selectedTenantId + "&CompliancePackageID=" + compliancePackageID + "&menuItemValue=" + menuItemVal); }
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (50 / 100);

    var win = $window.createPopup(url, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: packageCopyScreenWindowName, onclose: OnClientClose });
    winopen = true;
}

function OnClientClose(oWnd, args) {
    oWnd.remove_close(OnClientClose);
    if (winopen) {
        var arg = args.get_argument();

        if (arg && arg.PackageName != undefined && arg.PackageId != undefined) {
            var packageNameList = arg.PackageName.split(",");
            var packageIdList = arg.PackageId.split(",");
            if (_menuItemVal != undefined && _menuItemVal != null && _menuItemVal.toLowerCase() != 'menuitemcopymaster' && _menuItemVal.toLowerCase() != 'menuitemcopyclient'
                && packageIdList != '' && packageNameList != '') {
                for (var i = 0; i < packageIdList.length; i++) {
                    AddNewPackage(packageIdList[i], packageNameList[i]);
                }
            }
        }
        winopen = false;
        RefreshTree();
    }
    //oWnd.set_title('');

}

function ClientNodeDropping(sender, eventArgs) {
    //    var scrollPos;
    //    var treestart = sender._getTotalOffsetTop(sender.get_element());
    //    var selectednodepos = sender._getTotalOffsetTop(eventArgs.get_sourceNode().get_contentElement());
    //    var treenodedepth = selectednodepos - treestart;
    //    scrollPos = $find($jQuery("[id$=pnUpper]")[0].id).GetScrollPos();
    //    if (scrollPos != "" && scrollPos != undefined) {
    //        $jQuery("[id$=hdnScrollPosition]")[0].value = scrollPos.top - treenodedepth;
    //    }
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

function NavigateToSelectedNode(url) {
    //        window.setTimeout(function () {
    //            $find($jQuery("[id$=pnUpper]")[0].id).SetScrollPos(0, scrollPosition);
    //        }, 200);

    $jQuery("[id$=ifrDetails]").attr('src', url);
    loadDefaultDetailPage = false;
}

//UAT-1116: Package selection combo box on package screens
function AddNewPackage(newPackageId, packageName) {
    var control = $jQuery("[id$=ddlPackage]")[0];
    if (control != undefined || control != null) {
        var combo = $find(control.id);
        if (combo != undefined || combo != null) {
            combo.trackChanges();
            var items = combo.get_items();
            var comboItem = new Telerik.Web.UI.RadComboBoxItem();
            comboItem.set_text(packageName);
            comboItem.set_value(newPackageId);
            items.add(comboItem);
            combo.commitChanges();
            //var item = combo.findItemByValue(newPackageId);
            //if (item != null)
            //    item.set_checked(true);
        }
    }
}


function UpdatePackageDropDownPkg(newPackageId, packageName) {
    var control = $jQuery("[id$=ddlPackage]")[0];
    if (control != undefined || control != null) {
        var combo = $find(control.id);
        if (combo != undefined || combo != null) {
            combo.trackChanges();
            combo.commitChanges();
            var item = combo.findItemByValue(newPackageId);
            if (item != null) {
                item.set_text(packageName);
                combo.commitChanges();
            }
        }
    }

}

function ClearRightPanel() {
    var hdnIsSearchClicked = $jQuery("[id$=hdnIsSearchClicked]");
    if (hdnIsSearchClicked != undefined && hdnIsSearchClicked != null)
        hdnIsSearchClicked.val("");
    window.setTimeout(function () { $jQuery("[id$=ifrDetails]").attr('src', ""); }, 500);
}

function pageLoad() {
    SetDefaultButtonForSection("divSearchPanel", "btnSearch", true);
    var ddlPackage = $jQuery("[id$=ddlPackage]");
    if (ddlPackage != undefined && ddlPackage != null && ddlPackage.length > 0) {
        closeCmbBoxOnTab($find(ddlPackage[0].id));
    }
}

function closeCmbBoxOnTab(sender, e) {
    if (sender.get_dropDownVisible()) {
        sender.hideDropDown();
    }
}