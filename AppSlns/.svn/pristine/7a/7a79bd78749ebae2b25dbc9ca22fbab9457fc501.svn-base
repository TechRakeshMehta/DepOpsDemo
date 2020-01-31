var dialog = null;

var ProfileSharingConfirmation =
    {
        GetProfileSharingInvitationConfirmation: function () {
            $jQuery.ajax({
                type: "POST",
                url: "/ProfileSharing/Default.aspx/GetProfileSharingInvitationConfirmation",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var res = JSON.parse(data.d);
                    if (data.d != "{}") {
                            var groupId = res.GroupId;
                            var PSIC_Id = res.PSIC_Id;
                            var PSIC_IsSuccess = res.PSIC_IsSuccess;

                            if (dialog == null || dialog.IsClosed()) {

                                if (PSIC_IsSuccess.toLowerCase() == "true") {
                                    $jQuery("#dialog-message #spnGroupId").text(groupId);
                                    dialog = $window.showDialog($jQuery("#dialog-message").clone().show(), { closeBtn: { autoclose: true, text: "Ok", click: function () { ProfileSharingConfirmation.MarkIsViewedByInvitationConfirmationId(PSIC_Id); } }, }, 400, "Profile Sharing Completed");
                                }
                                else {
                                    var errorMsg = res.ErrorMsg;
                                    $jQuery("#dialog-message-error #spnErrorText").text(errorMsg);
                                    dialog = $window.showDialog($jQuery("#dialog-message-error").clone().show(), { closeBtn: { autoclose: true, text: "Ok", click: function () { ProfileSharingConfirmation.MarkIsViewedByInvitationConfirmationId(PSIC_Id); } } }, 400, "Unable To Share Profile");
                                }

                                var dialogId = dialog.get_id();
                                $jQuery("[id *= '" + dialogId + "'] a.rwCloseButton").hide();
                            }
                        }
                }
            });
        },
        MarkIsViewedByInvitationConfirmationId: function (PSIC_Id) {
            $jQuery.ajax({
                type: "POST",
                url: "/ProfileSharing/Default.aspx/MarkIsViewedByInvitationConfirmationId",
                data: "{'PSIC_Id': '" + PSIC_Id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                }
            });
    },
    IsNeedToStartPolling: function () {
        $jQuery.ajax({
            type: "POST",
            url: "/ProfileSharing/Default.aspx/IsNeedToStartPolling",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var res = data.d;
                if (data.d != "{}") {
                    window.parent.$jQuery('[id$=hdnStartPolling]').val(res.toLowerCase());
                }
            }
        });
    }
};


