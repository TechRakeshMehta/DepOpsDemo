function ViewDocument(TicketDocumentID, DocumentType, IsDownload, tenantId) {
    //debugger;
    var url = $page.url.create("~/ComplianceOperations/Pages/FormViewer.aspx?TicketDocumentID=" + TicketDocumentID + "&DocumentType=" + DocumentType + "&tenantId=" + tenantId);

    //if (IsDownload.toLowerCase() == "false") {
    var popupHeight = $jQuery(window).height() * (100 / 100);
    var win = $window.createPopup(url,
        {
            name: 'Document',
            size: "800," + popupHeight,
            behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal,
            onclose: OnClientClose
        },
        function () {
            this.set_title("Ticket Document");
            //this.setActive("true");
        }
    );

    if (IsDownload.toLowerCase() == "true") {
        win.Close();
    }

    return false;
    //}
    //else {
    //    var url = $page.url.create(url);
    //    location.href = url;
    //}
}

function OnClientClose(oWnd, args) {
    oWnd.remove_close(OnClientClose);
    winopen = false;
}