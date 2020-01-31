

function MaangeNotiication(sender, args) {
    sender.set_autoPostBack(false);
    var uid = args._commandArgument;
    var cid = $jQuery("[id$=hdfCrntUsrId]").val();

    $jQuery.ajax({
        type: "POST",
        url: "Default.aspx/UpdateInternalMsgNotificationSettings",
        data: '{id:"' + uid + '", cid:"' + cid + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (successResponse) {
            var _initialText = sender._text;
            if (successResponse.d == true && _initialText.toLowerCase() == "yes") {
                sender.set_text("No");
                sender.set_toolTip("Click to allow")
            }
            else if (successResponse.d == true && _initialText.toLowerCase() == "no") {
                sender.set_text("Yes");
                sender.set_toolTip("Click to disallow")
            }
        },
        failure: function (failureResponse) {
        }
    });
}


function ManageTwoFactorAuthentication(sender, args) {
    sender.set_autoPostBack(false);
    var cid = $jQuery("[id$=hdnCurrentUserId]").val();
    var uid = $jQuery("[id$=hdnUserId]").val();
    var tenantId = $jQuery("[id$=hdnTenantId]").val();
    var message = "Are you sure you want to disable Google Authentication?";

    $confirm(message, function (res) {
        if (res) {
            $jQuery.ajax({
                type: "POST",
                url: "Default.aspx/UpdateUserTwoFactorAuthenticationSettings",
                data: '{Userid:"' + uid + '", currentUserid:"' + cid + '",tenantId:"' + tenantId + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (successResponse) {
                    var _initialText = sender._text;
                    if (successResponse.d == true && _initialText.toLowerCase() == "enabled") {
                        sender.set_text("Disabled");
                        sender.set_toolTip("Disabled")
                        sender.set_enabled(false);
                    }
                },
                failure: function (failureResponse) {
                }
            });
        }
        else {
            return false;
        }
    }, 'Complio', true);
}