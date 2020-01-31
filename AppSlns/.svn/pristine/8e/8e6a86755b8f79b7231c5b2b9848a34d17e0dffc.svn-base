function ManageSelection(currentSelection) {
    var divPackages = $jQuery(".divBkgPackages");

    $jQuery('.divBkgPackages input:radio').each(function (index) {

        if ($jQuery(this).attr("id") != $jQuery(currentSelection).attr("id")) {
            $jQuery(this).attr('checked', false);
        }
    });
}


function ManageCtrlSelection(currentSelection, divClass, rbtnIdLike) {
    var divPackages = $jQuery("." + divClass);
    $jQuery('.' + divClass + ' input[id*=' + rbtnIdLike + ']').each(function (index) {
        if ($jQuery(this).attr("id") != $jQuery(currentSelection).attr("id")) {
            $jQuery(this).attr('checked', false);
        }
    });
}