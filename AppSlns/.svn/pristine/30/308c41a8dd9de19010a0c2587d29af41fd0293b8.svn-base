var minDate = new Date("01/01/1980");

function onCatChange(sender) {
    var _id = $jQuery(sender).attr("pkgid");
    var _catDiv = $jQuery("#div_" + _id);
    if (_catDiv != undefined && _catDiv.length > 0) {
        if ($jQuery(sender).text().toLowerCase() == "yes") {
            $jQuery(_catDiv).show();

            $jQuery("#div_" + _id + ' input[type="checkbox"]').each(function () {
                $jQuery(this).prop("checked", true);
            });
        }
        else {
            $jQuery(_catDiv).hide();
        }
    }
}

$page.add_beginRequest(function () {

});

$page.add_endRequest(function () {
    $jQuery('input[id*=rbtnYes]').each(function () {
        var _chk = $jQuery(this).is(":checked");
        if (_chk) {
            var _id = $jQuery(this).parent().closest("div").attr("pkgid");
            $jQuery("#div_" + _id).show();
        }
    });
});


function openPreview() {
    var _windowName = "Invitation Preview";
    var url = $page.url.create("~/ProfileSharing/Pages/InvitationPreview.aspx");
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "850," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: _windowName, onclose: OnClientClose });
}

function OnClientClose(oWnd, args) {
    var arg = args.get_argument();
    if (arg) {
        $jQuery('[id$=hdfStatusId]').val(arg.statusId);

        if (arg.statusId != "-1") {
            var btn = $jQuery('[id$=btnUpdateUI]');
            btn.click();
        }
    }
}

function disableValidators(sender, eventArgs) {
    var item = eventArgs.get_item();
    if (item.get_text() != "--Select--") {
        $jQuery("[id$=divAgencyTextMode]").css("display", "none");
        DisableValidator($jQuery("[id$=rfvAgency]")[0].id);
        DisableValidator($jQuery("[id$=rfvEmail]")[0].id);
        DisableValidator($jQuery("[id$=rfvPhone]")[0].id);
        DisableValidator($jQuery("[id$=rfvName]")[0].id);
        DisableValidator($jQuery("[id$=cvEmailAddress]")[0].id);
        DisableValidator($jQuery("[id$=revPhone]")[0].id);
    }
    else {
        $jQuery("[id$=divAgencyTextMode]").css("display", "block");
        EnableValidator($jQuery("[id$=rfvAgency]")[0].id);
        EnableValidator($jQuery("[id$=rfvEmail]")[0].id);
        EnableValidator($jQuery("[id$=rfvPhone]")[0].id);
        EnableValidator($jQuery("[id$=rfvName]")[0].id);
        EnableValidator($jQuery("[id$=cvEmailAddress]")[0].id);
        EnableValidator($jQuery("[id$=revPhone]")[0].id);
    }
}


// Code:: Validator Enabled::
function EnableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], true);
        $jQuery('#' + id).hide();
    }
}

//Code:: Validator Disabled ::
function DisableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], false);
    }
}


$page.showAlertMessageWithTitle = function (msg, msgtype, overriderErrorPanel) {
    msg = $jQuery("[id$=hdnErrorMessage]")[0].value;
    if (typeof (msg) == "undefined") return;
    var c = typeof (msgtype) != "undefined" ? msgtype : "";
    if (overriderErrorPanel) {
        $jQuery("#pageMsgBoxSchuduleInv").children("span")[0].innerHTML = msg;
        $jQuery("#pageMsgBoxSchuduleInv").children("span").attr("class", msgtype);
        if (c == 'sucs') {
            c = "Success";
        }
        else if (c == "info") {
            c = c.toUpperCase();
        }
        else (c = "Validation Message for Tracking Package:");

        $jQuery("[id$=pnlErrorSchuduleInv]").hide();

        $window.showDialog($jQuery("#pageMsgBoxSchuduleInv").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 500, c);
    }
    else {
        $jQuery("#pageMsgBoxSchuduleInv").fadeIn().children("span")[0].innerHTML = msg;
        $jQuery("#pageMsgBoxSchuduleInv").fadeIn().children("span").attr("class", msgtype);

    }
}

function SetMinDate(picker) {
    picker.set_minDate(minDate);
}

function CorrectStartToEndDateOnAdd(picker) {
    var date1 = $jQuery("[id$=dpStartDate]")[0].control.get_selectedDate();
    var date2 = $jQuery("[id$=dpEndDate]")[0].control.get_selectedDate();
    if (date1 != null && date2 != null) {
        if (date1 > date2)
            $jQuery("[id$=dpEndDate]")[0].control.set_selectedDate(null);
    }
}

function SetMinEndDateOnAdd(picker) {
    var date = $jQuery("[id$=dpStartDate]")[0].control.get_selectedDate();
    if (date != null) {
        picker.set_minDate(date);
    }
    else {
        picker.set_minDate(minDate);
    }
}

//UAT-2447
function MaskedUnmaskedInviteePhone1(sender) {

    if (sender == undefined)
        return;
    if (sender.checked) {
        $jQuery("[id$=txtPhone]").hide();
        $jQuery("[id$=txtIntInviteePhone]").show();
        ValidatorEnable($jQuery('[id$=rfvPhone]')[0], false);
        ValidatorEnable($jQuery('[id$=rfvUnmaskedPhone]')[0], true);
        $jQuery("[id$=rfvUnmaskedPhone]").hide();
        ValidatorEnable($jQuery('[id$=revPhone]')[0], false);
        ValidatorEnable($jQuery('[id$=revTxtMobilePrmyNonMasking]')[0], true);
        $jQuery("[id$=revTxtMobilePrmyNonMasking]").hide();
    }
    else {
        $jQuery("[id$=txtIntInviteePhone]").hide();
        $jQuery("[id$=txtPhone]").show();
        ValidatorEnable($jQuery('[id$=rfvPhone]')[0], true);
        ValidatorEnable($jQuery('[id$=revPhone]')[0], true);
        $jQuery("[id$=rfvPhone]").hide();
        $jQuery("[id$=revPhone]").hide();
        ValidatorEnable($jQuery('[id$=rfvUnmaskedPhone]')[0], false);
        ValidatorEnable($jQuery('[id$=revTxtMobilePrmyNonMasking]')[0], false);
    }
}
//UAT-2447
$page.add_pageLoad(function () {
    // debugger;
    MaskedUnmaskedInviteePhone1($jQuery("[id$=chkInternationalPhone]")[0]);

});
