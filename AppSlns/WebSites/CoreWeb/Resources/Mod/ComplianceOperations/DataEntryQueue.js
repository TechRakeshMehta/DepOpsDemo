function OpenMutlipleSubscriptionsPopup() {
    var popupWindowName = "Manage Multiple Subscriptions";
    var fromScreenName = "PortfolioSearch";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    var ApplicantUserID = $jQuery("[id$=hdnApplicantUserID]").val();
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/ComplianceOperations/Pages/PackageSelectionForDataEntry.aspx?TenantID=" + tenantID + "&ApplicantID=" + ApplicantUserID);
    var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnClose }
       );
    return false;
}

//This event fired when multiple subscription popup closed.
function OnClose(oWnd, args) {
    oWnd.remove_close(OnClose);
    var arg = args.get_argument();
    if (arg) {
        if (arg.Action == "Submit") {
            $jQuery("[id$=hdnPackageSubscriptionID]").val(arg.PackageSubscriptionID);
            $jQuery("[id$=hdnApplicantUserID]").val(arg.ApplicantId);
            // __doPostBack("<%= btnRedirect.ClientID %>", "");
            var btnId = $jQuery("[id$=btnRedirect]");
            btnId.click();
        }
    }
}