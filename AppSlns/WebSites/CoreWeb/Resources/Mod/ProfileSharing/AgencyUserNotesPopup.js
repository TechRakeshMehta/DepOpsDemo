//Open popup for adding rejection notes
function NotesPopup(ClinicalRotationID, TenantID, InvitationId, ScreenType, rotationInvitationIds) {
    var popupWindowName = "Add Rejection Reason";
    winopen = true;
    var url = $page.url.create("~/ProfileSharing/Pages/AgencyUserNotesPopup.aspx?RotationId=" + ClinicalRotationID + "&InvitationID=" + InvitationId + "&SelectedTenantId=" + TenantID + "&SrcScreen=" + ScreenType + "&RotationInvitationIds=" + rotationInvitationIds);

    var win = $window.createPopup(url, { size: "500,200", behaviors: Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnCloseAgencyUserNotesPopUp }, function () {
        this.set_title(popupWindowName);
    });
    return false;
}

function OnCloseAgencyUserNotesPopUp(oWnd, args) {
    oWnd.remove_close(OnCloseAgencyUserNotesPopUp);
    var arg = args.get_argument();
    if (arg) {
        if (arg.IsStatusSaved != undefined && arg.IsStatusSaved == true) {
            jQuery("[id$=hdnIsNotesDataSaved]").val("True");
            jQuery("[id$=btnDoPostBackForNotes]").click();
        }
        else
            jQuery("[id$=hdnIsNotesDataSaved]").val("False");
    }
}