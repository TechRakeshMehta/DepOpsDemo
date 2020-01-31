$page.add_pageLoad(function () {

    applyCalenderAccessibility();

    if ($jQuery('#pageMsgBox').css('display') != 'none') {
        //$jQuery("#pnlError").attr("tabindex", -1).focus();
        $jQuery("#lblError").attr("tabindex", -1).focus();

    }
});

function OnCalenderClosing(DatePicker) {
    if (DatePicker != undefined && DatePicker != null) {
        var Datepickerid = DatePicker._dateInput._clientID;
        $jQuery("[id$=" + Datepickerid + "]").focus();
    }
}

function applyCalenderAccessibility() { 
    //Altering Calender next month links title
    $jQuery("td .rcNext").each(function (element) {
        $jQuery(this).attr('title', "Go to next month");
        $jQuery(this).prepend('<span id="lbl" class="sr-only">Go to next month</span>');
        $jQuery(this).removeattribute = "tabindex";
        $jQuery(this).attr('tabindex', "0");
        $jQuery(this).removeAttr("role");
        $jQuery(this).attr("role", "button");
    });

    //Altering Calender Previous month links title
    $jQuery("td .rcPrev").each(function (element) {
        $jQuery(this).attr('title', "Go to Previous month");
        $jQuery(this).prepend('<span id="lbl" class="sr-only">Go to Previous month</span>');
        $jQuery(this).removeattribute = "tabindex";
        $jQuery(this).attr('tabindex', "0");
        $jQuery(this).removeAttr("role");
        $jQuery(this).attr('role', "button");
    });

    $jQuery("td[role='gridcell']").each(function (element) {
        var title = $jQuery(this).context.title;
      //  $jQuery(this).prepend('<span id="lbl" class="sr-only">' + title + '</span>');
        var anchor = $jQuery(this).find("a");
        anchor.attr("aria-label", title);

    });
    //columnheader
    $jQuery("th[role='columnheader']").each(function (element) {
        $jQuery(this).attr("aria-hidden", "true");
    });
    $jQuery("th[role='rowheader']").each(function (element) {
        $jQuery(this).attr('aria-hidden', "true");
    });

    $jQuery("[id$=calendar_Top]").attr("tabindex", "0");
    $jQuery(".rcTitlebar table").attr("tabindex", "0");
    $jQuery(".rcMain table").attr("tabindex", "0");

}