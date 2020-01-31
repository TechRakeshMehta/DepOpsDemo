$page.add_pageLoad(function () {
    //Accessibility Code

    $jQuery("[id$=ddlUserGroup_Input]").attr('role', 'combobox');
    $jQuery("[id$=ddlTenantName_Input]").attr('role', 'combobox');

    $jQuery("div[id*=ucCustomAttributeLoaderSearch] .col-md-3 input:visible").each(function (i, e) {
        var input = $jQuery(this);
        var relatedSpan = $jQuery(input).parents('.col-md-3').children(0);
        var relatedSpanId = $jQuery(relatedSpan).attr('id');

        if ($jQuery(input).attr('type') != 'radio') {
            $jQuery(input).attr('aria-labelledBy', relatedSpanId);
        }
    });

    $jQuery("input[type='radio']").each(function (i, e) {
        var relatedLbl = $jQuery(this).parents('.col-md-3').children().get(0);
        var newLblId = "lblForRbn_" + i;
        var rbnOptionText = $jQuery(this).siblings().attr('id', newLblId);
        $jQuery(this).attr('aria-labelledBy', $jQuery(relatedLbl)[0].id + " " + newLblId);
    });

    $jQuery('[id$=fsucCmdBarButton_btnSave_input]').attr('title', 'Click to search users per the criteria entered above');
    $jQuery('[id$=fsucCmdBarButton_btnSubmit_input]').attr('title', 'Click to remove all values entered in the search criteria above');
    $jQuery('[id$=fsucCmdBarButton_btnExtra_input]').attr('title', 'Click to view passport report');
    $jQuery('[id$=fsucCmdBarButton_btnCancel_input]').attr('title', 'Click to cancel. Any data entered will not be saved');
    $jQuery('[id$=fsucCmdBarButton_btnClear_input]').attr('title', 'Click to send message');
    $jQuery('[id$=fsucCmdBarButton_btnArchieve_input]').attr('title', 'Click to Archive');

    //Adding title to Select All Result checkbox
    $jQuery('[id$=chkSelectAllResults]').attr('title', 'Select all users on all active page(s)');

    //Adding title to header checkbox:
    $jQuery('[id$=chkSelectAll]').attr('title', 'Click this box to select all users on the active page');

    //Adding title to Grid checkbox(s):
    $jQuery('[id$=chkSelectUser]').each(function () {
        var userID = $jQuery(this).parent().attr('UserID');
        $jQuery(this).attr('title', 'Click to select user, For user ID ' + userID);
    });
    //Adding tool-tip to View Portfolio link
    $jQuery("span.rbText:contains('View Portfolio')").each(function (i, e) {
        var userID = $jQuery(e).parent().attr('UserID');
        $jQuery(e).append("<span class='sr-only'>&nbsp; - Click to view the applicant's profile, subscription, and order history details for User ID : " + userID + "</span>");
    });

});