// To close the popup.
function ClosePopup() {
    top.$window.get_radManager().getActiveWindow().close();
}

function pageLoad() {
    $jQuery("[id$=txtNotes]").focus();
    $jQuery(".rbPrimaryIcon.rbCancel").removeClass().addClass("fa fa-ban");
    $jQuery(".rbPrimaryIcon.rbSave").removeClass().addClass("fa fa-floppy-o");

    //For accessibility, we need to prevent focus to go outside after tabbing on last link
    $jQuery("a,button,:input:not([type=hidden]),[tabindex='0']").last().keydown(function (e) {
        if (e.keyCode == 9) {
            e.preventDefault();
            $jQuery("#lblNotes").focus();
        }
    })
    //For accessibility, we need to prevent focus to go outside after shift tab on firstmost element
    $jQuery(document).on("keydown", "[id$=txtNotes]", function (e) {
        var tabKey = 9;
        if (event.shiftKey && event.keyCode == tabKey) {
            e.preventDefault();
            $jQuery("[id$=<%= fsucFeatureActionList.CancelButton.ClientID %>]").focus();
        }

    })
}