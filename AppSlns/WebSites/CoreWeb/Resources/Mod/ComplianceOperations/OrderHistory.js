//click on link button while double click on any row of grid.  
function grd_rwDbClick(s, e) {
    var _id = "btnViewDetails";
    var b = e.get_gridDataItem().findControl(_id);
    if (b && typeof (b.click) != "undefined") { b.click(); }
}

var winopen = false;

function openSummaryWithOrderID(sender) {
    var btnID = sender.id;
    var containerID = btnID.substr(0, btnID.indexOf("lbtnOrderSummary"));
    var TenantId = $jQuery("[id$=hfTenantId]").val();
    var hdnfOrderID = $jQuery("[id$=" + containerID + "hdnfSummaryOrderID]").val();
    var documentType = "RecieptDocument";
    var reportType = "OrderSummaryReciept";
    var composeScreenWindowName = "Print Receipt";

    if ($jQuery("[id$=hdnPrintReceiptPopupText]") != undefined)
    {
        composeScreenWindowName = $jQuery("[id$=hdnPrintReceiptPopupText]").val();
    }

    var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + hdnfOrderID + "&DocumentType=" + documentType + "&ReportType=" + reportType + "&tenantId=" + TenantId + "&popupTitle=" + composeScreenWindowName);
    //var win = $window.createPopup(url, { size: "800,600", behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url,
                                     {
                                         size: "800,"+popupHeight,
                                         behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move
                                     },
                                     function () {
                                         //this.set_title("Print Receipt");
                                         this.set_title(composeScreenWindowName);
                                         this.set_destroyOnClose(true);
                                         this.set_status("");
                                     });

    winopen = true;
    return false;
}

function openReportWithOrderID(sender) {
    var btnID = sender.id;
    var containerID = btnID.substr(0, btnID.indexOf("lbtnOrderCompletion"));
    var TenantId = $jQuery("[id$=hfTenantId]").val();
    var hdnfOrderID = $jQuery("[id$=" + containerID + "hdnfOrderID]").val();
    var documentType = "ReportDocument";
    var reportType = "OrderCompletion";
    var composeScreenWindowName = "Report Detail";
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + hdnfOrderID + "&DocumentType=" + documentType + "&ReportType=" + reportType + "&tenantId=" + TenantId);
    var win = $window.createPopup(url, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
    winopen = true;
    return false;
}


function OnClientClose(oWnd, args) {
    oWnd.remove_close(OnClientClose);
    if (winopen) {
        winopen = false;
    }
}

function setPostBackSourceOH() {
    $jQuery('.postbacksource').val('OH');
    window.DashboardChildClick = 1;
}

function DownloadForm(url) {
    //debugger;
    location.href = url;
}

function DownloadServiceForm(url) {
    //debugger;
    location.href = url;
}

$page.showAlertMessageWithTitle = function (msg, msgtype, overriderErrorPanel) {
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

        var dialog = $window.showDialog($jQuery("#pageMsgBoxSchuduleInv").clone().show(), { closeBtn: { autoclose: true, text: "Ok", click: function () { ReloadScreen(); } } }, 400, c);
        var dialogId = dialog.get_id();
        parent.$jQuery("[id*='" + dialogId + "'] a.rwCloseButton").hide();
    }
    else {
        $jQuery("#pageMsgBoxSchuduleInv").fadeIn().children("span")[0].innerHTML = msg;
        $jQuery("#pageMsgBoxSchuduleInv").fadeIn().children("span").attr("class", msgtype);

    }
}

function ReloadScreen() {
    window.location.reload();
    //location.href = window.location;
}
//UAT 2444
function setCancelConfirmationText(confirmationText) {
    var isOKClicked = confirm(confirmationText);
   // alert(isOKClicked);
    if (isOKClicked)
        return true;
    else
        return false; 
  
}
  
function OpenItemPaymentFormOrderHistory(url) {
    var popupWindowName = "Item Payment";
    var widht = (window.screen.width) * (90 / 100);
    var height = (window.screen.height) * (80 / 100);
    var popupsize = widht + ',' + height;
    var url = $page.url.create(url + "?popupHeight=" + parseInt(height));
    var win = $window.createPopup(url, { size: popupsize, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnCloseItemPymtPopup }
       );
    return false;
}

function OnCloseItemPymtPopup(oWnd, args) {
    oWnd.remove_close(OnClose);
    win = false;
    $jQuery("[id$=btnUpdateOrderDetails]").click();
}


//function openOrderPayment() {

//    var TenantId = $jQuery("[id$=hfTenantId]").val();
//    //var OrderId = $jQuery(ctrl).attr("oid");
//    var popupWindowName = "Order Payment Details";
//    var hdfFingerPrint = $jQuery("[id$=hdfFingerPrint]").val();
//    var hdfPassport = $jQuery("[id$=hdfPassport]").val();
//    var OrderId = $jQuery("[id$=hdfOrderID]").val();
//    var widht = (window.screen.width) * (90 / 100);
//    var height = (window.screen.height) * (80 / 100);
//    var popupsize = widht + ',' + height;
//    var a = "~/ComplianceOperations/Pages/OrderPaymentDetails.aspx?TenantId=" + TenantId + "&OrderId=" + OrderId + "&hdfFingerPrint=" + hdfFingerPrint + "&hdfPassport=" + hdfPassport;
//    var url = $page.url.create(a);
//    var win = $window.createPopup(url, { size: popupsize, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnCloseItemPymtPopup }
//    );
//    return false;

//}

//function openModifyShippingPopup(OrderID) {
//    var TenantId = $jQuery("[id$=hfTenantId]").val();
//    //var hdnfOrderID = $jQuery("[id$=hdnfModifyShippingOrderID]").val();
//    var composeScreenWindowName = "Modify Shipping";
//    var popupHeight = $jQuery(window).height() * (70 / 100);
//    var url = $page.url.create("~/ComplianceOperations/ModifyShippingInfo.aspx?OrderID=" + OrderID + "&tenantId=" + TenantId);
//    var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnCloseModifyShippingPopup });
//    winopen = true;
//    return false;
//}

//function OnCloseModifyShippingPopup(oWnd, args) {
//    oWnd.remove_close(OnCloseModifyShippingPopup);
//    if (winopen) {
//        winopen = false;
//    }
//}