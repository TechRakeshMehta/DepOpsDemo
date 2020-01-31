
$jQuery(document).ready(function () {
    //alert("ss");
});

function pageLoad() {

    //Service Form Type
    //Form Version-0
    //New Form-1   
    if ($jQuery("[id$=rbtnFormVer]input:radio").length > 0 && $jQuery("[id$=rbtnNewForm]input:radio").length > 0) {
        var rbtnFormVer = $jQuery("[id$=rbtnFormVer]input:radio").is(":checked");
        var rbtnNewForm = $jQuery("[id$=rbtnNewForm]input:radio").is(":checked");
        var slectVal = 1;
        if (rbtnFormVer) {
            slectVal = 0;
        }
        ManageFormType(slectVal);
    }

    var checkedBtn = $jQuery(".dvDispatchType input:radio:checked").val();
    if (checkedBtn != undefined && checkedBtn != null) {
        ManageServiceFormDispatchType(checkedBtn);
    }
}

function ManageServiceFormDispatchType(checkedBtn) {
    //1: Automatic
    //0: Manual
    if (checkedBtn == "1" || checkedBtn == 1) {
        EnableTemplateValidator();
        $jQuery(".dvSrvcFormTemplate").show();
        $jQuery(".dvSrvcFormReminderTemplate").show();
        $jQuery("[id$=dvUploadSvcForm]").show()
    }
    else {
        DisableTemplateValidator();
        $jQuery(".dvSrvcFormTemplate").hide();
        $jQuery(".dvSrvcFormReminderTemplate").hide();
        $jQuery("[id$=dvUploadSvcForm]").hide();
        //DisableValidator($jQuery("[id$=rfvParentForm]")[0].id);
    }
}

function ManageServiceFormDispatchMode(sender) {
    //var sndrID = $jQuery(sender)[0].id;
    var checkedBtn = $jQuery(".dvDispatchType input:radio:checked").val();

    ManageServiceFormDispatchType(checkedBtn);
}

function OnClickFormType(sender) {
    $jQuery(".dvDispatchType input:radio").removeAttr("checked");
    var selectedValue = $jQuery(sender).val();
    ManageFormType(selectedValue);
}

//Service Form Type
//Form Version- selectedValue: 0
//New Form-selectedValue: 1   
function ManageFormType(selectedValue) {
    if (selectedValue == "0") {
        DisableTemplateValidator();
        DisableValidator($jQuery("[id$=rfvDispatchType]")[0].id);
        $jQuery(".dvDispatchType").hide();

        EnableValidator($jQuery("[id$=rfvParentForm]")[0].id);
        $jQuery(".dvParentForm").show();

        $jQuery("[id$=dvUploadSvcForm]").show();
        $jQuery(".dvSrvcFormTemplate").hide();
        $jQuery(".dvSrvcFormReminderTemplate").hide();
    }
    else {
        EnableValidator($jQuery("[id$=rfvDispatchType]")[0].id);
        DisableValidator($jQuery("[id$=rfvParentForm]")[0].id);
        $jQuery(".dvParentForm").hide();
        $jQuery(".dvDispatchType").show();
    }
}


// Code:: Validator Enabled::
function EnableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], true);
        $jQuery('#' + id).hide();
    }
}

function DisableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], false);
    }
}


function OnClientCommandExecuting(editor, args) {

    var name = args.get_name();
    var val = args.get_value();

    if (name == "ddPlaceHolders") {
        editor.pasteHtml(val);
        //Cancel the further execution of the command as such a command does not exist in the editor command list
        args.set_cancel(true);
    }
}

function DisableTemplateValidator() {
    if ($jQuery("[id$=rfvTemplateName]").length > 0) {
        DisableValidator($jQuery("[id$=rfvTemplateName]")[0].id);
    }
    if ($jQuery("[id$=rfvSubject]").length > 0) {
        DisableValidator($jQuery("[id$=rfvSubject]")[0].id);
    }
    if ($jQuery("[id$=rgvTemplateName]").length > 0) {
        DisableValidator($jQuery("[id$=rgvTemplateName]")[0].id);
    }
    if ($jQuery("[id$=rfvContent]").length > 0) {
        DisableValidator($jQuery("[id$=rfvContent]")[0].id);
    }

    if ($jQuery("[id$=rfvReminderTemplateName]").length > 0) {
        DisableValidator($jQuery("[id$=rfvReminderTemplateName]")[0].id);
    }
    if ($jQuery("[id$=rfvReminderSubject]").length > 0) {
        DisableValidator($jQuery("[id$=rfvReminderSubject]")[0].id);
    }
    if ($jQuery("[id$=rgvReminderTemplateName]").length > 0) {
        DisableValidator($jQuery("[id$=rgvReminderTemplateName]")[0].id);
    }
    if ($jQuery("[id$=rfvReminderContent]").length > 0) {
        DisableValidator($jQuery("[id$=rfvReminderContent]")[0].id);
    }
}

function EnableTemplateValidator() {
    if ($jQuery("[id$=rfvTemplateName]").length > 0) {
        EnableValidator($jQuery("[id$=rfvTemplateName]")[0].id);
    }
    if ($jQuery("[id$=rfvSubject]").length > 0) {
        EnableValidator($jQuery("[id$=rfvSubject]")[0].id);
    }
    if ($jQuery("[id$=rgvTemplateName]").length > 0) {
        EnableValidator($jQuery("[id$=rgvTemplateName]")[0].id);
    }
    if ($jQuery("[id$=rfvContent]").length > 0) {
        EnableValidator($jQuery("[id$=rfvContent]")[0].id);
    }

    if ($jQuery("[id$=rfvReminderTemplateName]").length > 0) {
        EnableValidator($jQuery("[id$=rfvReminderTemplateName]")[0].id);
    }
    if ($jQuery("[id$=rfvReminderSubject]").length > 0) {
        EnableValidator($jQuery("[id$=rfvReminderSubject]")[0].id);
    }
    if ($jQuery("[id$=rgvReminderTemplateName]").length > 0) {
        EnableValidator($jQuery("[id$=rgvReminderTemplateName]")[0].id);
    }
    if ($jQuery("[id$=rfvReminderContent]").length > 0) {
        EnableValidator($jQuery("[id$=rfvReminderContent]")[0].id);
    }
}

function OnCustomValidatorParentForm(sender, args) {
    if (args.Value == "--SELECT--") {
        args.IsValid = false;
    }
    else {
        args.IsValid = true;
    }
}