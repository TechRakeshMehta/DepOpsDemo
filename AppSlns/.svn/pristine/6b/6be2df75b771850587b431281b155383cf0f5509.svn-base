
function OnClientCommandExecuting(editor, args) {

    var name = args.get_name();
    var val = args.get_value();

    if (name == "ddPlaceHolders") {
        editor.pasteHtml(val);
        //Cancel the further execution of the command as such a command does not exist in the editor command list
        args.set_cancel(true);
    }
}

// Function added to handle the issue related to distortion of the Template management in Add/Edit mode
function OnClientLoad(editor, args) {
    $jQuery('ul.reToolbar').width('auto');
    //This function added to set focus on text box's in TemplatesMaintenanceFormEventSpecific.ascx to resolved the issue of User is not able to edit the Template Content 
    //Subject/Template Name until or unless make any changes in either Days and frequency fields
    setTimeout(function () {
        $jQuery("input[id$='txtNoOfDays']").focus();
        $jQuery("input[id$='txtTemplateName']").focus();
        $jQuery("input[id$='txtTemplateName']").val($jQuery("input[id$='txtTemplateName']").val());
        // $jQuery("input[id$='txtTemplateName']").blur();
        //$jQuery("input[id$='txtNoOfDays']").blur();
    });
}