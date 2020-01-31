function UserAccountLinking() {
    var popupWindowName = "Link an account";
    winopen = true;
    var url = $page.url.create("~/CommonOperations/Pages/AccountLinkingPage.aspx");
    var popupHeight = $jQuery(window).height() * (100 / 100);
    var win = $window.createPopup(url, {
        size: "750," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Move |
            Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnCloseUserAccountLinkingPopUp
    }, function () {
        this.set_title(popupWindowName);
        this.set_destroyOnClose(true);
    });
    return false;
}

function OnCloseUserAccountLinkingPopUp(oWnd, args) {
    oWnd.remove_close(OnCloseUserAccountLinkingPopUp);
    $jQuery('[id$=btnAccountLinkingDoPostBack]').click();
}
