
function OpenScheduleInvitationPopup(isNonScheduled, agencyID, isOnlyRotPkgShare) {
    var popupWindowName = "Schedule Invitation Window";
    winopen = true;
    var url = "ProfileSharing/Pages/ScheduleInvitation.aspx?isNonScheduled=" + isNonScheduled + "&AgencyID=" + agencyID + "&IsOnlyRotPkgShare=" + isOnlyRotPkgShare;
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "1100," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName, onclose: OnScheduleInvitationClose });
    return false;
}

function OnScheduleInvitationClose(oWnd, args) {
    oWnd.remove_close(OnScheduleInvitationClose);
    var arg = args.get_argument();
    if (arg) {
        if (arg.Action == "Submit") {
            $jQuery('[id$=hdnEffectiveDate]').val(arg.EffectiveDate);
            $jQuery('[id$=hdnIsOnlyRotPkgShare]').val(arg.IsOnlyRotPkgShare);
            window.parent.$jQuery('[id$=hdnStartPolling]').val("true");
            var btn = $jQuery('[id$=btnUpdateInvitationUI]');
            btn.click();
        }
    }
}

function OnAddAgencyUserPopup(agencyID, AgencyName, tenantID) {
    var popupWindowName = "Add User Information Window";
    winopen = true;
    var url = "ProfileSharing/Pages/AddUserInformation.aspx?AgencyID=" + agencyID + "&TenantID=" + tenantID + "&AgencyName=" + AgencyName;
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url,
        {
            size: "900," + popupHeight
            , behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName
            , onclose: OnAddAgencyUserInfoClose
        });
    return false;
}

function OnAddAgencyUserInfoClose(oWnd, args) {
    var cssclass = "";
    oWnd.remove_close(OnAddAgencyUserInfoClose);
    if (args != undefined) {
        if (args.get_argument() == "success") {
            cssclass = "sucs";
            var c = typeof (cssclass) != "undefined" ? cssclass : "";
            $jQuery("#pageMsgBox").children("span").text("User saved successfully.").attr("class", cssclass);
            if (c == 'sucs') {
                c = "Success";
            }
            else (c = c.toUpperCase());
            $window.showDialog($jQuery("#pageMsgBox").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 400, c);

            if (parseInt($jQuery('[id$=hdnIsRotationSharing]').val()) == "1") {
                if ($jQuery('[id$=hdnOpenSchInvPopupAfterAddingAU]').val() == "1") {
                    var isNonScheduledInvitation = $jQuery('[id$=hdnIsNonScheduledInvitation]').val();
                    var isOnlyRotPkgShare = $jQuery('[id$=hdnIsOnlyRotPkgShare]').val();
                    var selectedAgencyId = $jQuery('[id$=hdnSelectedAgencyId]').val();

                    if (parseInt(isOnlyRotPkgShare) == 0) {
                        isOnlyRotPkgShare = "true";
                    }
                    else {
                        isOnlyRotPkgShare = "false";
                    }

                    OpenScheduleInvitationPopup(isNonScheduledInvitation, selectedAgencyId, isOnlyRotPkgShare);
                }
                else {
                    $jQuery('[id$=btnPerformRotationPackageSharing]').trigger('click');
                }
            }
        }
        else if (args.get_argument() == "error") {
            cssclass = "error";
            var c = typeof (cssclass) != "undefined" ? cssclass : "";
            $jQuery("#pageMsgBox").children("span").text("Something went wrong.").attr("class", cssclass);
            if (c == 'error') {
                c = "Error";
            }
            else (c = c.toUpperCase());
            $window.showDialog($jQuery("#pageMsgBox").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 400, c);
        }
    }

}