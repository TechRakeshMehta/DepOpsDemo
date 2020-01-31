
$page.add_pageLoaded(function () {
    //$jQuery(".sec_cmds .ihelp").each(function () {
    //    $jQuery(this).click(function (e) {
    //        $jQuery("[id$=hdnIsExplanatoryNoteClosed]")[0].value = $jQuery(this).parent(".sec_cmds").parent(".mhdr").parents(".section").first().find(".tab-block").is(':visible');
    //        $jQuery(this).toggleClass("help_on").parents(".section").first().find(".tab-block").slideToggle();
    //        e.stopPropagation();
    //        SaveUpdateExplanationState(this);
    //    });
    //});


    //code to handle col/exp of appl details
    //(function ($) {
    //    $("#cmd_profile_info").click(function () {
    //        $el = $("#profile_info");
    //        if ($el.length < 1) { return; }
    //        if ($el.is(":visible")) {
    //            $el.stop().slideUp(function () {
    //                $("#cmd_profile_info").text("Show more details").removeClass("expdd").addClass("colsd");
    //            });
    //        }
    //        else {
    //            $("#cmd_profile_info").text("Show less details").removeClass("colsd").addClass("expdd");
    //            $el.stop().slideDown();
    //        }
    //    });
    //})($jQuery);

});


function validateItemRejectionReason(sender, eventargs) {
    var _notAppCode = $jQuery("[id$=hdfNotApproved]").val();
    var _incCode = $jQuery("[id$=hdfIncomplete]").val();
    var cnt = 0;

    $jQuery("[id$=rbtnListStatus]").each(function () {
        var selectedValue = $jQuery(":checked", this).val();

        if (selectedValue != undefined && selectedValue == _notAppCode) {
            var currentActionButtonItemId = $jQuery(this).attr("actionItemId");
            $jQuery("[id$=txtRejectionreason]").each(function () {
                var currentNotesItemId = $jQuery(this).attr("noteItemId");
                if (currentNotesItemId == currentActionButtonItemId) {
                    if ($jQuery(this).val().trim().length == 0) {
                        cnt += 1;
                    }
                }
            })
        }

    });

    var checkedItemsCount = 0;
    var confirmationMessage = 'Are you sure you want to delete ';

    $jQuery("[id$=chkDeleteItem]").each(function () {
        if ($jQuery(this).is(':checked') == true) {
            checkedItemsCount += 1;
        }
    });

    if (checkedItemsCount > 0) {
        confirmationMessage = confirmationMessage + checkedItemsCount + ' item(s)?';
    }

    if (cnt > 0) {
        sender.set_autoPostBack(false);
        $alert("Please enter the rejection reason for the item.");
    }
    else if (checkedItemsCount > 0) {
        var isOKClicked = confirm(confirmationMessage);

        if (isOKClicked)
            sender.set_autoPostBack(true);
        else
            sender.set_autoPostBack(false);
    }
    else {
        sender.set_autoPostBack(true);
    }
}

$jQuery(document).ready(function () {
    EnableDisableAllValidations();

    $jQuery("[id$=chkDeleteItem]").on('click', function () {
        var validators = $jQuery($jQuery(this).parent().parent()[0]).find('.vldx span');
        var validatorEnabled = true;

        if ($jQuery(this).is(':checked') == true) {
            validatorEnabled = false;
        }

        for (var i = 0; i < validators.length; i++) {
           // debugger;
            ValidatorEnable(validators[i], validatorEnabled);
        }
    });
});

function EnableDisableValidation(rfvCtrlId, enabledValidation) {
    $jQuery("[id*=" + rfvCtrlId + "]").each(function () {
        ValidatorEnable($jQuery(this)[0], enabledValidation);
    })
}

function GetValidationCtrlId(currentRbtnList) {
    var _catId = $jQuery(currentRbtnList).attr("catId");
    var _itmId = $jQuery(currentRbtnList).attr("itmId");
    return "rfv_" + _catId + "_" + _itmId + "_";
}

function ViewDocument(tenantId, applicantDocId) {
    winopen = true;
    var _docType = $jQuery("[id$=hdfDocType]").val();
    //var url = $page.url.create("~/ComplianceOperations/UserControl/DocumentViewer.aspx?DocumentType=" + _docType + "&tenantId=" + tenantId + "&documentId=" + applicantDocId);
    var url = $page.url.create("~/ComplianceOperations/Pages/FormViewer.aspx?DocumentType=" + _docType + "&tenantId=" + tenantId + "&documentId=" + applicantDocId + "&IsApplicantDocument=true");
    var popupWindowName = "doc";
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "1000," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName });
    return false;
}

function EnableDisableAllValidations() {
    var _incCode = $jQuery("[id$=hdfIncomplete]").val();

    $jQuery("[id$=rbtnListStatus]").each(function () {
        var selectedValue = $jQuery(":checked", this).val();
        if (selectedValue != undefined && selectedValue == _incCode) {
            var _rfvCtrlId = GetValidationCtrlId(this);
            EnableDisableValidation(_rfvCtrlId, false);
        }
    });

    $jQuery('[id$=rbtnListStatus]').change(function () {
        var _rfvCtrlId = GetValidationCtrlId(this);
        var selectedValue = $jQuery(":checked", this).val();

        if (selectedValue != undefined) {
            if (selectedValue == _incCode) {
                EnableDisableValidation(_rfvCtrlId, false);
            }
            else {
                $jQuery("[id*=" + _rfvCtrlId + "]").each(function () {
                    $jQuery(this)[0].enabled = true;
                })
            }
        }
    });

}
