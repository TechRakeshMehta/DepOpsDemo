$page.add_pageLoad(function () {

    //Accessibility Code
    //Adding title to Select All Result checkbox
    $jQuery('[id$=chkSelectAllResults]').attr('title', 'Select all users on all active page(s)');

    //Adding title to header checkbox:
    $jQuery('[id$=chkSelectAll]').attr('title', 'Click this box to select all users on the active page');

    //Adding title grid check boxes:-
    $jQuery("span[title*='Select order to approve'], span[title*='Order already queued for payment approval']").each(function (i, o) {
        var title = $jQuery(o).attr('title');
        $jQuery(o).children().attr('title', title);
    });

    //Adding tool-tip to detail link
    $jQuery("span.rbText:contains('Detail')").each(function (i, e) {
        var orderNumber = $jQuery(e).parent().attr('OrderNumber');
        $jQuery(e).append("<span class='sr-only'>&nbsp;Click here to view details or to review/approve payments or cancellation requests for Order Number : " + orderNumber + "</span>");
    });

});