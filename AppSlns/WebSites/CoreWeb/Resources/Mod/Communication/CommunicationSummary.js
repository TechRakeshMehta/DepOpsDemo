function grdCommunicationSummary_Command(sender, args) {

    if (args.get_commandName().toLowerCase() == "viewcontent") {
        args.set_cancel(true);
        var selectedId = sender.get_masterTableView().get_dataItems()[args.get_commandArgument()].getDataKeyValue("SystemCommunicationID");
        var url = $page.url.create("~/Messaging/Pages/CommunicationNotificationPreview.aspx?sysCommId=" + selectedId);
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url, { size: "650,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload || Telerik.Web.UI.WindowBehaviors.Resize });
    }
}

var selected = [];
var selectedIds = [];


var selectedSubEvents = [];
var selectedSubEventIds = [];

function grdCommunicationSummary_RowSelected(sender, args) {

    var systemCommunicationDeliveryId = args.getDataKeyValue("SystemCommunicationDeliveryID");
    
    var receiverOrganizationId = args.getDataKeyValue("ReceiverOrganizationUserId");
    if (!selected[systemCommunicationDeliveryId]) {
        selected[systemCommunicationDeliveryId] = true;
        selectedIds.push(systemCommunicationDeliveryId);
    }

}
function grdCommunicationSummary_RowDeselected(sender, args) {
    var systemCommunicationDeliveryId = args.getDataKeyValue("SystemCommunicationDeliveryID");
    

    if (selected[systemCommunicationDeliveryId]) {
        selected[systemCommunicationDeliveryId] = null;

        var index = selectedIds.indexOf(systemCommunicationDeliveryId);
        selectedIds.splice(index, 1);
    }
}

function grdCommunicationSummary_RowCreated(sender, args) {
    var systemCommunicationDeliveryId = args.getDataKeyValue("SystemCommunicationDeliveryID");
    if (selected[systemCommunicationDeliveryId]) {
        args.get_gridDataItem().set_selected(true);
    }
}
function GridCreated(sender, eventArgs) {
    var masterTable = sender.get_masterTableView(),
              headerCheckBox = $telerik.$(masterTable.HeaderRow).find(":checkbox")[0];

    if (headerCheckBox) {
        headerCheckBox.checked = masterTable.get_selectedItems().length == masterTable.get_pageSize();
    }
}

function ValidateSelection(sender, eventArgs) {
    if (selectedIds.length == 0) {
        sender.set_autoPostBack(false);
        $alert("Please select an email to re-send.");
    }
    else {
        sender.set_autoPostBack(true);
        FSObject.$("[id*=hdfSelectedIds]").val(selectedIds);
        var grid = $find($jQuery("[id*=grdCommunicationSummary]")[0].id)
        grid.clearSelectedItems();
        selected = [];
        selectedIds = [];
    }
}

var minDate = new Date("01/01/1980");
function SetMinDate(picker) {
    picker.set_minDate(minDate);
}

