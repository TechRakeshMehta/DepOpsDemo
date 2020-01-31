var loadDefaultDetailPage = true;
var compliancePackageID;
var selectedTenantId;
var winopen = false;
var _isPackageSelected = true;
var _SelectedPackageIds = "";
var _newPackageName = "";

$jQuery(document).ready(function () {

    var ddlTenant = $find($jQuery("[id$=ddlTenant]")[0].id);

    if (ddlTenant != undefined && loadDefaultDetailPage == true) {
        // $jQuery("[id$=ifrDetails]").attr('src', "SetUpServiceForServicegroup.aspx?SelectedTenantId=" + ddlTenant.control._selectedValue);
        //$jQuery("[id$=ifrDetails]").attr('src', "SetUpServiceGroupsForPackage.aspx?SelectedTenantId=" + ddlTenant.control._selectedValue);
        if (ddlTenant._value > 0) {
            $jQuery("[id$=ifrDetails]").attr('src', "SetupPackagesForInsitiute.aspx?tenantId=" + ddlTenant._value);
        }//        window.setTimeout(function () { $jQuery("[id$=ifrDetails]").attr('src', "packagelist.aspx?SelectedTenantId=" + ddlTenant.control._selectedValue); }, 500);
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
    else {
        $jQuery("[id$=hdnSelectedNode]")[0].value = sender.get_selectedNode().get_value();
    }
}

function RefreshTree() {

    Page.showProgress("Processing...");
    $jQuery("[id$=hdnSelectedNode]")[0].value = "";
    var btn = $jQuery('[id$=btnUpdateTree]');
    btn.click();
    var hdnIsSearchClicked = $jQuery("[id$=hdnIsSearchClicked]")[0].value
    var ddlTenant = $find($jQuery("[id$=ddlTenant]")[0].id);
    if (ddlTenant != undefined && (ddlTenant._value != 0 || ddlTenant._value != "") && hdnIsSearchClicked == "1") {
        window.setTimeout(function () { $jQuery("[id$=ifrDetails]").attr('src', "SetupPackagesForInsitiute.aspx?tenantId=" + ddlTenant._value); }, 500);
    }
    else {
        window.setTimeout(function () { $jQuery("[id$=ifrDetails]").attr('src', ""); }, 500);
    }

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
    ResetTimer();
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (65 / 100);

    var url = $page.url.create("~/ComplianceAdministration/Pages/PackageCopy.aspx?TenantId=" + selectedTenantId + "&CompliancePackageID=" + compliancePackageID + "&menuItemValue=" + menuItemVal);
    var win = $window.createPopup(url, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: packageCopyScreenWindowName, onclose: OnClientClose });
    winopen = true;
}

function OnClientClose(oWnd, args) {
    oWnd.remove_close(OnClientClose);
    if (winopen) {
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
//-------Start--Package dropDown changes :UAT-1116: Package selection combo box on package screens------
//Function to validate Background Package checkbox selection
function ValidateBackgroundPackage(Sender, args) {
    var checkedItems = $jQuery("[id$=chkBkgPackages]")[0].control.get_checkedItems();
    if (checkedItems.length > 0) {
        args.IsValid = true;
        _isPackageSelected = true;
        return false;
    }
    _isPackageSelected = true;
    args.IsValid = true;
}

//function to load the tree on search button click
function LoadTree() {

    if (_isPackageSelected) {
        $jQuery("[id$=hdnIsSearchClicked]")[0].value = "1";
        RefreshTree();
    }

}

//Mwthod to add the recently added package from add new package panel in package dropdown. 
function AddNewPackageInDropDown(newPackageId, packageName) {

    var control = $jQuery("[id$=chkBkgPackages]")[0];
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

//Update package name in package dropdown.
function UpdatePackageInDropDown(newPackageId, packageName, isPackageDeleted) {
    var control = $jQuery("[id$=chkBkgPackages]")[0];
    if (control != undefined || control != null) {
        var combo = $find(control.id);
        if (combo != undefined || combo != null) {
            combo.trackChanges();
            var item = combo.findItemByValue(newPackageId);
            if (item != null) {
                if (isPackageDeleted == "true") {
                    combo.get_items().remove(item);
                }
                else {
                    item.set_text(packageName);
                }
                combo.commitChanges();
            }
        }
    }

}

function ClearRightPanel() {
    var hdnIsSearchClicked = $jQuery("[id$=hdnIsSearchClicked]");
    if (hdnIsSearchClicked != undefined && hdnIsSearchClicked != null)
        hdnIsSearchClicked.val("0");
    window.setTimeout(function () { $jQuery("[id$=ifrDetails]").attr('src', ""); }, 500);
}
//-------END--Package dropDown changes :UAT-1116: Package selection combo box on package screens-----
