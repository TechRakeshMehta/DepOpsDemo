
$jQuery(document).ready(function () {
    if ($jQuery(".client-logo") != undefined) {
        $jQuery(".client-logo").css('display', 'none');
    }
})

function ValidateForm(sender, args) {
    if ($jQuery("[id$=chkAccept]") != undefined && $jQuery("[id$=chkAccept]").length > 0) {
        if (!$jQuery("[id$=chkAccept]")[0].checked) {
            $jQuery("[id$=dvError]")[0].style.display = "block"
            args.set_cancel(true);
        }
        else {
            $jQuery("[id$=dvError]")[0].style.display = "none"
        }
    }
    if ($jQuery("[id$=hiddenOutput]") != undefined) {
        var signatureJson = $jQuery("[id$=hiddenOutput]").val();
        if (signatureJson == "") {
            $jQuery("[id$=dvSignError]")[0].style.display = "block"
            args.set_cancel(true);
        }
        else {
            $jQuery("[id$=dvSignError]")[0].style.display = "none"
        }
    }
}

//Signature Work
function pageLoad() {
    //if (!isCanvasSupported()) {
    //    //$jQuery("[id$=lblSigUnSupport]")[0].innerHTML = "Signature field is not compatible with your browser. Please use different browser.";
    //}
    //else {
    var sig = $jQuery("[id$=hiddenOutput]").val();
    if (sig != undefined && sig != "") {
        $jQuery(document).ready(function () {
            $jQuery('.sigPad').signaturePad({ penWidth: 2, drawOnly: true, lineWidth: 0, validateFields: false }).regenerate(sig);
        });
    }
    else {
        var options = { penWidth: 2, drawOnly: true, lineWidth: 0, validateFields: false }
        $jQuery('.sigPad').signaturePad(options);
    }
    //}
}


function CheckIfAllDocsAcceptedAndSigned(sender) {
    //debugger;
    sender.set_autoPostBack(false);
    var AcceptanceValidationClear = false;
    var lstCheckBox = $jQuery(".dandrDisclosureCheckbox");

    if (lstCheckBox != undefined && lstCheckBox.length > 0) {
        for (i = 0; i < lstCheckBox.length ; i++) {
            if ($jQuery(lstCheckBox[i]).children(":first").prop('checked') != true) {
                var lblValidationMsg = $jQuery(".lblValidationMsg");
                $jQuery(".lblValidationMsg").text("Please agree and accept disclosure form(s)")
                return;
            }
        }
        AcceptanceValidationClear = true;
    }

    var signatureValidationClear = false;
    var sig = $jQuery("[id$=hiddenOutput]").val();
    if (sig != undefined && sig != "") { signatureValidationClear = true; }
    else {
        var lblValidationMsg = $jQuery(".lblValidationMsg");
        lblValidationMsg.text("Please sign the disclosure form(s)");
        return;
    }
    if ((signatureValidationClear) && (AcceptanceValidationClear)) {
        var lblValidationMsg = $jQuery(".lblValidationMsg");
        lblValidationMsg.text("");
        sender.set_autoPostBack(true);
    }
}